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
        public IActionResult menu(int id)
        {


            var categories = _categoryService.GetCategories();

            if (id == 0)
            {
                // Check if the categories collection is not empty
                if (categories.Any())
                {
                    id = categories.First().Id;
                    Console.WriteLine(id);
                }
                else
                {

                    Console.WriteLine("No categories available");

                    id = -1;
                }
            }

            // var menuitems = _context.MenuItems.Where(m => m.CategoryId == id && m.Isdeleted != true)
            // .ToList();

            var cat = new CategoryViewModel
            {
                categories = categories,
                // menuItems = menuitems,

            };



            return View(cat);

            // return View();
        }

        //   [Authorize]
        // public IActionResult menu()
        // {
        //     // var categories = _categoryService.GetCategories();
        //     // var cat = new CategoryViewModel
        //     // {
        //     //     categories = categories
        //     // };
        //     // return View(cat);

        //     return View();
        // }

        [HttpPost]
        public IActionResult addcategory(CategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _categoryService.AddCategory(model);

            return RedirectToAction("menu", "Category");


        }





        [HttpPost]
        public IActionResult deletecategory(int id)
        {
            _categoryService.DeleteCategory(id);

            return RedirectToAction("menu", "Category");
        }







        public IActionResult EditCategoryModal(int id)
        {
            Category category = _context.Categories.FirstOrDefault(c => c.Id == id);
            var viewModel = new CategoryViewModel
            {
                Id = category.Id,
                Category = category.Category1,
                Description = category.Description,
            };
            return PartialView("_modeleditcategory", viewModel);
        }




        // [HttpPost]
        // public IActionResult EditCategory(CategoryViewModel model)
        // {

        //     Category category = _context.Categories.FirstOrDefault(c => c.Id == model.Id);

        //     var viewModel = new Category
        //     {
        //         Id = category.Id,
        //         Category1 = model.Category,
        //         Description = model.Description,

        //     };

        //     _context.Categories.Update(viewModel);

        //     return RedirectToAction("menu", "Category");
        // }

        [HttpPost]
        public IActionResult EditCategory(int Id, string Category1, string? Description)
        {
            _categoryService.EditCategory(Id, Category1, Description);

            return RedirectToAction("menu", "Category");
        }










        // // ajaz
        // [HttpGet]
        // public IActionResult GetCategories()
        // {
        //     var categories = _categoryService.GetCategories();  // Fetch updated categories
        //     return PartialView("_CategoryList", categories);  // Return partial view with categories data
        // }

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

