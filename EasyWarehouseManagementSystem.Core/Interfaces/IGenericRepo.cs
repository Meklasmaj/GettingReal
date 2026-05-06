using System;
using System.Collections.Generic;
using System.Text;

namespace EasyWarehouseManagementSystem.Core.Interfaces;

// Generic repo interface
internal interface IGenericRepo<T>
{
    T? Get(string id);
    IEnumerable<T> GetAll();
    void Add(T item);
    void Update(T item);
    void Delete(string id);
    IEnumerable<T> Search(string term);
}
