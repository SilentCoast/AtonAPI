**AtonAPI**
---
AtonAPI is a web API for managing users with various functionalities available to administrators and regular users. The API supports creating, updating, reading, and deleting users, as well as user authentication via JWT tokens.

**Usage**
---
**Prerequisites**

MS SQL Server

.NET 8


**Setup**

Clone the repository:
```bash
git clone https://github.com/yourusername/AtonAPI.git
cd AtonAPI
```
Import the Database:

Locate the 'AtonDB.sql' SQL script in the repository and execute it to create the necessary database and tables in your MS SQL Server.


Configure Connection String:

Open appsettings.json and adjust the connection string to match your MS SQL Server configuration.
```json
"ConnectionStrings": {
    "DefaultConnection": "Server=your_server;Database=your_database;User Id=your_user;Password=your_password;"
}
```
Run the API

**Authentication**

Login to Get Token:

Call the login endpoint with credentials admin / admin to get a JWT token.

```json

POST /login
{
    "login": "admin",
    "password": "admin"
}
```
Copy the token from the response.
Authorize in Swagger UI:

Click the "Authorize" button in Swagger UI.
Enter the token in the format: Bearer 'token'.

Example of a Token:
```Token
Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJhZG1pbiIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IkFkbWluIiwianRpIjoiMjFlMTk2Y2UtMTcxNy00MDVhLWExMWUtMjIzMjBjZTBhMjFhIiwiZXhwIjoxNzE3OTI0NTE5LCJpc3MiOiJodHRwczovL3RoYXRndXkuY29tIiwiYXVkIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzAwOCJ9.qNo45gk9K68MFbEQnQ68ymyfEO_ux-Vb_KQuD2Rclhw
```

**Notes**

When a user changes their own login, the JWT token becomes invalid. A new token must be requested using the new login.
