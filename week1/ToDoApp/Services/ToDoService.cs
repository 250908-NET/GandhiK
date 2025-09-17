using ToDoApp.Models;
namespace ToDoApp.Services;

public class ToDoService : ItoDoService
{
    private List<ToDoItem> tasks;

    public ToDoService()
    {
        tasks = new List<ToDoItem>();
    }

    public void addItem(int id, string title)
    {
        ToDoItem newItem = new ToDoItem(id++, title);
        tasks.Add(newItem);
    }
    public bool MarkItemComplete(int id)
    {

        return false;
    }
    public bool MarkItemIncomplete(int id)
    {

        return false;
    }
    public bool DeleteItem(int id)
    {

        return false;
    }
    public void viewItems()
    {

    }

    public ToDoItem GetByID(int id)
    {
        for (int i = 0; i < tasks.count; i++)
        {
            if (tasks[i].id == id)
            {
                return tasks[i];
            }
        }
        return null;
    }

    public override string ToString()
    {
        string result = "";
        for (int i = 0; i < tasks.Count; i++)
        {
            result += tasks[i].ToString() + "\n";
        }
        return result;
    }
}