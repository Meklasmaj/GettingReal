using EasyWarehouseManagementSystem.Core.Interfaces;
using EasyWarehouseManagementSystem.Core.Models;

namespace EasyWarehouseManagementSystem.Core.Repositories;

public class InMemoryRepo<T> : IGenericRepo<T> where T : IHasId
{
    private List<T> _items = new List<T>();
    public bool _isReady { get; } = false;

    public T? Get(int id)
    {
        return _items.Find(p => p.Id == id);
    }

    public IEnumerable<T> GetAll()
    {
        return new List<T>(_items);
    }

    public void Add(T item)
    {
        _items.Add(item);
    }

    public void Update(T item)
    {
        _items[(_items.IndexOf(_items.Find(p => p.Id == item.Id)))] = item;
    }

    public void Delete(int id)
    {
        _items.Remove(_items.Find(p => p.Id == id));
    }

    public IEnumerable<T> Search(string term)
    {
        //Needs functionality after SearchEngine
        return new List<T>();
    }
}