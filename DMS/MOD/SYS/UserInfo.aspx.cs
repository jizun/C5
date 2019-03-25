using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using Ziri.MDL;

namespace DMS.MOD.SYS
{
    public partial class UserInfo : WorkbenchBase
    {
        //加载
        protected void Page_Load(object sender, EventArgs e)
        {
            //操作信息
            string Action = Request["action"];
            long.TryParse(Request["userid"], out long UserID);
            long.TryParse(Request["notificationsid"], out long NotificationsID);
            switch (Action)
            {
                case "checked":
                    //审核操作
                    Ziri.BLL.SYS.User.SetUserChecked(UserID, NotificationsID, out AlertMessage alertMessage);
                    if (alertMessage == null) { ListBind(); }
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CheckedMessage", alertMessage == null
                        ? string.Format("<script> swal('用户[{0}]审核成功并已启用！', '', '{1}'); </script>", UserID, AlertType.success)
                        : string.Format("<script> swal('{0}', '', '{1}'); </script>", alertMessage.Message, alertMessage.Type));
                    break;
            }

            //列表绑定
            if (!IsPostBack)
            {
                ListBind();
            }
        }

        //筛选按钮
        public void Filter_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(inpFilter.Text.Trim())) { return; }
            ListBind();
        }

        //筛选回车
        public void Filter_Change(object sender, EventArgs e)
        {
            ListBind();
        }

        //新增按钮
        public void AddNew_Click(object sender, EventArgs e)
        {
            InfoFormFill(0);
        }

        //列表排序按钮
        public void OrderBy_Click(object sender, EventArgs e)
        {
            SetOrderByFlag(sender);
            ListBind();
        }

        //列表换页按钮
        public void PagerChange(object sender, EventArgs e)
        {
            ListBind();
        }

        //列表状态格式化
        public string StateFormat(int StateID)
        {
            return Enum.Parse(typeof(UserState), StateID.ToString()).ToString();
        }

        //列表修改按钮
        public void ListEdit_Click(object sender, EventArgs e)
        {
            var a = (HtmlAnchor)sender;
            InfoFormFill(long.Parse(a.Attributes["userid"]));
        }

        //信息表单保存按钮
        public void InfoFormSubmit_Click(object sender, EventArgs e)
        {
            var FileUploadInfos = new List<FileUploadInfo>();
            for (int i = 0; i < HttpContext.Current.Request.Files.Count; i++)
            {
                if (HttpContext.Current.Request.Files[i].ContentLength == 0) { continue; }
                var FileUploadInfo = Ziri.BLL.SYS.DOC.Upload(HttpContext.Current.Request.Files[i], MapPath("/DOC/upload/"), out string Message);
                if (Message != null)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "InfoFormSaveMessage"
                        , string.Format("<script> document.getElementById('btnListInfoFormModal').click(); swal('{0}', '', '{1}'); </script>", Message, AlertType.error));
                    return;
                }
                FileUploadInfos.Add(FileUploadInfo);
            }
            long hidAvatarFileID = 0;
            try { hidAvatarFileID = long.Parse(hidInfoFormAvatarFileID.Value); } catch { }
            if (FileUploadInfos.Count == 0 && hidAvatarFileID > 0)
            {
                FileUploadInfos.Add(new FileUploadInfo { FileInfo = new FileInfo { ID = hidAvatarFileID, } });
            }

            Ziri.MDL.UserInfo userInfo = new Ziri.MDL.UserInfo
            {
                ID = long.Parse(hidInfoFormUserID.Value),
                Name = inpInfoFormUserName.Text,
                Password = inpInfoFormPassword.Text
            };
            PersonInfo personInfo = new PersonInfo
            {
                Name = inpInfoFormRealName.Value,
                GenderTypeID = inpInfoFormGenderMale.Checked ? (int)GenderType.男性
                    : inpInfoFormGenderFemale.Checked ? (int)GenderType.女性 : (int)GenderType.其他,
                IDCardNO = inpInfoFormIDCardNO.Value
            };
            if (string.IsNullOrWhiteSpace(inpInfoFormBirthday.Value) == false)
            {
                personInfo.Birthday = DateTime.Parse(inpInfoFormBirthday.Value);
            }
            ContactInfo contactInfo = new ContactInfo
            {
                Mobile = inpInfoFormMobile.Value,
                Phone = inpInfoFormPhone.Value,
                EMail = inpInfoFormEmail.Value,
                Address = inpInfoFormAddress.Value,
            };

            userInfo = Ziri.BLL.SYS.User.UserInfoUpload(userInfo, FileUploadInfos, personInfo, contactInfo, false, out AlertMessage alertMessage);
            if (alertMessage == null) { ListBind(); };
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "InfoFormMessage", alertMessage == null
                ? string.Format("<script> swal('保存完成，用户编号[{0}]。', '', '{1}'); </script>", userInfo.ID, AlertType.success)
                : string.Format("<script> document.getElementById('btnListInfoFormModal').click(); swal('{0}', '', '{1}'); </script>", alertMessage.Message, alertMessage.Type));
        }

        //列表详情按钮
        public void ListDetails_Click(object sender, EventArgs e)
        {
            var a = (HtmlAnchor)sender;
            long UserID = long.Parse(a.Attributes["userid"]);

            //清除表单
            txtDetailsModalTitle.Text = null;
            txtDetailsUserName.InnerText = null;
            txtDetailsPosition.InnerText = null;

            imgDetailsAvatar.Src = "/media/users/avatar_default.png";

            txtDetailsRaleName.InnerText = null;
            txtDetailsGender.InnerText = null;
            txtDetailsBirthday.InnerText = null;
            txtDetailsIDCardNO.InnerText = null;

            txtDetailsPhone.InnerText = null;
            txtDetailsEMail.InnerText = null;
            txtDetailsAddress.InnerText = null;

            if (UserID == 0) { return; }

            var userInfo = Ziri.BLL.SYS.User.GetDetailsInfo(UserID, out AlertMessage alertMessage);
            if (alertMessage != null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "DefailsMessage"
                    , string.Format("<script> swal('{0}', '', '{1}'); </script>", alertMessage.Message, alertMessage.Type));
                return;
            }
            if (userInfo == null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "DefailsMessage"
                    , string.Format("<script> swal('{0}', '', '{1}'); </script>"
                    , "用户编号[" + UserID + "]不存在", AlertType.error));
                return;
            }
            txtDetailsModalTitle.Text = userInfo.Name;
            txtDetailsUserName.InnerText = userInfo.Name;
            txtDetailsPosition.InnerText = Ziri.BLL.SYS.User.GetUserRoleNames(userInfo);

            var avatarInfo = Ziri.BLL.SYS.User.GetUserAvatarInfo(UserID);
            if (avatarInfo != null)
            {
                var fileExtName = Ziri.BLL.SYS.DOC.GetFileExtName(avatarInfo.ExtNameID);
                imgDetailsAvatar.Src = string.Format("/DOC/upload/{0}{1}", avatarInfo.GUID, fileExtName.Name);
            }

            var contactInfo = Ziri.BLL.SYS.User.GetUserContactInfo(UserID);
            if (contactInfo != null)
            {
                txtDetailsPhone.InnerText = contactInfo.Phone;
                txtDetailsEMail.InnerText = contactInfo.EMail;
                txtDetailsAddress.InnerText = contactInfo.Address;
            }

            var personInfo = Ziri.BLL.SYS.User.GetUserPersonInfo(UserID);
            if (personInfo != null)
            {
                txtDetailsRaleName.InnerText = personInfo.Name;
                txtDetailsGender.InnerText = Enum.Parse(typeof(GenderType), personInfo.GenderTypeID.ToString()).ToString();
                txtDetailsBirthday.InnerText = personInfo.Birthday?.ToString("yyyy/MM/dd");
                txtDetailsIDCardNO.InnerText = personInfo.IDCardNO;
            }

            Page.ClientScript.RegisterStartupScript(Page.GetType(), "DetailsModalActive"
                , "<script> document.getElementById('btnListDetailsModal').click(); </script>");
        }

        //列表操作启用、禁用按钮
        public void ListEnabled_Click(object sender, EventArgs e)
        {
            var a = (HtmlAnchor)sender;
            long UserID = long.Parse(a.Attributes["userid"]);

            Ziri.BLL.SYS.User.SetUserEnabled(UserID, a.ID == "btnListEnabled", out AlertMessage alertMessage);
            ListBind();
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "EnabledMessage",
                string.Format("<script> swal('{0}', '', '{1}'); </script>", alertMessage.Message, alertMessage.Type));
        }

        //列表模块授权按钮
        public void ListModuleAuthority_Click(object sender, EventArgs e)
        {
            var a = (HtmlAnchor)sender;
            long UserID = long.Parse(a.Attributes["userid"]);

            //用户信息
            var userInfo = Ziri.BLL.SYS.User.GetAuthInfo(UserID, out AlertMessage alertMessage);
            if (alertMessage != null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "AuthFormMessage"
                    , string.Format("<script> swal('{0}', '', '{1}'); </script>", alertMessage.Message, alertMessage.Type));
                return;
            }
            hidModuleAuthUserID.Value = userInfo.ID.ToString();
            txtModuleAuthUserName.Text = userInfo.Name;

            //模块列表
            lvModuleList.DataSource = Ziri.BLL.SYS.AUTH.GetModuleUserAuthList(userInfo.ID, out alertMessage);
            lvModuleList.DataBind();
            foreach (var item in lvModuleList.Items)
            {
                //操作列表
                var lvActionList = (ListView)item.FindControl("lvActionList");
                lvActionList.DataSource = Ziri.BLL.SYS.AUTH.GetModuleActionUserAuthList(userInfo.ID
                    , long.Parse(((HtmlInputCheckBox)item.FindControl("inpModuleAuth")).Attributes["moduleid"]), out alertMessage);
                lvActionList.DataBind();
            }
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "AuthorityModalActive"
                , "<script> document.getElementById('btnListModuleAuthorityModal').click(); </script>");
        }

        //授权表单保存按钮
        public void AuthFormSubmit_Click(object sender, EventArgs e)
        {
            long UserID = long.Parse(hidModuleAuthUserID.Value);
            List<UserModuleAuth> userModuleAuths = new List<UserModuleAuth>();
            List<UserActionAuth> userActionAuths = new List<UserActionAuth>();
            //模块列表
            foreach (var module in lvModuleList.Items)
            {
                var moduleCheckbox = (HtmlInputCheckBox)module.FindControl("inpModuleAuth");
                if (moduleCheckbox.Checked)
                {
                    userModuleAuths.Add(new UserModuleAuth
                    {
                        UserID = UserID,
                        ModuleID = long.Parse(moduleCheckbox.Attributes["moduleid"]),
                    });
                }
                //操作列表
                var lvActionList = (ListView)module.FindControl("lvActionList");
                foreach (var action in lvActionList.Items)
                {
                    var actionCheckbox = (HtmlInputCheckBox)action.FindControl("inpActionAuth");
                    if (actionCheckbox.Checked)
                    {
                        userActionAuths.Add(new UserActionAuth
                        {
                            UserID = UserID,
                            ActionID = long.Parse(actionCheckbox.Attributes["actionid"]),
                        });
                    }
                }
            }
            var UsersID = new long[] { UserID };
            userModuleAuths = Ziri.BLL.SYS.AUTH.UserModuleAuthUpload(UsersID, userModuleAuths, out AlertMessage alertMessage);
            if (alertMessage == null)
            {
                userActionAuths = Ziri.BLL.SYS.AUTH.UserActionAuthUpload(UsersID, userActionAuths, out alertMessage);
            }
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "InfoFormMessage", alertMessage == null
                ? string.Format("<script> swal('用户[{0}]模块授权完成。', '', '{1}'); </script>", txtModuleAuthUserName.Text, AlertType.success)
                : string.Format("<script> swal('{0}', '', '{1}'); </script>", alertMessage.Message, alertMessage.Type));
        }

        //列表角色指定按钮
        public void ListRoleAssign_Click(object sender, EventArgs e)
        {
            var a = (HtmlAnchor)sender;
            long UserID = long.Parse(a.Attributes["userid"]);

            //用户信息
            var userInfo = Ziri.BLL.SYS.User.GetRoleInfo(UserID, out AlertMessage alertMessage);
            if (alertMessage != null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "RoleFormMessage"
                    , string.Format("<script> swal('{0}', '', '{1}'); </script>", alertMessage.Message, alertMessage.Type));
                return;
            }
            hidRoleAssignUserID.Value = userInfo.ID.ToString();
            txtRoleAssignUserName.Text = userInfo.Name;

            //角色列表
            lvRoleList.DataSource = Ziri.BLL.SYS.User.GetRoleUserAssignList(userInfo.ID, out alertMessage);
            lvRoleList.DataBind();
            foreach (var item in lvRoleList.Items)
            {
                //角色模块权限列表
                var RoleID = long.Parse(((HtmlInputCheckBox)item.FindControl("inpUserRoleAssign")).Attributes["roleid"]);
                var lvRoleModuleAuthList = (ListView)item.FindControl("lvRoleModuleAuthList");
                lvRoleModuleAuthList.DataSource = Ziri.BLL.SYS.Role.GetRoleModuleAuths(RoleID);
                lvRoleModuleAuthList.DataBind();
                foreach (var module in lvRoleModuleAuthList.Items)
                {
                    var lvRoleActionAuthList = (ListView)module.FindControl("lvRoleActionAuthList");
                    var ModuleID = long.Parse(((HiddenField)module.FindControl("ModuleID")).Value);
                    lvRoleActionAuthList.DataSource = Ziri.BLL.SYS.Role.GetRoleModuleActionAuths(ModuleID, RoleID);
                    lvRoleActionAuthList.DataBind();
                }
            }
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "RoleModalActive"
                , "<script> document.getElementById('btnListRoleAssignModal').click(); </script>");

        }

        //角色表单提交按钮
        public void RoleFormSubmit_Click(object sender, EventArgs e)
        {
            long UserID = long.Parse(hidRoleAssignUserID.Value);
            List<UserRole> userRole = new List<UserRole>();
            //角色列表
            foreach (var role in lvRoleList.Items)
            {
                var RoleCheckbox = (HtmlInputCheckBox)role.FindControl("inpUserRoleAssign");
                if (RoleCheckbox.Checked)
                {
                    userRole.Add(new UserRole
                    {
                        UserID = UserID,
                        RoleID = long.Parse(RoleCheckbox.Attributes["roleid"]),
                    });
                }
            }
            var UsersID = new long[] { UserID };
            userRole = Ziri.BLL.SYS.User.UserRoleAssignUpload(UsersID, userRole, out AlertMessage alertMessage);
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "InfoFormMessage", alertMessage == null
                ? string.Format("<script> swal('用户[{0}]角色指定完成。', '', '{1}'); </script>", txtRoleAssignUserName.Text, AlertType.success)
                : string.Format("<script> swal('{0}', '', '{1}'); </script>", alertMessage.Message, alertMessage.Type));
        }

        //列表绑定
        private void ListBind()
        {
            //筛选条件
            var FilterFields = new List<ListFilterField>();
            if (!string.IsNullOrWhiteSpace(inpFilter.Text.Trim()))
            {
                FilterFields.Add(new ListFilterField
                {
                    Name = "Name",
                    CmpareMode = FilterCmpareMode.Like,
                    Value = new List<string>(inpFilter.Text.Trim().Split(' '))
                });
            }

            //排序字段
            var OrderByFields = new List<ListOrderField>();
            foreach (string item in new string[] { "ListOrderByID", "ListOrderByName", "ListOrderByStateID", "ListOrderByUpdateTime" })
            {
                GetOrderByField(UserInfoList, item, OrderByFields, out OrderByFields);
            }

            //显示列表页
            var userInfos = Ziri.BLL.SYS.User.GetUserInfos(FilterFields, OrderByFields
                , UserInfoListPager.PageSize, UserInfoListPager.PageIndex, out long rowCount, out AlertMessage alertMessage);
            UserInfoList.DataSource = userInfos;
            UserInfoList.DataBind();
            UserInfoListPager.RowCount = rowCount;

            //提示信息
            if (alertMessage != null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "ListMessage"
                , string.Format("<script> swal('{0}', '', '{1}'); </script>", alertMessage.Message, alertMessage.Type));
            }
        }

        //信息表单填充
        private void InfoFormFill(long UserID)
        {
            txtInfoFormTitle.Text = UserID == 0 ? "新用户" : UserID.ToString();
            hidInfoFormUserID.Value = UserID.ToString();
            inpInfoFormUserName.Text = null;
            inpInfoFormPassword.Text = null;

            hidInfoFormAvatarFileID.Value = 0.ToString();
            divInfoFormAvatar.Attributes["style"] = "background-image: url(/media/users/avatar_default.png)";

            inpInfoFormRealName.Value = null;
            inpInfoFormGenderMale.Checked = false;
            inpInfoFormGenderFemale.Checked = false;
            inpInfoFormGenderOther.Checked = false;
            inpInfoFormBirthday.Value = null;
            inpInfoFormIDCardNO.Value = null;

            inpInfoFormPhone.Value = null;
            inpInfoFormMobile.Value = null;
            inpInfoFormEmail.Value = null;
            inpInfoFormAddress.Value = null;

            var userInfo = Ziri.BLL.SYS.User.GetModifyInfo(UserID, out AlertMessage alertMessage);
            if (alertMessage != null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "InfoFormMessage"
                    , string.Format("<script> swal('{0}', '', '{1}'); </script>", alertMessage.Message, alertMessage.Type));
                return;
            }

            if (UserID > 0)
            {
                if (userInfo == null)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "InfoFormMessage"
                        , string.Format("<script> swal('{0}', '', '{1}'); </script>"
                        , "用户编号[" + UserID + "]不存在", AlertType.error));
                    return;
                }
                txtInfoFormTitle.Text = userInfo.Name;
                inpInfoFormUserName.Text = userInfo.Name;

                var avatarInfo = Ziri.BLL.SYS.User.GetUserAvatarInfo(UserID);
                if (avatarInfo != null)
                {
                    hidInfoFormAvatarFileID.Value = avatarInfo.ID.ToString();
                    var fileExtName = Ziri.BLL.SYS.DOC.GetFileExtName(avatarInfo.ExtNameID);
                    divInfoFormAvatar.Attributes["style"] = string.Format("background-image: url(/DOC/upload/{0}{1})", avatarInfo.GUID, fileExtName.Name);
                }

                var personInfo = Ziri.BLL.SYS.User.GetUserPersonInfo(UserID);
                if (personInfo != null)
                {
                    inpInfoFormRealName.Value = personInfo.Name;
                    switch (Enum.Parse(typeof(GenderType), personInfo.GenderTypeID.ToString()))
                    {
                        case GenderType.男性:
                            inpInfoFormGenderMale.Checked = true;
                            break;
                        case GenderType.女性:
                            inpInfoFormGenderFemale.Checked = true;
                            break;
                    }
                    if (personInfo.Birthday != null)
                    {
                        inpInfoFormBirthday.Value = personInfo.Birthday?.ToString("yyyy/MM/dd");
                    }
                    inpInfoFormIDCardNO.Value = personInfo.IDCardNO;
                }

                var contactInfo = Ziri.BLL.SYS.User.GetUserContactInfo(UserID);
                if (contactInfo != null)
                {
                    inpInfoFormPhone.Value = contactInfo.Phone;
                    inpInfoFormMobile.Value = contactInfo.Mobile;
                    inpInfoFormEmail.Value = contactInfo.EMail;
                    inpInfoFormAddress.Value = contactInfo.Address;
                }
            }

            Page.ClientScript.RegisterStartupScript(Page.GetType(), "InfoFormActive"
                , "<script> document.getElementById('btnListInfoFormModal').click(); </script>");
        }
    }
}