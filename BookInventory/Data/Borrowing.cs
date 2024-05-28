namespace BookInventory.Data
{
    public class Borrowing
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public Guid BookId { get; set; }

        public Book Book { get; set; }

        public DateTime BorrowTime { get; set; }

        public DateTime? ReturnTime { get; set; }
    }
}
