using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Transactions;
using System.Security.Cryptography;
using Ziri.MDL;
using Ziri.DAL;
using Ziri.BLL.LIST;

namespace Ziri.BLL.SYS
{
    public class User
    {
        //模块定义
        public static ModuleInfo ModuleInfo = new ModuleInfo { Name = "User", Title = "用户", IconFont = "fa fa-users", Enabled = true };
        public static List<ActionInfo> ActionInfos = new List<ActionInfo> {
            new ActionInfo{ Name = "List", Title = "列表", IconFont = "", Enabled = true },
            new ActionInfo{ Name = "Details", Title = "详情", IconFont = "", Enabled = true },
            new ActionInfo{ Name = "Create", Title = "创建", IconFont = "", Enabled = true },
            new ActionInfo{ Name = "Modify", Title = "修改", IconFont = "", Enabled = true },
            new ActionInfo{ Name = "Checked", Title = "审核", IconFont = "", Enabled = true },
            new ActionInfo{ Name = "Enabled", Title = "启用", IconFont = "", Enabled = true },
            new ActionInfo{ Name = "Disabled", Title = "禁用", IconFont = "", Enabled = true },
            new ActionInfo{ Name = "ModuleAuthority", Title = "模块授权", IconFont = "", Enabled = true },
            new ActionInfo{ Name = "RoleAssign", Title = "角色指定", IconFont = "", Enabled = true },
        };

        //登录用户查询
        public static UserInfo GetUserInfo(string UserName)
        {
            using (var EF = new EF())
            {
                return EF.UserInfos.Where(i => i.Name == UserName).FirstOrDefault();
            }
        }

        //获取用户列表
        public static List<UserInfoList> GetUserInfos(List<ListFilterField> FilterField, List<ListOrderField> OrderField
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
                IEnumerable<UserInfoList> DataList = from userInfos in EF.UserInfos
                               select new UserInfoList
                               {
                                   ID = userInfos.ID,
                                   Name = userInfos.Name,
                                   StateID = userInfos.StateID,
                                   UpdateTime = userInfos.UpdateTime
                               };

                //筛选
                foreach (var item in FilterField)
                {
                    if (item.Name == "Name" && item.Value.Count > 0)
                    {
                        var predicate = PredicateExtensions.False<UserInfoList>();    //设置为False，所有and条件都应该放在or之后，如where (type=1 or type=14) and status==0
                        foreach (var t in item.Value)
                        {
                            var KWPart = t.ToLower();
                            switch (item.CmpareMode)
                            {
                                case FilterCmpareMode.Equal:
                                    predicate = predicate.Or(p => p.Name.ToLower() == KWPart);
                                    break;
                                case FilterCmpareMode.Like:
                                    predicate = predicate.Or(p => p.Name.ToLower().Contains(KWPart));
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

        //获取用户修改信息
        public static UserInfo GetModifyInfo(long UserID, out AlertMessage alertMessage)
        {
            alertMessage = null;

            if (Login.GetLoginInfo() == null)
            {
                //登录检查
                alertMessage = new AlertMessage { Message = "未登录不允许更新用户信息", Type = AlertType.warning };
                return null;
            }
            else if (UserID == 0)
            {
                //权限检查
                if (!AUTH.PermissionCheck(ModuleInfo, ActionInfos.Where(i => i.Name == "Create").FirstOrDefault(), out string Message))
                {
                    alertMessage = new AlertMessage { Message = Message, Type = AlertType.warning };
                    return null;
                }
            }
            else if (UserID != Login.GetLoginInfo().UserInfo.ID)
            {
                //权限检查
                if (!AUTH.PermissionCheck(ModuleInfo, ActionInfos.Where(i => i.Name == "Modify").FirstOrDefault(), out string Message))
                {
                    alertMessage = new AlertMessage { Message = Message, Type = AlertType.warning };
                    return null;
                }
            }

            return GetUserInfo(UserID);
        }

        //更新用户信息
        public static UserInfo UserInfoUpload(UserInfo userInfo, List<FileUploadInfo> fileUploadInfo
            , PersonInfo personInfo, ContactInfo contactInfo, bool isRegisters, out AlertMessage alertMessage)
        {
            if (userInfo.ID == 0 && isRegisters) { userInfo.StateID = (int)UserState.提交; }
            else if (Login.GetLoginInfo() == null)
            {
                //登录检查
                alertMessage = new AlertMessage { Message = "未登录不允许更新用户信息", Type = AlertType.warning };
                return null;
            }
            else if (userInfo.ID != Login.GetLoginInfo().UserInfo.ID)
            {
                //权限检查
                if (!AUTH.PermissionCheck(ModuleInfo, ActionInfos.Where(i => i.Name == (userInfo.ID == 0 ? "Create" : "Modify")).FirstOrDefault(), out string Message))
                {
                    alertMessage = new AlertMessage { Message = Message, Type = AlertType.warning };
                    return null;
                }
            }

            //表单检查
            if (string.IsNullOrWhiteSpace(userInfo.Name))
            {
                alertMessage = new AlertMessage { Message = "用户名不能为空。", Type = AlertType.warning };
                return null;
            }
            if (string.IsNullOrWhiteSpace(userInfo.Password))
            {
                alertMessage = new AlertMessage { Message = "密码不能为空。", Type = AlertType.warning };
                return null;
            }

            using (var EF = new EF())
            {
                //检查用户初始化，可略去
                UserInfo user_admin = EF.UserInfos.Where(i => i.Name == "admin").FirstOrDefault();
                if (user_admin == null) { InitUserInfo(); }

                //修改是否存在？用户名唯一？
                UserInfo user_exist = null;
                UserInfo user_name_exist = null;
                if (userInfo.ID == 0)
                {
                    user_name_exist = EF.UserInfos.Where(i => i.StateID != (int)UserState.删除 && i.Name == userInfo.Name).FirstOrDefault();
                }
                else
                {
                    user_exist = EF.UserInfos.Where(i => i.StateID != (int)UserState.删除 && i.ID == userInfo.ID).FirstOrDefault();
                    if (user_exist == null)
                    {
                        alertMessage = new AlertMessage { Message = string.Format("用户编号[{0}]不存在。", userInfo.ID), Type = AlertType.warning };
                        return null;
                    }
                    user_name_exist = EF.UserInfos.Where(i => i.ID != userInfo.ID && i.StateID != (int)UserState.删除
                        && i.Name == userInfo.Name).FirstOrDefault();
                }
                if (user_name_exist != null && user_name_exist.ID > 0)
                {
                    alertMessage = new AlertMessage { Message = string.Format("用户名[{0}]已被ID[{1}]使用。", userInfo.Name, user_name_exist.ID), Type = AlertType.warning };
                    return null;
                }

                //数据保存
                using (TransactionScope TS = new TransactionScope())
                {
                    //日志信息
                    if (user_exist != null)
                    {
                        UserInfo_log userInfo_Log = new UserInfo_log
                        {
                            UserID = user_exist.ID,
                            GUID = user_exist.GUID,
                            Name = user_exist.Name,
                            Password = user_exist.Password,
                            StateID = user_exist.StateID,
                            UpdateTime = user_exist.UpdateTime
                        };
                        EF.UserInfo_logs.Add(userInfo_Log);
                        EF.SaveChanges();
                        var avatar_log = EF.UserAvatars.Where(i => i.UserID == user_exist.ID && i.LogID == null);
                        foreach (var item in avatar_log) { item.LogID = userInfo_Log.ID; }
                        var person_log = EF.UserPersons.Where(i => i.UserID == user_exist.ID && i.LogID == null);
                        foreach (var item in person_log) { item.LogID = userInfo_Log.ID; }
                        var contact_log = EF.UserContacts.Where(i => i.UserID == user_exist.ID && i.LogID == null);
                        foreach (var item in contact_log) { item.LogID = userInfo_Log.ID; }
                        EF.SaveChanges();
                    }

                    //用户信息
                    if (userInfo.ID == 0)
                    {
                        user_exist = EF.UserInfos.Add(new UserInfo
                        {
                            GUID = Guid.NewGuid().ToString(),
                            StateID = (int)UserState.创建,
                        });
                    }
                    user_exist.Name = userInfo.Name;
                    user_exist.Password = userInfo.Password;
                    user_exist.UpdateTime = DateTime.Now;
                    EF.SaveChanges();
                    //头像信息
                    if (fileUploadInfo != null && fileUploadInfo.Count > 0)
                    {
                        long fileID = fileUploadInfo[0].FileInfo.ID;
                        var userAvatar_exist = EF.UserAvatars.Where(i => i.UserID == user_exist.ID && i.FileID == fileID && i.LogID == null).FirstOrDefault();
                        if (userAvatar_exist == null)
                        {
                            EF.UserAvatars.Add(userAvatar_exist = new UserAvatar
                            {
                                UserID = user_exist.ID,
                                FileID = fileID,
                            });
                            EF.SaveChanges();
                        }
                    }
                    //个人信息
                    var person_exist = EF.PersonInfos.Where(i => i.Name == personInfo.Name
                        && i.GenderTypeID == personInfo.GenderTypeID
                        && i.Birthday == personInfo.Birthday
                        && i.IDCardNO == personInfo.IDCardNO).FirstOrDefault();
                    if (person_exist == null)
                    {
                        EF.PersonInfos.Add(person_exist = new PersonInfo
                        {
                            Name = personInfo.Name,
                            GenderTypeID = personInfo.GenderTypeID,
                            Birthday = personInfo.Birthday,
                            IDCardNO = personInfo.IDCardNO,
                        });
                        EF.SaveChanges();
                    }
                    var userPerson_exist = EF.UserPersons.Where(i => i.UserID == user_exist.ID
                        && i.PersonID == person_exist.ID && i.LogID == null).FirstOrDefault();
                    if (userPerson_exist == null)
                    {
                        EF.UserPersons.Add(userPerson_exist = new UserPerson
                        {
                            UserID = user_exist.ID,
                            PersonID = person_exist.ID
                        });
                        EF.SaveChanges();
                    }
                    //联系信息
                    var contact_exist = EF.ContactInfos.Where(i => i.Phone == contactInfo.Phone
                        && i.Mobile == contactInfo.Mobile
                        && i.EMail == contactInfo.EMail
                        && i.Address == contactInfo.Address).FirstOrDefault();
                    if (contact_exist == null)
                    {
                        EF.ContactInfos.Add(contact_exist = new ContactInfo
                        {
                            Phone = contactInfo.Phone,
                            Mobile = contactInfo.Mobile,
                            EMail = contactInfo.EMail,
                            Address = contactInfo.Address,
                        });
                        EF.SaveChanges();
                    }
                    var userContact = EF.UserContacts.Where(i => i.UserID == user_exist.ID
                        && i.ContactID == contact_exist.ID && i.LogID == null).FirstOrDefault();
                    if (userContact == null)
                    {
                        EF.UserContacts.Add(userContact = new UserContact
                        {
                            UserID = user_exist.ID,
                            ContactID = contact_exist.ID
                        });
                        EF.SaveChanges();
                    }

                    //更新完成
                    TS.Complete();
                    alertMessage = null;
                    return user_exist;
                }
            }
        }

        //获取用户详情信息
        public static UserInfo GetDetailsInfo(long UserID, out AlertMessage alertMessage)
        {
            alertMessage = null;

            if (Login.GetLoginInfo() == null)
            {
                //登录检查
                alertMessage = new AlertMessage { Message = "未登录不允许查看用户信息", Type = AlertType.warning };
                return null;
            }
            else if (UserID != Login.GetLoginInfo().UserInfo.ID)
            {
                //权限检查
                if (!AUTH.PermissionCheck(ModuleInfo, ActionInfos.Where(i => i.Name == "Details").FirstOrDefault(), out string Message))
                {
                    alertMessage = new AlertMessage { Message = Message, Type = AlertType.warning };
                    return null;
                }
            }

            return GetUserInfo(UserID);
        }

        //获取用户授权信息
        public static UserInfo GetAuthInfo(long UserID, out AlertMessage alertMessage)
        {
            alertMessage = null;

            if (Login.GetLoginInfo() == null)
            {
                //登录检查
                alertMessage = new AlertMessage { Message = "未登录不允许进行用户授权", Type = AlertType.warning };
                return null;
            }
            //权限检查
            if (!AUTH.PermissionCheck(ModuleInfo, ActionInfos.Where(i => i.Name == "ModuleAuthority").FirstOrDefault(), out string Message))
            {
                alertMessage = new AlertMessage { Message = Message, Type = AlertType.warning };
                return null;
            }

            return GetUserInfo(UserID);
        }

        //获取用户角色信息
        public static UserInfo GetRoleInfo(long UserID, out AlertMessage alertMessage)
        {
            alertMessage = null;

            if (Login.GetLoginInfo() == null)
            {
                //登录检查
                alertMessage = new AlertMessage { Message = "未登录不允许进行角色指定", Type = AlertType.warning };
                return null;
            }
            //权限检查
            if (!AUTH.PermissionCheck(ModuleInfo, ActionInfos.Where(i => i.Name == "RoleAssign").FirstOrDefault(), out string Message))
            {
                alertMessage = new AlertMessage { Message = Message, Type = AlertType.warning };
                return null;
            }

            return GetUserInfo(UserID);
        }

        //获取用户角色指定信息
        public static List<UserRoleAssign> GetRoleUserAssignList(long UserID, out AlertMessage alertMessage)
        {
            alertMessage = null;

            //权限检查
            if (!AUTH.PermissionCheck(ModuleInfo, ActionInfos.Where(i => i.Name == "RoleAssign").FirstOrDefault(), out string Message))
            {
                alertMessage = new AlertMessage { Message = Message, Type = AlertType.warning };
                return null;
            }

            using (var EF = new EF())
            {
                return (from roleInfo in EF.RoleInfos
                        join userRole in (EF.UserRoles.Where(i => i.UserID == UserID)) on roleInfo.ID equals userRole.RoleID into temp1
                        from userRole in temp1.DefaultIfEmpty()
                        where roleInfo.Enabled == true
                        orderby roleInfo.ID
                        select new UserRoleAssign
                        {
                            ID = roleInfo.ID,
                            IconFont = roleInfo.IconFont,
                            Name = roleInfo.Name,
                            Title = roleInfo.Title,
                            Enabled = roleInfo.Enabled,
                            UserID = userRole == null ? 0 : userRole.UserID,
                            UserAssign = userRole == null ? false : true
                        }).ToList();
            }
        }

        //更新用户角色信息
        public static List<UserRole> UserRoleAssignUpload(long[] UsersID, List<UserRole> userRoles, out AlertMessage alertMessage)
        {
            alertMessage = null;

            //权限检查
            if (!AUTH.PermissionCheck(ModuleInfo, ActionInfos.Where(i => i.Name == "RoleAssign").FirstOrDefault(), out string Message))
            {
                alertMessage = new AlertMessage { Message = Message, Type = AlertType.warning };
                return null;
            }

            using (var EF = new EF())
            {
                EF.UserRoles.RemoveRange(EF.UserRoles.Where(i => UsersID.Contains(i.UserID)));
                EF.SaveChanges();
                EF.UserRoles.AddRange(userRoles);
                EF.SaveChanges();
            }
            return userRoles;
        }

        //获取用户个人信息
        public static PersonInfo GetUserPersonInfo(long UserID)
        {
            using (var EF = new EF())
            {
                return (from userPreson in EF.UserPersons
                        join personInfo in EF.PersonInfos on userPreson.PersonID equals personInfo.ID
                        where userPreson.UserID == UserID && userPreson.LogID == null
                        select personInfo).FirstOrDefault();
            }
        }

        //获取用户联系信息
        public static ContactInfo GetUserContactInfo(long UserID)
        {
            using (var EF = new EF())
            {
                return (from userContact in EF.UserContacts
                        join contactInfo in EF.ContactInfos on userContact.ContactID equals contactInfo.ID
                        where userContact.UserID == UserID && userContact.LogID == null
                        select contactInfo).FirstOrDefault();
            }
        }

        //获取用户头像信息
        public static FileInfo GetUserAvatarInfo(long UserID)
        {
            using (var EF = new EF())
            {
                return (from userAvatar in EF.UserAvatars
                        join fileInfo in EF.FileInfos on userAvatar.FileID equals fileInfo.ID
                        where userAvatar.UserID == UserID && userAvatar.LogID == null
                        select fileInfo).FirstOrDefault();
            }
        }

        //发送审核通知
        public static UserNotifications SendNotifications(UserInfo userInfo, MDL.Notifications notifications)
        {
            using (var EF = new EF())
            {
                UserNotifications userNotifications_new = null;
                MDL.Notifications notifications_new = null;
                using (TransactionScope TS = new TransactionScope())
                {
                    //记录通知
                    EF.Notifications.Add(notifications_new = new MDL.Notifications
                    {
                        ModuleTypeID = (int)ModuleType.User,
                        Description = notifications.Description,
                        ProcessURL = notifications.ProcessURL,
                        UpdateTime = notifications.UpdateTime,
                    });
                    EF.SaveChanges();
                    //通知用户
                    EF.UserNotifications.Add(userNotifications_new = new UserNotifications
                    {
                        UserID = userInfo.ID,
                        NotificationsID = notifications_new.ID
                    });
                    EF.SaveChanges();
                    //完成
                    TS.Complete();
                    return userNotifications_new;
                }
            }
        }

        //用户注册审核
        public static void SetUserChecked(long UserID, long NotificationsID, out AlertMessage alertMessage)
        {
            alertMessage = null;

            //权限检查
            if (!AUTH.PermissionCheck(ModuleInfo, ActionInfos.Where(i => i.Name == "Checked").FirstOrDefault(), out string Message))
            {
                alertMessage = new AlertMessage { Message = Message, Type = AlertType.warning };
                return;
            }

            //更改通知状态为已读
            Notifications.SetHaveRead(NotificationsID);

            //审核用户
            var userInfo = GetUserInfo(UserID);
            if (userInfo == null)
            {
                alertMessage = new AlertMessage { Message = string.Format("用户编号[{0}]不存在！", UserID), Type = AlertType.error };
                return;
            }

            if (userInfo.StateID == (int)UserState.启用)
            {
                alertMessage = new AlertMessage { Message = string.Format("用户[{0}]已[{1}]！", userInfo.Name, Enum.Parse(typeof(UserState), userInfo.StateID.ToString())), Type = AlertType.warning };
                return;
            }

            SetUserState(UserID, UserState.启用);
        }

        //用户启用
        public static void SetUserEnabled(long UserID, bool Enabled, out AlertMessage alertMessage)
        {
            alertMessage = null;

            //权限检查
            if (!AUTH.PermissionCheck(ModuleInfo, ActionInfos.Where(i => i.Name == (Enabled ? "Enabled" : "Disabled")).FirstOrDefault(), out string Message))
            {
                alertMessage = new AlertMessage { Message = Message, Type = AlertType.warning };
                return;
            }

            //不允许禁用超级管理员
            var userInfo = GetUserInfo(UserID);
            if (!Enabled && userInfo.Name == "admin")
            {
                alertMessage = new AlertMessage { Message = "不允许禁用系统管理员[admin]！", Type = AlertType.error };
                return;
            }

            //启用禁用
            SetUserState(UserID, Enabled ? UserState.启用 : UserState.禁用);
            alertMessage = new AlertMessage { Message = "用户[" + userInfo.Name + "]" + (Enabled ? "启用" : "禁用") + "成功！", Type = AlertType.success };
        }

        //获取用户角色名称
        public static string GetUserRoleNames(UserInfo userInfo)
        {
            //var userInfo = GetUserInfo(UserID);
            if (userInfo.Name == "admin") { return "系统管理员"; }
            var roleInfo = GetUserRoles(userInfo.ID);
            return roleInfo.Count == 0 ? "无角色用户" : string.Join(",", roleInfo.Select(i => i.Title).ToArray());
        }

        //获取用户角色信息
        internal static List<RoleInfo> GetUserRoles(long UserID)
        {
            using (var EF = new EF())
            {
                return (from userRole in EF.UserRoles
                        join roleInfo in EF.RoleInfos on userRole.RoleID equals roleInfo.ID
                        where userRole.UserID == UserID
                        select roleInfo).ToList();
            }
        }

        //初始化用户信息
        private static void InitUserInfo()
        {
            using (var EF = new EF())
            {
                //清除数据表
                var delCount = EF.Database.ExecuteSqlCommand(@"
                    TRUNCATE TABLE UserInfo;
                    TRUNCATE TABLE UserInfo_log;
                    TRUNCATE TABLE UserAvatar;
                    TRUNCATE TABLE UserPerson;
                    TRUNCATE TABLE UserContact;
                    TRUNCATE TABLE UserModuleAuth;
                    TRUNCATE TABLE UserActionAuth;
                    TRUNCATE TABLE UserRole;"
                );
                //默认用户
                string[] keep_user_names = new string[] { "admin", "administrator", "guest", "member", "office", "jizun" };
                foreach (var name in keep_user_names)
                {
                    EF.UserInfos.Add(new UserInfo
                    {
                        GUID = Guid.NewGuid().ToString(),
                        Name = name,
                        Password = BitConverter.ToString(new SHA256Managed().ComputeHash(Encoding.UTF8.GetBytes("0"))).Replace("-", "").ToLower(),
                        StateID = (int)(new string[] { "admin", "jizun" }.Contains(name) ? UserState.启用 : UserState.禁用),
                        UpdateTime = DateTime.Now,
                    });
                }
                EF.SaveChanges();
            }
        }

        //获取用户信息
        private static UserInfo GetUserInfo(long UserID)
        {
            using (var EF = new EF())
            {
                return EF.UserInfos.Where(i => i.ID == UserID).FirstOrDefault();
            }
        }

        //设置用户状态
        private static UserInfo SetUserState(long UserID, UserState userState)
        {
            using (var EF = new EF())
            {
                var userInfo = EF.UserInfos.Where(i => i.ID == UserID).FirstOrDefault();
                if (userInfo != null)
                {
                    userInfo.StateID = (int)userState;
                    EF.SaveChanges();
                }
                return userInfo;
            }
        }
    }
}
