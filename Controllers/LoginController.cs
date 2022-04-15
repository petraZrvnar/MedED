using MedEd.Models;
using MedEd.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace MedEd.Controllers
{
    public class LoginController : Controller
    {
        private readonly MedEdContext _context;

        public LoginController(MedEdContext context)
        {
            _context = context;
        }

        public IActionResult Index(bool failPass = false, bool failUser = false)
        {
            var uloga = HttpContext.Session.GetString("Uloga");
            if (uloga != null)
            {
                return RedirectToAction("UserHome", "Login");
            }
            var model = new LoginViewModel
            {
                FailPass = failPass,
                FailUser = failUser
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Login(string user, string pass)
        {
            bool passFail = false;
            bool userFail = false;
            var korisnik = _context.RegistriraniKorisnik
                .Include(d => d.SifLjekarnaNavigation)
                .Include(d => d.SifUlogaNavigation)
                .Where(d => String.Equals(d.KorisnickoIme, user));

            if (!korisnik.Any())
            {
                userFail = true;
            }

            byte[] salt = new byte[128 / 8];
            string lozinka = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: pass,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA1,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));
            korisnik = korisnik.Where(d => String.Equals(d.Lozinka, lozinka));

            if (!korisnik.Any())
            {
                passFail = true;
            }

            if (passFail || userFail)
            {
                return RedirectToAction("Index", new { failPass = passFail, failUser = userFail });
            }
            var korisnik2 = korisnik.FirstOrDefault();

            HttpContext.Session.SetString("Uloga", korisnik2.SifUlogaNavigation.OpisUloga);
            HttpContext.Session.SetString("User", korisnik2.KorisnickoIme);
            HttpContext.Session.SetString("Ljekarna", korisnik2.SifLjekarna.ToString());

            if (String.Equals(korisnik2.SifUlogaNavigation.OpisUloga, "Administrator"))
            {
                return RedirectToAction("AdminHome");
            }

            else if (String.Equals(korisnik2.SifUlogaNavigation.OpisUloga, "Ljekarnik"))
            {
                return RedirectToAction("PharmaHome");
            }
            else
            {
                return RedirectToAction("PharmaAdminHome");
            }
        }

        public IActionResult AdminHome()
        {
            var uloga = HttpContext.Session.GetString("Uloga");
            var korisnik = HttpContext.Session.GetString("User");
            if (uloga == null || uloga != "Administrator")
            {
                return RedirectToAction("NotAuthorized");
            }
            return View();
        }

        public IActionResult PharmaHome()
        {
            var uloga = HttpContext.Session.GetString("Uloga");
            var korisnik = HttpContext.Session.GetString("User");
            if (uloga == null || uloga != "Ljekarnik")
            {
                return RedirectToAction("NotAuthorized");
            }
            var ljekarna = HttpContext.Session.GetString("Ljekarna");
            if (string.IsNullOrEmpty(ljekarna))
            {
                ViewBag.Zaposlen = "false";
            }
            else
            {
                ViewBag.Zaposlen = "true";
            }
            return View();
        }

        public IActionResult PharmaAdminHome()
        {
            var uloga = HttpContext.Session.GetString("Uloga");
            var korisnik = HttpContext.Session.GetString("User");
            if (uloga == null || uloga != "Ljekarnik Admin")
            {
                return RedirectToAction("NotAuthorized");
            }
            var ljekarna = HttpContext.Session.GetString("Ljekarna");
            if (string.IsNullOrEmpty(ljekarna))
            {
                ViewBag.Zaposlen = "false";
            }
            else
            {
                ViewBag.Zaposlen = "true";
            }
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Uloga");
            HttpContext.Session.Remove("User");
            return RedirectToAction("Index", "Home", new { odjava = true });
        }

        public IActionResult NotAuthorized()
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
            return View("NotAuthorized");
        }

        public IActionResult UserHome()
        {
            var uloga = HttpContext.Session.GetString("Uloga");
            if (string.IsNullOrEmpty(uloga))
            {
                return RedirectToAction("NotAuthorized");
            }
            else if (uloga == "Administrator")
            {
                return RedirectToAction("AdminHome");
            }
            else if (uloga == "Ljekarnik")
            {
                return RedirectToAction("PharmaHome");
            }
            else
            {
                return RedirectToAction("PharmaAdminHome");
            }
        }

    }
}
