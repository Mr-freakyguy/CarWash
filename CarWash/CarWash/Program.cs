using CarWash.Interfaces;
using CarWash.Models;
using CarWash.Services;
using CarWash.Repository;
using CarWash.Views;

namespace CarWash
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            IRepository<UserInfo> userInfo = new JsonFileStore<UserInfo>("UserInfo");
            IRepository<CarInfo> carInfo = new JsonFileStore<CarInfo>("CarInfo");

            var userService = new UserService(userInfo, carInfo);
            var validator = new Validator(userService);

            var authenticationView = new AuthenticationView(userService);
            var dashboardView = new DashboardView(userService, validator, authenticationView);
            userService.Notify += NotificationService.ShowMessage;
            authenticationView.MainMenu();
        }
    }
}
