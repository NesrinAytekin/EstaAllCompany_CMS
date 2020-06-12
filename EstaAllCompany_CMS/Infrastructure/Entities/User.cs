using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EstaAllCompany_CMS.Infrastructure.Entities
{
    [Table("Users")]
    public class User:BaseEntity
    {
        [Required]
        [MaxLength(10, ErrorMessage = "Maximum Length is 10")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(10, ErrorMessage = "Maximum Length is 10")]
        public string LastName { get; set; }

        [Required]
        [MaxLength(10, ErrorMessage = "Maximum Length is 10")]
        public string UserName { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Maximum Length is 50")]
        public string Email { get; set; }

        [Required]
        [MaxLength(10, ErrorMessage = "Maximum Length is 10")]
        public string Password { get; set; }

    }
}
