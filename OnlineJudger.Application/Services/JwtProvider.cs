using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using OnlineJudger.Application.Settings;
using OnlineJudger.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OnlineJudger.Application.Services
{
    public class JwtProvider
    {
        private readonly JwtSettings _jwtSettings;
        public JwtProvider(IOptions<JwtSettings> settings)
        {
            _jwtSettings = settings.Value;
        }
        public string GenerateAccessToken(User user)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            List<Claim> claims = [
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, user.Username),
                ];
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes),
                SigningCredentials = credentials
            };
            var tokenHandler = new JsonWebTokenHandler();
            string accessToken = tokenHandler.CreateToken(tokenDescriptor);
            return accessToken;
        }
    }
}
