﻿using System.ComponentModel.DataAnnotations;

namespace UserService.API.Models.Input;

public class UpdateUserInput
{
    public string About { get; set; }

    [EmailAddress]
    public string Email { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public UserImageInput? Image { get; set; }

}
