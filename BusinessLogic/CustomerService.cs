using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;
using DataAccess.Repositories;

namespace BusinessLogic
{
    public class CustomerService
    {
        private readonly CustomerRepository _customerRepository;

        public CustomerService()
        {
            _customerRepository = new CustomerRepository();
        }

        public Customer Authenticate(string email, string password)
        {
            return _customerRepository.Find(c => c.EmailAddress == email && c.Password == password).FirstOrDefault();
        }

        public IEnumerable<Customer> GetAllCustomers()
        {
            return _customerRepository.GetAll();
        }

        public Customer GetCustomerById(int id)
        {
            return _customerRepository.GetById(id);
        }

        public IEnumerable<Customer> SearchCustomers(string searchTerm)
        {
            return _customerRepository.Find(c => c.CustomerFullName.Contains(searchTerm) || c.EmailAddress.Contains(searchTerm));
        }

        public void AddCustomer(Customer customer)
        {
            _customerRepository.Add(customer);
        }

        public void UpdateCustomer(Customer customer)
        {
            _customerRepository.Update(customer);
        }

        public void DeleteCustomer(int id)
        {
            _customerRepository.Delete(id);
        }
    }
}
