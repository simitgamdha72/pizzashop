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
            var modifiers = _context.ModifiersGroups.ToList();

            var viewModel = new ModifiersViewModel
            {
                modifiersGroups = modifiers,
            };
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult addModifierModal()
        {
            return PartialView("_AddModifierModal");
        }

        // [HttpPost]
        // public IActionResult addModifier(ModifiersViewModel model)
        // {
        //     var viewModel = new ModifiersGroup
        //     {
        //         Name = model.Name,
        //         Description = model.Description,
        //     };



        //     _context.ModifiersGroups.Add(viewModel);
        //     _context.SaveChanges();

        //     return RedirectToAction("Modifiers", "Modifiers");


        // }

        public IActionResult GetModifiers()
        {
            var modifiers = _context.ModifiersGroups.ToList();

            var viewModel = new ModifiersViewModel
            {
                modifiersGroups = modifiers,
            };
            return PartialView("_ModifierSidebar", viewModel);
        }

        public IActionResult AddModifier(ModifiersViewModel model)
        {
            var viewModel = new ModifiersGroup
            {
                Name = model.Name,
                Description = model.Description,
            };



            _context.ModifiersGroups.Add(viewModel);
            _context.SaveChanges();


            return Json(new { success = true });
        }
    }
}
