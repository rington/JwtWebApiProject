namespace JwtWebApi.DAL.Entities;

public class User
{
	public Guid Id { get; set; }
	public string Username { get; set; }
	public string EmailAddress { get; set; }
	public byte[] PasswordHash { get; set; }
	public byte[] PasswordSalt { get; set; }
	public Role Role { get; set; }
}