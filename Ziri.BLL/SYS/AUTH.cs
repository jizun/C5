using System.Linq;
using System.Collections.Generic;
using Ziri.MDL;
using Ziri.DAL;

namespace Ziri.BLL.SYS
{
    public class AUTH
    {
        //模块定义
        public static ModuleInfo ModuleInfo = new ModuleInfo { Name = "AUTH", Title = "授权", IconFont = "fa fa-user-shield", Enabled = true };
        public static List<ActionInfo> ActionInfos = new List<ActionInfo> {
            new ActionInfo{ Name = "Init", Title = "初始化", IconFont = "" , Enabled = true},
            new ActionInfo{ Name = "List", Title = "列表", IconFont = "" , Enabled = true},
            new ActionInfo{ Name = "Modify", Title = "修改", IconFont = "" , Enabled = true},
        };

        #region 用户模块授权
        //查询模块用户授权信息列表
        public static List<ModuleActionUserAuth> GetModuleUserAuthList(long UserID, out AlertMessage alertMessage)
        {
            alertMessage = null;

            //权限检查
            if (!PermissionCheck(ModuleInfo, ActionInfos.Where(i => i.Name == "Modify").FirstOrDefault(), out string Message))
            {
                alertMessage = new AlertMessage { Message = Message, Type = AlertType.warning };
                return null;
            }

            using (var EF = new EF())
            {
                return (from moduleInfo in EF.ModuleInfos
                        join userModuleAuth in (EF.UserModuleAuths.Where(i => i.UserID == UserID)) on moduleInfo.ID equals userModuleAuth.ModuleID into temp1
                        from userModuleAuth in temp1.DefaultIfEmpty()
                        where moduleInfo.Enabled == true
                        orderby moduleInfo.ID
                        select new ModuleActionUserAuth
                        {
                            ID = moduleInfo.ID,
                            IconFont = moduleInfo.IconFont,
                            Name = moduleInfo.Name,
                            Title = moduleInfo.Title,
                            Enabled = moduleInfo.Enabled,
                            UserID = userModuleAuth == null ? 0 : userModuleAuth.UserID,
                            UserAuth = userModuleAuth == null ? false : true
                        }).ToList();
            }
        }

        //查询模块操作用户授权信息列表
        public static List<ModuleActionUserAuth> GetModuleActionUserAuthList(long UserID, long ModuleID, out AlertMessage alertMessage)
        {
            alertMessage = null;

            //权限检查
            if (!PermissionCheck(ModuleInfo, ActionInfos.Where(i => i.Name == "Modify").FirstOrDefault(), out string Message))
            {
                alertMessage = new AlertMessage { Message = Message, Type = AlertType.warning };
                return null;
            }

            using (var EF = new EF())
            {
                return (from moduleAction in EF.ModuleActions
                        join actionInfo in EF.ActionInfos on moduleAction.ActionID equals actionInfo.ID
                        join userActionAuth in (EF.UserActionAuths.Where(i => i.UserID == UserID)) on actionInfo.ID equals userActionAuth.ActionID into temp1
                        from userActionAuth in temp1.DefaultIfEmpty()
                        where moduleAction.ModuleID == ModuleID && actionInfo.Enabled == true
                        orderby actionInfo.ID
                        select new ModuleActionUserAuth
                        {
                            ID = actionInfo.ID,
                            IconFont = actionInfo.IconFont,
                            Name = actionInfo.Name,
                            Title = actionInfo.Title,
                            Enabled = actionInfo.Enabled,
                            UserID = userActionAuth == null ? 0 : userActionAuth.UserID,
                            UserAuth = userActionAuth == null ? false : true
                        }).ToList();
            }
        }

        //更新模块用户授权信息
        public static List<UserModuleAuth> UserModuleAuthUpload(long[] UsersID, List<UserModuleAuth> userModuleAuths, out AlertMessage alertMessage)
        {
            alertMessage = null;

            //权限检查
            if (!PermissionCheck(ModuleInfo, ActionInfos.Where(i => i.Name == "Modify").FirstOrDefault(), out string Message))
            {
                alertMessage = new AlertMessage { Message = Message, Type = AlertType.warning };
                return null;
            }

            using (var EF = new EF())
            {
                EF.UserModuleAuths.RemoveRange(EF.UserModuleAuths.Where(i => UsersID.Contains(i.UserID)));
                EF.SaveChanges();
                EF.UserModuleAuths.AddRange(userModuleAuths);
                EF.SaveChanges();
            }
            return userModuleAuths;
        }

        //更新模块操作用户授权信息
        public static List<UserActionAuth> UserActionAuthUpload(long[] UsersID, List<UserActionAuth> userActionAuths, out AlertMessage alertMessage)
        {
            alertMessage = null;

            //权限检查
            if (!PermissionCheck(ModuleInfo, ActionInfos.Where(i => i.Name == "Modify").FirstOrDefault(), out string Message))
            {
                alertMessage = new AlertMessage { Message = Message, Type = AlertType.warning };
                return null;
            }

            using (var EF = new EF())
            {
                EF.UserActionAuths.RemoveRange(EF.UserActionAuths.Where(i => UsersID.Contains(i.UserID)));
                EF.SaveChanges();
                EF.UserActionAuths.AddRange(userActionAuths);
                EF.SaveChanges();
            }
            return userActionAuths;
        }
        #endregion

        #region 角色模块授权
        //查询模块角色授权信息列表
        public static List<ModuleActionRoleAuth> GetModuleRoleAuthList(long RoleID, out AlertMessage alertMessage)
        {
            alertMessage = null;

            //权限检查
            if (!PermissionCheck(ModuleInfo, ActionInfos.Where(i => i.Name == "Modify").FirstOrDefault(), out string Message))
            {
                alertMessage = new AlertMessage { Message = Message, Type = AlertType.warning };
                return null;
            }

            using (var EF = new EF())
            {
                return (from moduleInfo in EF.ModuleInfos
                        join roleModuleAuth in (EF.RoleModuleAuths.Where(i => i.RoleID == RoleID)) on moduleInfo.ID equals roleModuleAuth.ModuleID into temp1
                        from roleModuleAuth in temp1.DefaultIfEmpty()
                        where moduleInfo.Enabled == true
                        orderby moduleInfo.ID
                        select new ModuleActionRoleAuth
                        {
                            ID = moduleInfo.ID,
                            IconFont = moduleInfo.IconFont,
                            Name = moduleInfo.Name,
                            Title = moduleInfo.Title,
                            Enabled = moduleInfo.Enabled,
                            RoleID = roleModuleAuth == null ? 0 : roleModuleAuth.RoleID,
                            RoleAuth = roleModuleAuth == null ? false : true
                        }).ToList();
            }
        }

        //查询模块操作角色授权信息列表
        public static List<ModuleActionRoleAuth> GetModuleActionRoleAuthList(long RoleID, long ModuleID, out AlertMessage alertMessage)
        {
            alertMessage = null;

            //权限检查
            if (!PermissionCheck(ModuleInfo, ActionInfos.Where(i => i.Name == "Modify").FirstOrDefault(), out string Message))
            {
                alertMessage = new AlertMessage { Message = Message, Type = AlertType.warning };
                return null;
            }

            using (var EF = new EF())
            {
                return (from moduleAction in EF.ModuleActions
                        join actionInfo in EF.ActionInfos on moduleAction.ActionID equals actionInfo.ID
                        join roleActionAuth in (EF.RoleActionAuths.Where(i => i.RoleID == RoleID)) on actionInfo.ID equals roleActionAuth.ActionID into temp1
                        from roleActionAuth in temp1.DefaultIfEmpty()
                        where moduleAction.ModuleID == ModuleID && actionInfo.Enabled == true
                        orderby actionInfo.ID
                        select new ModuleActionRoleAuth
                        {
                            ID = actionInfo.ID,
                            IconFont = actionInfo.IconFont,
                            Name = actionInfo.Name,
                            Title = actionInfo.Title,
                            Enabled = actionInfo.Enabled,
                            RoleID = roleActionAuth == null ? 0 : roleActionAuth.RoleID,
                            RoleAuth = roleActionAuth == null ? false : true
                        }).ToList();
            }
        }

        //更新模块角色授权信息
        public static List<RoleModuleAuth> RoleModuleAuthUpload(long[] RolesID, List<RoleModuleAuth> roleModuleAuths, out AlertMessage alertMessage)
        {
            alertMessage = null;

            //权限检查
            if (!PermissionCheck(ModuleInfo, ActionInfos.Where(i => i.Name == "Modify").FirstOrDefault(), out string Message))
            {
                alertMessage = new AlertMessage { Message = Message, Type = AlertType.warning };
                return null;
            }

            using (var EF = new EF())
            {
                EF.RoleModuleAuths.RemoveRange(EF.RoleModuleAuths.Where(i => RolesID.Contains(i.RoleID)));
                EF.SaveChanges();
                EF.RoleModuleAuths.AddRange(roleModuleAuths);
                EF.SaveChanges();
            }
            return roleModuleAuths;
        }

        //更新模块操作角色授权信息
        public static List<RoleActionAuth> RoleActionAuthUpload(long[] RolesID, List<RoleActionAuth> roleActionAuths, out AlertMessage alertMessage)
        {
            alertMessage = null;

            //权限检查
            if (!PermissionCheck(ModuleInfo, ActionInfos.Where(i => i.Name == "Modify").FirstOrDefault(), out string Message))
            {
                alertMessage = new AlertMessage { Message = Message, Type = AlertType.warning };
                return null;
            }

            using (var EF = new EF())
            {
                EF.RoleActionAuths.RemoveRange(EF.RoleActionAuths.Where(i => RolesID.Contains(i.RoleID)));
                EF.SaveChanges();
                EF.RoleActionAuths.AddRange(roleActionAuths);
                EF.SaveChanges();
            }
            return roleActionAuths;
        }
        #endregion

        //检查用户权限
        internal static bool PermissionCheck(ModuleInfo ModuleInfo, ActionInfo ActionInfo, out string Message)
        {
            Message = null;

            //查询用户信息
            var loginInfo = Login.GetLoginInfo();
            if (loginInfo == null) { Message = "未登录。"; return false; }

            if (loginInfo.UserInfo.Name == "admin") { return true; }

            //查询模块信息
            var moduleInfo = Module.GetModuleInfo(ModuleInfo);
            var actionInfo = ActionInfo == null ? null : Module.GetModuleActionInfo(moduleInfo.ID, ActionInfo); ;
            using (var EF = new EF())
            {
                //查询角色模块操作授权信息
                var userRole = EF.UserRoles.Where(i => i.UserID == loginInfo.UserInfo.ID).ToList();
                foreach (var item in userRole)
                {
                    if (EF.RoleModuleAuths.Where(i => i.RoleID == item.RoleID
                        && i.ModuleID == moduleInfo.ID).FirstOrDefault() != null) { return true; }
                    if (actionInfo != null && EF.RoleActionAuths.Where(i => i.RoleID == item.RoleID
                        && i.ActionID == actionInfo.ID).FirstOrDefault() != null) { return true; }
                }

                //查询用户模块操作授权信息
                if (EF.UserModuleAuths.Where(i => i.UserID == loginInfo.UserInfo.ID
                    && i.ModuleID == moduleInfo.ID).FirstOrDefault() != null) { return true; }
                if (actionInfo != null && EF.UserActionAuths.Where(i => i.UserID == loginInfo.UserInfo.ID
                    && i.ActionID == actionInfo.ID).FirstOrDefault() != null) { return true; }
            }

            //完成
            Message = string.Format("您无权限执行[{0}]模块{1}。"
                , moduleInfo.Title, (actionInfo == null ? null : string.Format("的[{0}]操作", actionInfo.Title)));
            return false;
        }
    }
}
