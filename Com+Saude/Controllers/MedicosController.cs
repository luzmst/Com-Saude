using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Com_Saude.Data;
using Com_Saude.Models;

namespace Com_Saude.Controllers
{
    public class MedicosController : Controller
    {
        private readonly Com_SaudeContext _context;

        public MedicosController(Com_SaudeContext context)
        {
            _context = context;
        }

        // GET: Medicos
        public async Task<IActionResult> Index()
        {
            var medicos = _context.Medico.Include(m => m.Especialidade);
            return View(await medicos.ToListAsync());
        }

        // GET: Medicos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var medico = await _context.Medico
                .Include(m => m.Especialidade)
                .FirstOrDefaultAsync(m => m.IdMedico == id);

            if (medico == null)
                return NotFound();

            return View(medico);
        }

        // GET: Medicos/Create
        public IActionResult Create()
        {
            ViewData["EspecialidadeId"] = new SelectList(_context.Especialidade, "IdEspecialidade", "Descricao");
            return View();
        }

        // POST: Medicos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Medico medico)
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

                medico.UrlFoto = "/fotos/" + fileName;
            }
            else
            {
                medico.UrlFoto = "/fotos/default.png";
            }

            if (ModelState.IsValid)
            {
                _context.Add(medico);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["EspecialidadeId"] = new SelectList(_context.Especialidade, "IdEspecialidade", "Descricao", medico.EspecialidadeId);
            return View(medico);
        }

        // GET: Medicos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var medico = await _context.Medico.FindAsync(id);

            if (medico == null)
                return NotFound();

            ViewData["EspecialidadeId"] = new SelectList(_context.Especialidade, "IdEspecialidade", "Descricao", medico.EspecialidadeId);
            return View(medico);
        }

        // POST: Medicos/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Medico medico)
        {
            if (id != medico.IdMedico)
                return NotFound();

            var medicoBanco = await _context.Medico.AsNoTracking().FirstOrDefaultAsync(m => m.IdMedico == id);

            if (medicoBanco == null)
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

                medico.UrlFoto = "/fotos/" + fileName;
            }
            else
            {
                medico.UrlFoto = medicoBanco.UrlFoto; // Mantener foto anterior
            }

            if (!ModelState.IsValid)
            {
                ViewData["EspecialidadeId"] = new SelectList(_context.Especialidade, "IdEspecialidade", "Descricao", medico.EspecialidadeId);
                return View(medico);
            }

            try
            {
                _context.Update(medico);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MedicoExists(medico.IdMedico))
                    return NotFound();
                else
                    throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Medicos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var medico = await _context.Medico
                .Include(m => m.Especialidade)
                .FirstOrDefaultAsync(m => m.IdMedico == id);

            if (medico == null)
                return NotFound();

            return View(medico);
        }

        // POST: Medicos/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var medico = await _context.Medico.FindAsync(id);

            if (medico != null)
                _context.Medico.Remove(medico);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MedicoExists(int id)
        {
            return _context.Medico.Any(e => e.IdMedico == id);
        }
    }
}
