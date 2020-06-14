using EstaAllCompany_CMS.Infrastructure.Context;
using EstaAllCompany_CMS.Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EstaAllCompany_CMS.Infrastructure.Components
{
    public class MainMenuViewComponent : ViewComponent //ViewComponent'dan miras alır.
    {
        private readonly ProjectContext _context;
        public MainMenuViewComponent(ProjectContext context)
        {
            this._context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var pages = await GetPageAsync();//aşagıdaki işleride yani Task'ları bana burası çağırır birbirlerini beklemeden asenkron olarak çalısacak aşagıdaki belirteceğim bütün Task'ları çağırabilirim.Biz bu işlmide Delegatlerle yapıyoruz.
            return View(pages);
        }
        //Delegate:Bir yada birden fazla Taskı(yani işi) yönetmek için kullandığım bir yapıdır.Birde bunları Asenkron olarak tasarlarsam birbirini beklemeden işlemler gerçekleşir.

        private Task<List<Page>> GetPageAsync()//birden fazle sayfa olacağından Liste tipine aldık page'leri 
        {
            return _context.Pages.OrderBy(x => x.Sorting).ToListAsync();//Bize Page'leri sorting yani home=0'dı 0'dan başlayarak 1,2,3... diye sıralayıp bana getirecek.
        }
    }
}
