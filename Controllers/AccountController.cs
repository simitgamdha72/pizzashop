using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using pizzashop.Models;
using pizzashop.ViewModels;
using Utility;
using pizzashop.Models;
using Microsoft.AspNetCore.Authorization;



namespace pizzashop.Controllers;

public class AccountController : Controller
{

    private readonly PizzashopContext _context;
    public readonly EmailSender1 es;


    public AccountController(PizzashopContext context, EmailSender1 ess1)
    {
        _context = context;
        es = ess1;
       
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

        return RedirectToAction("userlist", "Account");
    }



    [HttpGet]
    public IActionResult forgotpassword()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SendResetLink(ForgotPasswordViewModel m)
    {   

        


       



        string subject = "reset password";
        Extrathings object1 = new();
        string object2 = object1.getEmail();
        // string object1 ="";
        // Console.WriteLine(m.Email);
        await es.SendEmailAsync(m.Email, subject, object2);

        return RedirectToAction("ForgotPasswordConfirmation", "Account");
    }

    [HttpGet]
    public IActionResult resetpassword()
    {
        return View();
    }


    [HttpPost]
    public IActionResult ResetPassword(ResetPasswordViewModel m) 
    {
        User v = (from c in _context.Users
                                    where c.Email == m.Email
                                    select c).FirstOrDefault();
 
        if (v != null)
        {
            v.Password = m.Password;
         
            _context.SaveChanges();
            ViewBag.Message = "Customer record updated.";
        }
        else
        {
            ViewBag.Message = "Customer not found.";
        }
 
        return RedirectToAction("ResetPasswordConfirmation", "Account");
    }


    

    [HttpGet]
    public IActionResult ResetPasswordConfirmation()
    {
        return View();
    }


    public IActionResult userlist()
    {
        return View();
    }
    public IActionResult menu()
    {
        return View();
    }
}

