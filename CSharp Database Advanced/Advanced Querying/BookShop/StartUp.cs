using BookShop.Data;
using BookShop.Data.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;
using Z.EntityFramework.Plus;

namespace BookShop
{
    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            //var input = int.Parse(Console.ReadLine());

            Console.WriteLine(RemoveBooks(db));

        }

        //02.Age Restriction
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            var restriction = Enum.Parse<AgeRestriction>(command, true);

            var booksWithRestriction = context.Books
                .Where(b => b.AgeRestriction == restriction)
                .Select(b => b.Title)
                .OrderBy(x => x)
                .ToList();

            return string.Join(Environment.NewLine, booksWithRestriction);
        }

        //03.Golden Books
        public static string GetGoldenBooks(BookShopContext context)
        {
            var goldenBooks = context.Books
                .Where(b => b.Copies < 5000 &&
                b.EditionType == EditionType.Gold)
                .Select(b => new
                {
                    b.BookId,
                    b.Title
                })
                .OrderBy(b => b.BookId)
                .ToList();

            return string.Join(Environment.NewLine, goldenBooks.Select(b => b.Title));
        }

        //04.Books by Price
        public static string GetBooksByPrice(BookShopContext context)
        {
            var sb = new StringBuilder();

            var bookPrice = context.Books
                .Where(b => b.Price > 40)
                .Select(b => new
                {
                    b.Title,
                    b.Price
                })
                .OrderByDescending(b => b.Price)
                .ToList();

            foreach (var book in bookPrice)
            {
                sb.AppendLine($"{book.Title}! - ${book.Price:F2}");
            }

            return sb.ToString().TrimEnd();
        }

        //05.Not Released In
        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var bookRelease = context.Books
                .Where(b => b.ReleaseDate.Value.Year != year)
                .Select(b => new
                {
                    b.Title,
                    b.BookId
                })
                .OrderBy(b => b.BookId)
                .ToList();

            var sb = new StringBuilder();

            foreach (var item in bookRelease)
            {
                sb.AppendLine(item.Title);
            }

            return sb.ToString().TrimEnd();
        }

        //06.Book Titles by Category
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            var spllited = input
                    .ToLower()
                    .Split(" ", StringSplitOptions.RemoveEmptyEntries);

            var bookCategory = context.Books
                .Where(b => b.BookCategories.Any(c => spllited.Contains(c.Category.Name.ToLower())))
                .Select(t => t)
                .OrderBy(t => t)
                .ToList();

            return string.Join(Environment.NewLine, spllited);
        }

        //07.Released Before Date
        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            var dataFormat = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var bookRelease = context.Books
                .Where(b => b.ReleaseDate.Value < dataFormat)
                .Select(b => new
                {
                    b.Title,
                    b.EditionType,
                    b.Price,
                    b.ReleaseDate
                })
                .OrderByDescending(b => b.ReleaseDate)
                .ToList();

            var sb = new StringBuilder();

            foreach (var book in bookRelease)
            {
                sb.AppendLine($"{book.Title} - {book.EditionType} - ${book.Price}");
            }

            return sb.ToString().TrimEnd();
        }

        //08.Author Search
        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var author = context.Authors
                .Where(a => a.FirstName.EndsWith(input))
                .Select(x => $"{x.FirstName} {x.LastName}")
                .ToList()
                .OrderBy(a => a);

            return string.Join(Environment.NewLine, author);
        }

        //09.Book Search
        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var title = context.Books
                .Where(b => b.Title.Contains(input))
                .Select(b => b.Title)
                .OrderBy(b => b)
                .ToList();

            return string.Join(Environment.NewLine, title);
        }

        //10.Book Search by Author
        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var sb = new StringBuilder();

            var books = context.Books
                .Include(b => b.Author)
                .Where(a => a.Author.LastName.StartsWith(input))
                .Select(a => new
                {
                    a.BookId,
                    a.Title,
                    FullName = $"{a.Author.FirstName} {a.Author.LastName}"
                })
                .OrderBy(b => b.BookId)
                .ToList();

            foreach (var item in books)
            {
                sb.AppendLine($"{item.Title} ({item.FullName})");
            }

            return sb.ToString().TrimEnd();
        }

        //11.Count Books
        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            var titleLenght = context.Books
                .Where(b => b.Title.Length > lengthCheck)
                .Count();

            Console.WriteLine($"There are {titleLenght} books with longer title than {lengthCheck} symbols");

            return titleLenght;
        }

        //12.Total Book Copies
        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var sb = new StringBuilder();

            var bookCopies = context.Authors
                .Select(a => new
                {
                    FullName = $"{a.FirstName} {a.LastName}",
                    Copies = a.Books.Sum(c => c.Copies)
                })
                .OrderByDescending(c => c.Copies)
                .ToList();

            foreach (var item in bookCopies)
            {
                sb.AppendLine($"{item.FullName} - {item.Copies}");
            }

            return sb.ToString().TrimEnd();
        }

        //13.Profit by Category
        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var sb = new StringBuilder();

            var profit = context.Categories
                .Select(p => new
                {
                    p.Name,
                    TotalProfit = p.CategoryBooks.Sum(x => x.Book.Price * x.Book.Copies)
                })
                .OrderByDescending(c => c.TotalProfit)
                .ThenBy(c => c.Name)
                .ToList();

            foreach (var item in profit)
            {
                sb.AppendLine($"{item.Name} ${item.TotalProfit:F2}");
            }

            return sb.ToString().TrimEnd();
        }

        //14.Most Recent Books
        public static string GetMostRecentBooks(BookShopContext context)
        {
            var sb = new StringBuilder();

            var recent = context.Categories
                .Select(c => new
                {
                    c.Name,
                    Category = c.CategoryBooks.Select(x => new
                    {
                        x.Book.Title,
                        Release =x.Book.ReleaseDate.Value
                    })
                    .OrderByDescending(x => x.Release)
                    .Take(3)
                })
                .OrderBy(c => c.Name)
                .ToList();


            foreach (var item in recent)
            {
                sb.AppendLine($"--{item.Name}");

                foreach (var categ in item.Category)
                {
                    sb.AppendLine($"{categ.Title} ({categ.Release})");
                }
            }

            return sb.ToString().TrimEnd();
        }

        //15.Increase Price
        public static void IncreasePrices(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.ReleaseDate.Value.Year < 2010)
                //.Update(x => new Book() { Price = x.Price + 5 })
                .ToList();

            foreach (var item in books)
            {
                item.Price += 5;
            }

            context.SaveChanges();
        }

        //16.Remove Books
        public static int RemoveBooks(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.Copies < 4200)
                .ToList();

            context.Books.RemoveRange(books);
            context.SaveChanges();

            return books.Count;
        }
    }
}


