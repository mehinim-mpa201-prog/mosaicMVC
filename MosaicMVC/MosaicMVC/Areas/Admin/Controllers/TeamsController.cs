using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MosaicMVC.Areas.Admin.ViewModels;
using MosaicMVC.Contexts;
using MosaicMVC.Models;
using System.Threading.Tasks;

namespace MosaicMVC.Areas.Admin.Controllers;

[Area("Admin")]
public class TeamsController : Controller
{
    private readonly AppDbContext _context;

    private readonly IWebHostEnvironment _env;

    public TeamsController(AppDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    public async Task<IActionResult> Index()
    {
        var teams = await _context.Teams.Include(t => t.Position).ToListAsync();
        return View(teams);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var positions = await _context.Positions.ToListAsync();
        ViewBag.Positions = positions;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(TeamCreateVM teamCreateVM)
    {

        if (!ModelState.IsValid)
        {
            var positions = await _context.Positions.ToListAsync();
            ViewBag.Positions = positions;
            return View(teamCreateVM);
        }

        if (teamCreateVM.PositionId == 0)
        {
            ModelState.AddModelError("PositionId", "Zəhmət olmasa bir Position seçin.");
            var positions = await _context.Positions.ToListAsync();
            ViewBag.Positions = positions;
            return View(teamCreateVM);
        }

        Team team = new()
        {
            About = teamCreateVM.About,
            Name = teamCreateVM.Name,
            PositionId = teamCreateVM.PositionId,

        };

        #region AddImage

        string path = Path.Combine(_env.WebRootPath, "admin", "assets", "images", "uploads");
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        string imageName = Guid.NewGuid() + teamCreateVM.Img.FileName;
        using FileStream stream = new(Path.Combine(path, imageName), FileMode.Create);
        teamCreateVM.Img.CopyTo(stream);
        team.ImagePath= imageName;
        #endregion

        await _context.Teams.AddAsync(team);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }


    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        var positions = await _context.Positions.ToListAsync();
        ViewBag.Positions = positions;
        var team = await _context.Teams.Include(t=>t.Position).FirstOrDefaultAsync(t=>t.Id == id);
        if(team is null)
        {
            return NotFound();
        }
        TeamUpdateVM teamUpdateVM = new()
        {
            Name = team.Name,
            About = team.About,
            PositionId = team.PositionId
        };
        return View(teamUpdateVM);
    }

    [HttpPost]
    public async Task<IActionResult> Update(int id,TeamUpdateVM teamUpdateVM)
    {
        if(!ModelState.IsValid)
        {
            var positions = await _context.Positions.ToListAsync();
            ViewBag.Positions = positions;
            return View(teamUpdateVM);
        }
        var team = await _context.Teams.FirstOrDefaultAsync(t => t.Id == id);
        if(team is null)
        {
            return NotFound();
        }

        team.Name = teamUpdateVM.Name;
        team.About = teamUpdateVM.About;
        team.PositionId = teamUpdateVM.PositionId;
        if(teamUpdateVM.Img is not null)
        {
            #region AddImage
            string path = Path.Combine(_env.WebRootPath, "admin", "assets", "images", "uploads");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string imageName = Guid.NewGuid() + teamUpdateVM.Img.FileName;
            using FileStream stream = new(Path.Combine(path, imageName), FileMode.Create);
            teamUpdateVM.Img.CopyTo(stream);
            team.ImagePath = imageName;
            #endregion
        }

         _context.Teams.Update(team);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}
