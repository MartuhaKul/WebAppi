using Microsoft.AspNetCore.Mvc;
using WebAppi.Models;

namespace WebAppi.Controllers
{
    public class HomeController : Controller
    {
        private List<Shoe> _shoes = new List<Shoe>
        {
            new Shoe { Id = 1, Name = "Nike Running - Neon", Price = 210.00, ImageUrl = "/img/Frame1.svg", Description = "Great running shoes with neon design.", Rating = 4.8 },
            new Shoe { Id = 2, Name = "Nike - Zegama 2", Price = 220.00, ImageUrl = "/img/Frame2.svg", Description = "Lightweight shoes perfect for hiking.", Rating = 4.2 },
            new Shoe { Id = 3, Name = "Nike - Vaporfly 3", Price = 250.00, ImageUrl = "/img/Frame3.svg", Description = "Professional running shoes with advanced technology.", Rating = 4.9 }
        };

        public IActionResult Index()
        {
            // Додаємо властивість IsHighlyRated на основі рейтингу
            foreach (var shoe in _shoes)
            {
                shoe.IsHighlyRated = shoe.Rating >= 4.5;
            }

            return View(_shoes);
        }

        public IActionResult ShoeDetails(int id)
        {
            var shoe = _shoes.FirstOrDefault(s => s.Id == id);
            if (shoe == null)
            {
                return NotFound();
            }
            return View(shoe);
        }
    }
}