using JwtWebApi.Models;

namespace JwtWebApi.Interfaces;

public interface IUserRepository
{
	Task CreateAsync(UserModel user);
	Task<UserModel> Get(UserAuthModel credentials);
	Task<IEnumerable<UserModel>> GetAll();
	RoleModel GetUserRole(UserModel user);
}