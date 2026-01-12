using System;

namespace Ges_sco.API.DTOs.Responses
{
    public class AttendanceDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; } = string.Empty;
        public bool IsJustified { get; set; }
        public string? Justification { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public int CourseId { get; set; }
        public string SubjectName { get; set; } = string.Empty;
    }
}
