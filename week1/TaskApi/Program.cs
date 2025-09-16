using TaskApi.Models;
using TaskApi.Services;
using Serilog;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

//-----------------Serilog-----------------
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

//----------------Swagger--------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Enable Swagger in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//-------------------------Logging Middleware---------------------------
app.Use(async (HttpContext context, RequestDelegate next) =>
{
    var corrleationId = Guid.NewGuid().ToString();
    context.Items["CorrelationId"] = corrleationId;
    context.Response.Headers.Add("Correlation-ID", corrleationId);

    var statusCode = context.Response.StatusCode;
    var method = context.Request.Method;
    var path = context.Request.Path;

    Serilog.Log.Information("CorrelationID: ", corrleationId);
    Serilog.Log.Information("Status Code: ", statusCode);
    Serilog.Log.Information("Method: ", method);
    Serilog.Log.Information("Path: ", path);
});

//in-memory storage
var taskService = new TaskService();

//------------------------------Endpoints-------------------------------

//GET /api/tasks - Get all tasks with optional filtering
//Query parameters: isCompleted, priority, dueBefore
app.MapGet("/api/tasks/getAll",  (bool? isCompleted, Priority? priority, DateTime? dueBefore, ILogger<Program> logger) =>
{
    var tasks = taskService.GetAll(isCompleted, priority, dueBefore);
    return Results.Ok(new
    {
        success = true,
        data = tasks,
        message = "Operation completed successfully"
    });
}).WithName("GetAllTask");

//GET /api/tasks/{id} - Get specific task by ID
app.MapGet("/api/tasks/getById/{id}", (int id, ILogger<Program> logger) =>
{
    var task = taskService.getById(id);

    if (task is null)
    {
        return Results.NotFound(new
        {
            success = false,
            errors = new[] { $"Task with id {id} not found" },
            message = "Operation failed"
        });
        logger.LogInformation("fail");
    }

    return Results.Ok(new
    {
        success = true,
        data = task,
        message = "Operation completed successfully"
    });
}).WithName("GetTaskById");

//POST /api/tasks - Create new task
app.MapPost("/api/tasks/create", (TaskItem newTask, ILogger<Program> logger) =>
{
    if (string.IsNullOrWhiteSpace(newTask.Title))
    {
        return Results.BadRequest(new { Error = "Title is required" });
    }

    if (newTask.Title.Length > 100)
    {
        return Results.BadRequest(new { Error = "Title must be < 100 characters" });
    }

    if (!string.IsNullOrWhiteSpace(newTask.Description) && newTask.Description.Length > 500)
        return Results.BadRequest(new { Message = "Description must be < 500 characters" });

    var create = taskService.Add(newTask);

    return Results.Ok(new
    {
        success = true,
        data = create,
        message = "Operation completed successfully"
    });
}).WithName("TaskCreated");

//PUT /api/tasks/{id} - Update existing task
app.MapPut("/api/tasks/update/{id}", (int id, TaskItem updatedTask, ILogger<Program> logger) =>
{
    var task = taskService.Update(id, updatedTask);

    if (task is null)
    {
        return Results.NotFound(new
        {
            success = false,
            errors = $"Task with {id} not found",
            message = "Operation failed"
        });
    }

    return Results.Ok(new
    {
        success = true,
        data = task,
        message = "Operation completed successfully"
    });
}).WithName("TaskUpdated");

//DELETE /api/tasks/{id} - Delete task
app.MapDelete("/api/tasks/delete/{id}", (int id, ILogger<Program> logger) =>
{
    var delete = taskService.Delete(id);
    return Results.Ok(new
    {
        success = true,
        data = delete,
        message = "Operation completed successfully"
    });
}).WithName("TaskDeleted");
app.Run();