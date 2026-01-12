using System.ComponentModel.DataAnnotations;

namespace Ges_sco.API.DTOs.Requests
{
    public class RegisterDto
    {
        [Required]
        [StringLength(50)]
        public required string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public required string LastName { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [MinLength(6)]
        public required string Password { get; set; }

        [Required]
        [Compare("Password")]
        public required string ConfirmPassword { get; set; }

        [RegularExpression("^(Student|Teacher|Admin|SuperAdmin)$", ErrorMessage = "Role must be Student, Teacher, Admin or SuperAdmin")]
        public string Role { get; set; } = "Student";
    }
}
