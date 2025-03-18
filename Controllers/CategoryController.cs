using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pizzashop.Helpers;
using pizzashop.Models.Models;
using pizzashop.Models.ViewModels;
using pizzashop.Repositories;
using pizzashop.Services;

namespace pizzashop.Controllers
{
    public class CategoryController : Controller
    {

        private readonly PizzashopContext _context;

        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly icategoryservice _categoryService;



        public CategoryController(IWebHostEnvironment webHostEnvironment, icategoryservice categoryService, PizzashopContext context)
        {
            _categoryService = categoryService;
            _context = context;
            _webHostEnvironment = webHostEnvironment;


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
                return PartialView("_AddCategoryModal", model);
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

            // return RedirectToAction("menu", "Category");
            return Json(true);

        }




        public IActionResult EditCategoryModal(int id)
        {
            var viewModel = _categoryService.GetCategoryViewModel(id);
            return PartialView("_modeleditcategory", viewModel);
        }

        [HttpPost]
        public IActionResult EditCategory(CategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
                //    return PartialView("_modeleditcategory", model);
            }

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
                Taxpercentage = item.Taxpercentage ?? 0,
                ShortCode = item.ShortCode ?? 0,
                UnitId = item.UnitId,
                Description = item.Description,


            };

            return PartialView("_modeledititem", showitem);

        }

        public async Task<IActionResult> additem(ItemViewModel model)
        {

            if (!ModelState.IsValid)
            {
                TempData["SomethingIsMissing"] = "Something Went Wrong";
                return RedirectToAction("menu", "Category");
            }

            string fileName = null;
            if (model.MyImage != null && model.MyImage.Length > 0)
            {
                try
                {
                    var imageHelper = new ImageHelper(_webHostEnvironment);
                    fileName = await imageHelper.SaveImageAsync(model.MyImage);
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError("MyImage", ex.Message);
                    return View(model);
                }
            }

            var itemname = _context.MenuItems.FirstOrDefault(i => i.Name == model.Name);
            if (itemname != null)
            {
                TempData["ItemExist"] = "Item is Alreday Exist";
                return RedirectToAction("menu", "Category");
            }

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
                MyImage = fileName,

            };

            _context.MenuItems.Add(item);

            _context.SaveChanges();

            TempData["ItemisAdded"] = "Item added Successfully";

            return RedirectToAction("menu", "Category");
        }

        [HttpPost]
        public async Task<IActionResult> edititem(ItemViewModel model)
        {

            if (!ModelState.IsValid)
            {
                TempData["SomethingIsMissing"] = "Something Went Wrong";
                return RedirectToAction("menu", "Category");
            }
            string fileName = null;
            if (model.MyImage != null && model.MyImage.Length > 0)
            {
                try
                {
                    var imageHelper = new ImageHelper(_webHostEnvironment);
                    fileName = await imageHelper.SaveImageAsync(model.MyImage);
                }
                catch (ArgumentException ex)
                {
                    ModelState.AddModelError("MyImage", ex.Message);
                    return View(model);
                }
            }

            MenuItem item = _context.MenuItems.FirstOrDefault(x => x.Id == model.Id);
            var validateitem = _context.MenuItems.FirstOrDefault(x => x.Name == model.Name);

            if (validateitem != null)
            {
                if (item.Id != validateitem.Id)
                {
                    TempData["ItemExist"] = "Item is Alreday Exist";
                    return Json(false);

                }
            }



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
            item.MyImage = fileName;


            _context.MenuItems.Update(item);
            _context.SaveChanges();
            // return RedirectToAction("menu", "Category");
            // return PartialView("_dummy");
            TempData["ItemEdit"] = "Category is Updated";
            return Json(true);

        }

        [HttpPost]
        public IActionResult deleteitem(int id)
        {
            MenuItem item = _context.MenuItems.FirstOrDefault(x => x.Id == id);
            item.Isdeleted = true;
            _context.MenuItems.Update(item);
            _context.SaveChanges();
            // return RedirectToAction("menu", "Category");
            TempData["deleteItem"] = "Item Deleted Successfully";
            return Json(true);
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
                CategoryId = item.CategoryId,
            };

            return PartialView("_deleteitem", showitem);

        }




    }
}

