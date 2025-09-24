using LibaryApi.Data;
using LibaryApi.Endpoints;
using LibaryApi.Repositories;
using LibaryApi.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<LibraryContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register repositories
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();

string CS = File.ReadAllText("connection_string.env");

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapPost("/books", async (Book dto, IBookRepository repo) =>
{
    var book = new Book
    {
        Title = dto.Title,
        ISBN = dto.ISBN
    };
    var result = await repo.AddBookAsync(book);
    return Results.Created($"/books/{result.Id}", result);
});

app.MapGet("/books", async (IBookRepository repo) =>
{
    var books = await repo.GetBooksAsync();
    return Results.Ok(books);
});

// Author endpoints
app.MapPost("/authors", async (AuthorDto dto, IAuthorRepository repo) =>
{
    var author = new Author { Name = dto.Name };
    var result = await repo.AddAuthorAsync(author);
    return Results.Created($"/authors/{result.Id}", result);
});

app.MapGet("/authors", async (IAuthorRepository repo) =>
{
    var authors = await repo.GetAuthorsAsync();
    return Results.Ok(authors);
});

app.Run();