using TaskApi.Models;

namespace TaskApi.Services
{
    public class TaskService
    {
        public List<TaskItem> tasks = new List<TaskItem>();
        private int nextId = 1;

        public IEnumerable<TaskItem> GetAll(bool? isCompleted, Priority? priority, DateTime? dueBefore)
        {
            // Start with all tasks
            var resultSet = new List<TaskItem>();

            // Apply filters if provided
            foreach (var item in tasks)
            {
                if (isCompleted.HasValue && item.IsCompleted != isCompleted.Value)
                    continue;
                if (priority.HasValue && item.Priority != priority.Value)
                    continue;
                if (dueBefore.HasValue && item.DueDate >= dueBefore.Value)
                    continue;

                resultSet.Add(item);
            }

            return resultSet;
        }

        //get a sigle task by id
        public TaskItem getById(int id)
        {
            foreach (var task in tasks)
            {
                if (task.Id == id)
                {
                    return task;
                }
            }
            return null;
        }

        //add Item
        public TaskItem Add(TaskItem newTask)
        {
            int i = 0;
            newTask.Id = i++;
            newTask.CreatedAt = DateTime.UtcNow;
            newTask.UpdatedAt = DateTime.UtcNow;
            tasks.Add(newTask);
            return newTask;
        }

        public TaskItem Update(int id, TaskItem updatedTask)
        {
            var task = getById(id);
            task.Title = updatedTask.Title;
            task.Description = updatedTask.Description;
            task.IsCompleted = updatedTask.IsCompleted;
            task.Priority = updatedTask.Priority;
            task.DueDate = updatedTask.DueDate;
            task.UpdatedAt = DateTime.UtcNow;

            return task;
        }

        public bool Delete(int id)
        {
            var task = getById(id);
            tasks.Remove(task);
            return true;
        }
    }
}