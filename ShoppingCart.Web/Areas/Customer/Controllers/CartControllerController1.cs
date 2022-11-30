using Microsoft.AspNetCore.Mvc;

namespace ShoppingCart.Web.Areas.Customer.Controllers
{
    public class CartControllerController1 : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
