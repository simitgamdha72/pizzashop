using Microsoft.AspNetCore.Mvc;
using pizzashop.Models.ViewModels;
using pizzashop.Utility;
using pizzashop.Models.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;
using pizzashop.Repository;
using pizzashop.Repository.implementation;
using pizzashop.Services;

namespace pizzashop.Controllers;

public class AccountController : Controller
{
    public readonly EmailSender1 es;
    private readonly IConfiguration _configuration;
    private readonly IAccountService _accountService;
    private readonly icountryservice _countryservice;
    private readonly istateservice _stateservice;
    private readonly icityservice _cityservice;

    public AccountController(IAccountService accountService, icityservice cityservice, istateservice stateservice, icountryservice countryservice, EmailSender1 ess1, IConfiguration configuration)
    {
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

        var user = _accountService.GetUserProfileforlogin(cookie);

        if (user == null)
        {
            return View();
        }



        return RedirectToAction("UserList", "Account");
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

        // Set success message in TempData
        TempData["success"] = "Successfully logged in!";

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
        return RedirectToAction("UserList", "Account");
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> addnewuser()
    {
        var countries = await _countryservice.GetCountriesAsync();
        ViewBag.Countries = countries;
        return View();
    }


    [HttpGet]
    public async Task<JsonResult> GetStatesByCountry(int countryId)
    {

        var states = await _stateservice.GetStatesByCountryIdAsync(countryId);
        return Json(states.Select(s => new { value = s.Id, text = s.State1 }));
    }

    [HttpGet]
    public async Task<JsonResult> GetCitiesByState(int stateId)
    {
        var cities = await _cityservice.GetCitiesByStateIdAsync(stateId);
        return Json(cities.Select(c => new { value = c.Id, text = c.City1 }));
    }

    [HttpPost]
    public async Task<IActionResult> addnewuser(AddnewUserViewModel model)
    {
        var countries = await _countryservice.GetCountriesAsync();
        if (ModelState.IsValid)
        {
            var valid = _accountService.CreateUser(model);

            if (valid == 1)
            {
                ViewBag.Countries = countries;
                TempData["alredyexist"] = "Email or UserName is Already exist";
                return View();
            }

            return RedirectToAction("UserList", "Account");
        }

        ViewBag.Countries = countries;
        return View(model);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> UserProfile()
    {
        var countries = await _countryservice.GetCountriesAsync();
        ViewBag.Countries = countries;
        var cookie = Request.Cookies["cookieforid"];
        Console.WriteLine(cookie);
        var viewModel = await _accountService.GetUserProfile(cookie);
        return View(viewModel);
    }


    [HttpPost]
    public async Task<IActionResult> UserProfile(UserProfileViewModel model)
    {
        if (ModelState.IsValid)
        {
            _accountService.UpdateUserProfile(model);
            return RedirectToAction("UserProfile", "Account");
        }
        var countries = await _countryservice.GetCountriesAsync();
        ViewBag.Countries = countries;
        return View(model);
    }


    [HttpGet]
    public IActionResult forgotpassword()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> forgotpassword(ForgotPasswordViewModel m)
    {
        User? User = await _accountService.GetUserProfileforlogin(m.Email);
        if (User == null)
        {
            TempData["usernotexist"] = "User Not Exist";
            return View();
        }

        string subject = "reset password";
        Extrathings object1 = new();
        string object2 = object1.getEmail();
        await es.SendEmailAsync(m.Email, subject, object2);
        Response.Cookies.Append("cookie2", m.Email);

        TempData["emailIsSent"] = "Email is Sent on Email Address";
        return View();
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

        if (cookie == null)
        {
            TempData["cookieIsNull"] = "Invalid Attempt";
            return RedirectToAction("forgotpassword", "Account");
        }

        TempData["ResetPasswordConfirmation"] = "Password is Reset Successlly";


        _accountService.resetpassword(cookie, m);
        return RedirectToAction("Index", "Account");
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


    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        TempData["logout"] = "successfully logged out";
        Response.Cookies.Delete("AuthToken");
        Response.Cookies.Delete("cookie");
        Response.Cookies.Delete("cookieforid");

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

        var cookie = Request.Cookies["cookieforid"];
        var c = _accountService.changepassword(cookie, model);
        if (c == 1)
        {
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
    public async Task<IActionResult> userlist(int page = 1) // repo baki
    {

        var totalUsers = await _accountService.totaluser();

        var users = await _accountService.userlist(page);

        var model = new UserListViewModel
        {
            Users = (List<User>)users,
            CurrentPage = page,
            TotalPages = (int)Math.Ceiling((double)totalUsers / 5)
        };

        return View(model);
    }


    [HttpPost]
    public IActionResult DeleteUser(int id)
    {
        _accountService.DeleteUser(id);
        return RedirectToAction("UserList", "Account");
    }


    [HttpGet]
    [Authorize]
    public async Task<IActionResult> edituserAsync(int id)
    {
        var countries = await _countryservice.GetCountriesAsync();
        ViewBag.Countries = countries;

        var viewModel = _accountService.edituserget(id);

        if (viewModel == null)
        {
            return NotFound();
        }

        return View(viewModel);
    }


    [HttpPost]
    public async Task<IActionResult> edituser(EditUserViewModel model)
    {

        if (!ModelState.IsValid)
        {
            var countries = await _countryservice.GetCountriesAsync();
            ViewBag.Countries = countries;
            return View(model);
        }

        var e = _accountService.edituserpost(model);

        if (e == 1)
        {
            return NotFound();
        }

        return RedirectToAction("userlist", "Account");
    }


    [Authorize]
    [HttpGet]
    public IActionResult Dashboard()
    {
        return View();
    }


    [HttpGet]
    public IActionResult role()
    {
        return View();
    }


    [HttpGet]
    public IActionResult permission()
    {
        return View();
    }



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
}

