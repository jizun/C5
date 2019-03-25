using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ziri.MDL
{
    [Table("BrandInfo")]
    public class BrandInfo
    {
        [Key]
        public long ID { get; set; }
        public long LogoFileID { get; set; }
        public long BannerFileID { get; set; }
        [Required, StringLength(32)]
        public string Name { get; set; }
        [Required, StringLength(32)]
        public string Title { get; set; }
        public bool Enabled { get; set; }
    }

    public class BrandFullInfo
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public bool Enabled { get; set; }
        public long LogoFileID { get; set; }
        public string LogoFileGUID { get; set; }
        public string LogoFileName { get; set; }
        public string LogoFileExtName { get; set; }
        public long BrandFileID { get; set; }
        public string BrandFileGUID { get; set; }
        public string BrandFileName { get; set; }
        public string BrandFileExtName { get; set; }
    }
}
