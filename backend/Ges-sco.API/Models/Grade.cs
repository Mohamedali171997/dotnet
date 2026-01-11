using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ges_sco.API.Models
{
    public class Grade
    {
        public int Id { get; set; }

        public double Value { get; set; } // Note sur 20
        public double Coefficient { get; set; } = 1.0;
        public string Type { get; set; } = "Exam"; // Exam, CC, TP
        public string? Comment { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;

        public int StudentId { get; set; }
        [ForeignKey("StudentId")]
        public Student? Student { get; set; }

        public int CourseId { get; set; }
        [ForeignKey("CourseId")]
        public Course? Course { get; set; }
    }
}
