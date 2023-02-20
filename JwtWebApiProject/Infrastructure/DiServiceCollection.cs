using JwtWebApi.Interfaces;
using JwtWebApi.Repositories;

namespace JwtWebApi.Infrastructure;

public static class DiServiceCollection
{
	public static IServiceCollection AddDiServices(this IServiceCollection services)
	{
		services.AddScoped<IUnitOfWork, UnitOfWork>();
		services.AddScoped<IUserRepository, UserRepository>();
		services.AddScoped<IRoleRepository, RoleRepository>();

		return services;
	}
}