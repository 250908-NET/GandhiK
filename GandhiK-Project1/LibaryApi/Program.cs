using LibaryApi.Data;
using LibaryApi.Repositories;
using LibaryApi.Interfaces;
using LibaryApi.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
//string cs = File.ReadAllText("../connection_string.env");
builder.Services.AddDbContext<LibraryContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Register repositories
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<ILoanRepository, LoanRepository>();
builder.Services.AddScoped<IMemberRepository, MemberRepository>();

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/", () => "Hello");

//--------------------------Books endpoints-------------------------------
//get all books
app.MapGet("/books", async (LibraryContext db) =>
    await db.Books
        .Select(b => new 
        {
            b.Id,
            b.Title,
            b.ISBN,
            Authors = b.Authors.Select(a => new { a.Id, a.Name }) // include author names
        })
        .ToListAsync()
);


//post a new book
app.MapPost("/books", async (LibraryContext db, Book book) =>
{
    db.Books.ToListAsync();
});

//delete a book
app.MapDelete("/books/{id}", async (LibraryContext db, int id) =>
{
    var book = await db.Books.FindAsync(id);
    if (book is null) return Results.NotFound();

    db.Books.Remove(book);
    await db.SaveChangesAsync();
    return Results.Ok(book);
});
//--------------------------Member endpoints-------------------------------
app.MapGet("/members", async (LibraryContext db) =>
{
     await db.Members.ToListAsync();
});

app.MapPost("/members", async (LibraryContext db, Member member) =>
{
    db.Members.Add(member);
    await db.SaveChangesAsync();
    return Results.Created($"/members/{member.Id}", member);
});

//--------------------------Loan endpoints-------------------------------
app.MapGet("/loans", async (LibraryContext db) =>
    await db.Loans.Include(l => l.Book).Include(l => l.Member).ToListAsync());

app.MapPost("/loans", async (LibraryContext db, Loan loan) =>
{
    db.Loans.Add(loan);
    await db.SaveChangesAsync();
    return Results.Created($"/loans/{loan.Id}", loan);
});
app.Run();