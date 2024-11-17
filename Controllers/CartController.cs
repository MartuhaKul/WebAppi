using Microsoft.AspNetCore.Mvc;
using WebAppi.Data;
using WebAppi.Models;

namespace WebAppi.Controllers
{
   // Контролер для кошика
public class CartController : Controller
{
    private readonly AppDbContext _context;
    private static List<CartItem> _cart = new List<CartItem>();

    public CartController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult AddToCart(int id, int quantity = 1)
    {
        var shoe = _context.Shoes.FirstOrDefault(s => s.Id == id);
        if (shoe != null)
        {
            var existingItem = _cart.FirstOrDefault(c => c.Shoe.Id == id);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                _cart.Add(new CartItem { Shoe = shoe, Quantity = quantity });
            }
        }
        return RedirectToAction("Cart");
    }

    public IActionResult Cart()
    {
        return View(_cart);
    }

    // Додатково: видалення кросівок з кошика
    public IActionResult RemoveFromCart(int id)
    {
        var item = _cart.FirstOrDefault(c => c.Shoe.Id == id);
        if (item != null)
        {
            _cart.Remove(item);
        }
        return RedirectToAction("Cart");
    }

    // Перехід на форму для замовлення
    public IActionResult Checkout()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Checkout(Order order)
    {
        if (ModelState.IsValid)
        {
            order.OrderDate = DateTime.Now;
            _context.Orders.Add(order);

            // Додаємо товари в замовлення
            foreach (var cartItem in _cart)
            {
                var orderItem = new OrderItem
                {
                    ShoeId = cartItem.Shoe.Id,
                    Quantity = cartItem.Quantity,
                    Price = cartItem.Shoe.Price,
                    Order = order
                };
                _context.OrderItems.Add(orderItem);
            }

            _context.SaveChanges();

            // Очищаємо кошик після замовлення
            _cart.Clear();

            return RedirectToAction("OrderSuccess");
        }
        return View(order);
    }

    public IActionResult OrderSuccess()
    {
        return View();
    }
}

// Модель для елемента кошика
public class CartItem
{
    public Shoe Shoe { get; set; }
    public int Quantity { get; set; }
}

}