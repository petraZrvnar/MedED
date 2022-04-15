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
using System.Text;

namespace MedEd.Controllers
{
    public class MedicinskiProizvodController : Controller
    {
        private readonly MedEdContext _context;
        private readonly AppSettings appData;

        public MedicinskiProizvodController(MedEdContext context, IOptionsSnapshot<AppSettings> options)
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
            var proizvodi = _context.MedicinskiProizvod
                           .Include(d => d.SifKlasaNavigation)
                           .Include(d => d.SifProizvodjacNavigation)
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

            var proizvod = proizvodi
                    .Select(m => new MedProViewModel
                    {
                        SifraMedProizvod = m.SifraMedProizvod,
                        Namjena = m.Namjena,
                        KataloskiBroj = m.KataloskiBroj,
                        NazivMedProizvod = m.NazivMedProizvod,
                        Proizvodjac = m.SifProizvodjacNavigation.NazivProizvodjac,
                        Klasa = m.SifKlasaNavigation.OznKlasaRizika
                    })
                    .Skip((page - 1) * pagesize)
                    .Take(pagesize)
                    .ToList();

            var model = new MedProAdminViewModel
            {
                Proizvodi = proizvod,
                PagingInfo = pagingInfo,

            };

            if (bezRezultata)
            {
                model.BezRezultata = true;
                model.KatBr = "";
                model.Klasa = "";
                model.Naziv = "";
                model.Pro = "";
                model.Namjena = "";
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
        public IActionResult Search(string naziv, string namjena, string katBr, string pro, 
            string klasa, int page = 1)
        {
            var uloga = HttpContext.Session.GetString("Uloga");
            if (uloga == null || uloga != "Administrator")
            {
                return RedirectToAction("NotAuthorized", "Login");
            }
            int pagesize = appData.PageSize;
            var proizvodi = _context.MedicinskiProizvod
                           .Include(d => d.SifKlasaNavigation)
                           .Include(d => d.SifProizvodjacNavigation)
                           .AsQueryable();

            int count = proizvodi.Count();

            if (!string.IsNullOrEmpty(katBr))
            {
                proizvodi = proizvodi.Where(b => String.Equals(b.KataloskiBroj, katBr,
                   StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(naziv))
            {
                proizvodi = proizvodi.Where(b => String.Equals(b.NazivMedProizvod, naziv,
                   StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(namjena))
            {
                proizvodi = proizvodi.Where(b => b.Namjena.IndexOf(namjena, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            if (!string.IsNullOrEmpty(pro) && pro !="Svi proizvođači")
            {
                proizvodi = proizvodi.Where(b => String.Equals(b.SifProizvodjacNavigation.NazivProizvodjac, pro,
                   StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(klasa) && klasa != "Sve klase")
            {
                proizvodi = proizvodi.Where(b => String.Equals(b.SifKlasaNavigation.OznKlasaRizika, klasa,
                   StringComparison.OrdinalIgnoreCase));
            }

            if (!proizvodi.Any())
            {
                return RedirectToAction("Index", new { bezRezultata = true });
            }

            else
            {

                var proizvod2 = proizvodi
                      .Select(m => new MedProViewModel
                      {
                          SifraMedProizvod = m.SifraMedProizvod,
                          Namjena = m.Namjena,
                          KataloskiBroj = m.KataloskiBroj,
                          NazivMedProizvod = m.NazivMedProizvod,
                          Proizvodjac = m.SifProizvodjacNavigation.NazivProizvodjac,
                          Klasa = m.SifKlasaNavigation.OznKlasaRizika

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
                if (page > pagingInfo.TotalPages)
                {
                    return RedirectToAction(nameof(Index), new { page = pagingInfo.TotalPages });
                }

                var model = new MedProAdminViewModel
                {
                    Proizvodi = proizvod2,
                    PagingInfo = pagingInfo,
                    KatBr = katBr,
                    Namjena = namjena,
                    Pro = pro,
                    Naziv = naziv,
                    Klasa = klasa
                };
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult DetailsSearch(string naziv, string namjena, string katBr, string pro,
            string klasa, int id, int page = 1)
        {
            var uloga = HttpContext.Session.GetString("Uloga");
            if (uloga == null || uloga != "Administrator")
            {
                return RedirectToAction("NotAuthorized", "Login");
            }
            var proizvod = _context.MedicinskiProizvod
                            .Include(d => d.SifProizvodjacNavigation)
                            .Include(d => d.SifKlasaNavigation)
                            .Where(d => d.SifraMedProizvod == id)
                            .SingleOrDefault();

            if (proizvod == null)
            {
                return NotFound("Ne postoji medicinski proizvod s oznakom: " + id);
            }
            else
            {
                ViewBag.Naziv = naziv;
                ViewBag.Namjena = namjena;
                ViewBag.KatBr = katBr;
                ViewBag.Pro = pro;
                ViewBag.Klasa = klasa;
                ViewBag.Page = page;
                return View(proizvod);
            };
        }

        [HttpGet]
        public IActionResult Details(int id, int page = 1)
        {
            var uloga = HttpContext.Session.GetString("Uloga");
            if (uloga == null || uloga != "Administrator")
            {
                return RedirectToAction("NotAuthorized", "Login");
            }
            var proizvod = _context.MedicinskiProizvod
                            .Include(d => d.SifProizvodjacNavigation)
                            .Include(d => d.SifKlasaNavigation)
                            .Where(d => d.SifraMedProizvod == id)
                            .SingleOrDefault();

            if (proizvod == null)
            {
                return NotFound("Ne postoji medicinski proizvod s oznakom: " + id);
            }
            else
            {
                ViewBag.Page = page;
                return View(proizvod);
            }
        }

        public IActionResult Create()
        {
            var uloga = HttpContext.Session.GetString("Uloga");
            if (uloga == null || uloga != "Administrator")
            {
                return RedirectToAction("NotAuthorized", "Login");
            }
            ViewData["SifKlasa"] = new SelectList(_context.KlasaRizik, "SifraKlasa", "OznKlasaRizika");
            ViewData["SifProizvodjac"] = new SelectList(_context.Proizvodjac, "SifraProizvodjac", "NazivProizvodjac");
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("SifraMedProizvod,Namjena,SlikaMedProizvod,KataloskiBroj,NazivMedProizvod,SifProizvodjac,SifKlasa")] MedicinskiProizvod medPro)
        {
            var uloga = HttpContext.Session.GetString("Uloga");
            if (uloga == null || uloga != "Administrator")
            {
                return RedirectToAction("NotAuthorized", "Login");
            }
            var proizvod2 = _context.MedicinskiProizvod.Where(d => d.KataloskiBroj == medPro.KataloskiBroj);
            if (proizvod2.Any())
            {
                ModelState.AddModelError("KataloskiBroj", "Kataloški broj mora biti jedinstven");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(medPro);
                    _context.SaveChanges();
                    string naziv = medPro.NazivMedProizvod;
                    return RedirectToAction(nameof(Index), new { poruka = naziv, dodano = true });
                }
                catch (Exception ex)
                {
                    string poruka = "Došlo je do pogreške prilikom dodavanja " +
                        medPro.NazivMedProizvod + "\n Greška: " + ex.InnerException.Message;
                    return RedirectToAction(nameof(Index),
                        new { ex = poruka, greska = true });
                }

            }
            else
            {
                string porukaDodavanje = GreskaPoruka(medPro);
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
            var proizvod = _context.MedicinskiProizvod.Where(d => d.SifraMedProizvod == id).SingleOrDefault();
            if (proizvod == null)
            {
                return NotFound();
            }
            else
            {
                ViewBag.Page = page;
                ViewData["SifKlasa"] = new SelectList(_context.KlasaRizik, "SifraKlasa", "OznKlasaRizika", proizvod.SifKlasa);
                ViewData["SifProizvodjac"] = new SelectList(_context.Proizvodjac, "SifraProizvodjac", "NazivProizvodjac", proizvod.SifProizvodjac);
                return PartialView(proizvod);
            }

        }

        public IActionResult EditSearch(int id, string naziv, string namjena, string katBr, string pro,
            string klasa, int page = 1)
        {
            var uloga = HttpContext.Session.GetString("Uloga");
            if (uloga == null || uloga != "Administrator")
            {
                return RedirectToAction("NotAuthorized", "Login");
            }
            var proizvod = _context.MedicinskiProizvod.Where(d => d.SifraMedProizvod == id).SingleOrDefault();
            if (proizvod == null)
            {
                return NotFound();
            }
            else
            {
                ViewBag.Page = page;
                ViewBag.KataloskiBr = katBr;
                ViewBag.Naziv = naziv;
                ViewBag.Namjena = namjena;
                ViewBag.Pro = pro;
                ViewBag.Klasa = klasa;
                ViewData["SifKlasa"] = new SelectList(_context.KlasaRizik, "SifraKlasa", "OznKlasaRizika", proizvod.SifKlasa);
                ViewData["SifProizvodjac"] = new SelectList(_context.Proizvodjac, "SifraProizvodjac", "NazivProizvodjac", proizvod.SifProizvodjac);
                return View(proizvod);
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
                var proizvod = await _context.MedicinskiProizvod
                                   .Where(d => d.SifraMedProizvod == id)
                                   .FirstOrDefaultAsync();

                if (proizvod == null)
                {
                    return NotFound("Neispravna oznaka proizvoda: " + id);
                }

                var proizvod2 = _context.MedicinskiProizvod.Where(d => d.KataloskiBroj == proizvod.KataloskiBroj);
                if (proizvod2.Any())
                {
                    ModelState.AddModelError("KataloskiBroj", "Kataloški broj mora biti jedinstven");
                }

                if (await TryUpdateModelAsync<MedicinskiProizvod>(proizvod))
                {
                    ViewBag.Page = page;

                    try
                    {
                        await _context.SaveChangesAsync();
                        string poruka = proizvod.NazivMedProizvod;
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
                        return View(proizvod);
                    }
                }
                else
                {
                    string porukaDodavanje = GreskaPoruka(proizvod);
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
            var proizvod = _context.MedicinskiProizvod
                         .Where(d => d.SifraMedProizvod == id)
                         .SingleOrDefault();

            if (proizvod != null)
            {
                try
                {
                    string naziv = proizvod.NazivMedProizvod;
                    _context.Remove(proizvod);
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
                    string ex = "Došlo je do greške prilikom brisanja medicinskog proizvoda: "
                        + proizvod.NazivMedProizvod + "\n Greška: " + exc.InnerException.Message + "Kontaktirajte nas za pomoć";
                    return RedirectToAction(nameof(Index), new { page = page, greska = true, ex = ex });
                }
            }
            else
            {
                return NotFound("Ne postoji medicinski proizvod s oznakom: " + id);
            }
        }

        private string GreskaPoruka(MedicinskiProizvod medPro)
        {
            StringBuilder poruka = new StringBuilder();
            if (string.IsNullOrEmpty(medPro.NazivMedProizvod))
            {
                poruka.Append("Potrebno je unjeti naziv medicinskog proizvoda;");

            }
            if (!string.IsNullOrEmpty(medPro.NazivMedProizvod) && (medPro.NazivMedProizvod.Length > 255))
            {
                poruka.Append("Predgačak naziv proizvoda, naziv može imati najviše 255 znakova;");
            }

            if (string.IsNullOrEmpty(medPro.Namjena))
            {
                poruka.Append("Potrebno je unjeti namjenu medicinskog proizvoda;");

            }

            if (string.IsNullOrEmpty(medPro.KataloskiBroj))
            {
                poruka.Append("Potrebno je unjeti kataloški medicinskog proizvoda;");

            }
            if (!string.IsNullOrEmpty(medPro.KataloskiBroj) && (medPro.KataloskiBroj.Length > 20))
            {
                poruka.Append("Predgačak kataloški broj proizvoda, naziv može imati najviše 255 znakova;");
            }

            var medPro2 = _context.MedicinskiProizvod.Where(d => d.KataloskiBroj == medPro.KataloskiBroj);

            if (medPro2.Any())
            {
                poruka.Append("Medicinski proizvod s unesenim kataloškim brojem već postoji, kataloški broj mora biti jedinstven;");
            }

            string porukaStr = poruka.ToString();
            return porukaStr;
        }
    }
}
