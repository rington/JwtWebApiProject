namespace JwtWebApi.Models
{
	public class JwtTokenResponse
	{
		public string? Token { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime Expires { get; set; }
	}
}