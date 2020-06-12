using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EstaAllCompany_CMS.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;

namespace EstaAllCompany_CMS.Controllers
{
    public class CategoryController : Controller
    {

        private readonly ProjectContext _projectContext;

        public CategoryController(ProjectContext projectContext)
        {
            this._projectContext = projectContext;
        }
        public IActionResult Index()
        {
            return View(_projectContext.Categories.ToList());
        }
    }
}