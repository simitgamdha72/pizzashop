 using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;    
using Microsoft.AspNetCore.Mvc;
using pizzashop.Models;
using pizzashop.ViewModels;



namespace pizzashop.Controllers;

public class AccountController : Controller
{

    private readonly PizzashopContext _context;
   

    public AccountController(PizzashopContext context)
    {
        _context = context;
    }

     [HttpGet]
    public IActionResult Index()
    {

        return View();
    }

   [HttpPost]

    public IActionResult Index(LoginViewModel loginViewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(loginViewModel);
        }

        User? user = _context.Users.FirstOrDefault(u => u.Email == loginViewModel.Email);

        if (user == null)
        {
            TempData["error"] = "User Not Found";
            return View(loginViewModel);
        }

        if (user.Password != loginViewModel.Password)
        {
            TempData["error"] = "Password is Incorrect";
            return View(loginViewModel);
        }












        return RedirectToAction ("Index", "Home");
    }

    [HttpGet]
    public IActionResult forgotpassword()
    {
        return View();
    }
}

