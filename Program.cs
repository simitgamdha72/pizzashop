using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using pizzashop.Models;
using Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;







var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<PizzashopContext>();
builder.Services.AddScoped<EmailSender1>();
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
            // RoleClaimType = ClaimTypes.Role
           
        };
        options.Events = new JwtBearerEvents{
            OnMessageReceived = context => {
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


