namespace Uncast.Utils
{
    using System;
    using System.Diagnostics;

    using JetBrains.Annotations;

    public static class ThrowIf
    {
        [DebuggerStepThrough]
        // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Global
        public static void Null<T>([NoEnumeration] T obj, string paramName) where T : class?
        {
            if (obj is null)
                throw new ArgumentNullException(paramName);
        }

        [DebuggerStepThrough]
        // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Global
        public static void Null<T>([NoEnumeration] T obj, string paramName, string message) where T : class?
        {
            if (obj is null)
                throw new ArgumentNullException(paramName, message);
        }

        [DebuggerStepThrough]
        // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Global
        public static void NullOrWhiteSpace(string? value, string paramName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Parameter must not be null or white space", paramName);
        }

        [DebuggerStepThrough]
        // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Global
        public static void NullOrWhiteSpace(string? value, string paramName, string message)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException(message, paramName);
        }
    }
}