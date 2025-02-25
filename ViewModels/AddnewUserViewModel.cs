using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
namespace pizzashop.ViewModels;



public partial class AddnewUserViewModel
{



    [Required]
    public string Email { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;

    [Required]
    public string? FirstName { get; set; }

    [Required]
    public string? LastName { get; set; }

    [Required]
    public string? UserName { get; set; }

    [Required]
    public int? Phone { get; set; }

    public string? Country { get; set; }

    public string? State { get; set; }

    public string? City { get; set; }

    [Required]
    public string? Address { get; set; }

    [Required]
    public int? Zipcode { get; set; }

    public byte[]? Image { get; set; }

    public int? RoleId { get; set; }


}
