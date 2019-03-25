using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ziri.MDL
{
    [Table("WxUserInfo")]
    public class WxUserInfo
    {
        [Key]
        public long ID { get; set; }
        [Required, StringLength(32)]
        public string OpenID { get; set; }
        [StringLength(32)]
        public string NickName { get; set; }
        [StringLength(32)]
        public string Gender { get; set; }
        [StringLength(32)]
        public string Language { get; set; }
        [StringLength(32)]
        public string City { get; set; }
        [StringLength(32)]
        public string Province { get; set; }
        [StringLength(32)]
        public string Country { get; set; }
        [StringLength(256)]
        public string AvatarUrl { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
