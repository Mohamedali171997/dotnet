using System.ComponentModel.DataAnnotations;

namespace Ges_sco.API.DTOs.Requests
{
    public class UpdateSubjectDto
    {
        [StringLength(100)]
        public string? Name { get; set; }

        [StringLength(20)]
        public string? Code { get; set; }

        [Range(0.1, 10)]
        public double? Coefficient { get; set; }

        [Range(1, 30)]
        public int? Credits { get; set; }

        public string? Description { get; set; }
    }
}
