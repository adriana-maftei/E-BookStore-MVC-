using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BookStoreShoppingCartUI.Repositories
{
    public interface ICartRepository
    {
        Task<ShoppingCart> GetCart(string userID);
        Task<ShoppingCart> GetUserCart();
        Task<int> AddItemToCart(int bookId, int quantity);
        Task<int> RemoveItemFromCart(int bookId);
        Task<int> GetItemCountInCart(string userID = "");
        Task<bool> DoCheckout();
    }
}
