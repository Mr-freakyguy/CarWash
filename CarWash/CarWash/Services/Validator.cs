using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Text.RegularExpressions;
using CarWash.Models;
using CarWash.Interfaces;

namespace CarWash.Services
{
    internal class Validator
    {
        private readonly UserService _userService;
        public Validator(UserService userService)
        {
            this._userService = userService;
        }
        public static bool IsValidEmail(string email)
        {
            return MailAddress.TryCreate(email, out MailAddress? address) && address.Address == email;
        }
        public static bool IsValidPhoneNumber(string phone)
        {
            if(!long.TryParse(phone, out var phoneNumber))
            {
                return false;
            }
            return phoneNumber >= 1000000000 && phoneNumber <= 9999999999;
        }
        public static bool IsValidPassword(string password)
        {
            if(password.Length < 8)
            {
                return false;
            }
            return Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).+$");
        }
        public static bool IsValidRegistration(string registration)
        {
            return Regex.IsMatch(registration, @"^[A-Za-z]{2}\d{2}[A-Za-z]{2}\d{4}$");
        }
        public bool IsDuplicateCar(string registration)
        {
            return _userService.GetUserCars().Any(x => x.RegistrationNumber == registration);
        }
        public bool AuthenticateUser(string email, string password, List<UserInfo> list, out Guid UserId)
        {
            if(list.Count == 0)
            {
                UserId = default;
                return false;
            }
            var user = list.FirstOrDefault(x => x.Email == email);
            if (user != null && user.Password == password)
            {
                UserId = user.UserId;
                return true;
            }
            UserId = default;
            return false;
        }
    }
}
