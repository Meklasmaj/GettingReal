using System;
using System.Collections.Generic;
using System.Text;

namespace LagersystemHansenSeest.Interfaces;

// Generic repo interface
internal interface IGenericRepo<T>
{
    T? Get(int id);
    T? Get(string name);
    IEnumerable<T> GetAll();
    void Add(T item);
    void Update(T item);
    void Delete(int id);
    void Delete(string name);
    IEnumerable<T> Search(string term);
}
