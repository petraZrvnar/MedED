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
    public class KlasaRizikController : Controller
    {
        private readonly MedEdContext _context;

        public KlasaRizikController(MedEdContext context)
        {
            _context = context;
        }

        // GET: KlasaRizik
        public async Task<IActionResult> Index()
        {
            return View(await _context.KlasaRizik.ToListAsync());
        }

        // GET: KlasaRizik/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var klasaRizik = await _context.KlasaRizik
                .FirstOrDefaultAsync(m => m.SifraKlasa == id);
            if (klasaRizik == null)
            {
                return NotFound();
            }

            return View(klasaRizik);
        }

        // GET: KlasaRizik/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: KlasaRizik/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SifraKlasa,OznKlasaRizika")] KlasaRizik klasaRizik)
        {
            if (ModelState.IsValid)
            {
                _context.Add(klasaRizik);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(klasaRizik);
        }

        // GET: KlasaRizik/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var klasaRizik = await _context.KlasaRizik.FindAsync(id);
            if (klasaRizik == null)
            {
                return NotFound();
            }
            return View(klasaRizik);
        }

        // POST: KlasaRizik/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SifraKlasa,OznKlasaRizika")] KlasaRizik klasaRizik)
        {
            if (id != klasaRizik.SifraKlasa)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(klasaRizik);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KlasaRizikExists(klasaRizik.SifraKlasa))
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
            return View(klasaRizik);
        }

        // GET: KlasaRizik/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var klasaRizik = await _context.KlasaRizik
                .FirstOrDefaultAsync(m => m.SifraKlasa == id);
            if (klasaRizik == null)
            {
                return NotFound();
            }

            return View(klasaRizik);
        }

        // POST: KlasaRizik/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var klasaRizik = await _context.KlasaRizik.FindAsync(id);
            _context.KlasaRizik.Remove(klasaRizik);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KlasaRizikExists(int id)
        {
            return _context.KlasaRizik.Any(e => e.SifraKlasa == id);
        }
    }
}
