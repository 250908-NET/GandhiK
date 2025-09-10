using System;
namespace ToDoApp.Models
{
    public class ToDoItem
    {
        public int id { get; set; }
        public string Title { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedDate { get; set; }

        public ToDoItem(int id, string title, bool isCompleted, DateTime createdDate)
        {
            this.id = id;
            this.Title = title;
            this.IsCompleted = isCompleted;
            this.CreatedDate = createdDate;
        }

        public override string ToString()
        {
            return $"{id}: {Title}: (Created on {CreatedDate}) - {(IsCompleted ? "Completed" : "Pending")}";
        }
    }
}