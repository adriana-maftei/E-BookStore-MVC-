using Microsoft.Identity.Client;
using System.Security.Claims;

namespace BookStoreShoppingCartUI.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<IdentityUser> userMng;
        private readonly IHttpContextAccessor contextAccessor;

        public CartRepository(ApplicationDbContext _dbContext, UserManager<IdentityUser> _userMng, IHttpContextAccessor _contextAccessor)
        {
            this.dbContext = _dbContext;
            this.userMng = _userMng;
            this.contextAccessor = _contextAccessor;
        }

        public async Task<ShoppingCart> GetCart(string userID)
        {
            var cart = await dbContext.ShoppingCarts.FirstOrDefaultAsync(x => x.userID == userID);
            return cart;
        }

        private string GetUserID()
        {
            ClaimsPrincipal user = contextAccessor.HttpContext.User;
            string userID = userMng.GetUserId(user);
            return userID;
        }

        public async Task<ShoppingCart> GetUserCart()
        {
            string userID = GetUserID();
            if (userID == null)
                throw new Exception("Invalid user");

            var cart = await dbContext.ShoppingCarts.Include(a => a.CartDetails)
                                                    .ThenInclude(a => a.Book)
                                                    .ThenInclude(a => a.Genre)
                                                    .Where(a => a.userID == userID).FirstOrDefaultAsync();
            return cart;
        }

        public async Task<int> AddItemToCart(int bookId, int quantity)
        {
            string userID = GetUserID();
            using var transaction = dbContext.Database.BeginTransaction();

            try
            {
                //create a cart
                if (string.IsNullOrEmpty(userID))
                    throw new Exception("User not logged in");

                var cart = await GetCart(userID);
                if (cart is null)
                {
                    cart = new ShoppingCart
                    {
                        userID = userID
                    };
                    dbContext.ShoppingCarts.Add(cart);
                }
                dbContext.SaveChanges();

                //add cart details
                var cartItem = dbContext.CartDetails.FirstOrDefault(a => a.shoppingCartID == cart.Id && a.bookID == bookId);
                if (cartItem is not null)
                    cartItem.Quantity += quantity;
                else
                {
                    var book = dbContext.Books.Find(bookId);
                    cartItem = new CartDetail
                    {
                        bookID = bookId,
                        shoppingCartID = cart.Id,
                        Quantity = quantity,
                        UnitPrice = book.Price
                    };
                    dbContext.CartDetails.Add(cartItem);
                }
                dbContext.SaveChanges();
                transaction.Commit();
            }
            catch (Exception ex)
            {

            }
            var cartItemCount = await GetItemCountInCart(userID);
            return cartItemCount;
        }

        public async Task<int> RemoveItemFromCart(int bookId)
        {
            string userID = GetUserID();

            try
            {
                if (string.IsNullOrEmpty(userID))
                    throw new Exception("User not logged in");

                var cart = await GetCart(userID);
                if (cart is null)
                    throw new Exception("Invalid cart");

                //change cart details
                var cartItem = dbContext.CartDetails.FirstOrDefault(a => a.shoppingCartID == cart.Id && a.bookID == bookId);
                if (cartItem is null)
                    throw new Exception("Cart is empty");
                else if (cartItem.Quantity == 1)
                    dbContext.CartDetails.Remove(cartItem);
                else
                    cartItem.Quantity = cartItem.Quantity - 1;

                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {

            }
            var cartItemCount = await GetItemCountInCart(userID);
            return cartItemCount;
        }

        public async Task<int> GetItemCountInCart(string userID = "")
        {
            if (string.IsNullOrEmpty(userID))
                userID = GetUserID();

            var data = await (from cart in dbContext.ShoppingCarts
                              join CartDetail in dbContext.CartDetails
                              on cart.Id equals CartDetail.shoppingCartID
                              select new { CartDetail.Id }).ToListAsync();
            return data.Count;
        }

        public async Task<bool> DoCheckout()
        {
            var transaction = dbContext.Database.BeginTransaction();
            try
            {
                var userID = GetUserID();
                if (string.IsNullOrEmpty(userID))
                    throw new Exception("User not logged in");

                var cart = await GetCart(userID);
                if (cart is null)
                    throw new Exception("Invalid cart");

                //insert data in orderDetail from cartDetail

                var cartDetail = dbContext.CartDetails
                    .Where(a => a.shoppingCartID == cart.Id).ToList();
                if (cartDetail.Count == 0)
                    throw new Exception("Cart is empty");

                var order = new Order
                {
                    userID = userID,
                    createDate = DateTime.Now,
                    orderStatusID = 1 //pending order by default
                };

                dbContext.Orders.Add(order);
                dbContext.SaveChanges();

                foreach(var item in cartDetail)
                {
                    var orderDetail = new OrderDetail
                    {
                        bookID = item.bookID,
                        orderID = order.Id,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    };
                    dbContext.orderDetails.Add(orderDetail);
                }
                dbContext.SaveChanges();

                //remove data from cartDetail
                dbContext.CartDetails.RemoveRange(cartDetail);
                dbContext.SaveChanges();
                transaction.Commit();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
