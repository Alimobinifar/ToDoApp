using ToDoApp.Controller;
using ToDoApp.Data;
using ToDoApp.Models;
using System.Text; 

Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

bool exit = false;

var taskController = new TaskController();

while (!exit)
{
    Console.WriteLine("\n===== To-Do Task Manager =====");
    Console.WriteLine("1 - Add Task");
    Console.WriteLine("2 - Show All Tasks");
    Console.WriteLine("3 - Mark as Done");
    Console.WriteLine("4 - Delete Task");
    Console.WriteLine("5 - Update Task");
    Console.WriteLine("0 - Exit");

    var choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            Console.Write("Enter Task Title: ");
            var title = Console.ReadLine();


            Console.Write("Enter Task Description: ");
            var description = Console.ReadLine();

            var newTask = new ToDoTask
            {
                Title = title,
                Description = description,
                IsDone = false
            };

            var createResponse = await taskController.CreateTodoAsync(newTask);
            Console.WriteLine($"✅ {createResponse.Msg}");
            break;

        case "2":
            var listResponse = await taskController.ToDoList();
            if (!listResponse.Error && listResponse.List != null)
            {
                Console.WriteLine("\n===== Task List =====");
                foreach (var task in listResponse.List as List<ToDoTask>)
                {
                    Console.WriteLine($"{task.Id}. {task.Title} Task Id : {task.Id}- {(task.IsDone ? "✅ Done" : "❌ Not Done")}");
                }
            }
            else
            {
                Console.WriteLine($"🚫 {listResponse.Msg}");
            }
            break;

        case "3":
            Console.Write("Enter Task ID to mark as done: ");
            if (int.TryParse(Console.ReadLine(), out int markId))
            {
                var doneResponse = await taskController.DoneTaskAsync(markId);
                Console.WriteLine(doneResponse.Error ? $"🚫 {doneResponse.Msg}" : $"✅ {doneResponse.Msg}");
            }
            break;

        case "4":
            Console.Write("Enter Task ID to delete: ");
            if (int.TryParse(Console.ReadLine(), out int delId))
            {
                var deleteResponse = await taskController.DeleteToDoTaskAsync(delId);
                Console.WriteLine(deleteResponse.Error ? $"🚫 {deleteResponse.Msg}" : $"🗑️ {deleteResponse.Msg}");
            }
            break;

        case "5":
            Console.Write("Enter Task ID to update: ");
            if (int.TryParse(Console.ReadLine(), out int updateId))
            {
                Console.Write("Enter new Title: ");
                var newTitle = Console.ReadLine();

                Console.Write("Enter new Description: ");
                var newDescription = Console.ReadLine();

                var updatedTask = new ToDoTask
                {
                    Title = newTitle,
                    Description = newDescription
                };

                var updateResponse = await taskController.UpdateTaskAsync(updateId, updatedTask);
                Console.WriteLine(updateResponse.Error ? $"🚫 {updateResponse.Msg}" : $"✅ {updateResponse.Msg}");
            }
            break;

        case "0":
            exit = true;
            Console.WriteLine("👋 Exiting... Goodbye!");
            break;

        default:
            Console.WriteLine("⚠️ Invalid option! Please try again.");
            break;
    }
}


