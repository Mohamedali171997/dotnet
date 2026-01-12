using System.ComponentModel.DataAnnotations;

namespace Ges_sco.API.DTOs.Requests
{
    public class UpdateClassDto
    {
        [StringLength(50)]
        public string? Name { get; set; }

        [StringLength(50)]
        public string? Level { get; set; }

        [StringLength(20)]
        public string? AcademicYear { get; set; }

        [Range(1, 100)]
        public int? Capacity { get; set; }
    }
}
