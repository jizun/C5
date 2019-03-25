using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Com.Alipay;
using Com.Alipay.Domain;
using Com.Alipay.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ziri.MDL;

namespace Ziri.BLL
{
    //支付宝当面付https://docs.open.alipay.com/194/105170/
    public class AliPay
    {
        IAlipayTradeService serviceClient = F2FBiz.CreateClientInstance(
            AliPayAPI.serverUrl,
            AliPayAPI.appId,
            AliPayAPI.merchant_private_key,
            AliPayAPI.version,
            AliPayAPI.sign_type,
            AliPayAPI.alipay_public_key,
            AliPayAPI.charset);

        //条码支付https://docs.open.alipay.com/194/106039/
        //统一收单字段https://docs.open.alipay.com/api_1/alipay.trade.pay
        public JObject F2FPay(SOInfo SOInfo, string PayCode, out string Message)
        {
            var data = new AlipayTradePayContentBuilder()
            {
                subject = SOInfo.SalesOrder.BillNO,         //订单名称
                body = "test",                              //订单描述
                out_trade_no = SOInfo.SalesOrder.BillNO,    //订单编号
                total_amount = SOInfo.SOItems.Sum(i => i.Amount).ToString("0.00"),  //订单金额
                scene = "bar_code",                         //支付场景
                auth_code = PayCode,                        //付款条码
            };

            var result = serviceClient.tradePay(data);
            //{
            //"alipay_trade_pay_response": {
            //    "code": "10000",
            //    "msg": "Success",
            //    "buyer_logon_id": "jiz***@yahoo.cn",
            //    "buyer_pay_amount": "0.01",
            //    "buyer_user_id": "2088102549201514",
            //    "fund_bill_list": [
            //      {
            //        "amount": "0.01",
            //        "fund_channel": "PCREDIT"
            //      }
            //    ],
            //    "gmt_payment": "2019-03-19 17:00:54",
            //    "invoice_amount": "0.01",
            //    "out_trade_no": "SO201903191700107879",
            //    "point_amount": "0.00",
            //    "receipt_amount": "0.01",
            //    "total_amount": "0.01",
            //    "trade_no": "2019031922001401510517716161"
            //  },
            //  "sign": "D020VIGsiIqygRJLlSCEBulNxzN8KBMiUS+C/wW8tKZV6PaUc4OMSxc9uAM2UDhVlPDIdPsMZoIkTZ1BsDZldAR9k28oZvG9gAPluuHkHAvdqMgLDbvhOIBY3C7qT9tYe1M2n7HkruFP9nmhw09z1mCbG8EeH8s9zPMY8CG3QnQ+lDcbmivFXveCel7YmVP+AfXxTo/DKcf937ynOhDK1sinuG16kWOgZtxHVP9iPrF1DaVBmS3OnD/dYPm10QVY4OaMfw7fsPSH8moBGSUbsk7z4p95VmTT9JDdF/1wEK40LdXt6Tyo9/zNMhIv7sjdISvIyyu9nJ3ouPCtrwNFiA=="
            //}
            var resultObject = JsonConvert.DeserializeObject<JObject>(result.response.Body);
            switch (result.Status)
            {
                case ResultEnum.SUCCESS: Message = null; return resultObject;
                case ResultEnum.FAILED:
                    if (resultObject["alipay_trade_pay_response"]["sub_code"].ToString() == "ACQ.TRADE_HAS_SUCCESS")
                    {
                        Message = null;
                        return resultObject;
                    }
                    Message = resultObject["alipay_trade_pay_response"]["sub_msg"].ToString();
                    return null;
                case ResultEnum.UNKNOWN: Message = "网络异常，请检查网络配置后，更换外部订单号重试"; return null;
                default: Message = "支付宝交易状态（" + result.Status + "）未定义"; return null;
            }
        }

        //public YHJ_AliPayResult AliPayDataSave(JToken data)
        //{
        //    using (var EF = new DataAccess.RMSDataHelperEF())
        //    {
        //        var result = new YHJ_AliPayResult
        //        {
        //            code = data["code"].ToString(),
        //            msg = data["msg"].ToString(),
        //            sub_code = data["sub_code"] == null ? null : data["sub_code"].ToString(),
        //            sub_msg = data["sub_msg"] == null ? null : data["sub_msg"].ToString(),
        //            buyer_logon_id = data["buyer_logon_id"].ToString(),
        //            buyer_pay_amount = decimal.Parse(data["buyer_pay_amount"].ToString()),
        //            buyer_user_id = data["buyer_user_id"].ToString(),
        //            gmt_payment = DateTime.Parse(data["gmt_payment"].ToString()),
        //            invoice_amount = decimal.Parse(data["invoice_amount"].ToString()),
        //            out_trade_no = data["out_trade_no"].ToString(),
        //            point_amount = decimal.Parse(data["point_amount"].ToString()),
        //            receipt_amount = decimal.Parse(data["receipt_amount"].ToString()),
        //            total_amount = decimal.Parse(data["total_amount"].ToString()),
        //            trade_no = data["trade_no"].ToString(),
        //        };
        //        EF.YHJ_AliPayResult.Add(result);
        //        EF.SaveChanges();
        //        return result;
        //    }
        //}
    }
}
