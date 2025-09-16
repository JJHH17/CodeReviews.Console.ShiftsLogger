using System;
using Spectre.Console;

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
                        Console.WriteLine("Feature coming soon...");
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
    }
}
