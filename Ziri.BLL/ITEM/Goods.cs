using System;
using System.Linq;
using System.Transactions;
using System.Collections.Generic;
using Ziri.BLL.SYS;
using Ziri.BLL.LIST;
using Ziri.MDL;
using Ziri.DAL;

namespace Ziri.BLL.ITEM
{
    public class Goods
    {
        //模块定义
        public static ModuleInfo ModuleInfo = new ModuleInfo { Name = "Goods", Title = "商品", IconFont = "fa fa-th-large", Enabled = true };
        public static List<ActionInfo> ActionInfos = new List<ActionInfo> {
            new ActionInfo{ Name = "List", Title = "列表", IconFont = "", Enabled = true },
            new ActionInfo{ Name = "Details", Title = "详情", IconFont = "", Enabled = true },
            new ActionInfo{ Name = "Create", Title = "创建", IconFont = "", Enabled = true },
            new ActionInfo{ Name = "Modify", Title = "修改", IconFont = "", Enabled = true },
            new ActionInfo{ Name = "PublicSales", Title = "上架", IconFont = "", Enabled = true },
            new ActionInfo{ Name = "StopSales", Title = "下架", IconFont = "", Enabled = true },
            new ActionInfo{ Name = "Specs", Title = "规格", IconFont = "", Enabled = true },
        };

        //初始化信息
        public static void InitGoodsInfo(out AlertMessage alertMessage)
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
                var delCount = EF.Database.ExecuteSqlCommand(@"
                        TRUNCATE TABLE GoodsInfo;
                        TRUNCATE TABLE GoodsBrand;
                        TRUNCATE TABLE GoodsSpec;
                        TRUNCATE TABLE GoodsPhoto;
                        TRUNCATE TABLE GoodsDesc;
                    ");
                var goodsInfos = new List<GoodsInfo> {
                    new GoodsInfo{  Name = "GOODS0001", Title = "测试商品一", StateID = (int)GoodsState.上架, Enabled = true },
                    new GoodsInfo{  Name = "GOODS0002", Title = "测试商品二", StateID = (int)GoodsState.上架, Enabled = true },
                };
                EF.GoodsInfos.AddRange(goodsInfos);
                EF.SaveChanges();
            }
        }

        //获取列表
        public static List<GoodsInfoList> GetGoodsInfos(List<ListFilterField> FilterField, List<ListOrderField> OrderField
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
                IEnumerable<GoodsInfoList> DataList = from goodsInfos in EF.GoodsInfos
                                                      select new GoodsInfoList
                                                      {
                                                          ID = goodsInfos.ID,
                                                          Name = goodsInfos.Name,
                                                          Title = goodsInfos.Title,
                                                          Enabled = goodsInfos.Enabled,
                                                          StateID = goodsInfos.StateID,
                                                      };

                //筛选
                foreach (var item in FilterField)
                {
                    if (item.Name == "NameAndTitle" && item.Value.Count > 0)
                    {
                        var predicate = PredicateExtensions.False<GoodsInfoList>();    //设置为False，所有and条件都应该放在or之后，如where (type=1 or type=14) and status==0
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

        //更新信息
        public static GoodsInfo GoodsInfoUpload(GoodsInfo goodsInfo, GoodsBrand goodsBrand, List<GoodsKind> goodsKinds, List<GoodsTag> goodsTags
            , GoodsPhoto goodsPhoto, GoodsDesc goodsDesc, List<GoodsSpecFull> goodsSpecsFull, GoodsCounter goodsCounter, out AlertMessage alertMessage)
        {
            //权限检查
            if (!AUTH.PermissionCheck(ModuleInfo, ActionInfos.Where(i => i.Name == (goodsInfo.ID == 0 ? "Create" : "Modify")).FirstOrDefault(), out string Message))
            {
                alertMessage = new AlertMessage { Message = Message, Type = AlertType.warning };
                return null;
            }

            //表单检查
            if (string.IsNullOrWhiteSpace(goodsInfo.Name))
            {
                alertMessage = new AlertMessage { Message = "商品代码不能为空。", Type = AlertType.warning };
                return null;
            }
            if (string.IsNullOrWhiteSpace(goodsInfo.Title))
            {
                alertMessage = new AlertMessage { Message = "商品名称不能为空。", Type = AlertType.warning };
                return null;
            }

            using (var EF = new EF())
            {
                //修改是否存在？商品代码唯一？
                GoodsInfo goods_exist = null;
                GoodsInfo goods_name_exist = null;
                if (goodsInfo.ID == 0)
                {
                    goods_name_exist = EF.GoodsInfos.Where(i => i.Name == goodsInfo.Name).FirstOrDefault();
                }
                else
                {
                    goods_exist = EF.GoodsInfos.Where(i => i.ID == goodsInfo.ID).FirstOrDefault();
                    if (goods_exist == null)
                    {
                        alertMessage = new AlertMessage { Message = string.Format("商品编号[{0}]不存在。", goodsInfo.ID), Type = AlertType.warning };
                        return null;
                    }
                    goods_name_exist = EF.GoodsInfos.Where(i => i.ID != goodsInfo.ID && i.Name == goodsInfo.Name).FirstOrDefault();
                }
                if (goods_name_exist != null && goods_name_exist.ID > 0)
                {
                    alertMessage = new AlertMessage { Message = string.Format("商品代码[{0}]已被ID[{1}]使用。", goodsInfo.Name, goods_name_exist.ID), Type = AlertType.warning };
                    return null;
                }

                //数据保存
                using (TransactionScope TS = new TransactionScope())
                {
                    if (goodsInfo.ID == 0) { goods_exist = EF.GoodsInfos.Add(new GoodsInfo { Enabled = true, StateID = (int)GoodsState.上架, }); }
                    goods_exist.Name = goodsInfo.Name;
                    goods_exist.Title = goodsInfo.Title;
                    EF.SaveChanges();

                    //品牌
                    var brand_exist = EF.GoodsBrands.Where(i => i.GoodsID == goods_exist.ID).FirstOrDefault();
                    if (brand_exist == null)
                    {
                        if (goodsBrand != null)
                        {
                            EF.GoodsBrands.Add(brand_exist = new GoodsBrand
                            {
                                GoodsID = goods_exist.ID,
                                BrandID = goodsBrand.BrandID,
                            });
                        }
                    }
                    else
                    {
                        if (goodsBrand == null) { EF.GoodsBrands.Remove(brand_exist); }
                        else { brand_exist.BrandID = goodsBrand.BrandID; }
                    }
                    //品类
                    var kinds_exist = EF.GoodsKinds.Where(i => i.GoodsID == goods_exist.ID);
                    foreach (var kind_exist in kinds_exist)
                    {
                        var exist_find = false;
                        foreach (var goodsKind in goodsKinds)
                        {
                            if (goodsKind.KindLevel == kind_exist.KindLevel && goodsKind.KindID == kind_exist.KindID)
                            {
                                exist_find = true;
                                goodsKinds.Remove(goodsKind);
                                break;
                            }
                        }
                        if (!exist_find) { EF.GoodsKinds.Remove(kind_exist); }
                    }
                    foreach (var goodsKind in goodsKinds)
                    {
                        goodsKinds.ForEach(i => i.GoodsID = goods_exist.ID);
                        EF.GoodsKinds.Add(new GoodsKind
                        {
                            GoodsID = goods_exist.ID,
                            KindLevel = goodsKind.KindLevel,
                            KindID = goodsKind.KindID,
                        });
                    }
                    //标签
                    var tags_exist = EF.GoodsTags.Where(i => i.GoodsID == goods_exist.ID).ToList();
                    if (tags_exist != null)
                    {
                        EF.GoodsTags.RemoveRange(tags_exist);
                    }
                    if (goodsTags != null)
                    {
                        goodsTags.ForEach(i => i.GoodsID = goods_exist.ID);
                        EF.GoodsTags.AddRange(goodsTags);
                    }
                    //图文
                    var photo_exist = EF.GoodsPhotos.Where(i => i.GoodsID == goods_exist.ID).FirstOrDefault();
                    if (photo_exist == null) { EF.GoodsPhotos.Add(photo_exist = new GoodsPhoto { GoodsID = goods_exist.ID, }); }
                    photo_exist.FileIDs = goodsPhoto.FileIDs;
                    var desc_exist = EF.GoodsDescs.Where(i => i.GoodsID == goods_exist.ID).FirstOrDefault();
                    if (desc_exist == null) { EF.GoodsDescs.Add(desc_exist = new GoodsDesc { GoodsID = goods_exist.ID, }); }
                    desc_exist.Description = goodsDesc.Description;
                    //规格
                    var specs_exist = EF.GoodsSpecs.Where(i => i.GoodsID == goods_exist.ID).ToList();
                    if (specs_exist != null)
                    {
                        specs_exist.ForEach(i => i.Enabled = false);
                        EF.SaveChanges();
                    }
                    foreach (var goodsSpecFill in goodsSpecsFull)
                    {
                        var spec_exist = EF.GoodsSpecs.Where(i => i.GoodsID == goods_exist.ID && i.SpecValueIDs == goodsSpecFill.SpecValueIDs).FirstOrDefault();
                        if (spec_exist == null)
                        {
                            EF.GoodsSpecs.Add(spec_exist = new GoodsSpec
                            {
                                GoodsID = goods_exist.ID,
                                SpecValueIDs = goodsSpecFill.SpecValueIDs,
                            });
                        }
                        spec_exist.SpecValues = goodsSpecFill.SpecValues;
                        spec_exist.Enabled = goodsSpecFill.Enabled;
                        EF.SaveChanges();
                        var specCounter = EF.GoodsCounter.Where(i => i.GoodsID == goods_exist.ID && i.GoodsSpecID == spec_exist.ID).FirstOrDefault();
                        if (specCounter == null)
                        {
                            EF.GoodsCounter.Add(specCounter = new GoodsCounter
                            {
                                GoodsID = goods_exist.ID,
                                GoodsSpecID = spec_exist.ID,
                            });
                        }
                        specCounter.SKU = goodsSpecFill.SKU;
                        specCounter.UPC = goodsSpecFill.UPC;
                        specCounter.EAN = goodsSpecFill.EAN;
                        specCounter.JAN = goodsSpecFill.JAN;
                        specCounter.ISBN = goodsSpecFill.ISBN;
                        specCounter.Price = goodsSpecFill.Price ?? 0;
                        specCounter.Quantity = goodsSpecFill.Quantity ?? 0;
                    }
                    //柜台
                    var counter_exist = EF.GoodsCounter.Where(i => i.GoodsSpecID == 0 && i.GoodsID == goods_exist.ID).FirstOrDefault();
                    if (counter_exist == null)
                    {
                        EF.GoodsCounter.Add(counter_exist = new GoodsCounter { GoodsSpecID = 0, });
                    }
                    counter_exist.GoodsID = goods_exist.ID;
                    counter_exist.SKU = goodsCounter.SKU;
                    counter_exist.UPC = goodsCounter.UPC;
                    counter_exist.EAN = goodsCounter.EAN;
                    counter_exist.JAN = goodsCounter.JAN;
                    counter_exist.ISBN = goodsCounter.ISBN;
                    counter_exist.Price = goodsCounter.Price;
                    counter_exist.Quantity = goodsCounter.Quantity;
                    //保存
                    EF.SaveChanges();
                    TS.Complete();
                }

                //更新完成
                alertMessage = null;
                return goods_exist;
            }
        }

        //获取修改
        public static GoodsDetail GetModifyInfo(long GoodsID, out AlertMessage alertMessage)
        {
            alertMessage = null;

            //权限检查
            if (!AUTH.PermissionCheck(ModuleInfo, ActionInfos.Where(i => i.Name == (GoodsID == 0 ? "Create" : "Modify")).FirstOrDefault(), out string Message))
            {
                alertMessage = new AlertMessage { Message = Message, Type = AlertType.warning };
                return null;
            }

            return GetGoodsDetails(GoodsID);
        }

        //获取修改规格
        public static List<GoodsSpecFull> GetModifyGoodsSpecs(long GoodsID, List<GoodsSpecFull> specs_selected)
        {
            using (var EF = new EF())
            {
                var specs_exist = (from goodsSpec in EF.GoodsSpecs
                                   join specCounter in EF.GoodsCounter on goodsSpec.ID equals specCounter.GoodsSpecID into temp1
                                   from specCounter in temp1.DefaultIfEmpty()
                                   where goodsSpec.GoodsID == GoodsID
                                   select new GoodsSpecFull
                                   {
                                       SpecID = goodsSpec.ID,
                                       GoodsID = goodsSpec.GoodsID,
                                       SpecValueIDs = goodsSpec.SpecValueIDs,
                                       SpecValues = goodsSpec.SpecValues,
                                       Enabled = goodsSpec.Enabled,
                                       SKU = specCounter.SKU,
                                       UPC = specCounter.UPC,
                                       EAN = specCounter.EAN,
                                       JAN = specCounter.JAN,
                                       ISBN = specCounter.ISBN,
                                       Price = specCounter.Price,
                                       Quantity = specCounter.Quantity,
                                   }).ToList();
                if (specs_exist == null) { return specs_selected; }
                var specs_new = new List<GoodsSpecFull>();
                foreach (var spec_selected in specs_selected)
                {
                    var find = false;
                    foreach (var spec_exist in specs_exist)
                    {
                        if (spec_exist.SpecValueIDs == spec_selected.SpecValueIDs)
                        {
                            find = true;
                            spec_exist.Enabled = true;
                            specs_new.Add(spec_exist);
                            break;
                        }
                    }
                    if (!find) { specs_new.Add(spec_selected); }
                }
                return specs_new;
            }
        }

        //获取详情
        public static GoodsDetail GetDetailsInfo(long GoodsID, out AlertMessage alertMessage)
        {
            alertMessage = null;

            //权限检查
            if (!AUTH.PermissionCheck(ModuleInfo, ActionInfos.Where(i => i.Name == "Details").FirstOrDefault(), out string Message))
            {
                alertMessage = new AlertMessage { Message = Message, Type = AlertType.warning };
                return null;
            }

            return GetGoodsDetails(GoodsID, true);
        }

        //设置状态
        public static void SetGoodsEnabled(long GoodsID, bool Enabled, out AlertMessage alertMessage)
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
                var goodsInfo = EF.GoodsInfos.Where(i => i.ID == GoodsID).FirstOrDefault();
                goodsInfo.Enabled = Enabled;
                EF.SaveChanges();

                alertMessage = new AlertMessage { Message = "商品[" + goodsInfo.Name + "]" + (Enabled ? "启用" : "禁用") + "成功！", Type = AlertType.success };
            }
        }

        //设置上、下架
        public static void SetPublicSales(long GoodsID, GoodsState goodsState, out AlertMessage alertMessage)
        {
            alertMessage = null;

            //权限检查
            if (!AUTH.PermissionCheck(ModuleInfo, ActionInfos.Where(i => i.Name == (goodsState == GoodsState.上架 ? "PublicSales"
                : goodsState == GoodsState.下架 ? "StopSales" : null)).FirstOrDefault(), out string Message))
            {
                alertMessage = new AlertMessage { Message = Message, Type = AlertType.warning };
                return;
            }

            var goodsInfo = SetGoodsState(GoodsID, goodsState);
            alertMessage = new AlertMessage
            {
                Message = (goodsInfo == null ? "商品[" + GoodsID + "]不存在！" : "商品[" + goodsInfo.Name + "]" + goodsState + "成功！"),
                Type = AlertType.success
            };
        }

        //获取商品信息
        private static GoodsInfo GetGoodsInfo(long GoodsID)
        {
            using (var EF = new EF())
            {
                return EF.GoodsInfos.Where(i => i.ID == GoodsID).FirstOrDefault();
            }
        }

        //获取商品详情
        private static GoodsDetail GetGoodsDetails(long GoodsID, bool Enabled = false)
        {
            using (var EF = new EF())
            {
                var details = (from goodsInfo in EF.GoodsInfos
                               join goodsBrand in EF.GoodsBrands on goodsInfo.ID equals goodsBrand.GoodsID into temp1
                               from goodsBrand in temp1.DefaultIfEmpty()
                               join goodsPhoto in EF.GoodsPhotos on goodsInfo.ID equals goodsPhoto.GoodsID into temp2
                               from goodsPhoto in temp2.DefaultIfEmpty()
                               join goodsDesc in EF.GoodsDescs on goodsInfo.ID equals goodsDesc.GoodsID into temp3
                               from goodsDesc in temp3.DefaultIfEmpty()
                               where goodsInfo.ID == GoodsID
                               select new GoodsDetail
                               {
                                   ID = goodsInfo.ID,
                                   Name = goodsInfo.Name,
                                   Title = goodsInfo.Title,
                                   Enabled = goodsInfo.Enabled,
                                   StateID = goodsInfo.StateID,
                                   GoodsBrandID = goodsBrand.BrandID,
                                   PhotoFileIDs = goodsPhoto.FileIDs,
                                   Description = goodsDesc.Description,
                               }).FirstOrDefault();
                //补充
                if (details != null)
                {
                    //品牌
                    if (details.GoodsBrandID != null)
                    {
                        details.BrandFullInfo = Brand.GetBrandFullInfo(details.GoodsBrandID ?? 0);
                    }
                    //品类
                    details.GoodsKinds = EF.GoodsKinds.Where(i => i.GoodsID == GoodsID).ToList();
                    if (details.GoodsKinds != null)
                    {
                        List<long> KindIDs = details.GoodsKinds.Select(i => i.KindID).ToList();
                        details.KindInfos = EF.KindInfos.Where(i => KindIDs.Contains(i.ID)).ToList();
                    }
                    //标签
                    details.GoodsTags = EF.GoodsTags.Where(i => i.GoodsID == GoodsID).ToList();
                    //图片
                    details.PhotoUploadInfos = new List<FileUploadInfo>();
                    if (!string.IsNullOrWhiteSpace(details.PhotoFileIDs))
                    {
                        foreach (var FileID in details.PhotoFileIDs.Split(','))
                        {
                            details.PhotoUploadInfos.Add(DOC.GetFileUploadInfo(long.Parse(FileID)));
                        }
                    }
                    //规格
                    var goodsSpecsFull = from goodsSpec in EF.GoodsSpecs
                                         join goodsCounter in EF.GoodsCounter on goodsSpec.ID equals goodsCounter.GoodsSpecID into temp1
                                         from goodsCounter in temp1.DefaultIfEmpty()
                                         select new GoodsSpecFull
                                         {
                                             SpecID = goodsSpec.ID,
                                             GoodsID = goodsSpec.GoodsID,
                                             Enabled = goodsSpec.Enabled,
                                             SpecValueIDs = goodsSpec.SpecValueIDs,
                                             SpecValues = goodsSpec.SpecValues,
                                             SKU = goodsCounter.SKU,
                                             UPC = goodsCounter.UPC,
                                             EAN = goodsCounter.EAN,
                                             JAN = goodsCounter.JAN,
                                             ISBN = goodsCounter.ISBN,
                                             Price = goodsCounter.Price,
                                             Quantity = goodsCounter.Quantity,
                                         };
                    details.GoodsSpecsFull = Enabled
                        ? goodsSpecsFull.Where(i => i.GoodsID == GoodsID && i.Enabled == true).ToList()
                        : goodsSpecsFull.Where(i => i.GoodsID == GoodsID).OrderByDescending(i => i.Enabled).ToList();
                    if (details.GoodsSpecsFull != null)
                    {
                        details.SpecValues = new List<SpecValue>();
                        details.SpecInfos = new List<SpecInfo>();
                        foreach (var goodSpec in details.GoodsSpecsFull)
                        {
                            foreach (var specValueID in goodSpec.SpecValueIDs.Split(','))
                            {
                                var ValueID = long.Parse(specValueID);
                                details.SpecValues.Add(EF.SpecValues.Where(i => i.ID == ValueID).FirstOrDefault());
                            }
                        }
                        details.SpecValues = details.SpecValues.Distinct().ToList();
                        foreach (var specID in details.SpecValues.Select(i => i.SpecID).Distinct().ToList())
                        {
                            details.SpecInfos.Add(EF.SpecInfos.Where(i => i.ID == specID).FirstOrDefault());
                        }
                    }
                    //单价
                    details.GoodsCounter = EF.GoodsCounter.Where(i => i.GoodsID == GoodsID && i.GoodsSpecID == 0).FirstOrDefault();
                }
                return details;
            };
        }

        //设置商品状态
        private static GoodsInfo SetGoodsState(long GoodsID, GoodsState goodsState)
        {
            using (var EF = new EF())
            {
                var goodsInfo = EF.GoodsInfos.Where(i => i.ID == GoodsID).FirstOrDefault();
                if (goodsInfo != null)
                {
                    goodsInfo.StateID = (int)goodsState;
                    EF.SaveChanges();
                }
                return goodsInfo;
            }
        }

        //获取商品列表
        public static List<MallGoodsDetail> GetMallGoodsList(long BrandID = 0)
        {
            using (var EF = new EF())
            {
                var mallGoodsList = (from goodsInfo in EF.GoodsInfos
                                     join goodsBrand in EF.GoodsBrands on goodsInfo.ID equals goodsBrand.GoodsID    // into t1 from goodsBrand in t1.DefaultIfEmpty()
                                     join goodsPhoto in EF.GoodsPhotos on goodsInfo.ID equals goodsPhoto.GoodsID    // into t2 from goodsPhoto in t2.DefaultIfEmpty()
                                     where goodsInfo.Enabled == true && goodsInfo.StateID == (int)GoodsState.上架
                                     select new MallGoodsDetail
                                     {
                                         ID = goodsInfo.ID,
                                         Name = goodsInfo.Name,
                                         Title = goodsInfo.Title,
                                         BrandID = goodsBrand.BrandID,
                                         PhotoFileIDs = goodsPhoto.FileIDs
                                     }).ToList();
                if (BrandID > 0) { mallGoodsList = mallGoodsList.Where(i => i.BrandID == BrandID).ToList(); }

                foreach (var goodsInfo in mallGoodsList)
                {
                    //图片
                    goodsInfo.Photos = new List<string>();
                    if (!string.IsNullOrWhiteSpace(goodsInfo.PhotoFileIDs))
                    {
                        foreach (var FileID in goodsInfo.PhotoFileIDs.Split(','))
                        {
                            var fileInfo = DOC.GetFileUploadInfo(long.Parse(FileID));
                            goodsInfo.Photos.Add(fileInfo.FileInfo.GUID + fileInfo.FileExtName.Name);
                        }
                    }
                    //价格
                    var specCounter = EF.GoodsCounter.Where(i => i.GoodsSpecID > 0 && i.GoodsID == goodsInfo.ID).ToList();
                    if (specCounter.Count > 0)
                    {
                        var minPrice = specCounter.Min(i => i.Price);
                        var maxPrice = specCounter.Max(i => i.Price);
                        goodsInfo.Price = minPrice == maxPrice ? minPrice.ToString() : minPrice + " - " + maxPrice;
                    }
                    else
                    {
                        var singlePrice = EF.GoodsCounter.FirstOrDefault(i => i.GoodsSpecID == 0 && i.GoodsID == goodsInfo.ID);
                        goodsInfo.Price = (singlePrice == null ? 0.00M : singlePrice.Price).ToString();
                    }
                    //标签
                    goodsInfo.Tags = EF.GoodsTags.Where(i => i.GoodsID == goodsInfo.ID).Select(i => i.Title).ToList();
                }
                return mallGoodsList;
            }
        }

        //获取商品详情
        public static MallGoodsDetail GetMallGoodsDetail(long GoodsID)
        {
            using (var EF = new EF())
            {
                var mallGoodsDetail = (from goodsInfo in EF.GoodsInfos
                                       join goodsBrand in EF.GoodsBrands on goodsInfo.ID equals goodsBrand.GoodsID    // into t1 from goodsBrand in t1.DefaultIfEmpty()
                                       join goodsPhoto in EF.GoodsPhotos on goodsInfo.ID equals goodsPhoto.GoodsID    // into t2 from goodsPhoto in t2.DefaultIfEmpty()
                                       join goodsDesc in EF.GoodsDescs on goodsInfo.ID equals goodsDesc.GoodsID into temp3
                                       from goodsDesc in temp3.DefaultIfEmpty()
                                       where goodsInfo.Enabled == true && goodsInfo.StateID == (int)GoodsState.上架 && goodsInfo.ID == GoodsID
                                       select new MallGoodsDetail
                                       {
                                           ID = goodsInfo.ID,
                                           Name = goodsInfo.Name,
                                           Title = goodsInfo.Title,
                                           BrandID = goodsBrand.BrandID,
                                           PhotoFileIDs = goodsPhoto.FileIDs,
                                           Description = goodsDesc.Description,
                                       }).FirstOrDefault();

                //图片
                mallGoodsDetail.Photos = new List<string>();
                if (!string.IsNullOrWhiteSpace(mallGoodsDetail.PhotoFileIDs))
                {
                    foreach (var FileID in mallGoodsDetail.PhotoFileIDs.Split(','))
                    {
                        var fileInfo = DOC.GetFileUploadInfo(long.Parse(FileID));
                        mallGoodsDetail.Photos.Add(fileInfo.FileInfo.GUID + fileInfo.FileExtName.Name);
                    }
                }
                //价格
                var specCounter = EF.GoodsCounter.Where(i => i.GoodsSpecID > 0 && i.GoodsID == mallGoodsDetail.ID).ToList();
                if (specCounter.Count > 0)
                {
                    var minPrice = specCounter.Min(i => i.Price);
                    var maxPrice = specCounter.Max(i => i.Price);
                    mallGoodsDetail.Price = minPrice == maxPrice ? minPrice.ToString() : minPrice + " - " + maxPrice;
                }
                else
                {
                    var singlePrice = EF.GoodsCounter.FirstOrDefault(i => i.GoodsSpecID == 0 && i.GoodsID == mallGoodsDetail.ID);
                    mallGoodsDetail.Price = (singlePrice == null ? 0.00M : singlePrice.Price).ToString();
                }
                //标签
                mallGoodsDetail.Tags = EF.GoodsTags.Where(i => i.GoodsID == mallGoodsDetail.ID).Select(i => i.Title).ToList();
                //单价
                mallGoodsDetail.GoodsCounter = EF.GoodsCounter.Where(i => i.GoodsID == GoodsID && i.GoodsSpecID == 0).FirstOrDefault();
                //规格
                mallGoodsDetail.GoodsSpecsFull = (from goodsSpec in EF.GoodsSpecs
                                                  join goodsCounter in EF.GoodsCounter on goodsSpec.ID equals goodsCounter.GoodsSpecID into temp1
                                                  from goodsCounter in temp1.DefaultIfEmpty()
                                                  where goodsSpec.GoodsID == GoodsID && goodsSpec.Enabled == true
                                                  select new GoodsSpecFull
                                                  {
                                                      SpecID = goodsSpec.ID,
                                                      GoodsID = goodsSpec.GoodsID,
                                                      Enabled = goodsSpec.Enabled,
                                                      SpecValueIDs = goodsSpec.SpecValueIDs,
                                                      SpecValues = goodsSpec.SpecValues,
                                                      CounterID = goodsCounter.ID,
                                                      SKU = goodsCounter.SKU,
                                                      UPC = goodsCounter.UPC,
                                                      EAN = goodsCounter.EAN,
                                                      JAN = goodsCounter.JAN,
                                                      ISBN = goodsCounter.ISBN,
                                                      Price = goodsCounter.Price,
                                                      Quantity = goodsCounter.Quantity,
                                                  }).ToList();
                if (mallGoodsDetail.GoodsSpecsFull != null)
                {
                    mallGoodsDetail.SpecValues = new List<SpecValue>();
                    mallGoodsDetail.SpecInfos = new List<SpecInfo>();
                    foreach (var goodSpec in mallGoodsDetail.GoodsSpecsFull)
                    {
                        foreach (var specValueID in goodSpec.SpecValueIDs.Split(','))
                        {
                            var ValueID = long.Parse(specValueID);
                            mallGoodsDetail.SpecValues.Add(EF.SpecValues.Where(i => i.ID == ValueID).FirstOrDefault());
                        }
                    }
                    mallGoodsDetail.SpecValues = mallGoodsDetail.SpecValues.Distinct().OrderBy(i => i.ID).ToList();
                    foreach (var specID in mallGoodsDetail.SpecValues.Select(i => i.SpecID).Distinct().ToList())
                    {
                        mallGoodsDetail.SpecInfos.Add(EF.SpecInfos.Where(i => i.ID == specID).FirstOrDefault());
                    }
                    mallGoodsDetail.SpecInfos = mallGoodsDetail.SpecInfos.OrderBy(i => i.ID).ToList();
                }

                return mallGoodsDetail;
            }
        }
    }
}
