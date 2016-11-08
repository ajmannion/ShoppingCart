using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingCart.Models
{
    public class Order
    {
       public bool placed { get; set; }
        public int Id { get; set; }
        
        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }

        public  string Country { get; set; }

        public string Phone { get; set; }

        public decimal Total { get; set; }

        public DateTimeOffset OrderDate { get; set;}

        public string CustomerId { get; set; }

        public virtual ApplicationUser Customer { get; set; } /* one to one objects */

        public virtual ICollection<OrderDetails> OrderDetails { get; set; } /* one to many objects */
    }
}