using ToDoApp.Models;

namespace ToDoApp
{
    class program
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
                        

                }

            }
        }
    }
}