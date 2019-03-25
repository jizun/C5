using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ziri.MDL
{
    //用户信息数据表
    [Table("UserInfo")]
    public class UserInfo
    {
        [Key]
        public long ID { get; set; }
        [Required, StringLength(36)]
        public string GUID { get; set; }
        [Required, StringLength(64)]
        public string Name { get; set; }
        [Required, StringLength(64)]
        public string Password { get; set; }
        public int StateID { get; set; }
        public DateTime UpdateTime { get; set; }
    }

    //用户信息日志表
    [Table("UserInfo_log")]
    public class UserInfo_log
    {
        [Key]
        public long ID { get; set; }
        public long UserID { get; set; }
        public string GUID { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public int StateID { get; set; }
        public DateTime UpdateTime { get; set; }
    }

    //用户个人信息关系表
    [Table("UserAvatar")]
    public class UserAvatar
    {
        [Key]
        public long ID { get; set; }
        public long UserID { get; set; }
        public long FileID { get; set; }
        public long? LogID { get; set; }
    }

    //用户个人信息关系表
    [Table("UserPerson")]
    public class UserPerson
    {
        [Key]
        public long ID { get; set; }
        public long UserID { get; set; }
        public long PersonID { get; set; }
        public long? LogID { get; set; }
    }

    //用户联系信息关系表
    [Table("UserContact")]
    public class UserContact
    {
        [Key]
        public long ID { get; set; }
        public long UserID { get; set; }
        public long ContactID { get; set; }
        public long? LogID { get; set; }
    }

    //用户通知信息关系表
    [Table("UserNotifications")]
    public class UserNotifications
    {
        [Key]
        public long ID { get; set; }
        public long UserID { get; set; }
        public long NotificationsID { get; set; }
    }

    //用户模块权限表
    [Table("UserModuleAuth")]
    public class UserModuleAuth
    {
        [Key]
        public long ID { get; set; }
        public long UserID { get; set; }
        public long ModuleID { get; set; }
    }

    //用户操作权限表
    [Table("UserActionAuth")]
    public class UserActionAuth
    {
        [Key]
        public long ID { get; set; }
        public long UserID { get; set; }
        public long ActionID { get; set; }
    }

    //用户角色信息表
    [Table("UserRole")]
    public class UserRole
    {
        [Key]
        public long ID { get; set; }
        public long UserID { get; set; }
        public long RoleID { get; set; }
    }

    //用户角色指定信息
    public class UserRoleAssign
    {
        public long ID { get; set; }
        public string IconFont { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public bool Enabled { get; set; }
        public long UserID { get; set; }
        public bool UserAssign { get; set; }
    }

    //用户信息状态
    public enum UserState
    {
        创建 = 1,   //Created
        提交 = 2,   //Submitted
        审核 = 3,   //Checked
        撤销 = 4,   //Canceled
        启用 = 5,   //Enabled
        禁用 = 6,   //Disabled
        删除 = 7    //Deleted
    }

    //用户信息列表
    public class UserInfoList
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public int StateID { get; set; }
        public DateTime UpdateTime { get; set; }
    }

    //用户登录信息
    public class LoginInfo
    {
        public UserInfo UserInfo { get; set; }
        public string UserAvatarURL { get; set; }
        public string UserRoleName { get; set; }
    }

    //个人信息数据表
    [Table("PersonInfo")]
    public class PersonInfo
    {
        [Key]
        public long ID { get; set; }
        [StringLength(64)]
        public string Name { get; set; }
        public int GenderTypeID { get; set; }
        public DateTime? Birthday { get; set; }
        [StringLength(18)]
        public string IDCardNO { get; set; }
    }

    //性别
    public enum GenderType
    {
        其他 = 0,
        男性 = 1,
        女性 = 2,
    }

    //联系信息数据表
    [Table("ContactInfo")]
    public class ContactInfo
    {
        [Key]
        public long ID { get; set; }
        [StringLength(16)]
        public string Mobile { get; set; }
        [StringLength(16)]
        public string Phone { get; set; }
        [StringLength(64)]
        public string EMail { get; set; }
        [StringLength(256)]
        public string Address { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}
