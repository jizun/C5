using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ziri.MDL
{
    //角色信息表
    [Table("RoleInfo")]
    public class RoleInfo
    {
        [Key]
        public long ID { get; set; }
        [StringLength(32)]
        public string IconFont { get; set; }
        [Required, StringLength(16)]
        public string Name { get; set; }
        [Required, StringLength(16)]
        public string Title { get; set; }
        public bool Enabled { get; set; }
    }

    //角色模块授权信息表
    [Table("RoleModuleAuth")]
    public class RoleModuleAuth
    {
        [Key]
        public long ID { get; set; }
        public long RoleID { get; set; }
        public long ModuleID { get; set; }
    }

    //角色操作授权信息表
    [Table("RoleActionAuth")]
    public class RoleActionAuth
    {
        [Key]
        public long ID { get; set; }
        public long RoleID { get; set; }
        public long ActionID { get; set; }
    }

    //角色信息列表
    public class RoleInfoList
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public bool Enabled { get; set; }
    }
}
