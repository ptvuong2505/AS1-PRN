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
    internal class AdminViewModel : ViewModelBase
    {
        private readonly CustomerService _customerService;
        private readonly RoomService _roomService;
        private readonly BookingService _bookingService;
        private ObservableCollection<Customer> _customers;
        private ObservableCollection<Room> _rooms;
        private ObservableCollection<BookingReportItem> _bookings;
        private DateTime _startDate;
        private DateTime _endDate;
        private int _totalBookings;
        private decimal _totalRevenue;
        private string _customerSearchText;
        private string _roomSearchText;

        public ObservableCollection<Customer> Customers
        {
            get => _customers;
            set { _customers = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Room> Rooms
        {
            get => _rooms;
            set { _rooms = value; OnPropertyChanged(); }
        }

        public ObservableCollection<BookingReportItem> Bookings
        {
            get => _bookings;
            set { _bookings = value; OnPropertyChanged(); }
        }

        public DateTime StartDate
        {
            get => _startDate;
            set { _startDate = value; OnPropertyChanged(); }
        }

        public DateTime EndDate
        {
            get => _endDate;
            set { _endDate = value; OnPropertyChanged(); }
        }

        public int TotalBookings
        {
            get => _totalBookings;
            set { _totalBookings = value; OnPropertyChanged(); }
        }

        public decimal TotalRevenue
        {
            get => _totalRevenue;
            set { _totalRevenue = value; OnPropertyChanged(); }
        }

        public string CustomerSearchText
        {
            get => _customerSearchText;
            set { _customerSearchText = value; OnPropertyChanged(); }
        }

        public string RoomSearchText
        {
            get => _roomSearchText;
            set { _roomSearchText = value; OnPropertyChanged(); }
        }

        public ICommand AddCustomerCommand { get; }
        public ICommand UpdateCustomerCommand { get; }
        public ICommand DeleteCustomerCommand { get; }
        public ICommand SearchCustomersCommand { get; }
        public ICommand AddRoomCommand { get; }
        public ICommand UpdateRoomCommand { get; }
        public ICommand DeleteRoomCommand { get; }
        public ICommand SearchRoomsCommand { get; }
        public ICommand GenerateReportCommand { get; }
        public ICommand LogoutCommand { get; }
        public ICommand ExportReportCommand { get; }

        public AdminViewModel()
        {
            _customerService = new CustomerService();
            _roomService = new RoomService();
            _bookingService = new BookingService();

            Customers = new ObservableCollection<Customer>(_customerService.GetAllCustomers());
            Rooms = new ObservableCollection<Room>(_roomService.GetAllRooms());
            Bookings = new ObservableCollection<BookingReportItem>();

            // Initialize dates for report
            StartDate = DateTime.Today.AddMonths(-1); // Default to last month
            EndDate = DateTime.Today;

            // Initialize commands
            AddCustomerCommand = new RelayCommand(AddCustomer);
            UpdateCustomerCommand = new RelayCommand(UpdateCustomer);
            DeleteCustomerCommand = new RelayCommand(DeleteCustomer);
            SearchCustomersCommand = new RelayCommand(SearchCustomers);
            AddRoomCommand = new RelayCommand(AddRoom);
            UpdateRoomCommand = new RelayCommand(UpdateRoom);
            DeleteRoomCommand = new RelayCommand(DeleteRoom);
            SearchRoomsCommand = new RelayCommand(SearchRooms);
            GenerateReportCommand = new RelayCommand(GenerateReport);
            LogoutCommand = new RelayCommand(Logout);
            ExportReportCommand = new RelayCommand(ExportReport);

            // Generate initial report
            GenerateReport(null);
        }

        private void AddCustomer(object parameter)
        {
            var addWindow = new AddCustomerWindow();
            if (addWindow.ShowDialog() == true)
            {
                _customerService.AddCustomer(addWindow.Customer);
                Customers = new ObservableCollection<Customer>(_customerService.GetAllCustomers());
            }
        }

        private void UpdateCustomer(object parameter)
        {
            if (parameter is Customer customer)
            {
                var updateWindow = new UpdateCustomerWindow(customer);
                if (updateWindow.ShowDialog() == true)
                {
                    _customerService.UpdateCustomer(updateWindow.Customer);
                    Customers = new ObservableCollection<Customer>(_customerService.GetAllCustomers());
                }
            }
        }

        private void DeleteCustomer(object parameter)
        {
            if (parameter is Customer customer && MessageBox.Show("Are you sure you want to delete this customer?", "Confirm Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _customerService.DeleteCustomer(customer.CustomerID);
                Customers = new ObservableCollection<Customer>(_customerService.GetAllCustomers());
            }
        }

        private void SearchCustomers(object parameter)
        {
            if (parameter is string searchTerm)
            {
                Customers = new ObservableCollection<Customer>(_customerService.SearchCustomers(searchTerm));
            }
        }

        private void AddRoom(object parameter)
        {
            var addWindow = new AddRoomWindow();
            if (addWindow.ShowDialog() == true)
            {
                _roomService.AddRoom(addWindow.Room);
                Rooms = new ObservableCollection<Room>(_roomService.GetAllRooms());
            }
        }

        private void UpdateRoom(object parameter)
        {
            if (parameter is Room room)
            {
                var updateWindow = new UpdateRoomWindow(room);
                if (updateWindow.ShowDialog() == true)
                {
                    _roomService.UpdateRoom(updateWindow.Room);
                    Rooms = new ObservableCollection<Room>(_roomService.GetAllRooms());
                }
            }
        }

        private void DeleteRoom(object parameter)
        {
            if (parameter is Room room && MessageBox.Show("Are you sure you want to delete this room?", "Confirm Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _roomService.DeleteRoom(room.RoomID);
                Rooms = new ObservableCollection<Room>(_roomService.GetAllRooms());
            }
        }

        private void SearchRooms(object parameter)
        {
            if (parameter is string searchTerm)
            {
                Rooms = new ObservableCollection<Room>(_roomService.SearchRooms(searchTerm));
            }
        }

        private void GenerateReport(object parameter)
        {
            try
            {
                // Validate dates
                if (EndDate < StartDate)
                {
                    MessageBox.Show("End date cannot be before start date", "Invalid Date Range", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Get bookings for the selected period
                var bookings = _bookingService.GetBookingsByPeriod(StartDate, EndDate);

                // Create report items with additional calculated information
                var reportItems = new List<BookingReportItem>();
                
                foreach (var b in bookings)
                {
                    // Explicitly load customer if not loaded
                    if (b.Customer == null)
                    {
                        b.Customer = _customerService.GetCustomerById(b.CustomerID);
                    }
                    
                    reportItems.Add(new BookingReportItem
                    {
                        BookingID = b.BookingID,
                        CustomerID = b.CustomerID,
                        CustomerName = b.Customer?.CustomerFullName ?? $"Unknown (ID: {b.CustomerID})",
                        RoomID = b.RoomID,
                        RoomNumber = b.Room?.RoomNumber ?? $"Unknown (ID: {b.RoomID})",
                        StartDate = b.StartDate,
                        EndDate = b.EndDate,
                        Duration = (b.EndDate - b.StartDate).Days,
                        TotalPrice = b.TotalPrice,
                        Status = "Completed"
                    });
                }

                // Update the observable collection
                Bookings = new ObservableCollection<BookingReportItem>(reportItems);

                // Calculate summary statistics
                TotalBookings = Bookings.Count;
                TotalRevenue = Bookings.Sum(b => b.TotalPrice);

                // Show success message
                if (parameter != null) // Only show message when manually clicking button
                {
                    MessageBox.Show($"Report generated successfully. Found {TotalBookings} bookings with total revenue of {TotalRevenue:C}.",
                        "Report Generated", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating report: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExportReport(object parameter)
        {
            try
            {
                // Implement export to CSV or Excel functionality
                var saveFileDialog = new Microsoft.Win32.SaveFileDialog
                {
                    FileName = $"BookingReport_{StartDate:yyyy-MM-dd}_to_{EndDate:yyyy-MM-dd}",
                    DefaultExt = ".csv",
                    Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    using (var writer = new System.IO.StreamWriter(saveFileDialog.FileName))
                    {
                        // Write CSV header
                        writer.WriteLine("Booking ID,Customer,Room,Start Date,End Date,Duration (days),Total Price,Status");

                        // Write data rows
                        foreach (var booking in Bookings)
                        {
                            writer.WriteLine($"{booking.BookingID}," +
                                            $"\"{booking.CustomerName}\"," +
                                            $"\"{booking.RoomNumber}\"," +
                                            $"{booking.StartDate:yyyy-MM-dd}," +
                                            $"{booking.EndDate:yyyy-MM-dd}," +
                                            $"{booking.Duration}," +
                                            $"{booking.TotalPrice}," +
                                            $"{booking.Status}");
                        }
                    }

                    MessageBox.Show($"Report exported successfully to {saveFileDialog.FileName}",
                        "Export Complete", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting report: {ex.Message}", "Export Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Logout(object parameter)
        {
            if (MessageBox.Show("Are you sure you want to logout?", "Confirm Logout", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                // Show the login window
                var loginWindow = new LoginWindow();
                loginWindow.Show();

                // Close the current window
                if (parameter is Window window)
                {
                    window.Close();
                }
                else
                {
                    // Get the current window if not provided as a parameter
                    Window currentWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is AdminWindow);
                    if (currentWindow != null)
                    {
                        currentWindow.Close();
                    }
                }
            }
        }
    }

    // Class to represent booking data in the report
    public class BookingReportItem
    {
        public int BookingID { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public int RoomID { get; set; }
        public string RoomNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Duration { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }
    }
}
