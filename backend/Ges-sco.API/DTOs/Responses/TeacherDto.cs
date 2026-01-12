using System;

namespace Ges_sco.API.DTOs.Responses
{
    public class TeacherDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}";
        public string Email { get; set; } = string.Empty;
        public string? Specialization { get; set; }
        public string? Qualification { get; set; }
        public DateTime HireDate { get; set; }
        public bool IsActive { get; set; }
        public int CoursesCount { get; set; }
    }
}
