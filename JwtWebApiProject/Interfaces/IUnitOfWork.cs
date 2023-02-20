namespace JwtWebApi.Interfaces;

public interface IUnitOfWork : IDisposable
{
	IUserRepository Users { get; }

	IRoleRepository Roles { get; }

	Task SaveAsync();
}