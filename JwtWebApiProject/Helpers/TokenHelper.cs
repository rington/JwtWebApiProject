using JwtWebApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtWebApi.Helpers
{
	public class TokenHelper
	{
        public static JwtSecurityToken GenerateJwtToken(UserModel user, RoleModel role, string jwtKey, string jwtIssuer, string jwtAudience)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, role.Name)
        };

            var token = new JwtSecurityToken(jwtIssuer,
              jwtAudience,
              claims,
              expires: DateTime.Now.AddMinutes(30),
              signingCredentials: credentials);

            return token;
        }
    }
}
