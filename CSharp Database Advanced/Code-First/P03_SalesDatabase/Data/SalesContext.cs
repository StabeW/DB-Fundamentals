using Microsoft.EntityFrameworkCore;
using P03_SalesDatabase.Data.Models;
using System.Reflection.Emit;

namespace P03_SalesDatabase.Data
{
    public class SalesContext : DbContext
    {
        DbSet<Product> Products { get; set; }
        DbSet<Customer> Customers { get; set; }
        DbSet<Store> Stores { get; set; }
        DbSet<Sale> Sales { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Configuration.SqlConnection);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigurationProductEntity(modelBuilder);
            ConfigurationCustomerEntity(modelBuilder);
            ConfigurationStoreEntity(modelBuilder);
            ConfigurationSaleEntity(modelBuilder);
        }

        private void ConfigurationSaleEntity(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Sale>()
                .HasKey(s => s.SaleId);
        }

        private void ConfigurationStoreEntity(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Store>()
                .HasKey(s => s.StoreId);

            modelBuilder
                .Entity<Store>()
                .Property(n => n.Name)
                .HasMaxLength(80)
                .IsUnicode();

            modelBuilder
                .Entity<Store>()
                .HasMany(s => s.Sales)
                .WithOne(st => st.Store);
        }

        private void ConfigurationCustomerEntity(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Customer>()
                .HasKey(c => c.CustomerId);

            modelBuilder
                .Entity<Customer>()
                .Property(n => n.Name)
                .HasMaxLength(100)
                .IsUnicode();

            modelBuilder
                .Entity<Customer>()
                .Property(e => e.Email)
                .HasMaxLength(80)
                .IsUnicode(false);

            modelBuilder
                .Entity<Customer>()
                .HasMany(s => s.Sales)
                .WithOne(c => c.Customer);
        }

        private void ConfigurationProductEntity(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Product>()
                .HasKey(p => p.ProductId);

            modelBuilder
                .Entity<Product>()
                .Property(n => n.Name)
                .HasMaxLength(50)
                .IsUnicode();

            modelBuilder
                .Entity<Product>()
                .HasMany(s => s.Sales)
                .WithOne(p => p.Product);

            modelBuilder
                .Entity<Product>()
                .Property(d => d.Description)
                .HasMaxLength(250)
                .IsUnicode(false);
        }
    }
}
