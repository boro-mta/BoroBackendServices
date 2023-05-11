namespace Boro.Authentication.Models;

public record TokenDetails(string Token, DateTime ExpirationTime);
