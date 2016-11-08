using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShoppingCart.Models;
using Microsoft.AspNet.Identity;

namespace ShoppingCart.Controllers
{
    public class OrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Orders
        [Authorize]
        public ActionResult Index() { 
        {
            var userId = db.Users.Find(User.Identity.GetUserId());
            if (User.IsInRole("Admin"))
            {
                var adminOrders = db.Orders.Include(o => o.Customer);
                return View(adminOrders.ToList());
            }
            var user = db.Users.Find(User.Identity.GetUserId());
            var orders = db.Orders.Where(o => o.CustomerId == user.Id);
            return View(orders.ToList());
        }
         
        
}
    // GET: Orders/Details/5
    public ActionResult Details(int? id)
    {
        if (id == null)
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        Order order = db.Orders.Find(id);
        if (order == null)
        {
            return HttpNotFound();
        }
        return View(order);
    } 

        // GET: Orders/Create
        public ActionResult Create()
        {
            
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "Id,Address,City,State,ZipCode,Country,Phone,Total,OrderDate,CustomerId")] Order order)
        {
            if (ModelState.IsValid)
            {

                var userId = db.Users.Find(User.Identity.GetUserId());
                var total = db.ShoppingCarts.Sum(s => s.Item.Price * s.Count);
                order.Total = total;
                order.OrderDate = DateTime.Now;
                order.CustomerId = userId.Id;
                order.placed = false;
                db.Orders.Add(order);
                

                if (order != null)
                {
                    var cartd = db.ShoppingCarts.Where(c => c.CustomerId == userId.Id).ToList();
                    foreach (var x in cartd)
                    {
                        var details = new OrderDetails();
                        details.Item = x.Item;
                        details.OrderId = order.Id;
                        details.Quantity = x.Count;
                        details.UnitPrice = x.Item.Price;
                        db.OrderDetails.Add(details);

                    }

                    db.SaveChanges();
                    TempData["orderid"] = order.Id;
                    return RedirectToAction("Index", "OrderDetails");

                }
                }

                ViewBag.CustomerId = new SelectList(db.Users, "Id", "FirstName", order.CustomerId);
                return View(order);
            }


        // GET: Orders/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.Users, "Id", "FirstName", order.CustomerId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "Id,Address,City,State,ZipCode,Country,Phone,Total,OrderDate,CustomerId")] Order order)
        {
            if (ModelState.IsValid)
            {

                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(db.ApplicationUsers, "Id", "FirstName", order.CustomerId);
            return View(order);
        }

        // GET: Orders/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
