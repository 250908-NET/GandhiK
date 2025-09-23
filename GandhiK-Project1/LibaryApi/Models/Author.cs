namespace LibaryApi.Models;

public class Author
{
    public int Id { get; set; }
    public string Name { get; set; }

    //M to M
    public ICollection<Book> Books { get; set; } = new List<Book>();
}