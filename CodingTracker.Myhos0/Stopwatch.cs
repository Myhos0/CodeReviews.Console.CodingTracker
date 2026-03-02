using CodingTrackerProgram;
using Spectre.Console;

namespace CodingTrackerProgram;

internal class Stopwatch
{
    private readonly CodingSessionRepository codingSessionRepository;
    private DateTime? startDateTime;
    private bool running;
    private bool alreadySaved;

    public Stopwatch(DataBase dataBase) 
    {
        codingSessionRepository = new CodingSessionRepository(dataBase);
        Console.CancelKeyPress += OnCancelKeyPress;
        AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
    }

    public void StartTimer()
    {
        Console.Clear();

        startDateTime = DateTime.Now;
        running = true;

        AnsiConsole.MarkupLine("[green]Coding session started...[/]");
        AnsiConsole.MarkupLine("\nPress [yellow]ENTER[/] to stop.");

        AnsiConsole.Live(CreatePanel(TimeSpan.Zero))
            .AutoClear(false)
            .Start(ctx => 
            {
                while (running) 
                {
                    var elapsed = DateTime.Now - startDateTime!.Value;

                    ctx.UpdateTarget(CreatePanel(elapsed));

                    ctx.Refresh();

                    if (Console.KeyAvailable &&
                        Console.ReadKey(true).Key == ConsoleKey.Enter)
                    {
                        alreadySaved = false;
                        StopAndSave();
                        return;
                    }

                    Thread.Sleep(1000);
                }
            });
    }

    private static Panel CreatePanel(TimeSpan elapsed)
    {
        return new Panel(
            new Markup($"[bold yellow]{elapsed:hh\\:mm\\:ss}[/]").Centered())
        {
            Border = BoxBorder.Rounded,
            Header = new PanelHeader("Stopwatch", Justify.Center),
            Padding = new Padding(20,1),    
            Expand = true,
        };
    }

    

    public void StopAndSave()
    {
        if (alreadySaved) return;
        alreadySaved = true;

        running = false;

        DateTime endTime = DateTime.Now;
        TimeSpan duration = endTime - startDateTime.Value;

        if (duration < TimeSpan.FromMinutes(5))
        {
            Console.Clear();
            AnsiConsole.MarkupLine("\n[red]Session too short. Not saved.[/]");
            Console.ReadKey();
            return;
        }

        codingSessionRepository.Insert(new CodingSession
        {
            Date = startDateTime.Value,
            StartTime = startDateTime.Value.ToString(@"hh\:mm\:ss"),
            EndTime = endTime.ToString(@"hh\:mm\:ss"),
            Duration = duration.ToString(@"hh\:mm\:ss")

        });

        Console.Clear();
        AnsiConsole.MarkupLine($"[green]Session saved:[/]");
        Console.ReadKey();

    }

    public void StopAndSaveAndExit()
    {
        if (alreadySaved) return;
        alreadySaved = true;

        running = false;

        if (startDateTime != null)
        {
            DateTime endTime = DateTime.Now;
            TimeSpan duration = endTime - startDateTime.Value;

            if (duration < TimeSpan.FromMinutes(5))
            {
                Console.Clear();
                AnsiConsole.MarkupLine("\n[red]Session too short. Not saved.[/]");
                Console.ReadKey();
                return;
            }

            codingSessionRepository.Insert(new CodingSession
            {
                Date = startDateTime.Value,
                StartTime = startDateTime.Value.ToString(@"hh\:mm\:ss"),
                EndTime = endTime.ToString(@"hh\:mm\:ss"),
                Duration = duration.ToString(@"hh\:mm\:ss")
            });
        }
    }

    private void OnCancelKeyPress(object? sender, ConsoleCancelEventArgs e)
    {
        alreadySaved = false;
        StopAndSaveAndExit();
        Console.Clear();
        Environment.Exit(0);
    }

    private void OnProcessExit(object? sender, EventArgs e)
    {
        StopAndSaveAndExit();
    }
}