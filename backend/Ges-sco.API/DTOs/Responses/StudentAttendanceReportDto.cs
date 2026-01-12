using System.Collections.Generic;

namespace Ges_sco.API.DTOs.Responses
{
    public class StudentAttendanceReportDto
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
        public int TotalSessions { get; set; }
        public int PresentCount { get; set; }
        public int AbsentCount { get; set; }
        public int LateCount { get; set; }
        public int JustifiedAbsences { get; set; }
        public double AttendanceRate { get; set; }
        public List<AttendanceDto> Attendances { get; set; } = new();
    }
}
