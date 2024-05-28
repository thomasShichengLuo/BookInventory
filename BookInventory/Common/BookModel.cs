using System.ComponentModel.DataAnnotations;

namespace BookInventory.Common
{
    public class BookModel
    {
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Author is required")]
        public string Author { get; set; }

    }
}
