using DataAccess.Models;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PhanThanhVuongWPF
{
    /// <summary>
    /// Interaction logic for AddCustomerWindow.xaml
    /// </summary>
    public partial class AddCustomerWindow : Window
    {
        public Customer Customer { get; private set; }
        private string _passwordValue = string.Empty;

        public AddCustomerWindow()
        {
            InitializeComponent();
        }

        private void Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            _passwordValue = Password.Password;

            // Validate password
            if (string.IsNullOrEmpty(_passwordValue))
            {
                PasswordError.Text = "Password is required.";
                PasswordError.Visibility = Visibility.Visible;
                Password.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else if (_passwordValue.Length < 6)
            {
                PasswordError.Text = "Password must be at least 6 characters.";
                PasswordError.Visibility = Visibility.Visible;
                Password.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
            {
                PasswordError.Visibility = Visibility.Collapsed;
                Password.BorderBrush = new SolidColorBrush(Colors.LightGray);
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = true;

            // Validate Full Name
            if (string.IsNullOrWhiteSpace(FullName.Text))
            {
                FullNameError.Text = "Full name is required.";
                FullNameError.Visibility = Visibility.Visible;
                FullName.BorderBrush = new SolidColorBrush(Colors.Red);
                isValid = false;
            }
            else if (FullName.Text.Length < 2 || FullName.Text.Length > 50)
            {
                FullNameError.Text = "Full name must be between 2 and 50 characters.";
                FullNameError.Visibility = Visibility.Visible;
                FullName.BorderBrush = new SolidColorBrush(Colors.Red);
                isValid = false;
            }
            else
            {
                FullNameError.Visibility = Visibility.Collapsed;
                FullName.BorderBrush = new SolidColorBrush(Colors.LightGray);
            }

            // Validate Telephone
            var phoneRegex = new Regex(@"^[0-9]{10,12}$");
            if (string.IsNullOrWhiteSpace(Telephone.Text))
            {
                TelephoneError.Text = "Telephone is required.";
                TelephoneError.Visibility = Visibility.Visible;
                Telephone.BorderBrush = new SolidColorBrush(Colors.Red);
                isValid = false;
            }
            else if (!phoneRegex.IsMatch(Telephone.Text))
            {
                TelephoneError.Text = "Please enter a valid phone number (10-12 digits).";
                TelephoneError.Visibility = Visibility.Visible;
                Telephone.BorderBrush = new SolidColorBrush(Colors.Red);
                isValid = false;
            }
            else
            {
                TelephoneError.Visibility = Visibility.Collapsed;
                Telephone.BorderBrush = new SolidColorBrush(Colors.LightGray);
            }

            // Validate Email
            var emailRegex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
            if (string.IsNullOrWhiteSpace(Email.Text))
            {
                EmailError.Text = "Email is required.";
                EmailError.Visibility = Visibility.Visible;
                Email.BorderBrush = new SolidColorBrush(Colors.Red);
                isValid = false;
            }
            else if (!emailRegex.IsMatch(Email.Text))
            {
                EmailError.Text = "Please enter a valid email address.";
                EmailError.Visibility = Visibility.Visible;
                Email.BorderBrush = new SolidColorBrush(Colors.Red);
                isValid = false;
            }
            else
            {
                EmailError.Visibility = Visibility.Collapsed;
                Email.BorderBrush = new SolidColorBrush(Colors.LightGray);
            }

            // Validate Password
            if (string.IsNullOrEmpty(_passwordValue))
            {
                PasswordError.Text = "Password is required.";
                PasswordError.Visibility = Visibility.Visible;
                Password.BorderBrush = new SolidColorBrush(Colors.Red);
                isValid = false;
            }
            else if (_passwordValue.Length < 6)
            {
                PasswordError.Text = "Password must be at least 6 characters.";
                PasswordError.Visibility = Visibility.Visible;
                Password.BorderBrush = new SolidColorBrush(Colors.Red);
                isValid = false;
            }
            else
            {
                PasswordError.Visibility = Visibility.Collapsed;
                Password.BorderBrush = new SolidColorBrush(Colors.LightGray);
            }

            // Validate Birthday
            if (Birthday.SelectedDate != null && Birthday.SelectedDate > DateTime.Now)
            {
                MessageBox.Show("Birthday cannot be in the future.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                isValid = false;
            }

            if (!isValid)
            {
                return;
            }

            try
            {
                Customer = new Customer
                {
                    CustomerFullName = FullName.Text,
                    Telephone = Telephone.Text,
                    EmailAddress = Email.Text,
                    Password = _passwordValue,
                    CustomerBirthday = Birthday.SelectedDate,
                    CustomerStatus = 1
                };
                
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding customer: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
