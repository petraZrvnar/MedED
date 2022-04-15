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
    public class VrstaLjekarnaController : Controller
    {
        private readonly MedEdContext _context;

        public VrstaLjekarnaController(MedEdContext context)
        {
            _context = context;
        }

        // GET: VrstaLjekarna
        public async Task<IActionResult> Index()
        {
            return View(await _context.VrstaLjekarna.ToListAsync());
        }

        // GET: VrstaLjekarna/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vrstaLjekarna = await _context.VrstaLjekarna
                .FirstOrDefaultAsync(m => m.SifraVrstaLjekarna == id);
            if (vrstaLjekarna == null)
            {
                return NotFound();
            }

            return View(vrstaLjekarna);
        }

        // GET: VrstaLjekarna/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: VrstaLjekarna/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SifraVrstaLjekarna,OpisVrstaLjekarna")] VrstaLjekarna vrstaLjekarna)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vrstaLjekarna);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vrstaLjekarna);
        }

        // GET: VrstaLjekarna/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vrstaLjekarna = await _context.VrstaLjekarna.FindAsync(id);
            if (vrstaLjekarna == null)
            {
                return NotFound();
            }
            return View(vrstaLjekarna);
        }

        // POST: VrstaLjekarna/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SifraVrstaLjekarna,OpisVrstaLjekarna")] VrstaLjekarna vrstaLjekarna)
        {
            if (id != vrstaLjekarna.SifraVrstaLjekarna)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vrstaLjekarna);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VrstaLjekarnaExists(vrstaLjekarna.SifraVrstaLjekarna))
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
            return View(vrstaLjekarna);
        }

        // GET: VrstaLjekarna/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vrstaLjekarna = await _context.VrstaLjekarna
                .FirstOrDefaultAsync(m => m.SifraVrstaLjekarna == id);
            if (vrstaLjekarna == null)
            {
                return NotFound();
            }

            return View(vrstaLjekarna);
        }

        // POST: VrstaLjekarna/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vrstaLjekarna = await _context.VrstaLjekarna.FindAsync(id);
            _context.VrstaLjekarna.Remove(vrstaLjekarna);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VrstaLjekarnaExists(int id)
        {
            return _context.VrstaLjekarna.Any(e => e.SifraVrstaLjekarna == id);
        }
    }
}
