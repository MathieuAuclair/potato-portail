using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SysInternshipManagement.Models
{
    public class Student
    {
        [Key]
        public int idStudent { get; set; }

        [Required]
        public string firstName { get; set; }

        [Required]
        public string lastName { get; set; }

        [Required]
        public string emailSchool { get; set; }

        [Required]
        public string emailPersonal { get; set; }

        [Required]
        public string phone { get; set; }

        [Required]
        public string DaNumber { get; set; }

        [Required]
        public string permanentCode { get; set; }

        [Required]
        public string role { get; set; }
        
        [ForeignKey("preference")]
        public virtual ICollection<int> idPreference { get; set; }

    }
}