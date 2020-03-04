namespace Uncast.Utils
{
    using System.Collections.Concurrent;

    public static class ArrayUtils
    {
        public static T[] ArrayFromDequeuing<T>(T firstItem, ConcurrentQueue<T> nextItems)
        {
            ThrowIf.Null(nextItems, nameof(nextItems));

            var arr = new T[1 + nextItems.Count];
            arr[0] = firstItem;

            int i = 1;
            while (nextItems.TryDequeue(out var nextItem))
            {
                arr[i] = nextItem;
                i += 1;
            }

            return arr;
        }
    }
}