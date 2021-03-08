using Microsoft.EntityFrameworkCore;
using MmtEcommerce.Data.Models;
using System;

namespace MmtEcommerce.Data
{
    public class MmtEcommerceDbContext : DbContext
    {
        public MmtEcommerceDbContext(DbContextOptions<MmtEcommerceDbContext> options) : base(options)
        {
             
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}
