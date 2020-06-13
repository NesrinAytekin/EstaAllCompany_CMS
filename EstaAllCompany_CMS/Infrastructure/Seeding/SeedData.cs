using EstaAllCompany_CMS.Infrastructure.Context;
using EstaAllCompany_CMS.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EstaAllCompany_CMS.Infrastructure.Seeding
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ProjectContext(serviceProvider.GetRequiredService<DbContextOptions<ProjectContext>>()))
            {
                if (context.Categories.Any() && context.Pages.Any()) 
                {
                    return;
                }

                context.Categories.AddRange(
                    new Category
                    {
                        Name = "Eye",
                        Description = "Everything about  with Eye Makeup",
                    },
                    new Category
                    {
                        Name = "Lip",
                        Description = "Everything about  with Lip Makeupt",
                    }
                );

                context.Pages.AddRange(
                   new Page //Instance alıp Page Sınıfından verileri ekliyoruz.
                    {
                       Title = "Home",
                       Slug = "home",
                       Content = "home Page",
                       Sorting = 0,
                   },
                   new Page
                   {
                       Title = "About Us",
                       Slug = "about-us",
                       Content = "about us page",
                       Sorting = 100,
                   },
                   new Page
                   {
                       Title = "Services",
                       Slug = "services",
                       Content = "services page",
                       Sorting = 100,
                   },
                   new Page
                   {
                       Title = "Contact",
                       Slug = "contact",
                       Content = "contact page",
                       Sorting = 100,
                   }
                   );


                context.SaveChanges();
            }
        }
    }
}
