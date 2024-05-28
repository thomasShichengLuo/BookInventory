using BookInventory.Common;
using BookInventory.Data;
using BookInventory.Dtos;
using BookInventory.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace TestProject1
{
    public class UnitTest1
    {
        [Fact]
        public async Task GetAllBooks()
        {
            var connectionString =
                @"Server=LAPTOP-DKPU3OT5\\THOMASSQLSERVER;Database=aspnet-BookInventory-c7023200-510b-4542-873b-3fce175208e5;User ID=sa;Password=Thomas@00144277;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true";

            var dbContextOptionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlServer(connectionString);
            var appDbContext = new ApplicationDbContext(dbContextOptionsBuilder.Options);

            var logger = LoggerFactory.Create((builder => { })).CreateLogger<BookService>();

            var bookService = new BookService(appDbContext, logger);


            var response = await bookService.GetAllBooks();
            Assert.True(response != null);
        }

        [Fact]
        public async Task GetAllBorrowings()
        {
            var connectionString =
                @"Server=LAPTOP-DKPU3OT5\\THOMASSQLSERVER;Database=aspnet-BookInventory-c7023200-510b-4542-873b-3fce175208e5;User ID=sa;Password=Thomas@00144277;TrustServerCertificate=True;Trusted_Connection=true;MultipleActiveResultSets=true";

            var dbContextOptionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlServer(connectionString);
            var appDbContext = new ApplicationDbContext(dbContextOptionsBuilder.Options);

            var logger = LoggerFactory.Create((builder => { })).CreateLogger<BorrowingService>();

            var borrowingService = new BorrowingService(appDbContext, logger);


            var response = await borrowingService.BorrowedBooks($"thomasluo@book.co.nz");
            Assert.True(response != null);
        }
    }
}