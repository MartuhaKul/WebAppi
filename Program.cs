using Microsoft.EntityFrameworkCore;
using WebAppi.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")));


// Додаємо служб для MVC
builder.Services.AddControllersWithViews();

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

// Маршрутизація до контролерів і представлень
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();