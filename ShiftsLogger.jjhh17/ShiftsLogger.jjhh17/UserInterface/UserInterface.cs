using System;
using Spectre.Console;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;
using ShiftsLogger.jjhh17.Model;

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
                        AddNewShift();
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

        public async static void AddNewShift()
        {
            Console.Clear();
            AnsiConsole.MarkupLine("[bold blue]Add a new shift...[/]");

            // Todo - add to seperate method for validation....
            Console.WriteLine("Enter an employee name");
            string name = Console.ReadLine();
            TimeSpan clockIn;
            while (true)
            {
                Console.WriteLine("Clock in time... (e.g. HH:mm:ss");
                string input = Console.ReadLine();

                if (TimeSpan.TryParse(input, out clockIn))
                {
                    break;
                } else
                {
                    Console.WriteLine("Invalid time format detected. Enter any key to try again");
                    Console.ReadKey();
                }
            }
            TimeSpan clockOut;
            while (true)
            {
                Console.WriteLine("Clock out time... (e.g. HH:mm:ss");
                string input = Console.ReadLine();

                if (TimeSpan.TryParse(input,out clockOut) && clockOut > clockIn)
                {
                    break;
                }else
                {
                    Console.WriteLine("Invalid time format detected or clock out time is before clock in time. Enter any key to try again");
                    Console.ReadKey();
                }
            }

            Console.WriteLine("Enter a department");
            string department = Console.ReadLine();

            using HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5068/");

            var newShift = new Shift
            {
                Name = name,
                ClockIn = clockIn,
                ClockOut = clockOut,
                Department = department,
            };

            HttpResponseMessage response = await client.PostAsJsonAsync("api/shifts", newShift);

            if (response.IsSuccessStatusCode) {
                Console.WriteLine("Shift created successfully");
                Console.WriteLine("Enter any key to continue...");
            }
            else
            {
                Console.WriteLine($"Error {response.StatusCode}");
                string error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error details: {error}");
                Console.WriteLine("Enter any key to continue...");
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
