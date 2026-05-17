using EasyWarehouseManagementSystem.Core.Interfaces;
using EasyWarehouseManagementSystem.Core.Models;

namespace EasyWarehouseManagementSystem.ConsoleApp;

public class DraftOrderMenu : Menu
{
    private List<DraftOrder> _draftOrders;

    public DraftOrderMenu(IGenericRepo<DraftOrder> draftOrderRepo)
    {
        _draftOrders = draftOrderRepo.GetAll().ToList();
    }

    public override void ShowMenu()
    {
        ShowHeader("Kladdeordre");
        if(_draftOrders.Count > 0)
        {
            _draftOrders.Reverse();
            List<string> draftOrdersToShow = new List<string>();
            for (int i = 0; i < 5; i++)
            {
                if (_draftOrders.Count > i)
                {
                    draftOrdersToShow.Add(_draftOrders[i].Supplier.Name);
                }
            }
            
            string[] options = draftOrdersToShow.ToArray();

            int choice = ShowInteractiveMenu(options);

            switch (choice)
            {
                case -1:
                    return;
                case 1:
                    InspectDraftOrder(_draftOrders[0]);
                    break;
                case 2:
                    InspectDraftOrder(_draftOrders[1]);
                    break;
                case 3:
                    InspectDraftOrder(_draftOrders[2]);
                    break;
                case 4:
                    InspectDraftOrder(_draftOrders[3]);
                    break;
                case 5:
                    InspectDraftOrder(_draftOrders[4]);
                    break;
            }
        }
    }

    public void InspectDraftOrder(DraftOrder draftOrder)
    {
        
    }
}