using Boro.Authentication.Models;

namespace UserService.API.Models.Output;

public record class UserLoginResults(Guid UserId, bool FirstLogin, TokenDetails TokenDetails);
