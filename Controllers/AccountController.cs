using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAppi.Data;
using WebAppi.Models;

namespace WebAppi.Controllers;
[Route("[controller]/[action]")]
public class AccountController : Controller
{
    private readonly AppDbContext _context;

    public AccountController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        // Знаходимо користувача за email
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        if (user == null)
        {
            // Якщо користувача не знайдено, додаємо помилку до ModelState
            ModelState.AddModelError("", "Користувача з таким email не знайдено.");
            return View(); // Повертаємо ту саму сторінку з помилкою
        }

        // Перевірка пароля
        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            // Якщо пароль неправильний, додаємо помилку до ModelState
            ModelState.AddModelError("", "Неправильний пароль.");
            return View();
        }

        // Логіка авторизації (можна додати сесії або кукі тут)
        return RedirectToAction("Index", "Home"); // Перенаправлення на головну сторінку
    }


    [HttpGet]
    public IActionResult Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register(string email, string password, string name)
    {
        // Перевірка чи вже існує користувач із цим email
        if (await _context.Users.AnyAsync(u => u.Email == email))
        {
            // Додаємо повідомлення про помилку до ModelState
            ModelState.AddModelError("", "Користувач із таким email вже існує.");
            return View(); // Повертаємо ту саму сторінку з помилкою
        }

        // Якщо користувача немає, створюємо нового
        var user = new User
        {
            Email = email,
            Name = name,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Перенаправляємо на головну сторінку після успішної реєстрації
        return RedirectToAction("Index", "Home");
    }

}
