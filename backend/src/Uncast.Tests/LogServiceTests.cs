namespace Uncast.Tests
{
    using System;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    using MySql.Data.MySqlClient;

    using Uncast.Data.Services;
    using Uncast.Entities;

    using Xunit;

    public sealed class LogServiceTests
    {
        private const string ConnectionStringEnvironmentVariableName = "UNCAST_WEBAPI_CONNECTIONSTRING";

        private static readonly Random Random = new Random();

        [Fact]
        public async Task TestDbPersistenceAsync()
        {
            var rootServiceProvider = CreateServiceProvider();
            using var serviceScope = rootServiceProvider.CreateScope();
            var serviceProvider = serviceScope.ServiceProvider;

            var logService = serviceProvider.GetRequiredService<ILogService>();

            for (int j = 0; j < 10; j += 1)
            {
                await Task.Delay(Random.Next(0, 25));

                await logService.InsertWebApiEntriesAsync(Enumerable.Repeat(new WebApiLogEntry
                {
                    TimeWritten = DateTimeOffset.Now,
                    ServerName = Environment.MachineName,
                    Category = "",
                    Scope = "",
                    LogLevel = "",
                    EventId = 900,
                    EventName = null,
                    Message = $"Test{j}",
                    Exception = new Exception().StackTrace
                }, Random.Next(9000, 11000)));
            }

            // TODO: Read from DB to verify inserted entries
        }

        private static IServiceProvider CreateServiceProvider()
        {
            var baseConnectionString = Environment.GetEnvironmentVariable(ConnectionStringEnvironmentVariableName);
            if (baseConnectionString is null)
                throw new InvalidOperationException($"{ConnectionStringEnvironmentVariableName} environment variable is not set");

            var connectionString = new MySqlConnectionStringBuilder(baseConnectionString)
            {
                AllowUserVariables = true,
                AllowLoadLocalInfile = true
            };

            var services = new ServiceCollection();
            services.AddScoped<IDbConnection>(serviceProvider => new MySqlConnection(connectionString.ToString()));
            services.AddScoped<ILogService, LogService>();
            services.AddScoped<ILogger<LogService>>(_ => new ToConsoleLogger<LogService>());

            return services.BuildServiceProvider();
        }
    }
}