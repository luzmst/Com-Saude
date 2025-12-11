using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Com_Saude.Data;
using Com_Saude.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace Com_Saude.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly Com_SaudeContext _context;

        public UsuariosController(Com_SaudeContext context)
        {
            _context = context;
        }

       

[HttpGet]
    [AllowAnonymous]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(string email, string senha)
    {
        var usuario = _context.Usuario.FirstOrDefault(u => u.Email == email && u.Senha == senha);
        if (usuario != null)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, usuario.Nome),
            new Claim(ClaimTypes.Email, usuario.Email),
            new Claim("TipoUsuarioId", usuario.TipoUsuarioId.ToString())
        };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction("Index", "Home");
        }
        ViewBag.Erro = "Email ou senha inválidos.";
        return View();
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }

    // GET: Usuarios
    public async Task<IActionResult> Index()
        {
            var com_SaudeContext = _context.Usuario.Include(u => u.TipoUsuario);
            return View(await com_SaudeContext.ToListAsync());
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var usuario = await _context.Usuario
                .Include(u => u.TipoUsuario)
                .FirstOrDefaultAsync(m => m.IdUsuario == id);

            if (usuario == null)
                return NotFound();

            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            ViewData["TipoUsuarioId"] = new SelectList(_context.TipoUsuario, "IdTipoUsuario", "DescricaoTipoUsuario");
            return View();
        }

        // POST: Usuarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Usuario usuario)
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

                usuario.UrlFoto = "/fotos/" + fileName;
            }
            else
            {
                usuario.UrlFoto = "/fotos/default.png";
            }

            if (ModelState.IsValid)
            {
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["TipoUsuarioId"] = new SelectList(_context.TipoUsuario, "IdTipoUsuario", "DescricaoTipoUsuario", usuario.TipoUsuarioId);
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario == null)
                return NotFound();

            ViewData["TipoUsuarioId"] = new SelectList(_context.TipoUsuario, "IdTipoUsuario", "DescricaoTipoUsuario", usuario.TipoUsuarioId);
            return View(usuario);
        }

        // POST: Usuarios/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Usuario usuario)
        {
            if (id != usuario.IdUsuario)
                return NotFound();

            var usuarioBanco = await _context.Usuario.AsNoTracking().FirstOrDefaultAsync(u => u.IdUsuario == id);

            if (usuarioBanco == null)
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

                usuario.UrlFoto = "/fotos/" + fileName;
            }
            else
            {
                usuario.UrlFoto = usuarioBanco.UrlFoto; // Mantener foto actual
            }

            if (!ModelState.IsValid)
            {
                ViewData["TipoUsuarioId"] = new SelectList(_context.TipoUsuario, "IdTipoUsuario", "DescricaoTipoUsuario", usuario.TipoUsuarioId);
                return View(usuario);
            }

            try
            {
                _context.Update(usuario);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(usuario.IdUsuario))
                    return NotFound();
                else
                    throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var usuario = await _context.Usuario
                .Include(u => u.TipoUsuario)
                .FirstOrDefaultAsync(m => m.IdUsuario == id);

            if (usuario == null)
                return NotFound();

            return View(usuario);
        }

        // POST: Usuarios/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuario.FindAsync(id);

            if (usuario != null)
                _context.Usuario.Remove(usuario);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuario.Any(e => e.IdUsuario == id);
        }
    }
}
