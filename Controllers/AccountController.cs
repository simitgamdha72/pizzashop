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




namespace pizzashop.Controllers;


public class AccountController : Controller
{


    private readonly PizzashopContext _context;
    public readonly EmailSender1 es;
    //
     private readonly IConfiguration _configuration;
      private readonly TokenService _tokenService;

    


    public AccountController(PizzashopContext context, EmailSender1 ess1, IConfiguration configuration, TokenService tokenService)
    {

       
        _context = context;
        es = ess1;
        _configuration = configuration;
        _tokenService = tokenService;
       
    }

    [HttpGet]
    
    public IActionResult Index()
    {
        

         
           
   
        var cookie = Request.Cookies["cookie"];
         User? user = _context.Users.FirstOrDefault(u => u.Email == cookie);
         if (user == null){
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

        Response.Cookies.Append("authtoken",tokenString,cookieOptions);
        Response.Cookies.Append("cookie",user.Email,new CookieOptions{
            Expires = loginViewModel.RememberMe ? DateTime.UtcNow.AddDays(30) : DateTime.UtcNow.AddHours(1)
        });
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





//  User? user = _context.Users.FirstOrDefault(u => u.Email == m.Email);


        string subject = "reset password";
        Extrathings object1 = new();
        string object2 = object1.getEmail();
        // string object1 ="";
        // Console.WriteLine(m.Email);
        await es.SendEmailAsync(m.Email, subject, object2);

        //   Response.Cookies.Append("cookie2",user.Email,new CookieOptions{
        //     Expires = DateTime.UtcNow.AddDays(30)
        // });

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
    public IActionResult ForgotPasswordConfirmation()
    {
        return View();
    }



    [Authorize]
    public IActionResult userlist()
    {
     
       
        return View();
       
    }

    
[Authorize]
    public IActionResult menu()
    {
        return View();
    }

    
[Authorize]

     public IActionResult UserProfile()
    {
        return View();
    }
}

