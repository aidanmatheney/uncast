namespace Uncast.Data
{
    using System;

    using Uncast.Utils;

    public sealed class DbTempTableColumn<TRow>
    {
        public DbTempTableColumn(string name, string dbType, Type type, Func<TRow, object?> getValue)
        {
            ThrowIf.NullOrWhiteSpace(name, nameof(name));
            ThrowIf.NullOrWhiteSpace(dbType, nameof(dbType));
            ThrowIf.Null(type, nameof(type));
            ThrowIf.Null(getValue, nameof(getValue));

            Name = name;
            DbType = dbType;
            Type = type;
            GetValue = getValue;
        }

        public static DbTempTableColumn<TRow> From<TValue>(string name, string dbType, Func<TRow, TValue> getValue)
        {
            ThrowIf.NullOrWhiteSpace(name, nameof(name));
            ThrowIf.NullOrWhiteSpace(dbType, nameof(dbType));
            ThrowIf.Null(getValue, nameof(getValue));

            var type = typeof(TValue);
            object? GetBoxedValue(TRow obj) => getValue(obj);

            return new DbTempTableColumn<TRow>(name, dbType, type, GetBoxedValue);
        }

        public string Name { get; }
        public string DbType { get; }
        public Type Type { get; }
        public Func<TRow, object?> GetValue { get; }
    }
}