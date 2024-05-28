using BookInventory.Common;

namespace BookInventory.Data
{
    public class Book
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string? Author { get; set; }

        public BookStatus Status { get; set; }
    }
}
