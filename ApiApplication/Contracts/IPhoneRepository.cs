using ApiApplication.Models;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiApplication.Contracts
{
    public interface IPhoneRepository
    {
        Task<Phone> GetPhoneByIdAsync(Guid id, bool trackChanges);
        Task<PagedList<Phone>> GetAllPhonesAsync(PhoneParameters phoneParameters,bool trackChanges);
        void DeletePhone(Phone phone);
    }
}
