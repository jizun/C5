using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ziri.MDL
{
    [Table("SalesOrder")]
    public class SalesOrder
    {
        [Key]
        public long ID { get; set; }
        [StringLength(32)]
        public string BillNO { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public int StateID { get; set; }
        [StringLength(1024)]
        public string Remark { get; set; }
    }

    [Table("SalesOrderItem")]
    public class SalesOrderItem
    {
        [Key]
        public long ID { get; set; }
        public long SOID { get; set; }
        public int OrderID { get; set; }

        public long GoodsID { get; set; }
        public long SpecID { get; set; }
        public long CounterID { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
    }

    [Table("SOCustomerInfo")]
    public class SOCustomerInfo
    {
        [Key]
        public long ID { get; set; }
        public long SOID { get; set; }
        public int CustomerTypeID { get; set; }
        public long CustomerID { get; set; }
    }

    [Table("SOReceiveInfo")]
    public class SOReceiveInfo
    {
        [Key]
        public long ID { get; set; }
        public long SOID { get; set; }
        [StringLength(32)]
        public string Consignee { get; set; }
        [StringLength(32)]
        public string Phone { get; set; }
        public int ReceiveTypeID { get; set; }
        public long StoreID { get; set; }
        [StringLength(1024)]
        public string Address { get; set; }
        public string AddressCheck { get; set; }
    }

    [Table("SOPayInfo")]
    public class SOPayInfo
    {
        [Key]
        public long ID { get; set; }
        public long SOID { get; set; }
        public int PayTypeID { get; set; }
        [StringLength(64)]
        public string PrePayID { get; set; }
        [StringLength(64)]
        public string TransactionID { get; set; }
        public int StateID { get; set; }
        public long? PayID { get; set; }
    }

    public class SOInfo
    {
        public SalesOrder SalesOrder { get; set; }
        public List<SalesOrderItem> SalesOrderItems { get; set; }
        public SOCustomerInfo SOCustomerInfo { get; set; }
        public SOReceiveInfo SOReceiveInfo { get; set; }
        public SOPayInfo SOPayInfo { get; set; }

        public string SOStateTitle { get; set; }
        public string CustomerTypeTitle { get; set; }
        public string CustomerNickName { get; set; }
        public string CustomerGender { get; set; }
        public string CustomerAvatar { get; set; }
        public List<GoodsInfo> GoodsInfos { get; set; }
        public List<SOItem> SOItems { get; set; }
        public string ReceiptTypeTitle { get; set; }
        public StoreInfo ReceiptStore { get; set; }
        public string PayTypeTitle { get; set; }
        public string PayStateTitle { get; set; }
        public F2FPayInfo F2FPayInfo { get; set; }
    }

    public class SOFullInfo
    {
        public long ID { get; set; }
        public string BillNO { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public int CustomerTypeID { get; set; }
        public long CustomerID { get; set; }
        public string CustomerName { get; set; }
        public int StateID { get; set; }
        public string StateTitle { get; set; }
        public int PayTypeID { get; set; }
        public string PayTypeTitle { get; set; }
        public List<SOItem> SOItems { get; set; }

        public long GoodsID { get; set; }
        public string GoodsName { get; set; }
        public string GoodsTitle { get; set; }
        public long GoodsSpecID { get; set; }
        public string GoodsSpecValues { get; set; }

        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }

        public string Consignee { get; set; }
        public string Phone { get; set; }

        public int ReceiveTypeID { get; set; }
        public string ReceiveTypeTitle { get; set; }
        public long StoreID { get; set; }
        public string StoreName { get; set; }
        public string StoreTitle { get; set; }
        public string Address { get; set; }
        public string AddressCheck { get; set; }
    }

    public class SOItem
    {
        public int OrderID { get; set; }
        public long GoodsID { get; set; }
        public string GoodsName { get; set; }
        public string GoodsTitle { get; set; }
        public string GoodsPhotoIDs { get; set; }
        public List<string> GoodsPhotos { get; set; }
        public string GoodsSpecTitle { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
    }

    [Table("F2FPay")]
    public class F2FPay
    {
        [Key]
        public long ID { get; set; }
        [StringLength(32)]
        public string BillNO { get; set; }
        public DateTime CreateTime { get; set; }
        public string Remark { get; set; }
    }

    [Table("F2FPayItem")]
    public class F2FPayItem
    {
        [Key]
        public long ID { get; set; }
        public long BillID { get; set; }
        public int PayTypeID { get; set; }
        public decimal Amount { get; set; }
        public string TransactionID { get; set; }
    }

    public class F2FPayInfo
    {
        public F2FPay F2FPay { get; set; }
        public List<F2FPayItem> F2FPayItems { get; set; }
    }
}
