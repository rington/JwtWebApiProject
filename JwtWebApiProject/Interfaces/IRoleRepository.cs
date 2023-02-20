using JwtWebApi.Models;

namespace JwtWebApi.Interfaces;

public interface IRoleRepository
{
	Task<RoleModel> GetAsync(string roleName);
}