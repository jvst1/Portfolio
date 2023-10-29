using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Portfolio.Infrastructure.Extensions;
using Portfolio.Infrastructure.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Portfolio.Infrastructure.Security
{
    public class JwtHandler
    {
        private readonly AppSettings _appSettings;

        public JwtHandler(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public WebToken GeraTokenAutenticacao<T>(Guid codigo, string nome, T perfil) where T : Enum
        {
            var roles = GetRoles(perfil);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.SecretToken);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, codigo.ToString()),
                new Claim(ClaimTypes.Name, nome),
            };

            claims.AddRange(roles.Select(s =>
                new Claim(ClaimsIdentity.DefaultRoleClaimType, s)));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_appSettings.ExpiryMinutes),
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = _appSettings.AudienceApi,
                Issuer = _appSettings.Issuer
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var writeToken = tokenHandler.WriteToken(token);

            return new WebToken
            {
                Expires = _appSettings.ExpiryMinutes,
                AccessToken = writeToken
            };
        }

        public string GeraTokenSenha(Guid codigoUsuario, bool primeiroAcesso)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.SecretSenha);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, codigoUsuario.ToString()),
                new Claim(ClaimTypes.Role, primeiroAcesso ? "first" : "reset")
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_appSettings.ExpiryMinutesSenha),
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = _appSettings.AudienceSenha,
                Issuer = _appSettings.Issuer
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var writeToken = tokenHandler.WriteToken(token);

            return writeToken;
        }

        public JwtSecurityToken ValidateTokenSenha(string jwtToken)
        {
            IdentityModelEventSource.ShowPII = true;

            var validationParameters = new TokenValidationParameters
            {
                ValidateLifetime = false, // Validaremos a validade manualmente
                ValidAudience = _appSettings.AudienceSenha,
                ValidIssuer = _appSettings.Issuer,
                IssuerSigningKey =
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.SecretSenha))
            };

            var principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out var validatedToken);

            return validatedToken as JwtSecurityToken;
        }


        public IEnumerable<string> GetRoles<T>(T source) where T : Enum
        {
            var enumValues = EnumExtensions.GetValues<T>();

            var roles = (from value in enumValues where source.HasFlag(value) select value.ToString()).ToList();

            return roles;
        }
    }
    public class WebToken
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public long Expires { get; set; }
    }
}
