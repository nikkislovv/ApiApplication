using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace ApiApplication.Models
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
        public virtual ICollection<Order> Orders { get; set; }


    }
}
