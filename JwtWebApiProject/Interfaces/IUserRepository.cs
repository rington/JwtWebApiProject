using JwtWebApi.Models;

namespace JwtWebApi.Interfaces;

public interface IUserRepository
{
	Task CreateAsync(UserModel user);
	Task<UserModel> GetAsync(UserAuthModel credentials);
	Task<IEnumerable<UserModel>> GetAllAsync();
	RoleModel GetUserRole(UserModel user);
}