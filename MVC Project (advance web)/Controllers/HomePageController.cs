using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MVC_Project__advance_web_.Controllers
{
    public class HomePageController : Controller
    {
        [Authorize] //authorized home page if cookie is not generated
        public IActionResult Index()
        {
            return View(); //show to view if success
        }


    }
}
