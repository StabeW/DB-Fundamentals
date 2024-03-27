using BookShop.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Data
{
    public class BookShopContext : DbContext
    {
        public DbSet<Author> Authors { get; set; }

        public DbSet<Book> Books { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<BookCategory> BookCategories { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.SqlConnection);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>(entity =>
            {
                entity.HasKey(a => a.AuthorId);

                entity
                    .Property(a => a.FirstName)
                    .HasMaxLength(50)
                    .IsRequired(false)
                    .IsUnicode();

                entity
                    .Property(a => a.LastName)
                    .HasMaxLength(50)
                    .IsRequired(false)
                    .IsUnicode();
            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(b => b.BookId);

                entity
                    .Property(b => b.Title)
                    .HasMaxLength(50)
                    .IsUnicode();

                entity
                    .Property(b => b.Description)
                    .HasMaxLength(1000)
                    .IsUnicode();

                entity
                    .Property(b => b.ReleaseDate)
                    .IsRequired(false);

                entity
                    .HasOne(a => a.Author)
                    .WithMany(b => b.Books)
                    .HasForeignKey(b => b.AuthorId);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(c => c.CategoryId);

                entity
                    .Property(c => c.Name)
                    .HasMaxLength(50)
                    .IsUnicode();
            });

            modelBuilder.Entity<BookCategory>(entity =>
            {
                entity.HasKey(bc => new { bc.BookId, bc.CategoryId });

                entity
                    .HasOne(bc => bc.Book)
                    .WithMany(b => b.BookCategories)
                    .HasForeignKey(bc => bc.BookId);

                entity
                    .HasOne(bc => bc.Category)
                    .WithMany(c => c.CategoryBooks)
                    .HasForeignKey(bc => bc.CategoryId);
            });
        }
    }
}
