using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.API.Models
{
    public class UserInput
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string About { get; set; } = string.Empty;

        [EmailAddress]
        public string Email { get; set; }

    }
}
