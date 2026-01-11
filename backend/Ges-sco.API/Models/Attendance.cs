using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ges_sco.API.Models
{
    public class Attendance
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }
        public string Status { get; set; } = "Absent"; // Present, Absent, Late
        public bool IsJustified { get; set; } = false;
        public string? Justification { get; set; } // Path to document or text reason

        public int StudentId { get; set; }
        [ForeignKey("StudentId")]
        public Student? Student { get; set; }

        public int CourseId { get; set; }
        [ForeignKey("CourseId")]
        public Course? Course { get; set; }
    }
}
