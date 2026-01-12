using System;
using System.ComponentModel.DataAnnotations;

namespace Ges_sco.API.DTOs.Requests
{
    public class UpdateStudentDto
    {
        [StringLength(50)]
        public string? FirstName { get; set; }

        [StringLength(50)]
        public string? LastName { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? Address { get; set; }

        [Phone]
        public string? Phone { get; set; }

        public string? ParentContact { get; set; }

        public string? MedicalInfo { get; set; }

        public string? StudentIdentificationNumber { get; set; }

        public int? ClassId { get; set; }
    }
}
