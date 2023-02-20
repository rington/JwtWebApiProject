using JwtWebApi.Helpers;
using JwtWebApi.Interfaces;
using JwtWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace JwtWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _cfg;
    private readonly IUnitOfWork _uow;

	public AuthController(IConfiguration configuration, IUnitOfWork unitOfWork)
	{
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

		var user = await _uow.Users.Get(credentials);
		var userRole = _uow.Users.GetUserRole(user);

		if (user == null)
		{
			return BadRequest("User not found!");
		}

		if (!AuthHelper.VerifyPasswordHash(credentials.Password, user.PasswordHash, user.PasswordSalt))
		{
			return BadRequest("Wrong password!");
		}

		var token = AuthHelper.GenerateJwtToken(
			user, 
			userRole,
			_cfg.GetSection("Jwt:Key").Value,
			_cfg.GetSection("Jwt:Issuer").Value,
			_cfg.GetSection("Jwt:Audience").Value);

		return Ok(new JwtTokenResponse
		{
			Token = new JwtSecurityTokenHandler().WriteToken(token),
			CreatedAt = DateTime.Now,
			Expires = token.ValidTo
		});
	}
}