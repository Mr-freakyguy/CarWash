using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarWash.Services
{
    internal class NotificationService
    {
        public static void ShowMessage(string message)
        {
            Console.WriteLine("\n====================");
            Console.WriteLine($"Notification: {message}");
            Console.WriteLine("====================");
            ConsoleKeyInfo _ = Console.ReadKey();
        }
    }
}
