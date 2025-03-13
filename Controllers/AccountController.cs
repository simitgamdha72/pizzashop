using Microsoft.AspNetCore.Mvc;
using pizzashop.Models.ViewModels;
using pizzashop.Utility;
using pizzashop.Models.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using pizzashop.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using pizzashop.Service;
using pizzashop.Helpers;

namespace pizzashop.Controllers;

public class AccountController : Controller
{

    public readonly EmailSender1 es;

    private readonly IWebHostEnvironment _webHostEnvironment;

    private readonly dropdownService _dropdownService;
    private readonly IConfiguration _configuration;
    private readonly IAccountService _accountService;


    public AccountController(IWebHostEnvironment webHostEnvironment, dropdownService dropdownService, IAccountService accountService, EmailSender1 ess1, IConfiguration configuration)
    {
        _accountService = accountService;
        es = ess1;
        _configuration = configuration;
        _dropdownService = dropdownService;
        _webHostEnvironment = webHostEnvironment;

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




        return View();


    }





    [HttpGet]
    public async Task<JsonResult> GetStatesByCountry(int countryId)
    {

        List<SelectListItem> states = _dropdownService.GetState(countryId);
        return Json(states);
    }

    [HttpGet]
    public async Task<JsonResult> GetCitiesByState(int stateId)
    {
        List<SelectListItem> cities = _dropdownService.GetCity(stateId);
        return Json(cities);
    }

    [HttpPost]
    public async Task<IActionResult> addnewuser(AddnewUserViewModel model)
    {

        string fileName = null;
        if (model.ProfileImage != null && model.ProfileImage.Length > 0)
        {
            try
            {
                var imageHelper = new ImageHelper(_webHostEnvironment);
                fileName = await imageHelper.SaveImageAsync(model.ProfileImage);
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("ProfileImage", ex.Message);
                return View(model);
            }
        }


        // var countries = await _countryservice.GetCountriesAsync();
        if (ModelState.IsValid)
        {
            var valid = _accountService.CreateUser(model, fileName);

            if (valid == 1)
            {


                TempData["alredyexist"] = "Email or UserName is Already exist";
                return View(model);
            }


            TempData["usersadded"] = "New User is Added";

            return RedirectToAction("UserList", "Account");
        }



        // ViewBag.Countries = countries;
        return View(model);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> edituserAsync(int id)
    {
        // var countries = await _countryservice.GetCountriesAsync();


        var viewModel = _accountService.edituserget(id);

        // viewModel.Countries = countries;


        if (viewModel == null)
        {
            return NotFound();
        }

        return View(viewModel);
    }


    [HttpPost]
    public async Task<IActionResult> edituser(EditUserViewModel model)
    {

        string fileName = null;
        if (model.ProfileImage != null && model.ProfileImage.Length > 0)
        {
            try
            {
                var imageHelper = new ImageHelper(_webHostEnvironment);
                fileName = await imageHelper.SaveImageAsync(model.ProfileImage);
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("ProfileImage", ex.Message);
                return View(model);
            }
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var validator = _accountService.edituserpost(model, fileName);

        if (validator == 1)
        {
            return NotFound();
        }

        if (validator == 2)
        {
            TempData["usernameexist"] = "This UserName is Already in Use";
            return View(model);
        }

        TempData["edituser"] = "Users's details successfuly changed";

        return RedirectToAction("userlist", "Account");
    }




    [HttpGet]
    [Authorize]
    public async Task<IActionResult> UserProfile()
    {
        // var countries = await _countryservice.GetCountriesAsync();
        // ViewBag.Countries = countries;
        var cookie = Request.Cookies["cookieforid"];
        if (cookie == null)
        {
            return NotFound();
        }
        var viewModel = await _accountService.GetUserProfile(cookie);
        return View(viewModel);
    }


    [HttpPost]
    public async Task<IActionResult> UserProfile(UserProfileViewModel model)
    {

        if (ModelState.IsValid)
        {
            int validator = _accountService.UpdateUserProfile(model);

            if (validator == 1)
            {
                TempData["userprofiledatachanged"] = "Profile is Updated";
                return RedirectToAction("UserProfile", "Account");
            }

            if (validator == 0)
            {
                return NotFound();
            }

            if (validator == 2)
            {
                if (model.CountryId != 0)
                {
                    model.CountryId = 0;
                }
                TempData["usernameexist"] = "This UserName is Already in Use";
                return View(model);
            }

        }
        // var countries = await _countryservice.GetCountriesAsync();
        // ViewBag.Countries = countries;
        if (model.CountryId != 0)
        {
            model.CountryId = 0;
        }

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
        var validator = _accountService.changepassword(cookie, model);
        if (validator == 1)
        {
            TempData["ResetPasswordConfirmation"] = "Password is Successfully Changed";
            Response.Cookies.Delete("AuthToken");
            Response.Cookies.Delete("cookie");
            return RedirectToAction("Index", "Account");
        }
        if (validator == 0)
        {
            TempData["passwordNotMatch"] = "Password is Incorrect";
            return RedirectToAction("changepassword", "Account");
        }
        if (validator == 2)
        {
            TempData["BothpasswordSame"] = "New Password is have to different from current Password";
            return RedirectToAction("changepassword", "Account");
        }
        else
        {
            return NotFound();
        }
    }



    [Authorize]
    public async Task<IActionResult> userlist(int page = 1, string search = "", int? roleId = null, bool? status = null, int pageSize = 5)
    {
        // Get filtered users count for pagination
        var totalUsers = await _accountService.GetTotalUsersFilteredAsync(search, roleId, status);

        // Get filtered and paged users
        var users = await _accountService.GetFilteredUsersPagedAsync(page, pageSize, search, roleId, status);

        // Get all roles for dropdown
        var roles = await _accountService.GetAllRolesAsync();

        var model = new UserListViewModel
        {
            Users = (List<User>)users,
            Role = (List<Role>)roles,
            CurrentPage = page,
            TotalPages = (int)Math.Ceiling((double)totalUsers / pageSize),
            TotalUsers = totalUsers,
            SearchTerm = search,
            PageSize = pageSize,
            RoleFilter = roleId,
            StatusFilter = status
        };

        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            // If it's an AJAX request, return only the partial view
            return PartialView("_UserTable", model);
        }

        return View(model);
    }













    [HttpPost]
    public IActionResult DeleteUser(int id)
    {
        var cookie = Request.Cookies["cookieforid"];
        if (cookie == null)
        {
            return NotFound();
        }

        int validator = _accountService.DeleteUser(id, cookie);

        if (validator == 1)
        {
            TempData["deleteUser"] = "User Deleted Successfully";
            return RedirectToAction("UserList", "Account");
        }
        if (validator == 2)
        {
            TempData["selfdelete"] = "You Can not Delete Your Self";
            return RedirectToAction("UserList", "Account");
        }
        return NotFound();


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

