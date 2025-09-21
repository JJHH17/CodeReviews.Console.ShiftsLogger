# Shift Logger Application
- A command line based shift logger application, built using ASP.Net and Entity Framework

## Project Details:
- This is a command line based application, built using ASP.Net and Entity Framework.
- The main component of the project is a web api, which allows the user to Get, Put (edit), Post (Create) and Delete Shifts.
- Shifts contain an Employee ID, Employee Name, Clock in time, Clock out time, and Department.
- The project integrates with Microsoft SQL Server for its database use, specifically, a Local DB instance - A tutorial on how to create a local db instance can be found on: https://www.youtube.com/watch?v=M5DhHYQlnq8
- The default connection string can be found in the appSettings.Json file, and by default, points to a local db instance named "ShiftLoggerDb".
- The API for this project points to port 5058 and has a base endpoint of: http://localhost:5068/api/shifts.
- The "Frontend" / Command Line user experience has been built using Spectre Console.

## Technologies Used:
- .Net / C#
- ASP.Net
- Entity Framework
- Spectre Console
- Postman (API Testing)
- Swagger

## Packages Installed and Required
- Microsoft.EntityFrameworkCore.SqlServer
- Microsoft.EntityFrameworkCore.Tools
- Microsoft.EntityFrameworkCore.Design
- Spectre.Console
- Swashbuckle.AspNetCore
- Swagger

## How to Run and use: 
- Firstly, you'll need to boot the API by clicking on the .sln file inside of the project (if running in Visual Studio): <img width="136" height="20" alt="image" src="https://github.com/user-attachments/assets/e3aadf25-8288-4547-8cd8-7ab7743fdbba" />
- Ensure that you have a local db instance that matches what we have in the appSettings.Json file (it should be an SQL Server local db instance named "ShiftLoggerDb".
- Spin up a migration, which can be done via the following commands:
  
```dotnet tool install dotnet-ef --global```

```dotnet ef migrations add InitialCreate -o Data/Migrations```

```dotnet ef database update```
- Next, run the Program.cs file - it concurrently runs both the API and the console project.
You'll be presented with a menu of options:
<img width="213" height="145" alt="image" src="https://github.com/user-attachments/assets/de7acd3b-19d3-4618-8854-5051306d2e37" />

Creating a new Shift will prompt for:
- A user name
- A shift clock in time
- A shift clock out time
- A department of the employee

Choosing to edit, view all or delete a shift will present a list of all shifts that exist in the database: <img width="514" height="65" alt="image" src="https://github.com/user-attachments/assets/ad412c86-bf88-4cce-813b-e362ea378832" />

### Endpoint list:
List of endpoints for the API is as follows:
- GET  http://localhost:5609/api/shift/ - Fetches a list of all Shifts (Id, Name, Clock In, Clock Out, Department)
- GET  http://localhost:5609/api/shift/{Id} - Fetches a list of a single Shift (if it exists)
- POST  http://localhost:5609/api/shift/ - Allows user to enter a new Shift
- PUT  http://localhost:5609/api/shift/{Id} - Allows user to edit an existing Shift and change one or more of the instance variables
- DELETE  http://localhost:5609/api/shift/{Id} - Deletes a given Shift

## File and Structure overview:
- UserInterface - Used for terminal based interaction and API consumption.
- Model/Shift - Shift Class, responsible for creating objects and calculating duration of shifts.
- Data/Migrations and ShiftsDbContect - Responsible for creation of ShiftDb instance, as well as migrations (for EntityFramework integration).
- Controllers/ShiftsController - Responsible for endpoints of API.
- Services/ShiftService - Responsible for shift class actions.
- Program.cs - Runs API and console application.

## Notes and Learnings from Project:
In this project, I:
- Used ASP.Net and EntityFramework to create my own API, as well as Spectre.Console to consume the API via a terminal based application.
- I also used Postman and Swagger to test the API once created.
I have experience with Postman and Swagger, although it was a great learning experience to use ASP.Net and EF to build the application, I look forward to creating more of these types of applications.

In the future, I'll be looking to:
- Have a cleaner integration between the frontend and backend.
- Manage concurrent calls (API and frontend at the same time) which I feel could be handled more efficiently.
- Gain more experience with EF and ASP.Net to create these types of applications moving forwards.

- I'd like to thank the C# Academy for the opportunity to build this project, and I look forward to tackling the next application!
