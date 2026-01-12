namespace Ges_sco.API.DTOs.Responses
{
    public class SubjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public double Coefficient { get; set; }
        public int Credits { get; set; }
        public string? Description { get; set; }
        public int CoursesCount { get; set; }
    }
}
