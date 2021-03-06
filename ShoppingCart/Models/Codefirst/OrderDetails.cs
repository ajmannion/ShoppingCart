﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingCart.Models
{
    public class OrderDetails
    {

        public int Id { get; set; }

        public int ItemId { get; set; }

        public int OrderId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public virtual item Item { get; set; }

        public virtual Order Order { get; set; }
    }
}