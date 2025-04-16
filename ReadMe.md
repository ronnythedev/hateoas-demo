# HateoasDemo

A demonstration project showing how to implement HATEOAS (Hypermedia as the Engine of Application State) in an ASP.NET Core Minimal API application.

It’s a companion to [this blog post](https://ronnydelgado.com), where I dive deeper.

## Project Overview

This project implements a simple book management API using ASP.NET Core Minimal APIs with HATEOAS principles. It demonstrates how to create a RESTful API that includes hypermedia links in responses, allowing clients to navigate the API dynamically without hard-coding URLs.

## Features

- RESTful API built with ASP.NET Core Minimal APIs
- HATEOAS implementation for resource discovery
- In-memory book repository
- HTTP client testing file

## API Endpoints

| HTTP Method | Endpoint | Description | Name |
|-------------|----------|-------------|------|
| GET | `/api/books` | Get all books | GetBooks |
| GET | `/api/books/{id}` | Get a specific book by ID | GetBook |
| POST | `/api/books/{id}/borrow` | Borrow a book | BorrowBook |
| PUT | `/api/books/{id}` | Update a book | UpdateBook |
| DELETE | `/api/books/{id}` | Delete a book | DeleteBook |

## HATEOAS Implementation

The API returns book resources with hypermedia links that indicate what actions can be performed on each resource. For example, a book response might include links to:

- Self (the book itself)
- Collection (all books)
- Borrow action (if the book is available)
- Return action (if the book is borrowed)
- Update and delete actions

Example response:
```json
{
  "id": 1,
  "title": "The Great Gatsby",
  "author": "F. Scott Fitzgerald",
  "isAvailable": true,
  "links": [
    {
      "rel": "self",
      "href": "http://localhost:5050/api/books/1",
      "method": "GET"
    },
    {
      "rel": "borrow",
      "href": "http://localhost:5050/api/books/1/borrow",
      "method": "POST"
    },
    {
      "rel": "update",
      "href": "http://localhost:5050/api/books/1",
      "method": "PUT"
    },
    {
      "rel": "delete",
      "href": "http://localhost:5050/api/books/1",
      "method": "DELETE"
    }
  ]
}
```

## Testing the API

You can test the API using the included `HateoasDemo.http` file, which contains request templates for all endpoints. This file works with the HTTP Client in JetBrains Rider/Visual Studio Code.

To use it:
1. Start the application
2. Open the `HateoasDemo.http` file
3. Send requests to test the various endpoints

## Project Structure

- `Program.cs` - Contains all API endpoint definitions using minimal API syntax
- `Models/Book.cs` - Book entity model
- `Models/BookHateoasExtensions.cs` - Extension methods for creating HATEOAS links
- `HateoasDemo.http` - HTTP client testing file

## Running the Project

1. Clone the repository
2. Open the solution in your preferred IDE (Visual Studio, VS Code with C# extensions, or JetBrains Rider)
3. Start the application
4. The API will be available at `http://localhost:5000`

## Technologies Used

- ASP.NET Core
- .NET 9.0
- C# 13.0

## Benefits of HATEOAS

- Self-documenting API
- Loosely coupled clients and servers
- Clients can navigate the API without hard-coding URLs
- API evolution without breaking clients

This project demonstrates how to implement these principles in a modern ASP.NET Core application using minimal APIs.