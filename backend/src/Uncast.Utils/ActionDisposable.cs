namespace Uncast.Utils
{
    using System;

    public class ActionDisposable : IDisposable
    {
        private readonly Action _action;

        private bool _disposed = false;
        private readonly object _lockObj = new object();

        public ActionDisposable(Action action)
        {
            ThrowIf.Null(action, nameof(action));
            _action = action;
        }

        public static ActionDisposable NoOp() => new ActionDisposable(() => { });

        public void Dispose()
        {
            if (_disposed)
                return;

            lock (_lockObj)
            {
                if (_disposed)
                    return;

                _disposed = true;
            }

            _action();
        }
    }
}