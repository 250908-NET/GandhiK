# Library API

## Overview
A minimal ASP.NET Core Web API for managing a library system.  
It uses **Entity Framework Core** with SQL Server for database operations and follows repository pattern for CRUD functionality.  

## Features
- Manage **Books, Authors, Members, and Loans**
- Create, Read, and Delete operations
- Many-to-many relationship between Books and Authors
- Loan system to track which Member borrowed which Book

## Models
- **Author** – represents an author of books  
- **Book** – represents a library book (with multiple authors)  
- **Member** – represents a library member  
- **Loan** – represents a loan record linking a Book to a Member  

## Endpoints

### Books
- `GET /books` → Get all books
- `GET /books/{id}` → Get book by ID
- `POST /books` → Create a new book
- `DELETE /books/{id}` → Delete a book

### Authors
- `GET /authors` → Get all authors
- `GET /authors/{id}` → Get author by ID
- `POST /authors` → Create a new author
- `DELETE /authors/{id}` → Delete an author

### Members
- `GET /members` → Get all members
- `GET /members/{id}` → Get member by ID
- `POST /members` → Create a new member
- `DELETE /members/{id}` → Delete a member

### Loans
- `GET /loans` → Get all loans
- `GET /loans/{id}` → Get loan by ID
- `POST /loans` → Create a new loan (requires `bookId` + `memberId`)
- `DELETE /loans/{id}` → Delete a loan

## ER Diagram
<img width="913" height="645" alt="image" src="https://github.com/user-attachments/assets/d3e7f01e-1502-47b0-9033-7fde035cde69" />

### Prerequisites
- .NET 9 SDK
- Docker (for SQL Server container) or local SQL Server instance
