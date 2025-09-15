using TaskApi.Models;
using TaskApi.Services;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//in-memory storage
var taskService = new TaskService();

//GET /api/tasks - Get all tasks with optional filtering
//Query parameters: isCompleted, priority, dueBefore
app.MapGet("/api/tasks/getAll",  (bool? isCompleted, Priority? priority, DateTime? dueBefore) =>
{
    var tasks = taskService.GetAll(isCompleted, priority, dueBefore);
    return Results.Ok(tasks);
});

//GET /api/tasks/{id} - Get specific task by ID
app.MapGet("/api/tasks/getById/{id}", (int id) =>
{
    var task = taskService.getById(id);
    return Results.Ok(task);
    //return Results.Ok(new { Message = "Get task by ID", id });
});

//POST /api/tasks - Create new task
app.MapPost("/api/tasks/create", (TaskItem newTask) =>
{
    if (string.IsNullOrWhiteSpace(newTask.Title))
    {
        return Results.BadRequest(new { Error = "Title is required"});
    }

    var create = taskService.Add(newTask);
    //return Results.Ok(new { Message = "Create new task" });
    return Results.Created($"/api/tasks/{create.Id}", create);
});

//PUT /api/tasks/{id} - Update existing task
app.MapPut("/api/tasks/update/{id}", (int id, TaskItem updatedTask) =>
{

    return Results.Ok(new { Message = "Update task", id });
});

//DELETE /api/tasks/{id} - Delete task
app.MapDelete("/api/tasks/delete/{id}", (int id) =>
{
    return Results.Ok(new { Message = "Delete task", id });
});
app.Run();