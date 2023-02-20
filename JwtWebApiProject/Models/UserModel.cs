namespace JwtWebApi.Models;

public class UserModel
{
	public Guid Id { get; set; }
	public string? Username { get; set; }
	public string? EmailAddress { get; set; }
	public byte[]? PasswordHash { get; set; }
	public byte[]? PasswordSalt { get; set; }
	public RoleModel? Role { get; set; }
}