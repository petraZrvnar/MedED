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
    public class NositeljOdobrenjaController : Controller
    {
        private readonly MedEdContext _context;

        public NositeljOdobrenjaController(MedEdContext context)
        {
            _context = context;
        }

        // GET: NositeljOdobrenja
        public async Task<IActionResult> Index()
        {
            return View(await _context.NositeljOdobrenja.ToListAsync());
        }

        // GET: NositeljOdobrenja/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nositeljOdobrenja = await _context.NositeljOdobrenja
                .FirstOrDefaultAsync(m => m.SifraNositelj == id);
            if (nositeljOdobrenja == null)
            {
                return NotFound();
            }

            return View(nositeljOdobrenja);
        }

        // GET: NositeljOdobrenja/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: NositeljOdobrenja/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SifraNositelj,NazivNositelj")] NositeljOdobrenja nositeljOdobrenja)
        {
            if (ModelState.IsValid)
            {
                _context.Add(nositeljOdobrenja);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(nositeljOdobrenja);
        }

        // GET: NositeljOdobrenja/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nositeljOdobrenja = await _context.NositeljOdobrenja.FindAsync(id);
            if (nositeljOdobrenja == null)
            {
                return NotFound();
            }
            return View(nositeljOdobrenja);
        }

        // POST: NositeljOdobrenja/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SifraNositelj,NazivNositelj")] NositeljOdobrenja nositeljOdobrenja)
        {
            if (id != nositeljOdobrenja.SifraNositelj)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nositeljOdobrenja);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NositeljOdobrenjaExists(nositeljOdobrenja.SifraNositelj))
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
            return View(nositeljOdobrenja);
        }

        // GET: NositeljOdobrenja/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nositeljOdobrenja = await _context.NositeljOdobrenja
                .FirstOrDefaultAsync(m => m.SifraNositelj == id);
            if (nositeljOdobrenja == null)
            {
                return NotFound();
            }

            return View(nositeljOdobrenja);
        }

        // POST: NositeljOdobrenja/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var nositeljOdobrenja = await _context.NositeljOdobrenja.FindAsync(id);
            _context.NositeljOdobrenja.Remove(nositeljOdobrenja);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NositeljOdobrenjaExists(int id)
        {
            return _context.NositeljOdobrenja.Any(e => e.SifraNositelj == id);
        }
    }
}
