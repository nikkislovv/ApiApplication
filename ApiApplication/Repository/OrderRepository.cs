using ApiApplication.Contracts;
using ApiApplication.Models;
using ApiApplication.RequestFeatures;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiApplication.Repository
{
    public class OrderRepository: RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(RepositoryContext context):base(context)
        {
        }

        public void CreateOrder(Order order)
        {
            Create(order);
        }
        public void DeleteOrder(Order order)
        {
            Delete(order);
        }
        public async Task<Order> GetOrderByIdAsync(Guid id, bool trackChanges)
        {
            return await FindByCondition(e => e.Id.Equals(id), trackChanges).SingleOrDefaultAsync();
        }
        //пагинация...................................добавить
        public async Task<PagedList<Order>> GetAllOrdersAsync(OrderParameters orderParameters, bool trackChanges)
        {
            var _orders = await FindAll(trackChanges)
                .OrderBy(e => e.FullName)
                .Skip((orderParameters.PageNumber - 1) * orderParameters.PageSize)
                .Take(orderParameters.PageSize)
                .ToListAsync();
            var count = await FindAll(trackChanges).CountAsync();
            return new PagedList<Order>(_orders, count, orderParameters.PageNumber, orderParameters.PageSize);
        }

        public void UpdateOrder(Order order)
        {
            Update(order);
        }

    }
}
