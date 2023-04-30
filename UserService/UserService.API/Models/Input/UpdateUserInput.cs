﻿using System.ComponentModel.DataAnnotations;

namespace UserService.API.Models.Input;

public class UpdateUserInput
{
    public string About { get; set; } = string.Empty;

    [EmailAddress]
    public string Email { get; set; }

}