using System.ComponentModel.DataAnnotations;
using SysInternshipManagement.Controllers;

namespace SysInternshipManagement.Models.AspAuthentication
{
    public partial class AspNetUserClaims
    {
        public int Id { get; set; }

        [Required]
        [StringLength(128)]
        public string UserId { get; set; }

        public string ClaimType { get; set; }

        public string ClaimValue { get; set; }

        public virtual AspNetUsers AspNetUsers { get; set; }
    }
}
