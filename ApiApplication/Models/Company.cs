using System;
using System.Collections.Generic;

namespace ApiApplication.Models
{
    public class Company
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public virtual ICollection<Phone> Phones { get; set; } = new List<Phone>();
    }
}
