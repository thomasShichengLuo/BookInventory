using BookInventory.Common;
using BookInventory.Components.Pages;
using BookInventory.Data;
using BookInventory.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookInventory.Service
{

    public interface IBorrowingService
    {
        Task<bool> BorrowingBook(Guid id, string UserEmail);

        Task<bool> ReturnBook(Guid id, string UserEmail);

        Task<List<BookDto>> BorrowedBooks(string UserEmail);
    }

    public class BorrowingService : IBorrowingService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<BorrowingService> _logger;

        public BorrowingService(ApplicationDbContext dbContext,
                ILogger<BorrowingService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<List<BookDto>> BorrowedBooks(string UserEmail)
        {
            try
            {
                if (string.IsNullOrEmpty(UserEmail.Trim()))
                {
                    _logger.LogError($"BorrowedBooks | User Email is null");
                    return new List<BookDto>();
                }

                var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == UserEmail);
                if (user == null)
                {
                    _logger.LogError($"BorrowedBooks | User not found");

                    return new List<BookDto>();
                }

                var books = await _dbContext.Borrowings
                .AsNoTracking()
                .Where(x=> x.ReturnTime == null)
                .Include(x=> x.Book)
                    .Where(x => x.Book.Status == BookStatus.Borrowed)
                    .OrderBy(x => x.BorrowTime)
                    .Select(x =>x.Book).ToListAsync();

                return books.BooksToDtos();
            }
            catch (Exception x)
            {
                _logger.LogError($"BorrowedBooks | error");
                _logger.LogError($"\t{x.Message}");
                _logger.LogError($"\t{x.StackTrace}");
                return new List<BookDto>();
            }
        }

        public async Task<bool> BorrowingBook(Guid id, string UserEmail)
        {
            try
            {
                if (id == default)
                {
                    _logger.LogError($"BorrowingBook | book id is null");
                    return false;
                }

                var existingBook = await _dbContext.Books.FirstOrDefaultAsync(x => x.Id == id && x.Status == Common.BookStatus.Available);
                if (existingBook == null)
                {
                    _logger.LogError($"BorrowingBook | Book not found");

                    return false;
                }
                existingBook.Status = Common.BookStatus.Borrowed;

                var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == UserEmail);
                if(user == null)
                {
                    _logger.LogError($"BorrowingBook | User not found");

                    return false;
                }

                var borrowing = new Data.Borrowing()
                {
                    BookId = id,
                    BorrowTime = DateTime.Now,
                    UserId = user.Id,

                };


                _dbContext.Books.Update(existingBook);
                await _dbContext.Borrowings.AddAsync(borrowing);
                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception x)
            {
                _logger.LogError($"BorrowingBook | error");
                _logger.LogError($"\t{x.Message}");
                _logger.LogError($"\t{x.StackTrace}");
                return false;
            }
        }

        public async Task<bool> ReturnBook(Guid id, string UserEmail)
        {
            try
            {
                if (id == default)
                {
                    _logger.LogError($"ReturnBook | book id is null");
                    return false;
                }

                var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == UserEmail);
                if (user == null)
                {
                    _logger.LogError($"ReturnBook | User not found");

                    return false;
                }
                var existingBorrowing = await _dbContext.Borrowings.FirstOrDefaultAsync(x => x.BookId == id && x.ReturnTime == null && x.UserId == user.Id);
                if (existingBorrowing == null)
                {
                    _logger.LogError($"ReturnBook | Borrowing is not found");

                    return false;
                }
                existingBorrowing.ReturnTime = DateTime.Now;
                var existingBook = await _dbContext.Books.FirstOrDefaultAsync(x => x.Id == existingBorrowing.BookId && x.Status == Common.BookStatus.Borrowed);
                if (existingBook == null)
                {
                    _logger.LogError($"ReturnBook | Book not found");

                    return false;
                }
                existingBook.Status = Common.BookStatus.Available;

                _dbContext.Books.Update(existingBook);
                _dbContext.Borrowings.Update(existingBorrowing);
                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception x)
            {
                _logger.LogError($"ReturnBook | error");
                _logger.LogError($"\t{x.Message}");
                _logger.LogError($"\t{x.StackTrace}");
                return false;
            }
        }
    }
}
