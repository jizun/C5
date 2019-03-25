using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Ziri.MDL;

namespace DMS.MOD.SYS
{
    public partial class MenuInfo : WorkbenchBase
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

        //菜单初始化时
        public void MenuInit_Click(object sender, EventArgs e)
        {
            Ziri.BLL.SYS.Menu.InitMenuInfo(out AlertMessage alertMessage);
            if (alertMessage != null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "InitMessage"
                    , string.Format("<script> swal('{0}', '', '{1}'); </script>", alertMessage.Message, alertMessage.Type));
                return;
            }

            ListBind();
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "InitMessage"
                        , string.Format("<script> swal('{0}', '', '{1}'); </script>", "菜单初始化完成", AlertType.success));
        }

        //列表绑定
        private void ListBind()
        {
            //菜单组
            lvMenuGroups.DataSource = Ziri.BLL.SYS.Menu.GetMenuGroupInfos(out AlertMessage alertMessage);
            lvMenuGroups.DataBind();
            if (alertMessage != null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "ListMessage"
                    , string.Format("<script> swal('{0}', '', '{1}'); </script>", alertMessage.Message, alertMessage.Type));
                return;
            }

            foreach (var groupItem in lvMenuGroups.Items)
            {
                //根菜单
                var lvMenuRoots = (ListView)groupItem.FindControl("lvMenuRoots");
                lvMenuRoots.DataSource = Ziri.BLL.SYS.Menu.GetGroupMenuInfos(int.Parse(((HiddenField)groupItem.FindControl("lvMenuGroupID")).Value), true);
                lvMenuRoots.DataBind();
                foreach (var rootItem in lvMenuRoots.Items)
                {
                    //一级菜单
                    var lvMenuL1 = (ListView)rootItem.FindControl("lvMenuL1");
                    lvMenuL1.DataSource = Ziri.BLL.SYS.Menu.GetMenuInfos(long.Parse(((HiddenField)rootItem.FindControl("lvMenuRootID")).Value), true);
                    lvMenuL1.DataBind();
                    foreach (var L1Item in lvMenuL1.Items)
                    {
                        //二级菜单
                        var lvMenuL2 = (ListView)L1Item.FindControl("lvMenuL2");
                        lvMenuL2.DataSource = Ziri.BLL.SYS.Menu.GetMenuInfos(long.Parse(((HiddenField)L1Item.FindControl("lvMenuL1ID")).Value), true);
                        lvMenuL2.DataBind();
                    }
                }
            }
        }

        //操作修改时
        public void ListEdit_Click(object sender, EventArgs e)
        {
            var a = (HtmlAnchor)sender;
            long MenuID = long.Parse(a.Attributes["menuid"]);

            txtFormTitle.Text = null;
            hidInfoFormMenuID.Value = MenuID.ToString();
            inpInfoFormName.Value = null;
            inpInfoFormTitle.Value = null;
            inpInfoFormURL.Value = null;
            inpInfoFormIconFont.Value = null;
            inpInfoFormOrderBy.Value = 0.ToString();

            var menuInfo = Ziri.BLL.SYS.Menu.GetModifyInfo(MenuID, out AlertMessage alertMessage);
            if (alertMessage != null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "InfoFormMessage"
                    , string.Format("<script> swal('{0}', '', '{1}'); </script>", alertMessage.Message, alertMessage.Type));
                return;
            }
            if (menuInfo == null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "InfoFormMessage"
                    , string.Format("<script> swal('{0}', '', '{1}'); </script>", "菜单编号[" + MenuID + "]不存在", AlertType.error));
                return;
            }
            txtFormTitle.Text = menuInfo.Title;
            inpInfoFormName.Value = menuInfo.Name;
            inpInfoFormTitle.Value = menuInfo.Title;
            inpInfoFormURL.Value = menuInfo.URL;
            inpInfoFormIconFont.Value = menuInfo.IconFont;
            inpInfoFormOrderBy.Value = menuInfo.OrderBy.ToString();

            Page.ClientScript.RegisterStartupScript(Page.GetType(), "InfoFormActive"
                , string.Format("<script> document.getElementById('{0}').click(); </script>", "btnFormModal"));
        }

        //表单保存时
        public void InfoFormSubmit_Click(object sender, EventArgs e)
        {
            var menuInfo = new Ziri.MDL.MenuInfo()
            {
                ID = long.Parse(hidInfoFormMenuID.Value),
                Name = inpInfoFormName.Value,
                Title = inpInfoFormTitle.Value,
                URL = inpInfoFormURL.Value,
                OrderBy = int.Parse(inpInfoFormOrderBy.Value),
                IconFont = inpInfoFormIconFont.Value
            };

            menuInfo = Ziri.BLL.SYS.Menu.MenuInfoUpload(menuInfo, out AlertMessage alertMessage);
            if (alertMessage == null) { ListBind(); }
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "InfoFormMessage", alertMessage == null
                ? string.Format("<script> swal('保存完成，菜单编号[{0}]。', '', '{1}'); </script>", menuInfo.ID, AlertType.success)
                : string.Format("<script> document.getElementById('{0}').click(); swal('{1}', '', '{2}'); </script>", "btnFormModal", alertMessage.Message, alertMessage.Type));
        }

        //列表操作启用、禁用时
        public void ListEnabled_Click(object sender, EventArgs e)
        {
            var a = (HtmlAnchor)sender;
            long MenuID = long.Parse(a.Attributes["menuid"]);

            Ziri.BLL.SYS.Menu.SetMenuEnabled(MenuID, a.ID == "btnListEnabled", out AlertMessage alertMessage);
            ListBind();
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "EnabledMessage",
                string.Format("<script> swal('{0}', '', '{1}'); </script>", alertMessage.Message, alertMessage.Type));
        }
    }
}