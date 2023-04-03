using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreShoppingCartUI.Controllers
{
    [Authorize] //just the logged user can use this controller
    public class CartController : Controller
    {
        private readonly ICartRepository cartRepository;

        public CartController(ICartRepository _cartRepository)
        {
            this.cartRepository = _cartRepository;
        }

        public async Task<IActionResult> GetUserCart()
        {
            var cart = await cartRepository.GetUserCart();
            return View(cart);
        }

        public async Task<IActionResult> AddItem(int bookId, int quantity=1, int redirect=0)
        {
            var cartCount = await cartRepository.AddItemToCart(bookId, quantity);
            if (redirect == 0)
                return Ok(cartCount);

            return RedirectToAction("GetUserCart");
        }

        public async Task<IActionResult> RemoveItem(int bookId)
        {
            var cartCount = await cartRepository.RemoveItemFromCart(bookId);
            return RedirectToAction("GetUserCart");
        }

        public async Task<IActionResult> GetTotalItemInCart()
        {
            int cartItem = await cartRepository.GetItemCountInCart();
            return View(cartItem);
        }

        public async Task<IActionResult> Checkout()
        {
            bool isChecketOut =  await cartRepository.DoCheckout();

            if (!isChecketOut)
                throw new Exception("Something wrong in server side");

            return RedirectToAction("Index", "Home");
        }
    }
}
