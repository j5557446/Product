using Product.Models;
using Product.WebShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace Product.Controllers
{
    [LogExecutionTime]
    public class DrawController : Controller
    {
        dbProductEntities db = new dbProductEntities();
        // GET: Draw
        public ActionResult Bar()
        {
            var query = from c in db.產品類別
                        join p in db.產品資料 on c.類別編號 equals p.類別編號 into g
                        select new
                        {
                            CategoryName = c.類別名稱,
                            ProductCount = g.Count()
                        };

            var data = query.OrderByDescending(d => d.ProductCount).ToList();

            ViewBag.x = data.Select(d => d.CategoryName).ToArray();
            ViewBag.y = data.Select(d => d.ProductCount).ToArray();

           
            return View();
        }


    }
}