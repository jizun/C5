using System;
using System.Linq;
using System.Collections.Generic;
using Ziri.MDL;
using Ziri.DAL;
using Ziri.BLL.LIST;

namespace Ziri.BLL.SYS
{
    public class Role
    {
        //模块定义
        public static ModuleInfo ModuleInfo = new ModuleInfo { Name = "Role", Title = "角色", IconFont = "fa fa-user-tie", Enabled = true };
        public static List<ActionInfo> ActionInfos = new List<ActionInfo> {
            new ActionInfo{ Name = "List", Title = "列表", IconFont = "", Enabled = true },
            new ActionInfo{ Name = "Details", Title = "详情", IconFont = "", Enabled = true },
            new ActionInfo{ Name = "Create", Title = "创建", IconFont = "", Enabled = true },
            new ActionInfo{ Name = "Modify", Title = "修改", IconFont = "", Enabled = true },
            new ActionInfo{ Name = "Enabled", Title = "启用", IconFont = "", Enabled = true },
            new ActionInfo{ Name = "Disabled", Title = "禁用", IconFont = "", Enabled = true },
            new ActionInfo{ Name = "ModuleAuthority", Title = "模块授权", IconFont = "", Enabled = true },
        };

        //初始化信息
        public static void InitRoleInfo(out AlertMessage alertMessage)
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
                var delCount = EF.Database.ExecuteSqlCommand(@"TRUNCATE TABLE RoleInfo;");
                var roleInfos = new List<RoleInfo> {
                    new RoleInfo{  Name = "finance", Title = "财务", IconFont = "", Enabled = true },
                    new RoleInfo{  Name = "business", Title = "商务", IconFont = "", Enabled = true },
                };
                EF.RoleInfos.AddRange(roleInfos);
                EF.SaveChanges();
            }
        }

        //获取列表
        public static List<RoleInfoList> GetRoleInfos(List<ListFilterField> FilterField, List<ListOrderField> OrderField
            , int PageSize, int PageIndex, out long TotalRowCount, out AlertMessage alertMessage)
        {
            TotalRowCount = 0;
            alertMessage = null;

            //权限检查
            if (!AUTH.PermissionCheck(ModuleInfo, ActionInfos.Where(i => i.Name == "List").FirstOrDefault(), out string Message))
            {
                alertMessage = new AlertMessage { Message = Message, Type = AlertType.warning };
                return null;
            }

            //查询列表
            using (var EF = new EF())
            {
                IEnumerable<RoleInfoList> DataList = from roleInfos in EF.RoleInfos
                                                   select new RoleInfoList
                                                   {
                                                       ID = roleInfos.ID,
                                                       Name = roleInfos.Name,
                                                       Title = roleInfos.Title,
                                                       Enabled = roleInfos.Enabled
                                                   };

                //筛选
                foreach (var item in FilterField)
                {
                    if (item.Name == "NameAndTitle" && item.Value.Count > 0)
                    {
                        var predicate = PredicateExtensions.False<RoleInfoList>();    //设置为False，所有and条件都应该放在or之后，如where (type=1 or type=14) and status==0
                        foreach (var t in item.Value)
                        {
                            var KWPart = t.ToLower();
                            switch (item.CmpareMode)
                            {
                                case FilterCmpareMode.Equal:
                                    predicate = predicate.Or(p => p.Name.ToLower() == KWPart || p.Title.ToLower() == KWPart);
                                    break;
                                case FilterCmpareMode.Like:
                                    predicate = predicate.Or(p => p.Name.ToLower().Contains(KWPart) || p.Title.ToLower().Contains(KWPart));
                                    break;
                            }
                        }
                        DataList = DataList.Where(predicate.Compile());
                    }
                }

                //排序
                if (OrderField.Count == 0) { DataList = DataList.OrderByDescending(i => i.ID); }
                else
                {
                    foreach (var item in OrderField)
                    {
                        switch (item.Mode)
                        {
                            case OrderByMode.Asc:
                                DataList = from list in DataList
                                           orderby OrderBy.GetPropertyValue(list, item.Name)
                                           select list;
                                break;
                            case OrderByMode.Desc:
                                DataList = from list in DataList
                                           orderby OrderBy.GetPropertyValue(list, item.Name) descending
                                           select list;
                                break;
                        }
                    }
                }

                //分页
                TotalRowCount = DataList.Count();
                if (TotalRowCount == 0) { return null; }
                int PageCount = (int)Math.Ceiling((double)TotalRowCount / PageSize);
                if (PageIndex > PageCount) { PageIndex = PageCount; }
                return DataList.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
            }
        }

        //获取详情
        public static RoleInfo GetDetailsInfo(long RoleID, out AlertMessage alertMessage)
        {
            alertMessage = null;

            //权限检查
            if (!AUTH.PermissionCheck(ModuleInfo, ActionInfos.Where(i => i.Name == "Details").FirstOrDefault(), out string Message))
            {
                alertMessage = new AlertMessage { Message = Message, Type = AlertType.warning };
                return null;
            }

            return GetRoleInfo(RoleID);
        }

        //获取修改
        public static RoleInfo GetModifyInfo(long RoleID, out AlertMessage alertMessage)
        {
            alertMessage = null;

            //权限检查
            if (!AUTH.PermissionCheck(ModuleInfo, ActionInfos.Where(i => i.Name == (RoleID == 0 ? "Create" : "Modify")).FirstOrDefault(), out string Message))
            {
                alertMessage = new AlertMessage { Message = Message, Type = AlertType.warning };
                return null;
            }

            return GetRoleInfo(RoleID);
        }

        //设置状态
        public static void SetRoleEnabled(long RoleID, bool Enabled, out AlertMessage alertMessage)
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
                var roleInfo = EF.RoleInfos.Where(i => i.ID == RoleID).FirstOrDefault();
                roleInfo.Enabled = Enabled;
                EF.SaveChanges();

                alertMessage = new AlertMessage { Message = "角色[" + roleInfo.Name + "]" + (Enabled ? "启用" : "禁用") + "成功！", Type = AlertType.success };
            }
        }

        //获取授权
        public static RoleInfo GetAuthInfo(long RoleID, out AlertMessage alertMessage)
        {
            alertMessage = null;

            //权限检查
            if (!AUTH.PermissionCheck(ModuleInfo, ActionInfos.Where(i => i.Name == "ModuleAuthority").FirstOrDefault(), out string Message))
            {
                alertMessage = new AlertMessage { Message = Message, Type = AlertType.warning };
                return null;
            }

            return GetRoleInfo(RoleID);
        }

        //更新角色信息
        public static RoleInfo RoleInfoUpload(RoleInfo roleInfo, out AlertMessage alertMessage)
        {
            //权限检查
            if (!AUTH.PermissionCheck(ModuleInfo, ActionInfos.Where(i => i.Name == (roleInfo.ID == 0 ? "Create" : "Modify")).FirstOrDefault(), out string Message))
            {
                alertMessage = new AlertMessage { Message = Message, Type = AlertType.warning };
                return null;
            }

            //表单检查
            if (string.IsNullOrWhiteSpace(roleInfo.Name))
            {
                alertMessage = new AlertMessage { Message = "角色代码不能为空。", Type = AlertType.warning };
                return null;
            }
            if (string.IsNullOrWhiteSpace(roleInfo.Title))
            {
                alertMessage = new AlertMessage { Message = "角色名称不能为空。", Type = AlertType.warning };
                return null;
            }

            using (var EF = new EF())
            {
                //修改是否存在？用户名唯一？
                RoleInfo role_exist = null;
                RoleInfo role_name_exist = null;
                if (roleInfo.ID == 0)
                {
                    role_name_exist = EF.RoleInfos.Where(i => i.Name == roleInfo.Name).FirstOrDefault();
                }
                else
                {
                    role_exist = EF.RoleInfos.Where(i => i.ID == roleInfo.ID).FirstOrDefault();
                    if (role_exist == null)
                    {
                        alertMessage = new AlertMessage { Message = string.Format("角色编号[{0}]不存在。", roleInfo.ID), Type = AlertType.warning };
                        return null;
                    }
                    role_name_exist = EF.RoleInfos.Where(i => i.ID != roleInfo.ID && i.Name == roleInfo.Name).FirstOrDefault();
                }
                if (role_name_exist != null && role_name_exist.ID > 0)
                {
                    alertMessage = new AlertMessage { Message = string.Format("角色名称[{0}]已被ID[{1}]使用。", roleInfo.Name, role_name_exist.ID), Type = AlertType.warning };
                    return null;
                }

                //数据保存
                if (roleInfo.ID == 0) { role_exist = EF.RoleInfos.Add(new RoleInfo { Enabled = true, }); }
                role_exist.Name = roleInfo.Name;
                role_exist.Title = roleInfo.Title;
                role_exist.IconFont = roleInfo.IconFont;
                EF.SaveChanges();

                //更新完成
                alertMessage = null;
                return role_exist;
            }
        }

        //获取角色信息
        private static RoleInfo GetRoleInfo(long RoleID)
        {
            using (var EF = new EF())
            {
                return EF.RoleInfos.Where(i => i.ID == RoleID).FirstOrDefault();
            }
        }

        //获取角色的模块授权列表
        public static List<ModuleActionRoleAuth> GetRoleModuleAuths(long RoleID)
        {
            using (var EF = new EF())
            {
                return (from moduleInfo in EF.ModuleInfos
                        join roleModuleAuth in (EF.RoleModuleAuths.Where(i => i.RoleID == RoleID)) on moduleInfo.ID equals roleModuleAuth.ModuleID into temp1
                        from roleModuleAuth in temp1.DefaultIfEmpty()
                        select new ModuleActionRoleAuth
                        {
                            ID = moduleInfo.ID,
                            Name = moduleInfo.Name,
                            Title = moduleInfo.Title,
                            IconFont = moduleInfo.IconFont,
                            Enabled = moduleInfo.Enabled,
                            RoleID = roleModuleAuth == null ? 0 : roleModuleAuth.RoleID,
                            RoleAuth = roleModuleAuth == null ? false : true
                        }).ToList();
            }
        }

        //获取角色的模块操作授权列表
        public static List<ModuleActionRoleAuth> GetRoleModuleActionAuths(long ModuleID, long RoleID)
        {
            using (var EF = new EF())
            {
                return (from moduleInfo in EF.ModuleInfos
                        join moduleAction in EF.ModuleActions on moduleInfo.ID equals moduleAction.ModuleID
                        join actionInfo in EF.ActionInfos on moduleAction.ActionID equals actionInfo.ID
                        join roleActionAuth in (EF.RoleActionAuths.Where(i => i.RoleID == RoleID)) on actionInfo.ID equals roleActionAuth.ActionID into temp1
                        from roleActionAuth in temp1.DefaultIfEmpty()
                        where moduleInfo.ID == ModuleID
                        select new ModuleActionRoleAuth
                        {
                            ID = actionInfo.ID,
                            Name = actionInfo.Name,
                            Title = actionInfo.Title,
                            IconFont = actionInfo.IconFont,
                            Enabled = actionInfo.Enabled,
                            RoleID = roleActionAuth == null ? 0 : roleActionAuth.RoleID,
                            RoleAuth = roleActionAuth == null ? false : true
                        }).ToList();
            }
        }

        //获取角色的用户信息列表
        public static List<UserInfo> GetRoleUsers(long RoleID)
        {
            using (var EF = new EF())
            {
                return (from userRoles in EF.UserRoles
                        join userInfo in EF.UserInfos on userRoles.UserID equals userInfo.ID
                        where userRoles.RoleID == RoleID
                        select userInfo).ToList();
            }
        }
    }
}
