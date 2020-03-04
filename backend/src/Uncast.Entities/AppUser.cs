namespace Uncast.Entities
{
    using System;

    using Microsoft.AspNetCore.Identity;

    /// <summary>Represents a user in the Uncast identity system.</summary>
    public class AppUser
    {
        /// <summary>
        /// Initializes a new instance of <see cref="AppUser" />.
        /// </summary>
        /// <remarks>
        /// The Id property is initialized to form a new GUID value.
        /// </remarks>
        public AppUser()
        {
            Id = Guid.NewGuid();
            SecurityStamp = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Initializes a new instance of <see cref="AppUser" />.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <remarks>
        /// The Id property is initialized to form a new GUID value.
        /// </remarks>
        public AppUser(string userName) : this()
        {
            UserName = userName;
        }

        /// <summary>Gets or sets the primary key for this user.</summary>
        [PersonalData]
        public Guid Id { get; set; }

        /// <summary>Gets or sets the user name for this user.</summary>
        [ProtectedPersonalData]
        public string? UserName { get; set; }

        /// <summary>Gets or sets the normalized user name for this user.</summary>
        public string? NormalizedUserName { get; set; }

        /// <summary>Gets or sets the email address for this user.</summary>
        [ProtectedPersonalData]
        public string? Email { get; set; }

        /// <summary>
        /// Gets or sets the normalized email address for this user.
        /// </summary>
        public string? NormalizedEmail { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating if a user has confirmed their email address.
        /// </summary>
        /// <value>True if the email address has been confirmed, otherwise false.</value>
        [PersonalData]
        public bool EmailConfirmed { get; set; }

        /// <summary>
        /// Gets or sets a salted and hashed representation of the password for this user.
        /// </summary>
        public string? PasswordHash { get; set; }

        /// <summary>
        /// Gets whether this user has a password.
        /// </summary>
        public bool HasPassword => !(PasswordHash is null);

        /// <summary>
        /// A random value that must change whenever a users credentials change (password changed, login removed)
        /// </summary>
        public string? SecurityStamp { get; set; }

        /// <summary>
        /// A random value that must change whenever a user is persisted to the store
        /// </summary>
        public string? ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

        /// <summary>Gets or sets a telephone number for the user.</summary>
        [ProtectedPersonalData]
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating if a user has confirmed their telephone address.
        /// </summary>
        /// <value>True if the telephone number has been confirmed, otherwise false.</value>
        [PersonalData]
        public bool PhoneNumberConfirmed { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating if two factor authentication is enabled for this user.
        /// </summary>
        /// <value>True if 2fa is enabled, otherwise false.</value>
        [PersonalData]
        public bool TwoFactorEnabled { get; set; }

        /// <summary>
        /// Gets or sets the date and time, in UTC, when any user lockout ends.
        /// </summary>
        /// <remarks>A value in the past means the user is not locked out.</remarks>
        public DateTimeOffset? LockoutEnd { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating if the user could be locked out.
        /// </summary>
        /// <value>True if the user could be locked out, otherwise false.</value>
        public bool LockoutEnabled { get; set; }

        /// <summary>
        /// Gets or sets the number of failed login attempts for the current user.
        /// </summary>
        public int AccessFailedCount { get; set; }

        /// <summary>
        /// Gets or sets the two-factor authenticator key for this user.
        /// </summary>
        public string? AuthenticatorKey { get; set; }

        public void RemovePassword() => PasswordHash = null;

        /// <summary>Returns the username for this user.</summary>
        public override string ToString() => UserName ?? $"[{Id}]";
    }
}
