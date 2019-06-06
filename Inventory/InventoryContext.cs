using System;
using System.Collections.Generic;
using System.Text;
using Inventory.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory
{
    public class InventoryContext : DbContext
    {
        public InventoryContext(DbContextOptions<InventoryContext> options) : base(options)
        {
        }

        public DbSet<Footprint> Footprints { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Models.Inventory> Inventories { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Footprint>().HasIndex(e => e.FootprintName).IsUnique(true);

            builder.Entity<Location>().HasIndex(e => e.LocationName).IsUnique(false);
            builder.Entity<Location>().HasIndex(e => new { e.LocationName, e.LocationUnit }).IsUnique(true);

            builder.Entity<Product>().HasOne(e => e.Footprint).WithMany(e => e.Products)
                .HasForeignKey(e => e.FootprintId);
            builder.Entity<Product>().HasIndex(e => new { e.ProductName, e.FootprintId }).IsUnique(true);
            builder.Entity<Product>().HasIndex(e => e.Manufacturer).IsUnique(false);

            builder.Entity<Models.Inventory>().HasOne(e => e.Location).WithMany(e => e.Inventories)
                .HasForeignKey(e => e.LocationId);
            builder.Entity<Models.Inventory>().HasOne(e => e.Product).WithMany(e => e.Inventories)
                .HasForeignKey(e => e.ProductId);
            builder.Entity<Models.Inventory>().HasIndex(e => new {e.ProductId, e.LocationId}).IsUnique(true);

            builder.Entity<Supplier>().HasIndex(e => e.Name).IsUnique(true);
        }
    }
}
