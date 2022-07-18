using ApiApplication.Contracts;
using ApiApplication.Models;
using ApiApplication.ModelsDTO.OrderDTO;
using Contracts;
using Repository;
using System;
using System.Threading.Tasks;

namespace ApiApplication.PhoneService
{
    public class PhoneHelper:IPhoneHelper
    {
        IRepositoryManager _repositoryManager;
        public PhoneHelper(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public void ClearPhonesInCollection(Order order)
        {
            order.Phones.Clear();
        }
        public async Task<bool> AddPhoneCollection(Order order, OrderToHandleDto orderDto)
        {
            foreach (Guid item in orderDto.PhonesIds)
            {
                var phone = await _repositoryManager.Phones.GetPhoneByIdAsync(item, true);
                if (phone != null)
                {
                    order.Phones.Add(phone);///////////////////////
                }
            }
            return order.Phones.Count > 0;
        }
    }
}
