using GIC.OrderService.Models.DAO;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace GIC.OrderService.Data.Context
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions options) : base(options) { }
        public DbSet<Order> Orders => Set<Order>();
    }
}
