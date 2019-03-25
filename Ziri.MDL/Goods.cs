using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ziri.MDL
{
    [Table("GoodsInfo")]
    public class GoodsInfo
    {
        [Key]
        public long ID { get; set; }
        [StringLength(32)]
        public string Name { get; set; }
        [Required, StringLength(256)]
        public string Title { get; set; }
        public bool Enabled { get; set; }
        public int StateID { get; set; }
    }

    [Table("GoodsBrand")]
    public class GoodsBrand
    {
        [Key]
        public long ID { get; set; }
        public long GoodsID { get; set; }
        public long BrandID { get; set; }
    }

    [Table("GoodsKind")]
    public class GoodsKind
    {
        [Key]
        public long ID { get; set; }
        public long GoodsID { get; set; }
        public int KindLevel { get; set; }
        public long KindID { get; set; }
    }

    [Table("GoodsTag")]
    public class GoodsTag
    {
        [Key]
        public long ID { get; set; }
        public long GoodsID { get; set; }
        [Required, StringLength(16)]
        public string Title { get; set; }
    }

    [Table("GoodsSpec")]
    public class GoodsSpec
    {
        [Key]
        public long ID { get; set; }
        public long GoodsID { get; set; }
        [Required, StringLength(256)]
        public string SpecValueIDs { get; set; }
        [Required, StringLength(256)]
        public string SpecValues { get; set; }
        public bool Enabled { get; set; }
    }

    public class GoodsSpecFull
    {
        public long SpecID { get; set; }
        public long GoodsID { get; set; }
        public string SpecValueIDs { get; set; }
        public string SpecValues { get; set; }
        public bool Enabled { get; set; }
        public long CounterID { get; set; }
        public string SKU { get; set; }
        public string UPC { get; set; }
        public string EAN { get; set; }
        public string JAN { get; set; }
        public string ISBN { get; set; }
        public decimal? Price { get; set; }
        public decimal? Quantity { get; set; }
    }

    [Table("GoodsPhoto")]
    public class GoodsPhoto
    {
        [Key]
        public long ID { get; set; }
        public long GoodsID { get; set; }
        public string FileIDs { get; set; }
    }

    [Table("GoodsDesc")]
    public class GoodsDesc
    {
        [Key]
        public long ID { get; set; }
        public long GoodsID { get; set; }
        public string Description { get; set; }
    }

    [Table("GoodsCounter")]
    public class GoodsCounter
    {
        [Key]
        public long ID { get; set; }
        public long GoodsID { get; set; }
        public long GoodsSpecID { get; set; }
        [StringLength(32)]
        public string SKU { get; set; }
        [StringLength(32)]
        public string UPC { get; set; }
        [StringLength(32)]
        public string EAN { get; set; }
        [StringLength(32)]
        public string JAN { get; set; }
        [StringLength(32)]
        public string ISBN { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
    }

    public enum GoodsState
    {
        新建 = 1,
        下架 = 2,
        上架 = 3,
        删除 = 4,
    }

    public class GoodsInfoList
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public bool Enabled { get; set; }
        public int StateID { get; set; }
    }

    public class GoodsDetail
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public bool Enabled { get; set; }
        public int StateID { get; set; }
        public GoodsCounter GoodsCounter { get; set; }
        public long? GoodsBrandID { get; set; }
        public BrandFullInfo BrandFullInfo { get; set; }
        public List<GoodsKind> GoodsKinds { get; set; }
        public List<KindInfo> KindInfos { get; set; }
        public List<GoodsTag> GoodsTags { get; set; }
        public List<GoodsSpecFull> GoodsSpecsFull { get; set; }
        public List<SpecValue> SpecValues { get; set; }
        public List<SpecInfo> SpecInfos { get; set; }
        public string PhotoFileIDs { get; set; }
        public List<FileUploadInfo> PhotoUploadInfos { get; set; }
        public string Description { get; set; }
    }

    public class MallGoodsDetail
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public long BrandID { get; set; }
        public string PhotoFileIDs { get; set; }
        public List<string> Photos { get; set; }
        public string Price { get; set; }
        public List<string> Tags { get; set; }
        public GoodsCounter GoodsCounter { get; set; }          //单价
        public List<GoodsSpecFull> GoodsSpecsFull { get; set; } //规格列表
        public List<SpecInfo> SpecInfos { get; set; }           //规格目录  先列目录，根据目录列出值，然后根据选择值的ID组哈，从规格列表中对应价格
        public List<SpecValue> SpecValues { get; set; }         //规格值
        public string Description { get; set; }
    }
}
