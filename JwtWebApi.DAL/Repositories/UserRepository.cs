using JwtWebApi.EFContext;
using JwtWebApi.Interfaces;
using JwtWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace JwtWebApi.Repositories;

	public class UserRepository : IRepository<User>
{
	private readonly ApplicationContext _db;

	public UserRepository(ApplicationContext db)
	{
		_db = db;
	}

	public void Create(User user)
	{
		if (_db.Users.Find()
			{
				_db.Users.Add(user);
			}
			else
			{
				throw new NullReferenceException();
		}
		
		
	}

	public bool Delete(int id)
	{
		var user = _db.Users.Find(id);
		if (user != null)
		{
			_db.Users.Remove(user);

			return true;
		}

		return false;
	}

	public IEnumerable<User> Find(Func<User, bool> predicate)
	{
		return _db.Users.Where(predicate).ToList();
	}

	public User? Get(int id)
	{

		return _db.Users.Find(id);
	}

	public IEnumerable<User> GetAll()
	{
		return _db.Users;
	}

	public void Update(User	user)
	{
		_db.Entry(user).State = EntityState.Modified;
	}
}

