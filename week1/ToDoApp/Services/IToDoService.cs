using ToDoAPP.Models;

namespace ToDoApp.Services
{
    public interface ItoDoService
    {
        void addITem(string title);
        List<ToDoItem> getAllItems();
        bool MarkItemComplete(int id);
        bool MarkItemIncomplete(int id);
        bool DeleteItem(int id);
    }
}