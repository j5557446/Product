﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Product.Controllers
{
    public class PermissionErrorMsgController : Controller
    {
        // GET: PermissionErrorMsg
        [Authorize]
        public ActionResult Index(string msg)
        {
            ViewBag.ErrorMsg = msg;
            return View();
        }
    }
}