namespace LibaryApi.Models;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;

    //M to M
    public ICollection<Author> Authors { get; set; } = new List<Author>();
    public ICollection<Loan> Loans { get; set; } = new List<Loan>();
}