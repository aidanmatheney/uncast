namespace Uncast.Tests
{
    using System;

    using Microsoft.Extensions.Logging;

    using Uncast.Utils;

    internal sealed class ToConsoleLogger<TCategoryName> : ILogger<TCategoryName>
    {
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            Console.WriteLine($"{logLevel} {DateTime.Now}: {formatter(state, exception)}");
        }

        public bool IsEnabled(LogLevel logLevel) => logLevel >= LogLevel.Debug;

        public IDisposable BeginScope<TState>(TState state) => ActionDisposable.NoOp();
    }
}