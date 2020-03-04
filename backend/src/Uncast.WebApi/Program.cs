namespace Uncast.WebApi
{
    using System;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    using MySql.Data.MySqlClient;

    using Uncast.Data.Migrations;
    using Uncast.Services;

    internal static class Program
    {
        private const string ConnectionStringEnvironmentVariableName = "UNCAST_WEBAPI_CONNECTIONSTRING";

        private static void Main(string[] args)
        {
            var baseConnectionString = Environment.GetEnvironmentVariable(ConnectionStringEnvironmentVariableName);
            if (baseConnectionString is null)
                throw new InvalidOperationException($"{ConnectionStringEnvironmentVariableName} environment variable is not set");

            var connectionString = new MySqlConnectionStringBuilder(baseConnectionString)
            {
                AllowUserVariables = true
            };

            var migrator = new Migrator(connectionString.ToString());
            if (migrator.IsUpgradeRequired())
                throw new InvalidOperationException("The database requires upgrades. Run Uncast.Data.Migrations to execute the new change scripts.");
            
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var hostBuilder = Host.CreateDefaultBuilder(args);

            hostBuilder.ConfigureWebHostDefaults(webHostBuilder =>
            {
                webHostBuilder.UseStartup<Startup>();
            });

            hostBuilder.ConfigureLogging(loggingBuilder =>
            {
                var services = loggingBuilder.Services;

                services.AddSingleton<ILoggerProvider>(serviceProvider =>
                {
                    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                    var settings = configuration.GetSection("Logging:Database").Get<BatchLoggerSettings>();

                    return new DbLoggerProvider(settings, serviceProvider);
                });
            });

            return hostBuilder;
        }
    }
}