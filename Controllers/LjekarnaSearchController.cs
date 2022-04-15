using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedEd.Models;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using MedEd.ViewModels;
using Microsoft.AspNetCore.Http;

namespace MedEd.Controllers
{
    public class LjekarnaSearchController : Controller
    {
        private readonly MedEdContext _context;
        private readonly AppSettings appData;

        public LjekarnaSearchController(MedEdContext context, IOptionsSnapshot<AppSettings> options)
        {
            _context = context;
            appData = options.Value;
            ViewData["SifVrstaLjekarna"] = new SelectList(_context.VrstaLjekarna, "SifraVrstaLjekarna", "OpisVrstaLjekarna");
        }

        public IActionResult Index(bool bezRezultata = false)
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
            var model = new LjekarnaViewModel
            {
                BezRezultata = false
            };

            if (bezRezultata)
            {
                model.BezRezultata = true;
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Search(string nazivLjekarna, string sifVrstaLjekarna, string mjestoLjekarna,
            int page = 1)
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

            if ((string.IsNullOrEmpty(nazivLjekarna)) && (string.IsNullOrEmpty(sifVrstaLjekarna))
                && (string.IsNullOrEmpty(mjestoLjekarna)))
            {
                return View("Pretraga");
            }

            int pagesize = appData.PageSize;

            var ljekarne = _context.Ljekarna
                          .Include(d => d.SifVrstaLjekarnaNavigation)
                          .Include(d => d.SifMjestoNavigation)
                          .AsQueryable();

            int count = ljekarne.Count();

            if (!string.IsNullOrEmpty(nazivLjekarna))
            {
                ljekarne = ljekarne.Where(b => String.Equals(b.NazivLjekarna, nazivLjekarna,
                   StringComparison.OrdinalIgnoreCase));
            }
            if (sifVrstaLjekarna != "Sve vrste")
            {
                ljekarne = ljekarne.Where(b => String.Equals(b.SifVrstaLjekarnaNavigation.OpisVrstaLjekarna
                    , sifVrstaLjekarna,
                   StringComparison.OrdinalIgnoreCase));
            }
            if (mjestoLjekarna != "Sva mjesta")
            {
                ljekarne = ljekarne.Where(b => String.Equals(b.SifMjestoNavigation.NazivMjesto, mjestoLjekarna,
                   StringComparison.OrdinalIgnoreCase));
            }

            if (!ljekarne.Any())
            {
                return RedirectToAction("Index", new { bezRezultata = true });
            }
            else
            {
                var ljekarna2 = ljekarne
                      .Select(m => new LjekarnaAdminViewModel
                      {
                          SifraLjekarna = m.SifraLjekarna,
                          NazivLjekarna = m.NazivLjekarna,
                          AdresaLjekarna = m.AdresaLjekarna,
                          KontaktBroj = m.KontaktBroj,
                          Mjesto = m.SifMjestoNavigation.NazivMjesto,
                          VrstaLjekarna = m.SifVrstaLjekarnaNavigation.OpisVrstaLjekarna,
                          Email = m.EmailLjekarna,
                          RadnoVrijeme = m.RadnoVrijeme

                      }).Skip((page - 1) * pagesize)
                    .Take(pagesize)
                    .ToList();

                int count2 = ljekarne.Count();

                var pagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pagesize,
                    TotalItems = count2
                };

                if (page < 1)
                {
                    page = 1;
                }
                else if (page > pagingInfo.TotalPages)
                {
                    return RedirectToAction(nameof(Search), new { page = pagingInfo.TotalPages });
                }

                var model = new LjekarnaViewModel
                {
                    Ljekarne = ljekarna2,
                    PagingInfo = pagingInfo,
                    NazivLjekarne = nazivLjekarna,
                    MjestoLjekarne = mjestoLjekarna,
                    VrstaLjekarne = sifVrstaLjekarna
                };

                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Details(int id, string naziv, string mjesto, string vrsta, int page = 1)
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
            var ljekarna = _context.Ljekarna
                             .Include(d => d.SifMjestoNavigation)
                             .Include(d => d.SifVrstaLjekarnaNavigation)
                             .Where(d => d.SifraLjekarna == id)
                             .SingleOrDefault();

            if (ljekarna == null)
            {
                return NotFound("Ne postoji ljekarna s oznakom: " + id);
            }
            else
            {
                ViewBag.Naziv = naziv;
                ViewBag.Mjesto = mjesto;
                ViewBag.Vrsta = vrsta;
                ViewBag.Page = page;
                return View(ljekarna);
            }
        }
    }

}
