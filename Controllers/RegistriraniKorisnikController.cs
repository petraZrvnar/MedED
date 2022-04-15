using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MedEd.Models;
using Microsoft.Extensions.Options;
using MedEd.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;

namespace MedEd.Controllers
{
    public class RegistriraniKorisnikController : Controller
    {
        private readonly MedEdContext _context;
        private readonly AppSettings appData;

        public RegistriraniKorisnikController(MedEdContext context, IOptionsSnapshot<AppSettings> options)
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
            var korisnici = _context.RegistriraniKorisnik
                            .Include(d => d.SifUlogaNavigation)
                            .Include(d => d.SifLjekarnaNavigation)
                            .AsQueryable();

            int count = korisnici.Count();
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

            var korisnici2 = korisnici
                           .Select(m => new KorisnikViewModel
                           {
                               SifraKorisnik = m.SifraKorisnik,
                               Ime = m.Ime,
                               Prezime = m.Prezime,
                               KorisnickoIme = m.KorisnickoIme,
                               EmailKorisnik = m.EmailKorisnik,
                               SifLjekarna = m.SifLjekarna,
                               Uloga = m.SifUlogaNavigation.OpisUloga,
                               Ljekarna = m.SifLjekarnaNavigation.NazivLjekarna
                           })
                           .Skip((page - 1) * pagesize)
                           .Take(pagesize)
                           .ToList();
            var model = new KorisniciViewModel
            {
                Korisnici = korisnici2,
                PagingInfo = pagingInfo
            };
            if (bezRezultata)
            {
                model.BezRezultata = true;
                model.Ime = "";
                model.Prezime = "";
                model.Email = "";
                model.KIme = "";
                model.Uloga = "";
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
        public IActionResult Search(string ime, string prezime, string email, string kime, string uloga,
            int page = 1)
        {
            var uloga2 = HttpContext.Session.GetString("Uloga");
            if (uloga2 == null || uloga2 != "Administrator")
            {
                return RedirectToAction("NotAuthorized", "Login");
            }
            int pagesize = appData.PageSize;

            var korisnici = _context.RegistriraniKorisnik
                           .Include(d => d.SifUlogaNavigation)
                           .Include(d => d.SifLjekarnaNavigation)
                           .AsQueryable();
            int count = korisnici.Count();

            if (!string.IsNullOrEmpty(ime))
            {
                korisnici = korisnici.Where(d => String.Equals(d.Ime, ime,
                   StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(prezime))
            {
                korisnici = korisnici.Where(d => String.Equals(d.Prezime, prezime,
                  StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(email))
            {
                korisnici = korisnici.Where(d => d.EmailKorisnik == email);
            }

            if (!string.IsNullOrEmpty(kime))
            {
                korisnici = korisnici.Where(d => d.KorisnickoIme == kime);
            }

            if (!string.IsNullOrEmpty(uloga) && uloga != "--")
            {
                korisnici = korisnici.Where(d => String.Equals(d.SifUlogaNavigation.OpisUloga, uloga,
                  StringComparison.OrdinalIgnoreCase));
            }

            if (!korisnici.Any())
            {
                return RedirectToAction("Index", new { bezRezultata = true });
            }

            var korisnici2 = korisnici
                           .Select(m => new KorisnikViewModel
                           {
                               SifraKorisnik = m.SifraKorisnik,
                               Ime = m.Ime,
                               Prezime = m.Prezime,
                               KorisnickoIme = m.KorisnickoIme,
                               EmailKorisnik = m.EmailKorisnik,
                               SifLjekarna = m.SifLjekarna,
                               Uloga = m.SifUlogaNavigation.OpisUloga,
                               Ljekarna = m.SifLjekarnaNavigation.NazivLjekarna
                           })
                           .Skip((page - 1) * pagesize)
                           .Take(pagesize)
                           .ToList();
            int count2 = korisnici.Count();
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
            var model = new KorisniciViewModel
            {
                Korisnici = korisnici2,
                PagingInfo = pagingInfo,
                Ime = ime,
                Prezime = prezime,
                KIme = kime,
                Email = email,
                Uloga = uloga
            };
            return View(model);
        }

        [HttpGet]
        public IActionResult DetailsSearch(int id, string ime, string prezime, string email,
            string kime, string uloga, int page = 1)
        {
            var uloga2 = HttpContext.Session.GetString("Uloga");
            if (uloga2 == null || uloga2 != "Administrator")
            {
                return RedirectToAction("NotAuthorized", "Login");
            }
            var korisnik = _context.RegistriraniKorisnik
                           .Include(d => d.SifUlogaNavigation)
                           .Include(d => d.SifLjekarnaNavigation)
                           .Where(d => d.SifraKorisnik == id)
                           .SingleOrDefault();
            if (korisnik == null)
            {
                return NotFound("Ne postoji korisnik s oznakom: " + id);
            }
            else
            {
                ViewBag.Ime = ime;
                ViewBag.Prezime = prezime;
                ViewBag.KIme = kime;
                ViewBag.Email = email;
                ViewBag.Uloga = uloga;
                ViewBag.Page = page;
                return View(korisnik);
            }
        }

        [HttpGet]
        public IActionResult Details(int id, int page = 1)
        {
            var uloga = HttpContext.Session.GetString("Uloga");
            if (uloga == null || uloga != "Administrator")
            {
                return RedirectToAction("NotAuthorized", "Login");
            }
            var korisnik = _context.RegistriraniKorisnik
                           .Include(d => d.SifUlogaNavigation)
                           .Include(d => d.SifLjekarnaNavigation)
                           .Where(d => d.SifraKorisnik == id)
                           .SingleOrDefault();
            if (korisnik == null)
            {
                return NotFound("Ne postoji korisnik s oznakom: " + id);
            }
            else
            {
                ViewBag.Page = page;
                return View(korisnik);
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            var uloga = HttpContext.Session.GetString("Uloga");
            if (uloga == null || uloga != "Administrator")
            {
                return RedirectToAction("NotAuthorized", "Login");
            }
            ViewData["SifUloga"] = new SelectList(_context.UlogaKorisnik, "SifraUloga", "OpisUloga");
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("SifraKorisnik,Ime,Prezime,KorisnickoIme,EmailKorisnik,Lozinka,SifLjekarna,SifUloga")] RegistriraniKorisnik korisnik)
        {
            var uloga = HttpContext.Session.GetString("Uloga");
            if (uloga == null || uloga != "Administrator")
            {
                return RedirectToAction("NotAuthorized", "Login");
            }
            var korisnik2 = _context.RegistriraniKorisnik.Where(d => d.KorisnickoIme == korisnik.KorisnickoIme);
            if (korisnik2.Any())
            {
                ModelState.AddModelError("KorisnickoIme", "Korisničko ime mora biti jedinstveno");
            }

            var korisnik3 = _context.RegistriraniKorisnik.Where(d => d.EmailKorisnik == korisnik.EmailKorisnik);
            if (korisnik3.Any())
            {
                ModelState.AddModelError("EmailKorisnik", "E-mail mora biti jedinstven");
            }

            if (ModelState.IsValid)
            {
                byte[] salt = new byte[128 / 8];
                korisnik.Lozinka = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: korisnik.Lozinka,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

                try
                {
                    _context.Add(korisnik);
                    _context.SaveChanges();
                    string naziv = korisnik.Ime + " " + korisnik.Prezime;
                    return RedirectToAction(nameof(Index), new { poruka = naziv, dodano = true });
                }
                catch (Exception ex)
                {
                    string poruka = "Došlo je do pogreške prilikom dodavanja " +
                        korisnik.Ime + " " + korisnik.Prezime + "\n Greška: " + ex.InnerException.Message;
                    return RedirectToAction(nameof(Index),
                        new { ex = poruka, greska = true });
                }
            }
            else
            {
                string porukaDodavanje = GreskaPoruka(korisnik);
                return RedirectToAction("Index", new { greskaEdit = true, porukaDod = porukaDodavanje });
            }
        }

        [HttpGet]
        public IActionResult Edit(int id, int page = 1)
        {
            var uloga = HttpContext.Session.GetString("Uloga");
            if (uloga == null || uloga != "Administrator")
            {
                return RedirectToAction("NotAuthorized", "Login");
            }
            var korisnik = _context.RegistriraniKorisnik
                           .Where(d => d.SifraKorisnik == id)
                           .SingleOrDefault();
            if (korisnik == null)
            {
                return NotFound();
            }
            ViewBag.Page = page;
            ViewData["SifUloga"] = new SelectList(_context.UlogaKorisnik, "SifraUloga", "OpisUloga", korisnik.SifUloga);
            return PartialView(korisnik);
        }

        [HttpGet]
        public IActionResult EditSearch(int id, string ime, string prezime, string email,
            string kime, string uloga, int page = 1)
        {
            var uloga2 = HttpContext.Session.GetString("Uloga");
            if (uloga2 == null || uloga2 != "Administrator")
            {
                return RedirectToAction("NotAuthorized", "Login");
            }
            var korisnik = _context.RegistriraniKorisnik
                          .Where(d => d.SifraKorisnik == id)
                          .SingleOrDefault();
            if (korisnik == null)
            {
                return NotFound();
            }
            ViewBag.Ime = ime;
            ViewBag.Prezime = prezime;
            ViewBag.Email = email;
            ViewBag.Uloga = uloga;
            ViewBag.KIme = kime;
            ViewBag.Page = page;
            ViewData["SifUloga"] = new SelectList(_context.UlogaKorisnik, "SifraUloga", "OpisUloga", korisnik.SifUloga);
            return View(korisnik);
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
                var korisnik = await _context.RegistriraniKorisnik
                         .Where(d => d.SifraKorisnik == id)
                         .FirstOrDefaultAsync();
                if (korisnik == null)
                {
                    return NotFound("Neispravna oznaka korisnika: " + id);
                }

                var korisnik2 = _context.RegistriraniKorisnik.Where(d => d.KorisnickoIme == korisnik.KorisnickoIme)
                                .Where(d => d.SifraKorisnik != korisnik.SifraKorisnik);
                if (korisnik2.Any())
                {
                    ModelState.AddModelError("KorisnickoIme", "Korisničko ime mora biti jedinstveno");
                }

                var korisnik3 = _context.RegistriraniKorisnik.Where(d => d.EmailKorisnik == korisnik.EmailKorisnik)
                                .Where(d => d.SifraKorisnik != korisnik.SifraKorisnik);
                if (korisnik3.Any())
                {
                    ModelState.AddModelError("EmailKorisnik", "E-mail mora biti jedinstven");
                }

                if (await TryUpdateModelAsync<RegistriraniKorisnik>(korisnik))
                {
                    ViewBag.Page = page;

                    try
                    {
                        await _context.SaveChangesAsync();
                        string poruka = korisnik.Ime + " " + korisnik.Prezime;
                        return RedirectToAction(nameof(Index),
                            new
                            {
                                page = page,
                                ažurirano = true,
                                poruka = poruka
                            });

                    }
                    catch (Exception exc)
                    {
                        ModelState.AddModelError(string.Empty, exc.Message);
                        return View(korisnik);
                    }
                }
                else
                {
                    string porukaDodavanje = GreskaPoruka(korisnik);
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
        public IActionResult Delete(int id, int page = 1)
        {
            var uloga = HttpContext.Session.GetString("Uloga");
            if (uloga == null || uloga != "Administrator")
            {
                return RedirectToAction("NotAuthorized", "Login");
            }
            var korisnik = _context.RegistriraniKorisnik
                           .Where(d => d.SifraKorisnik == id)
                           .SingleOrDefault();
            if (korisnik != null)
            {
                try
                {
                    string naziv = korisnik.Ime + " " + korisnik.Prezime;
                    _context.Remove(korisnik);
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
                        + korisnik.Ime + " " + korisnik.Prezime + "\n Greška: " + exc.InnerException.Message + "Kontaktirajte nas za pomoć";
                    return RedirectToAction(nameof(Index), new { page = page, greska = true, ex = ex });
                }
            }
            else
            {
                return NotFound("Ne postoji korisnik s oznakom: " + id);
            }
        }

        private string GreskaPoruka(RegistriraniKorisnik korisnik)
        {
            StringBuilder poruka = new StringBuilder();

            if (string.IsNullOrEmpty(korisnik.Ime))
            {
                poruka.Append("Potrebno je unjeti ime korisnika;");
            }
            if(!string.IsNullOrEmpty(korisnik.Ime) && korisnik.Ime.Length > 255)
            {
                poruka.Append("Predugačko ime korisnika, ime može imati najviše 255 znakova;");
            }

            if (string.IsNullOrEmpty(korisnik.Prezime))
            {
                poruka.Append("Potrebno je unjeti prezime korisnika;");
            }
            if (!string.IsNullOrEmpty(korisnik.Prezime) && korisnik.Prezime.Length > 255)
            {
                poruka.Append("Predugačko prezime korisnika, ime može imati najviše 255 znakova;");
            }

            if (string.IsNullOrEmpty(korisnik.KorisnickoIme))
            {
                poruka.Append("Potrebno je unjeti korisničko ime korisnika;");
            }
            if (!string.IsNullOrEmpty(korisnik.KorisnickoIme) && korisnik.KorisnickoIme.Length > 255)
            {
                poruka.Append("Predugačko korisničko ime korisnika, ime može imati najviše 255 znakova;");
            }

            var korisnik2 = _context.RegistriraniKorisnik.Where(d => d.KorisnickoIme == korisnik.KorisnickoIme);
            if (korisnik2.Any())
            {
                poruka.Append("Registrirani korisnik s unesenim korisničkim imenom već postoji, korisničko ime mora biti jedinstveno;");
            }

            if (string.IsNullOrEmpty(korisnik.EmailKorisnik))
            {
                poruka.Append("Potrebno je unjeti e-mail korisnika;");
            }
            if (!string.IsNullOrEmpty(korisnik.EmailKorisnik) && korisnik.EmailKorisnik.Length > 255)
            {
                poruka.Append("Predugački e-mail korisnika, ime može imati najviše 255 znakova;");
            }

            var korisnik3 = _context.RegistriraniKorisnik.Where(d => d.EmailKorisnik == korisnik.EmailKorisnik);
            if (korisnik3.Any())
            {
                poruka.Append("Registrirani korisnik s unesenim e-mail-om već postoji, e-mail mora biti jedinstven;");
            }

            if (string.IsNullOrEmpty(korisnik.Lozinka))
            {
                poruka.Append("Potrebno je unjeti lozinku korisnika;");
            }
            if (!string.IsNullOrEmpty(korisnik.Lozinka) && korisnik.Lozinka.Length > 255)
            {
                poruka.Append("Predugačka lozinka korisnika, lozinka može imati najviše 255 znakova;");
            }

            string porukaStr = poruka.ToString();
            return porukaStr;
        }

    }
}
