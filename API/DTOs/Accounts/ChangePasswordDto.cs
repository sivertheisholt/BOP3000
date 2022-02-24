using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.Accounts
{
    public class ChangePasswordDto
    {
        public string CurrentPassword { get; set; }
        [Required]
        [StringLength(32, MinimumLength = 8)]
        public string NewPassword { get; set; }
    }
}