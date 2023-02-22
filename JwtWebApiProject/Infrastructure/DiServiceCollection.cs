using JwtWebApi.Interfaces;
using JwtWebApi.Repositories;
using JwtWebApi.Services;

namespace JwtWebApi.Infrastructure;

public static class DiServiceCollection
{
	public static IServiceCollection AddDiServices(this IServiceCollection services)
	{
		services.AddScoped<IUnitOfWork, UnitOfWork>();
		services.AddScoped<IUserRepository, UserRepository>();
		services.AddScoped<IRoleRepository, RoleRepository>();

		services.AddScoped<IUserService, UserService>();

		return services;
	}
}