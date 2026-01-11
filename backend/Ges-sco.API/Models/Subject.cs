using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ges_sco.API.Models
{
    public class Subject
    {
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public required string Code { get; set; }

        public double Coefficient { get; set; }
        public int Credits { get; set; }
        public string? Description { get; set; }

        // Navigation properties
        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}
