using System.Threading.Tasks;
using Bilet1.Context;
using Bilet1.Helpers;
using Bilet1.Models;
using Bilet1.ViewModels.ServiceViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace Bilet1.Areas.Admin.Controllers;
[Area("Admin")]
public class ServiceController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _environment;
    private readonly string _folderPath;

    public ServiceController(AppDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
        _folderPath = Path.Combine(_environment.WebRootPath,"img");
    }

    public async Task<IActionResult> Index()
    {
        var services = await _context.Services.Select(service => new ServiceGetVM()
        {
            Id=service.Id,
            Title=service.Title,
            Description=service.Description,
            CategoryName=service.category.Name,
            ImagePath=service.ImagePath

        }).ToListAsync();

        return View(services);
    }
    public async Task<IActionResult> Create()
    {
        await SendCategoriesWithViewbag();
        return View();

    }
    [HttpPost]

    public async Task<IActionResult> Create(ServiceCreateVM vm)
    {
        await SendCategoriesWithViewbag();
        if (!ModelState.IsValid)
            return View(vm);

        //Check categories

        var isExistCategory = await _context.Categories.AnyAsync(x => x.Id == vm.CategoryId);

        if (!isExistCategory)
        {
            ModelState.AddModelError("CategoryId", "Category id not found!");
            return View(vm);
        }

        //Check image size

        if (!vm.Image.CheckSize(2))
        {
            ModelState.AddModelError("Image", "Image size must be less than 2 mb");
            return View(vm);
        }

        //Check file type

        if (!vm.Image.CheckType("image"))
        {
            ModelState.AddModelError("", "File type must be image type");
            return View(vm);
        }

        //Create image

        string uniqueFileName = await vm.Image.FileUploadToAsync(_folderPath);

        Service service = new()
        {
            Title = vm.Title,
            Description = vm.Description,
            CategoryId = vm.CategoryId,
            ImagePath = uniqueFileName
        };
       await _context.Services.AddAsync(service);
        await _context.SaveChangesAsync();





        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var services = await _context.Services.FindAsync(id);
        if (services is null)
            return NotFound();
        _context.Services.Remove(services);
        await _context.SaveChangesAsync();

        //Delete image

        string deletedFilePath = Path.Combine(_folderPath, services.ImagePath);
        FileHelper.ImageDelete(deletedFilePath);


        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Update(int id)
    {
        await SendCategoriesWithViewbag();
        var service = await _context.Services.FindAsync(id);
        if (service is null)
            return NotFound();

        ServiceUpdateVM vm = new()
        {
            Id = service.Id,
            Title = service.Title,
            Description = service.Description,
            CategoryId = service.category.Id
            
        };

        return View(vm);
    }

    [HttpPost]

    public async Task<IActionResult> Update(ServiceUpdateVM vm)
    {
        await SendCategoriesWithViewbag();
        if (!ModelState.IsValid)
            return View(vm);
        

        //Find service for update
        var existService = await _context.Services.FindAsync(vm.Id);
        if (existService is null)
            return BadRequest();

        //Check categories

        var isExistCategory = await _context.Categories.AnyAsync(x => x.Id == vm.CategoryId);

        if (!isExistCategory)
        {
            ModelState.AddModelError("CategoryId", "Category id not found!");
            return View(vm);
        }

        //Check image size

        if (!vm.Image?.CheckSize(2) ?? false)
        {
            ModelState.AddModelError("Image", "Image size must be less than 2 mb");
            return View(vm);
        }

        //Check file type

        if (!vm.Image?.CheckType("image") ?? false)
        {
            ModelState.AddModelError("", "File type must be image type");
            return View(vm);
        }

        //Update
        existService.Title = vm.Title;
        existService.Description = vm.Description;
        existService.CategoryId = vm.CategoryId;

        if(vm.Image is { })
        {
            string newImagePath = await vm.Image.FileUploadToAsync(_folderPath);
            string deletPath = Path.Combine(_folderPath, existService.ImagePath);
            FileHelper.ImageDelete(deletPath);
            existService.ImagePath = newImagePath;
        }
        _context.Services.Update(existService);
        await _context.SaveChangesAsync();



        return RedirectToAction(nameof(Index));
    }


    private async Task SendCategoriesWithViewbag()
    {
        var categories = await _context.Categories.ToListAsync();
        ViewBag.Categories = categories;
    }
}
