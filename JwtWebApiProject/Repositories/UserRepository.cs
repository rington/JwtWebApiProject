using JwtWebApi.EFContext;
using JwtWebApi.Interfaces;
using JwtWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace JwtWebApi.Repositories;

public class UserRepository : IUserRepository
{
	private readonly ApplicationContext _db;

	public UserRepository(ApplicationContext db)
	{
		_db = db;
	}

	public async Task CreateAsync(UserModel user)
	{
		if (await _db.Users.AnyAsync(u => u.Username.ToLower() == user.Username))
		{
			throw new Exception("User with this username is already registered!");
		}

		await _db.Users.AddAsync(user);
	}

	public async Task<UserModel> Get(UserAuthModel credentials)
	{
		var user = await _db.Users
			.FirstOrDefaultAsync(u => u.Username.ToLower() == credentials.Username.ToLower() &&
									  u.EmailAddress == credentials.EmailAddress.ToLower());

		return user;
	}

	public async Task<IEnumerable<UserModel>> GetAll()
	{
		var users = await _db.Users.Include("Role").ToListAsync();

		return users;
	}

	public RoleModel GetUserRole(UserModel user)
	{
		var userRole = _db.Users.Include(u => u.Role).FirstOrDefault(u => u.Id == user.Id)?.Role;

		return userRole;
	}
}