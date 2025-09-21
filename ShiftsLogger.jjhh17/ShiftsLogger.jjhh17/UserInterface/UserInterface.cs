using System;
using System.Text;
using Spectre.Console;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;
using ShiftsLogger.jjhh17.Model;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using ShiftsLogger.jjhh17.Services;
using Microsoft.EntityFrameworkCore;
using ShiftsLogger.jjhh17.Data;

namespace ShiftsLogger.jjhh17.UserInterface
{
    public class UserInterface
    {
        enum MenuOptions
        {
            CreateShift,
            ViewAllShifts,
            ViewShift,
            DeleteShift,
            EditShift,
            Exit,
        }

        public static async Task ShowMenu()
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

                    case MenuOptions.ViewShift:
                        Console.Clear();
                        PrintShift();
                        Console.ReadKey();
                        break;

                    case MenuOptions.DeleteShift:
                        Console.Clear();
                        DeleteShift();
                        Console.ReadKey();
                        break;

                    case MenuOptions.EditShift:
                        Console.Clear();
                        EditShift();
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

            using HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5068/");

            var newShift = EnterShift();

            HttpResponseMessage response = await client.PostAsJsonAsync("api/shifts", newShift);

            if (response.IsSuccessStatusCode)
            {
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
            ViewAllShifts();
            Console.WriteLine("Press any key to continue...");
        }

        public async static void PrintShift()
        {
            Console.Clear();
            AnsiConsole.MarkupLine("[bold blue]Print a given shift[/]");
            int inputId;

            while (true)
            {
                Console.WriteLine("Enter the ID of the shift you wish to view...");
                string stringInput = Console.ReadLine();
                if (int.TryParse(stringInput, out inputId))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid entry... Please try again");
                }
            }

            using HttpClient client = new HttpClient();

            var table = new Table();
            table.AddColumn("Name");
            table.AddColumn("Clock In");
            table.AddColumn("Clock Out");
            table.AddColumn("Department");
            table.AddColumn("Duration");

            try
            {
                HttpResponseMessage response = await client.GetAsync($"http://localhost:5068/api/shifts/{inputId}");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                Shift shift = JsonSerializer.Deserialize<Shift>(responseBody, options);

                if (shift != null)
                {
                    table.AddRow(
                        shift.Name,
                        shift.ClockIn.ToString(),
                        shift.ClockOut.ToString(),
                        shift.Department,
                        $"{shift.Duration} hrs");

                    AnsiConsole.Write(table);
                } else
                {
                    AnsiConsole.MarkupLine("[red]Failed to get shift data[/]");
                }

            }
            catch (HttpRequestException e)
            {
                AnsiConsole.MarkupLine($"[red]Request error: {e.Message}[/]");
            }

            Console.WriteLine("Enter any key to continue...");
        }

        public static async Task DeleteShift()
        {
            Console.Clear();
            ViewAllShifts();
            AnsiConsole.MarkupLine("[bold blue]Delete a shift[/]");
            int inputId;

            while (true)
            {
                Console.WriteLine("Enter the ID of the shift you wish to view...");
                string stringInput = Console.ReadLine();
                if (int.TryParse(stringInput, out inputId))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid entry... Please try again");
                }
            }

            HttpClient client = new HttpClient();
            try
            {
                HttpResponseMessage response = await client.DeleteAsync($"http://localhost:5068/api/shifts/{inputId}");

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Shift ID: {inputId} deleted successfully");
                } else
                {
                    Console.WriteLine($"Shift with ID {inputId} not found");
                }

            } catch (HttpRequestException e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
        }

        public static async Task EditShift()
        {
            Console.Clear();
            AnsiConsole.MarkupLine("[bold blue]Edit a shift[/]");
            ViewAllShifts();

            int inputId;
            while (true)
            {
                Console.WriteLine("Enter the ID of the shift you wish to edit...");
                string stringInput = Console.ReadLine();
                if (int.TryParse(stringInput, out inputId))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid entry... Please try again...");
                }
            }

            using HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5068/");

            HttpResponseMessage getResponse = await client.GetAsync($"api/shifts/{inputId}");
            if (!getResponse.IsSuccessStatusCode)
            {
                AnsiConsole.MarkupLine($"[red]Shift ID {inputId} not found[/]");
                return;
            }

            var existingShiftJson = await getResponse.Content.ReadAsStringAsync();
            var existingShift = JsonSerializer.Deserialize<Shift>(existingShiftJson, new JsonSerializerOptions());

            if (existingShift == null)
            {
                AnsiConsole.MarkupLine("[red]Failed to parse shift data[/]");
                return;
            }

            Console.WriteLine($"Editing Shift ID: {inputId}");

            Console.WriteLine("Enter a new employee name (or leave blank to keep current):");
            string name = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(name))
            {
                existingShift.Name = name;
            }

            TimeSpan clockIn;
            while (true)
            {
                Console.WriteLine($"Clock in time (current: {existingShift.ClockIn}) (format: HH:mm:ss or leave blank to keep current):");
                string input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    clockIn = existingShift.ClockIn;
                    break;
                }
                if (TimeSpan.TryParse(input, out clockIn))
                {
                    existingShift.ClockIn = clockIn;
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid time format. Try again...");
                }
            }

            TimeSpan clockOut;
            while (true)
            {
                Console.WriteLine($"Clock out time (current: {existingShift.ClockOut}) (format: HH:mm:ss or leave blank to keep current):");
                string input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    clockOut = existingShift.ClockOut;
                    break;
                }
                if (TimeSpan.TryParse(input, out clockOut) && clockOut > clockIn)
                {
                    existingShift.ClockOut = clockOut;
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid time or clock-out is before clock-in. Try again...");
                }
            }

            Console.WriteLine("Enter a new department (or leave blank to keep current):");
            string department = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(department))
            {
                existingShift.Department = department;
            }

            var updatedShiftJson = JsonSerializer.Serialize(existingShift);
            var content = new StringContent(updatedShiftJson, Encoding.UTF8, "application/json");

            HttpResponseMessage putResponse = await client.PutAsync($"api/shifts/{inputId}", content);
            if (putResponse.IsSuccessStatusCode)
            {
                Console.WriteLine("Shift updated successfully.");
            }
            else
            {
                Console.WriteLine("Failed to update shift. Status Code: " + putResponse.StatusCode);
                string errorContent = await putResponse.Content.ReadAsStringAsync();
                Console.WriteLine($"Error details: {errorContent}");
            }
        }

        public async static void ViewAllShifts()
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
        }

        public static Shift EnterShift()
        {
            Console.Clear();
            Console.WriteLine("Enter a name:");
            string name = Console.ReadLine();
            TimeSpan clockIn;
            while (true)
            {
                Console.WriteLine("Clock in time... (e.g. HH:mm:ss");
                string input = Console.ReadLine();

                if (TimeSpan.TryParse(input, out clockIn))
                {
                    break;
                }
                else
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

                if (TimeSpan.TryParse(input, out clockOut) && clockOut > clockIn)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid time format detected or clock out time is before clock in time. Enter any key to try again");
                    Console.ReadKey();
                }
            }

            Console.WriteLine("Enter a department");
            string department = Console.ReadLine();
            var newShift = new Shift
            {
                Name = name,
                ClockIn = clockIn,
                ClockOut = clockOut,
                Department = department
            };
            return newShift;
        }
    }
}

