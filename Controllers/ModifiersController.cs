using Microsoft.AspNetCore.Mvc;
using pizzashop.Models.Models;
using pizzashop.Models.ViewModels;

namespace pizzashop.Controllers
{
    public class ModifiersController : Controller
    {

        private readonly PizzashopContext _context;

        public ModifiersController(PizzashopContext context)
        {
            _context = context;
        }

        public IActionResult Modifiers()
        {
            return View();
        }

        public IActionResult GetModifiersGroup()
        {
            var modifiers = _context.ModifiersGroups.Where(x => x.Isdeleted != true)
            .ToList();

            var viewModel = new ModifiersViewModel
            {
                modifiersGroups = modifiers,
            };
            return PartialView("_ModifierSidebar", viewModel);
        }

        [HttpGet]
        public IActionResult addModifierModalGroup()
        {
            return PartialView("_AddModifierModalGroup");
        }

        public IActionResult AddModifierGroup(ModifiersViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["SomethingIsMissing"] = "Something Went Wrong";
                return Json(false);
            }

            var validator = _context.ModifiersGroups.FirstOrDefault(x => x.Name == model.Name);
            if (validator != null)
            {
                TempData["ModifiersGroupExist"] = "ModifiersGroup is Alreday Exist";
                return Json(false);
            }


            var viewModel = new ModifiersGroup
            {
                Name = model.Name,
                Description = model.Description,
            };

            _context.ModifiersGroups.Add(viewModel);
            _context.SaveChanges();

            TempData["ModifiersGroupAdd"] = "New ModifiersGroup is Added ";
            return Json(new { success = true });
        }

        public IActionResult EditModifierGroupModal(int id)
        {
            ModifiersGroup? modifiersGroup = _context.ModifiersGroups.FirstOrDefault(x => x.Id == id);

            if (modifiersGroup == null)
            {
                TempData["SomethingIsMissing"] = "Something Went Wrong";
                return RedirectToAction("Modifiers", "Midifiers");
            }

            // var modifiersItem = _context.MenuModifiers.Where(x => x.ModifierGroupId == id)
            // .ToList();

            // if (modifiersItem != null)
            // {
            //     var viewModel = new ModifiersViewModel
            //     {
            //         Id = modifiersGroup.Id,
            //         Name = modifiersGroup.Name,
            //         Description = modifiersGroup.Description,
            //         menuModifiers = modifiersItem,
            //     };
            //     return PartialView("_EditModifierModal", viewModel);
            // }
            // else
            // {
            //     var viewModel = new ModifiersViewModel
            //     {
            //         Id = modifiersGroup.Id,
            //         Name = modifiersGroup.Name,
            //         Description = modifiersGroup.Description,
            //         menuModifiers = modifiersItem,
            //     };

            var viewModel = new ModifiersViewModel
            {
                Id = modifiersGroup.Id,
                Name = modifiersGroup.Name,
                Description = modifiersGroup.Description,

            };

            return PartialView("_EditModifierGroupModal", viewModel);

        }

        [HttpPost]
        public IActionResult EditModifierGroup(ModifiersViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["SomethingIsMissing"] = "Something Went Wrong";
                return RedirectToAction("Modifiers", "Midifiers");
            }

            ModifiersGroup? modifiersGroup = _context.ModifiersGroups.FirstOrDefault(x => x.Id == model.Id);
            var validator = _context.ModifiersGroups.FirstOrDefault(x => x.Name == model.Name);

            if (validator != null)
            {
                if (modifiersGroup.Id != validator.Id)
                {
                    TempData["ModifiersGroupExist"] = "ModifiersGroup is Alreday Exist";
                    return Json(false);
                }
            }

            modifiersGroup.Name = model.Name;
            modifiersGroup.Description = model.Description;

            _context.ModifiersGroups.Update(modifiersGroup);
            _context.SaveChanges();

            TempData["ModifiersGroupEdit"] = "ModifiersGroup is Updated";
            return Json(true);
        }


        public IActionResult DeleteModifierGroupModal(int id)
        {
            var modifiersGroup = _context.ModifiersGroups.FirstOrDefault(x => x.Id == id);
            var viewModel = new ModifiersViewModel
            {
                Id = id,
            };

            return PartialView("_DeleteModifierGroupModal", viewModel);
        }

        [HttpPost]
        public IActionResult DeleteModifiersGroup(int id)
        {
            var modifiersgroup = _context.ModifiersGroups.FirstOrDefault(x => x.Id == id);
            modifiersgroup.Isdeleted = true;
            _context.ModifiersGroups.Update(modifiersgroup);
            _context.SaveChanges();

            TempData["ModifiersGroupIsDeleted"] = "ModifiersGroup Deleted Successfully";
            return Json(true);
        }

        public IActionResult GetExistingModifiers(int id)
        {
            var ExistModifiers = _context.MenuModifiers.Where(x => x.ModifierGroupId == id).ToList();

            if (ExistModifiers != null)
            {
                var viewModel = new ModifiersViewModel
                {
                    menuModifiers = ExistModifiers,
                };
                return PartialView("_ExistingModifiers", viewModel);

            }
            else
            {
                return Json(false);
            }

        }

        public IActionResult GetModifiers(int id)
        {
            var ModifiersList = _context.MenuModifiers.Where(x => x.ModifierGroupId == id && x.Isdeleted != true).ToList();

            var viewModel = new PaginatedModifiersViewModel
            {
                Modifiers = ModifiersList,
            };

            return PartialView("_ModifiersList", viewModel);
        }

        [HttpGet]
        public IActionResult addModifierModal()
        {
            var modifiersGroup = _context.ModifiersGroups.ToList();
            var units = _context.Units.ToList();

            var model = new MenuModifiersViewModel
            {
                modifiersGroups = modifiersGroup,
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

            var validator = _context.MenuModifiers.FirstOrDefault(x => x.Name == model.Name && x.ModifierGroupId == model.ModifierGroupId);
            if (validator != null)
            {
                TempData["ModifierExist"] = "Modifier is Alreday Exist";
                return Json(false);
            }


            var viewModel = new MenuModifier
            {
                ModifierGroupId = model.ModifierGroupId,
                Name = model.Name,
                Rate = model.Rate,
                Quantity = model.Quantity,
                UnitId = model.UnitId,
                Description = model.Description,
            };

            _context.MenuModifiers.Add(viewModel);
            _context.SaveChanges();

            TempData["ModifierAdd"] = "New Modifier is Added ";
            return Json(new { success = true });
        }






    }
}
