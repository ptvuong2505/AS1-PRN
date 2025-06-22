using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PhanThanhVuongWPF
{
    /// <summary>
    /// Interaction logic for AddRoomWindow.xaml
    /// </summary>
    public partial class AddRoomWindow : Window
    {
        public Room Room { get; private set; }

        public AddRoomWindow()
        {
            InitializeComponent();

            // Set default room type
            RoomType.SelectedIndex = 0;
        }

        // Event handlers mentioned in the XAML but not defined in the code
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                // Clear error styling when user starts typing
                switch (textBox.Name)
                {
                    case "RoomNumber":
                        RoomNumberError.Visibility = Visibility.Collapsed;
                        RoomNumber.BorderBrush = new SolidColorBrush(Colors.LightGray);
                        break;
                    case "RoomDescription":
                        RoomDescriptionError.Visibility = Visibility.Collapsed;
                        RoomDescription.BorderBrush = new SolidColorBrush(Colors.LightGray);
                        break;
                    case "RoomMaxCapacity":
                        RoomMaxCapacityError.Visibility = Visibility.Collapsed;
                        RoomMaxCapacity.BorderBrush = new SolidColorBrush(Colors.LightGray);
                        break;
                    case "RoomPricePerDate":
                        RoomPricePerDateError.Visibility = Visibility.Collapsed;
                        RoomPricePerDate.BorderBrush = new SolidColorBrush(Colors.LightGray);
                        break;
                }
            }
        }

        private void RoomType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RoomTypeError.Visibility = Visibility.Collapsed;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            // Allow only digits
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void DecimalValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            // Allow digits and one decimal point
            Regex regex = new Regex(@"^[0-9]*(?:\.[0-9]*)?$");
            string newText = (sender as TextBox)?.Text + e.Text;
            if (newText != null)
            {
                e.Handled = !regex.IsMatch(newText);
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = true;

            // Validate Room Number
            if (string.IsNullOrWhiteSpace(RoomNumber.Text))
            {
                RoomNumberError.Text = "Room number is required.";
                RoomNumberError.Visibility = Visibility.Visible;
                RoomNumber.BorderBrush = new SolidColorBrush(Colors.Red);
                isValid = false;
            }
            else if (RoomNumber.Text.Length > 50)
            {
                RoomNumberError.Text = "Room number cannot exceed 50 characters.";
                RoomNumberError.Visibility = Visibility.Visible;
                RoomNumber.BorderBrush = new SolidColorBrush(Colors.Red);
                isValid = false;
            }
            else
            {
                RoomNumberError.Visibility = Visibility.Collapsed;
                RoomNumber.BorderBrush = new SolidColorBrush(Colors.LightGray);
            }

            // Validate Room Description (optional but with length limit)
            if (!string.IsNullOrWhiteSpace(RoomDescription.Text) && RoomDescription.Text.Length > 220)
            {
                RoomDescriptionError.Text = "Description cannot exceed 220 characters.";
                RoomDescriptionError.Visibility = Visibility.Visible;
                RoomDescription.BorderBrush = new SolidColorBrush(Colors.Red);
                isValid = false;
            }
            else
            {
                RoomDescriptionError.Visibility = Visibility.Collapsed;
                RoomDescription.BorderBrush = new SolidColorBrush(Colors.LightGray);
            }

            // Validate Max Capacity
            if (string.IsNullOrWhiteSpace(RoomMaxCapacity.Text))
            {
                RoomMaxCapacityError.Text = "Maximum capacity is required.";
                RoomMaxCapacityError.Visibility = Visibility.Visible;
                RoomMaxCapacity.BorderBrush = new SolidColorBrush(Colors.Red);
                isValid = false;
            }
            else
            {
                if (!int.TryParse(RoomMaxCapacity.Text, out int capacity))
                {
                    RoomMaxCapacityError.Text = "Please enter a valid number.";
                    RoomMaxCapacityError.Visibility = Visibility.Visible;
                    RoomMaxCapacity.BorderBrush = new SolidColorBrush(Colors.Red);
                    isValid = false;
                }
                else if (capacity < 1)
                {
                    RoomMaxCapacityError.Text = "Capacity must be at least 1.";
                    RoomMaxCapacityError.Visibility = Visibility.Visible;
                    RoomMaxCapacity.BorderBrush = new SolidColorBrush(Colors.Red);
                    isValid = false;
                }
                else
                {
                    RoomMaxCapacityError.Visibility = Visibility.Collapsed;
                    RoomMaxCapacity.BorderBrush = new SolidColorBrush(Colors.LightGray);
                }
            }

            // Validate Price Per Date
            if (string.IsNullOrWhiteSpace(RoomPricePerDate.Text))
            {
                RoomPricePerDateError.Text = "Price per day is required.";
                RoomPricePerDateError.Visibility = Visibility.Visible;
                RoomPricePerDate.BorderBrush = new SolidColorBrush(Colors.Red);
                isValid = false;
            }
            else
            {
                if (!decimal.TryParse(RoomPricePerDate.Text, out decimal price))
                {
                    RoomPricePerDateError.Text = "Please enter a valid price.";
                    RoomPricePerDateError.Visibility = Visibility.Visible;
                    RoomPricePerDate.BorderBrush = new SolidColorBrush(Colors.Red);
                    isValid = false;
                }
                else if (price < 0)
                {
                    RoomPricePerDateError.Text = "Price cannot be negative.";
                    RoomPricePerDateError.Visibility = Visibility.Visible;
                    RoomPricePerDate.BorderBrush = new SolidColorBrush(Colors.Red);
                    isValid = false;
                }
                else
                {
                    RoomPricePerDateError.Visibility = Visibility.Collapsed;
                    RoomPricePerDate.BorderBrush = new SolidColorBrush(Colors.LightGray);
                }
            }

            // Validate Room Type
            if (RoomType.SelectedItem == null)
            {
                RoomTypeError.Text = "Please select a room type.";
                RoomTypeError.Visibility = Visibility.Visible;
                isValid = false;
            }
            else
            {
                RoomTypeError.Visibility = Visibility.Collapsed;
            }

            if (!isValid)
            {
                return;
            }

            try
            {
                int roomTypeId = 1; // Default to Standard
                if (RoomType.SelectedItem is ComboBoxItem selectedItem && selectedItem.Tag != null)
                {
                    roomTypeId = int.Parse(selectedItem.Tag.ToString());
                }

                Room = new Room
                {
                    RoomNumber = RoomNumber.Text,
                    RoomDescription = RoomDescription.Text,
                    RoomMaxCapacity = int.Parse(RoomMaxCapacity.Text),
                    RoomPricePerDate = decimal.Parse(RoomPricePerDate.Text),
                    RoomTypeID = roomTypeId,
                    RoomStatus = 1 // Active
                };

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding room: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
