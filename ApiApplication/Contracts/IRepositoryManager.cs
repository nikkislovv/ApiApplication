using ApiApplication.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IRepositoryManager
    {
        IOrderRepository Orders { get; }
        IPhoneRepository Phones { get; }
        void Save();
        Task SaveAsync();

    }
}
