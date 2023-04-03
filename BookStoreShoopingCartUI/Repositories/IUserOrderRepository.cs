namespace BookStoreShoppingCartUI.Repositories
{
    public interface IUserOrderRepository
    {
        Task<IEnumerable<Order>> GetUserOrders();
    }
}