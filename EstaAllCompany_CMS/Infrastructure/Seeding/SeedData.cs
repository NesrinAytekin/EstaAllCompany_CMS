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
                if (context.Categories.Any()) 
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
               

                context.SaveChanges();
            }
        }
    }
}
