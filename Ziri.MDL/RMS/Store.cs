using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ziri.MDL
{
    //门店信息表
    [Table("StoreInfo")]
    public class StoreInfo
    {
        [Key]
        public long ID { get; set; }
        public long LogoFileID { get; set; }
        [Required, StringLength(32)]
        public string Name { get; set; }
        [Required, StringLength(32)]
        public string Title { get; set; }
        public bool Enabled { get; set; }
    }

    //门店联系信息表
    [Table("StoreContact")]
    public class StoreContact
    {
        [Key]
        public long ID { get; set; }
        public long StoreID { get; set; }
        public long ContactID { get; set; }
    }

    [Table("StorePhoto")]
    public class StorePhoto
    {
        [Key]
        public long ID { get; set; }
        public long StoreID { get; set; }
        public string FileIDs { get; set; }
    }

    [Table("StoreDesc")]
    public class StoreDesc
    {
        [Key]
        public long ID { get; set; }
        public long StoreID { get; set; }
        [StringLength(32)]
        public string BusinessHours { get; set; }
        public string Description { get; set; }
    }

    public class StoreFullInfo
    {
        public long ID { get; set; }
        public long LogoFileID { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public bool Enabled { get; set; }
        public string FileGUID { get; set; }
        public string FileName { get; set; }
        public string FileExtName { get; set; }
    }

    public class StoreDetails
    {
        public StoreFullInfo StoreFullInfo { get; set; }
        public StoreContact StoreContact { get; set; }
        public ContactInfo ContactInfo { get; set; }
        public string PhotoFileIDs { get; set; }
        public List<FileUploadInfo> PhotoUploadInfos { get; set; }
        public string BusinessHours { get; set; }
        public string Description { get; set; }
    }

    public class WxStoreList
    {
        public long ID { get; set; }
        public long LogoFileID { get; set; }
        public string LogoUrl { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public bool Enabled { get; set; }

        public string Mobile { get; set; }
        public string Phone { get; set; }
        public string EMail { get; set; }
        public string Address { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public double DistanceKM { get; set; }

        public string PhotoFileIDs { get; set; }
        public List<string> Photos { get; set; }

        public string Description { get; set; }
    }
}
