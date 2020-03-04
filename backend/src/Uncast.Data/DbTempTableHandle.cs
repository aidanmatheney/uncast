namespace Uncast.Data
{
    using System;
    using System.Threading.Tasks;

    using Uncast.Utils;

    public sealed class DbTempTableHandle : AsyncActionDisposable
    {
        public DbTempTableHandle(string name, Func<Task> dropTable) : base(dropTable)
        {
            ThrowIf.Null(name, nameof(name));
            Name = name;
        }

        public string Name { get; }
    }
}