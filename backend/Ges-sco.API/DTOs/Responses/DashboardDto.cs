namespace Ges_sco.API.DTOs.Responses
{
    public class DashboardDto
    {
        public int TotalStudents { get; set; }
        public int TotalTeachers { get; set; }
        public int TotalClasses { get; set; }
        public int TotalSubjects { get; set; }
        public int TotalCourses { get; set; }
        public double AverageAttendanceRate { get; set; }
        public double AverageGrade { get; set; }
        public int ActiveUsersToday { get; set; }
    }
}
