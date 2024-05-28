using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using static System.Reflection.Metadata.BlobBuilder;

namespace BookInventory.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<Book> Books { get; set; }

        public DbSet<Borrowing> Borrowings { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Book>(eb =>
            {
                eb.ToTable("Book");
                eb.HasKey(c => c.Id);
                eb.Property(c => c.Id).ValueGeneratedOnAdd();

                eb.Property(c => c.Name).HasColumnType("nvarchar(500)").IsRequired();
                eb.Property(c => c.Author).HasColumnType("nvarchar(500)").IsRequired();
            });


            modelBuilder.Entity<Borrowing>(eb =>
            {
                eb.ToTable("Borrowing");
                eb.HasKey(c => c.Id);
                eb.Property(c => c.Id).ValueGeneratedOnAdd();

                eb.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId);
                eb.HasOne(x => x.Book).WithMany().HasForeignKey(x => x.BookId);
            });
        }
    }
}
