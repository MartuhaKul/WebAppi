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

        public IActionResult Cart()
        {
            var cart = GetCart();
            return View(cart);
        }

        
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

        [HttpGet]
        public IActionResult Checkout()
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");

            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Login", "Account"); 
            }

            var cart = GetCart(); // Отримуємо кошик
            var orderViewModel = new OrderViewModel
            {
                CartItems = cart ?? new List<CartItem>(), 
                Address = "", 
                PhoneNumber = "",
                Email = ""
            };

            return View(orderViewModel); 
        }
        
        [HttpPost]
        public async Task<IActionResult> Checkout(OrderViewModel model)
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
                
                var cart = GetCart(); 

 
                if (cart == null || cart.Count == 0)
                {
     
                    Console.WriteLine("Cart is empty, nothing to save");
                    return View(model); 
                }
                
                var order = new Order
                {
                    UserName = user.Name, 
                    Address = model.Address,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    OrderDate = DateTime.Now
                };
                
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
                
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

         
                ClearCart();
                
                return RedirectToAction("OrderConfirmation");
            }

            // Якщо форма не валідна, відображаємо помилки
            return View(model);
        }

        // Підтвердження замовлення (відображення після успішного оформлення)
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
                return new List<CartItem>(); // Якщо кошик порожній, повертаємо порожній список
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

    // Модель для форми оформлення замовлення
    public class OrderViewModel
    {
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}
