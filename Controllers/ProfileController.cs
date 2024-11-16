using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAppi.Data;
using WebAppi.Models;

namespace WebAppi.Controllers;

public class ProfileController : Controller
{
    private readonly AppDbContext _context;

    public ProfileController(AppDbContext context)
    {
        _context = context;
    }

    // Перегляд профілю
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        // Перевіряємо сесію
        var userEmail = HttpContext.Session.GetString("UserEmail");

        if (string.IsNullOrEmpty(userEmail))
        {
            return RedirectToAction("Login", "Account"); // Якщо сесія пуста, перенаправляємо на логін
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        return View(user);
    }

    // Редагування профілю
    [HttpGet]
    public async Task<IActionResult> Edit()
    {
        // Перевірка сесії
        var userEmail = HttpContext.Session.GetString("UserEmail");
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);

        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var model = new EditProfileViewModel
        {
            Name = user.Name,
            Email = user.Email
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditProfileViewModel model)
    {
        if (ModelState.IsValid)
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            user.Name = model.Name;
            user.Email = model.Email;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        return View(model);
    }
}
