-- SETUP:
    -- Create a database server (docker)
        -- docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Passw0rd123" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest
    -- Connect to the server (Azure Data Studio / Database extension)
    -- Test your connection with a simple query (like a select)
    -- Execute the Chinook database (to create Chinook resources in your db)

    
USE MyDatabase;
-- On the Chinook DB, practice writing queries with the following exercises

-- BASIC CHALLENGES
-- List all customers (full name, customer id, and country) who are not in the USA
Select FirstName, CustomerId, Country From Customer Where Country != 'USA';
-- List all customers from Brazil
SELECT * FROM Customer WHERE Country = 'Brazil';
-- List all sales agents
SELECT EmployeeId, FirstName, LastName, Title FROM Employee WHERE Title = 'Sales Support Agent';
-- Retrieve a list of all countries in billing addresses on invoices
SELECT Distinct BillingCountry FROM Invoice ORDER BY BillingCountry;
-- Retrieve how many invoices there were in 2009, and what was the sales total for that year?
SELECT COUNT(*) AS InvoiceCount, SUM(Total) AS SalesTotal
FROM Invoice WHERE YEAR(InvoiceDate) = 2009;
    -- (challenge: find the invoice count sales total for every year using one query)
SELECT YEAR(InvoiceDate) AS InvoiceYear, COUNT(*), SUM(Total) FROM Invoice GROUP BY YEAR(InvoiceDate) ORDER BY InvoiceYear;
-- how many line items were there for invoice #37
SELECT COUNT(*) FROM InvoiceLine WHERE InvoiceId = 37;
-- how many invoices per country? BillingCountry  # of invoices -
SELECT BillingCountry, COUNT(*) AS InvoiceCount FROM Invoice GROUP BY BillingCountry;
-- Retrieve the total sales per country, ordered by the highest total sales first.
SELECT BillingCountry, SUM(Total) AS TotalSales FROM Invoice GROUP BY BillingCountry; 

-- JOINS CHALLENGES
-- Every Album by Artist
SELECT Artist.Name, Album.Title FROM Album JOIN Artist ON Album.ArtistId = Artist.ArtistId
ORDER BY Artist.Name, Album.Title;
-- All songs of the rock genre
SELECT Track.Name, Genre.Name FROM Track JOIN Genre ON Track.GenreId = Genre.GenreId
WHERE Genre.Name = 'Rock' ORDER BY Track.Name;
-- Show all invoices of customers from brazil (mailing address not billing)
SELECT 
    Invoice.InvoiceId,
    Invoice.InvoiceDate,
    Invoice.Total,
    Customer.FirstName + ' ' + Customer.LastName AS CustomerName,
    Customer.Country AS MailingCountry
FROM Invoice
JOIN Customer ON Invoice.CustomerId = Customer.CustomerId
WHERE Customer.Country = 'Brazil';

-- Show all invoices together with the name of the sales agent for each one
SELECT 
    Invoice.InvoiceId,
    Invoice.InvoiceDate,
    Invoice.Total,
    Customer.FirstName + ' ' + Customer.LastName AS CustomerName,
    Employee.FirstName + ' ' + Employee.LastName AS SalesAgent
FROM Invoice
JOIN Customer ON Invoice.CustomerId = Customer.CustomerId
JOIN Employee ON Customer.SupportRepId = Employee.EmployeeId
ORDER BY Invoice.InvoiceDate;
-- Which sales agent made the most sales in 2009?
SELECT TOP 1 Employee.FirstName + ' ' + Employee.LastName AS SalesAgent,
SUM(Invoice.Total) AS TotalSales FROM Invoice
JOIN Customer ON Invoice.CustomerId = Customer.CustomerId
JOIN Employee ON Customer.SupportRepId = Employee.EmployeeId
WHERE YEAR(Invoice.InvoiceDate) = 2009
GROUP BY Employee.FirstName, Employee.LastName ORDER BY TotalSales;
-- How many customers are assigned to each sales agent?
SELECT Employee.FirstName + ' ' + Employee.LastName AS SalesAgent,
COUNT(Customer.CustomerId) AS CustomerCount
FROM Employee
JOIN Customer ON Employee.EmployeeId = Customer.CustomerId
GROUP BY Employee.FirstName, Employee.LastName
ORDER BY CustomerCount;
-- Which track was purchased the most ing 20010?
SELECT TOP 1 Track.Name AS TrackName, COUNT(InvoiceLine.InvoiceLineId) AS PurchaseCount
FROM InvoiceLine
JOIN Invoice ON InvoiceLine.InvoiceId = Invoice.InvoiceId
JOIN Track ON InvoiceLine.TrackId = Track.TrackId
WHERE YEAR(Invoice.InvoiceDate) = 2010
GROUP BY Track.NAME ORDER BY PurchaseCount;
-- Show the top three best selling artists.
SELECT TOP 3
    Artist.Name AS ArtistName,
    SUM(InvoiceLine.Quantity) AS TotalSold
FROM InvoiceLine
JOIN Track ON InvoiceLine.TrackId = Track.TrackId
JOIN Album ON Track.AlbumId = Album.AlbumId
JOIN Artist ON Album.ArtistId = Artist.ArtistId
GROUP BY Artist.Name
ORDER BY TotalSold DESC;
-- Which customers have the same initials as at least one other customer?
SELECT 
    Customer.FirstName,
    Customer.LastName,
    Customer.CustomerId
FROM Customer
JOIN (
    SELECT LEFT(FirstName, 1) AS FirstInitial, 
           LEFT(LastName, 1) AS LastInitial
    FROM Customer
    GROUP BY LEFT(FirstName, 1), LEFT(LastName, 1)
    HAVING COUNT(*) > 1
) DuplicateInitials
ON LEFT(Customer.FirstName, 1) = DuplicateInitials.FirstInitial
AND LEFT(Customer.LastName, 1) = DuplicateInitials.LastInitial
ORDER BY Customer.LastName, Customer.FirstName;


-- ADVACED CHALLENGES
-- solve these with a mixture of joins, subqueries, CTE, and set operators.
-- solve at least one of them in two different ways, and see if the execution
-- plan for them is the same, or different.

-- 1. which artists did not make any albums at all?
SELECT Artist.Name FROM Artist WHERE ArtistId NOT IN (SELECT ArtistId FROM Album); 
-- 2. which artists did not record any tracks of the Latin genre?

-- 3. which video track has the longest length? (use media type table)

-- 4. find the names of the customers who live in the same city as the
--    boss employee (the one who reports to nobody)

-- 5. how many audio tracks were bought by German customers, and what was
--    the total price paid for them?

-- 6. list the names and countries of the customers supported by an employee
--    who was hired younger than 35.


-- DML exercises

-- 1. insert two new records into the employee table.
INSERT INTO Employee (EmployeeId, LastName, FirstName, Title, ReportsTo, BirthDate, HireDate, Address, City, State, Country, PostalCode, Phone, Fax, Email)
VALUES
(10, 'Smith', 'John', 'Sales Support Agent', 1, '1985-04-15', '2025-01-10', '123 Main St', 'Chicago', 'IL', 'USA', '60601', '555-1234', NULL, 'john.smith@example.com'),
(11, 'Brown', 'Emily', 'Sales Support Agent', 1, '1990-08-22', '2025-01-15', '456 Oak Ave', 'New York', 'NY', 'USA', '10001', '555-5678', NULL, 'emily.brown@example.com');
-- 2. insert two new records into the tracks table.
INSERT INTO Track (TrackId, Name, AlbumId, MediaTypeId, GenreId, Composer, Milliseconds, Bytes, UnitPrice)
VALUES
(5000, 'New Song One', 1, 1, 1, 'John Composer', 250000, 5000000, 0.99),
(5001, 'New Song Two', 1, 1, 1, 'Emily Composer', 300000, 6000000, 1.29);
-- 3. update customer Aaron Mitchell's name to Robert Walter
UPDATE Customer SET FirstName = 'Robert', LastName = 'Walter'
WHERE FirstName = 'Aaron' AND LastName = 'Mitchell';
-- 4. delete one of the employees you inserted.
DELETE FROM Employee WHERE EmployeeId = 11;
-- 5. delete customer Robert Walter.
DELETE FROM Customer WHERE FirstName = 'Robert' AND LastName = 'Walter';
