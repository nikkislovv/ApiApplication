using ApiApplication.Models;
using System.Collections.Generic;

namespace ApiApplication.ModelsDTO.OrderDTO
{
    public class OrdersToShowDto
    {
        public string FullName { get; set; }
        public string Address { get; set; } // адрес покупателя
        public string ContactPhone { get; set; } // контактный телефон покупателя
        public string Email { get; set; }
        public virtual ICollection<PhonesToShowDto> Phones { get; set; } = new List<PhonesToShowDto>();
    }
}
