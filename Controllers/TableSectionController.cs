using Microsoft.AspNetCore.Mvc;
using pizzashop.Models.Models;
using pizzashop.Models.ViewModels;

namespace pizzashop.Controllers
{
    public class TableSectionController : Controller
    {

        private readonly PizzashopContext _context;

        public TableSectionController(PizzashopContext context)
        {
            _context = context;
        }

        public IActionResult TableAndSection()
        {
            return View();
        }

        public IActionResult GetSection()
        {
            var sections = _context.Sections.Where(x => x.Isdeleted != true)
            .ToList();

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

            var validator = _context.Sections.FirstOrDefault(x => x.Name == model.Name);
            if (validator != null)
            {
                TempData["SectionExist"] = "Section is Alreday Exist";
                return Json(false);
            }


            var viewModel = new Section
            {
                Name = model.Name,
                Description = model.Description,
            };

            _context.Sections.Add(viewModel);
            _context.SaveChanges();

            TempData["SectionAdd"] = "New Section is Added ";
            return Json(new { success = true });
        }


        public IActionResult EditSectionModal(int id)
        {
            Section? sections = _context.Sections.FirstOrDefault(x => x.Id == id);

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

            Section? sections = _context.Sections.FirstOrDefault(x => x.Id == model.Id);
            var validator = _context.Sections.FirstOrDefault(x => x.Name == model.Name);

            if (sections == null)
            {
                TempData["SomethingIsMissing"] = "Something Went Wrong";
                return RedirectToAction("TableAndSection", "TableAndSection");
            }

            if (validator != null)
            {
                if (sections.Id != validator.Id)
                {
                    TempData["SectionExist"] = "Section is Alreday Exist";
                    return Json(false);
                }
            }

            sections.Name = model.Name;
            sections.Description = model.Description;

            _context.Sections.Update(sections);
            _context.SaveChanges();

            TempData["SectionEdit"] = "Section is Updated";
            return Json(true);
        }

        public IActionResult DeleteSectionModal(int id)
        {
            var section = _context.Sections.FirstOrDefault(x => x.Id == id);
            var viewModel = new SectionViewModal
            {
                Id = id,
            };

            return PartialView("_DeleteSectionModal", viewModel);
        }

        [HttpPost]
        public IActionResult DeleteSection(int id)
        {
            var sections = _context.Sections.FirstOrDefault(x => x.Id == id);

            if (sections == null)
            {
                TempData["SomethingIsMissing"] = "Something Went Wrong";
                return RedirectToAction("TableAndSection", "TableSection");
            }

            sections.Isdeleted = true;
            _context.Sections.Update(sections);
            _context.SaveChanges();

            TempData["SectionIsDeleted"] = "Section Deleted Successfully";
            return Json(true);
        }

        public IActionResult GetTables(int? id, int page = 1, int pageSize = 5, string searchTerm = "")
        {
            var Sections = _context.Sections.Where(c => c.Isdeleted != true).ToList().First();

            int? sid = Sections.Id;

            var query = _context.Tables.Where(x => x.Isdeleted != true);

            if (id.HasValue)
            {
                query = query.Where(m => m.SectionId == id);
            }
            else
            {
                id = sid;
                query = query.Where(m => m.SectionId == id);
            }
            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                query = query.Where(m => m.Name.ToLower().Contains(searchTerm));
            }

            var totalItems = query.Count();

            var tables = query
          .OrderBy(m => m.Name)
          .Skip((page - 1) * pageSize)
          .Take(pageSize)
          .ToList();

            var viewModel = new PaginatedTableViewModel
            {
                tables = tables,
                CurrentPage = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                SearchTerm = searchTerm,
                sectionId = id,
            };

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
            if (id == null)
            {
                var sectionId = _context.Sections.First();
                id = sectionId.Id;
            }
            var sectionlist = _context.Sections.Where(x => x.Isdeleted != true).ToList();
            var section = _context.Sections.FirstOrDefault(x => x.Id == id);
            var viewModel = new TableViewModel
            {
                SectionId = section.Id,
                sections = sectionlist,
            };
            return PartialView("_AddTableModal", viewModel);
        }

        public IActionResult AddTable(TableViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["SomethingIsMissing"] = "Something Went Wrong";
                return Json(false);
            }

            var validator = _context.Tables.FirstOrDefault(x => x.SectionId == model.SectionId && x.Name == model.Name);
            if (validator != null)
            {
                TempData["TableExist"] = "Table is Alreday Exist";
                return Json(false);
            }


            var viewModel = new Table
            {
                SectionId = model.SectionId,
                Name = model.Name,
                Capacity = model.Capacity,
                Status = model.Status,
            };

            _context.Tables.Add(viewModel);
            _context.SaveChanges();

            TempData["TableAdd"] = "New Table is Added ";
            return Json(true);
        }

        public IActionResult EditTableModal(int id)
        {
            var table = _context.Tables.FirstOrDefault(x => x.Id == id);
            var sections = _context.Sections.Where(x => x.Isdeleted != true)
            .ToList();

            if (table == null)
            {
                TempData["SomethingIsMissing"] = "Something Went Wrong";
                return RedirectToAction("TableAndSection", "TableSection");
            }

            var viewModel = new TableViewModel
            {
                Id = id,
                Name = table.Name,
                Status = table.Status,
                Capacity = table.Capacity,
                SectionId = table.SectionId,
                sections = sections,

            };

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

            Table? table = _context.Tables.FirstOrDefault(x => x.Id == model.Id);
            var validator = _context.Tables.FirstOrDefault(x => x.Name == model.Name && x.SectionId == model.SectionId);

            if (table == null)
            {
                TempData["SomethingIsMissing"] = "Something Went Wrong";
                return RedirectToAction("TableAndSection", "TableSection");
            }

            if (validator != null)
            {
                if (table.Id != validator.Id)
                {
                    TempData["TableExist"] = "Table is Alreday Exist";
                    return Json(false);
                }
            }

            table.Name = model.Name;
            table.SectionId = model.SectionId;
            table.Capacity = model.Capacity;
            table.Status = model.Status;

            _context.Tables.Update(table);
            _context.SaveChanges();

            TempData["TableEdit"] = "Table is Updated";
            return Json(true);
        }

        public IActionResult DeleteTableModal(int id)
        {
            var table = _context.Tables.FirstOrDefault(x => x.Id == id);
            var viewModel = new TableViewModel
            {
                Id = id,
                SectionId = table.SectionId,
            };

            return PartialView("_DeleteTableModal", viewModel);
        }


        [HttpPost]
        public IActionResult DeleteTable(int id)
        {
            var table = _context.Tables.FirstOrDefault(x => x.Id == id);

            if (table == null)
            {
                TempData["SomethingIsMissing"] = "Something Went Wrong";
                return RedirectToAction("TableAndSection", "TableSection");
            }

            table.Isdeleted = true;
            _context.Tables.Update(table);
            _context.SaveChanges();

            TempData["TableIsDeleted"] = "Table Deleted Successfully";
            return Json(true);
        }



    }
}
