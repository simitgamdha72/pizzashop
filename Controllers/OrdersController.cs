using Microsoft.AspNetCore.Mvc;
using pizzashop.Models.Models;
using pizzashop.Models.ViewModels;

namespace pizzashop.Controllers
{
    public class OrdersController : Controller
    {

        private readonly PizzashopContext _context;

        public OrdersController(PizzashopContext context)
        {
            _context = context;
        }

        public IActionResult Orders()
        {
            return View();
        }

    }
}
