using System;
using Spectre.Console;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;

namespace ShiftsLogger.jjhh17.UserInterface
{
    public class UserInterface 
    {
        enum MenuOptions
        {
            CreateShift,
            ViewAllShifts,
            Exit,
        }

        public static void ShowMenu()
        {
            bool running = true;

            while (running)
            {
                AnsiConsole.MarkupLine("[bold yellow]Welcome to the shift logger![/]");
                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<MenuOptions>()
                        .Title("Please select an option:")
                        .AddChoices(Enum.GetValues<MenuOptions>()));

                switch (choice)
                {
                    case MenuOptions.CreateShift:
                        Console.Clear(); 
                        Console.WriteLine("Feature coming soon...");
                        Console.ReadKey();
                        break;

                    case MenuOptions.ViewAllShifts:
                        Console.Clear(); 
                        PrintAllShifts();
                        Console.ReadKey();
                        break;

                    case MenuOptions.Exit:
                        running = false;
                        AnsiConsole.MarkupLine("[bold blue] Thank you for using the Shift Logger! [/]");
                        Console.WriteLine("Press Ctrl + C to exit...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        public async static void PrintAllShifts()
        {
            Console.Clear();
            AnsiConsole.MarkupLine("[bold blue]Printing all shifts...[/]");

            using HttpClient client = new HttpClient();

            var table = new Table();
            table.AddColumn("Id");
            table.AddColumn("Name");
            table.AddColumn("Clock In");
            table.AddColumn("Clock Out");
            table.AddColumn("Department");
            table.AddColumn("Duration");

            try
            {
                HttpResponseMessage response = await client.GetAsync("http://localhost:5068/api/shifts");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                foreach (var shift in System.Text.Json.JsonDocument.Parse(responseBody).RootElement.EnumerateArray())
                {
                    table.AddRow(
                        shift.GetProperty("id").ToString(),
                        shift.GetProperty("name").ToString(),
                        shift.GetProperty("clockIn").ToString(),
                        shift.GetProperty("clockOut").ToString(),
                        shift.GetProperty("department").ToString(),
                        shift.GetProperty("duration").ToString()
                    );
                }
                AnsiConsole.Write(table);

            }
            catch (HttpRequestException e)
            {
                AnsiConsole.MarkupLine($"[red]Request error: {e.Message}[/]");
            }

            Console.WriteLine("Press any key to continue...");
        }
    }
}
