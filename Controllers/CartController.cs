using Microsoft.AspNetCore.Mvc;
using WebAppi.Data;
using WebAppi.Models;

namespace WebAppi.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _context;
        private static List<Shoe> _cart = new List<Shoe>();

        public CartController(AppDbContext context)
        {
            _context = context;
        }

        // Додавання кросівок до кошика
        public IActionResult AddToCart(int id)
        {
            var shoe = _context.Shoes.FirstOrDefault(s => s.Id == id);
            if (shoe != null)
            {
                _cart.Add(shoe);
            }
            return RedirectToAction("Cart");
        }

        // Перегляд кошика
        public IActionResult Cart()
        {
            return View(_cart);
        }

        // Ви можете видалити цей метод, якщо більше не використовуєте статичний список
        private List<Shoe> ShoeList()
        {
            return _context.Shoes.ToList();
        }
    }
}