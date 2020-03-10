namespace Uncast.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using Uncast.Utils;

    public sealed class DbTempTableBuilder<TRow>
    {
        private readonly List<DbTempTableColumn<TRow>> _columns = new List<DbTempTableColumn<TRow>>();

        public DbTempTableBuilder() { }

        public DbTempTableBuilder(Action<DbTempTableBuilder<TRow>> configure)
        {
            ThrowIf.Null(configure, nameof(configure));
            configure(this);
        }

        public IReadOnlyList<DbTempTableColumn<TRow>> Columns => _columns;

        public void Column<TValue>(string name, string dbType, Func<TRow, TValue> getValue)
        {
            ThrowIf.Null(name, nameof(name));
            ThrowIf.Null(dbType, nameof(dbType));
            ThrowIf.Null(getValue, nameof(getValue));

            var registration = DbTempTableColumn<TRow>.From(name, dbType, getValue);
            _columns.Add(registration);
        }

        public DataTable ToDataTable(IEnumerable<TRow> rows)
        {
            ThrowIf.Null(rows, nameof(rows));

            var dataTable = new DataTable();
            foreach (var column in _columns)
                dataTable.Columns.Add(column.Name, column.Type);

            foreach (var row in rows)
            {
                if (row is null)
                    throw new ArgumentException("The rows collection must not contain a null row", nameof(rows));

                var values = _columns.Select(column => column.GetValue(row)).ToArray();
                dataTable.Rows.Add(values);
            }

            return dataTable;
        }
    }
}