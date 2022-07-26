﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectDemo.EDM;
using ProjectDemo.Filters;
using ProjectDemo.Models;

namespace ProjectDemo.Areas.Users.Controllers
{
    [CustomAuthenticationFilter]
    public class DefaultController : Controller
    {
        ecommerceEntities dc = new ecommerceEntities();
        // GET: Users/Default
        public ActionResult HomePage()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection fc)
        {
            string email = fc["email"];
            string pass = fc["password"];

            var obj = dc.tblUsers.Where(u => u.email == email && u.password == pass).FirstOrDefault();
            if (obj != null)
            {
                Session["UserId"] = obj.user_id;
                Session["UserName"] = obj.f_name;
                return RedirectToAction("HomePage");
            }
            else
            {
                ViewBag.loginerror = "Invalid Email or Password.!";
            }

            return View();
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("HomePage");
        }

        public ActionResult Register()
        {
            return View();
        }

        
        public ActionResult Products()
        {
            return View(dc.tblproducts.ToList());
        }
        public ActionResult Single(int id)
        {
            return View(dc.tblproducts.Find(id));
        }

        public string AddToCart(int id)
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            var cartobj = dc.tblcarts.Where(c => c.product_id == id).FirstOrDefault();
            if (cartobj==null)
            {
                tblcart obj = new tblcart();
                obj.product_id = id;
                obj.qty = 1;
                obj.user_id = userid;

                dc.tblcarts.Add(obj);
                dc.SaveChanges();
            }
            else
            {
                cartobj.qty += 1;
                dc.Entry(cartobj).State = System.Data.Entity.EntityState.Modified;
                dc.SaveChanges();
            }
            return "Product added to Cart.";
        }

        [HttpPost]
        public string UpdateCartQty(int id, int qty)
        {
            var obj = dc.tblcarts.Find(id);
            obj.qty = qty;

            dc.Entry(obj).State = System.Data.Entity.EntityState.Modified;
            dc.SaveChanges();
            return "Cart Qty updated.";
        }

        public ActionResult Cart()
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            return View(dc.tblcarts.Where(c=>c.user_id==userid).ToList());
        }

        public ActionResult Checkout()
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            tblorder objorder = new tblorder();
            objorder.orderdate = DateTime.Now;
            objorder.user_id = userid;
            objorder.status = (byte)OrderStatusEnum.Confirmed;

            dc.tblorders.Add(objorder);
            dc.SaveChanges();

            var cart = dc.tblcarts.Where(c=>c.user_id==userid).ToList();
            tblorderdetail objorderdetail = new tblorderdetail();
            foreach (var item in cart)
            {
                objorderdetail.product_id = item.product_id;
                objorderdetail.qty = item.qty;
                objorderdetail.order_id = objorder.order_id;

                dc.tblorderdetails.Add(objorderdetail);
                dc.SaveChanges();
            }

            return RedirectToAction("Success");
        }

        public ActionResult Success()
        {
            return View();
        }
    }
}