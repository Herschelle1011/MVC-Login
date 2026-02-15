using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC_Project__advance_web_.Models;

namespace MVC_Project__advance_web_.Controllers
{
    public class HomeController : Controller
    {
        [Authorize] //secure index view
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Privacy()  //secure privacy view
        {
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Authorize]
        public IActionResult Error() //secure error view
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
