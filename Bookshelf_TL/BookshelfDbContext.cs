using Microsoft.EntityFrameworkCore;
using Bookshelf_TL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Bookshelf_TL.Models.IntermediateModels;

namespace Bookshelf_TL
{
    public class BookshelfDbContext : IdentityDbContext<User>
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> ApplicationUsers { get; set; }
        public DbSet<BookAuthor> BookAuthors { get; set; }
        public DbSet<BookCategory> BookCategories { get; set; }
        public DbSet<BookUser> BookUsers { get; set; }

        public BookshelfDbContext() { }
        public BookshelfDbContext(DbContextOptions<BookshelfDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BookAuthor>()
                .HasKey(ba => new { ba.BookId, ba.AuthorId });

            modelBuilder.Entity<BookCategory>()
                .HasKey(bc => new { bc.BookId, bc.CategoryId });

            modelBuilder.Entity<BookUser>(entity =>
            {
                entity.HasKey(ba => new { ba.BookId, ba.UserId });

                entity.Property(p => p.Status)
                .IsRequired(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(u => u.FirstName)
                    .HasMaxLength(500)
                    .IsRequired(false);

                entity.Property(u => u.MiddleName)
                    .HasMaxLength(500)
                    .IsRequired(false);

                entity.Property(u => u.LastName)
                    .HasMaxLength(500)
                    .IsRequired(false);

                entity.Property(u => u.Description)
                    .HasMaxLength(100000)
                    .IsRequired(false);

                entity.Property(u => u.CoverPath)
                    .HasMaxLength(1000)
                    .IsRequired(false);

                entity.Property(u => u.BirthDate)
                    .IsRequired(false)
                    .HasColumnType("date");

            });

            modelBuilder.Entity<Author>(entity =>
            {
                entity.Property(u => u.FirstName)
                    .HasMaxLength(500)
                    .IsRequired(true);

                entity.Property(u => u.MiddleName)
                    .HasMaxLength(500)
                    .IsRequired(false);

                entity.Property(u => u.LastName)
                    .HasMaxLength(500)
                    .IsRequired(true);

                entity.Property(u => u.Country)
                    .HasMaxLength(100)
                    .IsRequired(false);

                entity.Property(u => u.CoverPath)
                    .HasMaxLength(1000)
                    .IsRequired(false);

                entity.Property(u => u.Description)
                    .HasMaxLength(50000)
                    .IsRequired(false);
            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity.Property(u => u.BookName)
                    .HasMaxLength(500)
                    .IsRequired(true);

                entity.Property(u => u.Series)
                    .HasMaxLength(500)
                    .IsRequired(false);

                entity.Property(u => u.CoverPath)
                    .HasMaxLength(1000)
                    .IsRequired(false);

                entity.Property(u => u.Description)
                    .HasMaxLength(50000)
                    .IsRequired(false);

                entity.Property(u => u.DateOfPublication)
                    .IsRequired(false)
                    .HasColumnType("date");

                entity.Property(b => b.AverageBookScore)
                    .HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(u => u.Name)
                    .HasMaxLength(500)
                    .IsRequired(true);
            });

        }

        public void ApplyMigrations(BookshelfDbContext context)
        {
            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
        }
    }
}