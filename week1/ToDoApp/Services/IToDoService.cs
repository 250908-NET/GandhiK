using ToDoApp.Models;
namespace ToDoApp.Services;

public interface ItoDoService
{
    ToDoTask GetbyID(int id);
    void addIItem(int id, string title);
    bool MarkItemComplete(int id);
    bool MarkItemIncomplete(int id);
    bool DeleteItem(int id);
}