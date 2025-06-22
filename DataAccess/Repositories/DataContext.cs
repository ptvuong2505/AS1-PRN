using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    internal class DataContext
    {
        private static DataContext _instance;
        private static readonly object _lock = new object();

        public List<Customer> Customers { get; set; }
        public List<Room> Rooms { get; set; }
        public List<RoomType> RoomTypes { get; set; }
        public List<Booking> Bookings { get; set; }

        // Singleton Pattern
        public static DataContext Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new DataContext();
                        _instance.InitializeData();
                    }
                    return _instance;
                }
            }
        }

        private DataContext()
        {
            Customers = new List<Customer>();
            Rooms = new List<Room>();
            RoomTypes = new List<RoomType>();
            Bookings = new List<Booking>();
        }

        private void InitializeData()
        {
            // Seed initial data
            RoomTypes.AddRange(new List<RoomType>
            {
                new RoomType { RoomTypeID = 1, RoomTypeName = "Standard", TypeDescription = "Standard Room", TypeNote = "Basic amenities" },
                new RoomType { RoomTypeID = 2, RoomTypeName = "Deluxe", TypeDescription = "Deluxe Room", TypeNote = "Premium amenities" }
            });

            Rooms.AddRange(new List<Room>
            {
                new Room { RoomID = 1, RoomNumber = "101", RoomDescription = "Standard Room", RoomMaxCapacity = 2, RoomStatus = 1, RoomPricePerDate = 50.00m, RoomTypeID = 1 },
                new Room { RoomID = 2, RoomNumber = "102", RoomDescription = "Deluxe Room", RoomMaxCapacity = 4, RoomStatus = 1, RoomPricePerDate = 100.00m, RoomTypeID = 2 }
            });

            Customers.AddRange(new List<Customer>
            {
                new Customer { CustomerID = 1, CustomerFullName = "Admin User", Telephone = "1234567890", EmailAddress = "admin@gmail.com", CustomerBirthday = null, CustomerStatus = 1, Password = "123" },
                new Customer { CustomerID = 2, CustomerFullName = "John Doe", Telephone = "0987654321", EmailAddress = "john.doe@example.com", CustomerBirthday = new DateTime(1990, 1, 1), CustomerStatus = 1, Password = "password123" },
                new Customer { CustomerID = 3, CustomerFullName = "Phan Vuong", Telephone = "0889841400", EmailAddress = "ptvuong@gmail.com", CustomerBirthday = new DateTime(2004, 05, 25), CustomerStatus = 1, Password = "1" }
            });
        }
    }
}
