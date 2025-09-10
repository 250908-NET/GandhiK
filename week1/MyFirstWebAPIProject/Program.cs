using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

//define a minimal API Endpoint for weather forecast. GET request to /weatherforecast
//HTTP Request Types: GET, POST, PUT, DELETE, PATCH
//GET - to get data from the server
//POST - to create data on the server
//PUT - to update data on the server
//DELETE - to delete data from the server
//PATCH - to partially update data on the server
//HEAD - a get request that returns only the headers without the body
//OPTIONS - returns the supported HTTP methods for a given endpoint

/*
HTTP GET v1.1
localhost:5000/weatherforecast
headers
{
    "Accept": "application/json"
    "Response-Type": "application/json"
}

Header
Body

*/


app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

/*app.MapGet("/", () => "Hello World! Welcome to My First Web API Project");

app.MapGet("/number", () =>
{
    return 67;
});
*/

//Challenge 1: Basic Calculator
app.MapGet("/calculator/add/{a}/{b}", (int a, int b) =>
{
    return new
    {
        operation = "add",
        result = a + b
    };
});

app.MapGet("/calculator/subtract/{a}/{b}", (int a, int b) =>
{
    return new
    {
        operation = "subtract",
        result = a - b
    };
});

app.MapGet("/calculator/multiply/{a}/{b}", (int a, int b) =>
{
    return new
    {
        operation = "multiply",
        result = a * b
    };
});

app.MapGet("/calculator/divide/{a}/{b}", (double a, double b) => 
{
    if (b == 0)
        return Results.BadRequest(new { error = "Cannot divide by zero" });
    
    var result = a / b;
    return Results.Ok(new { operation = "divide", input1 = a, input2 = b, result = result });
});

//Challenge 2: String Manipulator
app.MapGet("/text/reverse/{text}", (string text) =>
{
    var reversed = new string(text.Reverse().ToArray());
    return new { result = reversed };
});

app.MapGet("/text/uppercase/{text}", (string text) =>
{
    return new { result = text.ToUpper() };
});

app.MapGet("/text/lowercase/{text}", (string text) =>
{
    return new { result = text.ToLower() };
});

app.MapGet("/text/count/{text}", (string text) =>
{
    int charCount = text.Length;
    int wordCount = text.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
    int vowelCount = text.Count(c => "aeiouAEIOU".Contains(c));

    return Results.Ok(new
    {
        operation = "count",
        characters = charCount,
        words = wordCount,
        vowels = vowelCount
    });
});

app.MapGet("/text/palindrome/{text}", (string text) =>
{
    var clean = new string(text.ToArray()).ToLower();
    var reverse = new string(clean.Reverse().ToArray());
    bool isPalindrome = clean == reverse;
    return Results.Ok(new
    {
        text,
        isPalindrome
    });
});
app.Run();

//Challenge 3: Number Games
app.MapGet("/numbers/fizzbuzz/{count}", (int count) =>
{
    var result = new List<string>();

    for (int i = 1; i <= count; i++)
    {
        if (i % 15 == 0)
            result.Add("FizzBuzz");
        else if (i % 3 == 0)
            result.Add("Fizz");
        else if (i % 5 == 0)
            result.Add("Buzz");
        else result.Add(i.ToString());
    }
});

app.MapGet("/numbers/prime/{number}", (int num) =>
{
    if (num <= 1)
        return Results.Ok(new { number = num, isPrime = false });
    for (int i = 2; i * i <= num; i++)
    {
        if (num % i == 0)
            return Results.Ok(new { number = num, isPrime = false });
    }
    return Results.Ok(new { number = num, isPrime = true });
});

app.MapGet("/numbers/fibonacci/{count}", (int count) =>
{
    
});

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
