using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pizzashop.Models.Models;
using pizzashop.Models.ViewModels;

namespace pizzashop.Controllers;

public class RoleandPermissionController : Controller
{

    private readonly PizzashopContext _context;
    private readonly iroleandpermissionservice _roleandpermissionservice;


    public RoleandPermissionController(PizzashopContext context, iroleandpermissionservice roleandpermissionservice)
    {
        _context = context;
        _roleandpermissionservice = roleandpermissionservice;

    }

    [HttpGet]
    public IActionResult role()
    {
        return View();
    }

    [HttpGet]
    public IActionResult permission(int id)
    {
        var model = _roleandpermissionservice.GetPermission(id);

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult SavePermissions(RolePermissionsViewModel model)
    {
        // foreach (var perm in model.Permissions)
        // {
        //     var rolewisePermission = await _context.Roleswisepermissions
        //         .FirstOrDefaultAsync(rp => rp.RoleId == model.RoleId && rp.PermissionId == perm.PermissionId);

        //     if (rolewisePermission == null)
        //     {
        //         // Add new permission if not found
        //         _context.Roleswisepermissions.Add(new Roleswisepermission
        //         {
        //             RoleId = model.RoleId,
        //             PermissionId = perm.PermissionId,
        //             CanView = perm.CanView,
        //             CanAddEdit = perm.CanAddEdit,
        //             CanDelete = perm.CanDelete
        //         });
        //     }
        //     else
        //     {
        //         // Update existing permissions
        //         rolewisePermission.CanView = perm.CanView;
        //         rolewisePermission.CanAddEdit = perm.CanAddEdit;
        //         rolewisePermission.CanDelete = perm.CanDelete;
        //     }
        // }

        // await _context.SaveChangesAsync();

        _roleandpermissionservice.SavePermissions(model);
        return RedirectToAction("Userlist", "Account"); // Or redirect wherever appropriate

    }

}
