using EasyWarehouseManagementSystem.Core.Interfaces;
using EasyWarehouseManagementSystem.Core.Models;

namespace EasyWarehouseManagementSystem.ConsoleApp;

public class DraftOrderMenu : Menu
{
    private List<DraftOrder> _draftOrders;
    private IGenericRepo<Stock> _stockRepo;

    public DraftOrderMenu(IGenericRepo<DraftOrder> draftOrderRepo, IGenericRepo<Stock> stockRepo)
    {
        _draftOrders = draftOrderRepo.GetAll().ToList();
        _stockRepo = stockRepo;
    }

    public override void ShowMenu()
    {
        ShowHeader("Kladdeordrer");
        if(_draftOrders.Count > 0)
        {
            _draftOrders.Reverse();
            List<string> draftOrdersToShow = new List<string>();
            for (int i = 0; i < 5; i++)
            {
                if (_draftOrders.Count > i)
                {
                    draftOrdersToShow.Add(_draftOrders[i].ToString());
                }
            }
            
            string[] options = draftOrdersToShow.ToArray();

            int choice = ShowInteractiveMenu(options);

            switch (choice)
            {
                case -1:
                    return;
                default:
                    InspectDraftOrder(_draftOrders[choice - 1]);
                    break;
            }
        }
    }

    public void InspectDraftOrder(DraftOrder draftOrder)
    {
        Console.Clear();
        ShowHeader(draftOrder.Supplier.Name);
        List<string> orderLinesToShow = new List<string>();
        foreach (var orderLine in draftOrder.OrderLines)
        {
            orderLinesToShow.Add(orderLine.ToString());
        }
        string[] options = orderLinesToShow.ToArray();
        
        int choice = ShowInteractiveMenu(options);

        switch (choice)
        {
            case -1:
                return;
            default:
                InspectOrderLine(draftOrder.OrderLines[choice - 1], draftOrder);
                break;
        }
    }
    
    public void InspectOrderLine(OrderLine orderLine, DraftOrder draftOrder)
    {
        Console.Clear();
        ShowHeader($"{orderLine.Stock.Product.Name} x {orderLine.Amount}");
        string[] options = {"Ændrer antal", "Fjern"};
        int choice = ShowInteractiveMenu(options);

        switch (choice)
        {
            case -1:
                return;
            case 1:
                ChangeAmount(orderLine);
                return;
            case 2:
                draftOrder.OrderLines.Remove(orderLine);
                orderLine.Stock.ToggleStockActivity();
                _stockRepo.Update(orderLine.Stock);
                return;
        }
    }

    public void ChangeAmount(OrderLine orderLine)
    {
        Console.WriteLine("Hvor mange vil du bestille hjem i stedet?");
        bool parseable = false;
        int amount = 0;
        while (!parseable)
        {
            parseable = Int32.TryParse(Console.ReadLine(), out amount);
            if (!parseable)
            {
                Console.WriteLine("Skriv et tal");
            }
        }
        orderLine.EditAmount(amount);
    }
}
