# CodingTracker  
My fourth console project at C# Academy.  
A console application where we track code sessions by how long they lasted.
# Requirements  

- [x] This application has the same requirements as the previous project, except that now you'll be logging your daily coding time.  
- [x] To show the data on the console, you should use the Spectre.Console library.  
- [x] You're required to have separate classes in different files (i.e. UserInput.cs, Validation.cs, CodingController.cs)  
- [x] You should tell the user the specific format you want the date and time to be logged and not allow any other format.  
- [x] You'll need to create a configuration file called appsettings.json, which will contain your database path and connection strings (and any other configs you might need).  
- [x] You'll need to create a CodingSession class in a separate file. It will contain the properties of your coding session: Id, StartTime, EndTime, Duration. When reading from the database, you can't use an anonymous object, you have to read your table into a List of CodingSession.  
- [x] The user shouldn't input the duration of the session. It should be calculated based on the Start and End times  
- [x] The user should be able to input the start and end times manually.  
- [x] You need to use Dapper ORM for the data access instead of ADO.NET. (This requirement was included in Feb/2024)  
- [x] Follow the DRY Principle, and avoid code repetition.  
- [x] Don't forget the ReadMe explaining your thought process.

# Features  

- **SQLite database connection**
  - The program uses SQLite to create a database to store and read information.
  - When the program is started, the application generates the database and table necessary for its operation.
  - When the application is launched, the program inserts 100 test values (2023–2026).

- **Spectre Console UI**
  - The application uses a Spectre Console UI where we can navigate using the keyboard.

  <p align="center">
    <img width="251" height="131" alt="Main menu" src="https://github.com/user-attachments/assets/b70fb831-d6ea-498e-afb9-abd634503c0b" />
  </p>

  - The first menu shows two options:
    - Stopwatch
    - ConsultMenu
## Stopwatch

- The Stopwatch menu allows us to time the duration of a coding session and stop with the key ENTER, the coding session is saved when the stopwatch stops or the aplication is closes.
- The duration of the session must be greater than five minutes to save in both cases.
- **Examples:**
  
<p aling="center">
  <img width="1479" height="141" alt="imagen" src="https://github.com/user-attachments/assets/37de3cc3-355c-4bcf-94b2-3fb393e6229e" />
</p> 

<p align="center"> 
  <img width="1483" height="108" alt="imagen" src="https://github.com/user-attachments/assets/eeb9b018-734a-4dc3-9ff5-03cb8a5c304e" />
</p>

## ConsultMenu

<p align="center">
  <img width="256" height="203" alt="Consult menu" src="https://github.com/user-attachments/assets/22401651-3ccd-4691-a920-b95530aa6c0c" />
</p>

- The consult menu shows basic CRUD operations and a fifth option to perform filtered queries.

### PeriodMenu

<p align="center">
  <img width="463" height="184" alt="Period menu" src="https://github.com/user-attachments/assets/6ca18b10-a112-4270-96fe-90ae5289b83d" />
</p>

- The period menu shows five options to perform different queries:
  - Filter by day, week, month, and year
  - Results are ordered by:
    - Date (ascending)
    - Duration (ascending)

- **Example**

<p align="center">
  <img width="1887" height="301" alt="Example result" src="https://github.com/user-attachments/assets/7714cacb-0d7d-4beb-aea7-ea1dc2aa706e" />
</p>

# Chanllenges 
- One of the difficulties I encountered in completing this project was implementing the stopwatch, mainly due to two factors:
  - Display the stopwatch correctly on the live screen, spectre console made this point easy.
  - That the session will be saved correctly when the stopwatch is stopped or the application is closed abruptly.
- Add filtering of sessions by date, mainly by week, as we need to learn how Sqlite stores dates in order to filter them.
- Working with times and dates was complicated at first, but once I understood how TimeSpan and DateTime objects work, it became clearer how to implement them.

# What I have learned 
- How TimeSpan and DateTime work in C# and their methods for performing different operations.
- Add a repository to query the database.
- Use Spectre Console to display live information that is constantly updated.
- How to handle unexpected application closure without losing information.
- Add more details to the application to make it look better.
  
# Areas to improve
- Improve the way classes are organised to make them easier to understand, use folders to improve structure.
- Implement better methods, objects, or classes that help control exceptions if the application is forced to close.
- Learn more about how to write SQL queries that are better suited to what I want to do.
- How to correctly configure the database settings from a .json file and locate the database in a project folder of my choice.
  - For example, having a folder where the database is stored and that folder is in the root of the project where the .cs classes are stored. (I don't know if this is good practice.)

# Resourced Used 
- C# Separation of Concerns article: https://www.thecsharpacademy.com/article/30005/separation-of-concerns-csharp
- Specter Console Documentation: https://spectreconsole.net/
- Learn Dapper: https://www.learndapper.com/
- SQlite Web: https://www.sqlite.org/
- Microsoft documentation for DateTime: https://learn.microsoft.com/en-us/dotnet/api/system.datetime?view=net-10.0
- Microsoft documentation for TimeSpan: https://learn.microsoft.com/es-es/dotnet/api/system.timespan?view=net-8.0
- Microsoft documentation for IEnumerable https://learn.microsoft.com/es-es/dotnet/api/system.collections.ienumerable?view=net-8.0
