using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MedEd.Models;
using Microsoft.Extensions.Options;
using MedEd.ViewModels;
using Microsoft.AspNetCore.Http;

namespace MedEd.Controllers
{
    public class LijekSearchController : Controller
    {
        private readonly MedEdContext _context;
        private readonly AppSettings appData;

        public LijekSearchController(MedEdContext context, IOptionsSnapshot<AppSettings> options)
        {
            _context = context;
            appData = options.Value;
        }

        public ActionResult Index(bool bezRezultata = false)
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
            var model = new LijekSearchViewModel
            {
                BezRezultata = false
            };

            if (bezRezultata)
            {
                model.BezRezultata = true;
            }

            return View(model);
        }

        public IActionResult Search(string naziv, string brOd, string vrsta, string tvar,
            int? kolicina, string jedinica, string oblik, string pro, int? god, int page = 1)
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
            if (string.IsNullOrEmpty(naziv) && string.IsNullOrEmpty(brOd) && string.IsNullOrEmpty(vrsta)
                && string.IsNullOrEmpty(tvar) && (kolicina == null) && string.IsNullOrEmpty(jedinica)
                && string.IsNullOrEmpty(oblik) && string.IsNullOrEmpty(pro) && (god == null))
            {
                return View("Index");
            }

            int pagesize = appData.PageSize;
            var lijekovi = _context.Lijek
                           .Include(d => d.SifDjelatnaTvarNavigation)
                           .Include(d => d.SifFarmOblikNavigation)
                           .Include(d => d.SifNositeljNavigation)
                           .Include(d => d.SifProizvodjacNavigation)
                           .Include(d => d.SifVrstaLijekNavigation)
                           .AsQueryable();
            int count = lijekovi.Count();

            if (!string.IsNullOrEmpty(naziv))
            {
                lijekovi = lijekovi.Where(d => d.NazivLijek.IndexOf(naziv, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            if (!string.IsNullOrEmpty(vrsta) && vrsta != "Sve vrste")
            {
                lijekovi = lijekovi.Where(d => String.Equals(d.SifVrstaLijekNavigation.OpisVrstaLijek, vrsta,
                   StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(brOd))
            {
                lijekovi = lijekovi.Where(d => String.Equals(d.BrojOdobrenja, brOd,
                  StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(tvar))
            {
                lijekovi = lijekovi.Where(d => String.Equals(d.SifDjelatnaTvarNavigation.NazivDjelatnaTvar, tvar,
                  StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(jedinica) && jedinica != "--")
            {
                lijekovi = lijekovi.Where(d => String.Equals(d.Jedinica, jedinica,
                  StringComparison.OrdinalIgnoreCase));
            }

            if (kolicina != null)
            {
                lijekovi = lijekovi.Where(d => d.Kolicina == kolicina);
            }

            if (!string.IsNullOrEmpty(pro) && pro != "Svi proizvođači")
            {
                lijekovi = lijekovi.Where(d => String.Equals(d.SifProizvodjacNavigation.NazivProizvodjac, pro,
                  StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(oblik) && oblik != "Svi oblici")
            {
                lijekovi = lijekovi.Where(d => String.Equals(d.SifFarmOblikNavigation.OpisFarmOblik, oblik,
                  StringComparison.OrdinalIgnoreCase));
            }
            if (god != null)
            {
                lijekovi = lijekovi.Where(d => d.GodStavljanjaTrziste == god);
            }

            if (!lijekovi.Any())
            {
                return RedirectToAction("Index", new { bezRezultata = true });
            }

            var lijekovi2 = lijekovi
                 .Select(m => new LijekViewModel
                 {
                     SifraLijek = m.SifraLijek,
                     NazivLijek = m.NazivLijek,
                     BrojOdobrenja = m.BrojOdobrenja,
                     GodStavljanjaTrziste = m.GodStavljanjaTrziste,
                     NacinIzdavanja = m.NacinIzdavanja,
                     NaTrzistu = m.NaTrzistu,
                     Kolicina = m.Kolicina,
                     Jedinica = m.Jedinica,
                     FarmOblik = m.SifFarmOblikNavigation.OpisFarmOblik,
                     VrstaLijek = m.SifVrstaLijekNavigation.OpisVrstaLijek,
                     Nositelj = m.SifNositeljNavigation.NazivNositelj,
                     Proizvodjac = m.SifProizvodjacNavigation.NazivProizvodjac,
                     DjelatnaTvar = m.SifDjelatnaTvarNavigation.NazivDjelatnaTvar,
                     Count = m.LijekLjekarna.Count()
                 })
                 .Skip((page - 1) * pagesize)
                 .Take(pagesize)
                 .ToList();

            int count2 = lijekovi.Count();

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
            var model = new LijekSearchViewModel
            {
                Lijekovi = lijekovi2,
                PagingInfo = pagingInfo,
                Naziv = naziv,
                Vrsta = vrsta,
                Br = brOd,
                Tvar = tvar,
                Jedinica = jedinica,
                Kolicina = kolicina,
                Pro = pro,
                Oblik = oblik,
                God = god
            };

            if (!string.IsNullOrEmpty(naziv))
            {
                HttpContext.Session.SetString("NazivLijek", naziv);
            }
            else
            {
                HttpContext.Session.SetString("NazivLijek", "");
            }
            if (!string.IsNullOrEmpty(brOd))
            {
                HttpContext.Session.SetString("BrOd", brOd);
            }
            else
            {
                HttpContext.Session.SetString("BrOd", "");
            }
            if (!string.IsNullOrEmpty(vrsta))
            {
                HttpContext.Session.SetString("Vrsta", vrsta);
            }
            else
            {
                HttpContext.Session.SetString("Vrsta", "");
            }
            if (!string.IsNullOrEmpty(tvar))
            {
                HttpContext.Session.SetString("Tvar", tvar);
            }
            else
            {
                HttpContext.Session.SetString("Tvar", "");
            }
            if (kolicina != null)
            {
                string kol = kolicina.ToString();
                HttpContext.Session.SetString("Kolicina", kol);
            }
            else
            {
                HttpContext.Session.SetString("Kolicina", "");
            }
            if (!string.IsNullOrEmpty(jedinica))
            {
                HttpContext.Session.SetString("Jedinica", jedinica);
            }
            else
            {
                HttpContext.Session.SetString("Jedinica", "");
            }
            if (!string.IsNullOrEmpty(oblik))
            {
                HttpContext.Session.SetString("Oblik", oblik);
            }
            else
            {
                HttpContext.Session.SetString("Oblik", "");
            }
            if (!string.IsNullOrEmpty(pro))
            {
                HttpContext.Session.SetString("ProLijek", pro);
            }
            else
            {
                HttpContext.Session.SetString("ProLijek", "");
            }
            if (god != null)
            {
                string godina = god.ToString();
                HttpContext.Session.SetString("God", godina);
            }
            else
            {
                HttpContext.Session.SetString("God", "");
            }
            return View(model);
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

            var lijek = _context.Lijek
                           .Include(d => d.SifDjelatnaTvarNavigation)
                           .Include(d => d.SifFarmOblikNavigation)
                           .Include(d => d.SifNositeljNavigation)
                           .Include(d => d.SifProizvodjacNavigation)
                           .Include(d => d.SifVrstaLijekNavigation)
                           .Where(d => d.SifraLijek == id)
                           .SingleOrDefault();
            if (lijek == null)
            {
                return NotFound("Ne postoji lijek s oznakom: " + id);
            }
            ViewBag.Naziv = HttpContext.Session.GetString("NazivLijek");
            ViewBag.Vrsta = HttpContext.Session.GetString("Vrsta");
            ViewBag.Br = HttpContext.Session.GetString("BrOd");
            ViewBag.Tvar = HttpContext.Session.GetString("Tvar");
            ViewBag.Jedinica = HttpContext.Session.GetString("Jedinica");
            ViewBag.Pro = HttpContext.Session.GetString("ProLijek");
            ViewBag.Oblik = HttpContext.Session.GetString("Oblik");
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Kolicina")))
            {
                ViewBag.Kolicina = Convert.ToInt32(HttpContext.Session.GetString("Kolicina"));
            }
            else
            {
                ViewBag.Kolicina = HttpContext.Session.GetString("Kolicina");
            }
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("God")))
            {
                ViewBag.God = Convert.ToInt32(HttpContext.Session.GetString("God"));
            }
            else
            {
                ViewBag.God = HttpContext.Session.GetString("God");
            }
            ViewBag.Page = page;
            return View(lijek);
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

            var lijek = _context.Lijek
                           .Include(d => d.SifDjelatnaTvarNavigation)
                           .Include(d => d.SifFarmOblikNavigation)
                           .Include(d => d.SifNositeljNavigation)
                           .Include(d => d.SifProizvodjacNavigation)
                           .Include(d => d.SifVrstaLijekNavigation)
                           .Where(d => d.SifraLijek == id)
                           .SingleOrDefault();
            if (lijek == null)
            {
                return NotFound("Ne postoji lijek s oznakom: " + id);
            }
            ViewBag.Page = page;
            return View(lijek);
        }

        [HttpGet]
        public IActionResult DetailsLjekarna(int id, int lijekId, int page = 1)
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
                ViewBag.Lijek = lijekId;
                ViewBag.Page = page;
                return View(ljekarna);
            }
        }

        [HttpGet]
        public IActionResult DetailsLjekarnaSearch(int id, int lijekId, string naziv, string mjesto, 
            string cijena, int page = 1)
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
                ViewBag.Lijek = lijekId;
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
            var lijek = _context.Lijek
                          .Include(d => d.SifDjelatnaTvarNavigation)
                          .Include(d => d.SifFarmOblikNavigation)
                          .Include(d => d.SifNositeljNavigation)
                          .Include(d => d.SifProizvodjacNavigation)
                          .Include(d => d.SifVrstaLijekNavigation)
                          .Where(d => d.SifraLijek == id)
                          .SingleOrDefault();
            if (lijek == null)
            {
                return NotFound("Ne postoji lijek s oznakom: " + id);
            }
            else
            {
                ViewBag.Naziv = naziv;
                ViewBag.Mjesto = mjesto;
                ViewBag.Cijena = cijena;
                ViewBag.Page = page;
                return View(lijek);
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

            var lijekovi = _context.LijekLjekarna
                         .Include(d => d.SifLijekNavigation)
                         .Include(d => d.SifLjekarnaNavigation)
                         .Where(d => d.SifLijek == id);

            if (!lijekovi.Any())
            {
                var modelBez = new LijekoviLjekarnaViewModel
                {
                    BezCijene = true
                };
                return View(modelBez);
            }
            int count = lijekovi.Count();

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

            var lijekovi2 = lijekovi
                          .Select(m => new LijekLjekarnaViewModel
                          {
                              SifLijek = m.SifLijek,
                              SifLjekarna = m.SifLjekarna,
                              NazivLjekarna = m.SifLjekarnaNavigation.NazivLjekarna,
                              NazivLijek = m.SifLijekNavigation.NazivLijek,
                              AdresaLjekarna = m.SifLjekarnaNavigation.AdresaLjekarna,
                              Kontakt = m.SifLjekarnaNavigation.KontaktBroj,
                              MjestoLjekarna = m.SifLjekarnaNavigation.SifMjestoNavigation.NazivMjesto,
                              CijenaLijek = m.CijenaLijek,
                              DostupnostLijek = m.DostupnostLijek
                          })
                          .Skip((page - 1) * pagesize)
                          .Take(pagesize)
                          .ToList();
            var naz = _context.Lijek.Where(d => d.SifraLijek == id).SingleOrDefault();
            string naziv = naz.NazivLijek;
            HttpContext.Session.SetString("PuniNazivLijek", naziv);
            var model = new LijekoviLjekarnaViewModel
            {
                Lijekovi = lijekovi2,
                PagingInfo = pagingInfo,
                Lijek = id,
                NazivLijek = naziv
            };

            if (bezRezultata)
            {
                model.BezRezultata = true;
            }
            ViewBag.Naziv = HttpContext.Session.GetString("NazivLijek");
            ViewBag.Vrsta = HttpContext.Session.GetString("Vrsta");
            ViewBag.Br = HttpContext.Session.GetString("BrOd");
            ViewBag.Tvar = HttpContext.Session.GetString("Tvar");
            ViewBag.Jedinica = HttpContext.Session.GetString("Jedinica");
            ViewBag.Pro = HttpContext.Session.GetString("ProLijek");
            ViewBag.Oblik = HttpContext.Session.GetString("Oblik");
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Kolicina")))
            {
                ViewBag.Kolicina = Convert.ToInt32(HttpContext.Session.GetString("Kolicina"));
            }
            else
            {
                ViewBag.Kolicina = HttpContext.Session.GetString("Kolicina");
            }
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("God")))
            {
                ViewBag.God = Convert.ToInt32(HttpContext.Session.GetString("God"));
            }
            else
            {
                ViewBag.God = HttpContext.Session.GetString("God");
            }
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

            var query = _context.LijekLjekarna
                         .Include(d => d.SifLijekNavigation)
                         .Include(d => d.SifLjekarnaNavigation)
                         .Where(d => d.SifLijek == id);

            int count = query.Count();

            if (!string.IsNullOrEmpty(naziv))
            {
                query = query.Where(b => String.Equals(b.SifLjekarnaNavigation.NazivLjekarna, naziv,
                   StringComparison.OrdinalIgnoreCase));
            }

            if (mjesto != "Sva mjesta")
            {
                query = query.Where(b => String.Equals(b.SifLjekarnaNavigation.SifMjestoNavigation.NazivMjesto, mjesto,
                   StringComparison.OrdinalIgnoreCase));
            }

            if (cijena == "Rastuća" || cijena == "--")
            {
                query = query.OrderBy(d => d.CijenaLijek);
            }
            else
            {
                query = query.OrderByDescending(d => d.CijenaLijek);
            }
            if (!query.Any())
            {
                return RedirectToAction("Prices", new { id = id, bezRezultata = true });
            }
            var lijekovi2 = query
                          .Select(m => new LijekLjekarnaViewModel
                          {
                              SifLijek = m.SifLijek,
                              SifLjekarna = m.SifLjekarna,
                              NazivLjekarna = m.SifLjekarnaNavigation.NazivLjekarna,
                              NazivLijek = m.SifLijekNavigation.NazivLijek,
                              AdresaLjekarna = m.SifLjekarnaNavigation.AdresaLjekarna,
                              Kontakt = m.SifLjekarnaNavigation.KontaktBroj,
                              MjestoLjekarna = m.SifLjekarnaNavigation.SifMjestoNavigation.NazivMjesto,
                              CijenaLijek = m.CijenaLijek,
                              DostupnostLijek = m.DostupnostLijek
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
            var model = new LijekoviLjekarnaViewModel
            {
                Lijekovi = lijekovi2,
                PagingInfo = pagingInfo,
                Mjesto = mjesto,
                Naziv = naziv,
                Cijena = cijena,
                Lijek = id
            };
            ViewBag.PuniNaziv = HttpContext.Session.GetString("PuniNazivLijek");
            ViewBag.Naziv = HttpContext.Session.GetString("NazivLijek");
            ViewBag.Vrsta = HttpContext.Session.GetString("Vrsta");
            ViewBag.Br = HttpContext.Session.GetString("BrOd");
            ViewBag.Tvar = HttpContext.Session.GetString("Tvar");
            ViewBag.Jedinica = HttpContext.Session.GetString("Jedinica");
            ViewBag.Pro = HttpContext.Session.GetString("ProLijek");
            ViewBag.Oblik = HttpContext.Session.GetString("Oblik");
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("Kolicina")))
            {
                ViewBag.Kolicina = Convert.ToInt32(HttpContext.Session.GetString("Kolicina"));
            }
            else
            {
                ViewBag.Kolicina = HttpContext.Session.GetString("Kolicina");
            }
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("God")))
            {
                ViewBag.God = Convert.ToInt32(HttpContext.Session.GetString("God"));
            }
            else
            {
                ViewBag.God = HttpContext.Session.GetString("God");
            }
            return View(model);

        }
    }
}
