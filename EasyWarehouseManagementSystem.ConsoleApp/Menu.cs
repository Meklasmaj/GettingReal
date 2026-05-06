using System;
using System.Collections.Generic;
using System.Text;

namespace LagersystemHansenSeest;

public abstract class Menu
{
    public abstract void ShowMenu();

    // Arrow-key based menu system
    protected int ShowInteractiveMenu(string[] options)
    {
        Console.WriteLine("Anvend ▲ ▼ til at vælge et menupunkt.");
        Console.CursorVisible = false;

        int menuChoice = 1;
        int count = options.Length;
        string selected = "\u001b[32m"; // Color
        string reset = "\u001b[0m";

        (int left, int top) = Console.GetCursorPosition();

        while (true)
        {
            Console.SetCursorPosition(left, top);

            for (int i = 0; i < count; i++)
            {
                bool isSelected = (menuChoice == i + 1);
                Console.WriteLine($"{(isSelected ? selected + "►" : " ")} {options[i]}{reset}");
            }

            ConsoleKeyInfo key = Console.ReadKey(true);

            switch (key.Key)
            {
                case ConsoleKey.DownArrow:
                    menuChoice = (menuChoice == 4 ? 1 : menuChoice + 1);
                    break;

                case ConsoleKey.UpArrow:
                    menuChoice = (menuChoice == 1 ? 4 : menuChoice - 1);
                    break;

                case ConsoleKey.Enter:
                    return menuChoice;
            }
        }
    }
}