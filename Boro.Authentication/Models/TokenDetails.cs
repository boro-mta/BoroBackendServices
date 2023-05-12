namespace Boro.Authentication.Models;

public record class TokenDetails(string Token, DateTime ExpirationTime);
