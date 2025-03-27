using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using pizzashop.Models.Models;
using pizzashop.Repository;
using pizzashop.Utility;
using pizzashop.Service;
using pizzashop.Services;
using pizzashop.Repositories;
using pizzashop.Service.implementation;










var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<PizzashopContext>();
builder.Services.AddScoped<EmailSender1>();
builder.Services.AddScoped<EmailSenderservice>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<iuser, user>();
builder.Services.AddScoped<icountry, country>();
builder.Services.AddScoped<istate, state>();
builder.Services.AddScoped<icity, city>();
builder.Services.AddScoped<icountryservice, countryservice>();
builder.Services.AddScoped<istateservice, stateservice>();
builder.Services.AddScoped<icityservice, cityservice>();
builder.Services.AddScoped<icategory, category>();
builder.Services.AddScoped<icategoryservice, categoryservice>();
builder.Services.AddScoped<iunit, unit>();
builder.Services.AddScoped<iunitservice, unitservice>();
builder.Services.AddScoped<imenuitem, menuitem>();
builder.Services.AddScoped<imenuitemservice, menuitemservice>();
builder.Services.AddScoped<irole, role>();
builder.Services.AddScoped<ipermission, permission>();
builder.Services.AddScoped<irolewisepermission, rolewisepermission>();
builder.Services.AddScoped<iroleandpermissionservice, roleandpermissionservice>();
builder.Services.AddScoped<dropdownService>();
builder.Services.AddScoped<itax, tax>();
builder.Services.AddScoped<itable, table>();
builder.Services.AddScoped<isection, section>();
builder.Services.AddScoped<itaxservice, taxservice>();
builder.Services.AddScoped<itablesectionservice, tablesectionservice>();
builder.Services.AddScoped<imodifiersgroup, modifiersgroup>();
builder.Services.AddScoped<imenumodifiers, menumodifiers>();
builder.Services.AddScoped<imodifierservice, modifierservice>();

// builder.Services
//     .AddControllers()
//     .AddNewtonsoftJson(options =>
//     {
//         options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
//     });





// Add TokenService to the DI container
builder.Services.AddScoped<TokenService>();

// builder.Services.AddAuthentication();


builder.Services.AddHttpContextAccessor();




// builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//     .AddCookie(options =>
//     {
//         options.LoginPath = "/Account/Index"; // Specify your login page here
//         options.AccessDeniedPath = "/Account/AccessDenied";  // Optional: redirect when user is not authorized
//     });

//
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = "localhost",
            ValidAudience = "localhost",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("hey1234567890ojykjrkr6uluyk")),
            ClockSkew = TimeSpan.Zero,
            RoleClaimType = ClaimTypes.Role

        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var token = context.Request.Cookies["authtoken"];
                if (!string.IsNullOrEmpty(token))
                {
                    context.Token = token;
                }
                return Task.CompletedTask;
            }
        };
    }
    );


//


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Index}/{id?}");

app.Run();


