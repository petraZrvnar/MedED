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
    public class FarmaceutskiOblikController : Controller
    {
        private readonly MedEdContext _context;

        public FarmaceutskiOblikController(MedEdContext context)
        {
            _context = context;
        }

        // GET: FarmaceutskiOblik
        public async Task<IActionResult> Index()
        {
            return View(await _context.FarmaceutskiOblik.ToListAsync());
        }

        // GET: FarmaceutskiOblik/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var farmaceutskiOblik = await _context.FarmaceutskiOblik
                .FirstOrDefaultAsync(m => m.SifraFarmOblik == id);
            if (farmaceutskiOblik == null)
            {
                return NotFound();
            }

            return View(farmaceutskiOblik);
        }

        // GET: FarmaceutskiOblik/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FarmaceutskiOblik/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SifraFarmOblik,OpisFarmOblik")] FarmaceutskiOblik farmaceutskiOblik)
        {
            if (ModelState.IsValid)
            {
                _context.Add(farmaceutskiOblik);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(farmaceutskiOblik);
        }

        // GET: FarmaceutskiOblik/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var farmaceutskiOblik = await _context.FarmaceutskiOblik.FindAsync(id);
            if (farmaceutskiOblik == null)
            {
                return NotFound();
            }
            return View(farmaceutskiOblik);
        }

        // POST: FarmaceutskiOblik/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SifraFarmOblik,OpisFarmOblik")] FarmaceutskiOblik farmaceutskiOblik)
        {
            if (id != farmaceutskiOblik.SifraFarmOblik)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(farmaceutskiOblik);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FarmaceutskiOblikExists(farmaceutskiOblik.SifraFarmOblik))
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
            return View(farmaceutskiOblik);
        }

        // GET: FarmaceutskiOblik/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var farmaceutskiOblik = await _context.FarmaceutskiOblik
                .FirstOrDefaultAsync(m => m.SifraFarmOblik == id);
            if (farmaceutskiOblik == null)
            {
                return NotFound();
            }

            return View(farmaceutskiOblik);
        }

        // POST: FarmaceutskiOblik/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var farmaceutskiOblik = await _context.FarmaceutskiOblik.FindAsync(id);
            _context.FarmaceutskiOblik.Remove(farmaceutskiOblik);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FarmaceutskiOblikExists(int id)
        {
            return _context.FarmaceutskiOblik.Any(e => e.SifraFarmOblik == id);
        }
    }
}
