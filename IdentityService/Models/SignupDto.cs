using System.ComponentModel.DataAnnotations;

namespace IdentityService.Models
{
    public class SignupDto
    {
        [Required(AllowEmptyStrings = false)]
        public string Username { get; set; }

        [Required]
        [MinLength(4)]
        public string Password { get; set; }

        [EmailAddress]
        public string? Email { get; set; }
    }
}
