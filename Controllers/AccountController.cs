    using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using pizzashop.Models;

namespace pizzashop.Controllers;

public class AccountController : Controller
{

    public IActionResult Login()
    {
        return View();
    }
}
