using Microsoft.AspNetCore.Mvc;
using pizzashop.Models.Models;
using pizzashop.Models.ViewModels;

namespace pizzashop.Controllers
{
    public class ModifiersController : Controller
    {
        private readonly imodifierservice _modifierservice;
        private readonly iunitservice _unitservice;

        public ModifiersController(imodifierservice modifierservice, iunitservice unitservice)
        {
            _modifierservice = modifierservice;
            _unitservice = unitservice;
        }

        public IActionResult Modifiers()
        {
            return View();
        }

        public IActionResult GetModifiersGroup()
        {
            var modifiersGroup = _modifierservice.GetModifiersGroups();

            if (modifiersGroup == null || !modifiersGroup.Any())
            {
                TempData["ErrorMessage"] = "No modifiers found.";
                return PartialView("_ModifierSidebar", new ModifiersViewModel());
            }

            var viewModel = new ModifiersViewModel
            {
                modifiersGroups = modifiersGroup,
            };

            return PartialView("_ModifierSidebar", viewModel);
        }

        [HttpGet]
        public IActionResult addModifierModalGroup()
        {
            return PartialView("_AddModifierModalGroup");
        }

        [HttpPost]
        public IActionResult AddModifierGroup(ModifiersViewModel model, string modifierIds)
        {
            if (!ModelState.IsValid)
            {
                TempData["SomethingIsMissing"] = "Something Went Wrong";
                return Json(false);
            }

            var result = _modifierservice.AddModifierGroup(model, modifierIds);

            if (!result)
            {
                TempData["ModifiersGroupExist"] = "ModifiersGroup is Already Exist";
                return Json(false);
            }

            TempData["ModifiersGroupAdd"] = "New ModifiersGroup is Added ";
            return Json(new { success = true });
        }

        public IActionResult EditModifierGroupModal(int id)
        {
            var modifiersGroup = _modifierservice.GetModifierGroupById(id);

            if (modifiersGroup == null)
            {
                TempData["SomethingIsMissing"] = "Something Went Wrong";
                return RedirectToAction("Modifiers", "Modifiers");
            }

            var viewModel = new ModifiersViewModel
            {
                Id = modifiersGroup.Id,
                Name = modifiersGroup.Name,
                Description = modifiersGroup.Description ?? "",
            };

            return PartialView("_EditModifierGroupModal", viewModel);
        }

        [HttpPost]
        public IActionResult EditModifierGroup(ModifiersViewModel model, string modifierIds, string modifiersToRemove)
        {

            var result = _modifierservice.EditModifierGroup(model, modifierIds, modifiersToRemove);

            if (result == 1)
            {
                TempData["SomethingIsMissing"] = "Something Went Wrong";
                return RedirectToAction("Modifiers", "Modifiers");
            }
            if (result == 2)
            {
                TempData["ModifiersGroupExist"] = "ModifiersGroup is Already Exist";
                return Json(false);
            }
            else
            {
                TempData["ModifiersGroupEdit"] = "ModifiersGroup is Updated";
                return Json(true);
            }
        }


        public IActionResult DeleteModifierGroupModal(int id)
        {
            var viewModel = new ModifiersViewModel
            {
                Id = id,
            };

            return PartialView("_DeleteModifierGroupModal", viewModel);
        }

        [HttpPost]
        public IActionResult DeleteModifiersGroup(int id)
        {
            var success = _modifierservice.DeleteModifierGroup(id);

            if (!success)
            {
                TempData["SomethingIsMissing"] = "Something Went Wrong";
                return RedirectToAction("Modifiers", "Modifiers");
            }

            TempData["ModifiersGroupIsDeleted"] = "ModifiersGroup Deleted Successfully";
            return Json(true);
        }

        [HttpGet]
        public IActionResult GetExistingModifiers(int id)
        {
            var existingModifiers = _modifierservice.GetModifiersByGroupId(id);

            if (existingModifiers != null && existingModifiers.Any())
            {
                var viewModel = new ModifiersViewModel
                {
                    menuModifiers = existingModifiers,
                };

                return PartialView("_ExistingModifiers", viewModel);
            }
            else
            {
                return Json(false);
            }
        }


        [HttpGet]
        public IActionResult GetExistingModifiersbyModifierId(int id)
        {
            var existingModifiers = _modifierservice.GetModifiersById(id);

            if (existingModifiers != null && existingModifiers.Any())
            {
                var viewModel = new ModifiersViewModel
                {
                    menuModifiers = existingModifiers,
                };

                return PartialView("_ExistingModifiers", viewModel);
            }
            else
            {
                return Json(false);
            }
        }


        [HttpGet]
        public IActionResult GetModifiers(int? id, int page = 1, int pageSize = 5, string searchTerm = "")
        {
            var viewModel = _modifierservice.GetModifiers(id, page, pageSize, searchTerm);

            return PartialView("_ModifiersList", viewModel);
        }

        [HttpGet]
        public IActionResult SearchItems(int? ModifierGroupId, string searchTerm, int page = 1, int pageSize = 5)
        {
            return GetModifiers(ModifierGroupId, page, pageSize, searchTerm);
        }

        [HttpGet]
        public IActionResult addModifierModal()
        {
            var modifiersGroups = _modifierservice.GetAllModifiersGroups();
            var units = _unitservice.GetUnits();

            var model = new MenuModifiersViewModel
            {
                modifiersGroups = modifiersGroups,
                units = units,
            };
            return PartialView("_AddModifierModal", model);
        }

        public IActionResult AddModifier(MenuModifiersViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["SomethingIsMissing"] = "Something Went Wrong";
                return Json(false);
            }

            var result = _modifierservice.AddModifier(model);

            if (result == false)
            {
                TempData["ModifiersExist"] = "Modifier is Alreday Exist";
                return Json(false);
            }
            TempData["ModifierAdd"] = "New Modifier is Added ";
            return Json(true);
        }

        public IActionResult EditModifierModal(int id)
        {
            var viewModel = _modifierservice.EditModifierModal(id);

            if (viewModel == null)
            {
                TempData["SomethingIsMissing"] = "Something Went Wrong";
                return RedirectToAction("Modifiers", "Modifiers");
            }
            return PartialView("_EditModifierModal", viewModel);
        }

        [HttpPost]
        public IActionResult EditModifier(MenuModifiersViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["SomethingIsMissing"] = "Something Went Wrong";
                return RedirectToAction("Modifiers", "Midifiers");
            }

            var v = _modifierservice.EditModifier(model);

            if (v == 1)
            {
                TempData["SomethingIsMissing"] = "Something Went Wrong";
                return RedirectToAction("Modifiers", "Midifiers");
            }
            if (v == 2)
            {
                TempData["ModifiersExist"] = "Modifier is Alreday Exist";
                return Json(false);
            }
            else
            {
                TempData["ModifierEdit"] = "Modifier is Updated";
                return Json(true);
            }
        }

        public IActionResult DeleteModifierModal(int id)
        {

            var modifier = _modifierservice.GetMenuModifierById(id);

            // var modifier = _context.MenuModifiers.FirstOrDefault(x => x.Id == id);
            if (modifier == null)
            {
                TempData["SomethingIsMissing"] = "Something Went Wrong";
                return RedirectToAction("Modifiers", "Midifiers");
            }
            var viewModel = new MenuModifiersViewModel
            {
                Id = id,
                ModifierGroupId = modifier.ModifierGroupId,
            };

            return PartialView("_DeleteModifierModal", viewModel);
        }

        [HttpPost]
        public IActionResult DeleteModifiers(int id)
        {
            var modifiers = _modifierservice.GetMenuModifierById(id);

            if (modifiers == null)
            {
                TempData["SomethingIsMissing"] = "Something Went Wrong";
                return RedirectToAction("Modifiers", "Midifiers");
            }

            _modifierservice.DeleteMenuModifier(modifiers);

            TempData["ModifierIsDeleted"] = "Modifier Deleted Successfully";
            return Json(true);
        }

        [HttpGet]
        public IActionResult GetExistingModifiersTable(string searchTerm = "", int page = 1, int pageSize = 5)
        {
            var viewModel = _modifierservice.GetPaginatedMenuModifiers(searchTerm, page, pageSize);

            return PartialView("_SelectExistingModifierTable", viewModel);
        }

        [HttpGet]
        public IActionResult SearchItemsForExistingModifier(string searchTerm, int page = 1, int pageSize = 5)
        {
            return GetExistingModifiersTable(searchTerm, page, pageSize);
        }

        public IActionResult GetExistingModifierTable()
        {
            return PartialView("_SelectExistingModifiersModal");
        }
    }
}
