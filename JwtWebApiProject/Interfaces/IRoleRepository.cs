using JwtWebApi.Models.User;

namespace JwtWebApi.Interfaces;

public interface IRoleRepository
{
	Task<RoleModel> GetAsync(string roleName);
}