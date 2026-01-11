using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ges_sco.API.Models
{
    public class Teacher
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }

        public string? Specialization { get; set; }
        public string? Qualification { get; set; }
        public DateTime HireDate { get; set; }

        // Navigation properties
        public ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}
