using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using Ziri.MDL;
using Ziri.DAL;

namespace Ziri.BLL
{
    public class Pay
    {
        public static F2FPayInfo SetF2FPay(F2FPayInfo F2FPayInfo, out string Message)
        {
            using (var TS = new TransactionScope())
            {
                using (var EF = new EF())
                {
                    var pay_exist = EF.F2FPays.FirstOrDefault(i => i.ID == F2FPayInfo.F2FPay.ID);
                    if (pay_exist == null)
                    {
                        EF.F2FPays.Add(pay_exist = new F2FPay
                        {
                            BillNO = "PAY" + DateTime.Now.ToString("yyyyMMddHHmmssffff"),
                            CreateTime = DateTime.Now,
                        });
                    }
                    pay_exist.Remark = F2FPayInfo.F2FPay.Remark;
                    EF.SaveChanges();
                    F2FPayInfo.F2FPay = pay_exist;

                    var item_exist = EF.F2FPayItems.Where(i => i.BillID == pay_exist.ID);
                    if (item_exist != null)
                    {
                        EF.F2FPayItems.RemoveRange(item_exist);
                        EF.SaveChanges();
                    }
                    foreach (var item in F2FPayInfo.F2FPayItems)
                    {
                        EF.F2FPayItems.Add(new F2FPayItem
                        {
                            BillID = pay_exist.ID,
                            Amount = item.Amount,
                            PayTypeID = item.PayTypeID,
                            TransactionID = item.TransactionID,
                        });
                    }
                    EF.SaveChanges();
                    TS.Complete();
                }
            }
            return GetF2FPayInfo(F2FPayInfo.F2FPay.ID, out Message);
        }

        public static F2FPayInfo GetF2FPayInfo(long PayID, out string Message)
        {
            using (var EF = new EF())
            {
                var pay_exist = EF.F2FPays.FirstOrDefault(i => i.ID == PayID);
                if (pay_exist == null)
                {
                    Message = "支付号" + PayID + "不存在";
                    return null;
                }
                Message = null;
                return new F2FPayInfo
                {
                    F2FPay = pay_exist,
                    F2FPayItems = EF.F2FPayItems.Where(i => i.BillID == pay_exist.ID).ToList(),
                };
            }
        }
    }
}
