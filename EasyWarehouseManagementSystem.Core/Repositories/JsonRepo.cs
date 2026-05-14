using System.Text.Json;
using EasyWarehouseManagementSystem.Core.Interfaces;

namespace EasyWarehouseManagementSystem.Core.Repositories;

public class JsonRepo<T> : IGenericRepo<T> where T : IHasId
{
    private string _filePath;
    private int _id = 0;

    public JsonRepo(string path)
    {
        _filePath = path;
    }

    public T? Get(int id)
    {
        return GetAll().FirstOrDefault(p => p.Id == id);
    }

    public IEnumerable<T> GetAll()
    {
        List<T> items = new List<T>();
        string json = "";
        try
        {
            //Read file and add products to list
            if (File.Exists(_filePath))
            {
                json = File.ReadAllText(_filePath);
            }
            else
            {
                File.Create(_filePath).Close();
            }

            items = JsonSerializer.Deserialize<List<T>>(json);
        }
        catch (FileNotFoundException e)
        {
            Console.WriteLine(e.Message);
        }
        catch (DirectoryNotFoundException e)
        {
            Console.WriteLine(e.Message);
        }
        catch (IOException e)
        {
            Console.WriteLine(e.Message);
        }
        catch (JsonException e)
        {
            Console.WriteLine(e.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return items;
    }

    public void Add(T item)
    {
        List<T> items = GetAll().ToList();
        _id = items.Max(p => p.Id) + 1;
        item.Id = _id;
        items.Add(item);
        Save(items);
    }

    public void Update(T item)
    {
        List<T> items = GetAll().ToList();
        items[(items.IndexOf(items.Find(p => p.Id == item.Id)))] = item;
        Save(items);
    }

    public void Delete(int id)
    {
        List<T> items = GetAll().ToList();
        items.Remove(items.Find(p => p.Id == id));
        Save(items);
    }

    public void Save(List<T> items)
    {
        //Removes .json and adds .tmp
        string tempFilepath = $"{_filePath[..^5]}.tmp";

        try
        {
            string json = JsonSerializer.Serialize(items, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(tempFilepath, json);
            //Atomic Write Pattern
            //Only renames the .tmp file to .json if the saving succeeded
            File.Move(tempFilepath, _filePath, overwrite: true);
        }
        catch (IOException e)
        {
            Console.WriteLine(e.Message);
            File.Delete(tempFilepath);
        }
        catch (JsonException e)
        {
            Console.WriteLine(e.Message);
            File.Delete(tempFilepath);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            File.Delete(tempFilepath);
        }
    }

    public IEnumerable<T> Search(string term)
    {
        //Needs functionality after SearchEngine
        return new List<T>();
    }
}