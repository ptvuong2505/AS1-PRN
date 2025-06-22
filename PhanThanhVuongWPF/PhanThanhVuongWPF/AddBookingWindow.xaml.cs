using BusinessLogic;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PhanThanhVuongWPF
{
    /// <summary>
    /// Interaction logic for AddBookingWindow.xaml
    /// </summary>
    public partial class AddBookingWindow : Window
    {
        public Booking Booking { get; private set; }
        private readonly int _customerId;
        private readonly RoomService _roomService;
        private List<Room> _availableRooms;
        private Room _selectedRoom;

        public AddBookingWindow(int customerId)
        {
            InitializeComponent();

            _customerId = customerId;
            _roomService = new RoomService();
            Booking = new Booking { CustomerID = customerId };

            // Initialize dates to today and tomorrow
            StartDate.SelectedDate = DateTime.Today;
            EndDate.SelectedDate = DateTime.Today.AddDays(1);

            // Load available rooms
            LoadRooms();
        }

        private void LoadRooms()
        {
            _availableRooms = _roomService.GetAllRooms().ToList();

            var roomItems = _availableRooms.Select(r => new RoomDisplayItem
            {
                RoomID = r.RoomID,
                Display = $"Room {r.RoomNumber} - {r.RoomPricePerDate:C} per night",
                Room = r
            }).ToList();

            RoomSelect.ItemsSource = roomItems;

            // Select first room by default if there are any rooms
            if (roomItems.Any())
            {
                RoomSelect.SelectedIndex = 0;
            }
        }

        private void UpdatePriceEstimate()
        {
            if (_selectedRoom != null && StartDate.SelectedDate.HasValue && EndDate.SelectedDate.HasValue)
            {
                int days = (EndDate.SelectedDate.Value - StartDate.SelectedDate.Value).Days;

                if (days > 0)
                {
                    decimal totalPrice = _selectedRoom.RoomPricePerDate * days;
                    PriceEstimate.Text = $"{totalPrice:C} ({days} {(days == 1 ? "night" : "nights")} at {_selectedRoom.RoomPricePerDate:C}/night)";
                }
                else
                {
                    PriceEstimate.Text = "Please select a valid date range";
                }
            }
            else
            {
                PriceEstimate.Text = "";
            }
        }

        private void RoomSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RoomSelect.SelectedItem is RoomDisplayItem selectedItem)
            {
                _selectedRoom = selectedItem.Room;
                RoomDetails.Text = $"Room Number: {_selectedRoom.RoomNumber}\r\nType: {GetRoomTypeDescription(_selectedRoom.RoomTypeID)}\r\nDescription: {_selectedRoom.RoomDescription}\r\nMax Capacity: {_selectedRoom.RoomMaxCapacity}";
                UpdatePriceEstimate();

                RoomSelectError.Visibility = Visibility.Collapsed;
            }
        }

        private string GetRoomTypeDescription(int roomTypeId)
        {
            switch (roomTypeId)
            {
                case 1: return "Standard";
                case 2: return "Deluxe";
                case 3: return "Suite";
                default: return "Unknown";
            }
        }

        private void StartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ValidateStartDate();
            UpdatePriceEstimate();
        }

        private void EndDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ValidateEndDate();
            UpdatePriceEstimate();
        }

        private bool ValidateStartDate()
        {
            if (!StartDate.SelectedDate.HasValue)
            {
                StartDateError.Text = "Please select a start date";
                StartDateError.Visibility = Visibility.Visible;
                return false;
            }

            if (StartDate.SelectedDate < DateTime.Today)
            {
                StartDateError.Text = "Start date cannot be in the past";
                StartDateError.Visibility = Visibility.Visible;
                return false;
            }

            StartDateError.Visibility = Visibility.Collapsed;
            return true;
        }

        private bool ValidateEndDate()
        {
            if (!EndDate.SelectedDate.HasValue)
            {
                EndDateError.Text = "Please select an end date";
                EndDateError.Visibility = Visibility.Visible;
                return false;
            }

            if (StartDate.SelectedDate.HasValue && EndDate.SelectedDate <= StartDate.SelectedDate)
            {
                EndDateError.Text = "End date must be after start date";
                EndDateError.Visibility = Visibility.Visible;
                return false;
            }

            EndDateError.Visibility = Visibility.Collapsed;
            return true;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = true;

            // Validate room selection
            if (_selectedRoom == null)
            {
                RoomSelectError.Text = "Please select a room";
                RoomSelectError.Visibility = Visibility.Visible;
                isValid = false;
            }
            else
            {
                RoomSelectError.Visibility = Visibility.Collapsed;
            }

            // Validate dates
            if (!ValidateStartDate())
                isValid = false;

            if (!ValidateEndDate())
                isValid = false;

            if (!isValid)
                return;

            try
            {
                Booking.RoomID = _selectedRoom.RoomID;
                Booking.StartDate = StartDate.SelectedDate.Value;
                Booking.EndDate = EndDate.SelectedDate.Value;

                // The TotalPrice will be calculated by the BookingService based on room price and duration

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating booking: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        // Helper class for room display in the ComboBox
        private class RoomDisplayItem
        {
            public int RoomID { get; set; }
            public string Display { get; set; }
            public Room Room { get; set; }
        }
    }
}
