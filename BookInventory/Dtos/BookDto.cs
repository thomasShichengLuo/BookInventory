using BookInventory.Common;
using BookInventory.Data;

namespace BookInventory.Dtos
{
    public class BookDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Author { get; set; }

        public BookStatus Status { get; set; }
    }


    public static class BookDtoExtensions
    {
        public static BookDto BookToDto(this Book book)
        {
            return new BookDto
            {
                Id = book.Id,
                Author = book.Author,
                Name = book.Name,
                Status = book.Status,
            };
        }


        public static List<BookDto> BooksToDtos(this List<Book> books)
        {
            return books.Select(BookToDto).ToList();
        }
    }
}
