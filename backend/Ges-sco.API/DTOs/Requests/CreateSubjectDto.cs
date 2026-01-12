using System.ComponentModel.DataAnnotations;

namespace Ges_sco.API.DTOs.Requests
{
    public class CreateSubjectDto
    {
        [Required]
        [StringLength(100)]
        public required string Name { get; set; }

        [Required]
        [StringLength(20)]
        public required string Code { get; set; }

        [Range(0.1, 10)]
        public double Coefficient { get; set; } = 1.0;

        [Range(1, 30)]
        public int Credits { get; set; } = 3;

        public string? Description { get; set; }
    }
}
