using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EstaAllCompany_CMS.Infrastructure.Context;
using EstaAllCompany_CMS.Infrastructure.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EstaAllCompany_CMS.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly ProjectContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(ProjectContext context, IWebHostEnvironment webHostEnvironment)
        {
            this._context = context;
            this._webHostEnvironment = webHostEnvironment;//IWebHostEnvironment bize resimleri kaldırma ekleme kısımlarında yardım olacak bizim resimler için server'ımız olacak gibi düşünülebilir.. 

        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.Include(x => x.Category).ToListAsync());//Category'si olan tüm ürünleri listele diyoruz.
        }

        public IActionResult Create()
        {
            //Daha öncede product eklerken Category'i yüklenmiş halde getiriyorduk ama farklı şekilde. Biz önceden  modele dolduruyorduk burada Farklı olrak ViewBag'lerle taşıyoruz.
            //Viewbag ile category'ilerimi taşımak istiyorum bunuda CategoryId üzerinden yapıyorum.New SelectList yani yeni bir liste oluştur ve bunu Sort'una göre sırala ve Id ile Name'lerini getir diyorum.

            ViewBag.CategoryId = new SelectList(_context.Categories.OrderBy(x => x.Sorting), "Id", "Name");

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            ViewBag.CategoryId = new SelectList(_context.Categories.OrderBy(x => x.Sorting), "Id", "Name");

            if (ModelState.IsValid)//Eğer kendime model aldığım Product classındaki Propertyler istenilen şartlara sahipse
            {
                product.Slug = product.Name.ToLower().Replace(" ", "-");//product Slug'ına Name'ine verilen değeri küçük harflere çevir ve içinde boşuk varsa boşluk kadar tire(-) ekle ve Slug'ına o değeri ver diyorum.

                var slug = await _context.Products.FirstOrDefaultAsync(x => x.Slug == product.Slug);//daha sonra yeni olusturulan Slug'la aynı Slug değerine sahip bir Slug varmı yok mu diye bak ve bunun adına slug diyorum.

                if (slug != null)//Eğer bu slug boş değilse yani aynı değerde bir slug varsa
                {
                    ModelState.AddModelError("", "The product already exists..!");//zaten böyle bir product mevcut diye mesaj iletecek.Biz burada Slug'la kontrolleri gerçekleştiriyoruz Slug'ları Id gibi uniq yani tek gibi kullandıgımız için kontrolleri onun üzerinden gerçekleştiriyoruz.
                    return View(product);//Mesajla birlikte olusturulmaya çalışılan product ekranda görünsün
                }

                string imageName = "noimage.png";//İmageName'ine Default bir değer atadım.
                if (product.ImageUpload != null)//product'ın ImageUpload'ı null yani boş değilse
                {
                    //Dir birşeyin yolunu belirtir."-Dir" bir dizin yapısıdır.

                    string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");//Burada diyoumki _webHostEnvironment'in WebRootPath'unu al ve media içindeki products'ın içine göm diyoruz.
                                                                                                       //Not bu aşamadan sonra wwwroot içerinde media ve media klasörünün içerisine products klasörü açıyoruz.
                                                                                                       //Bu klasör sayesinde eklenen her fotograf aynı zamanda burada öbeklenecek.
                                                                                                       //_webHostEnvironment.WebRootPath bu sayede kendi fotograflarımıda bir yol gösterebiliyorum.Gidin burada kayıt olun diyoruz.

                    //Burada her bir resim yükleme işlemi esnasında her product'ın İmageName'ini Uniq yapıyorum ki çakışmalar olmasın
                    imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;//Guid olarak bir veri üret bunu string'e çevir ve bu veriyi yeni olusturulmaya çalışılan product'ın isminede araya tire ekleyerek db'de Imageatamasını yap diyoruz bunun yeni ismide imageName olarak atıyoruz

                    string filePath = Path.Combine(uploadDir, imageName);//uploadDir uzantısı ile imageName'ini birleştir yani Combine et ve bunu string tipte olan filePath'e ata diyoruz.

                    FileStream fs = new FileStream(filePath, FileMode.Create);//?
                    await product.ImageUpload.CopyToAsync(fs);//?
                    fs.Close();//?

                    //FileMode= İşletim sisteminin bir dosyayı nasıl açması gerektiğini belirtir.
                    //FileStream:dosyalar ile akış işlemleri yapmamızı sağlar. 
                    //Close: Akış kapatılır. 
                }

                product.Image = imageName;//Artık olusturmak istediğim product'ın Image'ine olusturmus oldugu imageName'i ekle

                _context.Add(product);
                await _context.SaveChangesAsync();

                TempData["Success"] = "The product has been added..!";
            }

            return View(product);
        }
        public async Task<IActionResult> Edit(int id)
        {
            Product product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }
            ViewBag.CategoryId = new SelectList(_context.Categories.OrderBy(x => x.Sorting), "Id", "Name", product.CategoryId);//Burada edit edeceğinden ayrıca db'den buldugumuz product'ın  CategoryId'sinide taşıyacak

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            ViewBag.CategoryId = new SelectList(_context.Categories.OrderBy(x => x.Sorting), "Id", "Name", product.CategoryId);

            if (ModelState.IsValid)
            {
                product.Slug = product.Name.ToLower().Replace(" ", "-");

                var slug = await _context.Products.Where(x => x.Id != id).FirstOrDefaultAsync(x => x.Slug == product.Slug);

                if (slug != null)
                {
                    ModelState.AddModelError("", "The Product already exist");
                    return View(product);
                }
                //Resimle ilgili kısımlar
                if (product.ImageUpload != null)//ImageUploadda biz image'in yolunu tutuyorduk.boş mu değil mi diye bakarız önce
                {
                    string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
                    if (!string.Equals(product.Image, "noimage.png"))//noimage.png bizim default yani başlangıç değerimizde gelen Imagele eşit değilse diyoruz
                    {
                        string oldImagePath = Path.Combine(uploadDir, product.Image);

                        if (System.IO.File.Exists(oldImagePath))//
                        {
                            System.IO.File.Delete(oldImagePath);//Bu Operasyonlar yeni foto uygunsa eski fotoyu sil diyoruz.
                        }
                    }
                    //Bu kısımda eski fotografı sildi artık yeni Image Ekliyecek gibi işlemleri sürdürecek aynı creat operasyonunda yaptığımız gibi.
                    string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;//Guid olarak bir veri üret bunu string'e çevirve bu veriyi yeni olusturulmaya çalışılan product'ın ImageUpload'ın FileName'ine yan yana yazıp araya tire ekle

                    string filePath = Path.Combine(uploadDir, imageName);//uploadDir uzantısı ile imageName'ini birleştir yani Combine et ve bunu string tipte olan filePath'e ata diyoruz.

                    FileStream fs = new FileStream(filePath, FileMode.Create);//Burada tüm işlemleri yapıp biraraya getiriyoruz.
                    await product.ImageUpload.CopyToAsync(fs);//?
                    fs.Close();//fs'i kapt
                    product.Image = imageName;//yeni olusturudugum fotografada imageName attım.


                }
                _context.Update(product);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Product has been edited";

                return RedirectToAction("Index");
            }
            TempData["Error"] = "Product hasn't been edited";

            return View(product);
        }
        public async Task<IActionResult> Delete(int id)
        {
            Product product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                TempData["Warning"] = "The Product doesn't exist";
            }
            else
            {
                if (!string.Equals(product.Image, "noimage.png"))//Yakaladıgımız product'ın Image varsa
                {
                    string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");//media içindeki products classındaki Image'i uploadDir yoluna koy
                    string oldImagePath = Path.Combine(uploadDir, product.Image);//bunuda eski fotografa ata

                    if (System.IO.File.Exists(oldImagePath))//Varsa böyle bir foto
                    {
                        System.IO.File.Delete(oldImagePath);//sistemden sil IO'nun Delete metodu sayesinde
                    }
                }


                _context.Remove(product);//db'den kaldır bu product'ı
                await _context.SaveChangesAsync();//Değişiklikleri kaydet

                TempData["Success"] = "The Product Has Been removed";//Mesajını göster
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Details(int id)
        {
            Product product = await _context.Products.Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
    }
}