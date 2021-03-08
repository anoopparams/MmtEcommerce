using System;
using System.Collections.Generic;

namespace MmtEcommerce.ViewModels
{
    public class OrderViewModel
    {
        public int OrderNumber { get; set; }
        public string OrderDate { get; set; }
        public string DeliveryAddress { get; set; }
        public List<OrderItemViewModel> OrderItems { get; set; }
        public string DeliveryExcepted { get; set; }
    }
}
