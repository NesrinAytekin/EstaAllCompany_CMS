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
        [MinLength(2, ErrorMessage = "Minumum Length is 2")]
        public string UserName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Password), Required, MinLength(4, ErrorMessage = "Minumum Length is 4 ")]
        public string Password { get; set; }

    }
}
