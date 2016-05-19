using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using WebApplication3.Models.Entities;

namespace WebApplication3.Models.EF
{
    public class EfDbContext : DbContext
    {
        public EfDbContext() : base("EfDbContext")
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<News> Newsfeed { get; set; }
        public DbSet<Cart> Carts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}