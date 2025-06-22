using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class CustomerRepository : IRepository<Customer>
    {
        private readonly DataContext _context;

        public CustomerRepository()
        {
            _context = DataContext.Instance;
        }

        public IEnumerable<Customer> GetAll()
        {
            return _context.Customers.Where(c => c.CustomerStatus==1);
        }

        public void Add(Customer entity)
        {
            entity.CustomerID = _context.Customers.Any() ? _context.Customers.Max(c => c.CustomerID) + 1 : 1;
            _context.Customers.Add(entity);
        }

        public void Delete(int id)
        {
            var entity = _context.Customers.FirstOrDefault(c => c.CustomerID == id);
            if (entity != null) { 
                entity.CustomerStatus = 2;
            }
        }

        public Customer GetById(int id)
        {
            return _context.Customers.FirstOrDefault(c => c.CustomerID == id && c.CustomerStatus == 1);
        }

        public IEnumerable<Customer> Find(Expression<Func<Customer, bool>> predicate)
        {
            return _context.Customers.AsQueryable().Where(predicate).Where(c => c.CustomerStatus == 1);
        }

        public void Update(Customer entity)
        {
            var existing = _context.Customers.FirstOrDefault(c => c.CustomerID==entity.CustomerID);
            if (existing != null) {
                existing.CustomerFullName = entity.CustomerFullName;
                existing.Telephone = entity.Telephone;
                existing.EmailAddress = entity.EmailAddress;
                existing.CustomerBirthday = entity.CustomerBirthday;
                existing.CustomerStatus = entity.CustomerStatus;
                existing.Password = entity.Password;
            }
        }
    }
}
