using System.Collections.Generic;
using System.Linq;
using Ziri.MDL;
using Ziri.DAL;
using Ziri.BLL.SYS;
using Ziri.BLL.LIST;
using System;

namespace Ziri.BLL.ITEM
{
    public class Brand
    {
        //模块定义
        public static ModuleInfo ModuleInfo = new ModuleInfo { Name = "Brand", Title = "品牌", IconFont = "fa fa-th-large", Enabled = true };
        public static List<ActionInfo> ActionInfos = new List<ActionInfo> {
            new ActionInfo{ Name = "Init", Title = "初始化", IconFont = "" , Enabled = true},
            new ActionInfo{ Name = "List", Title = "列表", IconFont = "", Enabled = true },
            new ActionInfo{ Name = "Details", Title = "详情", IconFont = "", Enabled = true },
            new ActionInfo{ Name = "Create", Title = "创建", IconFont = "", Enabled = true },
            new ActionInfo{ Name = "Modify", Title = "修改", IconFont = "", Enabled = true },
            new ActionInfo{ Name = "Enabled", Title = "启用", IconFont = "" , Enabled = true},
            new ActionInfo{ Name = "Disabled", Title = "禁用", IconFont = "" , Enabled = true},
        };

        //初始化信息
        public static void InitBrandInfo(out AlertMessage alertMessage)
        {
            alertMessage = null;

            //权限检查
            if (!AUTH.PermissionCheck(ModuleInfo, ActionInfos.Where(i => i.Name == "Init").FirstOrDefault(), out string Message))
            {
                alertMessage = new AlertMessage { Message = Message, Type = AlertType.warning };
                return;
            }

            using (var EF = new EF())
            {
                //清除数据表
                var delCount = EF.Database.ExecuteSqlCommand("TRUNCATE TABLE BrandInfo;");

                //品牌
                var brandInfos = new List<BrandInfo> {
                    new BrandInfo{ Name = "BENQ", Title="明基", Enabled = true },
                    new BrandInfo{ Name = "NEC", Title = "日电", Enabled = true },
                    new BrandInfo{ Name = "VICTOR", Title = "维可陶", Enabled = true },
                    new BrandInfo{ Name = "Yarrawood", Title = "雅拉德", Enabled = true },
                    new BrandInfo{ Name = "McPherson", Title = "麦克菲森", Enabled = true },
                    new BrandInfo{ Name = "Henschke", Title = "翰斯科", Enabled = true },
                    new BrandInfo{ Name = "Penfolds", Title = "奔富", Enabled = true },
                };
                EF.BrandInfos.AddRange(brandInfos);
                EF.SaveChanges();
            }
        }

        //更新信息
        public static BrandInfo BrandInfoUpload(BrandInfo brandInfo, out AlertMessage alertMessage)
        {
            //权限检查
            if (!AUTH.PermissionCheck(ModuleInfo, ActionInfos.Where(i => i.Name == (brandInfo.ID == 0 ? "Create" : "Modify")).FirstOrDefault(), out string Message))
            {
                alertMessage = new AlertMessage { Message = Message, Type = AlertType.warning };
                return null;
            }

            //表单检查
            if (string.IsNullOrWhiteSpace(brandInfo.Name))
            {
                alertMessage = new AlertMessage { Message = "品牌代码不能为空。", Type = AlertType.warning };
                return null;
            }
            if (string.IsNullOrWhiteSpace(brandInfo.Title))
            {
                alertMessage = new AlertMessage { Message = "品牌名称不能为空。", Type = AlertType.warning };
                return null;
            }

            using (var EF = new EF())
            {
                //修改是否存在？代码、名称唯一？
                BrandInfo brand_exist = null;
                BrandInfo brand_name_exist = null;
                BrandInfo brand_title_exist = null;
                if (brandInfo.ID == 0)
                {
                    brand_name_exist = EF.BrandInfos.Where(i => i.Name == brandInfo.Name).FirstOrDefault();
                    brand_title_exist = EF.BrandInfos.Where(i => i.Title == brandInfo.Title).FirstOrDefault();
                }
                else
                {
                    brand_exist = EF.BrandInfos.Where(i => i.ID == brandInfo.ID).FirstOrDefault();
                    if (brand_exist == null)
                    {
                        alertMessage = new AlertMessage { Message = string.Format("品牌编号[{0}]不存在。", brandInfo.ID), Type = AlertType.warning };
                        return null;
                    }
                    brand_name_exist = EF.BrandInfos.Where(i => i.ID != brandInfo.ID && i.Name == brandInfo.Name).FirstOrDefault();
                    brand_title_exist = EF.BrandInfos.Where(i => i.ID != brandInfo.ID && i.Title == brandInfo.Title).FirstOrDefault();
                }
                if (brand_name_exist != null && brand_name_exist.ID > 0)
                {
                    alertMessage = new AlertMessage { Message = string.Format("品牌代码[{0}]已被ID[{1}]使用。", brandInfo.Name, brand_name_exist.ID), Type = AlertType.warning };
                    return null;
                }
                if (brand_title_exist != null && brand_title_exist.ID > 0)
                {
                    alertMessage = new AlertMessage { Message = string.Format("品牌名称[{0}]已被ID[{1}]使用。", brandInfo.Title, brand_title_exist.ID), Type = AlertType.warning };
                    return null;
                }

                //数据保存
                if (brandInfo.ID == 0) { brand_exist = EF.BrandInfos.Add(new BrandInfo { Enabled = true, }); }
                brand_exist.Name = brandInfo.Name;
                brand_exist.Title = brandInfo.Title;
                brand_exist.LogoFileID = brandInfo.LogoFileID;
                brand_exist.BannerFileID = brandInfo.BannerFileID;
                EF.SaveChanges();

                //更新完成
                alertMessage = null;
                return brand_exist;
            }
        }

        //获取列表
        public static List<BrandFullInfo> GetBrandInfos(List<ListFilterField> FilterField, List<ListOrderField> OrderField
            , int PageSize, int PageIndex, out long TotalRowCount, out AlertMessage alertMessage)
        {
            TotalRowCount = 0;
            alertMessage = null;

            //权限检查
            if (!AUTH.PermissionCheck(ModuleInfo, ActionInfos.Where(i => i.Name == "List").FirstOrDefault(), out string Message))
            {
                alertMessage = new AlertMessage { Message = Message, Type = AlertType.warning };
                return null;
            }

            //查询列表
            using (var EF = new EF())
            {
                IEnumerable<BrandFullInfo> DataList = (from brandInfo in EF.BrandInfos
                                                       join logoFileInfo in EF.FileInfos on brandInfo.LogoFileID equals logoFileInfo.ID into temp1
                                                       from logoFileInfo in temp1.DefaultIfEmpty()
                                                       join logoFileExtName in EF.FileExtName on logoFileInfo.ExtNameID equals logoFileExtName.ID into temp2
                                                       from logoFileExtName in temp2.DefaultIfEmpty()
                                                       join brandFileInfo in EF.FileInfos on brandInfo.BannerFileID equals brandFileInfo.ID into temp3
                                                       from brandFileInfo in temp3.DefaultIfEmpty()
                                                       join brandFileExtName in EF.FileExtName on brandFileInfo.ExtNameID equals brandFileExtName.ID into temp4
                                                       from brandFileExtName in temp4.DefaultIfEmpty()
                                                       select new BrandFullInfo
                                                       {
                                                           ID = brandInfo.ID,
                                                           Name = brandInfo.Name,
                                                           Title = brandInfo.Title,
                                                           Enabled = brandInfo.Enabled,
                                                           LogoFileID = brandInfo.LogoFileID,
                                                           LogoFileGUID = logoFileInfo.GUID,
                                                           LogoFileName = logoFileInfo.Name,
                                                           LogoFileExtName = logoFileExtName.Name,
                                                           BrandFileID = brandInfo.BannerFileID,
                                                           BrandFileGUID = brandFileInfo.GUID,
                                                           BrandFileName = brandFileInfo.Name,
                                                           BrandFileExtName = brandFileExtName.Name,
                                                       });

                //筛选
                foreach (var item in FilterField)
                {
                    if (item.Name == "NameAndTitle" && item.Value.Count > 0)
                    {
                        var predicate = PredicateExtensions.False<BrandFullInfo>();    //设置为False，所有and条件都应该放在or之后，如where (type=1 or type=14) and status==0
                        foreach (var t in item.Value)
                        {
                            var KWPart = t.ToLower();
                            switch (item.CmpareMode)
                            {
                                case FilterCmpareMode.Equal:
                                    predicate = predicate.Or(p => p.Name.ToLower() == KWPart || p.Title.ToLower() == KWPart);
                                    break;
                                case FilterCmpareMode.Like:
                                    predicate = predicate.Or(p => p.Name.ToLower().Contains(KWPart) || p.Title.ToLower().Contains(KWPart));
                                    break;
                            }
                        }
                        DataList = DataList.Where(predicate.Compile());
                    }
                }

                //排序
                if (OrderField.Count == 0) { DataList = DataList.OrderByDescending(i => i.ID); }
                else
                {
                    foreach (var item in OrderField)
                    {
                        switch (item.Mode)
                        {
                            case OrderByMode.Asc:
                                DataList = from list in DataList
                                           orderby OrderBy.GetPropertyValue(list, item.Name)
                                           select list;
                                break;
                            case OrderByMode.Desc:
                                DataList = from list in DataList
                                           orderby OrderBy.GetPropertyValue(list, item.Name) descending
                                           select list;
                                break;
                        }
                    }
                }

                //分页
                TotalRowCount = DataList.Count();
                if (TotalRowCount == 0) { return null; }
                int PageCount = (int)Math.Ceiling((double)TotalRowCount / PageSize);
                if (PageIndex > PageCount) { PageIndex = PageCount; }
                return DataList.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
            }
        }

        //获取详细信息
        public static BrandFullInfo GetBrandFullInfo(long BrandID)
        {
            using (var EF = new EF())
            {
                return (from brandInfo in EF.BrandInfos
                        join logoFileInfo in EF.FileInfos on brandInfo.LogoFileID equals logoFileInfo.ID into temp1
                        from logoFileInfo in temp1.DefaultIfEmpty()
                        join logoFileExtName in EF.FileExtName on logoFileInfo.ExtNameID equals logoFileExtName.ID into temp2
                        from logoFileExtName in temp2.DefaultIfEmpty()
                        join brandFileInfo in EF.FileInfos on brandInfo.BannerFileID equals brandFileInfo.ID into temp3
                        from brandFileInfo in temp3.DefaultIfEmpty()
                        join brandFileExtName in EF.FileExtName on brandFileInfo.ExtNameID equals brandFileExtName.ID into temp4
                        from brandFileExtName in temp4.DefaultIfEmpty()
                        where brandInfo.ID == BrandID
                        select new BrandFullInfo
                        {
                            ID = brandInfo.ID,
                            Name = brandInfo.Name,
                            Title = brandInfo.Title,
                            Enabled = brandInfo.Enabled,
                            LogoFileID = brandInfo.LogoFileID,
                            LogoFileGUID = logoFileInfo.GUID,
                            LogoFileName = logoFileInfo.Name,
                            LogoFileExtName = logoFileExtName.Name,
                            BrandFileID = brandInfo.BannerFileID,
                            BrandFileGUID = brandFileInfo.GUID,
                            BrandFileName = brandFileInfo.Name,
                            BrandFileExtName = brandFileExtName.Name,
                        }).FirstOrDefault();
            }
        }

        public static List<BrandFullInfo> GetListBrandFullInfo(out AlertMessage alertMessage, bool Enabled = false)
        {
            alertMessage = null;

            //权限检查
            if (!AUTH.PermissionCheck(ModuleInfo, ActionInfos.Where(i => i.Name == "List").FirstOrDefault(), out string Message))
            {
                alertMessage = new AlertMessage { Message = Message, Type = AlertType.warning };
                return null;
            }

            //查询列表
            return GetBrandInfos(Enabled);
        }

        public static List<BrandFullInfo> GetBrandInfos(bool Enabled = false)
        {
            using (var EF = new EF())
            {
                var list = from brandInfo in EF.BrandInfos
                           join logoFileInfo in EF.FileInfos on brandInfo.LogoFileID equals logoFileInfo.ID into temp1
                           from logoFileInfo in temp1.DefaultIfEmpty()
                           join logoFileExtName in EF.FileExtName on logoFileInfo.ExtNameID equals logoFileExtName.ID into temp2
                           from logoFileExtName in temp2.DefaultIfEmpty()
                           join brandFileInfo in EF.FileInfos on brandInfo.BannerFileID equals brandFileInfo.ID into temp3
                           from brandFileInfo in temp3.DefaultIfEmpty()
                           join brandFileExtName in EF.FileExtName on brandFileInfo.ExtNameID equals brandFileExtName.ID into temp4
                           from brandFileExtName in temp4.DefaultIfEmpty()
                           select new BrandFullInfo
                           {
                               ID = brandInfo.ID,
                               Name = brandInfo.Name,
                               Title = brandInfo.Title,
                               Enabled = brandInfo.Enabled,
                               LogoFileID = brandInfo.LogoFileID,
                               LogoFileGUID = logoFileInfo.GUID,
                               LogoFileName = logoFileInfo.Name,
                               LogoFileExtName = logoFileExtName.Name,
                               BrandFileID = brandInfo.BannerFileID,
                               BrandFileGUID = brandFileInfo.GUID,
                               BrandFileName = brandFileInfo.Name,
                               BrandFileExtName = brandFileExtName.Name,
                           };
                if (Enabled)
                {
                    return list.Where(i => i.Enabled == true).ToList();
                }
                return list.ToList();
            }
        }

        //获取修改
        public static BrandInfo GetModifyInfo(long BrandID, out AlertMessage alertMessage)
        {
            alertMessage = null;

            //权限检查
            if (!AUTH.PermissionCheck(ModuleInfo, ActionInfos.Where(i => i.Name == (BrandID == 0 ? "Create" : "Modify")).FirstOrDefault(), out string Message))
            {
                alertMessage = new AlertMessage { Message = Message, Type = AlertType.warning };
                return null;
            }

            return GetBrandInfo(BrandID);
        }

        //获取详情
        public static BrandInfo GetDetailsInfo(long BrandID, out AlertMessage alertMessage)
        {
            alertMessage = null;

            //权限检查
            if (!AUTH.PermissionCheck(ModuleInfo, ActionInfos.Where(i => i.Name == "Details").FirstOrDefault(), out string Message))
            {
                alertMessage = new AlertMessage { Message = Message, Type = AlertType.warning };
                return null;
            }

            return GetBrandInfo(BrandID);
        }

        //获取信息
        private static BrandInfo GetBrandInfo(long BrandID)
        {
            using (var EF = new EF())
            {
                return EF.BrandInfos.Where(i => i.ID == BrandID).FirstOrDefault();
            }
        }

        //获取品牌的商品列表
        public static List<GoodsInfo> GetGoodsInfos(long BrandID)
        {
            using (var EF = new EF())
            {
                return (from brandInfo in EF.BrandInfos
                        join goodsBrand in EF.GoodsBrands on brandInfo.ID equals goodsBrand.BrandID
                        join goodsInfo in EF.GoodsInfos on goodsBrand.GoodsID equals goodsInfo.ID
                        where brandInfo.ID == BrandID
                        select goodsInfo
                        ).ToList();
            }
        }

        //设置状态
        public static void SetBrandEnabled(long BrandID, bool Enabled, out AlertMessage alertMessage)
        {
            alertMessage = null;

            //权限检查
            if (!AUTH.PermissionCheck(ModuleInfo, ActionInfos.Where(i => i.Name == (Enabled ? "Enabled" : "Disabled")).FirstOrDefault(), out string Message))
            {
                alertMessage = new AlertMessage { Message = Message, Type = AlertType.warning };
                return;
            }

            using (var EF = new EF())
            {
                var brandInfo = EF.BrandInfos.Where(i => i.ID == BrandID).FirstOrDefault();
                brandInfo.Enabled = Enabled;
                EF.SaveChanges();

                alertMessage = new AlertMessage { Message = "品牌[" + brandInfo.Title + "]" + (Enabled ? "启用" : "禁用") + "成功！", Type = AlertType.success };
            }
        }
    }
}
