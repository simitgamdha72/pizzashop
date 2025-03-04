// using System.Diagnostics;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using pizzashop.Models.Models;
// using pizzashop.Models.ViewModels;

// namespace pizzashop.Controllers;

// public class RoleandPermissionController : Controller
// {

//     private readonly PizzashopContext _context;


//     public RoleandPermissionController(PizzashopContext context)
//     {
//         _context = context;
//     }

//     [HttpGet]
//     public IActionResult role()
//     {
//         return View();
//     }

//     [HttpGet]
//     public async Task<IActionResult> permission(int id)
//     {
//         var role = await _context.Roles.Include(r => r.Roleswisepermissions)
//                                         .ThenInclude(rp => rp.Permission)
//                                         .FirstOrDefaultAsync(r => r.Id == id);

//         if (role == null)
//         {
//             return NotFound();
//         }

//         var permissions = await _context.Permissions.ToListAsync();

//         // Mapping the permissions to your view model for easier handling in the view
//         var viewModel = new RolePermissionsViewModel
//         {
//             RoleId = role.Id,
//             RoleName = role.Role1,
//             Permissions = permissions.Select(p => new PermissionViewModel
//             {
//                 PermissionId = p.Id,
//                 PermissionName = p.Permission1,
//                 CanView = role.Roleswisepermissions.Any(rp => rp.PermissionId == p.Id && rp.CanView),
//                 CanAddEdit = role.Roleswisepermissions.Any(rp => rp.PermissionId == p.Id && rp.CanAddEdit),
//                 CanDelete = role.Roleswisepermissions.Any(rp => rp.PermissionId == p.Id && rp.CanDelete)
//             }).ToList()
//         };

//         return View(viewModel);
//     }

//     [HttpPost]
//     [ValidateAntiForgeryToken]
//     public async Task<IActionResult> SavePermissions(RolePermissionsViewModel model)
//     {
//         // if (ModelState.IsValid)
//         // {
//         foreach (var perm in model.Permissions)
//         {
//             var rolewisePermission = await _context.Roleswisepermissions
//                 .FirstOrDefaultAsync(rp => rp.RoleId == model.RoleId && rp.PermissionId == perm.PermissionId);

//             if (rolewisePermission == null)
//             {
//                 // Add new permission if not found
//                 _context.Roleswisepermissions.Add(new Roleswisepermission
//                 {
//                     RoleId = model.RoleId,
//                     PermissionId = perm.PermissionId,
//                     CanView = perm.CanView,
//                     CanAddEdit = perm.CanAddEdit,
//                     CanDelete = perm.CanDelete
//                 });
//             }
//             else
//             {
//                 // Update existing permissions
//                 rolewisePermission.CanView = perm.CanView;
//                 rolewisePermission.CanAddEdit = perm.CanAddEdit;
//                 rolewisePermission.CanDelete = perm.CanDelete;
//             }
//         }

//         await _context.SaveChangesAsync();
//         return RedirectToAction("Userlist", "Account"); // Or redirect wherever appropriate
//         // }

//         return View("Permission", model); // Or redirect if there's a validation error
//     }

// }

using Microsoft.AspNetCore.Mvc;
using pizzashop.Models.ViewModels;
using pizzashop.service;
using System.Threading.Tasks;

namespace pizzashop.Controllers
{
    public class RoleandPermissionController : Controller
    {
        private readonly iroleservice _roleService;

        public RoleandPermissionController(iroleservice roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public IActionResult Role()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Permission(int id)
        {
            var viewModel = await _roleService.GetRolePermissionsAsync(id);
            if (viewModel == null)
            {
                return NotFound();
            }
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SavePermissions(RolePermissionsViewModel model)
        {

            await _roleService.SaveRolePermissionsAsync(model);
            return RedirectToAction("Userlist", "Account");


            return View("Permission", model); // Or redirect if there's a validation error
        }
    }
}
