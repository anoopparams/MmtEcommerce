using Microsoft.EntityFrameworkCore;
using MmtEcommerce.Data.Interface;
using MmtEcommerce.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MmtEcommerce.Data
{
    public class OrderRepository : IOrderRepository
    {
        private readonly MmtEcommerceDbContext _mmtEcommerceDbContext;
        public OrderRepository(MmtEcommerceDbContext mmtEcommerceDbContext)
        {
            _mmtEcommerceDbContext = mmtEcommerceDbContext;
        }

        public Task<Order> Add(Order entity)
        {
            throw new NotImplementedException();
        }

        public Task<Order> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Order> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Order>> GetAll()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Gets all order by customer id
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns>List of orders</returns>
        public async Task<IEnumerable<Order>> GetOrdersByCustomer(string customerId)
        {
            return await _mmtEcommerceDbContext.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.Product).ToListAsync();
        }

        public Task<Order> Update(Order entity)
        {
            throw new NotImplementedException();
        }
    }
}
