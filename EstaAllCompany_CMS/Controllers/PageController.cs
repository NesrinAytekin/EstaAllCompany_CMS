using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EstaAllCompany_CMS.Infrastructure.Context;
using EstaAllCompany_CMS.Infrastructure.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EstaAllCompany_CMS.Controllers
{
    [Authorize(Roles = "Admin,Editor")]
    [Area("Admin")]
    public class PageController : Controller
    {
        private readonly ProjectContext _context;
        public PageController(ProjectContext context)
        {
            this._context = context;
        }

        //  /page or/slug şeklinde olcak URL
        public async Task<IActionResult> Page(string slug)//Biz burada farklı bir URL Kullanıyoruz o yüzden o routing işlemlerini yapmamız gerekiyor bu işlemide Startup.cs de yazmamız gerekiyor bu aşamada
        {
            if (slug == null)
            {
                return View(await _context.Pages.Where(x => x.Slug == "home").FirstOrDefaultAsync());
            }

            Page page = await _context.Pages.Where(x => x.Slug == slug).FirstOrDefaultAsync();
            if (page == null)
            {
                return NotFound();
            }
            return View(page);
        }
    }
}