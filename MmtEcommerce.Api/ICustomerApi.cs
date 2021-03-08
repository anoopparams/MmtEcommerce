using MmtEcommerce.Api.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MmtEcommerce.Api
{
    public interface ICustomerApi
    {
        Task<Customer> GetCustomerByEmailAsync(string email);
    }
}
