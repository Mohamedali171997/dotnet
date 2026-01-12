using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ges_sco.API.DTOs.Requests
{
    public class BulkAttendanceDto
    {
        [Required]
        public int CourseId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public List<StudentAttendanceItem> Attendances { get; set; } = new();
    }

    public class StudentAttendanceItem
    {
        [Required]
        public int StudentId { get; set; }

        [Required]
        [RegularExpression("^(Present|Absent|Late)$")]
        public string Status { get; set; } = "Absent";
    }
}
