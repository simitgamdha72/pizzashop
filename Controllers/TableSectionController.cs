using Microsoft.AspNetCore.Mvc;
using pizzashop.Models.Models;
using pizzashop.Models.ViewModels;

namespace pizzashop.Controllers
{
    public class TableSectionController : Controller
    {
        private readonly itablesectionservice _tableSectionService;

        public TableSectionController(itablesectionservice tableSectionService)
        {
            _tableSectionService = tableSectionService;
        }

        public IActionResult TableAndSection()
        {
            return View();
        }

        public IActionResult GetSection()
        {
            var sections = _tableSectionService.GetActiveSections();
            var viewModel = new SectionViewModal
            {
                section = sections,
            };
            return PartialView("_SectionList", viewModel);
        }

        [HttpGet]
        public IActionResult AddSectionModal()
        {
            return PartialView("_AddSectionModal");
        }

        public IActionResult AddSection(SectionViewModal model)
        {
            if (!ModelState.IsValid)
            {
                TempData["SomethingIsMissing"] = "Something Went Wrong";
                return Json(false);
            }

            bool result = _tableSectionService.AddSection(model);

            if (result)
            {
                TempData["SectionAdd"] = "New Section is Added";
                return Json(new { success = true });
            }
            else
            {
                TempData["SectionExist"] = "Section is Already Exist";
                return Json(false);
            }
        }


        public IActionResult EditSectionModal(int id)
        {
            var sections = _tableSectionService.GetSectionById(id);

            if (sections == null)
            {
                TempData["SomethingIsMissing"] = "Something Went Wrong";
                return RedirectToAction("TableAndSection", "TableSection");
            }

            var viewModel = new SectionViewModal
            {
                Id = sections.Id,
                Name = sections.Name,
                Description = sections.Description ?? "",

            };

            return PartialView("_EditSectionModal", viewModel);
        }

        [HttpPost]
        public IActionResult EditSection(SectionViewModal model)
        {
            if (!ModelState.IsValid)
            {
                TempData["SomethingIsMissing"] = "Something Went Wrong";
                return RedirectToAction("TableAndSection", "TableSection");
            }

            bool result = _tableSectionService.EditSection(model);

            if (result)
            {
                TempData["SectionEdit"] = "Section is Updated";
                return Json(true);
            }
            else
            {
                TempData["SectionExist"] = "Section is Already Exist";
                return Json(false);
            }
        }

        public IActionResult DeleteSectionModal(int id)
        {
            var viewModel = new SectionViewModal
            {
                Id = id,
            };

            return PartialView("_DeleteSectionModal", viewModel);
        }

        [HttpPost]
        public IActionResult DeleteSection(int id)
        {
            bool result = _tableSectionService.DeleteSection(id);

            if (result)
            {
                TempData["SectionIsDeleted"] = "Section Deleted Successfully";
                return Json(true);
            }
            else
            {
                TempData["SomethingIsMissing"] = "Something Went Wrong";
                return Json(false);
            }
        }

        public IActionResult GetTables(int? id, int page = 1, int pageSize = 5, string searchTerm = "")
        {
            var viewModel = _tableSectionService.GetTables(id, page, pageSize, searchTerm);
            return PartialView("_TableList", viewModel);
        }

        [HttpGet]
        public IActionResult SearchItems(int? SectionId, string searchTerm, int page = 1, int pageSize = 5)
        {
            return GetTables(SectionId, page, pageSize, searchTerm);
        }

        [HttpGet]
        public IActionResult addTableModal(int? id)
        {
            var viewModel = _tableSectionService.GetAddTableModalViewModel(id);
            return PartialView("_AddTableModal", viewModel);
        }

        public IActionResult AddTable(TableViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["SomethingIsMissing"] = "Something Went Wrong";
                return Json(false);
            }

            var result = _tableSectionService.AddTable(model);

            if (result)
            {
                TempData["TableAdd"] = "New Table is Added";
                return Json(true);
            }

            TempData["TableExist"] = "Table Already Exists";
            return Json(false);
        }

        public IActionResult EditTableModal(int id)
        {
            var viewModel = _tableSectionService.GetEditTableModalViewModel(id);

            if (viewModel == null)
            {
                TempData["SomethingIsMissing"] = "Something Went Wrong";
                return RedirectToAction("TableAndSection", "TableSection");
            }

            return PartialView("_EditTableModal", viewModel);
        }

        [HttpPost]
        public IActionResult EditTable(TableViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["SomethingIsMissing"] = "Something Went Wrong";
                return RedirectToAction("TableAndSection", "TableSection");
            }

            int validator = _tableSectionService.EditTable(model);

            if (validator == 1)
            {
                TempData["SomethingIsMissing"] = "Something Went Wrong";
                return RedirectToAction("TableAndSection", "TableSection");
            }
            if (validator == 2)
            {
                TempData["TableExist"] = "Table is Alreday Exist";
                return Json(false);
            }
            else
            {
                TempData["TableEdit"] = "Table is Updated";
                return Json(true);
            }
        }

        public IActionResult DeleteTableModal(int id)
        {
            var table = _tableSectionService.GetTableForDelete(id);

            if (table == null)
            {
                TempData["SomethingIsMissing"] = "Table not found.";
                return RedirectToAction("TableAndSection", "TableSection");
            }

            var viewModel = new TableViewModel
            {
                Id = table.Id,
                SectionId = table.SectionId,
            };

            return PartialView("_DeleteTableModal", viewModel);
        }

        [HttpPost]
        public IActionResult DeleteTable(int id)
        {
            bool isSuccess = _tableSectionService.DeleteTable(id);

            if (isSuccess)
            {
                TempData["TableIsDeleted"] = "Table Deleted Successfully";
                return Json(true);
            }
            else
            {
                TempData["SomethingIsMissing"] = "Something Went Wrong";
                return Json(false);
            }
        }
    }
}
