using TaskApi.Models;
using TaskApi.Services;
using Serilog;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

//-----------------Serilog-----------------
/*Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();
*/
//----------------Swagger & Xml--------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Enable Swagger in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//--------------------Authentication & Authorization-------------------
app.UseHttpsRedirection();
//-------------------------Logging Middleware---------------------------
/*app.Use(async (HttpContext context, RequestDelegate next) =>
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
});*/

//in-memory storage
var taskService = new TaskService();
var users = new List<User>();

//------------------------------Endpoints-------------------------------

//auth endpoints
app.MapPost("/api/tasks/register", (User user) =>
{
    if (users.Any(u => u.Username == user.Username))
    {
        return Results.BadRequest(new
        {
            success = false,
            errors = "Username already exists",
            message = "Operation Failed"
        });
    }

    users.Add(user);
    return Results.Ok(new
    {
        success = true,
        data = new { user.Id, user.Username },
        message = "User registered successfully"
    });
});

app.MapPost("/api/auth/login", (User user) =>
{
    if (user == null)
    {
        return Results.BadRequest(new
        {
            success = false,
            errors = "Invalid credentials",
            message = "Login failed"
        });
    }

    //var token = GenerateJwtToken(user);
    return Results.Ok(new
    {
        success = true,
        //token,
        message = "Login successful"
    });
});

/// <summary>
/// get all tasks with optional filtering
/// <summary>
/// <param name="isCompleted">Filter by completion status</param>
/// <param name="priority">Filter by priority (Low, Medium, High)</param>
/// <param name="dueBefore">Filter tasks due before this date</param>
/// <returns>List of takss</returns>
app.MapGet("/api/tasks/getAll",  (bool? isCompleted, Priority? priority, DateTime? dueBefore,
                                ILogger<Program> logger) =>
{
    var tasks = taskService.GetAll(isCompleted, priority, dueBefore);
    return Results.Ok(new
    {
        success = true,
        data = tasks,
        message = "Operation completed successfully"
    });
}).WithName("GetAllTask");

/// <summary>
/// get all tasks by id
/// <summary>
/// <param name="id">unique id of task</param>
/// <returns>List of tasks by id</returns>
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

/// <summary>
/// get all tasks by id
/// <summary>
/// <param name="newTask">object handle tasks details</param>
/// <returns>auth of task creation</returns>
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

/// <summary>
/// update task by id
/// <summary>
/// <param name="id">unique id of task</param>
/// <returns>update task by id</returns>
app.MapPut("/api/tasks/update/{id}", (int id, [FromBody] TaskItem updatedTask, ILogger<Program> logger) =>
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

/// <summary>
/// delete task by id
/// <summary>
/// <param name="id">unique id of task</param>
/// <returns>deletion of task</returns>
app.MapDelete("/api/tasks/delete/{id}", (int id, ILogger<Program> logger) =>
{
    var delete = taskService.Delete(id);

    if (!delete) // task not found
    {
        return Results.NotFound(new
        {
            success = false,
            errors = new[] { $"Task with id {id} not found" },
            message = "Operation failed"
        });
    }
    
    return Results.Ok(new
    {
        success = true,
        data = delete,
        message = "Operation completed successfully"
    });
}).WithName("TaskDeleted");

app.Run();