using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        [TempData]
        public string StatusMessage { get; set; }

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

        // GET CREATE
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

        //POST CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubcategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var doesSubCategoryExists = _db.Subcategory.Include(s => s.Category).Where(s => s.Name == model.subcategory.Name && s.Category.Id == model.subcategory.CategoryId);

                if (doesSubCategoryExists.Count()>0)
                {
                    StatusMessage = "Error - Subcategory already exists." + doesSubCategoryExists.First().Name;
                }
                else
                {
                    _db.Subcategory.Add(model.subcategory);
                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            SubcategoryViewModel modelVM = new SubcategoryViewModel()
            {
                CategoryList = await _db.Category.ToListAsync(),
                subcategory = model.subcategory,
                subcategoryList = await _db.Subcategory.OrderBy(p => p.Name).Select(p => p.Name).ToListAsync(),
                StatusMessage = StatusMessage
            };
            return View(modelVM);
        }

        // ACTION METHOD 
        [ActionName("GetSubcategory")]
        public async Task<IActionResult> GetSubcategory(int id)
        {
            List<Subcategory> subcategories = new List<Subcategory>();

            subcategories = await (from subCategory in _db.Subcategory
                             where subCategory.CategoryId == id
                             select subCategory).ToListAsync();

            return Json(new SelectList(subcategories, "Id", "Name"));
        }

        // GET EDIT
        public async Task<IActionResult> Edit(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var subCategory = await _db.Subcategory.SingleOrDefaultAsync(m => m.Id == Id);
            if (subCategory == null)
            {
                return NotFound();
            }

            SubcategoryViewModel model = new SubcategoryViewModel()
            {
                CategoryList = await _db.Category.ToListAsync(),
                subcategory = subCategory,
                subcategoryList = await _db.Subcategory.OrderBy(p => p.Name).Select(p => p.Name).Distinct().ToListAsync()
            };

            return View(model);
        }

        //POST EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SubcategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var doesSubCategoryExists = _db.Subcategory.Include(s => s.Category).Where(s => s.Name == model.subcategory.Name && s.Category.Id == model.subcategory.CategoryId);

                if (doesSubCategoryExists.Count() > 0)
                {
                    StatusMessage = "Error - Subcategory already exists. " + doesSubCategoryExists.First().Name;
                }
                else
                {
                    var subCatFromDb = await _db.Subcategory.FindAsync(model.subcategory.Id);
                    subCatFromDb.Name = model.subcategory.Name;



                    _db.Subcategory.Add(model.subcategory);
                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            // IF STATE NOT VALID RELOAD THE MODEL 
            SubcategoryViewModel modelVM = new SubcategoryViewModel()
            {
                CategoryList = await _db.Category.ToListAsync(),
                subcategory = model.subcategory,
                subcategoryList = await _db.Subcategory.OrderBy(p => p.Name).Select(p => p.Name).ToListAsync(),
                StatusMessage = StatusMessage
            };
            return View(modelVM);
        }
    }
}