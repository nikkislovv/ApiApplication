using ApiApplication.Contracts;
using ApiApplication.Models;
using ApiApplication.Repository;
using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        IOrderRepository _orderRepository;
        IPhoneRepository _phoneRepository;
        RepositoryContext _context;
        public RepositoryManager(RepositoryContext context)
        {
            _context = context;
        }
        public IOrderRepository Orders
        {
            get
            {
                if (_orderRepository == null)
                    _orderRepository = new OrderRepository(_context);
                return _orderRepository;
            }
        }
        public IPhoneRepository Phones
        {
            get
            {
                if (_phoneRepository == null)
                    _phoneRepository = new PhoneRepository(_context);
                return _phoneRepository;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
