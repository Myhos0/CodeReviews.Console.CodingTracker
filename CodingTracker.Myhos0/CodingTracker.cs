using Spectre.Console;
using CodingTrackerProgram.Enums;
using System.Globalization;

namespace CodingTrackerProgram;

internal class CodingTracker
{
    private readonly CodingSession codingSession = new();
    private readonly CodingSessionRepository codingSessionRepository;
    private readonly Stopwatch stopwatch;
    private readonly ValidateInput validateInput = new();

    public CodingTracker(DataBase dataBase)
    {
        codingSessionRepository = new CodingSessionRepository(dataBase);
        stopwatch = new Stopwatch(dataBase);
    }

    public void MainMenu() 
    {
        bool open = true;

        while (open) 
        {
            Console.Clear();

            var menuOption = AnsiConsole.Prompt(
                new SelectionPrompt<Menu>()
                .Title("Select the option")
                .AddChoices(Enum.GetValues<Menu>()));

            switch (menuOption) 
            {
                case Menu.Stopwatch:
                    MainStopwatchMenu();
                    break;
                case Menu.ConsultMenu:
                    MainConsultMenu();
                    break;
                case Menu.Exit:
                    open = false;
                    break;
            }
        }
    }

    public void MainStopwatchMenu() 
    {
        while (true)
        {
            Console.Clear();

            var menuOption = AnsiConsole.Prompt(
                new SelectionPrompt<StopwatchMenu>()
                .Title("Select the option")
                .AddChoices(Enum.GetValues<StopwatchMenu>()));

            switch (menuOption)
            {
                case StopwatchMenu.Start:
                    stopwatch.StartTimer();
                    break;
                case StopwatchMenu.Exit:
                    return;
            }
        }
    }

    public void MainConsultMenu()
    {
        while (true)
        {
            Console.Clear();

            var menuOption = AnsiConsole.Prompt(
                new SelectionPrompt<ConsultMenu>()
                .Title("Select the option")
                .AddChoices(Enum.GetValues<ConsultMenu>()));

            switch (menuOption)
            {
                case ConsultMenu.Insert:
                    InsertMenu();
                    break;
                case ConsultMenu.Update:
                    UpdateMenu();
                    break;
                case ConsultMenu.View:
                    ViewMenu();
                    break;
                case ConsultMenu.Delete:
                    DeleteMenu();
                    break;
                case ConsultMenu.PeriodMenu:
                    MainPeriodMenu();
                    break;
                case ConsultMenu.Exit:
                    return;
            }
        }
    }

    public void MainPeriodMenu()
    {
        while (true)
        {
            Console.Clear();

            var menuOption = AnsiConsole.Prompt
                (new SelectionPrompt<PeriodMenu>()
                .Title("Select the option to show the filter")
                .AddChoices(Enum.GetValues<PeriodMenu>()));

            switch (menuOption)
            {
                case PeriodMenu.Day:
                    PerDayMenu();
                    break;
                case PeriodMenu.Week:
                    PerWeekMenu();
                    break;
                case PeriodMenu.Month:
                    PerMonthMenu();
                    break;
                case PeriodMenu.Year:
                    PerYearMenu();
                    break;
                case PeriodMenu.Cancel:
                    return;
            }
        }
    }

    private void InsertMenu()
    {
        List<string> values = GetUserInput(
            "Please enter the date of the coding session [#619B8A](yyyy-MM-dd)[/], [#75C9C8]T[/] to insert today's date or [#9A031E]0[/] to cancel",
            "Please enter the start time of the coding session [#619B8A](HH:mm:ss or HH:mm 00-23)[/], [#75C9C8]C[/] to insert current time or [#9A031E]0[/] to cancel",
            "Please enter the end time of the coding session [#619B8A](HH:mm:ss or HH:mm 00-23)[/], [#75C9C8]C[/] to insert current time plus one hour or [#9A031E]0[/] to cancel"
            );

        if (values.Exists(v => v == "0")) return;

        DateTime parsedDate = DateTime.ParseExact(values[0], "yyyy-MM-dd", CultureInfo.InvariantCulture);

        CodingSession session = new()
        {
            Date = parsedDate,
            StartTime = values[1],
            EndTime = values[2],
            Duration = values[3]
        };

        codingSessionRepository.Insert(session);
    }

    private void ViewMenu()
    {
        IEnumerable<CodingSession> sessions = codingSessionRepository.GetAll();

        if (!sessions.Any())
        {
            AnsiConsole.MarkupLine("[yellow]No coding sessions found.[/]");
            Console.ReadKey();
            return;
        }

        ShowTable(sessions);
        Console.ReadKey();
    }

    private void DeleteMenu()
    {
        ViewMenu();
        int id = validateInput.ValidateId("Select the [#619B8A]Id[/] of the session you want to delete or [#9A031E]0[/] to cancel", codingSessionRepository);

        if (id == -1) return;

        codingSessionRepository.Delete(id);
        AnsiConsole.MarkupLine("Session delete successfully");
        Console.ReadLine();
    }

    private void UpdateMenu()
    {
        ViewMenu();
        int id = validateInput.ValidateId("Select the [#619B8A]Id[/] of the session you want to update or [#9A031E]0[/] to cancel", codingSessionRepository);

        if (id == -1) return;

        List<string> values = GetUserInput(
            "Please enter the new date of the coding session [#619B8A](yyyy-MM-dd)[/], [#75C9C8]T[/] to insert today's date or [#9A031E]0[/] to cancel",
            "Please enter the new start time of the coding session [#619B8A](HH:mm:ss or HH:mm 00-23)[/], [#75C9C8]C[/] to insert current time or [#9A031E]0[/] to cancel",
            "Please enter the new end time of the coding session [#619B8A](HH:mm:ss or HH:mm 00-23)[/], [#75C9C8]C[/] to insert current time plus one hour or [#9A031E]0[/] to cancel"
            );

        if (values.Exists(v => v == "0")) return;

        DateTime parsedDate = DateTime.ParseExact(values[0], "yyyy-MM-dd", CultureInfo.InvariantCulture);

        CodingSession session = new()
        {
            Id = id,
            Date = parsedDate,
            StartTime = values[1],
            EndTime = values[2],
            Duration = values[3]
        };

        codingSessionRepository.Update(session);

    }

    private List<string> GetUserInput(string dateMessage, string startTimeMessage, string endTimeMessage)
    {
        List<string> values = new();

        string minimumDuration = TimeSpan.FromHours(1).ToString();

        string date = validateInput.ValidateDate(dateMessage);
        string startTime = validateInput.ValidateTime(startTimeMessage,false);
        string endTime = validateInput.ValidateEndTime(startTime, endTimeMessage,minimumDuration);

        TimeSpan calculateDuration = TimeSpan.Parse(endTime) - TimeSpan.Parse(startTime);

        string duration = calculateDuration.ToString();

        values.Add(date);
        values.Add(startTime);
        values.Add(endTime);
        values.Add(duration);

        AnsiConsole.MarkupLine($"Date: {date}");
        AnsiConsole.MarkupLine($"Star Time: {startTime}");
        AnsiConsole.MarkupLine($"End Time: {endTime}");
        AnsiConsole.MarkupLine($"Duration: {duration}");
        Console.ReadKey();

        return values;
    }

    private void PerDayMenu()
    {
        string date = validateInput.ValidateDate("Please enter a date [#619B8A](yyyy-MM-dd)[/]");

        DateTime start = DateTime.Parse(date);
        DateTime end = start.AddDays(1);

        IEnumerable<CodingSession> sessions = codingSessionRepository.GetSessionsByDateRange(start.ToString("yyyy-MM-dd"), end.ToString("yyyy-MM-dd"));

        if (!sessions.Any())
        {
            AnsiConsole.MarkupLine("[yellow]No coding sessions found.[/]");
            Console.ReadKey();
            return;
        }

        ShowTable(sessions);
        Console.ReadKey();
    }

    private void PerWeekMenu() 
    {
        int year = validateInput.ValidateYear("Enter the year [#619B8A](yyyy)[/]");
        int month = validateInput.ValidateMonth("Enter the month [#619B8A]1-12[/]");
        int week = validateInput.ValidateWeekInMonth(GetWeeksInMonth(year,month));

        var (starWeek, endWeek) = GetWeekDateRange(year,month,week);

        IEnumerable<CodingSession> sessions = codingSessionRepository.GetSessionsByDateRange(starWeek.ToString("yyyy-MM-dd"), endWeek.AddDays(1).ToString("yyyy-MM-dd"));

        if (!sessions.Any())
        {
            AnsiConsole.MarkupLine("[yellow]No coding sessions found.[/]");
            Console.ReadKey();
            return;
        }

        ShowTable(sessions);
        Console.ReadKey();
    }

    private void PerMonthMenu() 
    {
        int year = validateInput.ValidateYear("Enter the year [#619B8A](yyyy)[/]");
        int month = validateInput.ValidateMonth("Enter the month [#619B8A]1-12[/]");

        DateTime startMonth = new(year, month, 1);
        DateTime endMonth = startMonth.AddMonths(1);

        IEnumerable<CodingSession> sessions = codingSessionRepository.GetSessionsByDateRange(startMonth.ToString("yyyy-MM-dd"),endMonth.ToString("yyyy-MM-dd"));

        if (!sessions.Any())
        {
            AnsiConsole.MarkupLine("[yellow]No coding sessions found.[/]");
            Console.ReadKey();
            return;
        }

        ShowTable(sessions);
        Console.ReadKey();
    }

    private void PerYearMenu()
    {
        int year = validateInput.ValidateYear("Enter the year [#619B8A](yyyy)[/]");

        DateTime start = new(year, 1, 1);
        DateTime end = start.AddYears(1);

        IEnumerable<CodingSession> sessions = codingSessionRepository.GetSessionsByDateRange(start.ToString("yyyy-MM-dd"), end.ToString("yyyy-MM-dd"));
        if (!sessions.Any())
        {
            AnsiConsole.MarkupLine("[yellow]No coding sessions found.[/]");
            Console.ReadKey();
            return;
        }

        ShowTable(sessions);
        Console.ReadKey();
    }

    private void ShowTable(IEnumerable<CodingSession> sessions)
    {
        AnsiConsole.Clear();

        var table = new Table().Border(TableBorder.Rounded)
           .AddColumn("Id")
           .AddColumn("Date")
           .AddColumn("Start")
           .AddColumn("End")
           .AddColumn("Duration");

        foreach (var s in sessions)
        {
            table.AddRow(
                s.Id.ToString(),
                s.Date.ToString("yyyy-MM-dd"),
                TimeSpan.Parse(s.StartTime).ToString(@"hh\:mm\:ss"),
                TimeSpan.Parse(s.EndTime).ToString(@"hh\:mm\:ss"),
                TimeSpan.Parse(s.Duration).ToString(@"hh\:mm\:ss")
            );
        }

        table.Expand();
        AnsiConsole.Write(table);
    }

    private int GetWeeksInMonth(int year, int month)
    {
        DateTime firstDay = new(year, month, 1);
        DateTime lastDay = firstDay.AddMonths(1).AddDays(-1);

        int firstWeek = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(
            firstDay,
            CalendarWeekRule.FirstFourDayWeek,
            DayOfWeek.Monday);

        int lastWeek = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(
            lastDay,
            CalendarWeekRule.FirstFourDayWeek,
            DayOfWeek.Monday);

        if (lastWeek < firstWeek)
            lastWeek += 52;

        return lastWeek - firstWeek + 1;
    }

    private (DateTime Start, DateTime End) GetWeekDateRange(int year, int month, int week)
    {
        DateTime firstDayOfMonth = new(year, month, 1);

        while (firstDayOfMonth.DayOfWeek != DayOfWeek.Monday)
            firstDayOfMonth = firstDayOfMonth.AddDays(1);

        DateTime start = firstDayOfMonth.AddDays((week - 1) * 7);
        DateTime end = start.AddDays(6);

        return (start, end);
    }
}