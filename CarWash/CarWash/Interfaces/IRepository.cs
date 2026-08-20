using CarWash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWash.Interfaces
{
    internal interface IRepository<T> where T : class
    {
        List<T> GetAll();
        void Add(T item);
        void InvokeBooking(CarInfo item);
        void RevokeBooking(CarInfo item);
    }
}
