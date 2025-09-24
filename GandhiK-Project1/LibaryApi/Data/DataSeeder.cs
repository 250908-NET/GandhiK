using LibaryApi.Models;

namespace LibaryApi.Data
{
    public static class DataSeeder
    {
        public static void Seed(LibraryContext context)
        {
            if (!context.Books.Any())
            {
                var author = new Author { Name = "George Orwell" };
                var book = new Book { Title = "1984", ISBN = "1234567890", Authors = new List<Author> { author } };
                var member = new Member { Name = "John Doe", Email = "john@example.com" };

                context.Authors.Add(author);
                context.Books.Add(book);
                context.Members.Add(member);
                context.SaveChanges();
            }
        }
    }
}
