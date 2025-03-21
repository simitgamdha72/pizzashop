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
                Description = modifiersGroup.Description ?? "",

            };

            return PartialView("_EditModifierGroupModal", viewModel);

        }

        // [HttpPost]
        // public IActionResult EditModifierGroup(ModifiersViewModel model)
        // {
        //     if (!ModelState.IsValid)
        //     {
        //         TempData["SomethingIsMissing"] = "Something Went Wrong";
        //         return RedirectToAction("Modifiers", "Midifiers");
        //     }

        //     ModifiersGroup? modifiersGroup = _context.ModifiersGroups.FirstOrDefault(x => x.Id == model.Id);
        //     var validator = _context.ModifiersGroups.FirstOrDefault(x => x.Name == model.Name);

        //     if (modifiersGroup == null)
        //     {
        //         TempData["SomethingIsMissing"] = "Something Went Wrong";
        //         return RedirectToAction("Modifiers", "Midifiers");
        //     }

        //     if (validator != null)
        //     {
        //         if (modifiersGroup.Id != validator.Id)
        //         {
        //             TempData["ModifiersGroupExist"] = "ModifiersGroup is Alreday Exist";
        //             return Json(false);
        //         }
        //     }

        //     modifiersGroup.Name = model.Name;
        //     modifiersGroup.Description = model.Description;

        //     _context.ModifiersGroups.Update(modifiersGroup);
        //     _context.SaveChanges();

        //     TempData["ModifiersGroupEdit"] = "ModifiersGroup is Updated";
        //     return Json(true);
        // }

        [HttpPost]
        public IActionResult EditModifierGroup(ModifiersViewModel model, string remainingModifierIds, string modifiersToRemove)
        {
            // if (!ModelState.IsValid)
            // {
            //     TempData["SomethingIsMissing"] = "Something Went Wrong";
            //     return RedirectToAction("Modifiers", "Modifiers");
            // }

            ModifiersGroup? modifiersGroup = _context.ModifiersGroups.FirstOrDefault(x => x.Id == model.Id);
            var validator = _context.ModifiersGroups.FirstOrDefault(x => x.Name == model.Name);

            if (modifiersGroup == null)
            {
                TempData["SomethingIsMissing"] = "Something Went Wrong";
                return RedirectToAction("Modifiers", "Modifiers");
            }

            if (validator != null)
            {
                if (modifiersGroup.Id != validator.Id)
                {
                    TempData["ModifiersGroupExist"] = "ModifiersGroup is Already Exist";
                    return Json(false);
                }
            }

            // Update the basic info
            modifiersGroup.Name = model.Name;
            modifiersGroup.Description = model.Description;

            _context.ModifiersGroups.Update(modifiersGroup);

            // Process modifier changes
            List<int> remainingIds = new List<int>();
            List<int> idsToRemove = new List<int>();

            // Parse the remaining IDs from JSON
            if (!string.IsNullOrEmpty(remainingModifierIds))
            {
                remainingIds = System.Text.Json.JsonSerializer.Deserialize<List<int>>(remainingModifierIds);
            }

            // Parse the IDs to remove from JSON
            if (!string.IsNullOrEmpty(modifiersToRemove))
            {
                idsToRemove = System.Text.Json.JsonSerializer.Deserialize<List<int>>(modifiersToRemove);
            }

            // Mark as deleted any modifiers that were removed
            if (idsToRemove.Count > 0)
            {
                var modifiersToDelete = _context.MenuModifiers
                    .Where(m => idsToRemove.Contains(m.Id) && m.ModifierGroupId == model.Id)
                    .ToList();

                foreach (var modifier in modifiersToDelete)
                {
                    modifier.Isdeleted = true;
                    _context.MenuModifiers.Update(modifier);
                }
            }

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

            if (modifiersgroup == null)
            {
                TempData["SomethingIsMissing"] = "Something Went Wrong";
                return RedirectToAction("Modifiers", "Midifiers");
            }

            modifiersgroup.Isdeleted = true;
            _context.ModifiersGroups.Update(modifiersgroup);
            _context.SaveChanges();

            TempData["ModifiersGroupIsDeleted"] = "ModifiersGroup Deleted Successfully";
            return Json(true);
        }

        public IActionResult GetExistingModifiers(int id)
        {
            var ExistModifiers = _context.MenuModifiers.Where(x => x.ModifierGroupId == id && x.Isdeleted != true).ToList();

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

        public IActionResult GetModifiers(int? id, int page = 1, int pageSize = 5, string searchTerm = "")
        {
            var ModifierGroup = _context.ModifiersGroups.Where(c => c.Isdeleted != true).ToList().First();

            int? mgid = ModifierGroup.Id;

            var query = _context.MenuModifiers.Where(x => x.Isdeleted != true);

            if (id.HasValue)
            {
                query = query.Where(m => m.ModifierGroupId == id);
            }
            else
            {
                id = mgid;
                query = query.Where(m => m.ModifierGroupId == id);
            }
            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                query = query.Where(m => m.Name.ToLower().Contains(searchTerm));
            }

            var totalItems = query.Count();

            var menumodifiers = query
            .OrderBy(m => m.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

            var viewModel = new PaginatedModifiersViewModel
            {
                Modifiers = menumodifiers,
                CurrentPage = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                SearchTerm = searchTerm,
                ModifierGroupId = id,
            };

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
                TempData["ModifiersExist"] = "Modifier is Alreday Exist";
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
            return Json(true);
        }

        public IActionResult EditModifierModal(int id)
        {
            var menumodifier = _context.MenuModifiers.FirstOrDefault(x => x.Id == id);
            var modifiersGroup = _context.ModifiersGroups.ToList();
            var units = _context.Units.ToList();

            if (menumodifier == null)
            {
                TempData["SomethingIsMissing"] = "Something Went Wrong";
                return RedirectToAction("Modifiers", "Modifiers");
            }

            var viewModel = new MenuModifiersViewModel
            {
                Id = id,
                ModifierGroupId = menumodifier.ModifierGroupId,
                Name = menumodifier.Name,
                Rate = menumodifier.Rate,
                Quantity = menumodifier.Quantity,
                UnitId = menumodifier.UnitId,
                Description = menumodifier.Description ?? "",
                modifiersGroups = modifiersGroup,
                units = units,
            };

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

            MenuModifier? menuModifier = _context.MenuModifiers.FirstOrDefault(x => x.Id == model.Id);
            var validator = _context.MenuModifiers.FirstOrDefault(x => x.Name == model.Name && x.ModifierGroupId == model.ModifierGroupId);

            if (menuModifier == null)
            {
                TempData["SomethingIsMissing"] = "Something Went Wrong";
                return RedirectToAction("Modifiers", "Midifiers");
            }

            if (validator != null)
            {
                if (menuModifier.Id != validator.Id)
                {
                    TempData["ModifiersExist"] = "Modifier is Alreday Exist";
                    return Json(false);
                }
            }

            menuModifier.Name = model.Name;
            menuModifier.ModifierGroupId = model.ModifierGroupId;
            menuModifier.Rate = model.Rate;
            menuModifier.Quantity = model.Quantity;
            menuModifier.UnitId = model.UnitId;
            menuModifier.Description = model.Description;

            _context.MenuModifiers.Update(menuModifier);
            _context.SaveChanges();

            TempData["ModifierEdit"] = "Modifier is Updated";
            return Json(true);
        }

        public IActionResult DeleteModifierModal(int id)
        {
            var modifier = _context.MenuModifiers.FirstOrDefault(x => x.Id == id);
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
            var modifiers = _context.MenuModifiers.FirstOrDefault(x => x.Id == id);

            if (modifiers == null)
            {
                TempData["SomethingIsMissing"] = "Something Went Wrong";
                return RedirectToAction("Modifiers", "Midifiers");
            }

            modifiers.Isdeleted = true;
            _context.MenuModifiers.Update(modifiers);
            _context.SaveChanges();

            TempData["ModifierIsDeleted"] = "Modifier Deleted Successfully";
            return Json(true);
        }











        public IActionResult GetExistingModifiersTable(string searchTerm = "", int page = 1, int pageSize = 5)
        {
            var query = _context.MenuModifiers.Where(x => x.Isdeleted != true);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                query = query.Where(m => m.Name.ToLower().Contains(searchTerm));
            }

            var totalItems = query.Count();

            var menumodifiers = query
            .OrderBy(m => m.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

            var viewModel = new PaginatedExistingModifiersViewModel
            {
                Modifiers = menumodifiers,
                CurrentPage = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                SearchTerm = searchTerm,
            };

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







        // Add this method to your ModifiersController.cs file

        [HttpPost]
        public IActionResult RemoveModifierFromGroup(int modifierId, int modifierGroupId)
        {
            try
            {
                var modifier = _context.MenuModifiers.FirstOrDefault(m => m.Id == modifierId && m.ModifierGroupId == modifierGroupId);

                if (modifier != null)
                {
                    modifier.Isdeleted = true;
                    _context.MenuModifiers.Update(modifier);
                    _context.SaveChanges();

                    return Json(new { success = true });
                }

                return Json(new { success = false, message = "Modifier not found" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // Update the AddExistingModifiersToGroup method to check for duplicates
        [HttpPost]
        public IActionResult AddExistingModifiersToGroup(List<int> modifiersIds, int modifierGroupId)
        {
            if (modifiersIds == null || modifiersIds.Count == 0)
            {
                return Json(new { success = false, message = "No modifiers selected" });
            }

            try
            {
                // Get existing modifiers in the group to check for duplicates
                var existingModifiersInGroup = _context.MenuModifiers
                    .Where(m => m.ModifierGroupId == modifierGroupId && m.Isdeleted != true)
                    .Select(m => m.Name)
                    .ToList();

                // Get the modifiers that need to be added
                var modifiersToAdd = _context.MenuModifiers
                    .Where(m => modifiersIds.Contains(m.Id))
                    .ToList();

                // Filter out duplicates based on name
                var uniqueModifiers = modifiersToAdd
                    .Where(m => !existingModifiersInGroup.Contains(m.Name))
                    .ToList();

                if (uniqueModifiers.Count == 0)
                {
                    return Json(new { success = false, message = "All selected modifiers already exist in this group" });
                }

                // Add unique modifiers to the group
                foreach (var modifier in uniqueModifiers)
                {
                    // Create a copy of the modifier for the new group
                    var newModifier = new MenuModifier
                    {
                        ModifierGroupId = modifierGroupId,
                        Name = modifier.Name,
                        Rate = modifier.Rate,
                        Quantity = modifier.Quantity,
                        UnitId = modifier.UnitId,
                        Description = modifier.Description
                    };

                    _context.MenuModifiers.Add(newModifier);
                }

                _context.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }















    }
}
