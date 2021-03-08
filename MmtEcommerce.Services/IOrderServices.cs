using MmtEcommerce.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MmtEcommerce.Services
{
    public interface IOrderServices
    {
        Task<List<Order>> GetOrdersByCustomer(string email);
    }
}
