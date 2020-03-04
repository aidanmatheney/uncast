namespace Uncast.Data
{
    using System.Data;
    using System.Threading;
    using System.Threading.Tasks;

    using MySql.Data.MySqlClient;

    using Uncast.Utils;

    public static class DbExtensions
    {
        public static async Task EnsureOpenAsync(this MySqlConnection dbConnection, CancellationToken cancellationToken = default)
        {
            ThrowIf.Null(dbConnection, nameof(dbConnection));

            if (dbConnection.State == ConnectionState.Closed)
                await dbConnection.OpenAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}