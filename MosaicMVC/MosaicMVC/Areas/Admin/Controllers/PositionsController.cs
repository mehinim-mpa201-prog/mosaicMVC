using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MosaicMVC.Areas.Admin.ViewModels;
using MosaicMVC.Contexts;
using MosaicMVC.Models;
using System.Threading.Tasks;

namespace MosaicMVC.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,Moderator")]
public class PositionsController : Controller
{
    private readonly AppDbContext _context;

    public PositionsController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        List<Position> positions = await _context.Positions.ToListAsync();
        return View(positions);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(PositionCreateVM positionCreateVM)
    {
        if (!ModelState.IsValid)
        {
            return View(positionCreateVM);
        }
        Position position = new();
        position.Name = positionCreateVM.Name;

        _context.Positions.Add(position);
        _context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }


    [HttpGet]
    public IActionResult Update(int id)
    {
        Position? position = _context.Positions.Find(id);
        if (position == null) return NotFound();
        return View(position);
    }

    [HttpPost]
    public IActionResult Update(Position position)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }


        Position? basePosition = _context.Positions.Find(position.Id);
        if (basePosition == null) return NotFound();

        basePosition.Name = position.Name;

        _context.Positions.Update(basePosition);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));

    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        Position? position = await _context.Positions.FindAsync(id);
        if (position == null) return NotFound();
        _context.Positions.Remove(position);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
