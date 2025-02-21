using pizzashop.Models;
namespace pizzashop.ViewModels;

public class UserListViewModel
{
    public List<User> Users { get; set; }
     public List<Role> Role { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
   
}
