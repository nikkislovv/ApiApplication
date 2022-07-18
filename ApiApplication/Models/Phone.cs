using System;
using System.Collections.Generic;

namespace ApiApplication.Models
{
    public class Phone
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual Company Company { get; set; }
        public Guid CompanyId { get; set; }
        public int Price { get; set; }
        public virtual List<Order> Orders { get; set; }

        public Phone()
        {
            Orders = new List<Order>();
        }
    }
}
