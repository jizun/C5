--truncate table WxUserInfo
--模块
select * from ActionInfo
select * from ModuleInfo
select * from ModuleAction
--菜单
select * from MenuInfo
select * from MenuGroup
--文件
select * from FileInfo
select * from FileExtName
select * from FileMIME
--基础资料
select * from Slogan		--欢迎语
select * from UploadInfo	--上传信息
select * from PersonInfo	--个人信息
select * from ContactInfo	--联系信息
select * from DescInfo		--描述信息
--用户
select * from UserInfo
select * from UserInfo_log
select * from UserAvatar
select * from UserPerson
select * from UserContact
select * from UserModuleAuth
select * from UserActionAuth
select * from UserRole
--角色
select * from RoleInfo
select * from RoleModuleAuth
select * from RoleActionAuth
--商品
select * from BrandInfo
select * from KindInfo
select * from SpecInfo
select * from SpecValue
select * from GoodsInfo
select * from GoodsBrand
select * from GoodsKind
select * from GoodsTag
select * from GoodsPhoto
select * from GoodsDesc
select * from GoodsSpec
select * from GoodsCounter	--价格，specid=0为统一价
--门店
select * from StoreInfo
select * from StoreContact
select * from StorePhoto
select * from StoreDesc
--微信
select * from WxUserInfo
--订单
select * from SalesOrder
select * from SalesOrderItem
select * from SOCustomerInfo
select * from SOReceiveInfo
select * from SOPayInfo
select * from F2FPay
select * from F2FPayItem



--随机创建记录
declare @i int
set @i=1
while @i<200
begin
	insert into UserInfo(GUID,Name,Password,StateID,UpdateTime) Values(newid(),newid(),newid(),5,getdate())
	set @i=@i+1
end
select * from UserInfo

--删除所有表
use C5
GO
declare @sql varchar(8000)
while (select count(*) from sysobjects where type='U')>0
begin SELECT @sql='drop table ' + name FROM sysobjects
WHERE (type = 'U')
ORDER BY 'drop table ' + name
exec(@sql)
end