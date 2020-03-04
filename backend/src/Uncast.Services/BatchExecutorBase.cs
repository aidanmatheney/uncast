﻿namespace Uncast.Services
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.Extensions.DependencyInjection;

    using Uncast.Utils;

    public abstract class BatchExecutorBase<T> : IDisposable
    {
        private const int ConsecutiveLoggingExceptionMaxCount = 3;

        private readonly ConcurrentQueue<T> _queue = new ConcurrentQueue<T>();
        private readonly CancellationTokenSource _disposeCts = new CancellationTokenSource();

        protected BatchExecutorBase(TimeSpan interval, IServiceProvider serviceProvider)
        {
            ThrowIf.Null(serviceProvider, nameof(serviceProvider));

            Interval = interval;
            ServiceProvider = serviceProvider;

            new Thread(PeriodicallyExecute).Start();
        }

        protected TimeSpan Interval { get; }
        protected IServiceProvider ServiceProvider { get; }

        protected void Enqueue(T item) => _queue.Enqueue(item);
        public virtual void Dispose() => _disposeCts.Cancel();

        private async void PeriodicallyExecute()
        {
            while (true)
            {
                bool disposeWasRequested;
                try
                {
                    await Task.Delay(Interval, _disposeCts.Token);
                    disposeWasRequested = false;
                }
                catch (OperationCanceledException)
                {
                    disposeWasRequested = true;
                }

                if (_queue.TryDequeue(out var firstItem))
                {
                    var items = ArrayUtils.ArrayFromDequeuing(firstItem, _queue);

                    using var serviceScope = ServiceProvider.CreateScope();
                    try
                    {
                        await ExecuteAsync(items, serviceScope.ServiceProvider).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        await HandleExceptionWithRetryAsync(ex, items, serviceScope.ServiceProvider).ConfigureAwait(false);
                    }
                }

                if (disposeWasRequested)
                    break;
            }
        }

        private async Task HandleExceptionWithRetryAsync(Exception ex, T[] items, IServiceProvider serviceProvider)
        {
            var firstEx = ex;
            var currentEx = firstEx;
            var loggingExceptionCount = 0;

            while (true)
            {
                try
                {
                    await HandleExceptionAsync(currentEx, items, serviceProvider).ConfigureAwait(false);
                    break;
                }
                catch (Exception nextEx)
                {
                    loggingExceptionCount += 1;
                    // ReSharper disable once RedundantLogicalConditionalExpressionOperand
                    if (ConsecutiveLoggingExceptionMaxCount >= 0 && loggingExceptionCount > ConsecutiveLoggingExceptionMaxCount)
                        throw;

                    currentEx = nextEx;
                }
            }
        }

        protected abstract Task ExecuteAsync(IEnumerable<T> items, IServiceProvider scopedServiceProvider);
        protected virtual Task HandleExceptionAsync(Exception ex, IEnumerable<T> items, IServiceProvider scopedServiceProvider)
        {
            // Do not require implementing this method unless exceptions can occur in the subclass's domain
            Debug.Fail($"{nameof(BatchExecutorBase<T>)}.{nameof(HandleExceptionAsync)} must be implemented");
            throw ex;
        }
    }
}