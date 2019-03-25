using System;
using System.Linq;
using System.Configuration;
using System.Collections.Generic;
using Ziri.MDL;
using Ziri.DAL;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WxPayAPI;
using System.Threading;

namespace Ziri.BLL
{
    public class WeChat
    {
        //微信API
        private static string AppID = ConfigurationManager.ConnectionStrings["WxProgramAppID"].ToString();
        private static string AppSecret = ConfigurationManager.ConnectionStrings["WxProgramAppSecret"].ToString();
        private static string GetWebResult(string url, out string Message)
        {
            using (WebClient webClient = new WebClient())
            {
                try
                {
                    Message = null;
                    return webClient.DownloadString(url);
                }
                catch (Exception e)
                {
                    Message = e.Message;
                    return null;
                }
            }
        }

        #region 字典
        //微信支付状态
        public static List<ListDict> WxTradeState = new List<ListDict>
        {
            new ListDict { ID = 1, Name = "SUCCESS", Title = "支付成功", },
            new ListDict { ID = 2, Name = "REFUND", Title = "转入退款", },
            new ListDict { ID = 3, Name = "NOTPAY", Title = "未支付", },
            new ListDict { ID = 4, Name = "CLOSED", Title = "已关闭", },
            new ListDict { ID = 5, Name = "REVOKED", Title = "已撤销（刷卡支付）", },
            new ListDict { ID = 6, Name = "USERPAYING", Title = "用户支付中", },
            new ListDict { ID = 7, Name = "PAYERROR", Title = "支付失败(其他原因，如银行返回失败)", },
        };

        //微信用户语言
        public static List<ListDict> WxUserLang = new List<ListDict>
        {
            new ListDict { ID = 1, Name = "en", Title = "英文", },
            new ListDict { ID = 2, Name = "zh_CN", Title = "简体中文", },
            new ListDict { ID = 3, Name = "zh_TW", Title = "繁体中文", },
        };

        //微信用户性别
        public static List<ListDict> WxUserGender = new List<ListDict>
        {
            new ListDict { ID = 1, Name = "0", Title = "未知", },
            new ListDict { ID = 2, Name = "1", Title = "男", },
            new ListDict { ID = 3, Name = "2", Title = "女", },
        };

        #endregion 字典

        //获取微信用户信息
        public static WxUserInfo GetWxUserInfoByJSCode(string JSCode, out string Message)
        {
            var url = string.Format("https://api.weixin.qq.com/sns/jscode2session?appid={0}&secret={1}&js_code={2}&grant_type=authorization_code"
                    , AppID, AppSecret, JSCode);
            var result = GetWebResult(url, out Message);
            if (Message != null) { return null; }
            var api_data = JsonConvert.DeserializeObject<JObject>((string)result);
            //{"errmsg": "invalid appid, hints: [ req_id: 0ILAm44ce-Wheh5 ]"}
            //{"session_key":"NKuxRTlZitw3KGcmxhHqTA==","openid":"o4iXa1ZJ5MgEJwNWZsbaC0PV7xcw"}
            if (api_data["errmsg"] != null) { Message = (string)api_data["errmsg"]; return null; }
            return GetWxUserInfoByOpenID(api_data["openid"].ToString());
        }
        public static WxUserInfo GetWxUserInfoByOpenID(string OpenID)
        {
            using (var EF = new EF())
            {
                var user_exist = EF.WxUserInfos.FirstOrDefault(i => i.OpenID == OpenID);
                if (user_exist == null)
                {
                    EF.WxUserInfos.Add(user_exist = new WxUserInfo { OpenID = OpenID, CreateTime = DateTime.Now });
                    EF.SaveChanges();
                }
                return user_exist;
            }
        }

        //设置微信用户信息
        public static WxUserInfo SetWxUserInfo(WxUserInfo WxUserInfo)
        {
            using (var EF = new EF())
            {
                var user_exist = EF.WxUserInfos.FirstOrDefault(i => i.OpenID == WxUserInfo.OpenID);
                if (user_exist == null)
                {
                    EF.WxUserInfos.Add(user_exist = new WxUserInfo { OpenID = WxUserInfo.OpenID, CreateTime = DateTime.Now });
                }
                user_exist.NickName = WxUserInfo.NickName;
                user_exist.Gender = WxUserInfo.Gender;
                user_exist.Language = WxUserInfo.Language;
                user_exist.City = WxUserInfo.City;
                user_exist.Province = WxUserInfo.Province;
                user_exist.Country = WxUserInfo.Country;
                user_exist.AvatarUrl = WxUserInfo.AvatarUrl;
                user_exist.UpdateTime = DateTime.Now;
                EF.SaveChanges();
                return user_exist;
            }
        }

        //获取门店列表
        public static List<WxStoreList> GetWxStoreLists(double? Latitude, double? Longitude)
        {
            using (var EF = new EF())
            {
                var list = (from storeInfo in EF.StoreInfos
                            join fileInfo in EF.FileInfos on storeInfo.LogoFileID equals fileInfo.ID into temp1
                            from fileInfo in temp1.DefaultIfEmpty()
                            join fileExtName in EF.FileExtName on fileInfo.ExtNameID equals fileExtName.ID into temp2
                            from fileExtName in temp2.DefaultIfEmpty()
                            join storeContcat in EF.StoreContacts on storeInfo.ID equals storeContcat.StoreID
                            join contcatInfo in EF.ContactInfos on storeContcat.ContactID equals contcatInfo.ID
                            join storePhoto in EF.StorePhotos on storeInfo.ID equals storePhoto.StoreID into temp3
                            from storePhoto in temp3.DefaultIfEmpty()
                            join storeDesc in EF.StoreDescs on storeInfo.ID equals storeDesc.StoreID into temp4
                            from storeDesc in temp4.DefaultIfEmpty()
                            where storeInfo.Enabled == true
                            select new WxStoreList
                            {
                                ID = storeInfo.ID,
                                LogoFileID = storeInfo.LogoFileID,
                                LogoUrl = fileInfo.GUID + fileExtName.Name,
                                Name = storeInfo.Name,
                                Title = storeInfo.Title,
                                Enabled = storeInfo.Enabled,
                                PhotoFileIDs = storePhoto.FileIDs,
                                Mobile = contcatInfo.Mobile,
                                Phone = contcatInfo.Phone,
                                EMail = contcatInfo.EMail,
                                Address = contcatInfo.Address,
                                Latitude = contcatInfo.Latitude,
                                Longitude = contcatInfo.Longitude,

                                Description = storeDesc.Description,
                            }).ToList();
                foreach (var item in list)
                {
                    //照片
                    if (!string.IsNullOrWhiteSpace(item.PhotoFileIDs))
                    {
                        var PhotoFileIDs = item.PhotoFileIDs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();
                        item.Photos = (from fileInfo in EF.FileInfos
                                       join extNameInfo in EF.FileExtName on fileInfo.ExtNameID equals extNameInfo.ID
                                       where PhotoFileIDs.Contains(fileInfo.ID)
                                       select fileInfo.GUID + extNameInfo.Name).ToList();
                    }
                    //距离计算
                    if (Latitude != null && Longitude != null)
                    {
                        item.DistanceKM = MapHelper.GetDistance(Latitude ?? 0, Longitude ?? 0, (double)item.Latitude, (double)item.Longitude);
                    }
                }
                return list.OrderBy(i => i.DistanceKM).ToList();
            }
        }

        //获取微信支付JSAPI参数
        public static string GetWxPayJSApiParam(WxUserInfo WxUserInfo, SOInfo SOInfo, out string Message)
        {
            //统一下单https://pay.weixin.qq.com/wiki/doc/api/jsapi.php?chapter=9_1
            var data = new WxPayData();
            //var goods_name = string.Join(",", SOInfo.GoodsInfos.Select(i => i.Title).ToArray());
            data.SetValue("body", "test");    //goods_name.Length > 12 ? goods_name.Substring(0, 10) + "..." : goods_name
                                              //data.SetValue("attach", "test");    //附加数据
            data.SetValue("out_trade_no", SOInfo.SalesOrder.BillNO);
            data.SetValue("total_fee", (int)Math.Round(SOInfo.SalesOrderItems.Sum(i => i.Amount) * 100, 0));
            data.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));
            data.SetValue("time_expire", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss"));
            //data.SetValue("goods_tag", "test");   //订单优惠标记
            data.SetValue("trade_type", "JSAPI");
            data.SetValue("openid", WxUserInfo.OpenID);

            WxPayData result = null;
            try { result = WxPayApi.UnifiedOrder(data); }
            catch (Exception e) { Message = e.Message; return null; }
            if (result.GetValue("return_code") == null) { Message = "微信统一支付接口返回空值"; return null; }
            if (result.GetValue("return_code").ToString() != "SUCCESS") { Message = result.GetValue("return_msg").ToString(); return null; }

            //小程序支付文档https://pay.weixin.qq.com/wiki/doc/api/wxa/wxa_api.php?chapter=7_7&index=5
            var jsApiParam = new WxPayData();
            jsApiParam.SetValue("appId", result.GetValue("appid"));
            jsApiParam.SetValue("timeStamp", WxPayApi.GenerateTimeStamp());
            jsApiParam.SetValue("nonceStr", WxPayApi.GenerateNonceStr());
            jsApiParam.SetValue("package", "prepay_id=" + result.GetValue("prepay_id"));
            jsApiParam.SetValue("signType", "MD5");
            jsApiParam.SetValue("paySign", jsApiParam.MakeSign());

            //本地记录
            OMS.SalesOrder.SetPayInfo(SOInfo.SalesOrder.ID, new SOPayInfo { PrePayID = result.GetValue("prepay_id").ToString(), }, false, out Message);
            if (Message != null) { return null; }

            Message = null;
            return jsApiParam.ToJson();
        }

        //获取微信支付结果
        public static string GetWxPayOrderQuery(SOInfo SOInfo, out string Message)
        {
            var data = new WxPayData();
            if (string.IsNullOrEmpty(SOInfo.SOPayInfo.TransactionID)) { data.SetValue("out_trade_no", SOInfo.SalesOrder.BillNO); }
            else { data.SetValue("transaction_id", SOInfo.SOPayInfo.TransactionID); }
            var result = WxPayApi.OrderQuery(data);

            //通信标识
            if (result.GetValue("return_code") == null) { Message = "微信查询订单接口返回空值"; return null; }
            if (result.GetValue("return_code").ToString() != "SUCCESS") { Message = result.GetValue("return_msg").ToString(); return null; }

            //业务结果
            var result_code = result.GetValue("result_code").ToString();
            if (result_code != "SUCCESS")
            {
                Message = null;
                return result_code;
            }

            //交易状态
            var trade_state = WxTradeState.FirstOrDefault(i => i.Name == result.GetValue("trade_state").ToString());
            using (var EF = new EF())
            {
                var order_exist = EF.SalesOrders.FirstOrDefault(i => i.ID == SOInfo.SalesOrder.ID);
                if (order_exist == null) { Message = "订单编号" + SOInfo.SalesOrder.ID + "不存在"; return null; }
                var pay_exist = EF.SOPayInfos.FirstOrDefault(i => i.SOID == order_exist.ID);
                pay_exist.StateID = trade_state.ID;
                if (trade_state.Name == "SUCCESS")
                {
                    order_exist.StateID = OMS.SalesOrder.State.FirstOrDefault(i => i.Name == "Paid").ID;
                    pay_exist.TransactionID = result.GetValue("transaction_id").ToString();
                }
                EF.SaveChanges();
            }

            Message = null;
            return trade_state.Name;
        }

        //微信付款码支付https://pay.weixin.qq.com/wiki/doc/api/micropay.php?chapter=9_10&index=1
        public static WxPayData MicroPay(SOInfo SOInfo, string PayCode, out string Message)
        {
            //支付字典
            var data = new WxPayData();
            data.SetValue("auth_code", PayCode);                        //支付条码
            data.SetValue("out_trade_no", SOInfo.SalesOrder.BillNO);    //订单编号
            data.SetValue("body", "test");                              //订单描述
            data.SetValue("total_fee", (int)(SOInfo.SalesOrderItems.Sum(i => i.Amount) * 100)); //订单金额

            //发起支付
            var result = WxPayApi.Micropay(data, 10);
            //appid, wx1225898414356c5e
            //err_code, USERPAYING
            //err_code_des, 需要用户输入支付密码
            //mch_id, 1498738112
            //nonce_str, zllmCT5ykztv0WQW
            //result_code, FAIL
            //return_code, SUCCESS
            //return_msg, OK
            //sing, E003D8...
            if (!result.IsSet("return_code") || result.GetValue("return_code").ToString() == "FAIL")
            {
                Message = result.IsSet("return_msg") ? result.GetValue("return_msg").ToString() : "";
                return null;
            }

            //签名验证（公众平台与小程序混用APPID导致签名错误待梳理）
            //bool CheckSing = false;
            //try { CheckSing = result.CheckSign(); } catch (Exception e) { Message = e.Message; return null; }
            //if (CheckSing) { Message = "微信支付结果签名错误"; return null; };

            //直接成功
            if (result.GetValue("return_code").ToString() == "SUCCESS" && result.GetValue("result_code").ToString() == "SUCCESS")
            {
                Message = null;
                return result;
            }

            //1）业务结果明确失败
            if (result.GetValue("err_code").ToString() != "USERPAYING" && result.GetValue("err_code").ToString() != "SYSTEMERROR")
            {
                //MicroPay.Cancel(data.GetValue("out_trade_no").ToString());
                Message = result.GetValue("err_code_des").ToString();
                return null;
            }

            //2）不能确定是否失败（可能需要客户APP输密码），轮询10次查单
            string out_trade_no = data.GetValue("out_trade_no").ToString();
            int queryTimes = 10;
            while (queryTimes-- > 0)
            {
                int succResult = 0;
                var queryResult = WxPayAPI.MicroPay.Query(out_trade_no, out succResult);
                if (succResult == 2) { Thread.Sleep(2000); continue; }  //如果需要继续查询，则等待2s后继续
                else if (succResult == 1)
                {
                    result = queryResult;
                    break;
                }
                else
                {
                    WxPayAPI.MicroPay.Cancel(out_trade_no);
                    Message = result.ToPrintStr();
                    return null;
                }
            }
            Message = null;
            return result;
        }
    }
}
