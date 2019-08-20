using System.Collections.Generic;
using ui.Models;

namespace ui.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        IEnumerable<Order> GetCustomerOrders(int customerId);
    }
}