using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.Accounts
{
    public class ChangeForgottenPasswordDto
    {

        public string Email { get; set; }
        [Required]
        [StringLength(16, MinimumLength = 8)]
        public string NewPassword { get; set; }
    }
}