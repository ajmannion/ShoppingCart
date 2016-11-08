using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoppingCart.Models
{
    public class item
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string MediaUrl { get; set; }
        [AllowHtml]
        public string Description { get; set; }

        public string Category { get; set; }

        public string Brand { get; set; }

        public string Color { get; set; }

     
        public DateTimeOffset Created { get; set; }

        public DateTimeOffset? Updated { get; set; }
    }
}