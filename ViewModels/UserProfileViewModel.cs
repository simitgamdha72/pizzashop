using System.ComponentModel.DataAnnotations;



namespace pizzashop.ViewModels;

public class UserProfileViewModel    
{


    public string Email { get; set; } = null!;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? UserName { get; set; }

    public int? Phone { get; set; }

    public string? Country { get; set; }

    public string? State { get; set; }

    public string? City { get; set; }

    public string? Address { get; set; }

    public int? Zipcode { get; set; }

    public byte[]? Image { get; set; }


}