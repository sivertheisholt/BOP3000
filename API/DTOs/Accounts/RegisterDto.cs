using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Accounts
{
    public class RegisterDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [StringLength(16, MinimumLength = 8)]
        public string Password { get; set; }

        public string Gender { get; set; }
        public int CountryId { get; set; }
    }
}