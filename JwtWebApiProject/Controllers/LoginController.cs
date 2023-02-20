using System.Security.Claims;
using JwtWebApi.ConstsAndEnums;
using JwtWebApi.Interfaces;
using JwtWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
	private readonly IUnitOfWork _uow;

	public LoginController(IUnitOfWork uow)
	{
		_uow = uow;
	}

	[HttpGet(UserRoleNames.Administrator)]
	[Authorize(Roles = UserRoleNames.Administrator)]
	public IActionResult LoginAsAdmin()
	{
		var currentUser = GetUserFromContext();
		if (currentUser == null)
		{
			return BadRequest("User not found!");
		}

		return Ok($"Welcome {currentUser.Username}! You have {currentUser.Role?.Name} rights.");
	}


	[HttpGet($"{UserRoleNames.Administrator}/users")]
	[Authorize(Roles = UserRoleNames.Administrator)]
	public async Task<IEnumerable<UserModel>> GetAllUsersAsync()
	{
		var allUsers = await _uow.Users.GetAll();

		return allUsers;
	}

	[HttpGet(UserRoleNames.Manager)]
	[Authorize(Roles = UserRoleNames.Manager)]
	public IActionResult LoginAsManager()
	{
		var currentUser = GetUserFromContext();
		if (currentUser == null)
		{
			return BadRequest("User not found!");
		}

		return Ok($"Welcome {currentUser.Username}! You have {currentUser.Role?.Name} rights.");
	}

	[HttpGet(UserRoleNames.User)]
	[Authorize(Roles = UserRoleNames.User)]
	public IActionResult LoginAsUser()
	{
		var currentUser = GetUserFromContext();
		if (currentUser == null)
		{
			return BadRequest("User not found!");
		}

		return Ok($"Welcome {currentUser.Username}!");
	}

	private UserModel GetUserFromContext()
	{
		if (HttpContext.User.Identity is ClaimsIdentity identity)
		{
			var userClaims = identity.Claims;

			var userRole = new RoleModel
			{
				Name = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value
			};

			return new UserModel
			{
				Username = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value,
				EmailAddress = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value,
				Role = userRole
			};
		}

		return null;
	}
}