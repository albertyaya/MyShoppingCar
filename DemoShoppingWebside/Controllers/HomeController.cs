using DemoShoppingWebside.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace DemoShoppingWebside.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        dbShoppingCarEntities1 db = new dbShoppingCarEntities1();
        public ActionResult Index()
        {
            var products = db.table_Product.OrderByDescending(m => m.Id).ToList();
            return View("../Home/Index", "_LayoutMember", products);
        }
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Register(table_Member Member)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }

            var member = db.table_Member.Where(m => m.UserId == Member.UserId).FirstOrDefault();

            if (member == null)
            {
                db.table_Member.Add(Member);
                db.SaveChanges();

                return RedirectToAction("Login");
            }
            ViewBag.Message = "帳號已被使用，請重新註冊";
            return View();
        }
        [AllowAnonymous]
        public ActionResult Login()
        {

            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(string userid, string password)
        {
            var member = db.table_Member.Where(m => m.UserId == userid && m.Password == password).FirstOrDefault();
            if (member == null)
            {

                return View();

            }

            else
            {
                Session["Welcome"] = $"{userid}您好";
                FormsAuthentication.RedirectFromLoginPage(userid, true);
                return RedirectToAction("Index");
            }
        }
        public ActionResult Logout()
        {
            //using System.Web.Security;
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Home");
        }

       
        public ActionResult ShoppingCar()
        {
            string UserId = User.Identity.Name;

            var orderDetails = db.table_OrderDetail.Where(m => m.UserId == UserId && m.IsApproved == "否").ToList();
            return View(orderDetails);
        }
       


        public ActionResult AddCar(string productId)
        { 
          string userId=User.Identity.Name;
          var CurrentCar=db.table_OrderDetail.Where(m=>m.ProductId ==productId&& m.IsApproved=="否"&&m.UserId==userId).FirstOrDefault(); //篩選該使用者購物車內是否有商品且尚未形成訂單的資料
          if (CurrentCar == null)
          {
               
                var product=db.table_Product.Where(m=>m.ProductId==productId).FirstOrDefault();
                var orderDetail = new table_OrderDetail();
                orderDetail.ProductId =product.ProductId;
                orderDetail.UserId = userId;
                orderDetail.Name = product.Name;
                orderDetail.Quantity = 1;
                orderDetail.IsApproved = "否";
                db.table_OrderDetail.Add(orderDetail);
          }
            else
            {
                CurrentCar.Quantity++;
            }
            db.SaveChanges();
            return RedirectToAction("ShoppingCar");
        }

        public ActionResult DeleteCar(int Id)
        {
            var orderDetails = db.table_OrderDetail.Where(m => m.Id == Id).FirstOrDefault();

            db.table_OrderDetail.Remove(orderDetails);
            db.SaveChanges();
            return RedirectToAction("ShoppingCar");
        }

        
        public ActionResult ShoppingCar(string Receiver, string Email, string Address)
        {
            string userId = User.Identity.Name;
            string guid = Guid.NewGuid().ToString(); //產生隨機訂單編號

            //加入訂單至 table_Order 資料表
            var order = new table_Order();
            order.OrderGuid = guid;
            order.UserId = userId;
            order.Receiver = Receiver;
            order.Email = Email;
            order.Address = Address;
            order.Date = DateTime.Now;
            db.table_Order.Add(order);

            //訂單加入後，需一併更新訂單明細內容
            var carList = db.table_OrderDetail.Where(m => m.IsApproved == "否" && m.UserId == userId).ToList();
            foreach (var item in carList)
            {
                item.OrderGuid = guid;
                item.IsApproved = "是";
            }
            db.SaveChanges();
            return RedirectToAction("OrderList");
        }

        public ActionResult OrderList()
        { 
        
          return View();    
        }

    }
    
}