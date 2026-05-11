using System;
using System.Collections.Generic;
using System.Text;

namespace EasyWarehouseManagementSystem.Core.Interfaces;

// Generic repo interface
public interface IGenericRepo<T>
{
    T? Get(int id);
    IEnumerable<T> GetAll();
    void Add(T item);
    void Update(T item);
    void Delete(int id);
    IEnumerable<T> Search(string term);
}
