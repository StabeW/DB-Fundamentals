using BookShop.Data.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShop.Data.Models
{
    public class Book
    {
        public int BookId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public int Copies { get; set; }

        [Column(TypeName ="Money")]
        public decimal Price { get; set; }

        public EditionType EditionType { get; set; }

        public AgeRestriction AgeRestriction { get; set; }

        public int AuthorId { get; set; }
        public Author Author { get; set; }

        public ICollection<BookCategory> BookCategories { get; set; } = new HashSet<BookCategory>();
    }
}
