using JwtWebApi.EFContext;
using JwtWebApi.Interfaces;

namespace JwtWebApi.Repositories;

public class UnitOfWork : IUnitOfWork
{
	public IUserRepository UserRepository { get; }
	public IRoleRepository RoleRepository { get; }

	private readonly ApplicationContext _db;
	private bool _disposed;

	public UnitOfWork(ApplicationContext db, IUserRepository userRepository, IRoleRepository roleRepository)
	{
		_db = db;
		UserRepository = userRepository;
		RoleRepository = roleRepository;
	}

	public IUserRepository Users => UserRepository;

	public IRoleRepository Roles => RoleRepository;

	public async Task SaveAsync()
	{
		await _db.SaveChangesAsync();
	}

	// Since we don't work with unmanaged data,
	// implementing Dispose(bool) method isn't necessary. 
	public void Dispose()
	{
		if (_disposed)
		{
			return;
		}

		_db.Dispose();
		_disposed = true;
	}
}
