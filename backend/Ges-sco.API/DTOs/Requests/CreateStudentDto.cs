using System;
using System.ComponentModel.DataAnnotations;

namespace Ges_sco.API.DTOs.Requests
{
    public class CreateStudentDto
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
        public DateTime DateOfBirth { get; set; }

        public string? Address { get; set; }

        [Phone]
        public string? Phone { get; set; }

        public string? ParentContact { get; set; }

        public string? MedicalInfo { get; set; }

        public string? StudentIdentificationNumber { get; set; }

        [Required]
        public int ClassId { get; set; }
    }
}
