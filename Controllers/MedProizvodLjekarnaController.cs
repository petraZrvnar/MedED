using System;
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
    public class MedProizvodLjekarnaController : Controller
    {
        private readonly MedEdContext _context;
        private readonly AppSettings appData;

        public MedProizvodLjekarnaController(MedEdContext context, IOptionsSnapshot<AppSettings> options)
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
            if (uloga == null || (uloga != "Ljekarnik" && uloga!= "Ljekarnik Admin"))
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

            var proizvodi = _context.MedProizvodLjekarna
                            .Include(d => d.SifLjekarnaNavigation)
                            .Include(d => d.SifMedProizvodNavigation)
                            .Where(d => d.SifLjekarna == sifraLjekarna)
                            .AsQueryable();

            int count = proizvodi.Count();
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

            var pro = proizvodi
                  .Select(m => new MedLjekarnaViewModel
                  {
                      SifMedProizvod = m.SifMedProizvod,
                      SifLjekarna = m.SifLjekarna,
                      NazivLjekarna = m.SifLjekarnaNavigation.NazivLjekarna,
                      NazivProizvod = m.SifMedProizvodNavigation.NazivMedProizvod,
                      CijenaMedProizvod = m.CijenaMedProizvod,
                      DostupnostMedProizvod = m.DostupnostMedProizvod
                  })
                  .Skip((page - 1) * pagesize)
                  .Take(pagesize)
                  .ToList();
            var ljekarna = _context.Ljekarna.Where(d => d.SifraLjekarna == sifraLjekarna).SingleOrDefault();
            string nazivLjekarna = ljekarna.NazivLjekarna;

            var model = new MedProLjekarnaAdminViewModel
            {
                Proizvodi = pro,
                PagingInfo = pagingInfo,
                Ljekarna = sifraLjekarna,
                NazivLjekarna = nazivLjekarna
            };

            if (bezRezultata)
            {
                model.BezRezultata = true;
                model.Naziv = "";
                model.KatBr = "";
                model.Namjena = "";
                model.Klasa = "";
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
        public IActionResult Search(string naziv, string katBr, string klasa, string namjena,
            string dostupnost, string cijena, int page = 1)
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

            var proizvodi = _context.MedProizvodLjekarna
                            .Include(d => d.SifLjekarnaNavigation)
                            .Include(d => d.SifMedProizvodNavigation)
                            .Where(d => d.SifLjekarna == sifraLjekarna)
                            .AsQueryable();

            int count = proizvodi.Count();

            if (!string.IsNullOrEmpty(naziv))
            {
                proizvodi = proizvodi.Where(d => String.Equals(d.SifMedProizvodNavigation.NazivMedProizvod, naziv,
                  StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(katBr))
            {
                proizvodi = proizvodi.Where(d => String.Equals(d.SifMedProizvodNavigation.KataloskiBroj, katBr,
                  StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(klasa) && klasa != "Sve klase")
            {
                proizvodi = proizvodi.Where(d => String.Equals(d.SifMedProizvodNavigation.SifKlasaNavigation.OznKlasaRizika, klasa,
                  StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(namjena))
            {
                proizvodi = proizvodi.Where(d => d.SifMedProizvodNavigation.Namjena.IndexOf
                (naziv, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            if (!string.IsNullOrEmpty(dostupnost) && dostupnost != "--")
            {
                proizvodi = proizvodi.Where(d => String.Equals(d.DostupnostMedProizvod, dostupnost, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(cijena) && cijena != "--")
            {
                if (cijena == "Manja od 100kn")
                {
                    proizvodi = proizvodi.Where(d => d.CijenaMedProizvod < 100);
                }
                else if (cijena == "Od 100kn do 300kn")
                {
                    proizvodi = proizvodi.Where(d => d.CijenaMedProizvod > 100 && d.CijenaMedProizvod < 300);
                }
                else
                {
                    proizvodi = proizvodi.Where(d => d.CijenaMedProizvod > 300);
                }
            }

            if (!proizvodi.Any())
            {
                return RedirectToAction("Index", new { bezRezultata = true });
            }

            var pro = proizvodi
                  .Select(m => new MedLjekarnaViewModel
                  {
                      SifMedProizvod = m.SifMedProizvod,
                      SifLjekarna = m.SifLjekarna,
                      NazivLjekarna = m.SifLjekarnaNavigation.NazivLjekarna,
                      NazivProizvod = m.SifMedProizvodNavigation.NazivMedProizvod,
                      CijenaMedProizvod = m.CijenaMedProizvod,
                      DostupnostMedProizvod = m.DostupnostMedProizvod
                  })
                  .Skip((page - 1) * pagesize)
                  .Take(pagesize)
                  .ToList();
            int count2 = proizvodi.Count();
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
            var model = new MedProLjekarnaAdminViewModel
            {
                Proizvodi = pro,
                PagingInfo = pagingInfo,
                Ljekarna = sifraLjekarna,
                NazivLjekarna = nazivLjekarna,
                Naziv = naziv,
                KatBr = katBr,
                Namjena = namjena,
                Klasa = klasa,
                Dostupnost = dostupnost,
                Cijena = cijena
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult DetailsPro(int id, int page = 1)
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

            var proizvod = _context.MedicinskiProizvod
                           .Include(d => d.SifKlasaNavigation)
                           .Include(d => d.SifProizvodjacNavigation)
                           .Where(d => d.SifraMedProizvod == id)
                           .SingleOrDefault();

            if(proizvod == null)
            {
                return NotFound();
            }
            else
            {
                ViewBag.Page = page;
                return View(proizvod);
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
        public IActionResult DetailsSearchPro(int id, string naziv, string katBr, string klasa, string namjena,
            string dostupnost, string cijena, int page = 1)
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

            var proizvod = _context.MedicinskiProizvod
                           .Include(d => d.SifKlasaNavigation)
                           .Include(d => d.SifProizvodjacNavigation)
                           .Where(d => d.SifraMedProizvod == id)
                           .SingleOrDefault();

            if (proizvod == null)
            {
                return NotFound();
            }
            else
            {
                ViewBag.Naziv = naziv;
                ViewBag.KatBr = katBr;
                ViewBag.Klasa = klasa;
                ViewBag.Namjena = namjena;
                ViewBag.Dostupnost = dostupnost;
                ViewBag.Cijena = cijena;
                ViewBag.Page = page;
                return View(proizvod);
            }
        }

        public IActionResult DetailsSearchLjekarna(int id, string naziv, string katBr, string klasa, string namjena,
            string dostupnost, string cijena, int page = 1)
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
                ViewBag.KatBr = katBr;
                ViewBag.Klasa = klasa;
                ViewBag.Namjena = namjena;
                ViewBag.Dostupnost = dostupnost;
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
            ViewData["SifMedProizvod"] = new SelectList(_context.MedicinskiProizvod, "SifraMedProizvod", "NazivMedProizvod");
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("SifMedProizvod,CijenaMedProizvod,DostupnostMedProizvod")] MedProizvodLjekarna medProizvodLjekarna)
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
            medProizvodLjekarna.SifLjekarna = sifraLjekarna;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(medProizvodLjekarna);
                    _context.SaveChanges();
                    var pro = _context.MedicinskiProizvod.Where(d => d.SifraMedProizvod == medProizvodLjekarna.SifMedProizvod).SingleOrDefault();
                    string naziv = pro.NazivMedProizvod;
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
                string porukaDodavanje = GreskaPoruka(medProizvodLjekarna);
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

            var proLjekarna = _context.MedProizvodLjekarna
                              .Where(d => d.SifMedProizvod == id)
                              .Where(d => d.SifLjekarna == sifraLjekarna)
                              .SingleOrDefault();

            if (proLjekarna == null)
            {
                return NotFound();
            }

            var pro = _context.MedicinskiProizvod.Where(d => d.SifraMedProizvod == id).SingleOrDefault();
            string ime = pro.NazivMedProizvod;
            ViewBag.Page = page;
            ViewBag.Ime = ime;

            return PartialView(proLjekarna);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, [Bind("SifMedProizvod,SifLjekarna,CijenaMedProizvod,DostupnostMedProizvod")] MedProizvodLjekarna medProizvodLjekarna, int page = 1)
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
                    var pro = _context.MedicinskiProizvod.Where(d => d.SifraMedProizvod == medProizvodLjekarna.SifMedProizvod).SingleOrDefault();
                    string poruka = pro.NazivMedProizvod;
                    _context.Update(medProizvodLjekarna);
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
                    string ex = "Došlo je do greške prilikom ažuriranja proizvoda: "
                       + "\n Greška: " + exc.InnerException.Message + "Kontaktirajte nas za pomoć";
                    return RedirectToAction(nameof(Index), new { page = page, greska = true, ex = ex });
                }
            }
            else
            {
                string porukaDodavanje = GreskaPoruka(medProizvodLjekarna);
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

            var proLjekarna = _context.MedProizvodLjekarna
                                     .Where(d => d.SifMedProizvod == id)
                                     .Where(d => d.SifLjekarna == sifraLjekarna)
                                     .SingleOrDefault();

            if (proLjekarna != null)
            {
                try
                {
                    var pro = _context.MedicinskiProizvod.Where(d => d.SifraMedProizvod == id).SingleOrDefault();
                    string naziv = pro.NazivMedProizvod;
                    _context.Remove(proLjekarna);
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
                    string ex = "Došlo je do greške prilikom brisanja proizvoda: "
                         + "\n Greška: " + exc.InnerException.Message + "Kontaktirajte nas za pomoć";
                    return RedirectToAction(nameof(Index), new { page = page, greska = true, ex = ex });
                }
            }
            else
            {
                return NotFound("Ne postoji proizvod s oznakom: " + id);
            }
        }

        private string GreskaPoruka(MedProizvodLjekarna medPro)
        {
            StringBuilder poruka = new StringBuilder();
            if (string.IsNullOrEmpty(medPro.DostupnostMedProizvod))
            {
                poruka.Append("Potrebno je unijeti dostupnost medicinskog proizvoda;");
            }

            if (medPro.CijenaMedProizvod <= 0 || medPro.CijenaMedProizvod > 20000)
            {
                poruka.Append("Nedopuštena cijena medicinskog proizvoda;");
            }
            else if (medPro.CijenaMedProizvod > 0 && medPro.CijenaMedProizvod < 200000);
            else
            {
                poruka.Append("Potrebno je unjeti cijenu medicinskog proizvoda;");
            }

            string porukaStr = poruka.ToString();
            return porukaStr;
        }
    }
}
