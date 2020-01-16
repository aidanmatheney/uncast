namespace Uncast.Utils
{
    using System;

    public static class ThrowIf
    {
        // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Global
        public static void Null<T>(T obj, string paramName) where T : class
        {
            if (obj is null)
                throw new ArgumentNullException(paramName);
        }
    }
}