using DataAccess.Models;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace PhanThanhVuongWPF
{
    /// <summary>
    /// Interaction logic for UpdateCustomerWindow.xaml
    /// </summary>
    public partial class UpdateCustomerWindow : Window
    {
        public Customer Customer { get; private set; }
        private readonly Customer _originalCustomer;
        private string _passwordValue = string.Empty;
        private bool _passwordChanged = false;

        public UpdateCustomerWindow(Customer customer)
        {
            InitializeComponent();

            _originalCustomer = customer;

            // Populate fields with customer data
            CustomerID.Text = customer.CustomerID.ToString();
            CustomerName.Text = $"Customer: {customer.CustomerFullName}";
            FullName.Text = customer.CustomerFullName;
            Email.Text = customer.EmailAddress;
            Telephone.Text = customer.Telephone;
            Birthday.SelectedDate = customer.CustomerBirthday;

            // Handle password hint visibility
            PasswordHint.Visibility = Visibility.Visible;
            Password.GotFocus += (s, e) => PasswordHint.Visibility = Visibility.Collapsed;
            Password.LostFocus += (s, e) =>
            {
                if (string.IsNullOrEmpty(Password.Password))
                {
                    PasswordHint.Visibility = Visibility.Visible;
                }
            };
        }

        private void Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            _passwordValue = Password.Password;
            
            // Only set password as changed if it's not empty
            _passwordChanged = !string.IsNullOrEmpty(_passwordValue);

            // Hide hint if password is not empty
            PasswordHint.Visibility = string.IsNullOrEmpty(_passwordValue)
                ? Visibility.Visible
                : Visibility.Collapsed;

            // Validate password
            if (!string.IsNullOrEmpty(_passwordValue) && _passwordValue.Length < 6)
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

        private void Update_Click(object sender, RoutedEventArgs e)
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

            // Validate Password - Only if provided (optional for update)
            if (_passwordChanged && !string.IsNullOrEmpty(_passwordValue) && _passwordValue.Length < 6)
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
                // Create updated customer
                Customer = new Customer
                {
                    CustomerID = _originalCustomer.CustomerID,
                    CustomerFullName = FullName.Text,
                    Telephone = Telephone.Text,
                    EmailAddress = Email.Text,
                    CustomerBirthday = Birthday.SelectedDate,
                    CustomerStatus = _originalCustomer.CustomerStatus
                };

                // Only update password if a new one is provided
                if (_passwordChanged && !string.IsNullOrEmpty(_passwordValue))
                {
                    Customer.Password = _passwordValue;
                }
                else
                {
                    Customer.Password = _originalCustomer.Password;
                }

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating customer: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }

}
