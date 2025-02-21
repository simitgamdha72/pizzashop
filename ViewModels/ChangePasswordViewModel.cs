using System.ComponentModel.DataAnnotations;
namespace pizzashop.ViewModels;

public class ChangePasswordViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
  
    [Required]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 6)]
    public string newPassword { get; set; }

    [DataType(DataType.Password)]
    [Compare("newPassword")]
    public string ConfirmPassword { get; set; }


}
