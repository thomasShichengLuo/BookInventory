using Azure.Core;
using BookInventory.Common;
using BookInventory.Data;
using BookInventory.Dtos;
using Microsoft.EntityFrameworkCore;

namespace BookInventory.Service
{

    public interface IBookService
    {
        Task<bool> CreateBook(BookDto book);

        Task<bool> EditBook(BookDto book);

        Task<List<BookDto>> GetBooks();

        Task<List<BookDto>> GetAllBooks();

        Task<bool> DeleteBook(Guid bookId);

        Task<BookDto> GetBookById(Guid id);
    }

    public class BookService : IBookService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<BookService> _logger;

        public BookService(ApplicationDbContext dbContext,
                ILogger<BookService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        public async Task<bool> CreateBook(BookDto book)
        {
            try
            {
                if (book == null)
                {
                    _logger.LogError($"CreateBook | book is null");
                    return false;
                }
                book.Name = book.Name?.Trim();
                book.Author = book.Author?.Trim();

                if (string.IsNullOrWhiteSpace(book.Author))
                {
                    _logger.LogError($"CreateBook | book author is null");
                    return false;
                }

                if (string.IsNullOrWhiteSpace(book.Name))
                {
                    _logger.LogError($"CreateBook | book name is null");
                    return false;
                }

                var newBook = new Book()
                {
                    Id = Guid.NewGuid(),
                    Author = book.Author,
                    Name = book.Name,
                    Status = Common.BookStatus.Available,
                };

              
                await _dbContext.Books.AddAsync(newBook);
                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception x)
            {
                _logger.LogError($"CreateBook | error");
                _logger.LogError($"\t{x.Message}");
                _logger.LogError($"\t{x.StackTrace}");
                return false;
            }
        }

        public async Task<bool> DeleteBook(Guid bookId)
        {
            try
            {
                if (bookId == default)
                {
                    _logger.LogError($"DeleteBook | book id is null");
                    return false;
                }

                var existingBook = await _dbContext.Books.FirstOrDefaultAsync(x => x.Id == bookId && x.Status != Common.BookStatus.Deleted);
                if (existingBook == null)
                {
                    _logger.LogError($"DeleteBook | not found");

                    return false;
                }
                existingBook.Status = Common.BookStatus.Deleted;

                _dbContext.Books.Update(existingBook);
                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception x)
            {
                _logger.LogError($"DeleteBook | error");
                _logger.LogError($"\t{x.Message}");
                _logger.LogError($"\t{x.StackTrace}");
                return false;
            }
        }

        public async Task<bool> EditBook(BookDto book)
        {
            try
            {
                if (book == null)
                {
                    _logger.LogError($"EditBook | book is null");
                    return false;
                }
                book.Name = book.Name.Trim();
                book.Author = book.Author.Trim();

                if (string.IsNullOrWhiteSpace(book.Author))
                {
                    _logger.LogError($"EditBook | book author is null");
                    return false;
                }

                if (string.IsNullOrWhiteSpace(book.Name))
                {
                    _logger.LogError($"EditBook | book name is null");
                    return false;
                }
                if (book.Id == default)
                {
                    _logger.LogError($"EditBook | book id is null");
                    return false;
                }

                var existingBook = await _dbContext.Books.FirstOrDefaultAsync(x => x.Id == book.Id && x.Status != Common.BookStatus.Deleted);
                if (existingBook == null)
                {
                    _logger.LogError($"EditBook | not found");

                    return false;
                }
                existingBook.Name = book.Name;
                existingBook.Author = book.Author;

                _dbContext.Books.Update(existingBook);
                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception x)
            {
                _logger.LogError($"EditBook | error");
                _logger.LogError($"\t{x.Message}");
                _logger.LogError($"\t{x.StackTrace}");
                return false;
            }
        }

        public async Task<List<BookDto>> GetBooks()
        {
            try
            {
                var books = await _dbContext.Books
                    .AsNoTracking()
                        .Where(x =>x.Status == BookStatus.Available)
                        .OrderBy(x => x.Name).ToListAsync();

                return books.BooksToDtos();
            }
            catch (Exception x)
            {
                _logger.LogError($"GetBooks | Error");
                _logger.LogError($"\t{x.Message}");
                _logger.LogError($"\t{x.StackTrace}");
                return new List<BookDto>();
            }
        }


        public async Task<List<BookDto>> GetAllBooks()
        {
            try
            {
                var books = await _dbContext.Books
                    .AsNoTracking()
                        .Where(x => x.Status == BookStatus.Available || x.Status == BookStatus.Borrowed)
                        .OrderBy(x => x.Name).ToListAsync();

                return books.BooksToDtos();
            }
            catch (Exception x)
            {
                _logger.LogError($"GetBooks | Error");
                _logger.LogError($"\t{x.Message}");
                _logger.LogError($"\t{x.StackTrace}");
                return new List<BookDto>();
            }
        }


        public async Task<BookDto> GetBookById(Guid id)
        {
            try
            {
                var book = await _dbContext.Books
                    .AsNoTracking()
                        .Where(x => x.Id == id)
                        .FirstOrDefaultAsync();
                if(book != null)
                {
                    return book.BookToDto();
                }
                return null;
            }
            catch (Exception x)
            {
                _logger.LogError($"GetBookById | Error");
                _logger.LogError($"\t{x.Message}");
                _logger.LogError($"\t{x.StackTrace}");
                return null;
            }
        }
    }
}
