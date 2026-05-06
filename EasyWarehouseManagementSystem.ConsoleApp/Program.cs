using System.Text;

namespace EasyWarehouseManagementSystem.ConsoleApp;

class Program
{
    static void Main(string[] args)
    {
        // To show ÆØÅ in the console
        Console.OutputEncoding = Encoding.UTF8;
        
        Menu menu = new MainMenu();
        while(true)
        {
            menu.ShowMenu();
        }
    }
}