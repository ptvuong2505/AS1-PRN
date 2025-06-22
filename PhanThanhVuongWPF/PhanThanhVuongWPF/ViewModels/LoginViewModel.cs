using BusinessLogic;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace PhanThanhVuongWPF.ViewModels
{
    internal class LoginViewModel : ViewModelBase
    {
        private readonly CustomerService _customerService;
        private readonly IConfiguration _configuration;
        private string _email;
        private string _password;
        private string _errorMessage;

        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }

        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(); }
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            _customerService = new CustomerService();
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            _configuration = builder.Build();
            LoginCommand = new RelayCommand(ExecuteLogin);
        }
        private void ExecuteLogin(object parameter)
        {
            var customer = _customerService.Authenticate(Email, Password);
            if (customer != null)
            {
                if (Email == _configuration["AdminAccount:Email"])
                {
                    // Open Admin Window
                    var adminWindow = new AdminWindow();
                    adminWindow.Show();
                }
                else
                {
                    // Open Customer Window
                    var customerWindow = new CustomerWindow(customer);
                    customerWindow.Show();
                }
                Application.Current.Windows[0].Close(); // Close Login Window
            }
            else
            {
                ErrorMessage = "Invalid email or password.";
            }
        }


        public class RelayCommand : ICommand
        {
            private readonly Action<object> _execute;
            private readonly Func<object, bool> _canExecute;

            public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
            {
                _execute = execute;
                _canExecute = canExecute;
            }

            public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);

            public void Execute(object parameter) => _execute(parameter);

            public event EventHandler CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }
        }
    }
}
