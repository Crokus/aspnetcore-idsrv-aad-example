using System.Collections.Generic;
using System.Security.Claims;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace Wolnik.IdSrv
{
    public static class Config
    {
        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "b1ba6e90-92d1-4e66-ba79-f63801532fed",
                    Username = "Krzysztof",
                    Password = "123",
                    Claims = new List<Claim>
                    {
                        new Claim("name", "Krzysztof Ibisz"),
                        new Claim("given_name", "Krzysztof"),
                        new Claim("family_name", "Ibisz")
                    }
                },
                new TestUser
                {
                    SubjectId = "f268b141-3f6f-4a17-adf6-93ea2e27782c",
                    Username = "Jan",
                    Password = "123",
                    Claims = new List<Claim>
                    {
                        new Claim("name", "Jan Kowalski"),
                        new Claim("given_name", "Jan"),
                        new Claim("family_name", "Kowalski")
                    }
                }
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource> { new ApiResource("sensorsapi", "Sensors API") };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientName = "Sensor Dashboard",
                    ClientId = "sensorclient",
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    RedirectUris =
                    {
                        "https://localhost:44370/signin-oidc"
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "sensorsapi"
                    },
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    PostLogoutRedirectUris =
                    {
                        "https://localhost:44370/signout-callback-oidc"
                    },
                    IdentityProviderRestrictions =
                    {
                        "AAD"
                    }
                }
            };
        }
    }
}