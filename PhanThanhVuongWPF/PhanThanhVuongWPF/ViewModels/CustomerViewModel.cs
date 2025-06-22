using BusinessLogic;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static PhanThanhVuongWPF.ViewModels.LoginViewModel;

namespace PhanThanhVuongWPF.ViewModels
{
    internal class CustomerViewModel : ViewModelBase
    {
        private readonly CustomerService _customerService;
        private readonly BookingService _bookingService;
        private Customer _currentCustomer;
        private ObservableCollection<BookingViewModel> _bookings;

        public Customer CurrentCustomer
        {
            get => _currentCustomer;
            set { _currentCustomer = value; OnPropertyChanged(); }
        }

        public ObservableCollection<BookingViewModel> Bookings
        {
            get => _bookings;
            set { _bookings = value; OnPropertyChanged(); }
        }

        public ICommand UpdateProfileCommand { get; }
        public ICommand AddBookingCommand { get; }
        public ICommand LogoutCommand { get; }

        public CustomerViewModel(Customer customer)
        {
            _customerService = new CustomerService();
            _bookingService = new BookingService();
            CurrentCustomer = customer;

            // Convert bookings to view models with calculated properties
            var customerBookings = _bookingService.GetBookingsByCustomer(customer.CustomerID);
            Bookings = new ObservableCollection<BookingViewModel>(
                customerBookings.Select(b => new BookingViewModel(b)));

            UpdateProfileCommand = new RelayCommand(UpdateProfile);
            AddBookingCommand = new RelayCommand(AddBooking);
            LogoutCommand = new RelayCommand(Logout);
        }

        private void UpdateProfile(object parameter)
        {
            var updateWindow = new UpdateCustomerWindow(CurrentCustomer);
            if (updateWindow.ShowDialog() == true)
            {
                _customerService.UpdateCustomer(updateWindow.Customer);
                CurrentCustomer = _customerService.GetCustomerById(CurrentCustomer.CustomerID);
            }
        }

        private void AddBooking(object parameter)
        {
            var addBookingWindow = new AddBookingWindow(CurrentCustomer.CustomerID);
            if (addBookingWindow.ShowDialog() == true)
            {
                _bookingService.AddBooking(addBookingWindow.Booking);

                // Refresh bookings after adding a new one
                var customerBookings = _bookingService.GetBookingsByCustomer(CurrentCustomer.CustomerID);
                Bookings = new ObservableCollection<BookingViewModel>(
                    customerBookings.Select(b => new BookingViewModel(b)));
            }
        }

        private void Logout(object parameter)
        {
            if (MessageBox.Show("Are you sure you want to logout?", "Confirm Logout",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                // Show login window
                var loginWindow = new LoginWindow();
                loginWindow.Show();

                // Close the current window
                if (parameter is Window window)
                {
                    window.Close();
                }
            }
        }
    }

    // Helper class to expose calculated properties for bookings
    public class BookingViewModel
    {
        private readonly Booking _booking;

        public BookingViewModel(Booking booking)
        {
            _booking = booking;
        }

        // Original properties
        public int BookingID => _booking.BookingID;
        public int CustomerID => _booking.CustomerID;
        public int RoomID => _booking.RoomID;
        public DateTime StartDate => _booking.StartDate;
        public DateTime EndDate => _booking.EndDate;
        public decimal TotalPrice => _booking.TotalPrice;
        
        // Expose Room properties directly
        public string RoomNumber => _booking.Room?.RoomNumber ?? "N/A";
        public string RoomTypeName => _booking.Room?.RoomType?.RoomTypeName ?? "N/A";
        public Room Room => _booking.Room;

        // Calculated properties
        public int Duration => (EndDate - StartDate).Days;
    }
}
