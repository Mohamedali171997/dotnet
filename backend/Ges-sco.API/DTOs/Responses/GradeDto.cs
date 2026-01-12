using System;

namespace Ges_sco.API.DTOs.Responses
{
    public class GradeDto
    {
        public int Id { get; set; }
        public double Value { get; set; }
        public double Coefficient { get; set; }
        public string Type { get; set; } = string.Empty;
        public string? Comment { get; set; }
        public DateTime Date { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public int CourseId { get; set; }
        public string SubjectName { get; set; } = string.Empty;
    }
}
