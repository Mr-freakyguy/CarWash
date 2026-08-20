using CarWash.Interfaces;
using CarWash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CarWash.Repository
{
    internal class JsonFileStore<T> : IRepository<T> where T : class, new()
    {
        private readonly string _filePath;
        private List<T> _items;

        public JsonFileStore(string filePath)
        {
            this._filePath = filePath;
            this._items = Load();
        }
        private static readonly JsonSerializerOptions options = new();
        private List<T> Load()
        {
            if(!File.Exists(this._filePath))
            {
                return new List<T>();
            }
            string json = File.ReadAllText(this._filePath);
            if (string.IsNullOrWhiteSpace(json))
            {
                return new List<T>();
            }
            return JsonSerializer.Deserialize<List<T>>(json, options) ?? new List<T>();
        }
        private void Store()
        {
            string? directory = Path.GetDirectoryName(this._filePath);
            if(!string.IsNullOrWhiteSpace(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string json = JsonSerializer.Serialize(this._items, options);
            File.WriteAllText(this._filePath, json);        
        }
        public List<T> GetAll()
        {
            return _items;
        }
        public void Add(T item)
        {
            _items.Add(item);
            Store();
        }
        public void InvokeBooking(CarInfo car)
        {
            car.LastUsed = DateTime.Now;
            car.ToNotify = true;
            Store();
        }
        public void RevokeBooking(CarInfo car)
        {
            car.ToNotify = false;
            Store();
        }
    }

}
