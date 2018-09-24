using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SysInternshipManagement.Models
{
    public class Student
    {
        [Key]
        public int IdStudent { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string EmailSchool { get; set; }

        [Required]
        public string EmailPersonal { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string DaNumber { get; set; }

        [Required]
        public string PermanentCode { get; set; }

        [Required]
        public string Role { get; set; }
        
        public virtual Preference Preference { get; set; }

    }
}