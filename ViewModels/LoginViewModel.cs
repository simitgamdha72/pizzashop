using System.ComponentModel.DataAnnotations;



namespace pizzashop.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]

    public string? Email { get; set;}

    [Required(ErrorMessage = "password is required")]
    public string? Password { get; set;}

     public string? Role { get; set;}

    public bool RememberMe { get; set;} = false;

}