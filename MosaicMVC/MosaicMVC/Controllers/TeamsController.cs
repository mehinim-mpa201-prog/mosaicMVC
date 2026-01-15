using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MosaicMVC.Contexts;

namespace MosaicMVC.Controllers
{
    public class TeamsController : Controller
    {
        private readonly AppDbContext _context;

        public TeamsController(AppDbContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> Index()
        {
            var teams = await _context.Teams.Include(t => t.Position).ToListAsync();
            return View(teams);
        }
    }
}
