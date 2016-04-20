﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Manager;
using Tool;

namespace Delicacy.Controllers
{
    /// <summary>
    /// 用户中心控制器
    /// </summary>
    public class UserCenterController : BaseController
    {
        //
        // GET: /UserCenter/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult WaitCheck()
        {
            return View();
        }

        /// <summary>
        /// 获取待审核菜谱
        /// </summary>
        /// <returns></returns>
        public ContentResult GetWaitCheckCookBook()
        {//这里没有分页
            if (user == null)
                return Content(OutputHelper.GetOutputResponse(ResultCode.NoLogin));
            return Content(new CookBookManager().GetWaitCheckCookBook(user.UserId));
        }
	}
}