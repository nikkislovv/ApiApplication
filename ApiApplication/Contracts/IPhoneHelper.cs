using ApiApplication.Models;
using ApiApplication.ModelsDTO.OrderDTO;
using System.Threading.Tasks;

namespace ApiApplication.Contracts
{
    public interface IPhoneHelper
    {
        void ClearPhonesInCollection(Order order);
        Task<bool> AddPhoneCollection(Order order, OrderToHandleDto orderDto);
    }
}
