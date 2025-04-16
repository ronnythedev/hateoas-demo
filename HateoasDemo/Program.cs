using Microsoft.AspNetCore.Mvc.Infrastructure;
using HateoasDemo.Models;

var builder = WebApplication.CreateBuilder(args);

// Register services for IUrlHelper
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
builder.Services.AddControllers(); // Needed for IUrlHelperFactory

var app = builder.Build();

app.UseHttpsRedirection();

// In-memory book store
var books = new List<Book>
{
    new Book { Id = 1, Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", IsAvailable = true },
    new Book { Id = 2, Title = "1984", Author = "George Orwell", IsAvailable = false }
};

// Endpoints
app.MapGet("/api/books/{id}", (HttpContext context, int id) =>
{
    var book = books.FirstOrDefault(b => b.Id == id);
    return book != null ? Results.Ok(book.ToDto(context)) : Results.NotFound();
    
}).WithName("GetBook");

app.MapGet("/api/books", (HttpContext context) =>
{
    var bookDtos = books.Select(b => b.ToDto(context)).ToList();
    return Results.Ok(bookDtos);
    
}).WithName("GetBooks");

app.MapPost("/api/books/{id}/borrow", (HttpContext context, int id) =>
{
    var book = books.FirstOrDefault(b => b.Id == id);

    if (book == null)
    {
        return Results.NotFound();
    }

    if (!book.IsAvailable)
    {
        return Results.BadRequest("Book is not available.");
    }

    book.IsAvailable = false;
    
    return Results.Ok(book.ToDto(context));
    
}).WithName("BorrowBook");

app.MapPut("/api/books/{id}", (HttpContext context, int id, Book book) =>
{
    var bookToUpdate = books.FirstOrDefault(b => b.Id == id);
    
    if (bookToUpdate == null)
    {
        return Results.NotFound();
    }
    
    bookToUpdate.Title = book.Title;
    bookToUpdate.Author = book.Author;
    bookToUpdate.IsAvailable = book.IsAvailable;
    
    return Results.Ok(bookToUpdate.ToDto(context));
    
}).WithName("UpdateBook");

app.MapDelete("/api/books/{id}", (HttpContext context, int id) =>
{
    var bookToDelete = books.FirstOrDefault(b => b.Id == id);
    
    if (bookToDelete == null)
    {
        return Results.NotFound();
    }
    
    books.Remove(bookToDelete);
    
    return Results.Ok();
    
}).WithName("DeleteBook");


app.MapPost("/api/books/{id}/return", (HttpContext context, int id) =>
{
    var bookToReturn = books.FirstOrDefault(b => b.Id == id);
    
    if (bookToReturn == null)
    {
        return Results.NotFound();
    }
    
    bookToReturn.IsAvailable = true;
    
    return Results.Ok(bookToReturn.ToDto(context));
    
}).WithName("ReturnBook");

app.Run();