using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MmtEcommerce.ViewModels
{
    public class UserDto
    {
        [Required]
        public string User { get; set; }
        [Required]
        public string CustomerId { get; set; }
    }
}
