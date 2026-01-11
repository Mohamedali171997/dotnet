using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ges_sco.API.Models
{
    public class Class
    {
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; } // e.g. "L1-CS"

        public required string Level { get; set; }  // e.g. "Licence 1"
        
        public required string AcademicYear { get; set; } // e.g. "2024-2025"

        public int Capacity { get; set; } = 30;

        // Navigation properties
        public ICollection<Student> Students { get; set; } = new List<Student>();
        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}
