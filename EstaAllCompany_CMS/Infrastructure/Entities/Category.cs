using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EstaAllCompany_CMS.Infrastructure.Entities
{
    [Table("Categories")]
    public class Category:BaseEntity
    {
        [Required]
        [MinLength(3, ErrorMessage = "Minumum Length is 3")]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Slug { get; set; }
        public int Sorting { get; set; }
    }
}
