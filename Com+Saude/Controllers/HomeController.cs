using Com_Saude.Data; // Asegúrate de tener el using correcto
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly Com_SaudeContext _context;

    public HomeController(ILogger<HomeController> logger, Com_SaudeContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var especialidades = await _context.Especialidade.ToListAsync();
        return View(especialidades);
    }

    // ... resto del código



    public IActionResult Privacy()
    {
        return View();
    }

    

}

