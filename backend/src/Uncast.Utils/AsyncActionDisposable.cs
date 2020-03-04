namespace Uncast.Utils
{
    using System;
    using System.Threading.Tasks;

    public class AsyncActionDisposable : IAsyncDisposable
    {
        private readonly Func<Task> _action;

        private bool _disposed = false;
        private readonly object _lockObj = new object();

        public AsyncActionDisposable(Func<Task> action)
        {
            ThrowIf.Null(action, nameof(action));
            _action = action;
        }

        public static AsyncActionDisposable NoOp() => new AsyncActionDisposable(() => Task.CompletedTask);

        public async ValueTask DisposeAsync()
        {
            if (_disposed)
                return;

            lock (_lockObj)
            {
                if (_disposed)
                    return;

                _disposed = true;
            }

            await _action().ConfigureAwait(false);
        }
    }
}