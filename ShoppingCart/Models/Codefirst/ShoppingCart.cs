using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingCart.Models
{
    public class Shoppingcart
    {
        public int Id { get; set; }

        public int ItemId { get; set; }

        public String CustomerId { get; set; }

        public int Count { get; set; }

        public DateTimeOffset CreationDate { get; set; }

        public virtual item Item { get; set; }

        public virtual ApplicationUser Customer { get; set; }

    }
}