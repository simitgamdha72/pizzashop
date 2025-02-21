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
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;




namespace pizzashop.Controllers;


public class AccountController : Controller
{


    private readonly PizzashopContext _context;

       private const int PageSize = 10;
    public readonly EmailSender1 es;
    //
    private readonly IConfiguration _configuration;





    public AccountController(PizzashopContext context, EmailSender1 ess1, IConfiguration configuration)
    {


        _context = context;
        es = ess1;
        _configuration = configuration;

    }

    [HttpGet]

    public IActionResult Index()
    {





        var cookie = Request.Cookies["cookie"];
        User? user = _context.Users.FirstOrDefault(u => u.Email == cookie);
        if (user == null)
        {
            return View();
        }



        return RedirectToAction("userlist", "Account");

    }


    private string GenerateToken(string email)
    {
        var claims = new List<Claim>
        {

            new (ClaimTypes.Email, email),


        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("hey1234567890ojykjrkr6uluyk"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["JwtConfig:Issuer"],
            audience: _configuration["JwtConfig:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(30),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
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



        var tokenString = GenerateToken(user.Email);
        var cookieOptions = new CookieOptions
        {
            Secure = true,
            HttpOnly = true,
            SameSite = SameSiteMode.Strict
        };


        Response.Cookies.Append("AuthToken", tokenString, new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddDays(7),
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict
        });

        Response.Cookies.Append("cookie", user.Email, new CookieOptions
        {
            Expires = loginViewModel.RememberMe ? DateTime.UtcNow.AddDays(30) : DateTime.UtcNow.AddHours(1)
        });
        return RedirectToAction("userlist", "Account");



    }

    [HttpGet]
    public async Task<IActionResult> UserProfile()
    {
        var cookie = Request.Cookies["cookie"];
        User? user = _context.Users.FirstOrDefault(u => u.Email == cookie);
        if (user == null)
        {
            return NotFound();
        }
        var viewModel = new UserProfileViewModel
        {



            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            UserName = user.UserName,
            Phone = user.Phone,
            Country = user.Country,
            State = user.State,
            City = user.City,
            Address = user.Address,
            Zipcode = user.Zipcode,



        };

        return View(viewModel);

    }

    [HttpPost]
    public IActionResult UserProfile(UserProfileViewModel model)
    {
        var cookie = Request.Cookies["cookie"];
        User? user = _context.Users.FirstOrDefault(u => u.Email == cookie);



        if (user == null)
        {
            return NotFound();
        }

        // user.Email = model.Email;
        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        user.UserName = model.UserName;
        user.Phone = model.Phone;
        user.Address = model.Address;
        user.Zipcode = model.Zipcode;

        _context.Users.Update(user);
        _context.SaveChanges();

        return RedirectToAction("UserProfile", "Account");


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

        Response.Cookies.Append("cookie2", m.Email);


        return RedirectToAction("ForgotPasswordConfirmation", "Account");
    }

    [HttpGet]
    public IActionResult resetpassword()
    {
        //     var cookie = Request.Cookies["cookie2"];
        //  User? user = _context.Users.FirstOrDefault(u => u.Email == cookie);
        //  if (user == null){
        //      return View();
        //  }
        return View();
    }


    [HttpPost]
    public IActionResult ResetPassword(ResetPasswordViewModel m)
    {


        var cookie = Request.Cookies["cookie2"];
        User? user = _context.Users.FirstOrDefault(u => u.Email == cookie);

        // User v = (from c in _context.Users
        //           where c.Email == m.Email
        //           select c).FirstOrDefault();

        if (user != null)
        {
            user.Password = m.Password;

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
    public IActionResult ForgotPasswordConfirmation()
    {
        return View();
    }





    // [Authorize]
    // public IActionResult userlist()
    // {


    //     return View();

    // }


    [Authorize]
    public IActionResult menu()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        // 1. Delete the authentication token cookie (if you're using a token like JWT)
        Response.Cookies.Delete("AuthToken");
        Response.Cookies.Delete("cookie");
        // 2. Sign out the user using cookie authentication (ASP.NET Core Identity or cookie authentication)
        HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        // 3. Redirect the user to the login page after logout
        return RedirectToAction("Index", "Account");
    }
    [HttpGet]
    [Authorize]
    public IActionResult changepassword()
    {
        return View();
    }
    [HttpPost]
    public IActionResult changepassword(ChangePasswordViewModel model){
         var cookie = Request.Cookies["cookie"];
        User? user = _context.Users.FirstOrDefault(u => u.Email == cookie);

        if(user.Password == model.Password){
            user.Password = model.newPassword;
             _context.Users.Update(user);
        _context.SaveChanges();
         Response.Cookies.Delete("AuthToken");
        Response.Cookies.Delete("cookie");
        return RedirectToAction("Index", "Account");

        }
      else{
        return RedirectToAction("changepassword", "Account");
      }
    }




    public async Task<IActionResult> userlist(int page = 1)
    {
        var totalUsers = await _context.Users.CountAsync();
        var users = await _context.Users
            .Skip((page - 1) * PageSize) // Skip users for the current page
            .Take(PageSize) // Get the users for the current page
            .ToListAsync();

        var model = new UserListViewModel
        {
            Users = users,
            
            CurrentPage = page,
            TotalPages = (int)Math.Ceiling((double)totalUsers / PageSize)
        };

        return View(model);
    }


}

