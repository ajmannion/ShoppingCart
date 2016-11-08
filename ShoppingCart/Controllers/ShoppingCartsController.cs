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
    public class ShoppingCartsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        // GET: ShoppingCarts
        [Authorize]
        public ActionResult Index()
        {
            var userId = db.Users.Find(User.Identity.GetUserId());
            var shoppingCarts = db.ShoppingCarts.Where(s => s.CustomerId == userId.Id).ToList();
            if (shoppingCarts.Count != 0)
            {
                var total = shoppingCarts.Sum(s => s.Item.Price * s.Count);
                ViewBag.total = total;
            }
            //ViewBag.total=0;
            return View(shoppingCarts.ToList());
        }

        // GET: ShoppingCarts/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var shoppingCart = db.ShoppingCarts.Find(id);
            if (shoppingCart == null)
            {
                return HttpNotFound();
            }
            return View(shoppingCart);
        }

        // POST: ShoppingCarts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(int Itemid)
        {
            if (ModelState.IsValid)
            {

                Shoppingcart Cart = new Shoppingcart();
                var userId = db.Users.Find(User.Identity.GetUserId());
                var exshopping = db.ShoppingCarts.Where(s => s.CustomerId == userId.Id && s.Item.Id == Itemid).ToList();
                if (exshopping.Count == 0)
                {
                    Cart.CreationDate = System.DateTime.Now;
                    Cart.Count = 1;
                    Cart.CustomerId = userId.Id;
                    Cart.ItemId = Itemid;
                    db.ShoppingCarts.Add(Cart);
                    db.SaveChanges();
                    return RedirectToAction("index", "Home");
                }

                foreach (var items in exshopping)
                {
                    items.Count++;
                    db.Entry(items).Property("Count").IsModified = true;

                }db.SaveChanges();
            }

            return RedirectToAction("index", "Home");
        }

        // GET: ShoppingCarts/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shoppingcart shoppingCart = db.ShoppingCarts.Find(id);
            if (shoppingCart == null)
            {
                return HttpNotFound();
            }
            ViewBag.Itemid = new SelectList(db.Items, "Id", "Name", shoppingCart.ItemId);
            return View(shoppingCart);
        }

        // POST: ShoppingCarts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "Id,Itemid,Count,CreationDate")] Shoppingcart shoppingCart)
        {
            if (ModelState.IsValid)
            {
                var userId = db.Users.Find(User.Identity.GetUserId());
                shoppingCart.CustomerId = userId.Id;
                db.Entry(shoppingCart).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Itemid = new SelectList(db.Items, "Id", "Name", shoppingCart.ItemId);
            return View(shoppingCart);
        }

        [Authorize]
        public ActionResult AllDelete()
        {
            var userId = db.Users.Find(User.Identity.GetUserId());
            var shoppingCarts = db.ShoppingCarts.Where(s => s.CustomerId == userId.Id).ToList();
            foreach (var dcart in shoppingCarts)
            {
                db.ShoppingCarts.Remove(dcart);
               
        }
            db.SaveChanges();
            return RedirectToAction("Index","ShoppingCarts");

    } 
        // GET: ShoppingCarts/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shoppingcart shoppingCart = db.ShoppingCarts.Find(id);
            if (shoppingCart == null)
            {
                return HttpNotFound();
            }
            return View(shoppingCart);
        }

       

        // POST: ShoppingCarts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            Shoppingcart shoppingCart = db.ShoppingCarts.Find(id);
            db.ShoppingCarts.Remove(shoppingCart);
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
