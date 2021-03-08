using System;
using System.Collections.Generic;
using System.Text;

namespace MmtEcommerce.Api.ApiModels
{
    public class Customer
    {
        public string CustomerId { get; set; }
        public string Email { get; set; }
        public bool WebSite { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime LastLoggedIn { get; set; }
        public string HouseNumber { get; set; }
        public string Town { get; set; }
        public string PostCode { get; set; }
        public string PreferredLanguage { get; set; }
    }
}
