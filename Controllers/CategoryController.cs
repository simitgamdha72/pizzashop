using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pizzashop.Models.ViewModels;
using pizzashop.Services;

namespace pizzashop.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly icategoryservice _categoryService;
        private readonly iunitservice _unitService;
        private readonly imenuitemservice _menuitemService;

        public CategoryController(IWebHostEnvironment webHostEnvironment, icategoryservice categoryService, iunitservice unitService, imenuitemservice menuitemService)
        {
            _categoryService = categoryService;
            _webHostEnvironment = webHostEnvironment;
            _unitService = unitService;
            _menuitemService = menuitemService;
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

        [HttpGet]
        public IActionResult GetCategories()
        {
            var categories = _categoryService.GetCategories();

            return PartialView("_CategoryList", categories);
        }

        public IActionResult AddItemModal()
        {
            ViewBag.category = _categoryService.GetCategories();
            ViewBag.unit = _unitService.GetUnits();

            return PartialView("_modeladditem");
        }

        [HttpPost]
        public async Task<IActionResult> additem(ItemViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["SomethingIsMissing"] = "Something Went Wrong";
                return RedirectToAction("menu", "Category");
            }

            var validator = _menuitemService.AddItem(model);

            if (validator == 0)
            {
                TempData["ItemExist"] = "Item is Alreday Exist";
                return RedirectToAction("menu", "Category");
            }

            TempData["ItemisAdded"] = "Item added Successfully";
            return RedirectToAction("menu", "Category");
        }

        public IActionResult EditItemModal(int id)
        {
            if (id == null)
            {
                TempData["SomethingIsMissing"] = "Something Went Wrong";
                return RedirectToAction("menu", "Category");
            }

            ViewBag.category = _categoryService.GetCategories();
            ViewBag.unit = _unitService.GetUnits();

            var model = _menuitemService.EditItemModalShow(id);
            return PartialView("_modeledititem", model);
        }

        [HttpPost]
        public async Task<IActionResult> edititem(ItemViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["SomethingIsMissing"] = "Something Went Wrong";
                return RedirectToAction("menu", "Category");
            }

            var validator = _menuitemService.EditItem(model);

            if (validator == 0)
            {
                TempData["ItemExist"] = "Item is Alreday Exist";
                return Json(false);
            }

            TempData["ItemEdit"] = "Item is Updated";
            return Json(true);
        }

        [HttpPost]
        public IActionResult deleteitem(int id)
        {
            if (id == null)
            {
                TempData["SomethingIsMissing"] = "Something Went Wrong";
                return Json(false);
            }

            var validator = _menuitemService.DeleteItem(id);

            if (validator == 0)
            {
                TempData["SomethingIsMissing"] = "Something Went Wrong";
                return Json(false);
            }

            TempData["deleteItem"] = "Item Deleted Successfully";
            return Json(true);
        }

        public IActionResult GetMenuItemsTable(int? id, int page = 1, int pageSize = 5, string searchTerm = "")
        {
            var category = _categoryService.GetCategories().First();

            var viewModel = _menuitemService.GetPaginatedMenuItems(id, page, pageSize, searchTerm, category.Id);

            return PartialView("_itemlist", viewModel);
        }

        [HttpGet]
        public IActionResult SearchItems(int? categoryId, string searchTerm, int page = 1, int pageSize = 5)
        {
            return GetMenuItemsTable(categoryId, page, pageSize, searchTerm);
        }

        public IActionResult deleteitemmodel(int id)
        {
            var model = _menuitemService.DeleteItemModalShow(id);

            if (model == null)
            {
                TempData["SomethingIsMissing"] = "Something Went Wrong";
                return RedirectToAction("menu", "Category");
            }

            return PartialView("_deleteitem", model);
        }

    }
}

