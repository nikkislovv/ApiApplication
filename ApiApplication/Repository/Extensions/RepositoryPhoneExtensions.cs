using ApiApplication.Models;
using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Linq.Dynamic.Core;//to dynamically create our OrderBy query on the fly
using ApiApplication.Repository.Extensions.Utility;

namespace ApiApplication.Repository.RepositoryPhoneExtensions
{
    public static class RepositoryPhoneExtensions
    {
        public static IQueryable<Phone> FilterPhones(this IQueryable<Phone> phones, uint minPrice, uint maxPrice) =>
        phones.Where(e => (e.Price >= minPrice && e.Price <= maxPrice));

        public static IQueryable<Phone> Search(this IQueryable<Phone> phones, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return phones;
            var lowerCaseTerm = searchTerm.Trim().ToLower();
            return phones.Where(e => e.Name.ToLower().Contains(lowerCaseTerm));
        }

        public static IQueryable<Phone> Sort(this IQueryable<Phone> phones, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))//если orderByQueryString=null то по умолч сортируем по name
                return phones.OrderBy(e => e.Name);
            var orderQuery = OrderQueryBuilder.CreateOrderQuery<Phone>(orderByQueryString);//вынесли определение сортировки в отдельный класс (для всех объектов)generic 
            if (string.IsNullOrWhiteSpace(orderQuery))//если сортировка не указана используем по Name
                return phones.OrderBy(e => e.Name);
            return phones.OrderBy(orderQuery);//сортируем
        }
    }
}
