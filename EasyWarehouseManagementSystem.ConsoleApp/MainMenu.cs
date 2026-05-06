using System;
using System.Collections.Generic;
using System.Text;

namespace LagersystemHansenSeest;

public class MainMenu : Menu
{
    // The menu text
    private static readonly string[] Options = ["Mulighed 1", "Mulighed 2", "Mulighed 3", "Mulighed 4"];

    public override void ShowMenu()
    {
        int choice = ShowInteractiveMenu(Options);

        switch (choice)
        {
            case 1:

                break;
            case 2:

                break;
            case 3:

                break;
            case 4:

                break;
        }
    }
}
