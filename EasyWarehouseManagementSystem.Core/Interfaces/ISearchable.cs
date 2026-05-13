using System;
using System.Collections.Generic;
using System.Text;

namespace EasyWarehouseManagementSystem.Core.Interfaces;
// Interface that models will use to make them searchable
public interface ISearchable
{
    string GetSearchableText();
}
