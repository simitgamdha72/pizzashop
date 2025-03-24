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


        public IActionResult GetTaxTable()
        {
            var taxes = _context.TaxesFees.Where(x => x.Isdeleted != true)
            .ToList();

            var viewModel = new TaxViewModel
            {
                taxes = taxes,
            };
            return PartialView("_TaxTable", viewModel);
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
                TempData["TaxExist"] = "Section is Alreday Exist";
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

            TempData["SectionAdd"] = "New Section is Added ";
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

            TempData["SectionIsDeleted"] = "Section Deleted Successfully";
            return Json(true);
        }


    }
}
