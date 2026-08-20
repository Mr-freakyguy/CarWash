using CarWash.Interfaces;
using CarWash.Models;
using CarWash.Repository;
using System.Timers;

namespace CarWash.Services
{
    internal class UserService
    {
        public delegate void OnNotify(string message);
        public event OnNotify? Notify;
        private static Guid _userId;
        private readonly int washtime = 20;
        private static readonly SlotStateRepository _slotStateRepository = new SlotStateRepository();
        private readonly IRepository<CarInfo> _carRepository;
        private readonly IRepository<UserInfo> _userRepository;
        Validator _validator;
        public UserService(IRepository<UserInfo> userRepository, IRepository<CarInfo> carRepository)
        {
            this._userRepository = userRepository;
            this._carRepository = carRepository;
            this._validator = new Validator(this);
        }

        public static int Slot = _slotStateRepository.Load();
        public bool Login(string email, string password)
        {
            if(!Validator.IsValidEmail(email) || !Validator.IsValidPassword(password))
            {
                return false;
            }
            if(_validator.AuthenticateUser(email, password, _userRepository.GetAll().ToList(), out Guid userId))
            {
                _userId = userId;
                CheckPendingNotifications();
                return true;
            }
            return false;
        }
        public void RegisterUser(string name, string email, string phoneNumber, string password)
        { 
            if(Validator.IsValidEmail(email) && Validator.IsValidPhoneNumber(phoneNumber) && Validator.IsValidPassword(password))
            {
                UserInfo user = new UserInfo()
                {
                    UserId = Guid.NewGuid(),
                    Name = name,
                    Email = email,
                    Phone = phoneNumber,
                    Password = password
                };
                _userRepository.Add(user);
            }
        }
        public CarInfo RegisterCar(string make, string model, string registrationNumber)
        {
            if(Validator.IsValidRegistration(registrationNumber) && !_validator.IsDuplicateCar(registrationNumber))
            {
                CarInfo car = new CarInfo()
                {
                    Make = make,
                    Model = model,
                    RegistrationNumber = registrationNumber,
                    UserId = _userId,
                };
                _carRepository.Add(car);
                return car;
            }
            return new CarInfo();
        }
        public List<CarInfo> GetUserCars()
        {
            return _carRepository.GetAll().Where(x => x.UserId == _userId).ToList();
        }
        public bool BookSlot(CarInfo car)
        {
            if (Slot <= 0 || (DateTime.Now - car.LastUsed).TotalSeconds < washtime)
            {
                return false;
            }
            _carRepository.InvokeBooking(car);
            _slotStateRepository.Store(--Slot);
            IntimateUser(car, washtime);
            return true;
        }
        public void Logout()
        {
            _userId = default;
        }
        public void IntimateUser(CarInfo car, double time)
        {
            System.Timers.Timer timer = new System.Timers.Timer(time*1000);
            timer.AutoReset = false;
            timer.Elapsed += (sender, e) =>
            {

                if (car.UserId == _userId)
                {
                    _slotStateRepository.Store(++Slot);
                    _carRepository.RevokeBooking(car);
                    Notify?.Invoke($"Your car {car.RegistrationNumber} is Washed Successfully");
                }
                timer.Dispose();
            };
            timer.Start();
        }
        public void CheckPendingNotifications()
        {
            var list = _carRepository.GetAll().Where(x => x.UserId == _userId && x.ToNotify == true).ToList();
            foreach(var car in list)
            {
                if ((DateTime.Now - car.LastUsed).TotalSeconds > washtime)
                {
                    Notify?.Invoke($"Your car {car.RegistrationNumber} is Washed Successfully");
                    _slotStateRepository.Store(++Slot);
                    _carRepository.RevokeBooking(car);
                }
                else
                {
                    double TimeRemaining = (int)washtime - (DateTime.Now - car.LastUsed).TotalSeconds;
                    IntimateUser(car, TimeRemaining);
                }
            }
        }
    }
}
