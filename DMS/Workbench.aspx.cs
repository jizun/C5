using System;
using System.Linq;
using System.Web.UI.WebControls;
using Ziri.MDL;

namespace DMS
{
    public partial class Workbench : WorkbenchBase
    {
        //加载
        protected void Page_Load(object sender, EventArgs e)
        {
            AsideMenuBind();
            UserBarBind();
            NotificationsBind();
            //if (Request["taburl"] != null)
            //{
            //    string JS = "<script> $(document).ready(function () { var a = document.createElement('a'); a.setAttribute('data-url', '" + Request["taburl"] + "'); a.setAttribute('data-title', '标题'); Hui_admin_tab(a); } </script>";
            //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "tabURL", JS);
            //}
        }

        //绑定左边菜单
        private void AsideMenuBind()
        {
            //根菜单
            lvMenuRoots.DataSource = Ziri.BLL.SYS.Menu.GetGroupMenuInfos((int)MenuGroupType.AsideMenu, true);
            lvMenuRoots.DataBind();
            foreach (var rootItem in lvMenuRoots.Items)
            {
                //一级菜单
                ListView lv1 = (ListView)rootItem.FindControl("lvMenuLevel1");
                lv1.DataSource = Ziri.BLL.SYS.Menu.GetMenuInfos(long.Parse(((HiddenField)rootItem.FindControl("lvMenuRootID")).Value), true);
                lv1.DataBind();
                foreach (var lv1item in lv1.Items)
                {
                    //二级菜单
                    ListView lv2 = (ListView)lv1item.FindControl("lvMenuLevel2");
                    lv2.DataSource = Ziri.BLL.SYS.Menu.GetMenuInfos(long.Parse(((HiddenField)lv1item.FindControl("lvMenuLevel1ID")).Value), true);
                    lv2.DataBind();
                }
            }
        }

        //绑定用户工具条
        private void UserBarBind()
        {
            txtLoginUserName.InnerText = txtLoginUserName2.InnerText = LoginInfo.UserInfo.Name;
            imgLoginUserAvatar.Src = imgLoginUserAvatar2.Src = LoginInfo.UserAvatarURL;
            txtLoginUserRole.InnerText = LoginInfo.UserRoleName;
        }

        //绑定通知
        private void NotificationsBind()
        {
            int NotificationsCount = 0;

            //角色通知

            //用户通知
            var userNotifications = Ziri.BLL.Notifications.GetWorkbenchNotifications(LoginInfo.UserInfo.ID);
            NotificationsCount += userNotifications.Count;

            //合并角色与用户通知

            //显示通知
            txt_notifications_count.InnerText = (NotificationsCount == 0 ? "您没有新通知" : "您有" + NotificationsCount + "条新通知");
            icoNotification.Visible = !(NotificationsCount == 0);
            if (NotificationsCount > 0)
            {
                livNotification.DataSource = userNotifications;
                livNotification.DataBind();
            }
        }

        //退出登录时
        protected void Logout_ServerClick(object sender, EventArgs e)
        {
            Ziri.BLL.Login.SetLoginInfo(null);
            Response.Redirect("/Login.aspx");
        }
    }
}