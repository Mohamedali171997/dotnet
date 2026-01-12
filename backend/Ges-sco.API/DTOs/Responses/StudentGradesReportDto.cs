using System.Collections.Generic;

namespace Ges_sco.API.DTOs.Responses
{
    public class StudentGradesReportDto
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
        public string AcademicYear { get; set; } = string.Empty;
        public List<SubjectGradesDto> SubjectGrades { get; set; } = new();
        public double GeneralAverage { get; set; }
        public int TotalCredits { get; set; }
        public string Mention { get; set; } = string.Empty;
    }

    public class SubjectGradesDto
    {
        public string SubjectName { get; set; } = string.Empty;
        public string SubjectCode { get; set; } = string.Empty;
        public double Coefficient { get; set; }
        public int Credits { get; set; }
        public List<GradeDto> Grades { get; set; } = new();
        public double Average { get; set; }
        public bool IsPassed { get; set; }
    }
}
