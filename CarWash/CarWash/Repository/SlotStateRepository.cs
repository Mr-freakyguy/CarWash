using CarWash.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CarWash.Repository
{
    internal class SlotStateRepository : ISlotStateRepository
    {
        private readonly string filepath = "slotstate.json";
        public int Load()
        {
            if(!File.Exists(filepath))
            {
                return 3;
            }
            string json = File.ReadAllText(filepath);
            using JsonDocument document = JsonDocument.Parse(json);
            return document.RootElement.GetProperty("Value").GetInt32();
        }
        public void Store(int value)
        {
            string json = JsonSerializer.Serialize(new
            {
                Value = value
            });
            File.WriteAllText(filepath, json);
        }
    }
}
