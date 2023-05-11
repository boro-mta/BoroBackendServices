using Boro.Authentication.Models;

namespace UserService.API.Models.Output;

public record UserLoginResults(Guid UserId, bool FirstLogin, TokenDetails TokenDetails);
