using JwtWebApi.ConstsAndEnums;
using JwtWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using JwtWebApi.Helpers;
using JwtWebApi.Interfaces;

namespace JwtWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RegisterController : ControllerBase
{
	private readonly IUnitOfWork _uow;

	public RegisterController(IUnitOfWork unitOfWork)
	{
		_uow = unitOfWork;
	}

	[HttpPost("User")]
	public async Task<ActionResult<UserModel>> RegisterUserAsync([FromBody] UserAuthModel request)
	{
		if (string.IsNullOrEmpty(request.EmailAddress) ||
		    string.IsNullOrEmpty(request.Username) ||
		    string.IsNullOrEmpty(request.Password))
		{
			return BadRequest("Username, email or password should not be empty!");
		}

		var newUser = await InitializeUserAsync(request, UserRoleNames.User);
		try
		{
            await _uow.Users.CreateAsync(newUser);
            await _uow.SaveAsync();
		}
        catch (Exception e)
		{
            return BadRequest(e.Message);
		}

        return Ok(newUser);
    }

    [HttpPost("Admin")]
    public async Task<ActionResult<UserModel>> RegisterAdminAsync([FromBody] UserAuthModel request)
    {
	    if (string.IsNullOrEmpty(request.EmailAddress) ||
	        string.IsNullOrEmpty(request.Username) ||
	        string.IsNullOrEmpty(request.Password))
	    {
		    return BadRequest("Username, email or password should not be empty!");
	    }

		var newUserAdmin = await InitializeUserAsync(request, UserRoleNames.Administrator);
		try
	    {
		    await _uow.Users.CreateAsync(newUserAdmin);
		    await _uow.SaveAsync();
	    }
	    catch (Exception e)
	    {
		    return BadRequest(e.Message);
	    }

	    return Ok(newUserAdmin);
    }

	[HttpPost("Manager")]
	public async Task<ActionResult<UserModel>> RegisterManagerAsync([FromBody] UserAuthModel request)
	{
		if (string.IsNullOrEmpty(request.EmailAddress) ||
	        string.IsNullOrEmpty(request.Username) ||
	        string.IsNullOrEmpty(request.Password))
	    {
		    return BadRequest("Username, email or password should not be empty!");
	    }

		var newUserManager = await InitializeUserAsync(request, UserRoleNames.Manager);
		try
	    {
		    await _uow.Users.CreateAsync(newUserManager);
		    await _uow.SaveAsync();
	    }
	    catch (Exception e)
	    {
		    return BadRequest(e.Message);
	    }

	    return Ok(newUserManager);
    }

    private async Task<UserModel> InitializeUserAsync(UserAuthModel request, string role) 
    {
	    AuthHelper.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

		var userRole = await _uow.Roles.GetAsync(role);

	    var newUser = new UserModel()
	    {
		    Username = request.Username?.ToLower(),
		    EmailAddress = request.EmailAddress?.ToLower(),
		    PasswordHash = passwordHash,
		    PasswordSalt = passwordSalt,
		    Role = userRole,
	    };

	    return newUser;
    }
}
