﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EF;
using DataTransfer;
using Tool;

namespace Service
{
    public class VerifyRegisterServer : BaseService<VerifyRegister>
    {
        public bool Add(VerifyRegisterTsfer verifyDt)
        {
            base.Add(TransferObject.ConvertObjectByEntity<VerifyRegisterTsfer, VerifyRegister>(verifyDt));
            return Save() > 0;
        }
        public VerifyRegisterTsfer Get(string guid)
        {
            return TransferObject.ConvertObjectByEntity<VerifyRegister, VerifyRegisterTsfer>(Select(o => o.GUID == guid).FirstOrDefault());
        }

        public bool Update(VerifyRegisterTsfer verifyDt)
        {
            base.Update(TransferObject.ConvertObjectByEntity<VerifyRegisterTsfer, VerifyRegister>(verifyDt));
            return Save() > 0;
        }

        /// <summary>
        /// 判断发给此人的链接是否已经使用
        /// </summary>
        /// <param name="email"></param>
        /// <param name="type">1注册邮件  2忘记密码邮件</param>
        /// <returns></returns>
        public bool IsSend(string email,int type)
        {
            return Select(o => o.LoginId == email && (!o.IsUsed && o.OutDate < DateTime.Now)&&o.Type==type).Any();
        }
    }
}
