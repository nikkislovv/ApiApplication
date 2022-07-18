using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiApplication.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; } // адрес покупателя
        public string ContactPhone { get; set; } // контактный телефон покупателя
        public string? UserId { get; set; }
        public virtual User? User { get; set; }
       
        public virtual List<Phone> Phones { get; set; }
        public Order()
        {
            Phones = new List<Phone>();
        }

    }
}
