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
    public class LijekController : Controller
    {
        private readonly MedEdContext _context;
        private readonly AppSettings appData;

        public LijekController(MedEdContext context, IOptionsSnapshot<AppSettings> options)
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
            var lijekovi = _context.Lijek
                           .Include(d => d.SifDjelatnaTvarNavigation)
                           .Include(d => d.SifFarmOblikNavigation)
                           .Include(d => d.SifNositeljNavigation)
                           .Include(d => d.SifProizvodjacNavigation)
                           .Include(d => d.SifVrstaLijekNavigation)
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
                             DjelatnaTvar = m.SifDjelatnaTvarNavigation.NazivDjelatnaTvar
                         })
                         .Skip((page - 1) * pagesize)
                         .Take(pagesize)
                         .ToList();
            var model = new LijekoviViewModel
            {
                Lijekovi = lijekovi2,
                PagingInfo = pagingInfo
            };

            if (bezRezultata)
            {
                model.BezRezultata = true;
                model.Naziv = "";
                model.Vrsta = "";
                model.Br = "";
                model.Tvar = "";
                model.Jedinica = "";
                model.Kolicina = null;
                model.Pro = "";
                model.Oblik = "";
                model.Nositelj = "";
                model.God = null;
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
        public IActionResult Search(string naziv, string vrsta, string brOd, string tvar,
            string jedinica, int? kolicina, string pro, string oblik, string nositelj, int? god,
            int page = 1)
        {
            var uloga = HttpContext.Session.GetString("Uloga");
            if (uloga == null || uloga != "Administrator")
            {
                return RedirectToAction("NotAuthorized", "Login");
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

            if (!string.IsNullOrEmpty(nositelj) && nositelj != "Svi nositelji")
            {
                lijekovi = lijekovi.Where(d => String.Equals(d.SifNositeljNavigation.NazivNositelj, 
                  nositelj, StringComparison.OrdinalIgnoreCase));
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
                             DjelatnaTvar = m.SifDjelatnaTvarNavigation.NazivDjelatnaTvar
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
            var model = new LijekoviViewModel
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
                Nositelj = nositelj,
                God = god
            };
            return View(model);
        }

        [HttpGet]
        public IActionResult DetailsSearch(int id, string naziv, string vrsta, string brOd, string tvar,
            string jedinica, int? kolicina, string pro, string oblik, string nositelj, int? god, int page = 1)
        {
            var uloga = HttpContext.Session.GetString("Uloga");
            if (uloga == null || uloga != "Administrator")
            {
                return RedirectToAction("NotAuthorized", "Login");
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
                ViewBag.Vrsta = vrsta;
                ViewBag.Br = brOd;
                ViewBag.Tvar = tvar;
                ViewBag.Jedinica = jedinica;
                ViewBag.Kolicina = kolicina;
                ViewBag.Pro = pro;
                ViewBag.Oblik = oblik;
                ViewBag.Nositelj = nositelj;
                ViewBag.God = god;
                ViewBag.Page = page;
                return View(lijek);
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
        public IActionResult Create()
        {
            var uloga = HttpContext.Session.GetString("Uloga");
            if (uloga == null || uloga != "Administrator")
            {
                return RedirectToAction("NotAuthorized", "Login");
            }
            ViewData["SifDjelatnaTvar"] = new SelectList(_context.DjelatnaTvar, "SifraDjelatnaTvar", "NazivDjelatnaTvar");
            ViewData["SifFarmOblik"] = new SelectList(_context.FarmaceutskiOblik, "SifraFarmOblik", "OpisFarmOblik");
            ViewData["SifNositelj"] = new SelectList(_context.NositeljOdobrenja, "SifraNositelj", "NazivNositelj");
            ViewData["SifProizvodjac"] = new SelectList(_context.Proizvodjac, "SifraProizvodjac", "NazivProizvodjac");
            ViewData["SifVrstaLijek"] = new SelectList(_context.VrstaLijek, "SifraVrstaLijek", "OpisVrstaLijek");
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("SifraLijek,NazivLijek,BrojOdobrenja,GodStavljanjaTrziste,SlikaLijek,NacinIzdavanja,NaTrzistu,Kolicina,Jedinica,SifFarmOblik,SifVrstaLijek,SifNositelj,SifProizvodjac,SifDjelatnaTvar")] Lijek lijek)
        {
            var uloga = HttpContext.Session.GetString("Uloga");
            if (uloga == null || uloga != "Administrator")
            {
                return RedirectToAction("NotAuthorized", "Login");
            }
            var lijek2 = _context.Lijek.Where(d => d.BrojOdobrenja == lijek.BrojOdobrenja);
            if (lijek2.Any())
            {
                ModelState.AddModelError("BrojOdobrenja", "Broj odobrenja mora biti jedinstven");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(lijek);
                    _context.SaveChanges();
                    string naziv = lijek.NazivLijek;
                    return RedirectToAction(nameof(Index), new { poruka = naziv, dodano = true });
                }
                catch (Exception ex)
                {
                    string poruka = "Došlo je do pogreške prilikom dodavanja " +
                        lijek.NazivLijek + "\n Greška: " + ex.InnerException.Message;
                    return RedirectToAction(nameof(Index),
                        new { ex = poruka, greska = true });
                }
            }
            else
            {
                string porukaDodavanje = GreskaPoruka(lijek);
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

            var lijek = _context.Lijek.Where(d => d.SifraLijek == id).SingleOrDefault();
            if (lijek == null)
            {
                return NotFound();
            }
            ViewBag.Page = page;
            ViewData["SifDjelatnaTvar"] = new SelectList(_context.DjelatnaTvar, "SifraDjelatnaTvar", "NazivDjelatnaTvar", lijek.SifDjelatnaTvar);
            ViewData["SifFarmOblik"] = new SelectList(_context.FarmaceutskiOblik, "SifraFarmOblik", "OpisFarmOblik", lijek.SifFarmOblik);
            ViewData["SifNositelj"] = new SelectList(_context.NositeljOdobrenja, "SifraNositelj", "NazivNositelj", lijek.SifNositelj);
            ViewData["SifProizvodjac"] = new SelectList(_context.Proizvodjac, "SifraProizvodjac", "NazivProizvodjac", lijek.SifProizvodjac);
            ViewData["SifVrstaLijek"] = new SelectList(_context.VrstaLijek, "SifraVrstaLijek", "OpisVrstaLijek", lijek.SifVrstaLijek);
            return PartialView("Edit", lijek);
        }

        public IActionResult EditSearch(int id, string naziv, string vrsta, string brOd, string tvar,
            string jedinica, int? kolicina, string pro, string oblik, string nositelj, int? god, 
            int page = 1)
        {
            var uloga = HttpContext.Session.GetString("Uloga");
            if (uloga == null || uloga != "Administrator")
            {
                return RedirectToAction("NotAuthorized", "Login");
            }

            var lijek = _context.Lijek.Where(d => d.SifraLijek == id).SingleOrDefault();
            if (lijek == null)
            {
                return NotFound();
            }
            ViewBag.Naziv = naziv;
            ViewBag.Vrsta = vrsta;
            ViewBag.Br = brOd;
            ViewBag.Tvar = tvar;
            ViewBag.Jedinica = jedinica;
            ViewBag.Kolicina = kolicina;
            ViewBag.Pro = pro;
            ViewBag.Oblik = oblik;
            ViewBag.Nositelj = nositelj;
            ViewBag.God = god;
            ViewBag.Page = page;
            ViewData["SifDjelatnaTvar"] = new SelectList(_context.DjelatnaTvar, "SifraDjelatnaTvar", "NazivDjelatnaTvar", lijek.SifDjelatnaTvar);
            ViewData["SifFarmOblik"] = new SelectList(_context.FarmaceutskiOblik, "SifraFarmOblik", "OpisFarmOblik", lijek.SifFarmOblik);
            ViewData["SifNositelj"] = new SelectList(_context.NositeljOdobrenja, "SifraNositelj", "NazivNositelj", lijek.SifNositelj);
            ViewData["SifProizvodjac"] = new SelectList(_context.Proizvodjac, "SifraProizvodjac", "NazivProizvodjac", lijek.SifProizvodjac);
            ViewData["SifVrstaLijek"] = new SelectList(_context.VrstaLijek, "SifraVrstaLijek", "OpisVrstaLijek", lijek.SifVrstaLijek);
            return View(lijek);
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

                var lijek = await _context.Lijek
                    .Where(d => d.SifraLijek == id)
                    .FirstOrDefaultAsync();
                if (lijek == null)
                {
                    return NotFound("Neispravna oznaka lijeka " + id);
                }

                var lijek2 = _context.Lijek.Where(d => d.BrojOdobrenja == lijek.BrojOdobrenja);
                if (lijek2.Any())
                {
                    ModelState.AddModelError("BrojOdobrenja", "Broj odobrenja mora biti jedinstven");
                }

                if (await TryUpdateModelAsync<Lijek>(lijek))
                {
                    ViewBag.Page = page;

                    try
                    {
                        await _context.SaveChangesAsync();
                        string poruka = lijek.NazivLijek;
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
                        return View(lijek);
                    }
                }
                else
                {
                    string porukaDodavanje = GreskaPoruka(lijek);
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
            var lijek = _context.Lijek.Where(d => d.SifraLijek == id).SingleOrDefault();
            if (lijek != null)
            {
                try
                {
                    string naziv = lijek.NazivLijek;
                    _context.Remove(lijek);
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
                        + lijek.NazivLijek + "\n Greška: " + exc.InnerException.Message + "Kontaktirajte nas za pomoć";
                    return RedirectToAction(nameof(Index), new { page = page, greska = true, ex = ex });
                }
            }
            else
            {
                return NotFound("Ne postoji lijek s oznakom: " + id);
            }
        }

        private string GreskaPoruka(Lijek lijek)
        {
            StringBuilder poruka = new StringBuilder();
            if (string.IsNullOrEmpty(lijek.NazivLijek))
            {
                poruka.Append("Potrebno je unjeti naziv lijeka;");
            }
            else if(!string.IsNullOrEmpty(lijek.NazivLijek))
            {
                if (lijek.NazivLijek.Length > 255)
                {
                    poruka.Append("Predugačak naziv lijeka, naziv lijeka može imati najviše 255 znakova;");
                }
            }

            if (string.IsNullOrEmpty(lijek.BrojOdobrenja))
            {
                poruka.Append("Potrebno je unjeti broj odobrenja;");
            }
            else if(!string.IsNullOrEmpty(lijek.BrojOdobrenja))
            {
                if (lijek.BrojOdobrenja.Length > 14)
                {
                    poruka.Append("Predugačak broj odobrenja lijeka, broj odobrenja lijeka može imati najviše 255 znakova;");
                }

                var lijek2 = _context.Lijek.Where(d => d.BrojOdobrenja == lijek.BrojOdobrenja);
                if (lijek2.Any())
                {
                    poruka.Append("Već postoji lijek s upisanim brojem odobrenja, broj odobrenja mora biti jedinstven;");
                }
            }

            if (string.IsNullOrEmpty(lijek.GodStavljanjaTrziste.ToString()))
            {
                poruka.Append("Potrebno je unjeti godinu stavljanja lijeka na tržište;");
            }
            else if (!string.IsNullOrEmpty(lijek.GodStavljanjaTrziste.ToString()))
            {
                if (lijek.GodStavljanjaTrziste < 1950 || lijek.GodStavljanjaTrziste > 2030)
                {
                    poruka.Append("Nedopuštena godina stavljanja lijeka na tržište;");
                }
            }

            if (string.IsNullOrEmpty(lijek.NacinIzdavanja))
            {
                poruka.Append("Potrebno je unjeti način izdavanja lijeka;");
            }
            else if(!string.IsNullOrEmpty(lijek.NacinIzdavanja))
            {
                if (lijek.NazivLijek.Length > 255)
                {
                    poruka.Append("Predugačak način izdavanja lijeka, način izdavanja lijeka može imati najviše 255 znakova;");
                }
            }

            if (string.IsNullOrEmpty(lijek.NaTrzistu))
            {
                poruka.Append("Potrebno je jeli lijek na tržištu ili nije;");
            }

            if (string.IsNullOrEmpty(lijek.Kolicina.ToString()))
            {
                poruka.Append("Potrebno je unjeti količinu djelatne tvari lijeka;");
            }
            else if(!string.IsNullOrEmpty(lijek.Kolicina.ToString()))
            {
                if (lijek.Kolicina > 10000 || lijek.Kolicina < 1)
                {
                    poruka.Append("Nedopuštena količina djelatne tvari;");
                }
            }

            if (string.IsNullOrEmpty(lijek.Jedinica))
            {
                poruka.Append("Potrebno je unjeti mjernu jedinicu količine djelatne tvari;");
            }

            string porukaStr = poruka.ToString();
            return porukaStr;
        }
    }
}
