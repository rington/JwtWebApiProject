using JwtWebApi.EFContext;
using JwtWebApi.Interfaces;
using JwtWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace JwtWebApi.Repositories;

public class RoleRepository : IRoleRepository
{
	private readonly ApplicationContext _db;

	public RoleRepository(ApplicationContext db)
	{
		_db = db;
	}

	public async Task<RoleModel> Get(string roleName)
	{
		var userRole = await _db.Roles.FirstOrDefaultAsync(r => r.Name == roleName);

		if (userRole == null)
		{
			throw new Exception("Role not found!");
		}

		return userRole;
	}
}
