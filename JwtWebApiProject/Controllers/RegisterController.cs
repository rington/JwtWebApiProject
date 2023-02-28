using JwtWebApi.ConstsAndEnums;
using Microsoft.AspNetCore.Mvc;
using JwtWebApi.Interfaces;
using JwtWebApi.Models.User;

namespace JwtWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RegisterController : ControllerBase
{
	private readonly IUnitOfWork _uow;
	private readonly IUserService _userService;

	public RegisterController(IUnitOfWork unitOfWork, IUserService userService)
	{
		_uow = unitOfWork;
		_userService = userService;
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

		var newUser = await _userService.InitializeUserAsync(_uow, request, UserRoleNames.User);
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

		var newUserAdmin = await _userService.InitializeUserAsync(_uow, request, UserRoleNames.Administrator);
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

		var newUserManager = await _userService.InitializeUserAsync(_uow, request, UserRoleNames.Manager);
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
}
