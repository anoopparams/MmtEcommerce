using MmtEcommerce.Api;
using MmtEcommerce.Data.Interface;
using MmtEcommerce.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MmtEcommerce.Services
{
    public class OrderServices : IOrderServices
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerApi _customerApi;

        public OrderServices(IOrderRepository orderRepository, ICustomerApi customerApi)
        {
            _orderRepository = orderRepository;
            _customerApi = customerApi;
        }

        public async Task<List<Order>> GetOrdersByCustomer(string email)
        {
            var customer = await _customerApi.GetCustomerByEmail("cat.owner@mmtdigital.co.uk");

            if(customer == null)
            {
                throw new Exception("Customer doesn't exist!");
            }

            return await _orderRepository.GetOrdersByCustomer(customer.CustomerId);
        }
    }
}
