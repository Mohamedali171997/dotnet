namespace Ges_sco.API.DTOs.Responses
{
    public class ClassDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;
        public string AcademicYear { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public int StudentsCount { get; set; }
        public int CoursesCount { get; set; }
    }
}
