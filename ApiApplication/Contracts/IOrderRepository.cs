using ApiApplication.Models;
using ApiApplication.RequestFeatures;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiApplication.Contracts
{
    public interface IOrderRepository
    {
        void CreateOrder(Order order);
        void DeleteOrder(Order order);
        Task<PagedList<Order>> GetAllOrdersAsync(OrderParameters orderParameters, bool trackChanges);
        void UpdateOrder(Order order);
        Task<Order> GetOrderByIdAsync(Guid id, bool trackChanges);
    }
}
