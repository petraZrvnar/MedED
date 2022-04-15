using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MedEd.Models;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using MedEd.ViewModels;
using System.Text;

namespace MedEd.Controllers
{
    public class LijekLjekarnaController : Controller
    {
        private readonly MedEdContext _context;
        private readonly AppSettings appData;

        public LijekLjekarnaController(MedEdContext context, IOptionsSnapshot<AppSettings> options)
        {
            _context = context;
            appData = options.Value;
        }

        [HttpGet]
        public IActionResult Index(string ex, string poruka, string porukaDod,
            bool ažurirano = false, bool greskaEdit = false,
            int page = 1, bool obrisano = false, bool greska = false,
            bool dodano = false, bool bezRezultata = false,
            bool greskaDod = false)
        {
            var uloga = HttpContext.Session.GetString("Uloga");
            if (uloga == null || (uloga != "Ljekarnik" && uloga != "Ljekarnik Admin"))
            {
                return RedirectToAction("NotAuthorized", "Login");
            }

            var sifra = HttpContext.Session.GetString("Ljekarna");
            if (string.IsNullOrEmpty(sifra))
            {
                return RedirectToAction("PharmaHome", "Login");
            }

            int sifraLjekarna = Convert.ToInt32(sifra);
            int pagesize = appData.PageSize;

            var lijekovi = _context.LijekLjekarna
                           .Include(d => d.SifLijekNavigation)
                           .Include(d => d.SifLjekarnaNavigation)
                           .Where(d => d.SifLjekarna == sifraLjekarna)
                           .AsQueryable();

            int count = lijekovi.Count();
            var pagingInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = pagesize,
                TotalItems = count
            };

            if (page > pagingInfo.TotalPages)
            {
                return RedirectToAction(nameof(Index), new { page = pagingInfo.TotalPages, ex, poruka, obrisano });
            }

            var lijekovi2 = lijekovi
                         .Select(m => new LijekLjekarnaViewModel
                         {
                             SifLijek = m.SifLijek,
                             SifLjekarna = m.SifLjekarna,
                             NazivLjekarna = m.SifLjekarnaNavigation.NazivLjekarna,
                             NazivLijek = m.SifLijekNavigation.NazivLijek,
                             DjelatnaTvar = m.SifLijekNavigation.SifDjelatnaTvarNavigation.NazivDjelatnaTvar,
                             Vrsta = m.SifLijekNavigation.SifVrstaLijekNavigation.OpisVrstaLijek,
                             BrOd = m.SifLijekNavigation.BrojOdobrenja,
                             CijenaLijek = m.CijenaLijek,
                             DostupnostLijek = m.DostupnostLijek
                         })
                         .Skip((page - 1) * pagesize)
                         .Take(pagesize)
                         .ToList();
            var ljekarna = _context.Ljekarna.Where(d => d.SifraLjekarna == sifraLjekarna).SingleOrDefault();
            string nazivLjekarna = ljekarna.NazivLjekarna;
            var model = new LijekLjekarnaAdminViewModel
            {
                Lijekovi = lijekovi2,
                PagingInfo = pagingInfo,
                Ljekarna = sifraLjekarna,
                NazivLjekarna = nazivLjekarna
            };

            if (bezRezultata)
            {
                model.BezRezultata = true;
                model.Naziv = "";
                model.Vrsta = "";
                model.Br = "";
                model.Tvar = "";
                model.Dostupnost = "";
                model.Cijena = "";
            }

            if (ažurirano)
            {
                model.Azurirano = true;
            }
            if (obrisano)
            {
                model.Obrisano = true;
            }
            if (greska)
            {
                model.Greska = true;
            }
            if (dodano)
            {
                model.Dodano = true;
            }
            if (greskaDod)
            {
                model.GreskaDodavanje = true;
            }
            if (greskaEdit)
            {
                model.GreskaEdit = true;
            }
            if (!string.IsNullOrEmpty(porukaDod))
            {
                model.PorukaDodavanje = porukaDod;
            }
            if (!string.IsNullOrEmpty(poruka))
            {
                model.Poruka = poruka;
            }
            if (!string.IsNullOrEmpty(ex))
            {
                model.Exc = ex;
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Search(string naziv, string br, string tvar, string dostupnost, 
            string vrsta, string cijena, int page = 1)
        {
            var uloga = HttpContext.Session.GetString("Uloga");
            if (uloga == null || (uloga != "Ljekarnik" && uloga != "Ljekarnik Admin"))
            {
                return RedirectToAction("NotAuthorized", "Login");
            }

            var sifra = HttpContext.Session.GetString("Ljekarna");
            if (string.IsNullOrEmpty(sifra))
            {
                return RedirectToAction("PharmaHome", "Login");
            }

            int sifraLjekarna = Convert.ToInt32(sifra);
            int pagesize = appData.PageSize;

            var lijekovi = _context.LijekLjekarna
                           .Include(d => d.SifLijekNavigation)
                           .Include(d => d.SifLjekarnaNavigation)
                           .Where(d => d.SifLjekarna == sifraLjekarna)
                           .AsQueryable();

            int count = lijekovi.Count();

            if (!string.IsNullOrEmpty(naziv))
            {
                lijekovi = lijekovi.Where(d => d.SifLijekNavigation.NazivLijek.IndexOf
                (naziv, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            if (!string.IsNullOrEmpty(br))
            {
                lijekovi = lijekovi.Where(d => String.Equals(d.SifLijekNavigation.BrojOdobrenja, br,
                  StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(tvar))
            {
                lijekovi = lijekovi.Where(d => String.Equals(d.SifLijekNavigation.SifDjelatnaTvarNavigation.NazivDjelatnaTvar, tvar,
                  StringComparison.OrdinalIgnoreCase));
            }

            if(!string.IsNullOrEmpty(vrsta) && vrsta != "Sve vrste")
            {
                lijekovi = lijekovi.Where(d => String.Equals(d.SifLijekNavigation.SifVrstaLijekNavigation.OpisVrstaLijek, vrsta,
                   StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(dostupnost) && dostupnost != "--")
            {
                lijekovi = lijekovi.Where(d => String.Equals(d.DostupnostLijek, dostupnost, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(cijena) && cijena != "--")
            {
                if (cijena == "Manja od 50kn")
                {
                    lijekovi = lijekovi.Where(d => d.CijenaLijek < 50);
                }
                else if (cijena == "Od 50 do 100kn")
                {
                    lijekovi = lijekovi.Where(d => d.CijenaLijek > 50 && d.CijenaLijek < 100);
                }
                else
                {
                    lijekovi = lijekovi.Where(d => d.CijenaLijek > 100);
                }
            }

            if (!lijekovi.Any())
            {
                return RedirectToAction("Index", new { bezRezultata = true });
            }

            var lijekovi2 = lijekovi
                         .Select(m => new LijekLjekarnaViewModel
                         {
                             SifLijek = m.SifLijek,
                             SifLjekarna = m.SifLjekarna,
                             NazivLjekarna = m.SifLjekarnaNavigation.NazivLjekarna,
                             NazivLijek = m.SifLijekNavigation.NazivLijek,
                             DjelatnaTvar = m.SifLijekNavigation.SifDjelatnaTvarNavigation.NazivDjelatnaTvar,
                             Vrsta = m.SifLijekNavigation.SifVrstaLijekNavigation.OpisVrstaLijek,
                             BrOd = m.SifLijekNavigation.BrojOdobrenja,
                             CijenaLijek = m.CijenaLijek,
                             DostupnostLijek = m.DostupnostLijek
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
            if (page > pagingInfo.TotalPages)
            {
                return RedirectToAction(nameof(Index), new { page = pagingInfo.TotalPages });
            }
            var ljekarna = _context.Ljekarna.Where(d => d.SifraLjekarna == sifraLjekarna).SingleOrDefault();
            string nazivLjekarna = ljekarna.NazivLjekarna;
            var model = new LijekLjekarnaAdminViewModel
            {
                Lijekovi = lijekovi2,
                PagingInfo = pagingInfo,
                Naziv = naziv,
                Br = br,
                Vrsta = vrsta,
                Tvar = tvar,
                Cijena = cijena,
                Dostupnost = dostupnost,
                Ljekarna = sifraLjekarna,
                NazivLjekarna = nazivLjekarna
            };

            return View(model);


        }
        
        [HttpGet]
        public IActionResult DetailsLijek(int id, int page = 1)
        {
            var uloga = HttpContext.Session.GetString("Uloga");
            if (uloga == null || (uloga != "Ljekarnik" && uloga != "Ljekarnik Admin"))
            {
                return RedirectToAction("NotAuthorized", "Login");
            }

            var sifra = HttpContext.Session.GetString("Ljekarna");
            if (string.IsNullOrEmpty(sifra))
            {
                return RedirectToAction("PharmaHome", "Login");
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
                return NotFound("Ne postoji lijek s oznakom:" + id);
            }
            else
            {
                ViewBag.Page = page;
                return View(lijek);
            }
        }

        [HttpGet]
        public IActionResult DetailsLjekarna(int id, int page = 1)
        {
            var uloga = HttpContext.Session.GetString("Uloga");
            if (uloga == null || (uloga != "Ljekarnik" && uloga != "Ljekarnik Admin"))
            {
                return RedirectToAction("NotAuthorized", "Login");
            }

            var sifra = HttpContext.Session.GetString("Ljekarna");
            if (string.IsNullOrEmpty(sifra))
            {
                return RedirectToAction("PharmaHome", "Login");
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
                ViewBag.Page = page;
                return View(ljekarna);
            }
        }

        [HttpGet]
        public IActionResult DetailsSearchLijek(int id, string naziv, string br, string tvar, string dostupnost,
            string vrsta, string cijena, int page = 1)
        {
            var uloga = HttpContext.Session.GetString("Uloga");
            if (uloga == null || (uloga != "Ljekarnik" && uloga != "Ljekarnik Admin"))
            {
                return RedirectToAction("NotAuthorized", "Login");
            }

            var sifra = HttpContext.Session.GetString("Ljekarna");
            if (string.IsNullOrEmpty(sifra))
            {
                return RedirectToAction("PharmaHome", "Login");
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
                return NotFound("Ne postoji lijek s oznakom:" + id);
            }
            else
            {
                ViewBag.Naziv = naziv;
                ViewBag.Br = br;
                ViewBag.Tvar = tvar;
                ViewBag.Dostupnost = dostupnost;
                ViewBag.Vrsta = vrsta;
                ViewBag.Cijena = cijena;
                ViewBag.Page = page;
                return View(lijek);
            }
        }

        [HttpGet]
        public IActionResult DetailsSearchLjekarna(int id, string naziv, string br, string tvar, string dostupnost,
            string vrsta, string cijena, int page = 1)
        {
            var uloga = HttpContext.Session.GetString("Uloga");
            if (uloga == null || (uloga != "Ljekarnik" && uloga != "Ljekarnik Admin"))
            {
                return RedirectToAction("NotAuthorized", "Login");
            }

            var sifra = HttpContext.Session.GetString("Ljekarna");
            if (string.IsNullOrEmpty(sifra))
            {
                return RedirectToAction("PharmaHome", "Login");
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
                ViewBag.Br = br;
                ViewBag.Tvar = tvar;
                ViewBag.Dostupnost = dostupnost;
                ViewBag.Vrsta = vrsta;
                ViewBag.Cijena = cijena;
                ViewBag.Page = page;
                return View(ljekarna);
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            var uloga = HttpContext.Session.GetString("Uloga");
            if (uloga == null || (uloga != "Ljekarnik" && uloga != "Ljekarnik Admin"))
            {
                return RedirectToAction("NotAuthorized", "Login");
            }

            var sifra = HttpContext.Session.GetString("Ljekarna");
            if (string.IsNullOrEmpty(sifra))
            {
                return RedirectToAction("PharmaHome", "Login");
            }
            int sifraLjekarna = Convert.ToInt32(sifra);
            var ljekarna = _context.Ljekarna.Where(d => d.SifraLjekarna == sifraLjekarna).SingleOrDefault();
            ViewBag.Ime = ljekarna.NazivLjekarna;
            ViewData["SifLijek"] = new SelectList(_context.Lijek, "SifraLijek", "NazivLijek");
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("SifLijek,CijenaLijek,DostupnostLijek")] LijekLjekarna lijekLjekarna)
        {
            var uloga = HttpContext.Session.GetString("Uloga");
            if (uloga == null || (uloga != "Ljekarnik" && uloga != "Ljekarnik Admin"))
            {
                return RedirectToAction("NotAuthorized", "Login");
            }

            var sifra = HttpContext.Session.GetString("Ljekarna");
            if (string.IsNullOrEmpty(sifra))
            {
                return RedirectToAction("PharmaHome", "Login");
            }
            int sifraLjekarna = Convert.ToInt32(sifra);
            lijekLjekarna.SifLjekarna = sifraLjekarna;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(lijekLjekarna);
                    _context.SaveChanges();
                    var lijek = _context.Lijek.Where(d => d.SifraLijek == lijekLjekarna.SifLijek).SingleOrDefault();
                    string naziv = lijek.NazivLijek;
                    return RedirectToAction(nameof(Index), new { poruka = naziv, dodano = true });
                }
                catch (Exception ex)
                {
                    string poruka = "Došlo je do pogreške prilikom dodavanja novog lijeka" + "\n Greška: " + ex.InnerException.Message;
                    return RedirectToAction(nameof(Index),
                        new { ex = poruka, greska = true });
                }
            }
            else
            {
                string porukaDodavanje = GreskaPoruka(lijekLjekarna);
                return RedirectToAction("Index", new { greskaEdit = true, porukaDod = porukaDodavanje });
            }

        }

        public IActionResult Edit(int id, int page = 1)
        {
            var uloga = HttpContext.Session.GetString("Uloga");
            if (uloga == null || (uloga != "Ljekarnik" && uloga != "Ljekarnik Admin"))
            {
                return RedirectToAction("NotAuthorized", "Login");
            }

            var sifra = HttpContext.Session.GetString("Ljekarna");
            if (string.IsNullOrEmpty(sifra))
            {
                return RedirectToAction("PharmaHome", "Login");
            }
            int sifraLjekarna = Convert.ToInt32(sifra);

            var lijekLjekarna = _context.LijekLjekarna.Where(d => d.SifLjekarna == sifraLjekarna)
                                                .Where(d => d.SifLijek == id)
                                                .SingleOrDefault();
            if (lijekLjekarna == null)
            {
                return NotFound();
            }

            var lijek = _context.Lijek.Where(d => d.SifraLijek == id).SingleOrDefault();
            ViewBag.Ime = lijek.NazivLijek;
            ViewBag.Page = page;

            return PartialView(lijekLjekarna);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, [Bind("SifLjekarna,SifLijek,DostupnostLijek,CijenaLijek")] LijekLjekarna lijekLjekarna, int page = 1)
        {
            var uloga = HttpContext.Session.GetString("Uloga");
            if (uloga == null || (uloga != "Ljekarnik" && uloga != "Ljekarnik Admin"))
            {
                return RedirectToAction("NotAuthorized", "Login");
            }

            var sifra = HttpContext.Session.GetString("Ljekarna");
            if (string.IsNullOrEmpty(sifra))
            {
                return RedirectToAction("PharmaHome", "Login");
            }
            int sifraLjekarna = Convert.ToInt32(sifra);

            if (ModelState.IsValid)
            {
                try
                {
                    var lijek = _context.Lijek.Where(d => d.SifraLijek == lijekLjekarna.SifLijek).SingleOrDefault();
                    string poruka = lijek.NazivLijek;
                    _context.Update(lijekLjekarna);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index),
                            new
                            {
                                page = page,
                                ažurirano = true,
                                poruka = poruka
                            });
                }
                catch (DbUpdateConcurrencyException exc)
                {
                    string ex = "Došlo je do greške prilikom ažuriranja lijeka: "
                       + "\n Greška: " + exc.InnerException.Message + "Kontaktirajte nas za pomoć";
                    return RedirectToAction(nameof(Index), new { page = page, greska = true, ex = ex });
                }
            }
            else
            {
                string porukaDodavanje = GreskaPoruka(lijekLjekarna);
                return RedirectToAction("Index", new { greskaEdit = true, porukaDod = porukaDodavanje, page = page });
            }

        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, int page = 1)
        {
            var uloga = HttpContext.Session.GetString("Uloga");
            if (uloga == null || (uloga != "Ljekarnik" && uloga != "Ljekarnik Admin"))
            {
                return RedirectToAction("NotAuthorized", "Login");
            }

            var sifra = HttpContext.Session.GetString("Ljekarna");
            if (string.IsNullOrEmpty(sifra))
            {
                return RedirectToAction("PharmaHome", "Login");
            }
            int sifraLjekarna = Convert.ToInt32(sifra);
            var lijekLjekarna = _context.LijekLjekarna
                                     .Where(d => d.SifLijek == id)
                                     .Where(d => d.SifLjekarna == sifraLjekarna)
                                     .SingleOrDefault();
            if (lijekLjekarna != null)
            {
                try
                {
                    var lijek = _context.Lijek.Where(d => d.SifraLijek == id).SingleOrDefault();
                    string naziv = lijek.NazivLijek;
                    _context.Remove(lijekLjekarna);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index),
                        new
                        {
                            page = page,
                            obrisano = true,
                            poruka = naziv
                        });
                }
                catch (Exception exc)
                {
                    string ex = "Došlo je do greške prilikom brisanja lijeka: "
                        +  "\n Greška: " + exc.InnerException.Message + "Kontaktirajte nas za pomoć";
                    return RedirectToAction(nameof(Index), new { page = page, greska = true, ex = ex });
                }
            }
            else
            {
                return NotFound("Ne postoji lijek s oznakom: " + id);
            }
        }

        private string GreskaPoruka (LijekLjekarna lijekLjekarna)
        {
            StringBuilder poruka = new StringBuilder();

            if (string.IsNullOrEmpty(lijekLjekarna.DostupnostLijek))
            {
                poruka.Append("Potrebno je unijeti dostupnost lijeka;");
            }
            if (lijekLjekarna.CijenaLijek <= 0 || lijekLjekarna.CijenaLijek > 20000)
            {
                poruka.Append("Nedopuštena cijena lijeka;");
            }
            else if (lijekLjekarna.CijenaLijek > 0 && lijekLjekarna.CijenaLijek < 20000);
            else
            {
                poruka.Append("Potrebno je unjeti cijenu lijeka;");
            }

            string porukaStr = poruka.ToString();
            return porukaStr;
        }
    }
}
