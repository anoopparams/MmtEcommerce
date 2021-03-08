using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MmtEcommerce.Api;
using MmtEcommerce.Api.ApiModels;
using MmtEcommerce.Controllers;
using MmtEcommerce.Data.Interface;
using MmtEcommerce.Data.Models;
using MmtEcommerce.ViewModels;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace MmtEcommerce.UnitTest
{
    public class MmtEcommerceOrderControllerTest
    {
        private readonly Mock<ICustomerApi> mockCustomerApi;
        private readonly Mock<IOrderRepository> mockOrderRepository;
        private readonly Mock<ILogger<OrderController>> mockLogger;

        public MmtEcommerceOrderControllerTest()
        {
            mockCustomerApi = new Mock<ICustomerApi>(MockBehavior.Strict);
            mockOrderRepository = new Mock<IOrderRepository>();
            mockLogger = new Mock<ILogger<OrderController>>();
        }
        [Fact]
        public async void Should_Return_OK_Status_With_Order_Details_When_Customer_Email_Posted()
        {
            mockCustomerApi.Setup(m => m.GetCustomerByEmailAsync(It.IsAny<string>())).Returns(async () => 
            {
                await Task.Yield();
                return GetCustomerAptResult(); 
            });
            mockOrderRepository.Setup(m => m.GetOrdersByCustomer(It.IsAny<string>())).Returns(async () =>
            {
                await Task.Yield();
                return GetCustomerOrderResult();
            });

            var controller = new OrderController(mockOrderRepository.Object, mockCustomerApi.Object, mockLogger.Object);
            var task = await controller.Post(new UserDto { CustomerId = "C344", User = "test@test.com" });
            var result = task as ObjectResult;

            Assert.NotNull(result);
            Assert.Equal((int) HttpStatusCode.OK, result.StatusCode);

            var value = result.Value as List<CustomerOrderViewModel>;

            Assert.Equal("Test", value[0].Customer.FirstName);
            Assert.Equal(24, value[0].Order.OrderNumber);
        }

        [Fact]
        public async void Should_Return_BadRequest_Status_When_Customer_Id_And_Email_Not_Match()
        {
            var customer = GetCustomerAptResult();
            customer.CustomerId = "56678";
            mockCustomerApi.Setup(m => m.GetCustomerByEmailAsync(It.IsAny<string>())).Returns(async () =>
            {
                await Task.Yield();
                return customer;
            });

            var controller = new OrderController(mockOrderRepository.Object, mockCustomerApi.Object, mockLogger.Object);
            var task = await controller.Post(new UserDto { CustomerId = "C344", User = "test@test.com" });
            var result = task as ObjectResult;

            Assert.NotNull(result);
            Assert.Equal((int) HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("Customer Id is not matching with email.", result.Value);
        }

        [Fact]
        public async void Should_Return_Customer_Details_When_Order_Not_Found()
        {
            mockCustomerApi.Setup(m => m.GetCustomerByEmailAsync(It.IsAny<string>())).Returns(async () =>
            {
                await Task.Yield();
                return GetCustomerAptResult();
            });

            var controller = new OrderController(mockOrderRepository.Object, mockCustomerApi.Object, mockLogger.Object);
            var task = await controller.Post(new UserDto { CustomerId = "C344", User = "test@test.com" });
            var result = task as ObjectResult;

            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            var value = result.Value as List<CustomerOrderViewModel>;
            Assert.Equal("Test", value[0].Customer.FirstName);
        }


        [Fact]
        public async void Should_Return_BadRequest_Status_When_Customer_Not_Found()
        {
            mockCustomerApi.Setup(m => m.GetCustomerByEmailAsync(It.IsAny<string>())).Returns(async () =>
            {
                await Task.Yield();
                return null;
            });

            var controller = new OrderController(mockOrderRepository.Object, mockCustomerApi.Object, mockLogger.Object);
            var task = await controller.Post(new UserDto { CustomerId = "C344", User = "test@test.com" });
            var result = task as ObjectResult;

            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("Customer doesn't exist!", result.Value);
        }

        [Fact]
        public async void Should_Return_BadRequest_Status_When_Error_In_ModelState()
        {
            mockCustomerApi.Setup(m => m.GetCustomerByEmailAsync(It.IsAny<string>())).Returns(async () =>
            {
                await Task.Yield();
                return null;
            });

            var controller = new OrderController(mockOrderRepository.Object, mockCustomerApi.Object, mockLogger.Object);
            controller.ModelState.AddModelError("User", "User Id is missing");
            var task = await controller.Post(new UserDto { CustomerId = "C344", User = "" });
            var result = task as ObjectResult;

            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Fact]
        public async void Should_Sort_Order_By_Latest_Order_Date()
        {
            mockCustomerApi.Setup(m => m.GetCustomerByEmailAsync(It.IsAny<string>())).Returns(async () =>
            {
                await Task.Yield();
                return GetCustomerAptResult();
            });
            mockOrderRepository.Setup(m => m.GetOrdersByCustomer(It.IsAny<string>())).Returns(async () =>
            {
                await Task.Yield();
                return GetCustomerOrderResult();
            });

            var controller = new OrderController(mockOrderRepository.Object, mockCustomerApi.Object, mockLogger.Object);
            var task = await controller.Post(new UserDto { CustomerId = "C344", User = "test@test.com" });
            var result = task as ObjectResult;

            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);

            var value = result.Value as List<CustomerOrderViewModel>;
            Assert.Equal("11-Jan-2021", value[0].Order.OrderDate);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async void Should_Return_Order_As_Gift_If_ContainsGift_Is_True(bool containsGift)
        {
            mockCustomerApi.Setup(m => m.GetCustomerByEmailAsync(It.IsAny<string>())).Returns(async () =>
            {
                await Task.Yield();
                return GetCustomerAptResult();
            });

            var orders = GetCustomerOrderResult();
            orders[1].ContainsGift = containsGift;

            mockOrderRepository.Setup(m => m.GetOrdersByCustomer(It.IsAny<string>())).Returns(async () =>
            {
                await Task.Yield();
                return orders;
            });

            var controller = new OrderController(mockOrderRepository.Object, mockCustomerApi.Object, mockLogger.Object);
            var task = await controller.Post(new UserDto { CustomerId = "C344", User = "test@test.com" });
            var result = task as ObjectResult;

            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);

            var value = result.Value as List<CustomerOrderViewModel>;
            if(containsGift)
            {
                Assert.Equal("Gift", value[0].Order.OrderItems[0].Product);
            }
            else
            {
                Assert.Equal("Toys", value[0].Order.OrderItems[0].Product);
            }
        }

        private List<Order> GetCustomerOrderResult()
        {
            return new List<Order>
                {
                    new Order
                    {
                        CustomerId = "C344",
                        ContainsGift = true,
                        DeliveryExpected = new DateTime(2012, 12, 11),
                        OrderDate = new DateTime(2012, 12, 11),
                        OrderId = 23,
                        OrderItems = new List<OrderItems>
                        {
                            new OrderItems
                            {
                                OrderItemId = 2,
                                Price = 23,
                                Product = new Product
                                {
                                    Colour = "red",
                                    PackHeight = 12,
                                    PackWeight = 22,
                                    ProductId = 33,
                                    ProductName = "Toys"

                                },
                                Quantity = 23,
                                Returnable = true
                            }
                        }
                    },
                    new Order
                    {
                        CustomerId = "C344",
                        ContainsGift = false,
                        DeliveryExpected = new DateTime(2021, 12, 11),
                        OrderDate = new DateTime(2021, 1, 11),
                        OrderId = 24,
                        OrderItems = new List<OrderItems>
                        {
                            new OrderItems
                            {
                                OrderItemId = 2,
                                Price = 23,
                                Product = new Product
                                {
                                    Colour = "red",
                                    PackHeight = 12,
                                    PackWeight = 22,
                                    ProductId = 33,
                                    ProductName = "Toys"

                                },
                                Quantity = 23,
                                Returnable = true
                            }
                        }
                    }
                };
        }

        private Customer GetCustomerAptResult()
        {
            return new Customer
            { 
                CustomerId = "C344",
                Email = "test@test.com",
                FirstName = "Test",
                LastName = "User"
            };
        }
    }
}
