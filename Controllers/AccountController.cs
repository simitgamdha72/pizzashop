using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;

using pizzashop.Models.ViewModels;
using Utility;
using pizzashop.Models.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using MailKit;





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


    private string GenerateToken(string email,int? role)
    { 
        string Role = role.ToString();
        var claims = new List<Claim>
        {

            new (ClaimTypes.Email, email),
            new (ClaimTypes.Role, Role),


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
        // User? user2 = _context.Users.FirstOrDefault(u2 => u2.Email == user.Email);
        // Console.WriteLine(user2.Email);
        
       

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



        var tokenString = GenerateToken(user.Email,user.RoleId);
        var cookieOptions = new CookieOptions
        {
            Secure = true,
            HttpOnly = true,
            SameSite = SameSiteMode.Strict
        };

        if (loginViewModel.RememberMe == true)
        {
            Response.Cookies.Append("cookie", user.Email, new CookieOptions
        {
            Expires = loginViewModel.RememberMe ? DateTime.UtcNow.AddDays(30) : DateTime.UtcNow.AddHours(1)
        }); 
        }
      Response.Cookies.Append("AuthToken", tokenString, new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddDays(7),
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict
        });
         Response.Cookies.Append("cookieforid", user.Email, new CookieOptions
        {
            Expires = loginViewModel.RememberMe ? DateTime.UtcNow.AddDays(30) : DateTime.UtcNow.AddHours(1)
        }); 
        

        

      
        return RedirectToAction("userlist", "Account");



    }

    [HttpGet]
    public IActionResult addnewuser()
    {
      
      return View();
    }

    [HttpPost]
    public async Task<IActionResult> addnewuser([Bind("FirstName,LastName,UserName,Email,Password,Zipcode,Address,Phone")]User model){
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        _context.Add(model);
        await _context.SaveChangesAsync();

         string subject = "Login Details";
        Extrathingsforadduser object1 = new();
        string object2 = object1.getEmail();
        //  object2.Replace("{email}", model.Email);
        string emailbody = object2.Replace("{password}", model.Password);
        emailbody = emailbody.Replace("{email}",model.Email);
        // string object1 ="";
        // Console.WriteLine(m.Email);
        await es.SendEmailAsync(model.Email, subject, emailbody);

      


        return RedirectToAction("userlist", "Account");


    }


    [HttpGet]
    [Authorize]
    public async Task<IActionResult> UserProfile()
    {
        var cookie = Request.Cookies["cookieforid"];
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
        Response.Cookies.Delete("cookieforid");
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


//  public async Task<IActionResult> Delete(int? id)
//     {
//         if (id == null)
//         {
//             return NotFound();
//         }

//         var user = await _context.Users
//             .FirstOrDefaultAsync(m => m.UserId == id);
//         if (user == null)
//         {
//             return NotFound();
//         }

//         return View(user);  // This will pass the user to the Delete view
//     }

//    [HttpPost, ActionName("Delete")]
//     [ValidateAntiForgeryToken]
//     public async Task<IActionResult> DeleteConfirmed(int UserId)
//     {   
//         Console.WriteLine("123");
//         Console.WriteLine(UserId);
//         var user = await _context.Users.FindAsync(UserId);
//         if (user != null)
//         {
//             _context.Users.Remove(user);
//             await _context.SaveChangesAsync();
//         }
        
//         return RedirectToAction(nameof(Index));  // Redirect to the Users List (Index page)
//     }
  
  [HttpGet]
   public IActionResult edituser(int? id)
    {
         User? user = _context.Users.FirstOrDefault(u => u.UserId == id);
        if (user == null)
        {
            return NotFound();
        }
        var viewModel = new User
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
        Console.WriteLine(id);
        return View(viewModel);
    }

        [HttpPost]
    public IActionResult edituser(User u)
    {
     
        User? user = _context.Users.FirstOrDefault(u => u.UserId == u.UserId);
        Console.WriteLine(u.Email);


        if (user == null)
        {
            return NotFound();
        }

       
            user.Email = u.Email;
            user.FirstName = u.FirstName;
           user.LastName = u.LastName;
            user.UserName = u.UserName;
            user.Phone = u.Phone;
           user.Password = u.Password;
         user.Address = u.Address;
           user.Zipcode = u.Zipcode;

        _context.Users.Update(user);
        _context.SaveChanges();

        return RedirectToAction("userlist", "Account");


    }

    [Authorize]
    [HttpGet]
    public IActionResult Dashboard(){
        return View();
    }


}

