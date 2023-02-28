using JwtWebApi.Helpers;
using JwtWebApi.Interfaces;
using JwtWebApi.Models;
using JwtWebApi.Models.User;
using System.Security.Claims;

namespace JwtWebApi.Services;

public class UserService : IUserService
{
	private readonly IHttpContextAccessor _contextAccessor;

	public UserService(IHttpContextAccessor contextAccessor)
	{
		_contextAccessor = contextAccessor;
	}

	public async Task<UserModel> InitializeUserAsync(IUnitOfWork uow, UserAuthModel request, string role)
	{
		PasswordHelper.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

		var userRole = await uow.Roles.GetAsync(role);

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
	public UserModel GetUserFromContext()
	{
		if (_contextAccessor.HttpContext?.User.Identity is ClaimsIdentity identity)
		{
			var userClaims = identity.Claims;

			var userRole = new RoleModel
			{
				Name = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value
			};

			return new UserModel
			{
				Id = Guid.Parse(userClaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value),
				EmailAddress = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value,
				Role = userRole
			};
		}

		return null;
	}
}

