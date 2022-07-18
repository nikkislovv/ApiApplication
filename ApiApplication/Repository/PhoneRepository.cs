using ApiApplication.Contracts;
using ApiApplication.Models;
using ApiApplication.Repository.RepositoryPhoneExtensions;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiApplication.Repository
{
    public class PhoneRepository :RepositoryBase<Phone>, IPhoneRepository
    {
        public PhoneRepository(RepositoryContext context) : base(context)
        {
        }
        //по айди поиск
        public async Task<Phone> GetPhoneByIdAsync(Guid id, bool trackChanges)
        {
            return await FindByCondition(e => e.Id.Equals(id), trackChanges).SingleOrDefaultAsync();
        }

        //for
        //получить все
        public async Task<PagedList<Phone>> GetAllPhonesAsync(PhoneParameters phoneParameters, bool trackChanges)
        {
            var _phones = await FindAll(trackChanges)
                .FilterPhones(phoneParameters.MinPrice, phoneParameters.MaxPrice)//filtering
                .Search(phoneParameters.SearchTerm)//searching
                .Sort(phoneParameters.OrderBy)//sorting
                .Skip((phoneParameters.PageNumber - 1) * phoneParameters.PageSize)//paging
                .Take(phoneParameters.PageSize)
                .ToListAsync();
            var count = await FindAll(false)//for metaData(in header of response)
                .FilterPhones(phoneParameters.MinPrice, phoneParameters.MaxPrice)
                .Search(phoneParameters.SearchTerm)
                .CountAsync();
            return new PagedList<Phone>(_phones, count, phoneParameters.PageNumber,phoneParameters.PageSize);
        }
      
        public void DeletePhone(Phone phone)
        {
            Delete(phone);
        }
        
        //добавить телефон
        
     

    }
}
