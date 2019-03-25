using System;
using System.Linq;
using System.Collections.Generic;
using Ziri.MDL;
using Ziri.DAL;

namespace Ziri.BLL.SYS
{
    public class Menu
    {
        //模块定义
        public static ModuleInfo ModuleInfo = new ModuleInfo { Name = "Menu", Title = "菜单", IconFont = "fa fa-list", Enabled = true };
        public static List<ActionInfo> ActionInfos = new List<ActionInfo> {
            new ActionInfo{ Name = "Init", Title = "初始化", IconFont = "" , Enabled = true},
            new ActionInfo{ Name = "List", Title = "列表", IconFont = "" , Enabled = true},
            new ActionInfo{ Name = "Modify", Title = "修改", IconFont = "" , Enabled = true},
            new ActionInfo{ Name = "Enabled", Title = "启用", IconFont = "" , Enabled = true},
            new ActionInfo{ Name = "Disabled", Title = "禁用", IconFont = "" , Enabled = true},
        };

        //初始化菜单信息
        public static void InitMenuInfo(out AlertMessage alertMessage)
        {
            alertMessage = null;

            //权限检查
            if (!AUTH.PermissionCheck(ModuleInfo, ActionInfos.Where(i => i.Name == "Init").FirstOrDefault(), out string Message))
            {
                alertMessage = new AlertMessage { Message = Message, Type = AlertType.warning };
                return;
            }

            using (var EF = new EF())
            {
                //清除数据表
                var delCount = EF.Database.ExecuteSqlCommand("TRUNCATE TABLE MenuGroup; TRUNCATE TABLE MenuInfo;");

                //左边菜单
                var asideMenuRoots = new List<MenuInfo> {
                    new MenuInfo{ ParentID = 0, OrderBy = 1, Name = "BasicInfo", Title = "基础资料", IconFont = "fa fa-database", URL = null, Enabled = true, },
                    new MenuInfo{ ParentID = 0, OrderBy = 1, Name = "INV", Title = "存货管理", IconFont = "fa fa-warehouse", URL = null, Enabled = true, },
                    new MenuInfo{ ParentID = 0, OrderBy = 1, Name = "RMS", Title = "零售管理", IconFont = "flaticon2-supermarket", URL = null, Enabled = true, },
                    new MenuInfo{ ParentID = 0, OrderBy = 1, Name = "OMS", Title = "订单管理", IconFont = "fa fa-money-check-alt", URL = null, Enabled = true, },
                    new MenuInfo{ ParentID = 0, OrderBy = 1, Name = "CMS", Title = "客户管理", IconFont = "fa fa-user-friends", URL = null, Enabled = true, },
                };
                EF.MenuInfos.AddRange(asideMenuRoots);
                EF.SaveChanges();
                //根目录
                List<MenuInfo> asideMenuL1 = null;
                List<MenuInfo> asideMenuL2 = null;
                foreach (var rootItem in asideMenuRoots)
                {
                    //菜单组
                    EF.MenuGroups.Add(new MenuGroup { GroupID = (int)MenuGroupType.AsideMenu, MenuID = rootItem.ID });
                    EF.SaveChanges();
                    //一级菜单
                    switch (rootItem.Name)
                    {
                        case "BasicInfo":
                            asideMenuL1 = new List<MenuInfo> {
                                new MenuInfo{ ParentID = rootItem.ID, OrderBy = 1, Name = "UserManage", Title = "用户管理", IconFont = "fa fa-users", URL = null, Enabled = true, },
                                new MenuInfo{ ParentID = rootItem.ID, OrderBy = 2, Name = "GoodsManage", Title = "商品管理", IconFont = "fa fa-th", URL = null, Enabled = true, },
                            };
                            EF.MenuInfos.AddRange(asideMenuL1);
                            EF.SaveChanges();
                            foreach (var l1Item in asideMenuL1)
                            {
                                //二级菜单
                                switch (l1Item.Name)
                                {
                                    case "UserManage":
                                        asideMenuL2 = new List<MenuInfo> {
                                            new MenuInfo{ ParentID = l1Item.ID, OrderBy = 1, Name = "UserInfo", Title = "用户信息", IconFont = "fa fa-user", URL = "/MOD/SYS/UserInfo.aspx", Enabled = true, },
                                            new MenuInfo{ ParentID = l1Item.ID, OrderBy = 2, Name = "UoleInfo", Title = "角色信息", IconFont = "fa fa-user-tie", URL = "/MOD/SYS/RoleInfo.aspx", Enabled = true, },
                                        };
                                        EF.MenuInfos.AddRange(asideMenuL2);
                                        EF.SaveChanges();
                                        break;
                                    case "GoodsManage":
                                        asideMenuL2 = new List<MenuInfo> {
                                            new MenuInfo{ ParentID = l1Item.ID, OrderBy = 1, Name = "GoodsInfo", Title = "商品信息", IconFont = "fa fa-th-large", URL = "/MOD/ITEM/GoodsInfo.aspx", Enabled = true, },
                                            new MenuInfo{ ParentID = l1Item.ID, OrderBy = 2, Name = "BrandInfo", Title = "品牌信息", IconFont = "flaticon2-gift", URL = "/MOD/ITEM/BrandInfo.aspx", Enabled = true, },
                                            new MenuInfo{ ParentID = l1Item.ID, OrderBy = 3, Name = "KindInfo", Title = "品类信息", IconFont = "flaticon2-gift", URL = "/MOD/ITEM/KindInfo.aspx", Enabled = true, },
                                            new MenuInfo{ ParentID = l1Item.ID, OrderBy = 4, Name = "SpecInfo", Title = "规格信息", IconFont = "flaticon2-gift", URL = "/MOD/ITEM/SpecInfo.aspx", Enabled = true, },
                                        };
                                        EF.MenuInfos.AddRange(asideMenuL2);
                                        EF.SaveChanges();
                                        break;
                                }
                            }
                            break;
                        case "RMS":
                            asideMenuL1 = new List<MenuInfo> {
                                new MenuInfo{ ParentID = rootItem.ID, OrderBy = 1, Name = "StoreManage", Title = "门店管理", IconFont = "fa fa-store-alt", URL = null, Enabled = true, },
                                new MenuInfo{ ParentID = rootItem.ID, OrderBy = 2, Name = "CashManage", Title = "收银管理", IconFont = "fa fa-solar-panel", URL = null, Enabled = true, },
                            };
                            EF.MenuInfos.AddRange(asideMenuL1);
                            EF.SaveChanges();
                            foreach (var l1Item in asideMenuL1)
                            {
                                //二级菜单
                                switch (l1Item.Name)
                                {
                                    case "StoreManage":
                                        asideMenuL2 = new List<MenuInfo> {
                                            new MenuInfo{ ParentID = l1Item.ID, OrderBy = 1, Name = "StoreInfo", Title = "门店信息", IconFont = "fa fa-store", URL = "/MOD/RMS/StoreInfo.aspx", Enabled = true, },
                                            new MenuInfo{ ParentID = l1Item.ID, OrderBy = 2, Name = "SalesInfo", Title = "店员信息", IconFont = "fa fa-user", URL = "/MOD/RMS/SalesInfo.aspx", Enabled = true, },
                                        };
                                        EF.MenuInfos.AddRange(asideMenuL2);
                                        EF.SaveChanges();
                                        break;
                                    case "CashManage":
                                        asideMenuL2 = new List<MenuInfo> {
                                            new MenuInfo{ ParentID = l1Item.ID, OrderBy = 1, Name = "Receipt", Title = "收银单", IconFont = "fa fa-receipt", URL = "/MOD/RMS/Receipt.aspx", Enabled = true, },
                                        };
                                        EF.MenuInfos.AddRange(asideMenuL2);
                                        EF.SaveChanges();
                                        break;
                                }
                            }
                            break;
                        case "OMS":
                            asideMenuL1 = new List<MenuInfo> {
                                new MenuInfo{ ParentID = rootItem.ID, OrderBy = 1, Name = "SOManage", Title = "订单管理", IconFont = "fa fa-money-check-alt", URL = null, Enabled = true, },
                            };
                            EF.MenuInfos.AddRange(asideMenuL1);
                            EF.SaveChanges();
                            foreach (var l1Item in asideMenuL1)
                            {
                                //二级菜单
                                switch (l1Item.Name)
                                {
                                    case "SOManage":
                                        asideMenuL2 = new List<MenuInfo> {
                                            new MenuInfo{ ParentID = l1Item.ID, OrderBy = 1, Name = "SalesOrder", Title = "销售订单", IconFont = "fa fa-money-check", URL = "/MOD/OMS/SalesOrder.aspx", Enabled = true, },
                                        };
                                        EF.MenuInfos.AddRange(asideMenuL2);
                                        EF.SaveChanges();
                                        break;
                                }
                            }
                            break;
                        case "CMS":
                            asideMenuL1 = new List<MenuInfo> {
                                new MenuInfo{ ParentID = rootItem.ID, OrderBy = 1, Name = "CustomerInfo", Title = "客户信息", IconFont = "fa fa-user-friends", URL = null, Enabled = true, },
                            };
                            EF.MenuInfos.AddRange(asideMenuL1);
                            EF.SaveChanges();
                            foreach (var l1Item in asideMenuL1)
                            {
                                //二级菜单
                                switch (l1Item.Name)
                                {
                                    case "CustomerInfo":
                                        asideMenuL2 = new List<MenuInfo> {
                                            new MenuInfo{ ParentID = l1Item.ID, OrderBy = 1, Name = "WeChatUser", Title = "微信用户", IconFont = "socicon-wechat", URL = "/MOD/CMS/WeChatUser.aspx", Enabled = true, },
                                        };
                                        EF.MenuInfos.AddRange(asideMenuL2);
                                        EF.SaveChanges();
                                        break;
                                }
                            }
                            break;
                    }
                }
            }
        }

        //获取菜单组标题
        private static string GetMenuGroupTitle(int MenuGroupTypeID)
        {
            switch (MenuGroupTypeID)
            {
                case 1: return "左边菜单";
                case 2: return "上边菜单";
                default: return "菜单组[" + MenuGroupTypeID + "]标题未定义";
            }
        }

        //获取修改信息
        public static MenuInfo GetModifyInfo(long MenuID, out AlertMessage alertMessage)
        {
            alertMessage = null;

            //权限检查
            if (!AUTH.PermissionCheck(ModuleInfo, ActionInfos.Where(i => i.Name == "Modify").FirstOrDefault(), out string Message))
            {
                alertMessage = new AlertMessage { Message = Message, Type = AlertType.warning };
                return null;
            }
            return GetMenuInfo(MenuID);
        }

        //获取菜单信息
        private static MenuInfo GetMenuInfo(long MenuID)
        {
            using (var EF = new EF())
            {
                return EF.MenuInfos.Where(i => i.ID == MenuID).FirstOrDefault();
            }
        }

        //更新菜单信息
        public static MenuInfo MenuInfoUpload(MenuInfo menuInfo, out AlertMessage alertMessage)
        {
            alertMessage = null;

            //权限检查
            if (!AUTH.PermissionCheck(ModuleInfo, ActionInfos.Where(i => i.Name == "Modify").FirstOrDefault(), out string Message))
            {
                alertMessage = new AlertMessage { Message = Message, Type = AlertType.warning };
                return null;
            }

            using (var EF = new EF())
            {
                var menu_exist = EF.MenuInfos.Where(i => i.ID == menuInfo.ID).FirstOrDefault();
                if (menu_exist == null) { return null; }
                menu_exist.Name = menuInfo.Name;
                menu_exist.Title = menuInfo.Title;
                menu_exist.URL = menuInfo.URL;
                menu_exist.OrderBy = menuInfo.OrderBy;
                menu_exist.IconFont = menuInfo.IconFont;
                EF.SaveChanges();
                return menu_exist;
            }
        }

        //获取菜单组列表
        public static List<MenuGroupInfo> GetMenuGroupInfos(out AlertMessage alertMessage)
        {
            alertMessage = null;

            //权限检查
            if (!AUTH.PermissionCheck(ModuleInfo, ActionInfos.Where(i => i.Name == "List").FirstOrDefault(), out string Message))
            {
                alertMessage = new AlertMessage { Message = Message, Type = AlertType.warning };
                return null;
            }

            var menuGroupList = new List<MenuGroupInfo>();
            string[] EnumText = Enum.GetNames(typeof(MenuGroupType));
            int[] EnumValue = (int[])Enum.GetValues(typeof(MenuGroupType));
            for (int i = 0; i < EnumText.Length; i++)
            {
                menuGroupList.Add(new MenuGroupInfo
                {
                    GroupID = EnumValue[i],
                    GroupName = EnumText[i],
                    GroupTitle = GetMenuGroupTitle(EnumValue[i])
                });
            }
            return menuGroupList;
        }

        //获取菜单组菜单列表
        public static List<MenuInfo> GetGroupMenuInfos(int GroupID, bool Enabled = false)
        {
            using (var EF = new EF())
            {
                var list = from menuGroup in EF.MenuGroups.Where(i => i.GroupID == GroupID)
                           join menuInfos in EF.MenuInfos on menuGroup.MenuID equals menuInfos.ID
                           orderby menuInfos.OrderBy
                           select menuInfos;
                if (Enabled) { list = list.Where(i => i.Enabled == true); }
                return list.ToList();
            }
        }

        //获取菜单列表
        public static List<MenuInfo> GetMenuInfos(long MenuID = 0, bool Enabled = false)
        {
            using (var EF = new EF())
            {
                if (Enabled)
                {
                    return EF.MenuInfos.Where(i => i.ParentID == MenuID).OrderBy(i => i.OrderBy).ToList();
                }
                else
                {
                    return EF.MenuInfos.Where(i => i.Enabled == true && i.ParentID == MenuID)
                        .OrderBy(i => i.OrderBy).ToList();
                }
            }
        }

        //获取菜单路径
        public static List<MenuInfo> GetMenuInfos(List<MenuInfo> menuInfos, long MenuID)
        {
            using (var EF = new EF())
            {
                var menuInfo = EF.MenuInfos.Where(i => i.ID == MenuID).FirstOrDefault();
                if (menuInfo != null)
                {
                    menuInfos.Add(menuInfo);
                    if (menuInfo.ParentID > 0 && menuInfo.ParentID != MenuID && menuInfos.Count < 50)
                    {
                        menuInfos = GetMenuInfos(menuInfos, menuInfo.ParentID);
                    }
                }
            }
            return menuInfos;
        }

        //设置状态
        public static void SetMenuEnabled(long MenuID, bool Enabled, out AlertMessage alertMessage)
        {
            alertMessage = null;

            //权限检查
            if (!AUTH.PermissionCheck(ModuleInfo, ActionInfos.Where(i => i.Name == (Enabled ? "Enabled" : "Disabled")).FirstOrDefault(), out string Message))
            {
                alertMessage = new AlertMessage { Message = Message, Type = AlertType.warning };
                return;
            }

            using (var EF = new EF())
            {
                var menuInfo = EF.MenuInfos.Where(i => i.ID == MenuID).FirstOrDefault();
                menuInfo.Enabled = Enabled;
                EF.SaveChanges();

                alertMessage = new AlertMessage { Message = "菜单[" + menuInfo.Name + "]" + (Enabled ? "启用" : "禁用") + "成功！", Type = AlertType.success };
            }
        }
    }
}
