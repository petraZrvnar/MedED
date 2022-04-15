using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MedEd.Models;

namespace MedEd.Controllers
{
    public class UlogaKorisnikController : Controller
    {
        private readonly MedEdContext _context;

        public UlogaKorisnikController(MedEdContext context)
        {
            _context = context;
        }

        // GET: UlogaKorisnik
        public async Task<IActionResult> Index()
        {
            return View(await _context.UlogaKorisnik.ToListAsync());
        }

        // GET: UlogaKorisnik/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ulogaKorisnik = await _context.UlogaKorisnik
                .FirstOrDefaultAsync(m => m.SifraUloga == id);
            if (ulogaKorisnik == null)
            {
                return NotFound();
            }

            return View(ulogaKorisnik);
        }

        // GET: UlogaKorisnik/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UlogaKorisnik/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SifraUloga,OpisUloga")] UlogaKorisnik ulogaKorisnik)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ulogaKorisnik);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ulogaKorisnik);
        }

        // GET: UlogaKorisnik/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ulogaKorisnik = await _context.UlogaKorisnik.FindAsync(id);
            if (ulogaKorisnik == null)
            {
                return NotFound();
            }
            return View(ulogaKorisnik);
        }

        // POST: UlogaKorisnik/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SifraUloga,OpisUloga")] UlogaKorisnik ulogaKorisnik)
        {
            if (id != ulogaKorisnik.SifraUloga)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ulogaKorisnik);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UlogaKorisnikExists(ulogaKorisnik.SifraUloga))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(ulogaKorisnik);
        }

        // GET: UlogaKorisnik/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ulogaKorisnik = await _context.UlogaKorisnik
                .FirstOrDefaultAsync(m => m.SifraUloga == id);
            if (ulogaKorisnik == null)
            {
                return NotFound();
            }

            return View(ulogaKorisnik);
        }

        // POST: UlogaKorisnik/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ulogaKorisnik = await _context.UlogaKorisnik.FindAsync(id);
            _context.UlogaKorisnik.Remove(ulogaKorisnik);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UlogaKorisnikExists(int id)
        {
            return _context.UlogaKorisnik.Any(e => e.SifraUloga == id);
        }
    }
}
