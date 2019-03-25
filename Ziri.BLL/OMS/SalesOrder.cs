using System;
using System.Collections.Generic;
using System.Linq;
using Ziri.BLL.SYS;
using Ziri.MDL;
using Ziri.DAL;
using Ziri.BLL.LIST;
using System.Transactions;

namespace Ziri.BLL.OMS
{
    public class SalesOrder
    {
        //模块定义
        public static ModuleInfo ModuleInfo = new ModuleInfo { Name = "SalesOrder", Title = "订单", IconFont = "fa fa-money-check-alt", Enabled = true };
        public static List<ActionInfo> ActionInfos = new List<ActionInfo> {
            new ActionInfo{ Name = "List", Title = "列表", IconFont = "", Enabled = true },
            new ActionInfo{ Name = "Details", Title = "详情", IconFont = "", Enabled = true },
            new ActionInfo{ Name = "Create", Title = "创建", IconFont = "", Enabled = true },
            new ActionInfo{ Name = "Modify", Title = "修改", IconFont = "", Enabled = true },
            new ActionInfo{ Name = "Confirm", Title = "确认", IconFont = "" , Enabled = true},
            new ActionInfo{ Name = "Unconfirm", Title = "撤销确认", IconFont = "" , Enabled = true},
            new ActionInfo{ Name = "Send", Title = "发货", IconFont = "" , Enabled = true},
            new ActionInfo{ Name = "Unsend", Title = "撤销发货", IconFont = "" , Enabled = true},
        };

        #region 字典
        //客户类型
        public static List<ListDict> CustomerType = new List<ListDict>
        {
            new ListDict { ID = 1, Name = "WxUser", Title = "微信用户", },
        };
        //收货方式
        public static List<ListDict> ReceiveType = new List<ListDict>
        {
            new ListDict { ID = 1, Name = "Stores", Title = "门店自提", },
            new ListDict { ID = 2, Name = "Logistics", Title = "物流派送", },
        };
        //支付方式
        public static List<ListDict> PayType = new List<ListDict>
        {
            new ListDict { ID = 1, Name = "F2F", Title = "到店支付", },
            new ListDict { ID = 2, Name = "WxPay", Title = "微信支付", },
            new ListDict { ID = 3, Name = "AliPay", Title = "支付宝支付", },
            new ListDict { ID = 4, Name = "Cash", Title = "现金支付", },
            new ListDict { ID = 5, Name = "Card", Title = "刷卡支付", },
        };
        //订单状态
        public static List<ListDict> State = new List<ListDict>
        {
            new ListDict { ID = 1, Name = "Create", Title = "创建", },
            new ListDict { ID = 2, Name = "Confirm", Title = "确认", },
            new ListDict { ID = 3, Name = "Cancel", Title = "取消", },
            new ListDict { ID = 4, Name = "Paid", Title = "付款", },
            new ListDict { ID = 5, Name = "Send", Title = "发货", },
            new ListDict { ID = 6, Name = "Taken", Title = "收货", },
            new ListDict { ID = 7, Name = "Return", Title = "退货", },
            new ListDict { ID = 8, Name = "ApplyCancel", Title = "申请取消", },
            new ListDict { ID = 9, Name = "AgreedCancel", Title = "同意取消", },
        };

        #endregion 字典

        //设置订单信息
        public static SOInfo SetSalesOrder(SOInfo SOInfo, out string Message)
        {
            using (var EF = new EF())
            {
                //检查单号
                if (!string.IsNullOrWhiteSpace(SOInfo.SalesOrder.BillNO))
                {
                    var billno_check = EF.SalesOrders.FirstOrDefault(i => i.ID != SOInfo.SalesOrder.ID && i.BillNO == SOInfo.SalesOrder.BillNO);
                    if (billno_check != null) { Message = "单号已被占用，请重试。"; return null; }
                }

                using (var TS = new TransactionScope())
                {
                    //主表信息
                    MDL.SalesOrder order_exist = null;
                    if (SOInfo.SalesOrder.ID == 0)
                    {
                        EF.SalesOrders.Add(order_exist = new MDL.SalesOrder
                        {
                            //WxUserID = salesOrder.WxUserID,
                            BillNO = "SO" + DateTime.Now.ToString("yyyyMMddHHmmssffff"),
                            StateID = State.FirstOrDefault(i => i.Name == "Create").ID,
                            CreateTime = DateTime.Now,
                        });
                    }
                    else
                    {
                        order_exist = EF.SalesOrders.FirstOrDefault(i => i.ID == SOInfo.SalesOrder.ID);
                        if (order_exist == null) { Message = "订单编号" + SOInfo.SalesOrder.ID + "不存在"; return null; }
                        //检查
                        var order_state = State.FirstOrDefault(i => i.ID == order_exist.StateID);
                        if (order_state.ID != State.FirstOrDefault(i => i.Name == "Create").ID) { Message = "订单状态为" + order_state.Title + "，不能修改！"; return null; }
                        var customer_check = EF.SOCustomerInfos.FirstOrDefault(i => i.SOID == order_exist.ID);
                        if (customer_check.CustomerID != SOInfo.SOCustomerInfo.CustomerID) { Message = "不能修改其他客户的订单"; return null; }
                        //通过
                        order_exist.UpdateTime = DateTime.Now;
                    }
                    order_exist.Remark = SOInfo.SalesOrder.Remark;
                    EF.SaveChanges();

                    //分录信息
                    var items_check = EF.SalesOrderItems.Where(i => i.SOID == order_exist.ID).ToList();
                    if (items_check != null) { EF.SalesOrderItems.RemoveRange(items_check); }
                    var items_exist = new List<SalesOrderItem>();
                    var item_goods_id = SOInfo.SalesOrderItems.Select(j => j.GoodsID);
                    var item_goods = EF.GoodsInfos.Where(i => item_goods_id.Contains(i.ID)).ToList();
                    foreach (var item in SOInfo.SalesOrderItems)
                    {
                        var counter = EF.GoodsCounter.FirstOrDefault(i => i.ID == item.CounterID);
                        var item_exist = new SalesOrderItem
                        {
                            SOID = order_exist.ID,
                            OrderID = SOInfo.SalesOrderItems.IndexOf(item) + 1,
                            GoodsID = item.GoodsID,
                            SpecID = item.SpecID,
                            CounterID = item.CounterID,
                            //Counter = item.Counter,
                            Quantity = item.Quantity,
                            Price = counter.Price,
                            Amount = item.Quantity * counter.Price,
                        };
                        EF.SalesOrderItems.Add(item_exist);
                        items_exist.Add(item_exist);
                    }

                    //客户信息
                    var customer_exist = EF.SOCustomerInfos.FirstOrDefault(i => i.SOID == order_exist.ID);
                    if (customer_exist == null) { EF.SOCustomerInfos.Add(customer_exist = new SOCustomerInfo { SOID = order_exist.ID, }); }
                    customer_exist.CustomerTypeID = SOInfo.SOCustomerInfo.CustomerTypeID;
                    customer_exist.CustomerID = SOInfo.SOCustomerInfo.CustomerID;

                    //收货信息
                    var receive_exist = EF.SOReceiveInfos.FirstOrDefault(i => i.SOID == order_exist.ID);
                    if (receive_exist == null) { EF.SOReceiveInfos.Add(receive_exist = new SOReceiveInfo { SOID = order_exist.ID, }); }
                    receive_exist.Consignee = SOInfo.SOReceiveInfo.Consignee;
                    receive_exist.Phone = SOInfo.SOReceiveInfo.Phone;
                    receive_exist.ReceiveTypeID = SOInfo.SOReceiveInfo.ReceiveTypeID;
                    receive_exist.StoreID = SOInfo.SOReceiveInfo.StoreID;
                    receive_exist.Address = SOInfo.SOReceiveInfo.Address;
                    receive_exist.AddressCheck = SOInfo.SOReceiveInfo.AddressCheck;

                    //支付信息
                    var pay_exist = EF.SOPayInfos.FirstOrDefault(i => i.SOID == order_exist.ID);
                    if (pay_exist == null)
                    {
                        EF.SOPayInfos.Add(pay_exist = new SOPayInfo
                        {
                            SOID = order_exist.ID,
                            StateID = WeChat.WxTradeState.FirstOrDefault(i => i.Name == "NOTPAY").ID,
                        });
                    }
                    pay_exist.PayTypeID = SOInfo.SOPayInfo.PayTypeID;

                    //保存
                    EF.SaveChanges();
                    TS.Complete();

                    Message = null;
                    return new SOInfo
                    {
                        SalesOrder = order_exist,
                        SalesOrderItems = items_exist,
                        SOCustomerInfo = customer_exist,
                        SOReceiveInfo = receive_exist,
                        SOPayInfo = pay_exist,

                        GoodsInfos = item_goods,
                    };
                }
            }
        }

        //查询订单信息
        public static SOInfo GetSalesOrder(long SOID, out string Message)
        {
            using (var EF = new EF())
            {
                var order_exist = EF.SalesOrders.FirstOrDefault(i => i.ID == SOID);
                if (order_exist == null)
                {
                    Message = "订单号" + SOID + "不存在";
                    return null;
                }

                Message = null;
                var result = new SOInfo
                {
                    SalesOrder = order_exist,
                    SalesOrderItems = EF.SalesOrderItems.Where(i => i.SOID == order_exist.ID).ToList(),
                    SOCustomerInfo = EF.SOCustomerInfos.FirstOrDefault(i => i.SOID == order_exist.ID),
                    SOReceiveInfo = EF.SOReceiveInfos.FirstOrDefault(i => i.SOID == order_exist.ID),
                    SOPayInfo = EF.SOPayInfos.FirstOrDefault(i => i.SOID == order_exist.ID),
                };

                result.SOStateTitle = State.FirstOrDefault(i => i.ID == order_exist.StateID).Title;
                var customerType = CustomerType.FirstOrDefault(i => i.ID == result.SOCustomerInfo.CustomerTypeID);
                result.CustomerTypeTitle = customerType.Title;
                switch (customerType.Name)
                {
                    case "WxUser":
                        var wxUser = EF.WxUserInfos.FirstOrDefault(i => i.ID == result.SOCustomerInfo.CustomerID);
                        result.CustomerNickName = wxUser.NickName;
                        result.CustomerGender = WeChat.WxUserGender.FirstOrDefault(i => i.Name == wxUser.Gender).Title;
                        result.CustomerAvatar = wxUser.AvatarUrl;
                        break;
                }
                var goodsid = result.SalesOrderItems.Select(i => i.GoodsID).ToList();
                result.GoodsInfos = EF.GoodsInfos.Where(i => goodsid.Contains(i.ID)).ToList();
                result.SOItems = (from soItem in EF.SalesOrderItems
                                  join goodsInfo in EF.GoodsInfos on soItem.GoodsID equals goodsInfo.ID
                                  join goodsPhoto in EF.GoodsPhotos on goodsInfo.ID equals goodsPhoto.GoodsID into t1
                                  from goodsPhoto in t1.DefaultIfEmpty()
                                  join specInfo in EF.GoodsSpecs on soItem.SpecID equals specInfo.ID
                                  where soItem.SOID == result.SalesOrder.ID
                                  select new SOItem
                                  {
                                      OrderID = soItem.OrderID,
                                      GoodsID = soItem.ID,
                                      GoodsName = goodsInfo.Name,
                                      GoodsTitle = goodsInfo.Title,
                                      GoodsPhotoIDs = goodsPhoto.FileIDs,
                                      GoodsSpecTitle = specInfo.SpecValues,
                                      Quantity = soItem.Quantity,
                                      Price = soItem.Price,
                                      Amount = soItem.Amount,
                                  }).ToList();
                foreach (var goods in result.SOItems)
                {
                    if (!string.IsNullOrWhiteSpace(goods.GoodsPhotoIDs))
                    {
                        var fileids = goods.GoodsPhotoIDs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();
                        goods.GoodsPhotos = (from file in EF.FileInfos
                                             join extname in EF.FileExtName on file.ExtNameID equals extname.ID
                                             where fileids.Contains(file.ID)
                                             select file.GUID + extname.Name).ToList();
                    }
                }
                result.ReceiptTypeTitle = ReceiveType.FirstOrDefault(i => i.ID == result.SOReceiveInfo.ReceiveTypeID).Title;
                result.ReceiptStore = EF.StoreInfos.FirstOrDefault(i => i.ID == result.SOReceiveInfo.StoreID);
                result.PayTypeTitle = PayType.FirstOrDefault(i => i.ID == result.SOPayInfo.PayTypeID).Title;
                result.PayStateTitle = WeChat.WxTradeState.FirstOrDefault(i => i.ID == result.SOPayInfo.StateID).Title;
                if (result.SOPayInfo.PayID > 0) { result.F2FPayInfo = Pay.GetF2FPayInfo(result.SOPayInfo.PayID ?? 0, out Message); }

                return result;
            }
        }

        //设置支付信息
        public static void SetPayInfo(long SOID, SOPayInfo SOPayInfo, bool Paid, out string Message)
        {
            using (var TS = new TransactionScope())
            {
                using (var EF = new EF())
                {
                    var so_exist = EF.SalesOrders.FirstOrDefault(i => i.ID == SOID);
                    if (so_exist == null)
                    {
                        Message = "订单号" + SOID + "不存在";
                        return;
                    }

                    var pay_exist = EF.SOPayInfos.FirstOrDefault(i => i.SOID == so_exist.ID);
                    pay_exist.PrePayID = SOPayInfo.PrePayID;
                    pay_exist.PayID = SOPayInfo.PayID;
                    EF.SaveChanges();

                    if (Paid)
                    {
                        so_exist.StateID = State.FirstOrDefault(i => i.Name == "Paid").ID;
                        pay_exist.StateID = Ziri.BLL.WeChat.WxTradeState.FirstOrDefault(i => i.Name == "SUCCESS").ID;
                        EF.SaveChanges();
                    }

                    TS.Complete();
                    Message = null;
                }
            }
        }

        //获取微信订单状态组
        public static int GetSOStateCount(long CustomerID, string StateGroup)
        {
            using (var EF = new EF())
            {
                var WxCustomerTypeID = CustomerType.FirstOrDefault(i => i.Name == "WxUser").ID;
                var f2fid = PayType.FirstOrDefault(i => i.Name == "F2F").ID;
                var createid = State.FirstOrDefault(i => i.Name == "Create").ID;
                var confirmid = State.FirstOrDefault(i => i.Name == "Confirm").ID;
                var sendid = State.FirstOrDefault(i => i.Name == "Send").ID;
                switch (StateGroup)
                {
                    case "全部":
                        return (from soInfo in EF.SalesOrders
                                join soCustomer in EF.SOCustomerInfos on soInfo.ID equals soCustomer.SOID
                                where soCustomer.CustomerTypeID == WxCustomerTypeID && soCustomer.CustomerID == CustomerID
                                select soInfo.ID).Count();
                    case "待付款":
                        return (from soInfo in EF.SalesOrders
                                join soCustomer in EF.SOCustomerInfos on soInfo.ID equals soCustomer.SOID
                                join payInfo in EF.SOPayInfos on soInfo.ID equals payInfo.SOID
                                where soCustomer.CustomerTypeID == WxCustomerTypeID && soCustomer.CustomerID == CustomerID
                                && soInfo.StateID == createid && payInfo.PayTypeID != f2fid
                                select soInfo.ID).Count();
                    case "待提货":
                        return (from soInfo in EF.SalesOrders
                                join soCustomer in EF.SOCustomerInfos on soInfo.ID equals soCustomer.SOID
                                join payInfo in EF.SOPayInfos on soInfo.ID equals payInfo.SOID
                                where soCustomer.CustomerTypeID == WxCustomerTypeID && soCustomer.CustomerID == CustomerID
                                && soInfo.StateID == confirmid && payInfo.PayTypeID == f2fid
                                select soInfo.ID).Count();
                    case "待收货":
                        return (from soInfo in EF.SalesOrders
                                join soCustomer in EF.SOCustomerInfos on soInfo.ID equals soCustomer.SOID
                                where soCustomer.CustomerTypeID == WxCustomerTypeID && soCustomer.CustomerID == CustomerID
                                && soInfo.StateID == sendid
                                select soInfo.ID).Count();
                    default: return 0;
                }
            }
        }

        //获取微信订单列表
        public static List<SOFullInfo> GetSOList(long CustomerID, string StateGroup, PagerCount PagerCount, out PagerCount PagerCountNew)
        {
            using (var EF = new EF())
            {
                //列表
                var WxCustomerTypeID = CustomerType.FirstOrDefault(i => i.Name == "WxUser").ID;
                var list = from soInfo in EF.SalesOrders
                           join soCustomer in EF.SOCustomerInfos on soInfo.ID equals soCustomer.SOID
                           join payInfo in EF.SOPayInfos on soInfo.ID equals payInfo.SOID
                           orderby soInfo.CreateTime descending
                           select new SOFullInfo
                           {
                               ID = soInfo.ID,
                               BillNO = soInfo.BillNO,
                               CreateTime = soInfo.CreateTime,
                               CustomerTypeID = soCustomer.CustomerTypeID,
                               CustomerID = soCustomer.CustomerID,
                               StateID = soInfo.StateID,
                               PayTypeID = payInfo.PayTypeID
                           };

                //状态组
                var f2fid = PayType.FirstOrDefault(i => i.Name == "F2F").ID;
                var createid = State.FirstOrDefault(i => i.Name == "Create").ID;
                var confirmid = State.FirstOrDefault(i => i.Name == "Confirm").ID;
                var sendid = State.FirstOrDefault(i => i.Name == "Send").ID;
                switch (StateGroup)
                {
                    case "全部":
                        list = list.Where(i => i.CustomerTypeID == WxCustomerTypeID && i.CustomerID == CustomerID);
                        break;
                    case "待付款":
                        list = list.Where(i => i.CustomerTypeID == WxCustomerTypeID && i.CustomerID == CustomerID
                        && i.StateID == createid && i.PayTypeID != f2fid);
                        break;
                    case "待提货":
                        list = list.Where(i => i.CustomerTypeID == WxCustomerTypeID && i.CustomerID == CustomerID
                        && i.StateID == confirmid && i.PayTypeID == f2fid);
                        break;
                    case "待收货":
                        list = list.Where(i => i.CustomerTypeID == WxCustomerTypeID && i.CustomerID == CustomerID
                        && i.StateID == sendid);
                        break;
                }

                //分页
                PagerCountNew = PagerCount;
                PagerCountNew.ItemCount = list.Count();
                if (PagerCountNew.ItemCount == 0) { return null; }
                PagerCountNew.PageCount = (int)Math.Ceiling((double)PagerCountNew.ItemCount / PagerCountNew.PageSize);
                if (PagerCountNew.PageIndex > PagerCountNew.PageCount) { PagerCountNew.PageIndex = PagerCountNew.PageCount; }
                var PageList = list.Skip((PagerCountNew.PageIndex - 1) * PagerCountNew.PageSize).Take(PagerCountNew.PageSize).ToList();

                //补充
                foreach (var so in PageList)
                {
                    so.StateTitle = State.FirstOrDefault(i => i.ID == so.StateID).Title;
                    so.PayTypeTitle = PayType.FirstOrDefault(i => i.ID == so.PayTypeID).Title;
                    so.SOItems = (from soItem in EF.SalesOrderItems
                                  join goodsInfo in EF.GoodsInfos on soItem.GoodsID equals goodsInfo.ID
                                  join goodsPhoto in EF.GoodsPhotos on goodsInfo.ID equals goodsPhoto.GoodsID into t1
                                  from goodsPhoto in t1.DefaultIfEmpty()
                                  join specInfo in EF.GoodsSpecs on soItem.SpecID equals specInfo.ID into t2
                                  from specInfo in t2.DefaultIfEmpty()
                                  where soItem.SOID == so.ID
                                  select new SOItem
                                  {
                                      OrderID = soItem.OrderID,
                                      GoodsID = soItem.ID,
                                      GoodsName = goodsInfo.Name,
                                      GoodsTitle = goodsInfo.Title,
                                      GoodsPhotoIDs = goodsPhoto.FileIDs,
                                      GoodsSpecTitle = specInfo.SpecValues,
                                      Quantity = soItem.Quantity,
                                      Price = soItem.Price,
                                      Amount = soItem.Amount,
                                  }).ToList();
                    foreach (var goods in so.SOItems)
                    {
                        if (!string.IsNullOrWhiteSpace(goods.GoodsPhotoIDs))
                        {
                            var fileids = goods.GoodsPhotoIDs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();
                            goods.GoodsPhotos = (from file in EF.FileInfos
                                                 join extname in EF.FileExtName on file.ExtNameID equals extname.ID
                                                 where fileids.Contains(file.ID)
                                                 select file.GUID + extname.Name).ToList();
                        }
                    }
                }

                return PageList;
            }
        }

        //获取后台订单列表
        public static List<SOFullInfo> GetSOInfos(List<ListFilterField> FilterField, List<ListOrderField> OrderField
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
                var WxCustomerTypeID = CustomerType.FirstOrDefault(i => i.Name == "WxUser").ID;
                IEnumerable<SOFullInfo> DataList = (from soInfo in EF.SalesOrders
                                                    join soCustomer in (
                                                        from wxCustomer in EF.SOCustomerInfos
                                                        join wxUser in EF.WxUserInfos on wxCustomer.CustomerID equals wxUser.ID
                                                        where wxCustomer.CustomerTypeID == WxCustomerTypeID
                                                        select new { wxCustomer, wxUser }
                                                        ) on soInfo.ID equals soCustomer.wxCustomer.SOID
                                                    join soItems in EF.SalesOrderItems on soInfo.ID equals soItems.SOID
                                                    join goodsInfo in EF.GoodsInfos on soItems.GoodsID equals goodsInfo.ID
                                                    join goodsCounter in EF.GoodsCounter on soItems.CounterID equals goodsCounter.ID
                                                    join goodsSpec in EF.GoodsSpecs on goodsCounter.GoodsSpecID equals goodsSpec.ID into t1
                                                    from goodsSpec in t1.DefaultIfEmpty()
                                                    join soPay in EF.SOPayInfos on soInfo.ID equals soPay.SOID
                                                    join soReceipt in EF.SOReceiveInfos on soInfo.ID equals soReceipt.SOID
                                                    join storeInfo in EF.StoreInfos on soReceipt.StoreID equals storeInfo.ID
                                                    select new SOFullInfo
                                                    {
                                                        ID = soInfo.ID,
                                                        BillNO = soInfo.BillNO,
                                                        CreateTime = soInfo.CreateTime,
                                                        UpdateTime = soInfo.UpdateTime,
                                                        StateID = soInfo.StateID,
                                                        PayTypeID = soPay.PayTypeID,
                                                        CustomerID = soCustomer.wxCustomer.CustomerID,
                                                        CustomerName = soCustomer.wxUser.NickName,
                                                        GoodsID = goodsInfo.ID,
                                                        GoodsName = goodsInfo.Name,
                                                        GoodsTitle = goodsInfo.Title,
                                                        GoodsSpecID = goodsCounter.GoodsSpecID,
                                                        GoodsSpecValues = goodsSpec.SpecValues,
                                                        Quantity = soItems.Quantity,
                                                        Price = soItems.Price,
                                                        Amount = soItems.Amount,
                                                        Consignee = soReceipt.Consignee,
                                                        Phone = soReceipt.Phone,
                                                        ReceiveTypeID = soReceipt.ReceiveTypeID,
                                                        StoreID = soReceipt.StoreID,
                                                        StoreName = storeInfo.Name,
                                                        StoreTitle = storeInfo.Title,
                                                        Address = soReceipt.Address,
                                                        //AddressCheck = null,
                                                    });

                //筛选
                foreach (var item in FilterField)
                {
                    if (item.Name == "BillNO" && item.Value.Count > 0)
                    {
                        var predicate = PredicateExtensions.False<SOFullInfo>();
                        foreach (var t in item.Value)
                        {
                            var KWPart = t.ToLower();
                            switch (item.CmpareMode)
                            {
                                case FilterCmpareMode.Equal:
                                    predicate = predicate.Or(p => p.BillNO.ToLower() == KWPart);
                                    break;
                                case FilterCmpareMode.Like:
                                    predicate = predicate.Or(p => p.BillNO.ToLower().Contains(KWPart));
                                    break;
                            }
                        }
                        DataList = DataList.Where(predicate.Compile());
                    }
                    else if (item.Name == "StateID" && item.Value.Count > 0)
                    {
                        var predicate = PredicateExtensions.False<SOFullInfo>();
                        predicate = predicate.Or(p => item.Value.Contains(p.StateID.ToString()));
                        DataList = DataList.Where(predicate.Compile());
                    }
                    else if (item.Name == "ReceiptTypeID" && item.Value.Count > 0)
                    {
                        var predicate = PredicateExtensions.False<SOFullInfo>();
                        predicate = predicate.Or(p => item.Value.Contains(p.ReceiveTypeID.ToString()));
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
                var PageList = DataList.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();

                //补充
                foreach (var item in PageList)
                {
                    item.StateTitle = State.FirstOrDefault(i => i.ID == item.StateID).Title;
                    item.PayTypeTitle = PayType.FirstOrDefault(i => i.ID == item.PayTypeID).Title;
                    item.ReceiveTypeTitle = ReceiveType.FirstOrDefault(i => i.ID == item.ReceiveTypeID).Title;
                }

                //返回
                return PageList;
            }
        }

        //设置确认
        public static MDL.SalesOrder SetConfirm(long SOID, bool Confirm, out string Message)
        {
            if (!AUTH.PermissionCheck(ModuleInfo, ActionInfos.Where(i => i.Name == (Confirm ? "Confirm" : "Unconfirm")).FirstOrDefault(), out Message)) { return null; }
            using (var EF = new EF())
            {
                var so_exist = EF.SalesOrders.FirstOrDefault(i => i.ID == SOID);
                if (so_exist == null) { Message = "订单号" + SOID + "不存在"; return null; }
                var so_state = State.FirstOrDefault(i => i.ID == so_exist.StateID);
                if (Confirm)
                {
                    if (so_state.ID != State.FirstOrDefault(i => i.Name == "Create").ID)
                    {
                        Message = "订单号" + SOID + "状态为" + so_state.Title + "，不能确认。";
                        return null;
                    }
                    var receipt_exist = EF.SOReceiveInfos.FirstOrDefault(i => i.SOID == so_exist.ID);
                    var receipt_type = ReceiveType.FirstOrDefault(i => i.ID == receipt_exist.ReceiveTypeID);
                    if (receipt_type.ID != ReceiveType.FirstOrDefault(i => i.Name == "Stores").ID)
                    {
                        Message = "订单号" + SOID + "收货方式为" + receipt_type.Title + "，不能确认。";
                        return null;
                    }
                    so_exist.StateID = State.FirstOrDefault(i => i.Name == "Confirm").ID;
                }
                else
                {
                    if (so_state.ID != State.FirstOrDefault(i => i.Name == "Confirm").ID)
                    {
                        Message = "订单号" + SOID + "状态为" + so_state.Title + "，不能撤销确认。";
                        return null;
                    }
                    so_exist.StateID = State.FirstOrDefault(i => i.Name == "Create").ID;
                }
                EF.SaveChanges();
                return so_exist;
            }
        }

        //设置发货
        public static MDL.SalesOrder SetSend(long SOID, bool Send, out string Message)
        {
            if (!AUTH.PermissionCheck(ModuleInfo, ActionInfos.Where(i => i.Name == (Send ? "Send" : "Unsend")).FirstOrDefault(), out Message)) { return null; }
            using (var EF = new EF())
            {
                var so_exist = EF.SalesOrders.FirstOrDefault(i => i.ID == SOID);
                if (so_exist == null) { Message = "订单号" + SOID + "不存在"; return null; }
                var so_state = State.FirstOrDefault(i => i.ID == so_exist.StateID);
                if (Send)
                {
                    if (so_state.ID != State.FirstOrDefault(i => i.Name == "Paid").ID)
                    {
                        Message = "订单号" + SOID + "状态为" + so_state.Title + "，不能发货。";
                        return null;
                    }
                    var receipt_exist = EF.SOReceiveInfos.FirstOrDefault(i => i.SOID == so_exist.ID);
                    var receipt_type = ReceiveType.FirstOrDefault(i => i.ID == receipt_exist.ReceiveTypeID);
                    if (receipt_type.ID != ReceiveType.FirstOrDefault(i => i.Name == "Logistics").ID)
                    {
                        Message = "订单号" + SOID + "收货方式为" + receipt_type.Title + "，不能发货。";
                        return null;
                    }
                    so_exist.StateID = State.FirstOrDefault(i => i.Name == "Send").ID;
                }
                else
                {
                    if (so_state.ID != State.FirstOrDefault(i => i.Name == "Send").ID)
                    {
                        Message = "订单号" + SOID + "状态为" + so_state.Title + "，不能撤销发货。";
                        return null;
                    }
                    so_exist.StateID = State.FirstOrDefault(i => i.Name == "Paid").ID;
                }
                EF.SaveChanges();
                return so_exist;
            }
        }

        //收银检查
        public static SOInfo GetReceiptCheck(long SOID, out string Message)
        {
            var SOInfo = GetSalesOrder(SOID, out Message);
            if (Message != null) { return null; }
            var so_state = State.FirstOrDefault(i => i.ID == SOInfo.SalesOrder.StateID);
            if (so_state.ID != State.FirstOrDefault(i => i.Name == "Confirm").ID) { Message = "订单号" + SOID + "未确认"; return null; }
            if (so_state.ID == State.FirstOrDefault(i => i.Name == "Paid").ID) { Message = "订单号" + SOID + "已付款"; return null; }
            return SOInfo;
        }

        //设置申请取消
        public static void SetApplyCancel(long SOID, out string Message)
        {
            using (var EF = new EF())
            {
                var so_exist = EF.SalesOrders.FirstOrDefault(i => i.ID == SOID);
                if (so_exist == null) { Message = "订单号" + SOID + "不存在"; return; }
                if (so_exist.StateID == State.FirstOrDefault(i => i.Name == "Create").ID)
                {
                    //新建状态，直接取消
                    so_exist.StateID = State.FirstOrDefault(i => i.Name == "Cancel").ID;
                    Message = "订单" + so_exist.BillNO + "已取消";
                }
                else
                {
                    so_exist.StateID = State.FirstOrDefault(i => i.Name == "ApplyCancel").ID;
                    //确认状态，取消库存锁定，再取消（因库存管理模块未完成，当前作标记取消处理）
                    //支付状态，取消库存销定，发起退款成功，再取消（因库存管理模块未完成，退款功能未完成，当前作标记取消处理）
                    //发货、收货状态，标记取消，后台人工处理退款、退货业务，再取消
                    Message = "订单" + so_exist.BillNO + "已进入取消流程，请等待系统审核退货退款";
                }
                EF.SaveChanges();
                Message += "，欢迎您的再次惠顾！";
            }
        }

        //设置同意申请取消
        public static MDL.SalesOrder SetAgreedCancel(long SOID, out string Message)
        {
            using (var EF = new EF())
            {
                var so_exist = EF.SalesOrders.FirstOrDefault(i => i.ID == SOID);
                if (so_exist == null) { Message = "订单号" + SOID + "不存在"; return null; }
                var so_state = State.FirstOrDefault(i => i.ID == so_exist.StateID);
                if (so_state.ID != State.FirstOrDefault(i => i.Name == "ApplyCancel").ID)
                {
                    Message = "订单" + so_exist.BillNO + "状态为" + so_state.Title + "，不能同意申请取消";
                    return null;
                }
                so_exist.StateID = State.FirstOrDefault(i => i.Name == "AgreedCancel").ID;
                EF.SaveChanges();
                Message = null;
                return so_exist;
            }
        }

        //设置取消
        public static MDL.SalesOrder SetCancel(long SOID, out string Message)
        {
            using (var EF = new EF())
            {
                var so_exist = EF.SalesOrders.FirstOrDefault(i => i.ID == SOID);
                if (so_exist == null) { Message = "订单号" + SOID + "不存在"; return null; }
                var so_state = State.FirstOrDefault(i => i.ID == so_exist.StateID);
                //是否退货
                //是否退款
                so_exist.StateID = State.FirstOrDefault(i => i.Name == "Cancel").ID;
                EF.SaveChanges();
                Message = null;
                return so_exist;
            }
        }

        //设置收货
        public static void SetTaken(long SOID, out string Message)
        {
            using (var EF = new EF())
            {
                var so_exist = EF.SalesOrders.FirstOrDefault(i => i.ID == SOID);
                if (so_exist == null) { Message = "订单号" + SOID + "不存在"; return; }
                if (so_exist.StateID != State.FirstOrDefault(i => i.Name == "Send").ID)
                {
                    Message = "订单" + so_exist.BillNO + "未发货，不能收货";
                    return;
                }
                so_exist.StateID = State.FirstOrDefault(i => i.Name == "Taken").ID;
                EF.SaveChanges();
                Message = "订单" + so_exist.BillNO + "已收货，感谢您的惠顾，欢迎再次光临！";
            }
        }
    }
}
