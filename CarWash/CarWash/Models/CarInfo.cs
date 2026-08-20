using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWash.Models
{
    internal class CarInfo
    {
        public string Make { get; init; } = string.Empty;
        public string Model { get; init; } = string.Empty;
        public string RegistrationNumber { get; init; } = string.Empty;
        public DateTime LastUsed { get; set; } = DateTime.Now.AddSeconds(-30);
        public bool ToNotify { get; set; } = false;
        public Guid UserId { get; init; }
    }
}
