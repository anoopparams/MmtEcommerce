using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MmtEcommerce.Api;
using MmtEcommerce.Api.ApiModels;
using MmtEcommerce.Data.Interface;
using MmtEcommerce.Data.Models;
using MmtEcommerce.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MmtEcommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerApi _customerApi;
        private readonly ILogger _logger;
        public OrderController(IOrderRepository orderRepository, ICustomerApi customerApi, ILogger<OrderController> logger)
        {
            _orderRepository = orderRepository;
            _customerApi = customerApi;
            _logger = logger;
        }
        // POST api/<OrderController>
        /// <summary>
        /// Posts user details to get order.
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(UserDto userDto)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var customer = await _customerApi.GetCustomerByEmailAsync(userDto.User);
                if (customer == null)
                {
                    return BadRequest("Customer doesn't exist!");
                }

                if (customer.CustomerId != userDto.CustomerId)
                {
                    return BadRequest("Customer Id is not matching with email.");
                }

                //Get all the orders of the customer
                var orders = await _orderRepository.GetOrdersByCustomer(customer.CustomerId);

                //if orders is not available return the user details
                var customerOrders = orders.Any()
                    ? CreateCustomerOrderViewModel(customer, orders)
                    : CreateCustomerViewModel(customer);

                return Ok(customerOrders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return BadRequest();
        }

        /// <summary>
        /// Creates the view model for customer and order
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="orders"></param>
        /// <returns></returns>
        private static List<CustomerOrderViewModel> CreateCustomerOrderViewModel(Customer customer, IEnumerable<Order> orders)
        {
            var customerOrders = new List<CustomerOrderViewModel>();
            
            //create the view model order by order date
            foreach (var order in orders.OrderByDescending(o => o.OrderDate))
            {
                customerOrders.Add(new CustomerOrderViewModel
                {
                    Customer = new CustomerViewModel
                    {
                        FirstName = customer.FirstName,
                        LastName = customer.LastName
                    },
                    Order = new OrderViewModel
                    {
                        OrderNumber = order.OrderId,
                        OrderDate = order.OrderDate.ToString("dd-MMM-yyyy"),
                        DeliveryAddress = "Address not available", //TODO: delivery address is not in collection
                        DeliveryExcepted = order.DeliveryExpected.ToString("dd-MMM-yyyy"),

                        OrderItems = order.OrderItems.Select(o => new OrderItemViewModel
                        {
                            PriceEach = o.Price,
                            Product = order.ContainsGift ? "Gift" : o.Product.ProductName, //To avoid spoiling the surprise of presents, orders which are marked as contains a gift should return Gift in place of the product name.
                            Quantity = o.Quantity
                        }).ToList()
                    }
                });
            }

            return customerOrders;
        }
        /// <summary>
        /// Creates the customer view model
        /// </summary>
        /// <param name="customer"></param>
        /// 
        /// <returns></returns>
        private static List<CustomerOrderViewModel> CreateCustomerViewModel(Customer customer)
        {
            return new List<CustomerOrderViewModel>
            {
                new CustomerOrderViewModel
                {
                    Customer = new CustomerViewModel
                    {
                        FirstName = customer.FirstName,
                        LastName = customer.LastName
                    }
                }
            };
        }
    }
}
