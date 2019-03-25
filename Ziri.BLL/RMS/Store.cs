using System;
using System.Collections.Generic;
using System.Linq;
using Ziri.MDL;
using Ziri.DAL;
using Ziri.BLL.SYS;
using Ziri.BLL.LIST;
using System.Transactions;

namespace Ziri.BLL.RMS
{
    public class Store
    {
        //模块定义
        public static ModuleInfo ModuleInfo = new ModuleInfo { Name = "Store", Title = "门店", IconFont = "fa fa-store-alt", Enabled = true };
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
        public static void InitStoreInfo(out AlertMessage alertMessage)
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
                var delCount = EF.Database.ExecuteSqlCommand("TRUNCATE TABLE StoreInfo;");

                //门店
                var storeInfos = new List<StoreInfo> {
                    new StoreInfo{ Name = "DBD", Title = "滨海新区店", Enabled = true },
                    new StoreInfo{ Name = "BOTON", Title="马家龙波顿店", Enabled = true },
                    new StoreInfo{ Name = "MMVTC", Title = "茂名建专店", Enabled = true },
                    new StoreInfo{ Name = "CHNSKIN", Title = "江夏瓷肌店", Enabled = true },
                    new StoreInfo{ Name = "VICTOR", Title = "龙胜维可陶店", Enabled = true },
                    new StoreInfo{ Name = "SCUT", Title = "五山华工店", Enabled = true },
                    new StoreInfo{ Name = "AUSTSHORE", Title = "科苑澳海岸店", Enabled = true },
                    new StoreInfo{ Name = "GUOANJU", Title = "民治国安居店", Enabled = true },
                    new StoreInfo{ Name = "YHJ", Title = "西丽一号机店", Enabled = true },
                    new StoreInfo{ Name = "OODUU", Title = "华强北九方店", Enabled = true },
                };
                EF.StoreInfos.AddRange(storeInfos);
                EF.SaveChanges();

                foreach (var storeInfo in storeInfos)
                {
                    ContactInfo contactInfo = null;
                    switch (storeInfo.Name)
                    {
                        case "DBD": contactInfo = new ContactInfo { EMail = null, Phone = null, Address = "广东省茂名市电白区电城镇", Latitude = 21.515M, Longitude = 111.2974M, }; break;
                        case "BOTON": contactInfo = new ContactInfo { EMail = null, Phone = null, Address = "广东省深圳市南山区马家龙", Latitude = 22.550205M, Longitude = 113.926M, }; break;
                        case "MMVTC": contactInfo = new ContactInfo { EMail = null, Phone = null, Address = "广东省茂名市建设中专", Latitude = 21.6745M, Longitude = 110.933M, }; break;
                        case "CHNSKIN": contactInfo = new ContactInfo { EMail = null, Phone = null, Address = "广东省广州市白云区江厦村", Latitude = 23.209616M, Longitude = 113.272667M, }; break;
                        case "VICTOR": contactInfo = new ContactInfo { EMail = null, Phone = null, Address = "广东省深圳市龙华区赤岭头村", Latitude = 22.648M, Longitude = 114.01M, }; break;
                        case "SCUT": contactInfo = new ContactInfo { EMail = null, Phone = null, Address = "广东省广州市天河区五山路", Latitude = 23.156593M, Longitude = 113.34314M, }; break;
                        case "AUSTSHORE": contactInfo = new ContactInfo { EMail = null, Phone = null, Address = "广东省深圳市南山区中地大楼", Latitude = 22.52894M, Longitude = 113.944717M, }; break;
                        case "GUOANJU": contactInfo = new ContactInfo { EMail = null, Phone = null, Address = "广东省深圳市龙华区国安居", Latitude = 22.62M, Longitude = 114.034M, }; break;
                        case "YHJ": contactInfo = new ContactInfo { EMail = null, Phone = null, Address = "广东省深圳市南山区健兴科技大厦", Latitude = 22.569059M, Longitude = 113.954163M, }; break;
                        case "OODUU": contactInfo = new ContactInfo { EMail = null, Phone = null, Address = "深圳市福田区九方购物中心", Latitude = 22.542381M, Longitude = 114.0823M, }; break;
                    }
                    EF.ContactInfos.Add(contactInfo);
                    EF.SaveChanges();
                    EF.StoreContacts.Add(new StoreContact { StoreID = storeInfo.ID, ContactID = contactInfo.ID, });
                }
                EF.SaveChanges();
            }
        }

        //更新信息
        public static StoreInfo StoreInfoUpload(StoreInfo storeInfo, ContactInfo contactInfo
            , StorePhoto storePhoto, StoreDesc storeDesc, out AlertMessage alertMessage)
        {
            //权限检查
            if (!AUTH.PermissionCheck(ModuleInfo, ActionInfos.Where(i => i.Name == (storeInfo.ID == 0 ? "Create" : "Modify")).FirstOrDefault(), out string Message))
            {
                alertMessage = new AlertMessage { Message = Message, Type = AlertType.warning };
                return null;
            }

            //表单检查
            if (string.IsNullOrWhiteSpace(storeInfo.Name))
            {
                alertMessage = new AlertMessage { Message = "门店代码不能为空。", Type = AlertType.warning };
                return null;
            }
            if (string.IsNullOrWhiteSpace(storeInfo.Title))
            {
                alertMessage = new AlertMessage { Message = "门店名称不能为空。", Type = AlertType.warning };
                return null;
            }

            using (var EF = new EF())
            {
                //修改是否存在？代码、名称唯一？
                StoreInfo store_exist = null;
                StoreInfo store_name_exist = null;
                StoreInfo store_title_exist = null;
                if (storeInfo.ID == 0)
                {
                    store_name_exist = EF.StoreInfos.Where(i => i.Name == storeInfo.Name).FirstOrDefault();
                    store_title_exist = EF.StoreInfos.Where(i => i.Title == storeInfo.Title).FirstOrDefault();
                }
                else
                {
                    store_exist = EF.StoreInfos.Where(i => i.ID == storeInfo.ID).FirstOrDefault();
                    if (store_exist == null)
                    {
                        alertMessage = new AlertMessage { Message = string.Format("门店编号[{0}]不存在。", storeInfo.ID), Type = AlertType.warning };
                        return null;
                    }
                    store_name_exist = EF.StoreInfos.Where(i => i.ID != storeInfo.ID && i.Name == storeInfo.Name).FirstOrDefault();
                    store_title_exist = EF.StoreInfos.Where(i => i.ID != storeInfo.ID && i.Title == storeInfo.Title).FirstOrDefault();
                }
                if (store_name_exist != null && store_name_exist.ID > 0)
                {
                    alertMessage = new AlertMessage { Message = string.Format("门店代码[{0}]已被ID[{1}]使用。", storeInfo.Name, store_name_exist.ID), Type = AlertType.warning };
                    return null;
                }
                if (store_title_exist != null && store_title_exist.ID > 0)
                {
                    alertMessage = new AlertMessage { Message = string.Format("门店名称[{0}]已被ID[{1}]使用。", storeInfo.Title, store_title_exist.ID), Type = AlertType.warning };
                    return null;
                }

                //数据保存
                using (TransactionScope TS = new TransactionScope())
                {
                    //店名
                    if (storeInfo.ID == 0) { store_exist = EF.StoreInfos.Add(new StoreInfo { Enabled = true, }); }
                    store_exist.Name = storeInfo.Name;
                    store_exist.Title = storeInfo.Title;
                    store_exist.LogoFileID = storeInfo.LogoFileID;
                    EF.SaveChanges();
                    //联系信息
                    var contactInfo_exist = EF.ContactInfos.Where(i => i.EMail == contactInfo.EMail && i.Phone == contactInfo.Phone
                        && i.Address == contactInfo.Address
                        && i.Latitude == contactInfo.Latitude
                        && i.Longitude == contactInfo.Longitude).FirstOrDefault();
                    if (contactInfo_exist == null)
                    {
                        EF.ContactInfos.Add(contactInfo_exist = contactInfo);
                        EF.SaveChanges();
                    }
                    var storeContact_exist = EF.StoreContacts.Where(i => i.StoreID == store_exist.ID).FirstOrDefault();
                    if (storeContact_exist == null)
                    {
                        EF.StoreContacts.Add(storeContact_exist = new StoreContact
                        {
                            StoreID = store_exist.ID,
                            ContactID = contactInfo_exist.ID,
                        });
                    }
                    else
                    {
                        storeContact_exist.ContactID = contactInfo_exist.ID;
                    }
                    //图文
                    var photo_exist = EF.StorePhotos.Where(i => i.StoreID == store_exist.ID).FirstOrDefault();
                    if (photo_exist == null) { EF.StorePhotos.Add(photo_exist = new StorePhoto { StoreID = store_exist.ID, }); }
                    photo_exist.FileIDs = storePhoto.FileIDs;
                    var desc_exist = EF.StoreDescs.Where(i => i.StoreID == store_exist.ID).FirstOrDefault();
                    if (desc_exist == null) { EF.StoreDescs.Add(desc_exist = new StoreDesc { StoreID = store_exist.ID, }); }
                    desc_exist.BusinessHours = storeDesc.BusinessHours;
                    desc_exist.Description = storeDesc.Description;

                    //保存
                    EF.SaveChanges();
                    TS.Complete();

                }
                //更新完成
                alertMessage = null;
                return store_exist;
            }
        }

        //获取列表
        public static List<StoreFullInfo> GetStoreInfos(List<ListFilterField> FilterField, List<ListOrderField> OrderField
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
                IEnumerable<StoreFullInfo> DataList = (from storeInfo in EF.StoreInfos
                                                       join fileInfo in EF.FileInfos on storeInfo.LogoFileID equals fileInfo.ID into temp1
                                                       from fileInfo in temp1.DefaultIfEmpty()
                                                       join fileExtName in EF.FileExtName on fileInfo.ExtNameID equals fileExtName.ID into temp2
                                                       from fileExtName in temp2.DefaultIfEmpty()
                                                       select new StoreFullInfo
                                                       {
                                                           ID = storeInfo.ID,
                                                           Name = storeInfo.Name,
                                                           Title = storeInfo.Title,
                                                           Enabled = storeInfo.Enabled,
                                                           LogoFileID = storeInfo.LogoFileID,
                                                           FileGUID = fileInfo.GUID,
                                                           FileName = fileInfo.Name,
                                                           FileExtName = fileExtName.Name,
                                                       });

                //筛选
                foreach (var item in FilterField)
                {
                    if (item.Name == "NameAndTitle" && item.Value.Count > 0)
                    {
                        var predicate = PredicateExtensions.False<StoreFullInfo>();    //设置为False，所有and条件都应该放在or之后，如where (type=1 or type=14) and status==0
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
        public static StoreFullInfo GetStoreFullInfo(long StoreID)
        {
            using (var EF = new EF())
            {
                return (from storeInfo in EF.StoreInfos
                        join fileInfo in EF.FileInfos on storeInfo.LogoFileID equals fileInfo.ID into temp1
                        from fileInfo in temp1.DefaultIfEmpty()
                        join fileExtName in EF.FileExtName on fileInfo.ExtNameID equals fileExtName.ID into temp2
                        from fileExtName in temp2.DefaultIfEmpty()
                        where storeInfo.ID == StoreID
                        select new StoreFullInfo
                        {
                            ID = storeInfo.ID,
                            Name = storeInfo.Name,
                            Title = storeInfo.Title,
                            Enabled = storeInfo.Enabled,
                            LogoFileID = storeInfo.LogoFileID,
                            FileGUID = fileInfo.GUID,
                            FileName = fileInfo.Name,
                            FileExtName = fileExtName.Name,
                        }).FirstOrDefault();
            }
        }

        public static List<StoreFullInfo> GetStoreInfos(out AlertMessage alertMessage, bool Enabled = false)
        {
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
                var list = from storeInfo in EF.StoreInfos
                           join fileInfo in EF.FileInfos on storeInfo.LogoFileID equals fileInfo.ID into temp1
                           from fileInfo in temp1.DefaultIfEmpty()
                           join fileExtName in EF.FileExtName on fileInfo.ExtNameID equals fileExtName.ID into temp2
                           from fileExtName in temp2.DefaultIfEmpty()
                           select new StoreFullInfo
                           {
                               ID = storeInfo.ID,
                               Name = storeInfo.Name,
                               Title = storeInfo.Title,
                               Enabled = storeInfo.Enabled,
                               LogoFileID = storeInfo.LogoFileID,
                               FileGUID = fileInfo.GUID,
                               FileName = fileInfo.Name,
                               FileExtName = fileExtName.Name,
                           };
                if (Enabled)
                {
                    return list.Where(i => i.Enabled == true).ToList();
                }
                return list.ToList();
            }
        }

        //获取修改
        public static StoreDetails GetModifyInfo(long StoreID, out AlertMessage alertMessage)
        {
            alertMessage = null;

            //权限检查
            if (!AUTH.PermissionCheck(ModuleInfo, ActionInfos.Where(i => i.Name == (StoreID == 0 ? "Create" : "Modify")).FirstOrDefault(), out string Message))
            {
                alertMessage = new AlertMessage { Message = Message, Type = AlertType.warning };
                return null;
            }

            return GetStoreDetails(StoreID);
        }

        //获取详情
        public static StoreInfo GetDetailsInfo(long StoreID, out AlertMessage alertMessage)
        {
            alertMessage = null;

            //权限检查
            if (!AUTH.PermissionCheck(ModuleInfo, ActionInfos.Where(i => i.Name == "Details").FirstOrDefault(), out string Message))
            {
                alertMessage = new AlertMessage { Message = Message, Type = AlertType.warning };
                return null;
            }

            return GetStoreInfo(StoreID);
        }

        //获取信息
        private static StoreInfo GetStoreInfo(long StoreID)
        {
            using (var EF = new EF())
            {
                return EF.StoreInfos.Where(i => i.ID == StoreID).FirstOrDefault();
            }
        }

        //获取详情
        private static StoreDetails GetStoreDetails(long StoreID)
        {
            using (var EF = new EF())
            {
                var details = (from storeInfo in EF.StoreInfos
                               join storePhoto in EF.StorePhotos on storeInfo.ID equals storePhoto.StoreID into temp1
                               from storePhoto in temp1.DefaultIfEmpty()
                               join storeDesc in EF.StoreDescs on storeInfo.ID equals storeDesc.StoreID into temp2
                               from storeDesc in temp2.DefaultIfEmpty()
                               where storeInfo.ID == StoreID
                               select new StoreDetails
                               {
                                   StoreFullInfo = new StoreFullInfo
                                   {
                                       ID = storeInfo.ID,
                                       LogoFileID = storeInfo.LogoFileID,
                                       Name = storeInfo.Name,
                                       Title = storeInfo.Title,
                                       Enabled = storeInfo.Enabled,
                                   },
                                   PhotoFileIDs = storePhoto.FileIDs,
                                   BusinessHours = storeDesc.BusinessHours,
                                   Description = storeDesc.Description,
                               }).FirstOrDefault();

                var store_exist = EF.StoreInfos.Where(i => i.ID == StoreID).FirstOrDefault();
                if (store_exist == null) { return null; }

                //LOGO
                if (store_exist.LogoFileID > 0)
                {
                    var fileInfo = DOC.GetFileUploadInfo(store_exist.LogoFileID);
                    if (fileInfo != null)
                    {
                        details.StoreFullInfo.FileGUID = fileInfo.FileInfo.GUID;
                        details.StoreFullInfo.FileName = fileInfo.FileInfo.Name;
                        details.StoreFullInfo.FileExtName = fileInfo.FileExtName.Name;
                    }
                }
                //联系信息
                details.StoreContact = EF.StoreContacts.Where(i => i.StoreID == StoreID).FirstOrDefault();
                if (details.StoreContact != null)
                {
                    details.ContactInfo = EF.ContactInfos.Where(i => i.ID == details.StoreContact.ContactID).FirstOrDefault();
                }
                //图片
                details.PhotoUploadInfos = new List<FileUploadInfo>();
                if (!string.IsNullOrWhiteSpace(details.PhotoFileIDs))
                {
                    foreach (var FileID in details.PhotoFileIDs.Split(','))
                    {
                        details.PhotoUploadInfos.Add(DOC.GetFileUploadInfo(long.Parse(FileID)));
                    }
                }

                //结果
                return details;
            }
        }

        //设置状态
        public static void SetStoreEnabled(long StoreID, bool Enabled, out AlertMessage alertMessage)
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
                var StoreInfo = EF.StoreInfos.Where(i => i.ID == StoreID).FirstOrDefault();
                StoreInfo.Enabled = Enabled;
                EF.SaveChanges();

                alertMessage = new AlertMessage { Message = "门店[" + StoreInfo.Title + "]" + (Enabled ? "启用" : "禁用") + "成功！", Type = AlertType.success };
            }
        }
    }
}
