using JwtWebApi.Models;

namespace JwtWebApi.Interfaces;

public interface IRoleRepository
{
	Task<RoleModel> Get(string roleName);
}