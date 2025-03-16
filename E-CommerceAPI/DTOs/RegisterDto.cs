using System.ComponentModel.DataAnnotations;

namespace E_CommerceAPI.DTOs
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string DisplayName { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        [RegularExpression("^(?=.*[A-Z])(?=.*[a-z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$",
              ErrorMessage = "Password must be at least 8 characters long, contain an uppercase letter, a lowercase letter, a number, and a special character.")]
        public string Password { get; set; }
    }
}
