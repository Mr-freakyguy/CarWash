using CarWash.Enums;
using CarWash.Services;
using CarWash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace CarWash.Views
{
    internal class DashboardView
    {
        private readonly UserService _userService;
        private readonly Validator _validator;
        private readonly AuthenticationView _authenticationView;

        public DashboardView(UserService userService, Validator validator, AuthenticationView authenticationView)
        {
            this._userService = userService;
            this._validator = validator;
            this._authenticationView = authenticationView;
        }
        public void Dashboard()
        {
            Console.Clear();
            if (_userService.GetUserCars().Count != 0)
            {
                Console.WriteLine("==========Car History==========");
                foreach (var car in _userService.GetUserCars())
                {
                    string status = (DateTime.Now - car.LastUsed).TotalSeconds > 3 ? "Finished" : "In Progress";
                    Console.WriteLine($"{car.RegistrationNumber} - {car.Model} - {car.LastUsed} - {status}");
                }
            }
            Console.WriteLine("1.Book a Car Wash");
            Console.WriteLine("2.Logout");
            switch ((DashboardChoice)ConsoleHelper.ReadInt("Enter Choice: ", 1, 2))
            {
                case DashboardChoice.Book:
                    Book();
                    break;
                case DashboardChoice.Logout:
                    _userService.Logout();
                    _authenticationView.MainMenu();
                    break;
            }
        }
        public void Book()
        {
            int index = 1;
            Console.Clear();
            Console.WriteLine("==========Available Cars==========");
            foreach(var car in _userService.GetUserCars())
            {
                Console.WriteLine($"{index++} - {car.RegistrationNumber} - {car.Model} - {car.LastUsed}");
            }
            Console.WriteLine($"{index++}.Add new Car");
            Console.WriteLine($"{index}.Back");
            int choice = ConsoleHelper.ReadInt("Enter your choice: ", 1, index);
            if(choice == index - 1)
            {
                BookNew();
            }
            else if(choice <= index) 
            {
                _userService.BookSlot(_userService.GetUserCars()[choice - 1]);

            }
            Dashboard();
        }
        public void BookNew()
        {
            string make;
            string model;
            string RegistrationNumber;
            Console.Clear();
            make = ConsoleHelper.ReadNonEmptyString("Enter Car Make: ");
            model = ConsoleHelper.ReadNonEmptyString("Enter Car Model: ");
            do
            {
                RegistrationNumber = ConsoleHelper.ReadNonEmptyString("Enter Registration Number (TN34MD1234): ");
                if (!Validator.IsValidRegistration(RegistrationNumber)) Console.WriteLine("Invalid registration format.");
                if (_validator.IsDuplicateCar(RegistrationNumber)) Console.WriteLine("A car with this registration already exists.");
            } while (!Validator.IsValidRegistration(RegistrationNumber) || _validator.IsDuplicateCar(RegistrationNumber));
            CarInfo car = _userService.RegisterCar(make, model, RegistrationNumber);
            _userService.BookSlot(car);
        }
    }
}
