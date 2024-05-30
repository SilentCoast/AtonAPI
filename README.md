**AtonAPI**
AtonAPI is a web API for managing users with various functionalities available to administrators and regular users. The API supports creating, updating, reading, and deleting users, as well as user authentication via JWT tokens.

**Usage**
*Prerequisites*
MS SQL Server
.NET 8
Setup
Clone the repository:
```bash
Copy code
git clone https://github.com/yourusername/AtonAPI.git
cd AtonAPI
Import the Database:

Locate the SQL script in the repository and execute it to create the necessary database and tables in your MS SQL Server.
Configure Connection String:

Open appsettings.json and adjust the connection string to match your MS SQL Server configuration.
```json
Copy code
"ConnectionStrings": {
    "DefaultConnection": "Server=your_server;Database=your_database;User Id=your_user;Password=your_password;"
}

Run the API

*Authentication*
Login to Get Token:

Call the login endpoint with credentials admin / admin to get a JWT token.
Example:
```json
POST /login
{
    "login": "admin",
    "password": "admin"
}
Copy the token from the response.
Authorize in Swagger UI:

Click the "Authorize" button in Swagger UI.
Enter the token in the format: Bearer <token>.

**Notes**
When a user changes their own login, the JWT token becomes invalid. A new token must be requested using the new login.
