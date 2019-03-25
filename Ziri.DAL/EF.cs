using System.Configuration;
using System.Data.Entity;
using Ziri.MDL;

namespace Ziri.DAL
{
    public class EF : EFContextBase
    {
        public EF() : base(ConfigurationManager.ConnectionStrings["C5DB"].ToString()) { }

        #region 系统
        public virtual DbSet<ModuleInfo> ModuleInfos { set; get; }
        public virtual DbSet<ActionInfo> ActionInfos { get; set; }
        public virtual DbSet<ModuleAction> ModuleActions { get; set; }
        public virtual DbSet<MenuInfo> MenuInfos { get; set; }
        public virtual DbSet<MenuGroup> MenuGroups { get; set; }
        public virtual DbSet<Slogan> Slogans { set; get; }
        public virtual DbSet<Notifications> Notifications { get; set; }
        #endregion

        #region 文档
        public virtual DbSet<DocFolder> DocFolder { set; get; }
        public virtual DbSet<DocFile> DocFile { set; get; }
        public virtual DbSet<FileExtName> FileExtName { set; get; }
        public virtual DbSet<FileMIME> FileMIME { set; get; }
        public virtual DbSet<Doc> Doc { set; get; }
        public virtual DbSet<DocPath> DocPath { set; get; }
        public virtual DbSet<UploadInfo> UploadInfos { get; set; }
        public virtual DbSet<FileInfo> FileInfos { get; set; }
        #endregion

        #region 用户
        public virtual DbSet<PersonInfo> PersonInfos { get; set; }
        public virtual DbSet<ContactInfo> ContactInfos { get; set; }
        public virtual DbSet<UserInfo> UserInfos { set; get; }
        public virtual DbSet<UserInfo_log> UserInfo_logs { set; get; }
        public virtual DbSet<UserAvatar> UserAvatars { set; get; }
        public virtual DbSet<UserPerson> UserPersons { set; get; }
        public virtual DbSet<UserContact> UserContacts { set; get; }
        public virtual DbSet<UserNotifications> UserNotifications { get; set; }
        public virtual DbSet<UserModuleAuth> UserModuleAuths { get; set; }
        public virtual DbSet<UserActionAuth> UserActionAuths { get; set; }
        public virtual DbSet<RoleInfo> RoleInfos { get; set; }
        public virtual DbSet<RoleModuleAuth> RoleModuleAuths { get; set; }
        public virtual DbSet<RoleActionAuth> RoleActionAuths { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        #endregion

        #region 机构

        #endregion

        #region 商品
        public virtual DbSet<GoodsInfo> GoodsInfos { get; set; }
        public virtual DbSet<GoodsBrand> GoodsBrands { get; set; }
        public virtual DbSet<GoodsKind> GoodsKinds { get; set; }
        public virtual DbSet<GoodsSpec> GoodsSpecs { get; set; }
        public virtual DbSet<GoodsTag> GoodsTags { get; set; }
        public virtual DbSet<GoodsPhoto> GoodsPhotos { get; set; }
        public virtual DbSet<GoodsDesc> GoodsDescs { get; set; }
        public virtual DbSet<BrandInfo> BrandInfos { get; set; }
        public virtual DbSet<KindInfo> KindInfos { get; set; }
        public virtual DbSet<SpecInfo> SpecInfos { get; set; }
        public virtual DbSet<SpecValue> SpecValues { get; set; }
        public virtual DbSet<GoodsCounter> GoodsCounter { get; set; }

        #endregion

        #region 门店
        public virtual DbSet<StoreInfo> StoreInfos { get; set; }
        public virtual DbSet<StoreContact> StoreContacts { get; set; }
        public virtual DbSet<StorePhoto> StorePhotos { get; set; }
        public virtual DbSet<StoreDesc> StoreDescs { get; set; }
        #endregion

        #region 微信
        public virtual DbSet<WxUserInfo> WxUserInfos { get; set; }
        #endregion

        #region 订单
        public virtual DbSet<SalesOrder> SalesOrders { get; set; }
        public virtual DbSet<SalesOrderItem> SalesOrderItems { get; set; }
        public virtual DbSet<SOCustomerInfo> SOCustomerInfos { get; set; }
        public virtual DbSet<SOReceiveInfo> SOReceiveInfos { get; set; }
        public virtual DbSet<SOPayInfo> SOPayInfos { get; set; }
        #endregion

        #region 支付
        public virtual DbSet<F2FPay> F2FPays { get; set; }
        public virtual DbSet<F2FPayItem> F2FPayItems { get; set; }
        #endregion 支付
    }

}
