using JwtWebApi.Models.User;

namespace JwtWebApi.Interfaces;

public interface IUserRepository
{
	Task CreateAsync(UserModel user);
	Task<UserModel> GetAsync(UserAuthModel credentials);
	Task<UserModel> GetUserByIdAsync(Guid userId);
	Task<IEnumerable<UserModel>> GetAllAsync();
	RoleModel GetUserRole(UserModel user);
	void Update(UserModel user);
}