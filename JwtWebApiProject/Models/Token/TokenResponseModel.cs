namespace JwtWebApi.Models.Token;
public class TokenResponseModel
{
	public string? AccessToken { get; set; }
	public string? RefreshToken { get; set; }
	public DateTime AccessTokenExpires { get; set; }
}