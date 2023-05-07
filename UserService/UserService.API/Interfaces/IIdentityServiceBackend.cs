﻿using UserService.API.Models.Output;

namespace UserService.API.Interfaces;

public interface IIdentityServiceBackend
{
    Task<UserLoginResults> LoginWithFacebookAsync(string accessToken, string facebookId);

    Task<string> RefreshTokenAsync(Guid userId);
}