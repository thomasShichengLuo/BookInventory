using BookInventory.Data;

namespace BookInventory.Dtos
{
    public class BorrowingDto
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public Guid BookId { get; set; }

        public DateTime BorrowTime { get; set; }

        public DateTime? ReturnTime { get; set; }
    }
}
