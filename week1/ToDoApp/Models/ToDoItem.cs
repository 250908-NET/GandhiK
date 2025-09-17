namespace ToDoApp.Models;

public class ToDoItem : ToDoItemBase
{
    public int id { get; set; }
    public string Title { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CreatedDate { get; set; }

    public ToDoItem(int id, string title)
    {
        this.id = 0;
        this.Title = "";
        this.IsCompleted = false;
        this.CreatedDate = DateTime.Now;
    }

    public ToDoItem(int id, string title)
    {
        this.id = id;
        this.Title = title;
        this.IsCompleted = false;
        this.CreatedDate = DateTime.Now;
    }

    public override string ToString()
    {
        return $"{id}: {Title}: (Created on {CreatedDate}) - {(IsCompleted ? "Completed" : "Pending")}";
    }
}