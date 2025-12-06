using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Com_Saude.Data;
using Com_Saude.Models;

namespace Com_Saude.Controllers
{
    public class RecepcionistasController : Controller
    {
        private readonly Com_SaudeContext _context;

        public RecepcionistasController(Com_SaudeContext context)
        {
            _context = context;
        }

        // GET: Recepcionistas
        public async Task<IActionResult> Index()
        {
            return View(await _context.Recepcionista.ToListAsync());
        }

        // GET: Recepcionistas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var recepcionista = await _context.Recepcionista
                .FirstOrDefaultAsync(m => m.IdRecepcionista == id);

            if (recepcionista == null)
                return NotFound();

            return View(recepcionista);
        }

        // GET: Recepcionistas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Recepcionistas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Recepcionista recepcionista)
        {
            var fotoUpload = Request.Form.Files["FotoUpload"];

            if (fotoUpload != null && fotoUpload.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(fotoUpload.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/fotos", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await fotoUpload.CopyToAsync(stream);
                }

                recepcionista.UrlFoto = "/fotos/" + fileName;
            }
            else
            {
                recepcionista.UrlFoto = "/fotos/";
            }

            if (ModelState.IsValid)
            {
                _context.Add(recepcionista);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(recepcionista);
        }

        // GET: Recepcionistas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var recepcionista = await _context.Recepcionista.FindAsync(id);
            if (recepcionista == null)
                return NotFound();

            return View(recepcionista);
        }

        // POST: Recepcionistas/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Recepcionista recepcionista)
        {
            if (id != recepcionista.IdRecepcionista)
                return NotFound();

            var recepcionistaBanco = await _context.Recepcionista
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.IdRecepcionista == id);

            if (recepcionistaBanco == null)
                return NotFound();

            var fotoUpload = Request.Form.Files["FotoUpload"];

            if (fotoUpload != null && fotoUpload.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(fotoUpload.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/fotos", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await fotoUpload.CopyToAsync(stream);
                }

                recepcionista.UrlFoto = "/fotos/" + fileName;
            }
            else
            {
                // Mantener foto actual
                recepcionista.UrlFoto = recepcionistaBanco.UrlFoto;
            }

            if (!ModelState.IsValid)
                return View(recepcionista);

            try
            {
                _context.Update(recepcionista);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecepcionistaExists(recepcionista.IdRecepcionista))
                    return NotFound();
                else
                    throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Recepcionistas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var recepcionista = await _context.Recepcionista
                .FirstOrDefaultAsync(m => m.IdRecepcionista == id);

            if (recepcionista == null)
                return NotFound();

            return View(recepcionista);
        }

        // POST: Recepcionistas/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recepcionista = await _context.Recepcionista.FindAsync(id);

            if (recepcionista != null)
                _context.Recepcionista.Remove(recepcionista);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecepcionistaExists(int id)
        {
            return _context.Recepcionista.Any(e => e.IdRecepcionista == id);
        }
    }
}
