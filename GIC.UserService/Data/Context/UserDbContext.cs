using GIC.UserService.Models.DAO;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace GIC.UserService.Data.Context
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions options) : base(options) { }
        public DbSet<User> Users => Set<User>();
    }
}
