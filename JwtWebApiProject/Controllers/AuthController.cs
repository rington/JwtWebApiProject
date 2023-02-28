using JwtWebApi.Helpers;
using JwtWebApi.Interfaces;
using JwtWebApi.Models.Token;
using JwtWebApi.Models.User;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace JwtWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
	private readonly ITokenService _tokenService;
	private readonly IUserService _userService;
	private readonly IConfiguration _cfg;
	private readonly IUnitOfWork _uow;

	public AuthController(ITokenService tokenService, IUserService userService, IConfiguration configuration, IUnitOfWork unitOfWork)
	{
		_tokenService = tokenService;
		_userService = userService;
		_cfg = configuration;
		_uow = unitOfWork;
	}

	[HttpPost]
	public async Task<IActionResult> AuthenticateUserAsync([FromBody] UserAuthModel credentials)
	{
		if (string.IsNullOrEmpty(credentials.Username) ||
		    string.IsNullOrEmpty(credentials.EmailAddress) ||
		    string.IsNullOrEmpty(credentials.Password))
		{
			return BadRequest("Username, email or password should not be empty!");
		}

		var user = await _uow.Users.GetAsync(credentials);
		var userRole = _uow.Users.GetUserRole(user);

		if (user == null)
		{
			return BadRequest("User not found!");
		}

		if (!PasswordHelper.VerifyPasswordHash(credentials.Password, user.PasswordHash, user.PasswordSalt))
		{
			return BadRequest("Wrong password!");
		}
		
		var jwtToken = _tokenService.GenerateAccessToken(user, 
			_cfg.GetSection("Jwt:Key").Value,
			_cfg.GetSection("Jwt:Issuer").Value,
			_cfg.GetSection("Jwt:Audience").Value);

		var refreshToken = _tokenService.GenerateRefreshToken();
		user.RefreshToken = refreshToken;
		user.RefreshTokenExpires = DateTime.Now.AddDays(7);

		try
		{
			_uow.Users.Update(user);
			await _uow.SaveAsync();
		}
		catch (Exception e)
		{
			return BadRequest(e.Message);
		}

		return Ok(new TokenResponseModel
		{
			AccessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken),
			RefreshToken = refreshToken,
			AccessTokenExpires = jwtToken.ValidTo,
		});
	}

	[HttpPost("RefreshToken")]
	public async Task<IActionResult> RefreshTokenAsync(TokenRequestModel tokenRequest)
	{
		if (tokenRequest == null)
		{
			return BadRequest("Invalid request!");
		}

		var claimsPrincipal = _tokenService.ValidateAccessToken(tokenRequest.AccessToken);
		var userId = Guid.Parse(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier));

		var user = await _uow.Users.GetUserByIdAsync(userId);
		var userRole = _uow.Users.GetUserRole(user);

		if (user == null || user.RefreshToken != tokenRequest.RefreshToken || user.RefreshTokenExpires <= DateTime.Now)
		{
			return BadRequest("You need to authenticate!");
		}

		var newJwtToken = _tokenService.GenerateAccessToken(user,
			_cfg.GetSection("Jwt:Key").Value,
			_cfg.GetSection("Jwt:Issuer").Value,
			_cfg.GetSection("Jwt:Audience").Value);
		var newRefreshToken = _tokenService.GenerateRefreshToken();

		user.RefreshToken = newRefreshToken;
		try
		{
			_uow.Users.Update(user);
			await _uow.SaveAsync();
		}
		catch (Exception e)
		{
			return BadRequest(e.Message);
		}

		return Ok(new TokenResponseModel
		{
			AccessToken = new JwtSecurityTokenHandler().WriteToken(newJwtToken),
			RefreshToken = user.RefreshToken,
			AccessTokenExpires = newJwtToken.ValidTo,
		});
	}

	[HttpPost("RevokeRefreshToken")]
	public async Task<IActionResult> RevokeRefreshTokenAsync()
	{
		var userFromContext = _userService.GetUserFromContext();
		if (userFromContext == null)
		{
			return BadRequest("User not found!");
		}

		var user = await _uow.Users.GetUserByIdAsync(userFromContext.Id);
		var userRole = _uow.Users.GetUserRole(user);

		if (user == null)
		{
			return BadRequest("User not found!");
		}
		if (user.RefreshToken == null)
		{
			return BadRequest("Token already revoked!");
		}

		user.RefreshTokenExpires = DateTime.Now;
		try
		{
			_uow.Users.Update(user);
			await _uow.SaveAsync();
		}
		catch(Exception e)
		{
			return BadRequest(e.Message);
		}

		return NoContent();
	}
}