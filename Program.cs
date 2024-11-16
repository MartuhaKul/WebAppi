using Microsoft.EntityFrameworkCore;
using WebAppi.Data;

var builder = WebApplication.CreateBuilder(args);

// Налаштовуємо контекст бази даних для використання MySQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")));

// Додаємо служб для MVC
builder.Services.AddControllersWithViews();

// Налаштовуємо сесії для збереження ідентифікаторів користувачів
builder.Services.AddDistributedMemoryCache(); // Додаємо кеш для сесій
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true; // Тільки для сервера доступ до сесії
    options.Cookie.IsEssential = true; // Обов'язково
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Час до завершення сесії
});

var app = builder.Build();

// Налаштовуємо маршрутизацію
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Дозволяє обробляти статичні файли з wwwroot

// Використовуємо сесії
app.UseSession();

// Маршрутизація до контролерів і представлень
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();