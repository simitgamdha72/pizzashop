using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pizzashop.Models.Models;
using pizzashop.Models.ViewModels;
using pizzashop.Repositories;
using pizzashop.Services;

namespace pizzashop.Controllers
{
    public class CategoryController : Controller
    {

        private readonly PizzashopContext _context;


        private readonly icategoryservice _categoryService;



        public CategoryController(icategoryservice categoryService, PizzashopContext context)
        {
            _categoryService = categoryService;
            _context = context;


        }


        [Authorize]
        public IActionResult menu()
        {
            return View();
        }

        public IActionResult ShowAddCategoryModal()
        {
            return PartialView("_AddCategoryModal");
        }

        [HttpPost]
        public IActionResult addcategory(CategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            int validator = _categoryService.AddCategory(model);

            if (validator == 1)
            {
                TempData["CategoryExist"] = "This Category is Allready Exist";
                return RedirectToAction("menu", "Category");
            }

            TempData["CategoryAdded"] = "New Category is Added";
            return RedirectToAction("menu", "Category");
        }

        public IActionResult DeleteCategoryModel(int id)
        {
            var showcategory = _categoryService.GetCategoryViewModelOnlyId(id);

            return PartialView("_deletecategory", showcategory);
        }

        [HttpPost]
        public IActionResult deletecategory(int id)
        {
            _categoryService.DeleteCategory(id);

            TempData["CategoryIsDeleted"] = "Category Deleted Successfully";

            return RedirectToAction("menu", "Category");

        }




        public IActionResult EditCategoryModal(int id)
        {
            var viewModel = _categoryService.GetCategoryViewModel(id);
            return PartialView("_modeleditcategory", viewModel);
        }

        [HttpPost]
        public IActionResult EditCategory(CategoryViewModel model)
        {

            var validator = _categoryService.EditCategory(model);

            if (validator == 1)
            {
                return Json(false);
            }

            if (validator == 2)
            {
                TempData["CategoryExist"] = "Category is alredy exist!";
                return Json(false);
            }

            else
            {
                TempData["CategoryEdit"] = "Category is Updated";
                return Json(true);
            }



        }



        // ajaz
        [HttpGet]
        public IActionResult GetCategories()
        {
            var categories = _categoryService.GetCategories();  // Fetch updated categories
            return PartialView("_CategoryList", categories);  // Return partial view with categories data
        }

        public IActionResult AddItemModal()
        {
            ViewBag.category = _context.Categories.Where(u => u.Isdeleted != true)
            .ToList();
            ViewBag.unit = _context.Units.ToList();
            return PartialView("_modeladditem");  // This returns the _modeladditem partial view
        }

        public IActionResult EditItemModal(int id)
        {
            ViewBag.category = _context.Categories.Where(u => u.Isdeleted != true)
            .ToList();
            ViewBag.unit = _context.Units.ToList();
            var item = _context.MenuItems.FirstOrDefault(x => x.Id == id);

            var showitem = new ItemViewModel
            {
                Id = item.Id,
                Name = item.Name,
                CategoryId = item.CategoryId,
                ItemType = item.ItemType,
                Rate = item.Rate,
                Quantity = item.Quantity,
                Available = item.Available,
                DefaultTax = item.DefaultTax,
                Taxpercentage = item.Taxpercentage,
                ShortCode = item.ShortCode,
                UnitId = item.UnitId,
                Description = item.Description,


            };

            return PartialView("_modeledititem", showitem);

        }

        public IActionResult additem(ItemViewModel model)
        {

            var item = new MenuItem
            {
                Name = model.Name,
                CategoryId = model.CategoryId,
                ItemType = model.ItemType,
                Rate = model.Rate,
                Quantity = model.Quantity,
                Available = model.Available,
                DefaultTax = model.DefaultTax,
                Taxpercentage = model.Taxpercentage,
                ShortCode = model.ShortCode,
                UnitId = model.UnitId,
                Description = model.Description,

            };

            _context.MenuItems.Add(item);

            _context.SaveChanges();

            return RedirectToAction("menu", "Category");
        }

        [HttpPost]
        public IActionResult edititem(ItemViewModel model)
        {

            MenuItem item = _context.MenuItems.FirstOrDefault(x => x.Id == model.Id);



            item.Name = model.Name;
            item.CategoryId = model.CategoryId;
            item.ItemType = model.ItemType;
            item.Rate = model.Rate;
            item.Quantity = model.Quantity;
            item.Available = model.Available;
            item.DefaultTax = model.DefaultTax;
            item.Taxpercentage = model.Taxpercentage;
            item.ShortCode = model.ShortCode;
            item.UnitId = model.UnitId;
            item.Description = model.Description;


            _context.MenuItems.Update(item);
            _context.SaveChanges();
            // return RedirectToAction("menu", "Category");
            return PartialView("_dummy");

        }

        [HttpPost]
        public IActionResult deleteitem(int id)
        {
            MenuItem item = _context.MenuItems.FirstOrDefault(x => x.Id == id);
            item.Isdeleted = true;
            _context.MenuItems.Update(item);
            _context.SaveChanges();
            // return RedirectToAction("menu", "Category");
            return Json(new { success = true }); ;
        }



        public IActionResult GetMenuItemsTable(int? id)
        {
            // Fetch your menu items from the database
            var menuItems = _context.MenuItems.Where(m => m.Isdeleted != true && m.CategoryId == id).ToList();

            // Return the Partial View with the menu items
            return PartialView("_itemlist", menuItems);
        }




        public IActionResult deleteitemmodel(int id)
        {

            var item = _context.MenuItems.FirstOrDefault(x => x.Id == id);

            var showitem = new ItemViewModel
            {
                Id = item.Id,
            };

            return PartialView("_deleteitem", showitem);

        }




    }
}

