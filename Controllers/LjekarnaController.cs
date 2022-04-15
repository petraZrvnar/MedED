using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MedEd.Models;
using MedEd.ViewModels;
using System;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace MedEd.Controllers
{
    public class LjekarnaController : Controller
    {
        private readonly MedEdContext _context;
        private readonly AppSettings appData;

        public LjekarnaController(MedEdContext context, IOptionsSnapshot<AppSettings> options)
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
            if (uloga == null || uloga != "Administrator")
            {
                return RedirectToAction("NotAuthorized", "Login");
            }
            int pagesize = appData.PageSize;

            var ljekarne = _context.Ljekarna
                           .Include(d => d.SifVrstaLjekarnaNavigation)
                           .Include(d => d.SifMjestoNavigation)
                           .AsQueryable();

            int count = ljekarne.Count();

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

            var ljekarna = ljekarne
                    .Select(m => new LjekarnaAdminViewModel
                    {
                       SifraLjekarna = m.SifraLjekarna,
                       NazivLjekarna = m.NazivLjekarna,
                       AdresaLjekarna = m.AdresaLjekarna,
                       KontaktBroj = m.KontaktBroj,
                       Mjesto = m.SifMjestoNavigation.NazivMjesto,
                       VrstaLjekarna = m.SifVrstaLjekarnaNavigation.OpisVrstaLjekarna

                    })
                    .Skip((page - 1) * pagesize)
                    .Take(pagesize)
                    .ToList();

            var model = new LjekarneAdminViewModel
            {
                Ljekarne = ljekarna,
                PagingInfo = pagingInfo,
                
            };
            if (bezRezultata)
            {
                model.BezRezultata = true;
                model.Naziv = "";
                model.Adresa = "";
                model.Mjesto = "";
                model.Kontakt = "";
                model.Vrsta = "";
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
        public IActionResult Search(string naziv, string adresa, string mjesto, string kontakt, 
            string vrsta, int page = 1)
        {
            var uloga = HttpContext.Session.GetString("Uloga");
            if (uloga == null || uloga != "Administrator")
            {
                return RedirectToAction("NotAuthorized", "Login");
            }
            int pagesize = appData.PageSize;
            var ljekarna = _context.Ljekarna
                           .Include(d => d.SifMjestoNavigation)
                           .Include(d => d.SifVrstaLjekarnaNavigation)
                           .AsQueryable();

            int count = ljekarna.Count();

            if (!string.IsNullOrEmpty(naziv))
            {
                ljekarna = ljekarna.Where(b => String.Equals(b.NazivLjekarna, naziv,
                   StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(adresa))
            {
                ljekarna = ljekarna.Where(b => String.Equals(b.AdresaLjekarna, adresa,
                   StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(mjesto) && mjesto != "Sva mjesta")
            {
                ljekarna = ljekarna.Where(b => String.Equals(b.SifMjestoNavigation.NazivMjesto, mjesto,
                   StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(kontakt))
            {
                ljekarna = ljekarna.Where(b => String.Equals(b.KontaktBroj, kontakt,
                   StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(vrsta) && vrsta != "Sve vrste")
            {
                ljekarna = ljekarna.Where(b => String.Equals(b.SifVrstaLjekarnaNavigation.OpisVrstaLjekarna,
                    vrsta,
                   StringComparison.OrdinalIgnoreCase));
            }

            if (!ljekarna.Any())
            {
                return RedirectToAction("Index", new { bezRezultata = true });
            }

            else
            {

                var ljekarna2 = ljekarna
                      .Select(m => new LjekarnaAdminViewModel
                      {
                          SifraLjekarna = m.SifraLjekarna,
                          NazivLjekarna = m.NazivLjekarna,
                          AdresaLjekarna = m.AdresaLjekarna,
                          KontaktBroj = m.KontaktBroj,
                          Mjesto = m.SifMjestoNavigation.NazivMjesto,
                          VrstaLjekarna = m.SifVrstaLjekarnaNavigation.OpisVrstaLjekarna

                      }).Skip((page - 1) * pagesize)
                    .Take(pagesize)
                    .ToList();

                int count2 = ljekarna.Count();

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

                var model = new LjekarneAdminViewModel
                {
                    Ljekarne = ljekarna2,
                    PagingInfo = pagingInfo,
                    Naziv = naziv,
                    Adresa = adresa,
                    Mjesto = mjesto,
                    Kontakt = kontakt,
                    Vrsta = vrsta
                };

                return View(model);
            }
        }

        public IActionResult Details(int id, int page = 1)
        {
            var uloga = HttpContext.Session.GetString("Uloga");
            if (uloga == null || uloga != "Administrator")
            {
                return RedirectToAction("NotAuthorized", "Login");
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

        public IActionResult DetailsSearch(int id, string naziv, string adresa, string mjesto, 
            string kontakt, string vrsta, int page = 1)
        {
            var uloga = HttpContext.Session.GetString("Uloga");
            if (uloga == null || uloga != "Administrator")
            {
                return RedirectToAction("NotAuthorized", "Login");
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
                ViewBag.Adresa = adresa;
                ViewBag.Mjesto = mjesto;
                ViewBag.Kontakt = kontakt;
                ViewBag.Vrsta = vrsta;
                ViewBag.Page = page;
                return View(ljekarna);
            }
        }

        public IActionResult Create()
        {
            var uloga = HttpContext.Session.GetString("Uloga");
            if (uloga == null || uloga != "Administrator")
            {
                return RedirectToAction("NotAuthorized", "Login");
            }
            ViewData["SifMjesto"] = new SelectList(_context.Mjesto, "SifraMjesto", "NazivMjesto");
            ViewData["SifVrstaLjekarna"] = new SelectList(_context.VrstaLjekarna, "SifraVrstaLjekarna", "OpisVrstaLjekarna");
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("NazivLjekarna,AdresaLjekarna,KontaktBroj,EmailLjekarna,RadnoVrijeme,SifMjesto,SifVrstaLjekarna")] Ljekarna ljekarna)
        {
            var uloga = HttpContext.Session.GetString("Uloga");
            if (uloga == null || uloga != "Administrator")
            {
                return RedirectToAction("NotAuthorized", "Login");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(ljekarna);
                    _context.SaveChanges();
                    string naziv = ljekarna.NazivLjekarna;
                    return RedirectToAction(nameof(Index), new { poruka = naziv, dodano = true });
                }catch (Exception ex)
                {
                    string poruka = "Došlo je do pogreške prilikom dodavanja " +
                        ljekarna.NazivLjekarna + "\nGreška: " + ex.InnerException.Message;
                    return RedirectToAction(nameof(Index),
                        new { ex = poruka, greska = true });
                }
               
            }
            else
            {
                string porukaDodavanje = GreskaPoruka(ljekarna);
                return RedirectToAction("Index", new { greskaEdit = true, porukaDod = porukaDodavanje });
            }
        }

        public IActionResult Edit(int id, int page = 1)
        {
            var uloga = HttpContext.Session.GetString("Uloga");
            if (uloga == null || uloga != "Administrator")
            {
                return RedirectToAction("NotAuthorized", "Login");
            }
            var ljekarna =  _context.Ljekarna.Where(d => d.SifraLjekarna == id).SingleOrDefault();
            if (ljekarna == null)
            {
                return NotFound();
            }
            else
            {
                ViewBag.Page = page;
                ViewData["SifMjesto"] = new SelectList(_context.Mjesto, "SifraMjesto", "NazivMjesto", ljekarna.SifMjesto);
                ViewData["SifVrstaLjekarna"] = new SelectList(_context.VrstaLjekarna, "SifraVrstaLjekarna", "OpisVrstaLjekarna", ljekarna.SifVrstaLjekarna);
                return PartialView("Edit", ljekarna);
            }
            
        }

        public IActionResult EditSearch(int id, string naziv, string adresa, string mjesto, string kontakt,
            string vrsta, int page = 1)
        {
            var uloga = HttpContext.Session.GetString("Uloga");
            if (uloga == null || uloga != "Administrator")
            {
                return RedirectToAction("NotAuthorized", "Login");
            }
            var ljekarna = _context.Ljekarna.Where(d => d.SifraLjekarna == id).SingleOrDefault();
            if (ljekarna == null)
            {
                return NotFound();
            }
            else
            {
                ViewBag.Page = page;
                ViewBag.Naziv = naziv;
                ViewBag.Adresa = adresa;
                ViewBag.Mjesto = mjesto;
                ViewBag.Kontakt = kontakt;
                ViewBag.Vrsta = vrsta;
                ViewData["SifMjesto"] = new SelectList(_context.Mjesto, "SifraMjesto", "NazivMjesto", ljekarna.SifMjesto);
                ViewData["SifVrstaLjekarna"] = new SelectList(_context.VrstaLjekarna, "SifraVrstaLjekarna", "OpisVrstaLjekarna", ljekarna.SifVrstaLjekarna);
                return PartialView(ljekarna);
            }
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, int page = 1)
        {
            var uloga = HttpContext.Session.GetString("Uloga");
            if (uloga == null || uloga != "Administrator")
            {
                return RedirectToAction("NotAuthorized", "Login");
            }
            try
            {
                var ljekarna = await _context.Ljekarna
                                   .Where(d => d.SifraLjekarna == id)
                                   .FirstOrDefaultAsync();

                if (ljekarna == null)
                {
                    return NotFound("Neispravna oznaka ljekarne: " + id);
                }

                if (await TryUpdateModelAsync<Ljekarna>(ljekarna))
                {
                    ViewBag.Page = page;

                    try
                    {
                        await _context.SaveChangesAsync();
                        string poruka = ljekarna.NazivLjekarna;
                        return RedirectToAction(nameof(Index),
                            new
                            {
                                page = page,
                                ažurirano = true,
                                poruka = poruka,
                            });
                        
                    }
                    catch (Exception exc)
                    {
                        ModelState.AddModelError(string.Empty, exc.Message);
                        return View(ljekarna);
                    }
                }
                else
                {
                    string porukaDodavanje = GreskaPoruka(ljekarna);
                    return RedirectToAction("Index", new { greskaEdit = true, porukaDod = porukaDodavanje, page = page });
                }
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Edit), id);
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id, int page = 1)
        {
            var uloga = HttpContext.Session.GetString("Uloga");
            if (uloga == null || uloga != "Administrator")
            {
                return RedirectToAction("NotAuthorized", "Login");
            }
            var ljekarna = _context.Ljekarna
                        .Where(d => d.SifraLjekarna == id)
                        .SingleOrDefault();

            if (ljekarna != null)
            {
                try
                {
                    string naziv = ljekarna.NazivLjekarna;
                    _context.Remove(ljekarna);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index),
                        new
                        {
                            page = page,
                            obrisano = true,
                            poruka = naziv,
                        });
                }
                catch (Exception exc)
                {
                    string ex = "Došlo je do greške prilikom brisanja ljekarne: "
                        + ljekarna.NazivLjekarna + "\nGreška: " + exc.InnerException.Message + "Kontaktirajte nas za pomoć";
                    return RedirectToAction(nameof(Index), new { page = page,  greska = true, ex = ex });
                }
            }
            else
            {
                return NotFound("Ne postoji ljekarna s oznakom: " + id);
            }
        }

        private string GreskaPoruka(Ljekarna ljekarna)
        {
            StringBuilder poruka = new StringBuilder();
            if (string.IsNullOrEmpty(ljekarna.NazivLjekarna))
            {
                poruka.Append("Potrebno je unjeti naziv ljekarne;");

            }
            else
            {
                if (ljekarna.NazivLjekarna.Length > 255)
                {
                    poruka.Append("Predgačak naziv ljekarne, naziv može imati najviše 255 znakova;");
                }
            }
            

            if (string.IsNullOrEmpty(ljekarna.AdresaLjekarna))
            {
                poruka.Append("Potrebno je unjeti adresu ljekarne;");
            }
            else
            {
                if (ljekarna.AdresaLjekarna.Length > 255)
                {
                    poruka.Append("Predugačka adersa ljekarne, adresa može imati najviše 255 znakova;");
                }
            }
            

            if (string.IsNullOrEmpty(ljekarna.KontaktBroj))
            {
                poruka.Append("Potrebno je unjeti kontakt ljekarne;");
            }
            else
            {
                if (ljekarna.KontaktBroj.Length > 255)
                {
                    poruka.Append("Predugačka kontakt ljekarne, kontakt može imati najviše 255 znakova;");
                }
            }
            

            if (string.IsNullOrEmpty(ljekarna.RadnoVrijeme))
            {
                poruka.Append("Potrebno je unjeti radno vrijeme ljekarne;");
            }

            if (!string.IsNullOrEmpty(ljekarna.EmailLjekarna) && ljekarna.EmailLjekarna.Length > 255)
            {
                poruka.Append("Predugačak e-mail ljekarne, e-mail može imati najviše 255 znakova;");
            }

            string porukaStr = poruka.ToString();
            return porukaStr;
        }

    }
}
