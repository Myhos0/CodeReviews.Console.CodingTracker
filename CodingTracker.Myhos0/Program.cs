using CodingTrackerProgram;
using Microsoft.Extensions.Configuration;

class Program 
{
    private static CodingTracker? codingTracker;
    private static CodingTrackerProgram.Stopwatch? stopwatch;
    public static void Main() 
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        string connectionString = configuration.GetConnectionString("DefaultConnection");

        DataBase db = new(connectionString);
        db.CreateTable();

        var repository = new CodingSessionRepository(db);

        if (!repository.HasAnySessions())
        {
            var seeder = new CodingSessionSeeder(db);
            seeder.Seed(100);
        }

        codingTracker = new CodingTracker(db);

        codingTracker.MainMenu();

        Environment.Exit(0);
    }
}