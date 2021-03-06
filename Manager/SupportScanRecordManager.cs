﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tool;
using Service;
using DataTransfer;

namespace Manager
{
    public class SupportScanRecordManager
    {
        private SupportScanRecordService Service = ObjectContainer.GetInstance<SupportScanRecordService>();
        
        public OutputModel Add(string cookBookId,int userId)
        {
            if (string.IsNullOrEmpty(cookBookId))
                return OutputHelper.GetOutputResponse(ResultCode.NoParameter);
            CookBookService cookService = new CookBookService();
            if(!cookService.IsExist(cookBookId))
                return OutputHelper.GetOutputResponse(ResultCode.NoData,"菜谱不存在");
            SupportScanRecordTsfer s = Service.Get(cookBookId, userId);
            if (s != null)
                return OutputHelper.GetOutputResponse(ResultCode.ConditionNotSatisfied, "您已赞过此菜谱");
            SupportScanRecordTsfer ssr = new SupportScanRecordTsfer { 
            CookBookId=cookBookId,
            DateTime=DateTime.Now,
            Type=1,
            UserId=userId
            };
            if (Service.Add(ssr))
                return OutputHelper.GetOutputResponse(ResultCode.OK);
            return OutputHelper.GetOutputResponse(ResultCode.Error);
        }
        public OutputModel Delete(int id)
        {
            if (Service.Delete(id))
                return OutputHelper.GetOutputResponse(ResultCode.OK);
            return OutputHelper.GetOutputResponse(ResultCode.Error);
        }
        public OutputModel Delete(SupportScanRecordTsfer support)
        {
            if (Service.Delete(support))
                return OutputHelper.GetOutputResponse(ResultCode.OK);
            return OutputHelper.GetOutputResponse(ResultCode.Error);
        }
        public OutputModel Get(int id)
        {
            SupportScanRecordTsfer s=Service.Get(id);
            if ( s== null)
                return OutputHelper.GetOutputResponse(ResultCode.NoData);
            return OutputHelper.GetOutputResponse(ResultCode.OK,s);
        }

         /// <summary>
        /// 获取某菜谱某用户的记录
        /// </summary>
        /// <param name="cookbookid">菜谱id</param>
        /// <param name="userid">用户id</param>
        /// <returns></returns>
        public OutputModel Get(string  cookbookid,int userid)
        {
            SupportScanRecordTsfer s = Service.Get(cookbookid, userid);
            if (s == null)
                return OutputHelper.GetOutputResponse(ResultCode.NoData);
            return OutputHelper.GetOutputResponse(ResultCode.OK, s);
        }

        /// <summary>
        ///  获取某用户赞过的菜谱
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <param name="type">1为点赞</param>
        /// <returns></returns>
        public OutputModel GetListUser(int userid)
        {
            List<SupportScanRecordTsfer> list = Service.GetListUser(userid);
            if (list == null)
                return OutputHelper.GetOutputResponse(ResultCode.NoData);
            return OutputHelper.GetOutputResponse(ResultCode.OK, list);
        }

        /// <summary>
        /// 获取某菜谱点赞的列表
        /// </summary>
        /// <param name="cookbookid"></param>
        /// <param name="type">1为点赞</param>
        /// <returns></returns>
        public OutputModel GetListCookbook(string  cookbookid)
        {
            List<SupportScanRecordTsfer> list = Service.GetListCookbook(cookbookid);
            if (list == null)
                return OutputHelper.GetOutputResponse(ResultCode.NoData);
            return OutputHelper.GetOutputResponse(ResultCode.OK);
        }

        public OutputModel GetList()
        {
            List<SupportScanRecordTsfer> list = Service.GetList();
            if (list == null)
                return OutputHelper.GetOutputResponse(ResultCode.NoData);
            return OutputHelper.GetOutputResponse(ResultCode.OK);
        }
    }
}
