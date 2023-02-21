﻿namespace JwtWebApi.Models
{
	public class JwtTokenResponse
	{
		public string? AccessToken { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime Expires { get; set; }
	}
}