using Microsoft.Extensions.Logging;
using MmtEcommerce.Api.ApiModels;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MmtEcommerce.Api
{
    public class CustomerApi : ICustomerApi
    {
        private readonly string _apiUrl;
        private readonly string _apiKey;

        public CustomerApi(string apiUrl, string apiKey)
        {
            _apiUrl = apiUrl;
            _apiKey = apiKey;
        }
        /// <summary>
        /// Gets the customer details by email address
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<Customer> GetCustomerByEmailAsync(string email)
        {
            var url = $"{_apiUrl}?code={_apiKey}&email={email}";
            var httpClient = new HttpClient();

            var response = await httpClient.GetAsync(url);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception($"Customer Api error returned with status code {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();
            Customer customer = JsonConvert.DeserializeObject<Customer>(content);

            return customer;
        }
    }
}
