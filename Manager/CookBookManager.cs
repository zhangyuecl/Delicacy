﻿﻿﻿﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service;
using Tool;
using DataTransfer;
using System.IO;
namespace Manager
{
    public class CookBookManager
    {
        private CookBookService service = ObjectContainer.GetInstance<CookBookService>();

        public OutputModel AddCookBook(int userId, string taste, string foodSort, string name, string description, string tips, string finalImg, string processImgDes, string mainMaterial, string status, string assistMaterial, string foodMaterial)
        {
            int iTaste, iFoodSort, iStatus;
            if (!int.TryParse(taste, out iTaste) || !int.TryParse(foodSort, out iFoodSort) || !int.TryParse(status, out iStatus))
            {
                return OutputHelper.GetOutputResponse(ResultCode.ErrorParameter);
            }
            //插入式菜谱的状态只能是0待审核  2存草稿
            if (iStatus != 0 && iStatus != 2)
                return OutputHelper.GetOutputResponse(ResultCode.Error);
            if (CheckParameter.IsNullOrEmpty(name, finalImg, processImgDes, mainMaterial, assistMaterial))
                return OutputHelper.GetOutputResponse(ResultCode.NoParameter);
            string cookBookId = Guid.NewGuid().ToString().Replace("-", "");
            //检查过程图  listProcess中的CookBookId还没有赋值
            List<CookProcessTsfer> listProcess = GetListProcess(processImgDes, cookBookId);
            if (listProcess.Count == 0)
                return OutputHelper.GetOutputResponse(ResultCode.ConditionNotSatisfied, "请插入菜谱过程");

            //判断食材
            List<CookMaterialTsfer> listMaterial = GetListMaterial(mainMaterial, assistMaterial, cookBookId);
            if (listMaterial.Count == 0)
                return OutputHelper.GetOutputResponse(ResultCode.ErrorParameter, "食材输入错误");
            //判断口味 类别
            TasteService tService = ObjectContainer.GetInstance<TasteService>();
            if (!tService.IsExist(iTaste))
                return OutputHelper.GetOutputResponse(ResultCode.ErrorParameter, "口味选择错误");
            FoodSortService foodService = ObjectContainer.GetInstance<FoodSortService>();
            if (!foodService.IsExist(iFoodSort))
                return OutputHelper.GetOutputResponse(ResultCode.ErrorParameter, "类别选择错误");
            //判断食材foodMaterial
            List<FoodMaterial_CookBookTsfer> listMaterCook = new List<FoodMaterial_CookBookTsfer>();
            if (!string.IsNullOrEmpty(foodMaterial))
            {
                string[] arrMaterial = foodMaterial.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                int temp;
                FoodMaterialService materialService = new FoodMaterialService();
                foreach (string material in arrMaterial)
                {
                    if (!int.TryParse(material, out temp))
                    {
                        return OutputHelper.GetOutputResponse(ResultCode.ErrorParameter, "食材选择错误");
                    }
                    if (!materialService.IsExist(temp))
                        return OutputHelper.GetOutputResponse(ResultCode.ErrorParameter, "食材选择错误");
                    FoodMaterial_CookBookTsfer materCook = new FoodMaterial_CookBookTsfer
                    {
                        CookBookId = cookBookId,
                        FoodMaterialId = temp
                    };
                    listMaterCook.Add(materCook);
                }
            }
            //进行插入
            CookBookTsfer bookTsfer = new CookBookTsfer()
            {
                CookBookId = cookBookId,
                Description = description,
                FoodSortId = iFoodSort,
                ImgUrl = finalImg,
                Name = name,
                Status = iStatus,
                TasteId = iTaste,
                Tips = tips,
                UserId = userId,
                ListProcess = listProcess,
                ListMaterial = listMaterial,
                DateTime = DateTime.Now

            };
            if (service.Add(bookTsfer, listMaterCook))
                return OutputHelper.GetOutputResponse(ResultCode.OK);
            else
                return OutputHelper.GetOutputResponse(ResultCode.Error);
        }

        /// <summary>
        /// 修改菜谱
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="taste"></param>
        /// <param name="foodSort"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="tips"></param>
        /// <param name="finalImg"></param>
        /// <param name="processImgDes"></param>
        /// <param name="mainMaterial"></param>
        /// <param name="status"></param>
        /// <param name="assistMaterial"></param>
        /// <param name="foodMaterial"></param>
        /// <param name="cookBookId"></param>
        /// <returns></returns>
        public OutputModel EditCookBook(int userId, string taste, string foodSort, string name, string description, string tips, string finalImg, string processImgDes, string mainMaterial, string status, string assistMaterial, string foodMaterial, string cookBookId)
        {
            int iTaste, iFoodSort, iStatus;
            if (!int.TryParse(taste, out iTaste) || !int.TryParse(foodSort, out iFoodSort) || !int.TryParse(status, out iStatus))
            {
                return OutputHelper.GetOutputResponse(ResultCode.ErrorParameter);
            }
            //插入式菜谱的状态只能是0待审核  2存草稿
            if (iStatus != 0 && iStatus != 2)
                return OutputHelper.GetOutputResponse(ResultCode.Error);
            if (CheckParameter.IsNullOrEmpty(name, finalImg, processImgDes, mainMaterial, assistMaterial))
                return OutputHelper.GetOutputResponse(ResultCode.NoParameter);
            //string cookBookId = Guid.NewGuid().ToString().Replace("-", "");
            //检查过程图  listProcess中的CookBookId还没有赋值
            List<CookProcessTsfer> listProcess = GetListProcess(processImgDes, cookBookId);
            if (listProcess.Count == 0)
                return OutputHelper.GetOutputResponse(ResultCode.ConditionNotSatisfied, "请插入菜谱过程");

            //判断食材
            List<CookMaterialTsfer> listMaterial = GetListMaterial(mainMaterial, assistMaterial, cookBookId);
            if (listMaterial.Count == 0)
                return OutputHelper.GetOutputResponse(ResultCode.ErrorParameter, "食材输入错误");
            //判断口味 类别
            TasteService tService = ObjectContainer.GetInstance<TasteService>();
            if (!tService.IsExist(iTaste))
                return OutputHelper.GetOutputResponse(ResultCode.ErrorParameter, "口味选择错误");
            FoodSortService foodService = ObjectContainer.GetInstance<FoodSortService>();
            if (!foodService.IsExist(iFoodSort))
                return OutputHelper.GetOutputResponse(ResultCode.ErrorParameter, "类别选择错误");
            //判断食材foodMaterial
            List<FoodMaterial_CookBookTsfer> listMaterCook = new List<FoodMaterial_CookBookTsfer>();

            if(!string.IsNullOrEmpty(foodMaterial))
            {
                string[] arrMaterial = foodMaterial.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                int temp;
                FoodMaterialService materialService = new FoodMaterialService();

                foreach (string material in arrMaterial)
                {
                    if (!int.TryParse(material, out temp))
                    {
                        return OutputHelper.GetOutputResponse(ResultCode.ErrorParameter, "食材选择错误");
                    }
                    if (!materialService.IsExist(temp))
                        return OutputHelper.GetOutputResponse(ResultCode.ErrorParameter, "食材选择错误");
                    FoodMaterial_CookBookTsfer materCook = new FoodMaterial_CookBookTsfer
                    {
                        CookBookId = cookBookId,
                        FoodMaterialId = temp
                    };
                    listMaterCook.Add(materCook);
                }
            }
           
            //进行插入
            CookBookTsfer bookTsfer = new CookBookTsfer()
            {
                CookBookId = cookBookId,
                Description = description,
                FoodSortId = iFoodSort,
                ImgUrl = finalImg,
                Name = name,
                Status = iStatus,
                TasteId = iTaste,
                Tips = tips,
                UserId = userId,
                ListProcess = listProcess,
                ListMaterial = listMaterial,
                DateTime = DateTime.Now

            };
            if (service.Edit(bookTsfer, listMaterCook))
                return OutputHelper.GetOutputResponse(ResultCode.OK);
            else
                return OutputHelper.GetOutputResponse(ResultCode.Error);
        }

        private List<CookProcessTsfer> GetListProcess(string processImgDes, string cookBookId)
        {
            List<CookProcessTsfer> listProcess = new List<CookProcessTsfer>();
            string[] arryProcess = processImgDes.Split(new[] { "|||" }, StringSplitOptions.RemoveEmptyEntries);
            //判断图片格式
            int sort = 1;
            foreach (string item in arryProcess)
            {
                string[] arrImgDesc = item.Split(new[] { "::" }, StringSplitOptions.RemoveEmptyEntries);
                if (arrImgDesc.Length < 2)
                    return new List<CookProcessTsfer>();
                if (RegExVerify.CheckImgExtension(Path.GetExtension(arrImgDesc[0])))
                {
                    CookProcessTsfer cpTsfer = new CookProcessTsfer()
                    {
                        CookBookId = cookBookId,
                        ImgUrl = arrImgDesc[0],
                        Description = arrImgDesc[1],
                        Sort = sort++
                    };
                    listProcess.Add(cpTsfer);
                }
                else
                    return new List<CookProcessTsfer>();
            }
            return listProcess;
        }

        private List<CookMaterialTsfer> GetListMaterial(string mainMaterial, string assistMaterial, string cookBookId)
        {

            //添加主料
            string arrMain = mainMaterial.Split(new[] { "|||::" }, StringSplitOptions.RemoveEmptyEntries)[0];
            List<CookMaterialTsfer> listMain = GenerateCookMaterial(arrMain, 1, cookBookId);
            if (listMain.Count == 0)
                return listMain;
            List<CookMaterialTsfer> list = new List<CookMaterialTsfer>();
            list.AddRange(listMain);
            //添加辅料
            string arrAssist = assistMaterial.Split(new[] { "|||::" }, StringSplitOptions.RemoveEmptyEntries)[0];
            List<CookMaterialTsfer> listAssist = GenerateCookMaterial(arrAssist, 2, cookBookId);
            if (listAssist.Count == 0)
                return listAssist;
            list.AddRange(listAssist);
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="material"></param>
        /// <param name="type">1主料 2辅料</param>
        /// <param name="cookBookId"></param>
        /// <returns></returns>
        private List<CookMaterialTsfer> GenerateCookMaterial(string material, int type, string cookBookId)
        {
            List<CookMaterialTsfer> list = new List<CookMaterialTsfer>();
            string[] arr = material.Split(new[] { "|||" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string item in arr)
            {
                string[] a = item.Split(new[] { "::" }, StringSplitOptions.RemoveEmptyEntries);
                if (a.Length < 2)
                    return new List<CookMaterialTsfer>();
                CookMaterialTsfer materialObj = new CookMaterialTsfer
                {
                    Amount = a[1],
                    CookBookId = cookBookId,
                    FoodMaterialName = a[0],
                    Type = type
                };
                list.Add(materialObj);
            }
            return list;
        }

        /// <summary>
        /// 获取待审核菜谱
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public OutputModel GetaPageCookBookByStatus(string pageIndex,string pageSize,int userId,string status)
        {
            int index, size,iStatus;
            CheckParameter.PageCheck(pageIndex, pageSize, out index, out size);
            if(!int.TryParse(status,out iStatus))
                return OutputHelper.GetOutputResponse(ResultCode.ErrorParameter);
            List<CookBookTsfer> list = service.GetPage(index, size, userId, iStatus);
            if (list.Count == 0)
                return OutputHelper.GetOutputResponse(ResultCode.NoData);
            UserInfoService userService = new UserInfoService();
            list.ForEach(o=>o.UserName=userService.Get(o.UserId).Name);

            return OutputHelper.GetOutputResponse(ResultCode.OK, list);
        }




        /// <summary>
        /// 根据cookbookId获取菜谱详情
        /// </summary>
        /// <param name="cookBookId"></param>
        /// <returns></returns>
        public OutputModel GetCookBook(string cookBookId)
        {
            if (string.IsNullOrEmpty(cookBookId))
                return OutputHelper.GetOutputResponse(ResultCode.NoParameter);
            CookBookTsfer cookbook = service.Get(cookBookId);
            if (cookbook == null)
                return OutputHelper.GetOutputResponse(ResultCode.ConditionNotSatisfied, "菜谱不存在");
            InitCookBook(cookbook);
            return OutputHelper.GetOutputResponse(ResultCode.OK, cookbook);
        }

        public CookBookTsfer GetCookbookById(string cookBookId)
        {
            if (string.IsNullOrEmpty(cookBookId))
                return null;
            CookBookTsfer  cookbook= service.Get(cookBookId);
             InitCookBook(cookbook);
             return cookbook;
        }

        /// <summary>
        /// 根据cookbookIds获取菜谱列表
        /// </summary>
        /// <param name="cookBookIds"></param>
        /// <returns></returns>
        public OutputModel GetListByIds(List<string> cookBookIds)
        {
            if (cookBookIds.Count <= 0)
                return OutputHelper.GetOutputResponse(ResultCode.NoParameter);
            List<CookBookTsfer> list = new List<CookBookTsfer>();
            foreach (string id in cookBookIds)
            {
                list.Add(service.Get(id,1));
            }
            if (list.Count <= 0)
                return OutputHelper.GetOutputResponse(ResultCode.NoData);
            return OutputHelper.GetOutputResponse(ResultCode.OK, list);
        }


        public OutputModel GetCookBookBySort(string  sort, string pageIndex, string pageSize)
        {
            int iSort,rowCount, index, size;
            
            CheckParameter.PageCheck(pageIndex, pageSize, out index, out size);
            if (!int.TryParse(sort, out iSort))
                return OutputHelper.GetOutputResponse(ResultCode.ErrorParameter);

            List<CookBookTsfer> list = service.GetPageBySort(iSort, index, size,1, out rowCount);//这里没有判断sortId是否存在
            if(list.Count==0)
                return OutputHelper.GetOutputResponse(ResultCode.NoData);
           int pageCount = (int)Math.Ceiling(rowCount * 1.0 / size);

            return OutputHelper.GetOutputResponse(ResultCode.OK, new { List=list,PageCount=pageCount});
        }

        public OutputModel SearchByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return OutputHelper.GetOutputResponse(ResultCode.NoParameter);
            List<CookBookTsfer> listCookBook = service.GetByName(name,1);
            if (listCookBook.Count == 0)
                return OutputHelper.GetOutputResponse(ResultCode.NoData);
            foreach (CookBookTsfer cookBook in listCookBook)
            {
                InitCookBook(cookBook);
            }
            return OutputHelper.GetOutputResponse(ResultCode.OK, listCookBook);
        }

        public void InitCookBook(CookBookTsfer cookBook)
        {
            //获取过程图
            CookProcessService processService = ObjectContainer.GetInstance<CookProcessService>();
            List<CookProcessTsfer> listProcess = processService.GetList(cookBook.CookBookId);
            //在录入的时候必须有三个步骤,这里没有判断步骤是否为空
            cookBook.ListProcess = listProcess;
            //获取材料
            CookMaterialService materialService = ObjectContainer.GetInstance<CookMaterialService>();
            List<CookMaterialTsfer> listMaterial = materialService.GetList(cookBook.CookBookId);
            cookBook.ListMaterial = listMaterial;
            //获取口味
            TasteService tasteService = ObjectContainer.GetInstance<TasteService>();
            TasteTsfer taste = tasteService.Get(cookBook.TasteId);
            if (taste != null)
                cookBook.TasteName = taste.Name;
            //获取分类
            FoodSortService sortService = ObjectContainer.GetInstance<FoodSortService>();
            FoodSortTsfer sort = sortService.Get(cookBook.FoodSortId);
            if (sort != null)
                cookBook.FoodSortName = sort.Name;
            //获取用户姓名
            UserInfoService user = ObjectContainer.GetInstance<UserInfoService>();
            cookBook.UserName = user.Get(cookBook.UserId).Name;
            
            UserInfoTsfer loginUser = System.Web.HttpContext.Current.Session["user"] as UserInfoTsfer;
            if (loginUser != null)
            {
                //判断是否收藏
                CollectionService collService = ObjectContainer.GetInstance<CollectionService>();
                cookBook.IsCollection = collService.IsExist(loginUser.UserId, cookBook.CookBookId);
                //是否点赞
                SupportScanRecordService ssService = ObjectContainer.GetInstance<SupportScanRecordService>();
                cookBook.IsSupport = ssService.IsExist(cookBook.CookBookId, loginUser.UserId, 1);
            }
            

            //获取点赞数量
            SupportScanRecordService ssrService = ObjectContainer.GetInstance<SupportScanRecordService>();
            cookBook.SupportCount = ssrService.GetSupportCount(cookBook.CookBookId);
        }

        public OutputModel GetPageByFoodMaterial(string foodMaterialId,string pageIndex,string pageSize)
        {
            int iFoodMaterialId, index, size,rowCount;
            if (!int.TryParse(foodMaterialId, out iFoodMaterialId))
                return OutputHelper.GetOutputResponse(ResultCode.ErrorParameter);
            CheckParameter.PageCheck(pageIndex, pageSize, out index, out size);
            List<CookBookTsfer> list = service.GetPageByFoodMaterial(iFoodMaterialId, index, size,1, out rowCount);
            if(list.Count==0)
                return OutputHelper.GetOutputResponse(ResultCode.NoData);
            int pageCount = (int)Math.Ceiling(rowCount * 1.0 / size);
            return OutputHelper.GetOutputResponse(ResultCode.OK, new { List = list, PageCount = pageCount });
        }
         public List<CookBookTsfer> GetPage(int pageindex, int pagesize, string status, out int pagecount)
        {
            int rowCount;
            List<CookBookTsfer> list = new List<CookBookTsfer>();
            if (!string.IsNullOrWhiteSpace(status))
            {
                int Status;
                CheckParameter.PageCheck(status, out Status);
                list = service.GetPage(pageindex, pagesize, Status, out rowCount);
            }
            else
                list = service.GetPage(pageindex, pagesize, out rowCount);
            pagecount = (int)Math.Ceiling(rowCount * 1.0 / pagesize);
            foreach (CookBookTsfer item in list)
            {
                //获取口味
                TasteService tasteService = ObjectContainer.GetInstance<TasteService>();
                TasteTsfer taste = tasteService.Get(item.TasteId);
                if (taste != null)
                    item.TasteName = taste.Name;
                //获取分类
                FoodSortService sortService = ObjectContainer.GetInstance<FoodSortService>();
                FoodSortTsfer sort = sortService.Get(item.FoodSortId);
                if (sort != null)
                    item.FoodSortName = sort.Name;
                //获取用户姓名
                UserInfoService user = ObjectContainer.GetInstance<UserInfoService>();
                item.UserName = user.Get(item.UserId).Name;
            }
            return list;
        }

        public OutputModel UpdateStaus(string ids,string status)
        {
            ids = ids.Trim(',');
            if (service.UpdateStaus(ids, status))
                return OutputHelper.GetOutputResponse(ResultCode.OK);
            return OutputHelper.GetOutputResponse(ResultCode.Error);
        }

        public List<CookBookTsfer> GetHottest(int topNum)
        {
            return  service.GetListByHottest(topNum);
        }

        public List<CookBookTsfer> GetRecent(int num)
        {
            return service.GetListRecent(num);
        }

        public OutputModel Delete(string id)
        {
          CookBookTsfer c=  service.Get(id);
          if (c == null)
              return OutputHelper.GetOutputResponse(ResultCode.NoData);
            if (service.Delete(c))
                return OutputHelper.GetOutputResponse(ResultCode.OK);
            return OutputHelper.GetOutputResponse(ResultCode.Error);
        }
    }

}