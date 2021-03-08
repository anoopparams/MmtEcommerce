using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MmtEcommerce.Data.Models
{
    public class OrderItems
    {
        [Key]
        public int OrderItemId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public bool Returnable { get; set; }
        public Product Product { get; set; }
    }
}
