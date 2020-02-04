using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spice.Data;
using Spice.Models;
using Spice.Models.ViewModels;

namespace Spice.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SubcategoryController : Controller
    {
        // Prop plus Constructor gives us Dependcy Injection
        private readonly ApplicationDbContext _db;

        public SubcategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET INDEX
        public async Task<IActionResult> Index()
        {
            // eager loading
            var subCategories = await _db.Subcategory.Include(s=>s.Category).ToListAsync();
            return View(subCategories);
        }

        public async Task<IActionResult> Create()
        {
            SubcategoryViewModel model = new SubcategoryViewModel()
            {
                CategoryList = await _db.Category.ToListAsync(),
                subcategory = new Models.Subcategory(),
                subcategoryList = await _db.Subcategory.OrderBy(p => p.Name).Select(p => p.Name).Distinct().ToListAsync()
            };

            return View(model);
        }
    }
}