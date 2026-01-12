using System;
using System.ComponentModel.DataAnnotations;

namespace Ges_sco.API.DTOs.Requests
{
    public class CreateAttendanceDto
    {
        [Required]
        public DateTime Date { get; set; }

        [Required]
        [RegularExpression("^(Present|Absent|Late)$", ErrorMessage = "Status must be Present, Absent or Late")]
        public string Status { get; set; } = "Absent";

        public bool IsJustified { get; set; } = false;

        public string? Justification { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        public int CourseId { get; set; }
    }
}
