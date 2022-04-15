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
    public class VrstaLijekController : Controller
    {
        private readonly MedEdContext _context;

        public VrstaLijekController(MedEdContext context)
        {
            _context = context;
        }

        // GET: VrstaLijek
        public async Task<IActionResult> Index()
        {
            return View(await _context.VrstaLijek.ToListAsync());
        }

        // GET: VrstaLijek/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vrstaLijek = await _context.VrstaLijek
                .FirstOrDefaultAsync(m => m.SifraVrstaLijek == id);
            if (vrstaLijek == null)
            {
                return NotFound();
            }

            return View(vrstaLijek);
        }

        // GET: VrstaLijek/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: VrstaLijek/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SifraVrstaLijek,OpisVrstaLijek")] VrstaLijek vrstaLijek)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vrstaLijek);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vrstaLijek);
        }

        // GET: VrstaLijek/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vrstaLijek = await _context.VrstaLijek.FindAsync(id);
            if (vrstaLijek == null)
            {
                return NotFound();
            }
            return View(vrstaLijek);
        }

        // POST: VrstaLijek/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SifraVrstaLijek,OpisVrstaLijek")] VrstaLijek vrstaLijek)
        {
            if (id != vrstaLijek.SifraVrstaLijek)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vrstaLijek);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VrstaLijekExists(vrstaLijek.SifraVrstaLijek))
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
            return View(vrstaLijek);
        }

        // GET: VrstaLijek/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vrstaLijek = await _context.VrstaLijek
                .FirstOrDefaultAsync(m => m.SifraVrstaLijek == id);
            if (vrstaLijek == null)
            {
                return NotFound();
            }

            return View(vrstaLijek);
        }

        // POST: VrstaLijek/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vrstaLijek = await _context.VrstaLijek.FindAsync(id);
            _context.VrstaLijek.Remove(vrstaLijek);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VrstaLijekExists(int id)
        {
            return _context.VrstaLijek.Any(e => e.SifraVrstaLijek == id);
        }
    }
}
