using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }

        public static IEnumerable<ApiResource> GetApis()
        {
            return new[]
            {
                new ApiResource("ffmapi", "Implicit Flow for the FFM API")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new[]
            {
                new Client
                {
                    ClientId = "ffmapi_Implicit",
                    ClientName = "Implicit Flow for the FFM API",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RedirectUris =
                    {
                        "http://localhost:5000/oauth2-redirect.html",
                        "http://localhost:5000/o2c.html",
                        "https://localhost:5001/oauth2-redirect.html",
                        "https://localhost:5001/o2c.html",
                        "http://localhost:5000/swagger/oauth2-redirect.html",
                        "http://localhost:5000/swagger/o2c.html",
                        "https://localhost:5001/swagger/oauth2-redirect.html",
                        "https://localhost:5001/swagger/o2c.html"
                    },
                    AllowedScopes = { "ffmapi" }
                }
            };
        }
    }
}