using JwtWebApi.Models.User;
using Microsoft.EntityFrameworkCore;

namespace JwtWebApi.EFContext;

public class ApplicationContext : DbContext
{

	public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
	{
		
	}

	public DbSet<UserModel> Users { get; set; }

	public DbSet<RoleModel> Roles { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<UserModel>().ToTable("Users");
		modelBuilder.Entity<RoleModel>().ToTable("Roles");
	}
}