using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MosaicMVC.Contexts;
using System.Threading.Tasks;

namespace MosaicMVC.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var teams = await _context.Teams.Include(t=>t.Position).ToListAsync();
        return View(teams);
    }
}
