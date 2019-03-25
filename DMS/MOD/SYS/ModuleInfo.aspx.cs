using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using Ziri.MDL;

namespace DMS.MOD.SYS
{
    public partial class ModuleInfo : WorkbenchBase
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
            foreach (string item in new string[] { "ListOrderByID", "ListOrderByName", "ListOrderByTitle", "ListOrderByIcon" })
            {
                GetOrderByField(ModuleInfoList, item, OrderByFields, out OrderByFields);
            }

            //显示列表页
            var userInfos = Ziri.BLL.SYS.Module.GetModuleInfos(FilterFields, OrderByFields
                , ModuleInfoListPager.PageSize, ModuleInfoListPager.PageIndex, out long rowCount, out AlertMessage alertMessage);
            ModuleInfoList.DataSource = userInfos;
            ModuleInfoList.DataBind();
            ModuleInfoListPager.RowCount = rowCount;

            //提示信息
            if (alertMessage != null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "ListMessage"
                , string.Format("<script> swal('{0}', '', '{1}'); </script>", alertMessage.Message, alertMessage.Type));
            }
        }

        //列表换页按钮
        public void PagerChange(object sender, EventArgs e)
        {
            ListBind();
        }

        //列表筛选按钮
        public void Filter_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(inpFilter.Text.Trim())) { return; }
            ListBind();
        }

        //列表筛选回车
        public void Filter_Change(object sender, EventArgs e)
        {
            ListBind();
        }

        //列表排序按钮
        public void OrderBy_Click(object sender, EventArgs e)
        {
            SetOrderByFlag(sender);
            ListBind();
        }

        //列表详情按钮
        public void ListDetails_Click(object sender, EventArgs e)
        {
            var a = (HtmlAnchor)sender;
            long ModuleID = long.Parse(a.Attributes["moduleid"]);

            txtDetailsModalTitle.Text = null;
            txtDetailsIconFnot.InnerHtml = null;
            txtDetailsName.InnerText = null;
            txtDetailsTitle.InnerText = null;
            lvActionInfos.DataSource = null;
            lvActionInfos.DataBind();

            var moduleInfo = Ziri.BLL.SYS.Module.GetModuleDetails(ModuleID, out AlertMessage alertMessage);
            if (alertMessage != null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "DetailsMessage"
                    , string.Format("<script> swal('{0}', '', '{1}'); </script>", alertMessage.Message, alertMessage.Type));
                return;
            }
            if (moduleInfo == null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "DefailsMessage"
                    , string.Format("<script> swal('{0}', '', '{1}'); </script>", "模块编号[" + ModuleID + "]不存在", AlertType.error));
                return;
            }
            txtDetailsModalTitle.Text = moduleInfo.Title;
            txtDetailsIconFnot.InnerHtml = "<i class=\"" + moduleInfo.IconFont + "\"></i>";
            txtDetailsName.InnerText = moduleInfo.Name;
            txtDetailsTitle.InnerText = moduleInfo.Title;

            var actionInfo = Ziri.BLL.SYS.Module.GetModuleActionInfo(ModuleID);
            if (actionInfo != null)
            {
                lvActionInfos.DataSource = actionInfo;
                lvActionInfos.DataBind();
            }

            Page.ClientScript.RegisterStartupScript(Page.GetType(), "DetailsModalActive"
                , "<script> document.getElementById('btnListDetailsModal').click(); </script>");
        }

        //列表修改按钮
        public void ListEdit_Click(object sender, EventArgs e)
        {
            var a = (HtmlAnchor)sender;
            long ModuleID = long.Parse(a.Attributes["moduleid"]);

            txtInfoFormTitle.Text = null;
            hidInfoFormModuleID.Value = ModuleID.ToString();
            inpInfoFormName.Value = null;
            inpInfoFormTitle.Value = null;
            inpInfoFormIconFont.Value = null;

            if (ModuleID > 0)
            {
                var moduleInfo = Ziri.BLL.SYS.Module.GetModuleModify(ModuleID, out AlertMessage alertMessage);
                if (alertMessage != null)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "InfoFormMessage"
                        , string.Format("<script> swal('{0}', '', '{1}'); </script>", alertMessage.Message, alertMessage.Type));
                    return;
                }
                if (moduleInfo == null)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "InfoFormMessage"
                        , string.Format("<script> swal('{0}', '', '{1}'); </script>", "模块编号[" + ModuleID + "]不存在", AlertType.error));
                    return;
                }
                txtInfoFormTitle.Text = moduleInfo.Title;
                inpInfoFormName.Value = moduleInfo.Name;
                inpInfoFormTitle.Value = moduleInfo.Title;
                inpInfoFormIconFont.Value = moduleInfo.IconFont;
            }

            Page.ClientScript.RegisterStartupScript(Page.GetType(), "InfoFormActive"
                , string.Format("<script> document.getElementById('{0}').click(); </script>", "btnFormModal"));
        }

        //信息表单保存按钮
        public void InfoFormSubmit_Click(object sender, EventArgs e)
        {
            var moduleInfo = new Ziri.MDL.ModuleInfo()
            {
                ID = long.Parse(hidInfoFormModuleID.Value),
                Name = inpInfoFormName.Value,
                Title = inpInfoFormTitle.Value,
                IconFont = inpInfoFormIconFont.Value
            };

            moduleInfo = Ziri.BLL.SYS.Module.ModuleInfoUpload(moduleInfo, out AlertMessage alertMessage);
            if (alertMessage == null) { ListBind(); }
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "InfoFormMessage", alertMessage == null
                    ? string.Format("<script> swal('保存完成，模块编号[{0}]。', '', '{1}'); </script>", moduleInfo.ID, AlertType.success)
                    : string.Format("<script> swal('{0}', '', '{1}'); </script>", alertMessage.Message, alertMessage.Type));
        }

        //初始化按钮
        public void ModuleInit_Click(object sender, EventArgs e)
        {
            Ziri.BLL.SYS.Module.InitModuleInfo(out AlertMessage alertMessage);
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