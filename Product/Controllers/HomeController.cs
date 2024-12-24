using Product.Models;
using Product.WebShared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Product.Controllers
{
    [LogExecutionTime]
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            //return Redirect("https://drive.google.com/file/d/1tfe6wqU1l6Cwn5AG5sIpn8kFg0V_7gZK/view?usp=drive_link");

            if (User.Identity.IsAuthenticated)
            {
                // 用戶已經登入，重定向到其他頁面
                return RedirectToAction("Index", "Category");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Index(string 帳號, string 密碼)
        {
            dbProductEntities db = new dbProductEntities();
            var member = db.會員
                .Where(m => m.帳號 == 帳號 && m.密碼 == 密碼)
                .FirstOrDefault();

            if (member != null)
            {
                FormsAuthentication.RedirectFromLoginPage
                    (member.帳號, true);
                return RedirectToAction("Index", "Category");
            }           
            ViewBag.IsLogin = true;
            return View();
        }

        public ActionResult DownloadFile()
        {
            var Path = Server.MapPath("~/Files/會員商品管理系統說明.pptx");

            // 檔案類型
            var contentType = "application/octet-stream";

            // 檔案名稱
            var fileName = "會員商品管理系統說明.pptx";

            return File(Path, contentType, fileName);
        }
    }
}