using CodingTrackerProgram;
using Spectre.Console;
using System;

namespace CodingTrackerProgram;

internal class CodingSessionSeeder
{
    private readonly CodingSessionRepository repository;
    private readonly Random random = new();

    public CodingSessionSeeder(DataBase dataBase)
    {
        repository = new CodingSessionRepository(dataBase);
    }

    public void Seed(int numberOfSessions)
    {
        for (int i = 0; i < numberOfSessions; i++)
        {
            CodingSession session = GenerateRandomSession();
            repository.Insert(session);
        }
    }

    private CodingSession GenerateRandomSession()
    {
        DateTime startRange = DateTime.Today.AddYears(-3);
        int range = (DateTime.Today - startRange).Days;

        DateTime date = startRange.AddDays(random.Next(0, range));

        TimeSpan start = TimeSpan.FromMinutes(random.Next(6 * 60, 22 * 60));

        TimeSpan duration = TimeSpan.FromMinutes(random.Next(60,241));

        TimeSpan end = start + duration;

        return new CodingSession
        {
            Date = date,
            StartTime = start.ToString(@"hh\:mm\:ss"),
            EndTime = end.ToString(@"hh\:mm\:ss"),
            Duration = duration.ToString(@"hh\:mm\:ss")
        };
    }
}

