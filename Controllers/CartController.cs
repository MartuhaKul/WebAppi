using Microsoft.AspNetCore.Mvc;
using WebAppi.Models;

namespace WebAppi.Controllers
{
    public class CartController : Controller
    {
        private static List<Shoe> _cart = new List<Shoe>();
        
        public IActionResult AddToCart(int id)
        {
            var shoe = ShoeList().FirstOrDefault(s => s.Id == id);
            if (shoe != null)
            {
                _cart.Add(shoe);
            }
            return RedirectToAction("Cart");
        }
        
        public IActionResult Cart()
        {
            return View(_cart);
        }
        
        private List<Shoe> ShoeList()
        {
            return new List<Shoe>
            {
                new Shoe { Id = 1, Name = "Nike Running - Neon", Price = 210.00, ImageUrl = "/img/Frame1.svg", Description = "Great running shoes with neon design.", Rating = 4.8 },
                new Shoe { Id = 2, Name = "Nike - Zegama 2", Price = 220.00, ImageUrl = "/img/Frame2.svg", Description = "Lightweight shoes perfect for hiking.", Rating = 4.2 },
                new Shoe { Id = 3, Name = "Nike - Vaporfly 3", Price = 250.00, ImageUrl = "/img/Frame3.svg", Description = "Professional running shoes with advanced technology.", Rating = 4.9 },
                new Shoe { Id = 4, Name = "Nike Air Max 270 React", Price = 180.00, ImageUrl = "/img/Frame4.svg", Description = "Innovative shoes with cushioning for all-day comfort.", Rating = 4.7 },
                new Shoe { Id = 5, Name = "Nike Air Zoom Pegasus 38", Price = 130.00, ImageUrl = "/img/Frame5.svg", Description = "Running shoes designed for long distances with high comfort.", Rating = 4.6 },
                new Shoe { Id = 6, Name = "Nike Free RN 5.0", Price = 120.00, ImageUrl = "/img/Frame6.svg", Description = "Lightweight and flexible shoes for training and running.", Rating = 4.4 }
            };
        }
    }
}