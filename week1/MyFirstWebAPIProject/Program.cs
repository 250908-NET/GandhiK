using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.AspNetCore.Mvc;

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
    var reverse = new string(text.Reverse().ToArray());
    bool isPalindrome = text == reverse;

    return Results.Ok(new
    {
        text,
        isPalindrome
    });
});

//Challenge 3: Number Games
app.MapGet("/numbers/fizzbuzz/{count}", (int count) =>
{
    var result = new List<string>();
    for (int i = 1; i <= count; i++)
    {
        if (i % 3 == 0 && i % 5 == 0)
            result.Add("FizzBuzz");
        else if (i % 3 == 0)
            result.Add("Fizz");
        else if (i % 5 == 0)
            result.Add("Buzz");
        else
            result.Add(i.ToString());
    }
    return Results.Ok(new { count, result });
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
    List<int> fib = new List<int>();
    int a = 0, b = 1;
    for (int i = 0; i < count; i++)
    {
        fib.Add(a);
        int temp = a + b;
        a = b;
        b = temp;
    }
    return Results.Ok(new { count = count, sequence = fib });
});

app.MapGet("/numbers/factors/{number}", (int num) =>
{
    var factors = new List<int>();
    for (int i = 1; i <= num; i++)
    {
        if (num % i == 0)
            factors.Add(i);
    }
    return Results.Ok(new { num, factors });
});

//challenge 4: Date and Time Utilities
app.MapGet("/date/today", () =>
{
    var today = DateTime.Today;
    return Results.Ok(new
    {
        ISO = today.ToString("yyyy-MM-dd")

    });
});

app.MapGet("/date/age/{birthYear}", (int birthYear) =>
{
    var currentYear = DateTime.Today.Year;
    var age = currentYear - birthYear;
    return Results.Ok(new { birthYear, age });
});

app.MapGet("/date/daysbetween/{date1}/{date2}", (DateTime date1, DateTime date2) =>
{
    var days = Math.Abs((date2 - date1).Days);
    return Results.Ok(new { daysBetween = days });
});

app.MapGet("/date/weekday/{date}", (DateTime date) =>
{
    var weekDay = date.DayOfWeek.ToString();
    return Results.Ok(new { weekDay });
});

//Challenge 5: Simple Collections
List<string> colors = new() { "Red", "Blue", "Green", "Yellow", "Purple", "Orange" };

app.MapGet("/colors", () =>
{
    return Results.Ok(new { colors });
});

app.MapGet("/colors/random", () =>
{
    var rand = new Random();
    var color = colors[rand.Next(colors.Count)];
    return Results.Ok(new { color });
});

app.MapGet("/colors/search/{letter}", (char letter) =>
{
    var match = colors.Where(c => c.StartsWith(letter.ToString(), StringComparison.OrdinalIgnoreCase)).ToList();
    return Results.Ok(new { match });
});

app.MapPost("/colors/add/{color}", (string color) =>
{
    colors.Add(color);
    return Results.Ok(new { colors });
});

//Challenge 6: Temperature Converter
app.MapGet("/temp/celsius-to-fahrenheit/{temp}", (double temp) =>
{
    var fahrenheit = (temp * 9 / 5) + 32;
    return Results.Ok(new { fahrenheit });
});

app.MapGet("/temp/fahrenheit-to-celsius/{temp}", (double temp) =>
{
    double celsius = (temp - 32) * 5 / 9;
    return Results.Ok(new { celsius });
});

app.MapGet("/temp/kelvin-to-celsius/{temp}", (double temp) =>
{
    double result = temp - 273.15;
    return Results.Ok(new { result });
});

app.MapGet("/temp/compare/{temp1}/{unit1}/{temp2}/{unit2}", (double temp1, string unit1, double temp2, string unit2) =>
{
    //helper function to convert any unit to celsius
    double ToCelsius(double temp, string unit)
    {
        return unit.ToLower() switch
        {
            "c" => temp,
            "f" => (temp - 32) * 5 / 9,
            "k" => temp - 273.15,
        };
    }

    double celsius1 = ToCelsius(temp1, unit1);
    double celsius2 = ToCelsius(temp2, unit2);
    string compare = celsius1 == celsius2 ? "equal" : (celsius1 > celsius2 ? "greater" : "less");
    return Results.Ok(new
    {
        temp1 = $"{temp1} {unit1}",
        temp2 = $"{temp2} {unit2}",
        compare = $"Temparature 1 is {compare} than Temparature 2"
    });
});

//Challenge 7: Password Generator
app.MapGet("/password/simple/{length}", (int length) =>
{
    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    Random rand = new Random();
    string password = "";

    for (int i = 0; i < length; i++)
    {
        password += chars[rand.Next(chars.Length)];
    }
    return Results.Ok(new { password });    
});

app.MapGet("/password/complex/{length}", (int length) =>
{
    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()";
    Random rand = new Random();
    string password = "";

    for (int i = 0; i < length; i++)
    {
        password += chars[rand.Next(chars.Length)];
    }
    return Results.Ok(new { password });
});

app.MapGet("/password/memorable/{words}", (int words) =>
{
    List<string> wordList = new List<string>
    {
        "blue", "hoop", "tree", "sun", "moon", "help", "code"
    };

    Random rand = new Random();
    string password = "";
    for (int i = 0; i < words; i++)
    {
        password += wordList[rand.Next(wordList.Count)];
    }
    return Results.Ok(new { password });
});

app.MapGet("/password/strength/{password}", (string password) =>
{
    if (password.Length < 8) return Results.Ok(new { strength = "weak" });
    bool hasUpper = password.Any(char.IsUpper);
    bool hasLower = password.Any(char.IsLower);
    bool hasDigit = password.Any(char.IsDigit);
    bool hasSpecial = password.Any(char.IsSymbol);
    if (hasUpper && hasLower && hasDigit && hasSpecial)
        return Results.Ok(new { strength = "strong" });
    return Results.Ok(new { strength = "weak" });
});

//Challenge 8: Simple Validator
app.MapGet("/validate/email/{email}", (string email) =>
{
    string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
    Regex regex = new Regex(pattern);
    return regex.IsMatch(email);
});

app.MapGet("/validate/phone/{phone}", (string phone) =>
{
    string pattern = @"^\+?[1-9]\d{1,14}$";
    Regex regex = new Regex(pattern);
    return regex.IsMatch(phone);
});

app.MapGet("/validate/creditcard/{number}", (string num) =>
{
    string pattern = @"^(?:4[0-9]{12}(?:[0-9]{3})?|5[1-5][0-9]{14}|3[47][0-9]{13}|3(?:0[0-5]|[68][0-9])[0-9]{11}|6(?:011|5[0-9]{2})[0-9]{12}|(?:2131|1800|35\d{3})\d{11})$";
    Regex regex = new Regex(pattern);
    return regex.IsMatch(num);
});

app.MapGet("/validate/strongpassword/{password}", (string password) =>
{
    string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
    Regex regex = new Regex(pattern);
    return regex.IsMatch(password);
});

//Challenge 9: Unit Converter

app.Run();