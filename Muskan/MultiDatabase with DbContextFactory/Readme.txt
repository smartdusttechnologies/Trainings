# ASP.NET MVC & Web API Project

This project demonstrates how to use both **ASP.NET MVC** and **ASP.NET Web API** within the same project. The **MVC** is handled in the `UserController` for user-related operations, and the **Web API** is managed in the `EmployeeController` for employee-related functionality. The project uses two different databases: one for the user (`Application2DbContext` using SQL Server) and another for the employee (`ApplicationDbContext` using MySQL).

## Project Structure

- **Controllers**:
  - **UserController**: Handles MVC actions for user-related views and pages.
  - **EmployeeController**: Provides Web API endpoints for employee-related operations.
  - **HomeController**: Handles general actions and views for the application.

- **Views**:
  - MVC views are located under the **Views/User** directory:
    - **Add**: View for adding a new user.
    - **Edit**: View for editing an existing user.
    - **Index**: Displays a list of users.

- **Models**:
  - Contains the data models used in both MVC and Web API.
  - **Entities**:
    - **Employee**
    - **User**

- **ViewModels**:
  - **AddViewModel**: Represents the view model for adding an employee.
  - **AddViewModel2**: Represents the view model for adding a user.

- **Data**:
  - **ApplicationDbContext**: For managing employee data (MySQL).
  - **Application2DbContext**: For managing user data (SQL Server).

## Prerequisites

Make sure you have the following installed:

- [.NET SDK](https://dotnet.microsoft.com/download) version 6.0 or higher.
- [Visual Studio](https://visualstudio.microsoft.com/) with ASP.NET and Web Development workload enabled.

## Getting Started

1. Clone the repository:

    ```bash
    git clone https://github.com/smartdusttechnologies/Trainings.git
    ```

2. Navigate to the project folder:

    ```bash
    cd MultiDatabase
    ```

3. Restore the project dependencies:

    ```bash
    dotnet restore

4. Update Connection Strings:
   Edit appsettings.json:
 "ConnectionStrings": {
    "EmployeePortal": "Server=localhost;port=your_port;Database=your_db;username=your_username;password=your_password;",
    "SqlServerUserPortal": "Server=your_server;Database=your_db;User Id=your_username;Password=your_password;"
  },  
  Restore the  dependencies
  dotnet restore

5. Run the migration for both the databases:
   - For the user database (SQL Server):
     ```bash
     Add-Migration Initial -context Application2DbContext
     update-database -context Application2DbContext
     ```
   - For the employee database (MySQL):
     ```bash
     Add-Migration Initial -context ApplicationDbContext
     update-database -context ApplicationDbContext
     ```

5. Run the project:

    ```bash
    dotnet run
    ```

6. Open your browser and go to `http://localhost:7122` to view the MVC user interface, or access the Web API at `http://localhost:7122/api/employee`.

## MVC - UserController

The `UserController` handles the user-related views and actions. It is responsible for rendering the views and responding to requests with appropriate HTML.

- **Example endpoint:**
  - `GET /User/Index`: Displays a list of users.

- **Actions:**
  - `Add`: Adds user information.
  - `Index`: Displays the main user page.
  - `Edit`: Allows editing of an existing user.

## Web API - EmployeeController

The `EmployeeController` exposes Web API endpoints for managing employee data. It responds with JSON data for various operations like retrieving, adding, and updating employees.

- **Example endpoints:**
  - `GET /api/employee`: Retrieves a list of all employees.
  - `GET /api/employee/{id}`: Retrieves a specific employee by ID.
  - `POST /api/employee`: Creates a new employee.
  - `PUT /api/employee/{id}`: Updates an existing employee.
  - `DELETE /api/employee/{id}`: Deletes an employee.

## Example Request to Web API

To retrieve a list of employees:

```bash
curl -X GET http://localhost:7122/api/employee

The frontend of the project is built using React, located in the /MultiDatabase-Frontend/react-app directory.

Steps for Setting Up the React App:
Navigate to the React app directory:
 
 cd MultiDatabase-Frontend/react-app

Install the required Node packages:

npm install

Start the React development server:

 npm start




