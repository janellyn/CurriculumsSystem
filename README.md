# Curriculums System
Automated Information System "Curriculums" using ASP.NET MVC, C# and SQL Server.

## Description

The Automated Information System "Curriculums" is developed on the ASP.NET MVC platform, utilizing the C# programming language and Microsoft SQL Server database. Its primary goal is to enhance the efficiency of monitoring academic plans by automating the storage and provision of information about curriculums. The application also includes a login/logout window with username and password authentication and different site page for student/administrator with specific access buttons.

## Key Features

The **"Curriculums System"** application offers the following functionalities:

1. **Authentication**
   - Users can log in to the system using their unique username and password.
   - Different access levels are defined based on the roles - student or administrator.

2. **Data Management**
   - Storage of data about faculties, educational programs, and specialities of educational institutions.
   - Dynamic adding, editing, deleting and supplementation of information based on user needs.
   - Filtering and searching information based on various parameters.

3. **Different Site Pages for Each Role**
   - Аdministrator can edit the data, and also has his own site page with a list of users, their logins and roles.
   - Student has a website page of his personal account - he can add study plans there, view their details and delete them.

4. **User-Friendly Interface**
   - The application provides an intuitive and easy-to-use interface.
     
## Technology Stack

- IDE: Visual Studio 2023.
- Programming Language: C#.
- Database Management System: Microsoft SQL Server.
- Technologies: ASP.NET MVC, ADO.NET.

## Requirements

To run the MVC application, ensure you have the following prerequisites:

- Installed .NET SDK and runtime.
- Installed SQL Server or access to an existing SQL Server to create and manage the database.

## Installation and Setup

1. Clone the repository from GitHub:

git clone https://github.com/janellyn/CurriculumsSystem.git

2. Open the project in Visual Studio or any other supported C# development environment.

3. Create a database in SQL Server and execute the script from the `database.sql` file to create the required tables and relationships.

4. Update the connection string in the appsettings.json file within your MVC project:

```{
  "ConnectionStrings": {
    "DefaultConnection": "Server=your-server;Database=your-database;Integrated Security=True;"
  },
  // other settings...
}
```
Modify the Server, Database, and any other necessary parameters based on your SQL Server configuration.

5. Build and run the application. Ensure that the database is properly configured, and the application connects to it successfully.

## Conclusion

Congratulations! Now you have installed and set up the "Curriculums System" application. You can start managing academic plans by automating the storage and provision of information about curriculums.

If you have any questions or encounter any issues, please create an Issue on GitHub, and I will do our best to assist you in resolving them.

Enjoy using the application!
