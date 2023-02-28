using JwtWebApi.Models.User;

namespace JwtWebApi.Interfaces
{
	public interface IUserService
	{
		Task<UserModel> InitializeUserAsync(IUnitOfWork uow, UserAuthModel request, string role);
		UserModel GetUserFromContext();
	}
}
