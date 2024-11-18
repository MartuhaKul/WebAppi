using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAppi.Data;
using WebAppi.Models;

namespace WebAppi.Controllers
{
    [Route("[controller]/[action]")]
    public class CartController : Controller
    {
        private readonly AppDbContext _context;
        private const string CartSessionKey = "Cart";

        public CartController(AppDbContext context)
        {
            _context = context;
        }

        // Додати елемент до кошика
        public IActionResult AddToCart(int id, int quantity = 1)
        {
            var shoe = _context.Shoes.FirstOrDefault(s => s.Id == id);
            if (shoe != null)
            {
                var cart = GetCart();
                var existingItem = cart.FirstOrDefault(c => c.Shoe.Id == id);
                if (existingItem != null)
                {
                    existingItem.Quantity += quantity;
                }
                else
                {
                    cart.Add(new CartItem { Shoe = shoe, Quantity = quantity });
                }
                SaveCart(cart);
            }
            return RedirectToAction("Cart");
        }

        // Показати кошик
        public IActionResult Cart()
        {
            var cart = GetCart();
            return View(cart);
        }

        // Видалити елемент з кошика
        public IActionResult RemoveFromCart(int id)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(c => c.Shoe.Id == id);
            if (item != null)
            {
                cart.Remove(item);
                SaveCart(cart);
            }
            return RedirectToAction("Cart");
        }

        // Показати форму для оформлення замовлення
        [HttpGet]
        public IActionResult Checkout()
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");

            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Login", "Account"); // Якщо сесія пуста, перенаправляємо на логін
            }

            // Створення порожньої моделі для форми
            var order = new Order();
            return View(order); // Передаємо модель у View
        }

        // Обробка даних форми та збереження замовлення
        [HttpPost]
        public async Task<IActionResult> Checkout(Order model)
        {
            if (ModelState.IsValid)
            {
                var userEmail = HttpContext.Session.GetString("UserEmail");

                // Перевірка, чи є користувач у базі
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                // Перевірка, чи кошик не порожній
                var cart = GetCart();
                if (cart == null || cart.Count == 0)
                {
                    return View(model); // Повертаємо на форму, якщо кошик порожній
                }

                // Створення нового замовлення
                var order = new Order
                {
                    UserName = user.Name,  // Використовуємо ім'я користувача з сесії
                    Address = model.Address,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    OrderDate = DateTime.Now
                };

                // Додаємо замовлення в базу
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                // Зберігаємо елементи кошика в таблиці OrderItems
                foreach (var cartItem in cart)
                {
                    var orderItem = new OrderItem
                    {
                        ShoeId = cartItem.Shoe.Id,
                        Quantity = cartItem.Quantity,
                        Price = cartItem.Shoe.Price,
                        OrderId = order.Id
                    };
                    _context.OrderItems.Add(orderItem);
                }
                await _context.SaveChangesAsync();

                // Очистити кошик після оформлення замовлення
                ClearCart();

                // Перенаправлення на сторінку успішного оформлення замовлення
                return RedirectToAction("OrderConfirmation");
            }

            // Якщо модель не валідна, повертаємо на форму
            return View(model);
        }

        // Підтвердження замовлення
        public IActionResult OrderConfirmation()
        {
            return View();
        }

        // Отримати кошик з сесії
        private List<CartItem> GetCart()
        {
            var cart = HttpContext.Session.GetString(CartSessionKey);
            if (string.IsNullOrEmpty(cart))
            {
                return new List<CartItem>();
            }
            return Newtonsoft.Json.JsonConvert.DeserializeObject<List<CartItem>>(cart);
        }

        // Зберегти кошик в сесії
        private void SaveCart(List<CartItem> cart)
        {
            HttpContext.Session.SetString(CartSessionKey, Newtonsoft.Json.JsonConvert.SerializeObject(cart));
        }

        // Очистити кошик
        private void ClearCart()
        {
            HttpContext.Session.Remove(CartSessionKey);
        }
    }

    // Модель для елемента кошика
    public class CartItem
    {
        public Shoe Shoe { get; set; }
        public int Quantity { get; set; }
    }
}
