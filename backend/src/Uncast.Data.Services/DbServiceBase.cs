namespace Uncast.Data.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Dapper;

    using Microsoft.Extensions.Logging;

    using MySql.Data.MySqlClient;

    using Uncast.Data;
    using Uncast.Utils;

    public abstract class DbServiceBase
    {
        protected DbServiceBase(MySqlConnection dbConnection, ILogger logger)
        {
            ThrowIf.Null(dbConnection, nameof(dbConnection));
            ThrowIf.Null(logger, nameof(logger));

            DbConnection = dbConnection;
            Logger = logger;
        }

        protected MySqlConnection DbConnection { get; }
        protected ILogger Logger { get; }

        #region Dapper wrappers

        protected Task ExecuteAsync(string commandText, CancellationToken cancellationToken)
            => ExecuteAsync(commandText, null, null, cancellationToken);
        protected Task ExecuteAsync(string commandText, object? parameters, CancellationToken cancellationToken)
            => ExecuteAsync(commandText, parameters, null, cancellationToken);
        protected Task ExecuteAsync(string commandText, IDbTransaction? transaction, CancellationToken cancellationToken)
            => ExecuteAsync(commandText, null, transaction, cancellationToken);
        protected Task ExecuteAsync(string commandText, object? parameters, IDbTransaction? transaction, CancellationToken cancellationToken)
            => DbConnection.ExecuteAsync(ProcessAndCreateCommand(commandText, parameters, transaction, cancellationToken));

        protected Task<T> ExecuteScalarAsync<T>(string commandText, CancellationToken cancellationToken)
            => ExecuteScalarAsync<T>(commandText, null, null, cancellationToken);
        protected Task<T> ExecuteScalarAsync<T>(string commandText, object? parameters, CancellationToken cancellationToken)
            => ExecuteScalarAsync<T>(commandText, parameters, null, cancellationToken);
        protected Task<T> ExecuteScalarAsync<T>(string commandText, IDbTransaction? transaction, CancellationToken cancellationToken)
            => ExecuteScalarAsync<T>(commandText, null, transaction, cancellationToken);
        protected Task<T> ExecuteScalarAsync<T>(string commandText, object? parameters, IDbTransaction? transaction, CancellationToken cancellationToken)
            => DbConnection.ExecuteScalarAsync<T>(ProcessAndCreateCommand(commandText, parameters, transaction, cancellationToken));

        protected Task<DbDataReader> ExecuteReaderAsync(string commandText, CancellationToken cancellationToken)
            => ExecuteReaderAsync(commandText, null, null, cancellationToken);
        protected Task<DbDataReader> ExecuteReaderAsync(string commandText, object? parameters, CancellationToken cancellationToken)
            => ExecuteReaderAsync(commandText, parameters, null, cancellationToken);
        protected Task<DbDataReader> ExecuteReaderAsync(string commandText, IDbTransaction? transaction, CancellationToken cancellationToken)
            => ExecuteReaderAsync(commandText, null, transaction, cancellationToken);
        protected Task<DbDataReader> ExecuteReaderAsync(string commandText, object? parameters, IDbTransaction? transaction, CancellationToken cancellationToken)
            => DbConnection.ExecuteReaderAsync(ProcessAndCreateCommand(commandText, parameters, transaction, cancellationToken));

        protected Task<IEnumerable<T>> QueryAsync<T>(string commandText, CancellationToken cancellationToken)
            => QueryAsync<T>(commandText, null, null, cancellationToken);
        protected Task<IEnumerable<T>> QueryAsync<T>(string commandText, object? parameters, CancellationToken cancellationToken)
            => QueryAsync<T>(commandText, parameters, null, cancellationToken);
        protected Task<IEnumerable<T>> QueryAsync<T>(string commandText, IDbTransaction? transaction, CancellationToken cancellationToken)
            => QueryAsync<T>(commandText, null, transaction, cancellationToken);
        protected Task<IEnumerable<T>> QueryAsync<T>(string commandText, object? parameters, IDbTransaction? transaction, CancellationToken cancellationToken)
            => DbConnection.QueryAsync<T>(ProcessAndCreateCommand(commandText, parameters, transaction, cancellationToken));

        protected Task<T> QuerySingleAsync<T>(string commandText, CancellationToken cancellationToken)
            => QuerySingleAsync<T>(commandText, null, null, cancellationToken);
        protected Task<T> QuerySingleAsync<T>(string commandText, object? parameters, CancellationToken cancellationToken)
            => QuerySingleAsync<T>(commandText, parameters, null, cancellationToken);
        protected Task<T> QuerySingleAsync<T>(string commandText, IDbTransaction? transaction, CancellationToken cancellationToken)
            => QuerySingleAsync<T>(commandText, null, transaction, cancellationToken);
        protected Task<T> QuerySingleAsync<T>(string commandText, object? parameters, IDbTransaction? transaction, CancellationToken cancellationToken)
            => DbConnection.QuerySingleAsync<T>(ProcessAndCreateCommand(commandText, parameters, transaction, cancellationToken));

        protected Task<T> QuerySingleOrDefaultAsync<T>(string commandText, CancellationToken cancellationToken)
            => QuerySingleOrDefaultAsync<T>(commandText, null, null, cancellationToken);
        protected Task<T> QuerySingleOrDefaultAsync<T>(string commandText, object? parameters, CancellationToken cancellationToken)
            => QuerySingleOrDefaultAsync<T>(commandText, parameters, null, cancellationToken);
        protected Task<T> QuerySingleOrDefaultAsync<T>(string commandText, IDbTransaction? transaction, CancellationToken cancellationToken)
            => QuerySingleOrDefaultAsync<T>(commandText, null, transaction, cancellationToken);
        protected Task<T> QuerySingleOrDefaultAsync<T>(string commandText, object? parameters, IDbTransaction? transaction, CancellationToken cancellationToken)
            => DbConnection.QuerySingleOrDefaultAsync<T>(ProcessAndCreateCommand(commandText, parameters, transaction, cancellationToken));

        protected Task<T> QueryFirstAsync<T>(string commandText, CancellationToken cancellationToken)
            => QueryFirstAsync<T>(commandText, null, null, cancellationToken);
        protected Task<T> QueryFirstAsync<T>(string commandText, object? parameters, CancellationToken cancellationToken)
            => QueryFirstAsync<T>(commandText, parameters, null, cancellationToken);
        protected Task<T> QueryFirstAsync<T>(string commandText, IDbTransaction? transaction, CancellationToken cancellationToken)
            => QueryFirstAsync<T>(commandText, null, transaction, cancellationToken);
        protected Task<T> QueryFirstAsync<T>(string commandText, object? parameters, IDbTransaction? transaction, CancellationToken cancellationToken)
            => DbConnection.QueryFirstAsync<T>(ProcessAndCreateCommand(commandText, parameters, transaction, cancellationToken));

        protected Task<T> QueryFirstOrDefaultAsync<T>(string commandText, CancellationToken cancellationToken)
            => QueryFirstOrDefaultAsync<T>(commandText, null, null, cancellationToken);
        protected Task<T> QueryFirstOrDefaultAsync<T>(string commandText, object? parameters, CancellationToken cancellationToken)
            => QueryFirstOrDefaultAsync<T>(commandText, parameters, null, cancellationToken);
        protected Task<T> QueryFirstOrDefaultAsync<T>(string commandText, IDbTransaction? transaction, CancellationToken cancellationToken)
            => QueryFirstOrDefaultAsync<T>(commandText, null, transaction, cancellationToken);
        protected Task<T> QueryFirstOrDefaultAsync<T>(string commandText, object? parameters, IDbTransaction? transaction, CancellationToken cancellationToken)
            => DbConnection.QueryFirstOrDefaultAsync<T>(ProcessAndCreateCommand(commandText, parameters, transaction, cancellationToken));

        protected Task<SqlMapper.GridReader> QueryMultipleAsync(string commandText, CancellationToken cancellationToken)
            => QueryMultipleAsync(commandText, null, null, cancellationToken);
        protected Task<SqlMapper.GridReader> QueryMultipleAsync(string commandText, object? parameters, CancellationToken cancellationToken)
            => QueryMultipleAsync(commandText, parameters, null, cancellationToken);
        protected Task<SqlMapper.GridReader> QueryMultipleAsync(string commandText, IDbTransaction? transaction, CancellationToken cancellationToken)
            => QueryMultipleAsync(commandText, null, transaction, cancellationToken);
        protected Task<SqlMapper.GridReader> QueryMultipleAsync(string commandText, object? parameters, IDbTransaction? transaction, CancellationToken cancellationToken)
            => DbConnection.QueryMultipleAsync(ProcessAndCreateCommand(commandText, parameters, transaction, cancellationToken));

        protected static CommandDefinition CreateCommand(string commandText, object? parameters, IDbTransaction? transaction, CancellationToken cancellationToken)
        {
            ThrowIf.Null(commandText, nameof(commandText));

            return new CommandDefinition
            (
                commandText: commandText,
                parameters: parameters,
                transaction: transaction,
                cancellationToken: cancellationToken
            );
        }

        private CommandDefinition ProcessAndCreateCommand(string commandText, object? parameters, IDbTransaction? transaction, CancellationToken cancellationToken)
        {
            ThrowIf.Null(commandText, nameof(commandText));

            Logger.LogTrace($"Executing database command with text:{Environment.NewLine}{{commandText}}", commandText);
            return CreateCommand(commandText, parameters, transaction, cancellationToken);
        }

        #endregion

        #region Bulk copy

        /// <summary>
        /// Load rows into a temporary table in the database. The name of the table is automatically generated and can be accessed through <see cref="DbTempTableHandle.Name" />.
        /// </summary>
        protected async Task<DbTempTableHandle> TempTableAsync<TRow>(IEnumerable<TRow> rows, Action<DbTempTableBuilder<TRow>> configureTable, CancellationToken cancellationToken)
        {
            ThrowIf.Null(rows, nameof(rows));
            ThrowIf.Null(configureTable, nameof(configureTable));

            var builder = new DbTempTableBuilder<TRow>();
            configureTable(builder);

            var tableName = $"__TempTable_{Guid.NewGuid():N}";

            await DbConnection.EnsureOpenAsync(cancellationToken).ConfigureAwait(false);
            await using (var transaction = await DbConnection.BeginTransactionAsync(cancellationToken).ConfigureAwait(false))
            {
                var dataTable = builder.ToDataTable(rows);

                try
                {
                    await ExecuteAsync
                    (
                        $"CREATE TEMPORARY TABLE {tableName}({string.Join(",", builder.Columns.Select(col => $"{col.Name} {col.DbType}"))});",
                        transaction,
                        cancellationToken
                    ).ConfigureAwait(false);
                    
                    var bulkCopy = new MySqlBulkCopy(DbConnection, transaction)
                    {
                        DestinationTableName = tableName,
                        NotifyAfter = 100
                    };
                    bulkCopy.RowsCopied += (sender, args) =>
                    {
                        Logger.LogTrace("Loaded {rowsCopied}/{totalRows} rows into {tableName}...", args.RowsCopied, dataTable.Rows.Count, tableName);
                    };
                    await bulkCopy.WriteToServerAsync(dataTable, cancellationToken).ConfigureAwait(false);

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Logger.LogError(ex, "Error loading rows into {tableName}", tableName);
                    throw;
                }

                Logger.LogDebug("Loaded {totalRows} rows into {tableName}({columnNames})",
                    dataTable.Rows.Count,
                    tableName,
                    string.Join(", ", builder.Columns.Select(col => col.Name)));
            }

            return new DbTempTableHandle(tableName, async () =>
            {
                await ExecuteAsync($"DROP TEMPORARY TABLE {tableName};", cancellationToken).ConfigureAwait(false);
                Logger.LogDebug("Dropped {tableName}", tableName);
            });
        }

        #endregion
    }
}