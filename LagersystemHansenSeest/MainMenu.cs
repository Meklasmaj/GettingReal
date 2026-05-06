using System;
using System.Collections.Generic;
using System.Text;

namespace LagersystemHansenSeest;

public class MainMenu : Menu
{
    public override void ShowMenu()
    {
        Console.WriteLine("Anvend ▲ ▼ til at vælge et menupunkt.");

        ConsoleKeyInfo key;
        Console.CursorVisible = false;
        int menuChoice = 1;
        bool running = true;
        (int left, int top) = Console.GetCursorPosition();
        string selected = "\u001b[32m";
        string reset = "\u001b[0m";

        while (running)
        {
            Console.SetCursorPosition(left, top);

            Console.WriteLine($"{(menuChoice == 1 ? selected + "►" : " ")} Mulighed 1{reset}");
            Console.WriteLine($"{(menuChoice == 2 ? selected + "►" : " ")} Mulighed 2{reset}");
            Console.WriteLine($"{(menuChoice == 3 ? selected + "►" : " ")} Mulighed 3{reset}");
            Console.WriteLine($"{(menuChoice == 4 ? selected + "►" : " ")} Mulighed 4{reset}");

            key = Console.ReadKey(true);

            switch (key.Key)
            {
                case ConsoleKey.DownArrow:
                    menuChoice = (menuChoice == 4 ? 1 : menuChoice + 1);
                    break;

                case ConsoleKey.UpArrow:
                    menuChoice = (menuChoice == 1 ? 4 : menuChoice - 1);
                    break;

                case ConsoleKey.Enter:
                    running = false;
                    break;
            }
        }

    }
}
