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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pizzashop.Models.ViewModels;
using pizzashop.Services;

namespace pizzashop.Controllers
{
    public class CategoryController : Controller
    {
        private readonly icategoryservice _categoryService;

        public CategoryController(icategoryservice categoryService)
        {
            _categoryService = categoryService;
        }

        [Authorize]
        public IActionResult menu()
        {
            var categories = _categoryService.GetCategories();
            var cat = new CategoryViewModel
            {
                categories = categories
            };
            return View(cat);
        }

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
    }
}

