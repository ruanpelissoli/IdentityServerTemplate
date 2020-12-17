using IdentityModel;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServerTemplate.Core.IdentityServerConfig
{
    public static class IdentityServerConfig
    {
        public static readonly string API_NAME = "esfer_api";

        public static IEnumerable<ApiScope> GetApiScopes() =>
            new List<ApiScope> {
                new ApiScope {
                    Name = API_NAME,
                    DisplayName = "Esfer API"
                }
            };

        public static IEnumerable<IdentityResource> GetIdentityResources() =>
            new List<IdentityResource> {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };

        public static IEnumerable<ApiResource> GetApis() =>
            new List<ApiResource> {
                new ApiResource(API_NAME, "Esfer API")
                {
                    Scopes = new List<string> { API_NAME },
                    Enabled = true
                }
            };

        public static IEnumerable<Client> GetClients() =>
            new List<Client> {
                new Client {
                    ClientId = "esfer_web_client",
                    ClientName = "Esfer Web Client",
                    ClientSecrets = { new Secret("esfer_web_client_secret".ToSha256()) },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AllowedScopes = { "openid", API_NAME },
                    AccessTokenLifetime = 43200, // 12h
                    IdentityTokenLifetime = 43200 // 12h
                },
                new Client {
                    ClientId = "esfer_desktop_client",
                    ClientName = "Esfer Desktop Client",
                    ClientSecrets = { new Secret("esfer_desktop_client_secret".ToSha256()) },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AllowedScopes = { "openid", API_NAME },
                    AccessTokenLifetime = 43200, // 12h
                    IdentityTokenLifetime = 43200 // 12h
                },
                new Client {
                    ClientId = "esfer_mobile_client",
                    ClientName = "Esfer Mobile Client",
                    ClientSecrets = { new Secret("esfer_mobile_client_secret".ToSha256()) },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AllowedScopes = { "openid", API_NAME },
                    AccessTokenLifetime = 43200, // 12h
                    IdentityTokenLifetime = 43200 // 12h
                }
            };
    }
}
