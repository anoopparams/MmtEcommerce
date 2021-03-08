using MmtEcommerce.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MmtEcommerce.Data.Interface
{
    public interface IOrderRepository : IRepository<Order>
    {
        public Task<IEnumerable<Order>> GetOrdersByCustomer(string customerId);
    }
}
