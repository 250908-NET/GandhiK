namespace ToDoApp.Models;

public class ToDoItemBase
{
    public int Id { get; set;}
    public string Title { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CreatedDate { get; set; }

    public override string ToString()
    {
        return $"ID: {Id}, Title: {Title}, Complete: {IsCompleted}, Date of Creation: {CreatedDate}";
    }
}