using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Com_Saude.Data;
using Com_Saude.Models;

namespace Com_Saude.Controllers
{
    public class PacientesController : Controller
    {
        private readonly Com_SaudeContext _context;

        public PacientesController(Com_SaudeContext context)
        {
            _context = context;
        }

        // GET: Pacientes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Paciente.ToListAsync());
        }

        // GET: Pacientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var paciente = await _context.Paciente
                .FirstOrDefaultAsync(m => m.IdPaciente == id);

            if (paciente == null)
                return NotFound();

            return View(paciente);
        }

        // GET: Pacientes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Pacientes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Paciente paciente)
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

                paciente.UrlFoto = "/fotos/" + fileName;
            }
            else
            {
                paciente.UrlFoto = "/fotos/default.png";
            }

            if (ModelState.IsValid)
            {
                _context.Add(paciente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(paciente);
        }

        // GET: Pacientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var paciente = await _context.Paciente.FindAsync(id);

            if (paciente == null)
                return NotFound();

            return View(paciente);
        }

        // POST: Pacientes/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Paciente paciente)
        {
            if (id != paciente.IdPaciente)
                return NotFound();

            var pacienteBanco = await _context.Paciente.AsNoTracking().FirstOrDefaultAsync(p => p.IdPaciente == id);

            if (pacienteBanco == null)
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

                paciente.UrlFoto = "/fotos/" + fileName;
            }
            else
            {
                paciente.UrlFoto = pacienteBanco.UrlFoto; // Mantener foto actual
            }

            if (!ModelState.IsValid)
                return View(paciente);

            try
            {
                _context.Update(paciente);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PacienteExists(paciente.IdPaciente))
                    return NotFound();
                else
                    throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Pacientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var paciente = await _context.Paciente
                .FirstOrDefaultAsync(m => m.IdPaciente == id);

            if (paciente == null)
                return NotFound();

            return View(paciente);
        }

        // POST: Pacientes/DeleteConfirmed
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var paciente = await _context.Paciente.FindAsync(id);

            if (paciente != null)
                _context.Paciente.Remove(paciente);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PacienteExists(int id)
        {
            return _context.Paciente.Any(e => e.IdPaciente == id);
        }
    }
}
