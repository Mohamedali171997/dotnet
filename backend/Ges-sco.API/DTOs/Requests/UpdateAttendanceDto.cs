using System;
using System.ComponentModel.DataAnnotations;

namespace Ges_sco.API.DTOs.Requests
{
    public class UpdateAttendanceDto
    {
        public DateTime? Date { get; set; }

        [RegularExpression("^(Present|Absent|Late)$", ErrorMessage = "Status must be Present, Absent or Late")]
        public string? Status { get; set; }

        public bool? IsJustified { get; set; }

        public string? Justification { get; set; }
    }
}
