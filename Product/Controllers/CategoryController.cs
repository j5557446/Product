using Product.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Product.WebShared;
using System.Web.Script.Serialization;

namespace Product.Controllers
{
    [LogExecutionTime]
    public class CategoryController : Controller
    {
        dbProductEntities db = new dbProductEntities();

        [Authorize]
        // GET: Category

        public ActionResult Index(string searchText)
        {
            string uid = User.Identity.Name;
            string Permission = db.會員.Where(m => m.帳號 == uid).FirstOrDefault().權限;
            ViewBag.Permission = Permission;

            List<產品類別> category = new List<產品類別>();
            foreach (var item in db.產品類別.Where(m => (m.類別名稱.Contains(searchText) || searchText == null)).OrderByDescending(m => m.類別編號))
            {
                category.Add(new 產品類別()
                {
                    類別編號 = item.類別編號,
                    類別名稱 = item.類別名稱,
                    編輯者 = item.編輯者,
                    修改日 = Sys.StringConverDateTimeString(item.修改日),
                    建立日 = Sys.StringConverDateTimeString(item.建立日)
                });
            }

            Session["cur_category"] = category; //record current category;
            return View(category);

        }

        [Authorize]
        public ActionResult Create()
        {
            string uid = User.Identity.Name;
            string Permission = db.會員.Where(m => m.帳號 == uid).FirstOrDefault().權限;
            if (!Permission.Contains("C"))
            {
                return RedirectToAction("Index", "PermissionErrorMsg", new { msg = "您的身份無新增的權限" });
            }
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(string 類別名稱)
        {
            string editdate = DateTime.Now.ToString("yyyyMMddHHmmss");
            產品類別 category = new 產品類別();
            category.類別名稱 = 類別名稱;
            category.編輯者 = User.Identity.Name;
            category.建立日 = editdate;
            category.修改日 = editdate;
            db.產品類別.Add(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Delete(int cid)
        {
            string uid = User.Identity.Name;
            string Permission = db.會員.Where(m => m.帳號 == uid).FirstOrDefault().權限;
            if (!Permission.Contains("D"))
            {
                return RedirectToAction("Index", "PermissionErrorMsg", new { msg = "您的身份無刪除的權限" });
            }

            var products = db.產品資料.Where(m => m.類別編號 == cid).ToList();
            var category = db.產品類別.Where(m => m.類別編號 == cid).FirstOrDefault();
            db.產品資料.RemoveRange(products);
            db.產品類別.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Edit(int cid)
        {
            string uid = User.Identity.Name;
            string Permission = db.會員.Where(m => m.帳號 == uid).FirstOrDefault().權限;
            if (!Permission.Contains("U"))
            {
                return RedirectToAction("Index", "PermissionErrorMsg", new { msg = "您的身份無編輯的權限" });
            }

            var category = db.產品類別.Where(m => m.類別編號 == cid).FirstOrDefault();
            return View(category);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(int 類別編號, string 類別名稱)
        {
            string editdate = DateTime.Now.ToString("yyyyMMddHHmmss");
            var category = db.產品類別.Where(m => m.類別編號 == 類別編號).FirstOrDefault();
            category.類別名稱 = 類別名稱;
            category.編輯者 = User.Identity.Name;
            category.修改日 = editdate;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Export()
        {
            List<產品類別> cur_category = Session["cur_category"] as List<產品類別>;
            if (cur_category == null)
            {
                // 處理 cur_category 為 null 的情況
                return RedirectToAction("Index");
            }

            Export e = new Export();
            return (e.ExportToExcel(cur_category));

        }

        public JsonResult IsCategoryAvailable(string 類別名稱, int 類別編號 = 0)
        {
            if (類別編號 == 0 )
            {
                // 新增操作
                var category = db.產品類別.FirstOrDefault(c => c.類別名稱 == 類別名稱);
                return Json(category == null, JsonRequestBehavior.AllowGet);
            }
            else
            {
                // 編輯操作
                var category = db.產品類別.FirstOrDefault(c => c.類別名稱 == 類別名稱 && c.類別編號 != 類別編號);
                return Json(category == null, JsonRequestBehavior.AllowGet);
            }
        }
    }


}