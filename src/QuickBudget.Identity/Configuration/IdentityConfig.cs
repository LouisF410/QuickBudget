using IdentityServer4;
using IdentityServer4.Models;

namespace QuickBudget.Identity.Configuration
{
    public class IdentityConfig
    {
        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                new ApiScope("api", "QuickBudget API")
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource> { new ApiResource("api", "QuickBudget API") };
        }

        // Identity resources are data like user ID, name, or email address of a user
        // see: http://docs.identityserver.io/en/release/configuration/resources.html
        public static IEnumerable<IdentityResource> GetResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

        public static IEnumerable<Client> GetClients(Dictionary<string, string> clientsUrl)
        {
            return new List<Client>
            {
                // JavaScript Client
                new Client
                {
                    ClientId = "js",
                    ClientName = "Web OpenId Client",
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    AllowAccessTokensViaBrowser = true,
                    RedirectUris =           { $"{clientsUrl["Web"]}/" },
                    RequireConsent = false,
                    RequireClientSecret = false,
                    RequirePkce = false,
                    AllowOfflineAccess = true,
                    PostLogoutRedirectUris = { $"{clientsUrl["Web"]}/" },
                    AllowedCorsOrigins =     { $"{clientsUrl["Web"]}" },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "api"
                    },
                },
                new Client
                {
                    ClientId = "apiswaggerui",
                    ClientName = "QuickBudget Swagger UI",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RedirectUris = { $"{clientsUrl["Api"]}/swagger/oauth2-redirect.html" },
                    PostLogoutRedirectUris = { $"{clientsUrl["Api"]}/swagger/" },
                    AllowedScopes =
                    {
                        "api"
                    }
                }
            };
        }
    }
}
