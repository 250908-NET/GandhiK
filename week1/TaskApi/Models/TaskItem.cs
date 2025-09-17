using System;
using System.ComponentModel.DataAnnotations;

namespace TaskApi.Models
{
    //enum for task priority levels
    public enum Priority
    {
        Low,
        Medium,
        High,
        Critical
    }

    //class representing a task item
    public class TaskItem
    {
        public int Id { get; set; }

        // Title is required and has a max length of 100 characters
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        // Description is optional but has a max length of 500 characters
        [MaxLength(500)]
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public Priority Priority { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UserId { get; set; }
    }
}