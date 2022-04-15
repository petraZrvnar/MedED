using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MedEd.Models;
using Microsoft.AspNetCore.Http;
using MedEd.ViewModels;

namespace MedEd.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index(bool odjava = false)
        {
            var uloga = HttpContext.Session.GetString("Uloga");
            if(uloga != null)
            {
                ViewBag.Ulogiran = "true";
            }
            else
            {
                ViewBag.Ulogiran = "false";
            }
            var model = new HomeViewModel
            {
                Odjava = odjava
            };
            return View(model);
        }

        public IActionResult Info()
        {
            var uloga = HttpContext.Session.GetString("Uloga");
            if (uloga != null)
            {
                ViewBag.Ulogiran = "true";
            }
            else
            {
                ViewBag.Ulogiran = "false";
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
