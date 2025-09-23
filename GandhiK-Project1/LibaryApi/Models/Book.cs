namespace LibaryApi.Models;

public class Books
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string ISBN { get; set; }

    //M to M
    public ICollection<Author> Authors { get; set; } = new List<Author>();
}