using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Ziri.MDL;

namespace DMS.MOD.SYS
{
    public partial class RoleInfo : WorkbenchBase
    {
        //加载
        protected void Page_Load(object sender, EventArgs e)
        {
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

        //列表绑定
        private void ListBind()
        {
            //筛选条件
            var FilterFields = new List<ListFilterField>();
            if (!string.IsNullOrWhiteSpace(inpFilter.Text.Trim()))
            {
                FilterFields.Add(new ListFilterField
                {
                    Name = "NameAndTitle",
                    CmpareMode = FilterCmpareMode.Like,
                    Value = new List<string>(inpFilter.Text.Trim().Split(' '))
                });
            }

            //排序字段
            var OrderByFields = new List<ListOrderField>();
            foreach (string item in new string[] { "ListOrderByID", "ListOrderByName", "ListOrderByTitle", "ListOrderByEnabled" })
            {
                GetOrderByField(RoleInfoList, item, OrderByFields, out OrderByFields);
            }

            //显示列表页
            var userInfos = Ziri.BLL.SYS.Role.GetRoleInfos(FilterFields, OrderByFields
                , RoleInfoListPager.PageSize, RoleInfoListPager.PageIndex, out long rowCount, out AlertMessage alertMessage);
            RoleInfoList.DataSource = userInfos;
            RoleInfoList.DataBind();
            RoleInfoListPager.RowCount = rowCount;

            //提示信息
            if (alertMessage != null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "ListMessage"
                , string.Format("<script> swal('{0}', '', '{1}'); </script>", alertMessage.Message, alertMessage.Type));
            }
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

        //列表修改按钮
        public void ListEdit_Click(object sender, EventArgs e)
        {
            var a = (HtmlAnchor)sender;
            InfoFormFill(long.Parse(a.Attributes["roleid"]));
        }

        //信息表单填充
        private void InfoFormFill(long RoleID)
        {
            txtInfoFormTitle.Text = RoleID == 0 ? "新角色" : RoleID.ToString();
            hidInfoFormRoleID.Value = RoleID.ToString();
            inpInfoFormRoleName.Text = null;
            inpInfoFormRoleTitle.Text = null;
            inpInfoFormIconFont.Value = null;

            var roleInfo = Ziri.BLL.SYS.Role.GetModifyInfo(RoleID, out AlertMessage alertMessage);
            if (alertMessage != null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "InfoFormMessage"
                    , string.Format("<script> swal('{0}', '', '{1}'); </script>", alertMessage.Message, alertMessage.Type));
                return;
            }

            if (RoleID > 0)
            {
                if (roleInfo == null)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "InfoFormMessage"
                        , string.Format("<script> swal('{0}', '', '{1}'); </script>", "角色编号[" + RoleID + "]不存在", AlertType.error));
                    return;
                }
                txtInfoFormTitle.Text = roleInfo.Name;
                inpInfoFormRoleName.Text = roleInfo.Name;
                inpInfoFormRoleTitle.Text = roleInfo.Title;
                inpInfoFormIconFont.Value = roleInfo.IconFont;
            }

            Page.ClientScript.RegisterStartupScript(Page.GetType(), "InfoFormActive"
                , "<script> document.getElementById('btnListInfoFormModal').click(); </script>");
        }

        //信息表单保存按钮
        public void InfoFormSubmit_Click(object sender, EventArgs e)
        {
            Ziri.MDL.RoleInfo roleInfo = new Ziri.MDL.RoleInfo
            {
                ID = long.Parse(hidInfoFormRoleID.Value),
                Name = inpInfoFormRoleName.Text,
                Title = inpInfoFormRoleTitle.Text,
                IconFont = inpInfoFormIconFont.Value,
            };

            roleInfo = Ziri.BLL.SYS.Role.RoleInfoUpload(roleInfo, out AlertMessage alertMessage);
            if (alertMessage == null) { ListBind(); }
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "InfoFormMessage", alertMessage == null
                ? string.Format("<script> swal('保存完成，角色编号[{0}]。', '', '{1}'); </script>", roleInfo.ID, AlertType.success)
                : string.Format("<script> document.getElementById('btnListInfoFormModal').click(); swal('{0}', '', '{1}'); </script>", alertMessage.Message, alertMessage.Type));
        }

        //列表详情按钮
        public void ListDetails_Click(object sender, EventArgs e)
        {
            var a = (HtmlAnchor)sender;
            long RoleID = long.Parse(a.Attributes["roleid"]);

            txtDefailsModalTitle.Text = null;
            txtDetailsIconFnot.InnerHtml = null;
            txtDetailsName.InnerText = null;
            txtDetailsTitle.InnerText = null;
            lvRoleUserInfos.DataSource = null;
            lvRoleUserInfos.DataBind();

            var roleInfo = Ziri.BLL.SYS.Role.GetDetailsInfo(RoleID, out AlertMessage alertMessage);
            if (alertMessage != null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "DetailsMessage"
                    , string.Format("<script> swal('{0}', '', '{1}'); </script>", alertMessage.Message, alertMessage.Type));
                return;
            }
            if (roleInfo == null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "DefailsMessage"
                    , string.Format("<script> swal('{0}', '', '{1}'); </script>", "角色编号[" + RoleID + "]不存在", AlertType.error));
                return;
            }
            txtDefailsModalTitle.Text = roleInfo.Title;
            txtDetailsIconFnot.InnerHtml = "<i class=\"" + roleInfo.IconFont + "\"></i>";
            txtDetailsName.InnerText = roleInfo.Name;
            txtDetailsTitle.InnerText = roleInfo.Title;
            lvRoleUserInfos.DataSource = Ziri.BLL.SYS.Role.GetRoleUsers(roleInfo.ID);
            lvRoleUserInfos.DataBind();

            Page.ClientScript.RegisterStartupScript(Page.GetType(), "DetailsModalActive"
                , "<script> document.getElementById('btnListDetailsModal').click(); </script>");
        }

        //列表状态按钮
        public void ListEnabled_Change(object sender, EventArgs e)
        {
            var checkbox = (HtmlInputCheckBox)sender;
            Ziri.BLL.SYS.Role.SetRoleEnabled(long.Parse(checkbox.Attributes["roleid"]), checkbox.Checked, out AlertMessage alertMessage);
            if (alertMessage.Type == AlertType.success) { ListBind(); };
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "EnabledMessage"
                , string.Format("<script> swal('{0}', '', '{1}'); </script>", alertMessage.Message, alertMessage.Type));
        }

        //列表模块授权按钮
        public void ListModuleAuth_Click(object sender, EventArgs e)
        {
            var a = (HtmlAnchor)sender;
            long RoleID = long.Parse(a.Attributes["roleid"]);

            //角色信息
            var roleInfo = Ziri.BLL.SYS.Role.GetAuthInfo(RoleID, out AlertMessage alertMessage);
            if (alertMessage != null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "AuthFormMessage"
                    , string.Format("<script> swal('{0}', '', '{1}'); </script>", alertMessage.Message, alertMessage.Type));
                return;
            }
            hidModuleAuthRoleID.Value = roleInfo.ID.ToString();
            txtModuleAuthRoleTitle.Text = roleInfo.Title;

            //模块列表
            lvModuleList.DataSource = Ziri.BLL.SYS.AUTH.GetModuleRoleAuthList(roleInfo.ID, out alertMessage);
            lvModuleList.DataBind();
            foreach (var item in lvModuleList.Items)
            {
                //操作列表
                var lvActionList = (ListView)item.FindControl("lvActionList");
                lvActionList.DataSource = Ziri.BLL.SYS.AUTH.GetModuleActionRoleAuthList(roleInfo.ID
                    , long.Parse(((HtmlInputCheckBox)item.FindControl("inpModuleAuth")).Attributes["moduleid"]), out alertMessage);
                lvActionList.DataBind();
            }
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "AuthorityModalActive"
                , "<script> document.getElementById('btnListModuleAuthModal').click(); </script>");
        }

        //授权表单保存按钮
        public void AuthFormSubmit_Click(object sender, EventArgs e)
        {
            long RoleID = long.Parse(hidModuleAuthRoleID.Value);
            List<RoleModuleAuth> roleModuleAuths = new List<RoleModuleAuth>();
            List<RoleActionAuth> roleActionAuths = new List<RoleActionAuth>();
            //模块列表
            foreach (var module in lvModuleList.Items)
            {
                var moduleCheckbox = (HtmlInputCheckBox)module.FindControl("inpModuleAuth");
                if (moduleCheckbox.Checked)
                {
                    roleModuleAuths.Add(new RoleModuleAuth
                    {
                        RoleID = RoleID,
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
                        roleActionAuths.Add(new RoleActionAuth
                        {
                            RoleID = RoleID,
                            ActionID = long.Parse(actionCheckbox.Attributes["actionid"]),
                        });
                    }
                }
            }

            var RolesID = new long[] { RoleID };
            roleModuleAuths = Ziri.BLL.SYS.AUTH.RoleModuleAuthUpload(RolesID, roleModuleAuths, out AlertMessage alertMessage);
            if (alertMessage == null)
            {
                roleActionAuths = Ziri.BLL.SYS.AUTH.RoleActionAuthUpload(RolesID, roleActionAuths, out alertMessage);
            }
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "AuthFormMessage", alertMessage == null
                ? string.Format("<script> swal('角色[{0}]模块授权完成。', '', '{1}'); </script>", txtModuleAuthRoleTitle.Text, AlertType.success)
                : string.Format("<script> swal('{0}', '', '{1}'); </script>", alertMessage.Message, alertMessage.Type));
        }

        //初始化按钮
        public void RoleInit_Click(object sender, EventArgs e)
        {
            Ziri.BLL.SYS.Role.InitRoleInfo(out AlertMessage alertMessage);
            if (alertMessage != null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "InitMessage"
                        , string.Format("<script> swal('{0}', '', '{1}'); </script>", alertMessage.Message, alertMessage.Type));
                return;
            }

            ListBind();
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "InitMessage"
                , string.Format("<script> swal('{0}', '', '{1}'); </script>", "模块初始化完成", AlertType.success));
        }
    }
}