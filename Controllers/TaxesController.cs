using Microsoft.AspNetCore.Mvc;
using pizzashop.Models.Models;
using pizzashop.Models.ViewModels;
using pizzashop.Services;

namespace pizzashop.Controllers
{
    public class TaxesController : Controller
    {
        private readonly itaxservice _taxService;

        public TaxesController(itaxservice taxService)
        {
            _taxService = taxService;
        }

        public IActionResult Taxes()
        {
            return View();
        }


        public IActionResult GetTaxTable(string searchTerm)
        {
            var viewModel = _taxService.GetTaxTable(searchTerm);
            return PartialView("_TaxTable", viewModel);
        }

        [HttpGet]
        public IActionResult SearchItems(string searchTerm)
        {
            return GetTaxTable(searchTerm);
        }

        public IActionResult AddTaxModal()
        {
            return PartialView("_AddTaxModal");
        }

        public IActionResult AddTax(TaxViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["SomethingIsMissing"] = "Something Went Wrong";
                return Json(false);
            }

            bool isTaxAdded = _taxService.AddTax(model);

            if (!isTaxAdded)
            {
                TempData["TaxExist"] = "Tax Already Exists";
                return Json(false);
            }

            TempData["TaxAdd"] = "New Tax is Added";
            return Json(new { success = true });
        }

        public IActionResult DeleteTaxModal(int id)
        {
            var viewModel = new TaxViewModel
            {
                Id = id,
            };

            return PartialView("_DeleteTax", viewModel);
        }

        [HttpPost]
        public IActionResult DeleteTax(int id)
        {
            var result = _taxService.DeleteTax(id);

            if (!result)
            {
                TempData["SomethingIsMissing"] = "Something Went Wrong";
                return RedirectToAction("Taxes", "Taxes");
            }

            TempData["TaxIsDeleted"] = "Tax Deleted Successfully";
            return Json(true);
        }

        public IActionResult EditTaxModal(int id)
        {
            var viewModel = _taxService.GetTaxForEdit(id);

            if (viewModel == null)
            {
                TempData["SomethingIsMissing"] = "Something Went Wrong";
                return RedirectToAction("Taxes", "Taxes");
            }

            return PartialView("_EditTaxModal", viewModel);
        }

        [HttpPost]
        public IActionResult EditTax(TaxViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["SomethingIsMissing"] = "Something Went Wrong";
                return RedirectToAction("Taxes", "Taxes");
            }

            if (!model.Id.HasValue)
            {
                return Json(true);
            }

            var result = _taxService.EditTax(model);

            if (!result)
            {
                TempData["TaxExist"] = "Tax Already Exists";
                return Json(false);
            }

            TempData["TaxEdit"] = "Tax is Updated";
            return Json(true);
        }
    }
}
