using System.ComponentModel.DataAnnotations;

namespace Ges_sco.API.DTOs.Requests
{
    public class CreateClassDto
    {
        [Required]
        [StringLength(50)]
        public required string Name { get; set; }

        [Required]
        [StringLength(50)]
        public required string Level { get; set; }

        [Required]
        [StringLength(20)]
        public required string AcademicYear { get; set; }

        [Range(1, 100)]
        public int Capacity { get; set; } = 30;
    }
}
