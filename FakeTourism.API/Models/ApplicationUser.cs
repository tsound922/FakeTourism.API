using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeTourism.API.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string Address { get; set; }
        //shopping cart
        public ShoppingCart ShoppingCart { get; set; }
        
        //orders
        //One user can have more than 1 order so use ICollection
        public ICollection<Order> Orders { get; set; }

        public virtual ICollection<IdentityUserRole<string>> UserRoles { get; set; }
        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }
        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }
        public virtual ICollection<IdentityUserToken<string>> Tokens { get; set; }
    }
}
