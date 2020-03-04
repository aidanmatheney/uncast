namespace Uncast.Entities
{
    using Microsoft.AspNetCore.Identity;

    using Uncast.Utils;

    public class AppUserLogin
    {
        public AppUserLogin() { }

        public AppUserLogin(string loginProvider, string providerKey, string providerDisplayName)
        {
            ThrowIf.Null(loginProvider, nameof(loginProvider));
            ThrowIf.Null(providerKey, nameof(providerKey));
            ThrowIf.Null(providerDisplayName, nameof(providerDisplayName));

            LoginProvider = loginProvider;
            ProviderKey = providerKey;
            ProviderDisplayName = providerDisplayName;
        }

        public AppUserLogin(UserLoginInfo loginInfo)
        {
            ThrowIf.Null(loginInfo, nameof(loginInfo));

            LoginProvider = loginInfo.LoginProvider;
            ProviderKey = loginInfo.ProviderKey;
            ProviderDisplayName = loginInfo.ProviderDisplayName;
        }

        public string? UserId { get; set; }
        public string? LoginProvider { get; set; }
        public string? ProviderKey { get; set; }
        public string? ProviderDisplayName { get; set; }

        public UserLoginInfo ToLoginInfo() => new UserLoginInfo(LoginProvider, ProviderKey, ProviderDisplayName);
    }
}
