using Microsoft.AspNetCore.Mvc;
using WebAppi.Data;


namespace WebAppi.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var shoes = _context.Shoes.ToList();

            // Set IsHighlyRated property based on Rating
            foreach (var shoe in shoes)
            {
                shoe.IsHighlyRated = shoe.Rating >= 4.5;
            }

            // Random selection of shoes for best sellers
            var randomShoes = shoes.OrderBy(s => Guid.NewGuid()).Take(3).ToList();
            ViewData["BestSellers"] = randomShoes;

            return View(randomShoes);
        }

        public IActionResult ShoeDetails(int id)
        {
            var selectedShoe = _context.Shoes.FirstOrDefault(s => s.Id == id);
            if (selectedShoe == null)
            {
                return NotFound();
            }

            // Set IsHighlyRated property based on Rating
            selectedShoe.IsHighlyRated = selectedShoe.Rating >= 4.5;

            // Random selection of shoes for best sellers
            var shoes = _context.Shoes.ToList();
            var randomShoes = shoes.OrderBy(s => Guid.NewGuid()).Take(3).ToList();
            ViewData["BestSellers"] = randomShoes;

            return View(selectedShoe);
        }
    }
}