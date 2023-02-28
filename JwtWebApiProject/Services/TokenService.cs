using JwtWebApi.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using JwtWebApi.Models.User;

namespace JwtWebApi.Services;

public class TokenService : ITokenService
{
	private readonly IConfiguration _cfg;

	public TokenService(IConfiguration configuration)
	{
		_cfg = configuration;
	}

	public JwtSecurityToken GenerateAccessToken(
		UserModel user,
		string jwtKey,
		string jwtIssuer,
		string jwtAudience)
	{
		var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
		var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

		var claims = new[]
		{
				new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
				new Claim(ClaimTypes.Role, user.Role.Name),
			};

		var token = new JwtSecurityToken(jwtIssuer,
			jwtAudience,
			claims,
			expires: DateTime.Now.AddMinutes(30),
			signingCredentials: credentials);

		return token;
	}

	public string GenerateRefreshToken()
	{
		var randomNumber = new byte[64];
		using (var rng = RandomNumberGenerator.Create())
		{
			rng.GetBytes(randomNumber);
			return Convert.ToBase64String(randomNumber);
		}
	}

	public ClaimsPrincipal ValidateAccessToken(string accessToken)
	{
		var tokenValidationParameters = new TokenValidationParameters
		{
			ValidateAudience = false,
			ValidateIssuer = false,
			ValidateIssuerSigningKey = true,
			IssuerSigningKey =
				new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_cfg.GetSection("Jwt:Key").Value)),
			ValidateLifetime = false,
		};

		var tokenHandler = new JwtSecurityTokenHandler();
		var principal = tokenHandler
			.ValidateToken(accessToken, tokenValidationParameters, out SecurityToken securityToken);

		var jwtSecurityToken = securityToken as JwtSecurityToken;
		if (jwtSecurityToken == null ||
			!jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
			throw new SecurityTokenException("Invalid token");

		return principal;
	}
}

