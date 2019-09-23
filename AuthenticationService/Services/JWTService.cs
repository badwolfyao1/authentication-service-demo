using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace AuthenticationService.Services
{
    public class JWTService
    {
        private const string _secret = "very_secret_password";
        private const string _issuer = "YourCompany";
        public string CreateToken(string username)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_secret);
                var tokenDescriptor = new SecurityTokenDescriptor()
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, username)
                    }),
                    Issuer = "YourCompany", // Issuer
                    Expires = DateTime.UtcNow.AddMinutes(15), // Expirate after 15 min
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), // Get key from secret
                    SecurityAlgorithms.HmacSha256Signature), // Choose encryption method
                };
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                string token = tokenHandler.WriteToken(securityToken);
                return token;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public bool VerifyToken(string jwtToken)
        {
            try
            {

                var validationParameters = new TokenValidationParameters
                {
                    ClockSkew = TimeSpan.FromMinutes(5),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_secret)),
                    RequireSignedTokens = true,
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ValidateAudience = false, // TODO: Need to research this
                    ValidateIssuer = true,
                    ValidIssuer = _issuer
                };

                var claimsPrincipal = new JwtSecurityTokenHandler()
                    .ValidateToken(jwtToken, validationParameters, out var rawValidatedToken);

                JwtSecurityToken jwtDecodedToken = (JwtSecurityToken)rawValidatedToken;

                return true;
            }
            catch (SecurityTokenValidationException stvex)
            {
                return false;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
