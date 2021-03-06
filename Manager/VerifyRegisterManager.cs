﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service;
using Tool;
using DataTransfer;

namespace Manager
{
    public class VerifyRegisterManager
    {
        private VerifyRegisterServer verifyServer = ObjectContainer.GetInstance<VerifyRegisterServer>();
        public bool VerifyEmail(string guid)
        {
            if (string.IsNullOrEmpty(guid))
                return false;
            VerifyRegisterTsfer verifyDt = verifyServer.Get(guid);
            if (verifyDt == null || (verifyDt.OutDate < DateTime.Now) || verifyDt.IsUsed)
                return false;
            //验证通过
            UserInfoService userServer = ObjectContainer.GetInstance<UserInfoService>();
            UserInfoTsfer userDt = userServer.GetByLoginId(verifyDt.LoginId);
            if (userDt == null)
                return false;
            if(userDt.Status==1)
            {
                System.Web.HttpContext.Current.Session["user"] = userDt;
                return true;
            }
            userDt.Status = 1;
            verifyDt.IsUsed = true;

            if (!userServer.Update(userDt, verifyDt))
                return false;
            System.Web.HttpContext.Current.Session["user"] = userDt;
            return true;
        }

        /// <summary>
        /// 忘记密码的验证邮箱
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool VerifyEmailResetPwd(string guid, out string email)
        {
            email = "";
            if (string.IsNullOrEmpty(guid))
                return false;
            VerifyRegisterTsfer verifyDt = verifyServer.Get(guid);
            if (verifyDt == null || (verifyDt.OutDate < DateTime.Now) || verifyDt.IsUsed)
                return false;
            email = verifyDt.LoginId;
            verifyDt.IsUsed = true;
            if (verifyServer.Update(verifyDt))
                return true;
            else
                return false;
        }
    }
}
