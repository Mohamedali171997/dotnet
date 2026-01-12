using System;

namespace Ges_sco.API.DTOs.Responses
{
    public class StudentDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}";
        public string Email { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? ParentContact { get; set; }
        public string? MedicalInfo { get; set; }
        public string? StudentIdentificationNumber { get; set; }
        public int ClassId { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
