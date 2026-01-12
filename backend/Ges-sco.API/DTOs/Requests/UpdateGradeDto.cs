using System;
using System.ComponentModel.DataAnnotations;

namespace Ges_sco.API.DTOs.Requests
{
    public class UpdateGradeDto
    {
        [Range(0, 20)]
        public double? Value { get; set; }

        [Range(0.1, 5)]
        public double? Coefficient { get; set; }

        [RegularExpression("^(Exam|CC|TP|TD|Projet)$", ErrorMessage = "Type must be Exam, CC, TP, TD or Projet")]
        public string? Type { get; set; }

        public string? Comment { get; set; }

        public DateTime? Date { get; set; }
    }
}
