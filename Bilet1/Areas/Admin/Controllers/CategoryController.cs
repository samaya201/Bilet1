using Bilet1.Context;
using Bilet1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Bilet1.Areas.Admin.Controllers;
[Area("Admin")]

public class CategoryController(AppDbContext _context) : Controller
{
    public async Task<IActionResult> Index()
    {
        //Categorylari ekranda gosterir
        var categories = await _context.Categories.ToListAsync();
        return View(categories);
    }

    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(Category category)
    {
        if(!ModelState.IsValid)
        {
            return View(category);
        }

        Category newCategory = new()
        {
            Name = category.Name
        };
        await _context.Categories.AddAsync(newCategory);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");

    }

    public async Task<IActionResult> UpdateAsync(int id)
    {
        var categories = await _context.Categories.FindAsync(id);
        return View(categories);
    }

    [HttpPost]
    public async Task<IActionResult> Update(Category category)
    {
        if (!ModelState.IsValid)
        {
            return View(category);
        }
        var existCategory = await _context.Categories.FindAsync(category.Id);
        if (existCategory is null)
        {
            return NotFound();
        }
        existCategory.Name = category.Name;

        _context.Categories.Update(existCategory);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Delete(int id)
    {
        var category = await _context.Categories.FindAsync(id); 
        if(category is null)
        {
            return NotFound();
        }
        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");

    }
}
