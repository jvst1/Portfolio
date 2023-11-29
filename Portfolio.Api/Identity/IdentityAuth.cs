using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using IdentityModel;
using IdentityModel.Client;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Portfolio.Infrastructure.Security;

namespace Portfolio.Api.Identity
{
    public static class IdentityAuth
    {
        public static string AcessTokenJwt(string identityURL)
        {
            IdentityModelEventSource.ShowPII = true;
            return RequestTokenAsync(identityURL).GetAwaiter().GetResult().AccessToken;
        }

        private static async Task<TokenResponse> RequestTokenAsync(string identityURL)
        {
            var client = new HttpClient();

            var disco = await client.GetDiscoveryDocumentAsync(identityURL);
            if (disco.IsError) throw new Exception(disco.Error);

            var clientToken = CreateClientToken("api", disco.TokenEndpoint);

            using var request = new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "api",
                Scope = "api",
                ClientAssertion = { Type = OidcConstants.ClientAssertionTypes.JwtBearer, Value = clientToken },
                ClientCredentialStyle = ClientCredentialStyle.PostBody
            };

            var response = await client.RequestClientCredentialsTokenAsync(request);

            if (response.IsError) throw new Exception(response.Error);
            return response;
        }

        private static string CreateClientToken(string clientId, string audience)
        {
            var pem = File.ReadAllText("Identity\\private.pem");
            var provider = RsaKeys.ImportPrivateKey(pem);
            var now = DateTime.UtcNow;

            var token = new JwtSecurityToken(
                clientId,
                audience,
                new List<Claim>
                {
                    new Claim("jti", Guid.NewGuid().ToString()),
                    new Claim(JwtClaimTypes.Subject, clientId),
                    new Claim(JwtClaimTypes.IssuedAt, now.ToEpochTime().ToString(), ClaimValueTypes.Integer64)
                },
                now,
                now.AddMinutes(1),
                new SigningCredentials(new RsaSecurityKey(provider.ExportParameters(true)),
                    SecurityAlgorithms.RsaSha256
                )
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
    }
}