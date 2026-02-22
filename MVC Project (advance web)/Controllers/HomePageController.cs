using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MVC_Project__advance_web_.Controllers
{
    public class HomePageController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }


    }
}
