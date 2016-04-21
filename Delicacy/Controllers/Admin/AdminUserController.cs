﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Manager;
using DataTransfer;
using Tool;

namespace Delicacy.Controllers.Admin
{
    public class AdminUserController : BaseController
    {
        
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string userId,string pwd)
        {
            return Content(new AdminUserManager().Login(userId, pwd));
        }
	}
}