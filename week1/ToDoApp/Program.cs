/*using ToDoApp.Models;
using ToDoApp.Services;
using System;

class Program
{
    static void Main(string[] args)
    {
        ItoDoService toDoService = new ToDoService();

        bool run = true;
        while (run)
        {
            console.WriteLine("=== TO-DO LIST MANAGER ===");
            console.WriteLine("1. Add new item");
            Console.WriteLine("2. View all items");
            Console.WriteLine("3. Mark Item Complete");
            Console.WriteLine("4. Mark Item Incomplete");
            Console.WriteLine("5. Delete Item");
            Console.WriteLine("6. Exit");

            Console.write("Choose an option (1-6): ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case 1:
                    int id = 1;
                    Console.Write("Enter Task Title: ");
                    string title = Console.ReadLine();
                    toDoService.addItem(id, title);
                    break;
                case 2:
                    toDoService.viewItems();
                    break;
                case 3:
                    Console.Write("Enter item ID to mark complete: ");
                    int completeID = int.Parse(Console.ReadLine());
                    toDoService.markItemComplete(completeID);
                    break;
                case 4:
                    Console.Write("Enter item ID to mark incomplete: ");
                    int incompleteID = int.Parse(Console.ReadLine());
                    toDoService.markItemIncomplete(incompleteID);
                    break;
                case 5:
                    Console.Write("Enter item ID to delete: ");
                    int deleteID = int.Parse(Console.ReadLine());
                    toDoService.deleteItem(deleteID);
                    break;
                case 6:
                    run = false;
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }

        }
    }
}*/
using System;
using ToDoApp.Models;
using ToDoApp.Services;
class Program
{

    static void Main(string[] args)
    {
        ToDoServices toDoServices = new ToDoServices();
        

        Console.WriteLine("=== TO-DO LIST MANAGER ===");
        Console.WriteLine("1. Add To-Do List");
        Console.WriteLine("2. View all To-Do Items");
        Console.WriteLine("3. Mark To-Do Item as Completed");
        Console.WriteLine("4. Mark To-Do Item as Incomplete");
        Console.WriteLine("5. Remove To-Do Item");
        Console.WriteLine("6. Exit");
        string input = "";
        do
        {

            Console.Write("Select an option (1-6): ");
            input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    Console.Write("Enter ID: ");
                    int id = int.Parse(Console.ReadLine());
                    Console.Write("Title: ");
                    string title = Console.ReadLine();
                    toDoServices.AddItem(id, title);

                    break;
                case "2":
                    Console.WriteLine(toDoServices);
                    break;
                case "3":
                    Console.Write("Enter ID: ");
                    int compId = int.Parse(Console.ReadLine());
                    if (toDoServices.MarkItemAsCompleted(compId))
                    {
                        Console.WriteLine($"Item Has been marked as completed.");
                    }
                    else
                    {
                        Console.WriteLine($"Item with ID {compId} not found.");
                    }
                    break;
                case "4":
                    Console.Write("Enter ID: ");
                    int inCompId = int.Parse(Console.ReadLine());
                    if (toDoServices.MarkItemAsNotCompleted(inCompId))
                    {
                        Console.WriteLine($"Item Has been marked as Incompleted.");
                    }
                    else
                    {
                        Console.WriteLine($"Item with ID {inCompId} not found.");
                    }
                    break;
                case "5":
                    Console.Write("Enter ID: ");
                    int delId = int.Parse(Console.ReadLine());
                    if (toDoServices.DeleteItem(delId))
                    {
                        Console.WriteLine($"Item Has been deleted.");
                    }
                    else
                    {
                        Console.WriteLine($"Item with ID {delId} not found.");
                    }
                    break;
                case "6":
                    Console.WriteLine("Exiting the application. Goodbye!");
                    break;
                default:
                    Console.WriteLine("Invalid option. Please select a number between 1 and 6.");
                    break;
            }


        } while (input != "6");
        
    }

}