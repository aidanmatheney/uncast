namespace Uncast.Entities
{
    using System;

    /// <summary>Represents a role in the Uncast identity system.</summary>
    public class AppRole
    {
        /// <summary>
        ///     Initializes a new instance of <see cref="AppRole" />.
        /// </summary>
        /// <remarks>
        ///     The Id property is initialized to form a new GUID value.
        /// </remarks>
        public AppRole()
        {
            Id = Guid.NewGuid();
        }

        /// <summary>
        ///     Initializes a new instance of <see cref="AppRole" />.
        /// </summary>
        /// <param name="roleName">The role name.</param>
        /// <remarks>
        ///     The Id property is initialized to form a new GUID string value.
        /// </remarks>
        public AppRole(string roleName) : this()
        {
            Name = roleName;
        }

        /// <summary>Gets or sets the primary key for this role.</summary>
        public Guid Id { get; set; }

        /// <summary>Gets or sets the name for this role.</summary>
        public string? Name { get; set; }

        /// <summary>Gets or sets the normalized name for this role.</summary>
        public string? NormalizedName { get; set; }

        /// <summary>
        ///     A random value that should change whenever a role is persisted to the store
        /// </summary>
        public string? ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

        /// <summary>Returns the name of the role.</summary>
        public override string ToString() => Name ?? $"[{Id}]";
    }
}