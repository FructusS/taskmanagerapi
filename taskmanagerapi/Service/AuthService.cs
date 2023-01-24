using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using taskmanagerapi.Configs;
using taskmanagerapi.Models;

namespace taskmanagerapi.Service
{
    public class AuthService
    {
        private readonly JwtConfig _jwtConfig;

        public AuthService(IOptions<JwtConfig> jwtConfig)
        {
            _jwtConfig = jwtConfig.Value;
        }
        public TokenModel GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var tokeOptions = new JwtSecurityToken(
                issuer: _jwtConfig.Issuer,
                audience: _jwtConfig.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtConfig.AccessTokenLifeTime),
                signingCredentials: new SigningCredentials(_jwtConfig.SymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return new TokenModel { AccessToken = tokenString};
        }
    }
}
