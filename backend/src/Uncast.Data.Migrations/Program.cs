namespace Uncast.Data.Migrations
{
    using MySql.Data.MySqlClient;
    using System;
    using System.Linq;

    internal static class Program
    {
        private const string ConnectionStringEnvironmentVariableName = "UNCAST_MIGRATIONS_CONNECTIONSTRING";

        private static int Main(string[] args)
        {
            var baseConnectionString = args.FirstOrDefault() ?? Environment.GetEnvironmentVariable(ConnectionStringEnvironmentVariableName);
            if (baseConnectionString is null)
                throw new InvalidOperationException($"No connection string was specified as an argument to the program, and the {ConnectionStringEnvironmentVariableName} environment variable is not set");

            var connectionString = new MySqlConnectionStringBuilder(baseConnectionString)
            {
                AllowUserVariables = true
            };

            var migrator = new Migrator(connectionString.ToString());
            var result = migrator.Upgrade();

            if (!result.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Error);
                Console.ResetColor();
#if DEBUG
                Console.ReadLine();
#endif
                return 1;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success!");
            Console.ResetColor();
            return 0;
        }
    }
}