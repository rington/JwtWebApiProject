using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using JwtWebApi.Models.User;

namespace JwtWebApi.Interfaces;

public interface ITokenService
{
	ClaimsPrincipal ValidateAccessToken(string accessToken);
	JwtSecurityToken GenerateAccessToken(UserModel user, string jwtKey, string jwtIssuer, string jwtAudience);
	string GenerateRefreshToken();
}
