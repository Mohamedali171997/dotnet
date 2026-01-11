using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ges_sco.API.Models
{
    public class Student
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }

        public DateTime DateOfBirth { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? ParentContact { get; set; } // Phone or Email
        public string? MedicalInfo { get; set; }
        public string? StudentIdentificationNumber { get; set; } // Matricule

        public int ClassId { get; set; }
        [ForeignKey("ClassId")]
        public Class? Class { get; set; }
    }
}
