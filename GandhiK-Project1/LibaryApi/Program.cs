using LibaryApi.Data;
using LibaryApi.Repositories;
using LibaryApi.Interfaces;
using LibaryApi.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
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
    db.Books.Add(book);
    await db.SaveChangesAsync();
    return Results.Created($"/books/{book.Id}", book);
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
// GET all members
app.MapGet("/members", async (LibraryContext db) =>
    await db.Members
        .Select(m => new 
        {
            m.Id,
            m.Name,
            Loans = m.Loans.Select(l => new { l.Id, Book = new { l.Book.Id, l.Book.Title } })
        })
        .ToListAsync()
);

app.MapPost("/members", async (LibraryContext db, Member member) =>
{
    db.Members.Add(member);
    await db.SaveChangesAsync();
    return Results.Created($"/members/{member.Id}", member);
});

// DELETE member
app.MapDelete("/members/{id}", async (LibraryContext db, int id) =>
{
    var member = await db.Members.FindAsync(id);
    if (member is null) return Results.NotFound();
    db.Members.Remove(member);
    await db.SaveChangesAsync();
    return Results.Ok(member);
});

//--------------------------Loan endpoints-------------------------------
// GET all loans
app.MapGet("/loans", async (LibraryContext db) =>
    await db.Loans
        .Select(l => new 
        {
            l.Id,
            Book = new { l.Book.Id, l.Book.Title },
            Member = new { l.Member.Id, l.Member.Name },
            l.LoanDate,
            l.ReturnDate
        })
        .ToListAsync()
);

// POST new loan
app.MapPost("/loans", async (LibraryContext db, Loan loan) =>
{
    var bookExists = await db.Books.AnyAsync(b => b.Id == loan.BookId);
    var memberExists = await db.Members.AnyAsync(m => m.Id == loan.MemberId);

    if (!bookExists || !memberExists)
        return Results.BadRequest("Invalid BookId or MemberId");

    db.Loans.Add(loan);
    await db.SaveChangesAsync();
    return Results.Created($"/loans/{loan.Id}", loan);
});

// DELETE loan
app.MapDelete("/loans/{id}", async (LibraryContext db, int id) =>
{
    var loan = await db.Loans.FindAsync(id);
    if (loan is null) return Results.NotFound();
    db.Loans.Remove(loan);
    await db.SaveChangesAsync();
    return Results.Ok(loan);
});

app.Run();