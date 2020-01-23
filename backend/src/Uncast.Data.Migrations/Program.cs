namespace Uncast.Data.Migrations
{
    using System;
    using System.Linq;

    internal static class Program
    {
        private const string ConnectionStringEnvironmentVariableName = "UNCAST_WEBAPI_CONNECTIONSTRING";

        private static int Main(string[] args)
        {
            var connectionString = args.FirstOrDefault() ?? Environment.GetEnvironmentVariable(ConnectionStringEnvironmentVariableName);
            if (connectionString is null)
                throw new InvalidOperationException($"No connection string was specified as an argument to the program, and the {ConnectionStringEnvironmentVariableName} environment variable is not set");

            var migrator = new Migrator(connectionString);
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