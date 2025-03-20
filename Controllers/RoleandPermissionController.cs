using Microsoft.AspNetCore.Mvc;
using pizzashop.Models.ViewModels;

namespace pizzashop.Controllers;

public class RoleandPermissionController : Controller
{
    private readonly iroleandpermissionservice _roleandpermissionservice;

    public RoleandPermissionController(iroleandpermissionservice roleandpermissionservice)
    {
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
        _roleandpermissionservice.SavePermissions(model);
        return RedirectToAction("Userlist", "Account");
    }
}
