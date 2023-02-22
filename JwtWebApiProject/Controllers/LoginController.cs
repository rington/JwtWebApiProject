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
	private readonly IUserService _userService;

	public LoginController(IUnitOfWork uow, IUserService userService)
	{
		_uow = uow;
		_userService = userService;
	}

	[HttpGet(UserRoleNames.Administrator)]
	[Authorize(Roles = UserRoleNames.Administrator)]
	public IActionResult LoginAsAdmin()
	{
		var currentUser = _userService.GetUserFromContext();
		if (currentUser == null)
		{
			return BadRequest("User not found!");
		}

		return Ok($"Welcome! You have {currentUser.Role?.Name} rights.");
	}


	[HttpGet($"{UserRoleNames.Administrator}/users")]
	[Authorize(Roles = UserRoleNames.Administrator)]
	public async Task<IEnumerable<UserModel>> GetAllUsersAsync()
	{
		var allUsers = await _uow.Users.GetAllAsync();

		return allUsers;
	}

	[HttpGet(UserRoleNames.Manager)]
	[Authorize(Roles = UserRoleNames.Manager)]
	public IActionResult LoginAsManager()
	{
		var currentUser = _userService.GetUserFromContext();
		if (currentUser == null)
		{
			return BadRequest("User not found!");
		}

		return Ok($"Welcome! You have {currentUser.Role?.Name} rights.");
	}

	[HttpGet(UserRoleNames.User)]
	[Authorize(Roles = UserRoleNames.User)]
	public IActionResult LoginAsUser()
	{
		var currentUser = _userService.GetUserFromContext();
		if (currentUser == null)
		{
			return BadRequest("User not found!");
		}

		return Ok($"Welcome!");
	}
}