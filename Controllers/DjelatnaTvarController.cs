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
    public class DjelatnaTvarController : Controller
    {
        private readonly MedEdContext _context;

        public DjelatnaTvarController(MedEdContext context)
        {
            _context = context;
        }

        // GET: DjelatnaTvar
        public async Task<IActionResult> Index()
        {
            return View(await _context.DjelatnaTvar.ToListAsync());
        }

        // GET: DjelatnaTvar/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var djelatnaTvar = await _context.DjelatnaTvar
                .FirstOrDefaultAsync(m => m.SifraDjelatnaTvar == id);
            if (djelatnaTvar == null)
            {
                return NotFound();
            }

            return View(djelatnaTvar);
        }

        // GET: DjelatnaTvar/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DjelatnaTvar/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SifraDjelatnaTvar,NazivDjelatnaTvar")] DjelatnaTvar djelatnaTvar)
        {
            if (ModelState.IsValid)
            {
                _context.Add(djelatnaTvar);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(djelatnaTvar);
        }

        // GET: DjelatnaTvar/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var djelatnaTvar = await _context.DjelatnaTvar.FindAsync(id);
            if (djelatnaTvar == null)
            {
                return NotFound();
            }
            return View(djelatnaTvar);
        }

        // POST: DjelatnaTvar/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SifraDjelatnaTvar,NazivDjelatnaTvar")] DjelatnaTvar djelatnaTvar)
        {
            if (id != djelatnaTvar.SifraDjelatnaTvar)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(djelatnaTvar);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DjelatnaTvarExists(djelatnaTvar.SifraDjelatnaTvar))
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
            return View(djelatnaTvar);
        }

        // GET: DjelatnaTvar/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var djelatnaTvar = await _context.DjelatnaTvar
                .FirstOrDefaultAsync(m => m.SifraDjelatnaTvar == id);
            if (djelatnaTvar == null)
            {
                return NotFound();
            }

            return View(djelatnaTvar);
        }

        // POST: DjelatnaTvar/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var djelatnaTvar = await _context.DjelatnaTvar.FindAsync(id);
            _context.DjelatnaTvar.Remove(djelatnaTvar);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DjelatnaTvarExists(int id)
        {
            return _context.DjelatnaTvar.Any(e => e.SifraDjelatnaTvar == id);
        }
    }
}
