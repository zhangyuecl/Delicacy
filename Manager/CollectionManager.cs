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
    public class CollectionManager
    {
        private CollectionService service = new CollectionService();
        public OutputModel Add(string cookBookId, int userId)
        {
            if (string.IsNullOrEmpty(cookBookId) || userId == 0)
                return OutputHelper.GetOutputResponse(ResultCode.NoParameter);
            //判断是否关注过
            CollectionTsfer l = service.Get(userId, cookBookId);
            if (l != null)
                return Delete(cookBookId, userId);

            //判断菜谱是否存在
            CookBookService cookService = new CookBookService();
            if (!cookService.IsExist(cookBookId))
                return OutputHelper.GetOutputResponse(ResultCode.ConditionNotSatisfied, "菜谱不存在");
            CollectionTsfer like = new CollectionTsfer
            {
                OperateId = cookBookId,
                DateTime = DateTime.Now,
                UserId = userId,
                Type = 1,

            };
            if (service.Add(like))
                return OutputHelper.GetOutputResponse(ResultCode.OK, "已收藏菜谱");
            return OutputHelper.GetOutputResponse(ResultCode.Error);
        }

        public OutputModel Delete(int id)
        {
            if (service.Delete(id))
                return OutputHelper.GetOutputResponse(ResultCode.OK);
            return OutputHelper.GetOutputResponse(ResultCode.Error);
        }
        public OutputModel Delete(string cookbookid,int userid)
        {

            if (service.Delete(cookbookid,userid))
                return OutputHelper.GetOutputResponse(ResultCode.OK, "已取消收藏");
            return OutputHelper.GetOutputResponse(ResultCode.Error);
        }
        /// <summary>
        /// 获取某收藏数据
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public OutputModel Get(int id)
        {
            CollectionTsfer like = service.Get(id);
            if (like == null)
                return OutputHelper.GetOutputResponse(ResultCode.NoData);
            return OutputHelper.GetOutputResponse(ResultCode.OK, like);
        }
        /// <summary>
        /// 获取某用户某菜谱的收藏
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <param name="cookbookid">菜谱id</param>
        /// <returns></returns>
        public OutputModel Get(int userid, string cookbookid)
        {
            CollectionTsfer like = service.Get(userid, cookbookid);
            if (like == null)
                return OutputHelper.GetOutputResponse(ResultCode.NoData);
            return OutputHelper.GetOutputResponse(ResultCode.OK, like);
        }
        public OutputModel GetList()
        {
            List<CollectionTsfer> list = service.GetList();
            if (list.Count == 0)
                return OutputHelper.GetOutputResponse(ResultCode.NoData);
            return OutputHelper.GetOutputResponse(ResultCode.OK, list);
        }
        /// <summary>
        /// 获取某用户的收藏
        /// </summary>
        /// <param name="userid">用户id</param>
        /// <returns></returns>
        public OutputModel GetsUser(string pageIndex, string pageSize, int userId)
        {
            int index, size;
            CheckParameter.PageCheck(pageIndex, pageSize, out index, out size);
            List<CollectionTsfer> list = service.GetsUser(index, size, userId);
            if (list.Count <= 0)
                return OutputHelper.GetOutputResponse(ResultCode.NoData);
            List<string> ids = new List<string>();
            foreach (var collect in list)
            {
                ids.Add(collect.OperateId);
            }
            OutputModel o = new CookBookManager().GetListByIds(ids);
            if (o.StatusCode != 1)
                return o;
            List<CookBookTsfer> listcook = new List<CookBookTsfer>();
            listcook = (List<CookBookTsfer>)o.Data;
            UserInfoService userService = new UserInfoService();
            listcook.ForEach(l => l.UserName = userService.Get(l.UserId).Name);
            return OutputHelper.GetOutputResponse(ResultCode.OK, listcook);
        }
        /// <summary>
        /// 获取某菜谱的收藏列表
        /// </summary>
        /// <param name="userid">菜谱id</param>
        /// <returns></returns>
        public OutputModel GetsCookbook(string cookbookid)
        {
            List<CollectionTsfer> list = service.GetsCookbook(cookbookid);
            if (list.Count == 0)
                return OutputHelper.GetOutputResponse(ResultCode.NoData);
            return OutputHelper.GetOutputResponse(ResultCode.OK, list);
        }
    }
}
