using EasyWarehouseManagementSystem.Core.Interfaces;
using EasyWarehouseManagementSystem.Core.Models;

namespace EasyWarehouseManagementSystem.Core.Repositories;

public class InMemoryRepo<T> : IGenericRepo<T> where T : IHasId, ISearchable
{
    private List<T> _items = new List<T>();
    private int _id = 0;

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
        _id = _items.Max(p => p.Id) + 1;
        item.Id = _id;
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
        string[] terms = term.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return _items.Where(item => terms.All(t => item.GetSearchableText().ToLower().Contains(t)));
    }
}