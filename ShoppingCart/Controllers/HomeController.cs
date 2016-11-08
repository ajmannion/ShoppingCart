using Microsoft.AspNet.Identity;
using ShoppingCart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoppingCart.Controllers
{

    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult index()
        {
           // var user = db.Users.Find(User.Identity.GetUserId());
           //var ShoppingCartList = db.ShoppingCarts.Where(s => s.CustomerId == user.Id).ToList();
            var itemList = db.Items.ToList();
            return View(itemList);
        }

        public ActionResult products()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult checkout()
        {
            ViewBag.Message = "Your checkout page.";

            return View();
        }

        public ActionResult account()
        {
            ViewBag.Message = "Your Account page.";

            return View();
        }
        public ActionResult register()
        {
            ViewBag.Message = "Your register page.";

            return View();
        }

        public ActionResult single(int? id)
        {
            item items = db.Items.Find(id);

            return View(items);
        }
        public ActionResult Thankyou()
        {
            //var finalOrder = db.Orders.Find(orderId);
            // finalOrder.placed = true;
            // db.Entry("Orders").Property("placed").IsModified = true;
            // db.SaveChanges();*@
            var usr = db.Users.Find(User.Identity.GetUserId());
            var userShop = db.ShoppingCarts.Where(s => s.CustomerId == usr.Id).ToList();
            foreach (var deleteCart in userShop)
            {
                db.ShoppingCarts.Remove(deleteCart);
            }
            db.SaveChanges();
            return View();
        }
        public ActionResult typo()
        {
            ViewBag.Message = "Your Typo page.";

            return View();
        }
    }
}