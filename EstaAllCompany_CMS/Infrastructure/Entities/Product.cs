using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EstaAllCompany_CMS.Infrastructure.Entities
{
    [Table("Products")]
    public class Product:BaseEntity
    {
        [Required]
        [MinLength(2, ErrorMessage = "Minumum Length is 2")]
        public string Name { get; set; }
       
        public string Slug { get; set; }
      
        [Required]
        [MaxLength(256, ErrorMessage = "Maximum Length is 256")]
        public string Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]//Type decimal olacak 18,2 demek iki haneli olacak virgülden sonrada tek hane olacak demek 18,2'nin bir anlamı yok istediğin sayıyı yazabiliriz 
        public decimal Price { get; set; }
        public string Image { get; set; }

        //[NotMapped]//Tabloda ayağa kalkma sutun olarak diyorum bu Attribute ile       
        //public IFormFile ImageUpload { get; set; }
      
        [Range(1, int.MaxValue, ErrorMessage = "You must choose a Category")]//MİN Değeri 1 ve int'rın max değerine kadar değer alabilir kullanıcı bir category seçmezse error mesajı vermesi için
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")] //Burda extradan ForeignKey diye belirledik ama yazmasakta olmaz default olarak zaten foreignKey oldugunu biliyor.
        public virtual Category Category { get; set; }
    }
}
