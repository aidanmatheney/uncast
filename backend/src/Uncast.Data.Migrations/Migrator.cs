namespace Uncast.Data.Migrations
{
    using DbUp;
    using DbUp.Builder;
    using DbUp.Engine;

    using Uncast.Utils;

    public sealed class Migrator
    {
        private readonly string _connectionString;

        public Migrator(string connectionString)
        {
            ThrowIf.Null(connectionString, nameof(connectionString));

            _connectionString = connectionString;
        }

        public bool IsUpgradeRequired()
        {
            var upgrader = CreateUpgraderBuilder()
                .LogToNowhere()
                .Build();

            return upgrader.IsUpgradeRequired();
        }

        public DatabaseUpgradeResult Upgrade()
        {
            EnsureDatabase.For.MySqlDatabase(_connectionString);

            var upgrader = CreateUpgraderBuilder()
                .WithTransactionPerScript()
                .LogToConsole()
                .Build();

            var result = upgrader.PerformUpgrade();
            return result;
        }

        private UpgradeEngineBuilder CreateUpgraderBuilder() => DeployChanges.To
            .MySqlDatabase(_connectionString)
            .WithScriptsEmbeddedInAssembly(typeof(Migrator).Assembly);
    }
}