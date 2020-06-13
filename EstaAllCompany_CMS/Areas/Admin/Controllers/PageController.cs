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
    [Area("Admin")] //Bu Attribute yazmamın sebebi Admin areasında çalıştıgını belirtmek için yazılır.
    public class PageController : Controller
    {
        //Asencron programlamanın amacı:standart .Net'te birbirini takip eden işlemler yapımaktaydı.Ancak Asencron programlamada farklı taskların aynı anda eş zamanlı olarak multi taskların çalışmaları durumunda kullanıyoruz.
        //Linkedin twitter gibi renderbodyde gerçekleşen olaylar hareket halindeyken sağ sol taraftaki tasklar sabit kalıyor.

        private readonly ProjectContext _context;
        public PageController(ProjectContext context)
        {
           this._context= context;
        }

        public async Task<IActionResult> Index()  //Asencron programlama kullanımı ile listeleme işlemi
        {
            return View(await _context.Pages.Where(x=>x.Status==Status.Active).OrderBy(x => x.Sorting).ToListAsync());
        }

        public async Task <IActionResult>Details(int id)//Listedki veriyi tıkladığımda taşıyacağı id 
        {
            Page page = await _context.Pages.FirstOrDefaultAsync(x => x.Id == id);

            if (page==null)
            {
                return NotFound(); //Bulunamadı diye dönüş yap
            }
            else
            {
                return View(page);
            }
        }

        public IActionResult Create() => View();//Tek Satır oldugunda (=>)bu şekilde gösterdik

        [HttpPost]
        [ValidateAntiForgeryToken] //İşlem yapılacak yerde çözülmesi için ekstra güvenlik önlemi Klasik .Net'te login işlemnde yapmıştık aynısını
        public async Task<IActionResult> Create(Page page)
        {
            if (ModelState.IsValid)
            {
                page.Slug = page.Title.ToLower().Replace(" ", "-");
                page.Sorting = 25;

                var slug = await _context.Pages.FirstOrDefaultAsync(x => x.Slug == page.Slug);

                if (slug !=null)
                {
                    ModelState.AddModelError("", "The Page Already Exist");
                }
                _context.Add(page); //Yoksa Db'ye ekle yeni oluşturduğum modeli diyoruz.
                await _context.SaveChangesAsync(); //Buradada Db'ye kaydet diyoruz.

                TempData["Success"] = "The page has been added";
                return RedirectToAction("Index");

            }
            else
            {
                TempData["Error"] = "The page hasn't been added";//Eğer en başta if blogunda istemiş oldugum modelimdeki uygunlukta değilse verilerim zaten if bloguna girmeden else'den çıkıp error ver diyip 
                return View(page);//Olusturulmaya çalısılan model ekranda görünsün diyoruz.
            }
           
        }

        public async Task<IActionResult> Edit(int id) //Update operasyonu 
        {
            Page page =await _context.Pages.FindAsync(id);//Yakaldığın id'ye sahip Page'i yakala

            if (page==null)//Eğer boşsa böyle bir page yoksa
            {
                NotFound();//Bulunamadı mesajı ver
            }
            return View(page);//Bulduysan bana o sayfayı dön diyorum
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //İşlem yapılacak yerde çözülmesi için ekstra güvenlik önlemi almak için.
        public async Task<IActionResult> Edit(Page page)
        {
            if (ModelState.IsValid)
            {
                page.Slug = page.Id == 1 ? "home" : page.Title.ToLower().Replace(" ", "-");

                var slug = await _context.Pages.Where(x => x.Id != page.Id).FirstOrDefaultAsync(x => x.Slug == page.Slug);
                if (slug !=null)
                {
                    ModelState.AddModelError("", "The home Page can't edit...");
                }

                _context.Update(page);
                await _context.SaveChangesAsync();

                TempData["Success"] = "The Page has been Edit";//Bu şartlara uygun olursa "The Page has been Edit" diye ekran sayfaya gelecek.
                return RedirectToAction("Edit", new { id = page.Id });//Core'daki redirect formatı
            }

            else
            {
                TempData["Error"] = "The page hasn't been edit..";//Bu şartlara uygun değilse "The Page hasn't been Edit" diye error mesajı verecek 
                return View(page);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            Page page =await  _context.Pages.FindAsync(id);//Once silmek istediğimiz modeli id'den yakalıyoruz

            if (page==null)//Eğer boyle bir page sayfası bulamazsa
            {
                TempData["Error"] = "The Page doesn't exist";//Error mesajını göster
            }

            else
            {
                page.Status = Status.Passive;
                page.DeleteDate = DateTime.Now;
                await _context.SaveChangesAsync();
                TempData["Success"] = "The Page has been deleted";//Success mesajını göster
                
            }

            return RedirectToAction("Index");//Sonuç başarılı da olsa başarısızda olsa Index kısmına geri dön diyoruz
        }

        public async Task <IActionResult> ReOrder(int[] id)//İçine parametre olarak int tipinde[] dizi alacak
        {
            int count = 1; //Say ve başlangıç değerini "1" al diyorum
            foreach (var pageId in id)//foreachle bizim page'gnderdiğim ve adı id olan dizinin içerisnde pageId olarak dolşıyorum
            {
                Page page = await _context.Pages.FindAsync(pageId);//Sürükleyeceğim o andaki row Id'den 
                page.Sorting = count;//Bu andan itibaren saymaya başla
                _context.Update(page);//EntityFramework'ün Update metodu ile update et
                await _context.SaveChangesAsync();//Değişikleri kaydet
                count++;//sonrada geçtiği row'dan itibaren say birer birer diyoruz
            }

            return Ok(); //Status code.
        }
    }
}