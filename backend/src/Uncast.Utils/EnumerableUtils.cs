namespace Uncast.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class EnumerableUtils
    {
        public static IEnumerable<(T Element, int Index)> Index<T>(this IEnumerable<T> source)
        {
            ThrowIf.Null(source, nameof(source));

            int i = 0;
            foreach (var element in source)
            {
                yield return (element, i);
                i += 1;
            }
        }

        public static IEnumerable<T> Repeat<T>(this IReadOnlyCollection<T> source, int count)
        {
            ThrowIf.Null(source, nameof(source));
            if (count < 0)
                throw new ArgumentException("Repeat count must not be negative", nameof(count));

            for (int i = 0; i < count; i += 1)
            {
                foreach (var element in source)
                    yield return element;
            }
        }

        public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> source, int size)
        {
            ThrowIf.Null(source, nameof(source));
            if (size <= 0)
                throw new ArgumentOutOfRangeException(nameof(size), "Size must be positive");

            using var enumerator = source.GetEnumerator();

            var hasCurrent = enumerator.MoveNext();
            if (!hasCurrent)
                yield break;

            IEnumerable<T> GetBatch()
            {
                // ReSharper disable AccessToDisposedClosure
                yield return enumerator.Current;

                for (int i = 1; i < size; i += 1)
                {
                    hasCurrent = enumerator.MoveNext();
                    if (!hasCurrent)
                        yield break;

                    yield return enumerator.Current;
                }

                hasCurrent = enumerator.MoveNext();
                // ReSharper restore AccessToDisposedClosure
            }

            while (hasCurrent)
                yield return GetBatch();
        }

        public static IEnumerable<T> ConcatMany<T>(this IEnumerable<T> first, params IEnumerable<T>[] others)
        {
            ThrowIf.Null(first, nameof(first));
            ThrowIf.Null(others, nameof(others));

            return others.Aggregate(first, (concatenated, current) => concatenated.Concat(current));
        }
    }
}