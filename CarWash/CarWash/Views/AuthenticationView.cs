using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarWash.Enums;
using CarWash.Services;

namespace CarWash.Views
{
    internal class AuthenticationView
    {
        private readonly UserService _userService;
        public AuthenticationView(UserService userService)
        {
            this._userService = userService;
        }
        public void MainMenu()
        {
            _userService.Logout();
            Console.Clear();
            Console.WriteLine("==========Car Wash==========");
            Console.WriteLine("1.Login");
            Console.WriteLine("2.Register");
            Console.WriteLine("3.Exit");
            switch((AuthenticationChoice)ConsoleHelper.ReadInt("Enter your Choice: ", 1, 3))
            {
                case AuthenticationChoice.Login:
                    Login();
                    break;
                case AuthenticationChoice.Register:
                    Register();
                    break;
                case AuthenticationChoice.Exit:
                    return;
            }
        }
        public void Login()
        {
            Console.Clear();
            string email = ConsoleHelper.ReadNonEmptyString("Enter Email: ");
            string password = ConsoleHelper.ReadNonEmptyString("Enter Password: ");
            if (_userService.Login(email, password))
            {
                var dashboard = new DashboardView(_userService, new Validator(_userService), this);
                dashboard.Dashboard();
                return;
            }
            Console.WriteLine("Invalid Email or Password.");
            Console.WriteLine("Press any key to Continue");
            Console.ReadKey();
            MainMenu();
        }
        public void Register()
        {
            string name;
            string email;
            string phoneNumber;
            string password;
            Console.Clear();
            name = ConsoleHelper.ReadNonEmptyString("Enter Name: ");
            do
            {
                email = ConsoleHelper.ReadNonEmptyString("Enter Email: ");
                if (!Validator.IsValidEmail(email)) Console.WriteLine("Invalid email format.");
            } while (!Validator.IsValidEmail(email));
            do {
                phoneNumber = ConsoleHelper.ReadNonEmptyString("Enter Phone Number: ");
                if (!Validator.IsValidPhoneNumber(phoneNumber)) Console.WriteLine("Invalid phone number.");
            } while(!Validator.IsValidPhoneNumber(phoneNumber));
            do
            {
                password = ConsoleHelper.ReadNonEmptyString("Enter Password: ");
                if (!Validator.IsValidPassword(password)) Console.WriteLine("Password must contain upper, lower, digit and special char and be at least 8 chars long.");
            } while(!Validator.IsValidPassword(password));
            _userService.RegisterUser(name, email, phoneNumber, password);
            MainMenu();
        }
    }
}
