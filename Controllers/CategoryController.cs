// using System.Diagnostics;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using pizzashop.Models.Models;
// using pizzashop.Models.ViewModels;

// namespace pizzashop.Controllers;

// public class CategoryController : Controller
// {
//     private readonly PizzashopContext _context;

//     public CategoryController(PizzashopContext context)
//     {
//         _context = context;
//     }

//     [Authorize]
//     public IActionResult menu()
//     {
//         var categories = _context.Categories.Where(c => c.Isdeleted != true)
//         .ToList();
//         var cat = new CategoryViewModel
//         {
//             categories = categories,
//         };
//         return View(cat);
//     }


//     [HttpPost]
//     public IActionResult addcategory(CategoryViewModel model)
//     {

//         if (!ModelState.IsValid)
//         {
//             return View(model);
//         }

//         var viewModel = new Category
//         {
//             Category1 = model.Category,
//             Description = model.Description,
//         };

//         _context.Categories.Add(viewModel);
//         _context.SaveChanges();


//         return RedirectToAction("menu", "Category");
//     }

//     [HttpPost]
//     public IActionResult deletecategory(int id)
//     {
//         Category? cat = _context.Categories.FirstOrDefault(c => c.Id == id);

//         cat.Isdeleted = true;

//         Console.WriteLine(cat.Isdeleted);

//         _context.Categories.Update(cat);
//         _context.SaveChanges();

//         return RedirectToAction("menu", "Category");
//     }

//     [HttpPost]
//     public IActionResult EditCategory(int Id, string Category1, string? Description)
//     {
//         var category = _context.Categories.Find(Id);

//         if (category == null)
//         {
//             return NotFound(); // If category not found, return 404
//         }

//         // Update the category details
//         category.Category1 = Category1;
//         category.Description = Description;

//         // Save the changes to the database
//         _context.SaveChanges();

//         // Redirect to the category list or another page after updating
//         return RedirectToAction("menu", "Category");
//     }



// }




// CategoryController.cs
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

        // private readonly PizzashopContext _context;


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

            var menuitems = _context.MenuItems.Where(m => m.CategoryId == id)
            .ToList();

            var cat = new CategoryViewModel
            {
                categories = categories,
                menuItems = menuitems,

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
            ViewBag.category = _context.Categories.ToList();
            ViewBag.unit = _context.Units.ToList();
            return PartialView("_modeladditem");  // This returns the _modeladditem partial view
        }

        public IActionResult EditItemModal(int id)
        {
            ViewBag.category = _context.Categories.ToList();
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
            return View("menu", "Category");

        }



    }
}

