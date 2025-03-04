using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;

using pizzashop.Models.ViewModels;
using pizzashop.Utility;
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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using pizzashop.Repository;
using pizzashop.Repository.implementation;
using pizzashop.Services;





namespace pizzashop.Controllers;


public class AccountController : Controller
{
    private readonly PizzashopContext _context;
    private readonly iuser _user;
    private readonly icountry _country;
    private readonly istate _state;
    private readonly icity _city;
    private const int PageSize = 5;
    public readonly EmailSender1 es;
    private readonly IConfiguration _configuration;
    private readonly IAccountService _accountService;
    private readonly icountryservice _countryservice;
    private readonly istateservice _stateservice;
    private readonly icityservice _cityservice;



    public AccountController(PizzashopContext context, IAccountService accountService, icityservice cityservice, istateservice stateservice, icountryservice countryservice, EmailSender1 ess1, IConfiguration configuration, iuser user, icountry country, istate state, icity city)
    {
        _user = user;
        _context = context;
        _country = country;
        _state = state;
        _city = city;
        _accountService = accountService;
        _countryservice = countryservice;
        _stateservice = stateservice;
        _cityservice = cityservice;
        es = ess1;
        _configuration = configuration;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var cookie = Request.Cookies["cookie"];
        Console.WriteLine(cookie);

        if (cookie == null)
        {
            return View();
        }
        var user = _accountService.GetUserProfile(cookie);

        if (user == null)
        {
            return View();
        }

        return RedirectToAction("UserList", "Account");
    }

    // [HttpGet]

    // public IActionResult Index()
    // {
    //     var cookie = Request.Cookies["cookie"];
    //     User? user = _user.GetFirstOrDefault(u => u.Email == cookie);
    //     if (user == null)
    //     {
    //         return View();
    //     }
    //     return RedirectToAction("userlist", "Account");
    // }

    private string GenerateToken(string email, int? role)
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
    public async Task<IActionResult> Index(LoginViewModel loginViewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(loginViewModel);
        }

        var user = await _accountService.LoginAsync(loginViewModel);
        if (user == null)
        {
            TempData["error"] = "User not found or invalid password.";
            return View(loginViewModel);
        }

        var tokenString = GenerateToken(user.Email, user.RoleId);
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

        // Handle Token and Cookies as in your existing logic...
        return RedirectToAction("UserList", "Account");
    }

    // [HttpPost]

    // public IActionResult Index(LoginViewModel loginViewModel)
    // {
    //     if (!ModelState.IsValid)
    //     {
    //         return View(loginViewModel);
    //     }

    //     User? user = _user.GetFirstOrDefault(u => u.Email == loginViewModel.Email);
    //     // User? user2 = _context.Users.FirstOrDefault(u2 => u2.Email == user.Email);
    //     // Console.WriteLine(user2.Email);

    //     if (user == null)
    //     {
    //         TempData["error"] = "User Not Found";
    //         return View(loginViewModel);
    //     }

    //     if (user.Password != loginViewModel.Password)
    //     {
    //         TempData["error"] = "Password is Incorrect";
    //         return View(loginViewModel);
    //     }

    //     var tokenString = GenerateToken(user.Email, user.RoleId);
    //     var cookieOptions = new CookieOptions
    //     {
    //         Secure = true,
    //         HttpOnly = true,
    //         SameSite = SameSiteMode.Strict
    //     };

    //     if (loginViewModel.RememberMe == true)
    //     {
    //         Response.Cookies.Append("cookie", user.Email, new CookieOptions
    //         {
    //             Expires = loginViewModel.RememberMe ? DateTime.UtcNow.AddDays(30) : DateTime.UtcNow.AddHours(1)
    //         });
    //     }
    //     Response.Cookies.Append("AuthToken", tokenString, new CookieOptions
    //     {
    //         Expires = DateTimeOffset.UtcNow.AddDays(7),
    //         HttpOnly = true,
    //         Secure = true,
    //         SameSite = SameSiteMode.Strict
    //     });
    //     Response.Cookies.Append("cookieforid", user.Email, new CookieOptions
    //     {
    //         Expires = loginViewModel.RememberMe ? DateTime.UtcNow.AddDays(30) : DateTime.UtcNow.AddHours(1)
    //     });
    //     return RedirectToAction("userlist", "Account");
    // }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> addnewuser()
    {
        // Call the service layer to get the countries
        var countries = await _countryservice.GetCountriesAsync();

        // Pass countries to the view using ViewBag
        ViewBag.Countries = countries;

        return View();
    }


    // [HttpGet]
    // [Authorize]
    // public IActionResult addnewuser()
    // {
    //     ViewBag.Countries = _country.ToList();
    //     return View();
    // }


    [HttpGet]
    public async Task<JsonResult> GetStatesByCountry(int countryId)
    {
        var states = await _stateservice.GetStatesByCountryIdAsync(countryId);
        return Json(states.Select(s => new { value = s.Id, text = s.State1 }));
    }
    // [HttpGet]
    // public JsonResult GetStatesByCountry(int countryId)
    // {
    //     var states = _state.GetStatesByCountryId(countryId);
    //     // var states = _context.States.Where(s => s.CountryId == countryId).ToList();
    //     return Json(states.Select(s => new { value = s.Id, text = s.State1 }));

    // }

    [HttpGet]
    public async Task<JsonResult> GetCitiesByState(int stateId)
    {
        var cities = await _cityservice.GetCitiesByStateIdAsync(stateId);
        return Json(cities.Select(c => new { value = c.Id, text = c.City1 }));
    }
    // [HttpGet]
    // public JsonResult GetCitiesByState(int stateId)
    // {
    //     var cities = _city.GetCitiesByStateId(stateId);
    //     // var cities = _context.Cities.Where(c => c.StateId == stateId).ToList();
    //     return Json(cities.Select(c => new { value = c.Id, text = c.City1 }));
    //     // var cities = _context.Cities.Where(c => c.Id == stateId).ToList();
    //     // return Json(cities);
    // }

    [HttpPost]
    public async Task<IActionResult> addnewuser(AddnewUserViewModel model)
    {
        if (ModelState.IsValid)
        {
            _accountService.CreateUser(model);
            return RedirectToAction("UserList", "Account");
        }

        ViewBag.Countries = _country.ToList();
        return View(model);
    }


    // [HttpPost]
    // public async Task<IActionResult> addnewuser(AddnewUserViewModel model)
    // {

    //     if (!ModelState.IsValid)
    //     {
    //         ViewBag.Countries = _country.ToList();

    //         return View(model);
    //     }

    //     Console.WriteLine(model.CountryId);
    //     Country? country = _country.GetFirstOrDefault(c => c.Id == model.CountryId);
    //     State? state = _state.GetFirstOrDefault(s => s.Id == model.StateId);
    //     City? city = _city.GetFirstOrDefault(y => y.Id == model.CityId);
    //     var viewmodel = new User
    //     {
    //         FirstName = model.FirstName,
    //         LastName = model.LastName,
    //         UserName = model.UserName,
    //         Email = model.Email,
    //         Password = model.Password,
    //         Zipcode = model.Zipcode,
    //         Address = model.Address,
    //         Phone = model.Phone,
    //         RoleId = model.RoleId,
    //         Country = country.Country1,
    //         State = state.State1,
    //         City = city.City1,
    //     };
    //     if (!ModelState.IsValid)
    //     {
    //         return View(model);
    //     }

    //     _user.Add(viewmodel);
    //     await _user.SaveAsync();

    //     string subject = "Login Details";
    //     Extrathingsforadduser object1 = new();
    //     string object2 = object1.getEmail();
    //     string emailbody = object2.Replace("{password}", model.Password);
    //     emailbody = emailbody.Replace("{email}", model.Email);
    //     await es.SendEmailAsync(model.Email, subject, emailbody);
    //     return RedirectToAction("userlist", "Account");
    // }

    //   [HttpGet]
    //         public IActionResult UserProfile()
    //         {
    //             var cookie = Request.Cookies["cookieforid"];
    //             var user = _accountService.GetUserProfile(cookie);
    //             if (user == null)
    //             {
    //                 return NotFound();
    //             }

    //             var viewModel = new UserProfileViewModel
    //             {
    //                 FirstName = user.FirstName,
    //                 LastName = user.LastName,
    //                 UserName = user.UserName,
    //                 Email = user.Email,
    //                 Phone = user.Phone,
    //                 Address = user.Address,
    //                 Zipcode = user.Zipcode,
    //             };

    //             return View(viewModel);
    //         }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> UserProfile()
    {
        ViewBag.Countries = _country.ToList();

        var cookie = Request.Cookies["cookieforid"];
        User? user = _user.GetFirstOrDefault(u => u.Email == cookie);
        //  Country? country= _context.Countries.FirstOrDefault(c => c.Country1 == user.Country);
        // State? state= _context.States.FirstOrDefault(s => s.State1 == user.State);
        // City? city= _context.Cities.FirstOrDefault(y => y.City1 == user.City);

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
            // CountryId = country.Id,
            // StateId = state.Id,
            // CityId = city.Id,
            Address = user.Address,
            Zipcode = user.Zipcode,
        };
        return View(viewModel);
    }

    [HttpPost]
    public IActionResult UserProfile(UserProfileViewModel model)
    {
        if (ModelState.IsValid)
        {
            _accountService.UpdateUserProfile(model);
            return RedirectToAction("UserProfile", "Account");
        }

        ViewBag.Countries = _country.ToList();
        return View(model);
    }

    // [HttpPost]
    // public IActionResult UserProfile(UserProfileViewModel model)
    // {

    //     if (!ModelState.IsValid)
    //     {
    //         ViewBag.Countries = _country.ToList();
    //         // ViewBag.States = _context.States.ToList();
    //         // ViewBag.cities = _context.Cities.ToList();
    //         return View(model);
    //     }

    //     var cookie = Request.Cookies["cookieforid"];
    //     User? user = _user.GetFirstOrDefault(u => u.Email == cookie);
    //     Country? country = _country.GetFirstOrDefault(c => c.Id == model.CountryId);
    //     State? state = _state.GetFirstOrDefault(s => s.Id == model.StateId);
    //     City? city = _city.GetFirstOrDefault(y => y.Id == model.CityId);

    //     if (user == null)
    //     {
    //         return NotFound();
    //     }
    //     // user.Email = model.Email;
    //     user.FirstName = model.FirstName;
    //     user.LastName = model.LastName;
    //     user.UserName = model.UserName;
    //     user.Phone = model.Phone;
    //     user.Address = model.Address;
    //     user.Zipcode = model.Zipcode;
    //     user.Country = country.Country1;
    //     user.State = state.State1;
    //     user.City = city.City1;
    //     _user.Update(user);
    //     _user.Save();
    //     return RedirectToAction("UserProfile", "Account");
    // }


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

        return View();
    }


    [HttpPost]
    public IActionResult ResetPassword(ResetPasswordViewModel m)
    {
        var cookie = Request.Cookies["cookie2"];
        User? user = _user.GetFirstOrDefault(u => u.Email == cookie);

        if (user != null)
        {
            user.Password = m.Password;

            _user.Save();
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
    public IActionResult changepassword(ChangePasswordViewModel model)
    {
        // if (!ModelState.IsValid)
        // {
        //     return View(model);
        // }
        var cookie = Request.Cookies["cookieforid"];
        User? user = _user.GetFirstOrDefault(u => u.Email == cookie);

        if (user.Password == model.Password)
        {
            user.Password = model.newPassword;
            _user.Update(user);
            _user.Save();
            Response.Cookies.Delete("AuthToken");
            Response.Cookies.Delete("cookie");
            return RedirectToAction("Index", "Account");
        }
        else
        {
            return RedirectToAction("changepassword", "Account");
        }
    }


    [Authorize]
    public async Task<IActionResult> userlist(string searchTerm, int page = 1) // repo baki
    {
        // int pageSize = itemsPerPage;
        var totalUsers = await _user.GetTotalUsersAsync();
        // var users = await _user.GetActiveUsersAsync(); 
        var users = await _context.Users.Where(u => u.Isdeleted != true) // <--
            .Skip((page - 1) * PageSize) // Skip users for the current page
            .Take(PageSize) // Get the users for the current page
            .ToListAsync();

        //  int totalPages = (int)Math.Ceiling((double)totalUsers / pageSize);

        var model = new UserListViewModel
        {
            Users = users,
            // TotalPages = totalPages,
            // ItemsPerPage = pageSize,
            CurrentPage = page,
            TotalPages = (int)Math.Ceiling((double)totalUsers / PageSize)
        };
        return View(model);
    }

    // UsersController.cs

    [HttpPost]
    public IActionResult DeleteUser(int id)
    {
        _accountService.DeleteUser(id);
        return RedirectToAction("UserList", "Account");
    }

    // [HttpPost]
    // [ValidateAntiForgeryToken]
    // public async Task<IActionResult> DeleteUser(int id)
    // {
    //     Console.WriteLine(id);
    //     var user = await _user.GetUserByIdAsync(id);
    //     Console.WriteLine(user);
    //     // var user = await _context.Users.FindAsync(id);
    //     if (user == null)
    //     {
    //         return NotFound();
    //     }
    //     user.Isdeleted = true;
    //     Console.WriteLine(user.Isdeleted);
    //     _user.Update(user);
    //     await _user.SaveAsync();
    //     // Redirect to the user list after deleting the user
    //     return RedirectToAction("userlist", "Account");
    // }


    [HttpGet]
    [Authorize]
    public IActionResult edituser(int? id)
    {
        ViewBag.Countries = _country.ToList();
        //      ViewBag.States = _context.States.ToList();
        //  ViewBag.cities = _context.Cities.ToList();
        User? user = _user.GetFirstOrDefault(u => u.UserId == id);
        Country? country = _country.GetFirstOrDefault(c => c.Country1 == user.Country);
        State? state = _state.GetFirstOrDefault(s => s.State1 == user.State);
        City? city = _city.GetFirstOrDefault(y => y.City1 == user.City);
        //ViewBag.state = state.State1;
        if (user == null)
        {
            return NotFound();
        }

        var viewModel = new EditUserViewModel
        {
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            UserName = user.UserName,
            Phone = user.Phone ?? 0,
            RoleId = user.RoleId ?? 0,
            Status = user.Status ?? true,
            // Image = user.Image,
            // CountryId = country.Id,
            // StateId = state.Id,
            // CityId = city.Id,
            Address = user.Address,
            Zipcode = user.Zipcode ?? 0,
        };
        Console.WriteLine(id);
        return View(viewModel);
    }


    [HttpPost]
    public IActionResult edituser(EditUserViewModel model)
    {

        if (!ModelState.IsValid)
        {
            ViewBag.Countries = _context.Countries.ToList();
            // ViewBag.States = _context.States.ToList();
            // ViewBag.cities = _context.Cities.ToList();
            return View(model);
        }

        Country? country = _country.GetFirstOrDefault(c => c.Id == model.CountryId);
        State? state = _state.GetFirstOrDefault(s => s.Id == model.StateId);
        City? city = _city.GetFirstOrDefault(y => y.Id == model.CityId);
        User? user = _user.GetFirstOrDefault(u => u.Email == model.Email);

        if (user == null)
        {
            return NotFound();
        }

        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        user.UserName = model.UserName;
        user.Phone = model.Phone;
        // user.Status = u.Status;
        // Console.WriteLine(model.MyImage);
        // user.MyImage = model.MyImage.ToString();
        // user.Password = u.Password;
        // Console.WriteLine(model.RoleId);
        user.Status = model.Status;
        user.RoleId = model.RoleId;
        user.Address = model.Address;
        user.Zipcode = model.Zipcode;
        user.Country = country.Country1;
        user.State = state.State1;
        user.City = city.City1;

        _user.Save();

        return RedirectToAction("userlist", "Account");
    }

    [Authorize]
    [HttpGet]
    public IActionResult Dashboard()
    {
        return View();
    }
}

