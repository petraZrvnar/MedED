using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedEd.Models;
using MedEd.ViewModels;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;

namespace MedEd.Controllers
{
    public class MedProizvodSearchController : Controller
    {
        private readonly MedEdContext _context;
        private readonly AppSettings appData;
        public string Naziv;
        public string Namjena;
        public string KatBr;
        public string Pro;
        public string Klasa;

        public MedProizvodSearchController(MedEdContext context, IOptionsSnapshot<AppSettings> options)
        {
            _context = context;
            appData = options.Value;
        }

        // Stranica za unos kriterija pretrage
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
            var model = new MedProizvodViewModel
            {
                BezRezultata = false
            };

            if (bezRezultata)
            {
                model.BezRezultata = true;
            }

            return View(model);
        }


        public IActionResult Search(string nazivProizvoda, string namjena, 
            string klasaRizik, string kataloskiBroj, string proizvodjac, int page = 1)
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
            if ((string.IsNullOrEmpty(nazivProizvoda)) && (string.IsNullOrEmpty(namjena))
                  && (string.IsNullOrEmpty(klasaRizik)) && (string.IsNullOrEmpty(kataloskiBroj)) &&
                  (string.IsNullOrEmpty(proizvodjac)))
            {
                return View("Index");
            }

            int pagesize = appData.PageSize;

            var proizvodi = _context.MedicinskiProizvod
                            .Include(d => d.SifKlasaNavigation)
                            .Include(d => d.SifProizvodjacNavigation)
                            .AsQueryable();

            int count = proizvodi.Count();

            if (!string.IsNullOrEmpty(nazivProizvoda))
            {
                proizvodi = proizvodi.Where(b => String.Equals(b.NazivMedProizvod, nazivProizvoda,
                   StringComparison.OrdinalIgnoreCase));
            }
            if (!string.IsNullOrEmpty(namjena))
            {
                proizvodi = proizvodi.Where(b => b.Namjena.IndexOf(namjena, StringComparison.OrdinalIgnoreCase) >= 0);
            }
            if (!string.IsNullOrEmpty(klasaRizik) && klasaRizik != "Sve klase")
            {
                proizvodi = proizvodi.Where(b => String.Equals(b.SifKlasaNavigation.OznKlasaRizika, klasaRizik,
                   StringComparison.OrdinalIgnoreCase));
            }
            if (!string.IsNullOrEmpty(kataloskiBroj))
            {
                proizvodi = proizvodi.Where(b => String.Equals(b.KataloskiBroj, kataloskiBroj,
                   StringComparison.OrdinalIgnoreCase));
            }
            if (!string.IsNullOrEmpty(proizvodjac) && proizvodjac != "Svi proizvođači")
            {
                proizvodi = proizvodi.Where(b => String.Equals(b.SifProizvodjacNavigation.NazivProizvodjac, proizvodjac,
                   StringComparison.OrdinalIgnoreCase));
            }

            if (!proizvodi.Any())
            {
                return RedirectToAction("Index", new { bezRezultata = true });

            }
            else
            {
                var proizvodi2 = proizvodi
                      .Select(m => new MedProViewModel
                      {
                          SifraMedProizvod = m.SifraMedProizvod,
                          NazivMedProizvod = m.NazivMedProizvod,
                          KataloskiBroj = m.KataloskiBroj,
                          Namjena = m.Namjena,
                          Count = m.MedProizvodLjekarna.Count(),
                          SifKlasa = m.SifKlasa,
                          Klasa = m.SifKlasaNavigation.OznKlasaRizika,
                          SifProizvodjac = m.SifProizvodjac,
                          Proizvodjac = m.SifProizvodjacNavigation.NazivProizvodjac

                      }).Skip((page - 1) * pagesize)
                    .Take(pagesize)
                    .ToList();

                int count2 = proizvodi.Count();

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

                var model = new MedProizvodViewModel
                {
                    MedProizvodi = proizvodi2,
                    PagingInfo = pagingInfo,
                    NazivProizvoda = nazivProizvoda,
                    Namjena = namjena,
                    KlasaRizika = klasaRizik,
                    KataloskiBroj = kataloskiBroj,
                    Proizvodjac = proizvodjac
                };

                if (!string.IsNullOrEmpty(nazivProizvoda))
                {
                    HttpContext.Session.SetString("NazivMed", nazivProizvoda);
                }
                else
                {
                    HttpContext.Session.SetString("NazivMed", "");
                }

                if (!string.IsNullOrEmpty(namjena))
                {
                    HttpContext.Session.SetString("Namjena", namjena);
                }
                else
                {
                    HttpContext.Session.SetString("Namjena", "");
                }
                if (!string.IsNullOrEmpty(klasaRizik))
                {
                    HttpContext.Session.SetString("Klasa", klasaRizik);
                }
                else
                {
                    HttpContext.Session.SetString("Klasa", "");
                }
                if (!string.IsNullOrEmpty(proizvodjac))
                {
                    HttpContext.Session.SetString("Pro", proizvodjac);
                }
                else
                {
                    HttpContext.Session.SetString("Pro", "");
                }
                if (!string.IsNullOrEmpty(kataloskiBroj))
                {
                    HttpContext.Session.SetString("KatBr", kataloskiBroj);
                }
                else
                {
                    HttpContext.Session.SetString("KatBr", "");
                }
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Details(int id, int page = 1)
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
            var proizvod = _context.MedicinskiProizvod
                             .Include(d => d.SifKlasaNavigation)
                             .Include(d => d.SifProizvodjacNavigation)
                             .Where(d => d.SifraMedProizvod == id)
                             .SingleOrDefault();

            if (proizvod == null)
            {
                return NotFound("Ne postoji proizvod s oznakom: " + id);
            }
            else
            {
                ViewBag.Naziv = HttpContext.Session.GetString("NazivMed");
                ViewBag.Namjena = HttpContext.Session.GetString("Namjena");
                ViewBag.Pro = HttpContext.Session.GetString("Pro");
                ViewBag.KatBr = HttpContext.Session.GetString("KatBr");
                ViewBag.Klasa = HttpContext.Session.GetString("Klasa");
                ViewBag.Page = page;
                return View(proizvod);
            }
        }

        [HttpGet]
        public IActionResult DetailsPrices(int id, int page = 1)
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
            var proizvod = _context.MedicinskiProizvod
                             .Include(d => d.SifKlasaNavigation)
                             .Include(d => d.SifProizvodjacNavigation)
                             .Where(d => d.SifraMedProizvod == id)
                             .SingleOrDefault();

            if (proizvod == null)
            {
                return NotFound("Ne postoji proizvod s oznakom: " + id);
            }
            else
            {
                ViewBag.Page = page;
                return View(proizvod);
            }
        }

        [HttpGet]
        public IActionResult LjekarnaDetails(int id, int medId, int page = 1)
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
                ViewBag.Med = medId;
                ViewBag.Page = page;
                return View(ljekarna);
            }
        }

        [HttpGet]
        public IActionResult DetailsLjekarnaSearch(int id, int medId, string naziv, string mjesto, string cijena, int page = 1)
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
                ViewBag.Cijena = cijena;
                ViewBag.Med = medId;
                ViewBag.Page = page;
                return View(ljekarna);
            }
        }

        [HttpGet]
        public IActionResult DetailsPricesSearch(int id, string naziv, string mjesto, string cijena, int page = 1)
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
            var proizvod = _context.MedicinskiProizvod
                             .Include(d => d.SifKlasaNavigation)
                             .Include(d => d.SifProizvodjacNavigation)
                             .Where(d => d.SifraMedProizvod == id)
                             .SingleOrDefault();

            if (proizvod == null)
            {
                return NotFound("Ne postoji proizvod s oznakom: " + id);
            }
            else
            {
                ViewBag.Naziv = naziv;
                ViewBag.Mjesto = mjesto;
                ViewBag.Cijena = cijena;
                ViewBag.Page = page;
                return View(proizvod);
            }
        }

        [HttpGet]
        public IActionResult Prices(int id,  
            int page = 1, bool bezRezultata = false)
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
            int pagesize = appData.PageSize;

            var proizvodi = _context.MedProizvodLjekarna
                            .Include(d => d.SifLjekarnaNavigation)
                            .Include(d => d.SifMedProizvodNavigation)
                            .Where(d => d.SifMedProizvod == id);
            if (!proizvodi.Any())
            {
                var modelBez = new MedLjekarneViewModel
                {
                    BezCijene = true
                };
                return View(modelBez);
            }
            int count = proizvodi.Count();

            var pagingInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = pagesize,
                TotalItems = count
            };

            if (page > pagingInfo.TotalPages)
            {
                return RedirectToAction(nameof(Index), new { page = pagingInfo.TotalPages });
            }
            var pro = proizvodi
                   .Select(m => new MedLjekarnaViewModel
                   {
                      SifMedProizvod = m.SifMedProizvod,
                      SifLjekarna = m.SifLjekarna,
                      NazivLjekarna = m.SifLjekarnaNavigation.NazivLjekarna,
                      NazivProizvod = m.SifMedProizvodNavigation.NazivMedProizvod,
                      AdresaLjekarna = m.SifLjekarnaNavigation.AdresaLjekarna,
                      Kontakt = m.SifLjekarnaNavigation.KontaktBroj,
                      MjestoLjekarna = m.SifLjekarnaNavigation.SifMjestoNavigation.NazivMjesto,
                      CijenaMedProizvod = m.CijenaMedProizvod,
                      DostupnostMedProizvod = m.DostupnostMedProizvod
                   })
                   .Skip((page - 1) * pagesize)
                   .Take(pagesize)
                   .ToList();
            var naz = _context.MedicinskiProizvod.Where(d => d.SifraMedProizvod == id).SingleOrDefault();
            string naziv = naz.NazivMedProizvod;
            HttpContext.Session.SetString("PuniNazivMed", naziv);
            var model = new MedLjekarneViewModel
            {
                Proizvodi = pro,
                PagingInfo = pagingInfo,
                Proizvod = id,
                NazivProizvod = naziv,
            };

            if (bezRezultata)
            {
                model.BezRezultata = true;
            }

            ViewBag.Naziv = HttpContext.Session.GetString("NazivMed");
            ViewBag.Namjena = HttpContext.Session.GetString("Namjena");
            ViewBag.Pro = HttpContext.Session.GetString("Pro");
            ViewBag.KatBr = HttpContext.Session.GetString("KatBr");
            ViewBag.Klasa = HttpContext.Session.GetString("Klasa");
            return View(model);
        }

        [HttpGet]
        public IActionResult PricesSearch(int id, 
            string naziv, string mjesto, string cijena, int page = 1)
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
            int pagesize = appData.PageSize;
            var query = _context.MedProizvodLjekarna
                            .Include(d => d.SifLjekarnaNavigation)
                            .Include(d => d.SifMedProizvodNavigation)
                            .Where(d => d.SifMedProizvod == id);

            int count = query.Count();

            if (!string.IsNullOrEmpty(naziv))
            {
                query = query.Where(b => String.Equals(b.SifLjekarnaNavigation.NazivLjekarna, naziv,
                   StringComparison.OrdinalIgnoreCase));
            }

            if(mjesto != "Sva mjesta")
            {
                query = query.Where(b => String.Equals(b.SifLjekarnaNavigation.SifMjestoNavigation.NazivMjesto, mjesto,
                   StringComparison.OrdinalIgnoreCase));
            }

            if (cijena == "Rastuća" || cijena == "--")
            {
                query = query.OrderBy(d => d.CijenaMedProizvod);
            }
            else
            {
                query = query.OrderByDescending(d => d.CijenaMedProizvod);
            }

            if (!query.Any())
            {
                return RedirectToAction("Prices", new { id = id, bezRezultata = true });
            }
            else
            {
                var pro = query
                  .Select(m => new MedLjekarnaViewModel
                  {
                      SifMedProizvod = m.SifMedProizvod,
                      SifLjekarna = m.SifLjekarna,
                      NazivLjekarna = m.SifLjekarnaNavigation.NazivLjekarna,
                      NazivProizvod = m.SifMedProizvodNavigation.NazivMedProizvod,
                      AdresaLjekarna = m.SifLjekarnaNavigation.AdresaLjekarna,
                      Kontakt = m.SifLjekarnaNavigation.KontaktBroj,
                      MjestoLjekarna = m.SifLjekarnaNavigation.SifMjestoNavigation.NazivMjesto,
                      CijenaMedProizvod = m.CijenaMedProizvod,
                      DostupnostMedProizvod = m.DostupnostMedProizvod
                  })
                  .Skip((page - 1) * pagesize)
                  .Take(pagesize)
                  .ToList();

                int count2 = query.Count();

                var pagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pagesize,
                    TotalItems = count2
                };
                if (page > pagingInfo.TotalPages)
                {
                    return RedirectToAction(nameof(Index), new { page = pagingInfo.TotalPages });
                }

                var model = new MedLjekarneViewModel
                {
                    Proizvodi = pro,
                    PagingInfo = pagingInfo,
                    Mjesto = mjesto,
                    Naziv = naziv,
                    Cijena = cijena,
                    Proizvod = id,
                };
                ViewBag.PuniNaz = HttpContext.Session.GetString("PuniNazivMed");
                ViewBag.Naziv = HttpContext.Session.GetString("NazivMed");
                ViewBag.Namjena = HttpContext.Session.GetString("Namjena");
                ViewBag.Pro = HttpContext.Session.GetString("Pro");
                ViewBag.KatBr = HttpContext.Session.GetString("KatBr");
                ViewBag.Klasa = HttpContext.Session.GetString("Klasa");
                return View(model);
            }
        }
      
    }
}
