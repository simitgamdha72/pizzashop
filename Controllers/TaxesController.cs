using Microsoft.AspNetCore.Mvc;
using pizzashop.Models.Models;
using pizzashop.Models.ViewModels;

namespace pizzashop.Controllers
{
    public class TaxesController : Controller
    {

        private readonly PizzashopContext _context;

        public TaxesController(PizzashopContext context)
        {
            _context = context;
        }

        public IActionResult Taxes()
        {
            return View();
        }


        public IActionResult GetTaxTable(string searchTerm)
        {
            var taxes = _context.TaxesFees.Where(x => x.Isdeleted != true);
            // .ToList();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                taxes = taxes.Where(m => m.Name.ToLower().Contains(searchTerm));
            }

            var viewModel = new TaxViewModel
            {
                taxes = taxes,
                SearchTerm = searchTerm,
            };
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


            var validator = _context.TaxesFees.FirstOrDefault(x => x.Name == model.Name);
            if (validator != null)
            {
                TempData["TaxExist"] = "Tax is Alreday Exist";
                return Json(false);
            }


            var viewModel = new TaxesFee
            {
                Name = model.Name,
                Type = model.Type,
                TaxAmount = model.TaxAmount,
                IsEnable = model.IsEnable,
                IsDefault = model.IsDefault,


            };

            _context.TaxesFees.Add(viewModel);
            _context.SaveChanges();

            TempData["TaxAdd"] = "New Tax is Added ";
            return Json(new { success = true });
        }

        public IActionResult DeleteTaxModal(int id)
        {
            var tax = _context.TaxesFees.FirstOrDefault(x => x.Id == id);
            var viewModel = new TaxViewModel
            {
                Id = id,
            };

            return PartialView("_DeleteTax", viewModel);
        }

        [HttpPost]
        public IActionResult DeleteTax(int id)
        {
            var tax = _context.TaxesFees.FirstOrDefault(x => x.Id == id);

            if (tax == null)
            {
                TempData["SomethingIsMissing"] = "Something Went Wrong";
                return RedirectToAction("TableAndSection", "TableSection");
            }

            tax.Isdeleted = true;
            _context.TaxesFees.Update(tax);
            _context.SaveChanges();

            TempData["TaxIsDeleted"] = "Tax Deleted Successfully";
            return Json(true);
        }

        public IActionResult EditTaxModal(int id)
        {
            var tax = _context.TaxesFees.FirstOrDefault(x => x.Id == id);

            if (tax == null)
            {
                TempData["SomethingIsMissing"] = "Something Went Wrong";
                return RedirectToAction("TableAndSection", "TableSection");
            }

            var viewModel = new TaxViewModel
            {
                Id = tax.Id,
                Name = tax.Name,
                Type = tax.Type,
                TaxAmount = tax.TaxAmount,
                IsEnable = tax.IsEnable,
                IsDefault = tax.IsDefault,

            };

            return PartialView("_EditTaxModal", viewModel);

        }


        [HttpPost]
        public IActionResult EditTax(TaxViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["SomethingIsMissing"] = "Something Went Wrong";
                return RedirectToAction("TableAndSection", "TableSection");
            }

            TaxesFee? tax = _context.TaxesFees.FirstOrDefault(x => x.Id == model.Id);
            var validator = _context.TaxesFees.FirstOrDefault(x => x.Name == model.Name);

            if (tax == null)
            {
                TempData["SomethingIsMissing"] = "Something Went Wrong";
                return RedirectToAction("TableAndSection", "TableAndSection");
            }

            if (validator != null)
            {
                if (tax.Id != validator.Id)
                {
                    TempData["TaxExist"] = "Tax is Alreday Exist";
                    return Json(false);
                }
            }


            tax.Name = model.Name;
            tax.Type = model.Type;
            tax.TaxAmount = model.TaxAmount;
            tax.IsEnable = model.IsEnable;
            tax.IsDefault = model.IsDefault;


            _context.TaxesFees.Update(tax);
            _context.SaveChanges();

            TempData["TaxEdit"] = "Tax is Updated";
            return Json(true);
        }











    }
}
