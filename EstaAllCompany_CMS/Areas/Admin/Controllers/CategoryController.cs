using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EstaAllCompany_CMS.Infrastructure.Context;
using EstaAllCompany_CMS.Infrastructure.Entities;
using EstaAllCompany_CMS.Infrastructure.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EstaAllCompany_CMS.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ProjectContext _context;
        public CategoryController(ProjectContext context)
        {
            this._context = context;
        }
        public async Task< IActionResult> Index()
        {
            return View(await _context.Categories.Where(x=>x.Status==Status.Active).OrderBy(x=>x.CreateDate).ToListAsync());
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                category.Slug = category.Name.ToLower().Replace(" ", "-");
                category.Sorting = 10;

                var slug = await _context.Categories.FirstOrDefaultAsync(x => x.Slug == category.Slug);
                if (slug !=null)
                {
                    ModelState.AddModelError("", "The Category already exists..");
                    return View(category);
                }
                _context.Add(category);
                await _context.SaveChangesAsync();
                TempData["Success"] = "The Category has been added";
                return RedirectToAction("Index");

            }
            else
            {
                TempData["Error"] = "The Category hasn't  been added";
                return View(category);
            }
        }

        public async Task<IActionResult>Details(int id)
        {
            Category category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category==null)
            {
                return NotFound(); //Bulunamadı diye dönüş yap
            }
            return View(category); //Bulursa o sayfayı bana döndür diyoruz.
        }

        public async Task<IActionResult>Edit(int id)
        {
            Category category = await _context.Categories.FindAsync(id);
            if (category==null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (ModelState.IsValid)
            {
                var slug = await _context.Categories.Where(x => x.Id != id).FirstOrDefaultAsync(x => x.Slug == category.Slug);
                if (slug !=null)
                {
                    ModelState.AddModelError("", "The Category already exist");
                    return View(category);
                }
                _context.Update(category);
                await _context.SaveChangesAsync();

                TempData["Success"] = "The Category has been edited";

                return RedirectToAction("Edit", new { id });//edit'e git giderkende id'yi yanında götür diyoruz.
            }
            else
            {
                TempData["Error"] = "The Category hasn't been edited";

                return View(category);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            Category category = await _context.Categories.FindAsync(id);//Once silmek istediğimiz modeli id'den yakalıyoruz

            if (category == null)//Eğer boyle bir CATEGORY sayfası bulamazsa boşsa yani
            {
                TempData["Error"] = "The Category doesn't exist";//Error mesajını göster
            }
            else
            {

                category.Status = Status.Passive;
                category.DeleteDate = DateTime.Now;
                await _context.SaveChangesAsync(); //Değişiklikleri kaydet
                TempData["Success"] = "The Page has been deleted";//Success mesajını göster
            }
            return RedirectToAction("Index");//Sonuç başarılı da olsa başarısızda olsa ındex kısmına geri dön diyoruz
        }
        public async Task<IActionResult> Reorder(int[] id)//içine int tipte bir dizi alacak
        {
            int count = 1;//saymak için count adında int tipte ve başlangıç değeri bir olan (yani default değeri 1) tanımladık

            foreach (var categoryId in id)//id isimli dizinin içinde categoryId categoryId dolaş diyoruz.
            {
                Category category = await _context.Categories.FindAsync(categoryId);//instanse aldığımız category adındaaki nesneye gelen categoryId'yi içine doldur varsa böyle bir id
                category.Sorting = count;//Geldiğinde o anki count değeri neyse Sorting'ine Onu vericez sonra 
                await _context.SaveChangesAsync();//Değişiklikleri kaydedip
                count++;//count'u birer birer artırıcaz.
            }

            return Ok();//En sonundada status code'u ok vericez yani işlem tamamlandı gibi.
        }
    }
}