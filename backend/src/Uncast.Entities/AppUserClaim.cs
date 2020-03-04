namespace Uncast.Entities
{
    using System;
    using System.Security.Claims;

    using Uncast.Utils;

    public class AppUserClaim
    {
        public AppUserClaim() { }

        public AppUserClaim(string claimType, string claimValue)
        {
            ThrowIf.Null(claimType, nameof(claimType));
            ThrowIf.Null(claimValue, nameof(claimValue));

            ClaimType = claimType;
            ClaimValue = claimValue;
        }

        public AppUserClaim(Claim claim)
        {
            ThrowIf.Null(claim, nameof(claim));

            ClaimType = claim.Type;
            ClaimValue = claim.Value;
        }

        public Guid UserId { get; set; }
        public string? ClaimType { get; set; }
        public string? ClaimValue { get; set; }

        public Claim ToClaim() => new Claim(ClaimType, ClaimValue);
    }
}
