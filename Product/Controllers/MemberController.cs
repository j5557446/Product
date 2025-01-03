﻿using Product.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Product.WebShared;
using SixLabors.ImageSharp;

namespace Product.Controllers
{
    [LogExecutionTime]
    public class MemberController : Controller
    {
        dbProductEntities db = new dbProductEntities();
        // GET: Member
        [Authorize]
        public ActionResult Index(string searchText)
        {
            //string uid = User.Identity.Name;
            //string role = db.會員.Where(m => m.帳號 == uid).FirstOrDefault().角色;
            if (!IsAdmin())
            {
                return RedirectToAction("index", "PermissionErrorMsg", new { msg = "您的身份無管理會員的權限" });
            }

            List<會員> members = new List<會員>();
            foreach (var item in db.會員.Where(m => (m.帳號.Contains(searchText) || searchText == null)))
            {
                var member = new 會員();
                member.帳號 = item.帳號;
                member.密碼 = item.密碼;
                member.權限 =
                    (item.權限.Contains("R") ? "讀取 " : "") +
                    (item.權限.Contains("C") ? "新增 " : "") +
                    (item.權限.Contains("U") ? "修改 " : "") +
                    (item.權限.Contains("D") ? "刪除 " : "");
                member.角色 = item.角色;
                members.Add(member);
            }

            Session["cur_members"] = members; //record current category;
            return View(members);
            
        }

        [Authorize]
        public ActionResult Create() 
        {
            //string uid = User.Identity.Name;
            //string role = db.會員.Where(m => m.帳號 == uid).FirstOrDefault().角色;
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "PermissionErrorMsg", new { msg = "您的身份無管理會員的權限" });
            }
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(string 帳號, string 密碼, string 角色, string[] 權限)
        {
            string uid = 帳號;
            var tempMember = db.會員.Where(m => m.帳號 == uid).FirstOrDefault();
            if (tempMember != null)
            {
                ViewBag.IsMember = true;
                return View();
            }

            string Permission = "R"; 

            if (權限 != null)
            {
                for (int i = 0; i < 權限.Length; i++)
                {
                    Permission += 權限[i];
                }
            }

            會員 member = new 會員();
            member.帳號 = 帳號;
            member.密碼 = 密碼;
            member.角色 = 角色;
            member.權限 = Permission;
            db.會員.Add(member);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Delete(string userid)
        {
            //string uid = User.Identity.Name;
            //string role = db.會員.Where(m => m.帳號 == uid).FirstOrDefault().角色;
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "PermissionErrorMsg", new { msg = "您的身份無管理會員的權限" });
            }
            var member = db.會員.Where(m => m.帳號 == userid).FirstOrDefault();
            db.會員.Remove(member);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Edit(string userid)
        {
            //string uid = User.Identity.Name;
            //string role = db.會員.Where(m => m.帳號 == uid).FirstOrDefault().角色;
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "PermissionErrorMsg", new { msg = "您的身份無管理會員的權限" });
            }

            var member = db.會員.Where(m => m.帳號 == userid).FirstOrDefault();
            return View(member);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(string 帳號, string 密碼, string 角色, string[] 權限)
        {
            string Permission = "R";
            if (權限 != null)
            {
                for (int i = 0; i < 權限.Length; i++)
                {
                    Permission += 權限[i];
                }
            }

            var member = db.會員.Where(m => m.帳號 == 帳號).FirstOrDefault();
            member.密碼 = 密碼;
            member.角色 = 角色;
            member.權限 = Permission;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Export()
        {
            
            if (!IsAdmin())
            {
                return RedirectToAction("index", "PermissionErrorMsg", new { msg = "您的身份無管理會員的權限" });
            }

            
                List<會員> cur_members = Session["cur_members"] as List<會員>;
                if (cur_members == null)
                {
                // 處理 cur_members 為 null 的情況
                return RedirectToAction("Index");
                }

                Export e = new Export();
                return (e.ExportToExcel(cur_members));           
        }

        public Boolean IsAdmin()
        {

           string uid = User.Identity.Name;
            string role = db.會員.Where(m => m.帳號 == uid).FirstOrDefault().帳號;
            if (role == "admin")
            {
                return true;
            }
            return false;
        }
    }
}