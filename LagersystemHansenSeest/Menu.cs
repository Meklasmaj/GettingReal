using System;
using System.Collections.Generic;
using System.Text;

namespace LagersystemHansenSeest;

public abstract class Menu
{
    public abstract void ShowMenu();

    // Handles menu choices, takes input from user internally.
    public virtual int HandleMenuChoice(int minChoice, int maxChoice)
    {
        int choice = 0;
        bool parseable = false;

        // While input can't be parsed, loop
        while (!parseable)
        {
            string input = Console.ReadLine();
            parseable = int.TryParse(input, out choice);

            // If input is not a number
            if (!parseable)
            {
                Console.WriteLine("Ugyldigt!\nIndtast venligst et tal fra listen.");
            }
            else
            {
                // If input is number but not within min/max
                if (choice < minChoice || choice > maxChoice)
                {
                    Console.WriteLine("Ugyldigt!\nIndtast venligst et tal fra listen.");
                    parseable = false;
                }
            }
        }
        return choice;
    }
}