using System;
using System.ComponentModel.DataAnnotations;

namespace Ges_sco.API.DTOs.Requests
{
    public class CreateTeacherDto
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

        public string? Specialization { get; set; }

        public string? Qualification { get; set; }

        public DateTime HireDate { get; set; } = DateTime.UtcNow;
    }
}
