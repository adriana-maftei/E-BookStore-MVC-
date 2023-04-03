using BookStoreShoopingCartUI.Models;
using BookStoreShoppingCartUI.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BookStoreShoopingCartUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeRepository _homeRepository;

        public HomeController(ILogger<HomeController> logger, IHomeRepository homeRepository)
        {
            _logger = logger;
            _homeRepository = homeRepository;
        }

        public async Task<IActionResult> Index(string searchTerm="", int genreID = 0)
        {
            IEnumerable<Book> books = await _homeRepository.GetBooks(searchTerm, genreID);
            IEnumerable<Genre> genres = await _homeRepository.GetGenres();

            BookDisplayModel bookModel = new BookDisplayModel
            {
                Books = books,
                Genres = genres,
                searchTerm = searchTerm,
                genreID = genreID
            };
            return View(bookModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}