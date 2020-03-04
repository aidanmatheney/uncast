namespace Uncast.WebApi
{
    public static class AuthorizationPolicyNames
    {
        private const string Require = "Require";
        private const string Role = "Role";

        public const string RequireCuratorRole = Require + RoleNames.Curator + Role;
        public const string RequireAdministratorRole = Require + RoleNames.Administrator + Role;
    }
}