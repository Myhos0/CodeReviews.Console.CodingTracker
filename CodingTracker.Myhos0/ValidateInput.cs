using Spectre.Console;
using System.Globalization;

namespace CodingTrackerProgram;

internal class ValidateInput
{
    internal string ValidateDate(string message)
    {
        while (true)
        {
            AnsiConsole.MarkupLine(message);
            string input = Console.ReadLine()?.Trim() ?? "";

            if (string.Equals(input, "T", StringComparison.OrdinalIgnoreCase))
            {
                return DateTime.Today.ToString("yyyy-MM-dd");
            }

            if (input == "0") return "0";

            if (DateTime.TryParseExact(input, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsed))
            {
                return parsed.ToString("yyyy-MM-dd");
            }

            message = "[#FF0A0A]Invalid date.[/] Try again:";
        }
    }

    internal string ValidateTime(string message,bool isEndTime)
    {
        string[] timeFormat = { @"hh\:mm", @"hh\:mm\:ss" };

        while (true)
        {
            AnsiConsole.MarkupLine(message);
            string input = Console.ReadLine()?.Trim() ?? "";

            if (string.Equals(input, "C", StringComparison.OrdinalIgnoreCase) && !isEndTime)
            {
                return DateTime.Now.ToString(@"hh\:mm\:ss");
            }
            
            if(string.Equals(input, "C", StringComparison.OrdinalIgnoreCase) && isEndTime)
            {
                return DateTime.Now.AddHours(1).ToString(@"hh\:mm\:ss"); ; 
            }

            if (input == "0") return "0";

            if (TimeSpan.TryParseExact(input, timeFormat, CultureInfo.InvariantCulture, out var parsed))
            {
                return parsed.ToString(@"hh\:mm\:ss");
            }

            message = "[#FF0A0A]Invalid time.[/] Try again:";
        }
    }

    internal int ValidateId(string message, CodingSessionRepository codingSessionRepository)
    {
        while (true)
        {
            AnsiConsole.MarkupLine(message);
            string id = Console.ReadLine()?.Trim() ?? "";

            if (id == "0") return -1;

            if (int.TryParse(id, out var parsed))
            {
                if (codingSessionRepository.SessionExist(parsed))
                {
                    return parsed;
                }
            }

            message = "[#FF0A0A]Invalid Id.[/] Try again:";
        }
    }

    internal string ValidateEndTime(string startTime, string endTimeMessage, string minimumDuration)
    {
        while (true)
        {
            string endTime = ValidateTime(endTimeMessage,true);

            TimeSpan duration = TimeSpan.Parse(endTime) - TimeSpan.Parse(startTime);


            if (duration >= TimeSpan.Parse(minimumDuration) && TimeSpan.Parse(endTime) >= TimeSpan.Parse(startTime))
            {
                return endTime.ToString();
            }

            endTimeMessage = "The end time cannot be the same as or earlier than the start time, please insert new one or [#75C9C8]C[/] to insert current time plus one hour.";
        }
    }

    public int ValidateWeekInMonth(int maxWeeks)
    {
        int week;

        while (true)
        {
            AnsiConsole.MarkupLine($"Enter week of the month [green](1 - {maxWeeks})[/]:");

            string input = Console.ReadLine()?.Trim() ?? "";

            if (!int.TryParse(input, out week))
            {
                AnsiConsole.MarkupLine("[red]Week must be a number.[/]");
                continue;
            }

            if (week < 1 || week > maxWeeks)
            {
                AnsiConsole.MarkupLine($"[red]Week must be between 1 and {maxWeeks}.[/]");
                continue;
            }

            return week;
        }
    }

    public int ValidateMonth(string message)
    {
        int month;

        while (true)
        {
            AnsiConsole.MarkupLine(message);
            string input = Console.ReadLine()?.Trim() ?? "";

            if (!int.TryParse(input, out month))
            {
                AnsiConsole.MarkupLine("[red]Month must be a number.[/]");
                continue;
            }

            if (month < 1 || month > 12)
            {
                AnsiConsole.MarkupLine("[red]Month must be between 1 and 12.[/]");
                continue;
            }

            return month;
        }
    }

    public int ValidateYear(string message)
    {
        int year;

        while (true)
        {
            AnsiConsole.MarkupLine(message);
            string input = Console.ReadLine()?.Trim() ?? "";

            if (!int.TryParse(input, out year))
            {
                AnsiConsole.MarkupLine("[red]Year must be a number.[/]");
                continue;
            }

            if (year < 1900 || year > DateTime.Now.Year + 1)
            {
                AnsiConsole.MarkupLine($"[red]Year must be between 1900 and {DateTime.Now.Year + 1}.[/]");
                continue;
            }

            return year;
        }
    }
}
