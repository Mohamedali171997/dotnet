using System;
using System.ComponentModel.DataAnnotations;

namespace Ges_sco.API.DTOs.Requests
{
    public class CreateGradeDto
    {
        [Required]
        [Range(0, 20)]
        public double Value { get; set; }

        [Range(0.1, 5)]
        public double Coefficient { get; set; } = 1.0;

        [Required]
        [RegularExpression("^(Exam|CC|TP|TD|Projet)$", ErrorMessage = "Type must be Exam, CC, TP, TD or Projet")]
        public string Type { get; set; } = "Exam";

        public string? Comment { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;

        [Required]
        public int StudentId { get; set; }

        [Required]
        public int CourseId { get; set; }
    }
}
