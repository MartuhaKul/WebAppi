using Microsoft.AspNetCore.Mvc;
using WebAppi.Models;

namespace WebAppi.Controllers
{
    public class HomeController : Controller
    {
        private List<Shoe> _shoes = new List<Shoe>
        {
            new Shoe
            {
                Id = 1, Name = "Nike Running - Neon", Price = 210.00, ImageUrl = "/img/Frame1.svg",
                Description = "Great running shoes with neon design.", Rating = 4.8
            },
            new Shoe
            {
                Id = 2, Name = "Nike - Zegama 2", Price = 220.00, ImageUrl = "/img/Frame2.svg",
                Description = "Lightweight shoes perfect for hiking.", Rating = 4.2
            },
            new Shoe
            {
                Id = 3, Name = "Nike - Vaporfly 3", Price = 250.00, ImageUrl = "/img/Frame3.svg",
                Description = "Professional running shoes with advanced technology.", Rating = 4.9
            },
            new Shoe
            {
                Id = 4, Name = "Nike Air Max 270 React", Price = 180.00, ImageUrl = "/img/Frame4.svg",
                Description = "Innovative shoes with cushioning for all-day comfort.", Rating = 4.7
            },
            new Shoe
            {
                Id = 5, Name = "Nike Air Zoom Pegasus 38", Price = 130.00, ImageUrl = "/img/Frame5.svg",
                Description = "Running shoes designed for long distances with high comfort.", Rating = 4.6
            },
            new Shoe
            {
                Id = 6, Name = "Nike Free RN 5.0", Price = 120.00, ImageUrl = "/img/Frame6.svg",
                Description = "Lightweight and flexible shoes for training and running.", Rating = 4.4
            }
        };

        // Головна сторінка
        public IActionResult Index()
        {
            // Встановлюємо, чи є товар highly rated
            foreach (var shoe in _shoes)
            {
                shoe.IsHighlyRated = shoe.Rating >= 4.5;
            }

            // Випадкові товари для відображення на головній сторінці
            var randomShoes = _shoes.OrderBy(s => Guid.NewGuid()).Take(3).ToList();

            // Визначаємо товари з високими рейтингами
            var highlyRatedShoes = _shoes.Where(s => s.IsHighlyRated).ToList();
            ViewData["HighlyRated"] = highlyRatedShoes;  // Передаємо highly rated товари для відображення на сторінці

            return View(randomShoes);
        }

        // Сторінка деталей товару
        public IActionResult ShoeDetails(int id)
        {
            // Шукаємо товар за ID
            var shoe = _shoes.FirstOrDefault(s => s.Id == id);
            if (shoe == null)
            {
                return NotFound();
            }

            // Отримуємо highly rated товари з ViewData
            var highlyRatedShoes = ViewData["HighlyRated"] as List<Shoe>;

            if (highlyRatedShoes == null)
            {
                // Якщо немає highly rated товарів в ViewData, визначаємо їх
                highlyRatedShoes = _shoes.Where(s => s.IsHighlyRated).ToList();
            }

            // Передаємо highly rated товари для відображення на сторінці деталей
            ViewData["HighlyRated"] = highlyRatedShoes;

            return View(shoe);
        }
    }
}
