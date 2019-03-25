using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ziri.MDL;

namespace DMS.MOD.OMS
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class WeChat : WebService
    {
        //微信静默登录
        [WebMethod(EnableSession = true)]
        public object Login(string js_code)
        {
            if (string.IsNullOrWhiteSpace(js_code)) { return ResponseWrite(new Result { Code = ResultCode.Error, Message = "授权code为空" }); }
            var userInfo = Ziri.BLL.WeChat.GetWxUserInfoByJSCode(js_code, out string Message);
            if (Message != null) { return ResponseWrite(new Result { Code = ResultCode.Error, Message = Message }); }

            Context.Session["WxUserInfo"] = userInfo;
            return ResponseWrite(new Result { Code = ResultCode.Success, Data = userInfo });
        }

        //微信用户注册
        [WebMethod(EnableSession = true)]
        public object SetUserInfo(string js_code, string rawData)
        {
            if (string.IsNullOrWhiteSpace(js_code)) { return ResponseWrite(new Result { Code = ResultCode.Error, Message = "授权code为空" }); }
            var UserInfo = Ziri.BLL.WeChat.GetWxUserInfoByJSCode(js_code, out string Message);
            if (Message != null) { return ResponseWrite(new Result { Code = ResultCode.Error, Message = Message }); }

            var userInfo = JsonConvert.DeserializeObject<JObject>(rawData);
            UserInfo.NickName = (string)userInfo["nickName"];
            UserInfo.Gender = (string)userInfo["gender"];
            UserInfo.Language = (string)userInfo["language"];
            UserInfo.City = (string)userInfo["city"];
            UserInfo.Province = (string)userInfo["province"];
            UserInfo.Country = (string)userInfo["country"];
            UserInfo.AvatarUrl = (string)userInfo["avatarUrl"];
            UserInfo = Ziri.BLL.WeChat.SetWxUserInfo(UserInfo);
            //会话
            Context.Session["WxUserInfo"] = UserInfo;
            return ResponseWrite(new Result { Code = ResultCode.Success, Data = Context.Session["WxUserInfo"] });
        }

        //门店列表
        [WebMethod(EnableSession = true)]
        public object GetWxStoreLists(double? latitude, double? longitude)
        {
            var list = Ziri.BLL.WeChat.GetWxStoreLists(latitude, longitude);
            return ResponseWrite(new Result { Code = ResultCode.Success, Data = list });
        }

        //品牌列表
        [WebMethod(EnableSession = true)]
        public object GetMallBrandList()
        {
            var list = Ziri.BLL.ITEM.Brand.GetBrandInfos(true).OrderByDescending(i => i.ID).ToList();
            return ResponseWrite(new Result { Code = ResultCode.Success, Data = list });
        }

        //商品列表
        [WebMethod(EnableSession = true)]
        public object GetMallGoodsList(long brand_id)
        {
            var list = Ziri.BLL.ITEM.Goods.GetMallGoodsList(brand_id);
            return ResponseWrite(new Result { Code = ResultCode.Success, Data = list });
        }

        //商品详情
        [WebMethod(EnableSession = true)]
        public object GetMallGoodsDetail(long goods_id)
        {
            var goodsDetails = Ziri.BLL.ITEM.Goods.GetMallGoodsDetail(goods_id);
            return ResponseWrite(new Result { Code = ResultCode.Success, Data = goodsDetails });
        }

        //设置订单信息
        [WebMethod(EnableSession = true)]
        public object SetOrderInfo(string user_code, string order_info, string receive_info)
        {
            string Message = null;

            #region 检查会话
            if (Context.Session["WxUserInfo"] == null)
            {
                if (string.IsNullOrWhiteSpace(user_code))
                {
                    return ResponseWrite(new Result { Code = ResultCode.Error, Message = "用户未登录，或者登录状态已失效，请重新登录后再操作。" });
                }
                var userInfo = Ziri.BLL.WeChat.GetWxUserInfoByJSCode(user_code, out Message);
                if (Message != null) { return ResponseWrite(new Result { Code = ResultCode.Error, Message = Message }); }
                Context.Session["WxUserInfo"] = userInfo;
            }
            var wxUserInfo = (WxUserInfo)Context.Session["WxUserInfo"];
            #endregion 检查会话

            #region 检查表单
            Message = "";
            // - 订单信息
            var orderInfo = JsonConvert.DeserializeObject<JObject>(order_info);
            long BillID = 0;
            try { BillID = (long)orderInfo["BillID"]; } catch { Message += "订单编号不正确"; }
            string BillNO = null;
            try { BillNO = (string)orderInfo["BillNO"]; } catch { Message += "订单单号不正确"; }

            string Counter = null;
            try { Counter = JsonConvert.SerializeObject(orderInfo["Counter"]); } catch { Message += "订单商品规格信息不正确"; }
            long GoodsID = 0;
            try { GoodsID = (long)orderInfo["Counter"]["GoodsID"]; } catch { Message += "订单商品编号不正确"; }

            long CounterID = 0;
            long SpecID = 0;
            if (orderInfo["Counter"]["CounterID"] == null)
            {
                //无规格
                try { CounterID = (long)orderInfo["Counter"]["ID"]; } catch { Message += "订单商品价格编号不正确"; }
            }
            else
            {
                //有规格
                try { CounterID = (long)orderInfo["Counter"]["CounterID"]; } catch { Message += "订单商品价格编号不正确"; }
                try { SpecID = (long)orderInfo["Counter"]["SpecID"]; } catch { Message += "订单商品规格编号不正确"; }
            }

            decimal Quantity = 0;
            try { Quantity = (decimal)orderInfo["Quantity"]; } catch { Message += "订单商品数量不正确"; }
            decimal Price = 0.00M;
            try { Price = (decimal)orderInfo["Counter"]["Price"]; } catch { Message += "订单商品单价不正确"; }
            decimal Amount = 0.00M;
            try { Amount = (decimal)orderInfo["Amount"]; } catch { Message += "订单商品金额不正确"; }
            // - 收货信息
            var receiveInfo = JsonConvert.DeserializeObject<JObject>(receive_info);
            string Consignee = null;
            try { Consignee = (string)receiveInfo["Consignee"]; } catch { Message += "收货信息联系人不正确"; }
            string Phone = null;
            try { Phone = (string)receiveInfo["Phone"]; } catch { Message += "收货信息联系电话不正确"; }
            int ReceiveTypeID = 0;
            try { ReceiveTypeID = Ziri.BLL.OMS.SalesOrder.ReceiveType.FirstOrDefault(i => i.Name == (string)receiveInfo["ReceiveType"]).ID; } catch { Message += "收货方式不正确"; }
            long StoreID = 0;
            try { StoreID = (long)receiveInfo["StoreID"]; } catch { Message += "自提门店编号不正确"; }
            string Address = null;
            try { Address = (string)receiveInfo["Address"]; } catch { Message += "收货地址不正确"; }
            string AddressCheck = null;
            try { AddressCheck = JsonConvert.SerializeObject(receiveInfo["AddressCheck"]); } catch { Message += "收货地址检查结果不正确"; }
            if (AddressCheck == "null") { AddressCheck = null; }
            // - 支付信息
            ListDict PayType = null;
            try { PayType = Ziri.BLL.OMS.SalesOrder.PayType.FirstOrDefault(i => i.Name == (string)receiveInfo["PayType"]); } catch { Message += "支付类型不正确"; }
            // - 备注
            string Remark = null;
            try { Remark = (string)receiveInfo["Remark"]; } catch { }
            // - 完成检查
            if (!string.IsNullOrWhiteSpace(Message)) { return ResponseWrite(new Result { Code = ResultCode.Error, Message = Message }); }
            Message = null;
            #endregion 检查表单

            #region 创建订单
            var SOInfo = new SOInfo
            {
                SalesOrder = new Ziri.MDL.SalesOrder
                {
                    ID = BillID,
                    BillNO = BillNO,
                    Remark = Remark,
                },
                SalesOrderItems = new List<SalesOrderItem>(),
                SOCustomerInfo = new SOCustomerInfo
                {
                    SOID = BillID,
                    CustomerTypeID = Ziri.BLL.OMS.SalesOrder.CustomerType.FirstOrDefault(i => i.Name == "WxUser").ID,
                    CustomerID = wxUserInfo.ID,
                },
                SOReceiveInfo = new SOReceiveInfo
                {
                    SOID = BillID,
                    Consignee = Consignee,
                    Phone = Phone,
                    ReceiveTypeID = ReceiveTypeID,
                    StoreID = StoreID,
                    Address = Address,
                    AddressCheck = AddressCheck,
                },
                SOPayInfo = new SOPayInfo
                {
                    SOID = BillID,
                    PayTypeID = PayType.ID,
                },
            };
            //历遍购物车（未实现）
            SOInfo.SalesOrderItems.Add(
                 new SalesOrderItem
                 {
                     GoodsID = GoodsID,
                     SpecID = SpecID,
                     CounterID = CounterID,
                     //Counter = Counter,
                     Quantity = Quantity,
                     Price = Price,
                     Amount = Amount,
                 }
                );
            SOInfo = Ziri.BLL.OMS.SalesOrder.SetSalesOrder(SOInfo, out Message);
            if (Message != null) { return ResponseWrite(new Result { Code = ResultCode.Error, Message = Message }); }
            #endregion 创建订单

            #region 支付信息
            bool Paid = false;
            object PayInfo = null;
            switch (PayType.Name)
            {
                case "WxPay":
                    Paid = Ziri.BLL.WeChat.GetWxPayOrderQuery(SOInfo, out Message) == "SUCCESS";
                    if (Message != null)
                    {
                        return ResponseWrite(new Result { Code = ResultCode.Success, Message = Message, Data = new { SOInfo, Paid, PayInfo } });
                    }
                    if (!Paid)
                    {
                        PayInfo = Ziri.BLL.WeChat.GetWxPayJSApiParam(wxUserInfo, SOInfo, out Message);
                        if (Message != null)
                        {
                            return ResponseWrite(new Result { Code = ResultCode.Success, Message = Message, Data = new { SOInfo, Paid, PayInfo } });
                        }
                    }
                    break;
            }
            #endregion 支付信息

            return ResponseWrite(new Result { Code = ResultCode.Success, Data = new { SOInfo, Paid, PayInfo } });
        }

        //获取订单支付单
        [WebMethod(EnableSession = true)]
        public object GetOrderPay(string user_code, int so_id)
        {
            string Message = null;

            #region 检查会话
            if (Context.Session["WxUserInfo"] == null)
            {
                if (string.IsNullOrWhiteSpace(user_code))
                {
                    return ResponseWrite(new Result { Code = ResultCode.Error, Message = "用户未登录，或者登录状态已失效，请重新登录后再操作。" });
                }
                var userInfo = Ziri.BLL.WeChat.GetWxUserInfoByJSCode(user_code, out Message);
                if (Message != null) { return ResponseWrite(new Result { Code = ResultCode.Error, Message = Message }); }
                Context.Session["WxUserInfo"] = userInfo;
            }
            var wxUserInfo = (WxUserInfo)Context.Session["WxUserInfo"];
            #endregion 检查会话

            // - 订单号
            var SOInfo = Ziri.BLL.OMS.SalesOrder.GetSalesOrder(so_id, out Message);
            if (Message != null) { return ResponseWrite(new Result { Code = ResultCode.Error, Message = Message }); }

            // - 支付信息
            ListDict PayType = Ziri.BLL.OMS.SalesOrder.PayType.FirstOrDefault(i => i.ID == SOInfo.SOPayInfo.PayTypeID);
            #region 支付信息
            bool Paid = false;
            object PayInfo = null;
            switch (PayType.Name)
            {
                case "WxPay":
                    Paid = Ziri.BLL.WeChat.GetWxPayOrderQuery(SOInfo, out Message) == "SUCCESS";
                    if (Message != null)
                    {
                        return ResponseWrite(new Result { Code = ResultCode.Success, Message = Message, Data = new { SOInfo, Paid, PayInfo } });
                    }
                    if (!Paid)
                    {
                        PayInfo = Ziri.BLL.WeChat.GetWxPayJSApiParam(wxUserInfo, SOInfo, out Message);
                        if (Message != null)
                        {
                            return ResponseWrite(new Result { Code = ResultCode.Success, Message = Message, Data = new { SOInfo, Paid, PayInfo } });
                        }
                    }
                    break;
            }
            #endregion 支付信息

            return ResponseWrite(new Result { Code = ResultCode.Success, Data = new { SOInfo, Paid, PayInfo } });
        }

        //设置订单支付结果
        [WebMethod(EnableSession = true)]
        public object SetOrderPaid(string user_code, string order_info)
        {
            string Message = null;

            //检查会话
            if (Context.Session["WxUserInfo"] == null)
            {
                if (string.IsNullOrWhiteSpace(user_code))
                {
                    return ResponseWrite(new Result { Code = ResultCode.Error, Message = "用户未登录，或者登录状态已失效，请重新登录后再操作。" });
                }
                var userInfo = Ziri.BLL.WeChat.GetWxUserInfoByJSCode(user_code, out Message);
                if (Message != null) { return ResponseWrite(new Result { Code = ResultCode.Error, Message = Message }); }
                Context.Session["WxUserInfo"] = userInfo;
            }
            var wxUserInfo = (WxUserInfo)Context.Session["WxUserInfo"];

            //检查表单
            var orderInfo = JsonConvert.DeserializeObject<JObject>(order_info);
            long BillID = 0;
            try { BillID = (long)orderInfo["BillID"]; } catch { }
            if (BillID == 0) { try { BillID = (long)orderInfo["SalesOrder"]["ID"]; } catch { Message = "订单编号不正确"; } }
            if (Message != null) { return ResponseWrite(new Result { Code = ResultCode.Error, Message = Message }); }
            var SOInfo = Ziri.BLL.OMS.SalesOrder.GetSalesOrder(BillID, out Message);
            if (Message != null) { return ResponseWrite(new Result { Code = ResultCode.Error, Message = Message }); }

            //检查支付状态
            var PayState = Ziri.BLL.WeChat.WxTradeState.FirstOrDefault(i => i.ID == SOInfo.SOPayInfo.StateID);
            bool Paid = PayState.Name == "SUCCESS";
            if (!Paid)
            {
                var PayType = Ziri.BLL.OMS.SalesOrder.PayType.FirstOrDefault(i => i.ID == SOInfo.SOPayInfo.PayTypeID);
                switch (PayType.Name)
                {
                    case "WxPay":
                        Paid = Ziri.BLL.WeChat.GetWxPayOrderQuery(SOInfo, out Message) == "SUCCESS";
                        if (Message != null) { return ResponseWrite(new Result { Code = ResultCode.Error, Message = Message }); }
                        break;
                }
            }

            return ResponseWrite(new Result { Code = ResultCode.Success, Data = new { SOInfo, Paid } });
        }

        //订单列表
        [WebMethod(EnableSession = true)]
        public object GetSOList(string user_code, string state_name, string pager)
        {
            string Message = null;

            //检查会话
            if (Context.Session["WxUserInfo"] == null)
            {
                if (string.IsNullOrWhiteSpace(user_code))
                {
                    return ResponseWrite(new Result { Code = ResultCode.Error, Message = "用户未登录，或者登录状态已失效，请重新登录后再操作。" });
                }
                var userInfo = Ziri.BLL.WeChat.GetWxUserInfoByJSCode(user_code, out Message);
                if (Message != null) { return ResponseWrite(new Result { Code = ResultCode.Error, Message = Message }); }
                Context.Session["WxUserInfo"] = userInfo;
            }
            var wxUserInfo = (WxUserInfo)Context.Session["WxUserInfo"];

            //状态统计
            var StateCount = new
            {
                Total = Ziri.BLL.OMS.SalesOrder.GetSOStateCount(wxUserInfo.ID, "全部"),
                Submit = Ziri.BLL.OMS.SalesOrder.GetSOStateCount(wxUserInfo.ID, "待付款"),
                Paid = Ziri.BLL.OMS.SalesOrder.GetSOStateCount(wxUserInfo.ID, "待提货"),
                Send = Ziri.BLL.OMS.SalesOrder.GetSOStateCount(wxUserInfo.ID, "待收货"),
            };
            //分页
            var pagerCount = new PagerCount
            {
                PageSize = 10,
                PageIndex = 1,
            };
            JObject Pager = null;
            try { Pager = JsonConvert.DeserializeObject<JObject>(pager); } catch { }
            if (Pager["PageSize"] != null) { try { pagerCount.PageSize = int.Parse(Pager["PageSize"].ToString()); } catch { } }
            if (Pager["PageIndex"] != null) { try { pagerCount.PageIndex = int.Parse(Pager["PageIndex"].ToString()); } catch { } }
            //订单页
            var list = Ziri.BLL.OMS.SalesOrder.GetSOList(wxUserInfo.ID, state_name, pagerCount, out PagerCount PageCountNew);
            //结果
            return ResponseWrite(new Result { Code = ResultCode.Success, Data = new { StateCount, list, pager = PageCountNew } });
        }

        //申请取消订单
        [WebMethod(EnableSession = true)]
        public object SetApplyCancel(string user_code, string so_id)
        {
            string Message = null;

            //检查会话
            if (Context.Session["WxUserInfo"] == null)
            {
                if (string.IsNullOrWhiteSpace(user_code))
                {
                    return ResponseWrite(new Result { Code = ResultCode.Error, Message = "用户未登录，或者登录状态已失效，请重新登录后再操作。" });
                }
                var userInfo = Ziri.BLL.WeChat.GetWxUserInfoByJSCode(user_code, out Message);
                if (Message != null) { return ResponseWrite(new Result { Code = ResultCode.Error, Message = Message }); }
                Context.Session["WxUserInfo"] = userInfo;
            }
            var wxUserInfo = (WxUserInfo)Context.Session["WxUserInfo"];

            //检查表单
            long BillID = 0;
            try { BillID = long.Parse(so_id); } catch { Message = "订单编号不正确"; }
            if (Message != null) { return ResponseWrite(new Result { Code = ResultCode.Error, Message = Message }); }

            Ziri.BLL.OMS.SalesOrder.SetApplyCancel(BillID, out Message);
            return ResponseWrite(new Result { Code = ResultCode.Success, Data = Message });
        }

        //设置收货
        [WebMethod(EnableSession = true)]
        public object SetTaken(string user_code, string so_id)
        {
            string Message = null;

            //检查会话
            if (Context.Session["WxUserInfo"] == null)
            {
                if (string.IsNullOrWhiteSpace(user_code))
                {
                    return ResponseWrite(new Result { Code = ResultCode.Error, Message = "用户未登录，或者登录状态已失效，请重新登录后再操作。" });
                }
                var userInfo = Ziri.BLL.WeChat.GetWxUserInfoByJSCode(user_code, out Message);
                if (Message != null) { return ResponseWrite(new Result { Code = ResultCode.Error, Message = Message }); }
                Context.Session["WxUserInfo"] = userInfo;
            }
            var wxUserInfo = (WxUserInfo)Context.Session["WxUserInfo"];

            //检查表单
            long BillID = 0;
            try { BillID = long.Parse(so_id); } catch { Message = "订单编号不正确"; }
            if (Message != null) { return ResponseWrite(new Result { Code = ResultCode.Error, Message = Message }); }

            Ziri.BLL.OMS.SalesOrder.SetTaken(BillID, out Message);
            return ResponseWrite(new Result { Code = ResultCode.Success, Data = Message });
        }

        //结果格式化
        private object ResponseWrite(Result result)
        {
            return new { Code = result.Code.ToString(), result.Message, result.Data };
            //以下小程序引发错误 System.RuntimeMethodHandle.InvokeMethod
            //Context.Response.Charset = "UTF-8";
            //Context.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            //Context.Response.Write(JsonConvert.SerializeObject(new { State = result.Code.ToString(), result.Message, result.Data }));
            //Context.Response.End();
        }
    }

    public class Result
    {
        public ResultCode Code { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }

    public enum ResultCode
    {
        Error,
        Success,
        Fail,
    }
}
