using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWash.Interfaces
{
    internal interface ISlotStateRepository
    {
        int Load();
        void Store(int value);
    }
}
