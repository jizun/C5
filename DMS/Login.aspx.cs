using System;
using System.Web.UI;
using Ziri.MDL;
using Ziri.BLL;

namespace DMS
{
    public partial class Login : Page
    {
        //加载
        protected void Page_Load(object sender, EventArgs e)
        {
            var LoginInfo = Ziri.BLL.Login.GetLoginInfo();
            if (LoginInfo != null)
            {
                //已登录
                Server.Transfer("/Workbench.aspx");
            }
            if (!IsPostBack)
            {
                //企业标语
                var slogan = Ziri.BLL.Slogan.GetRandom();
                txtSlogan.Text = slogan == null ? "欢迎登录" : slogan.Text;
            }
            inp_log_user_name.Focus();
        }

        //注册提交按钮
        protected void Registers_ServerClick(object sender, EventArgs e)
        {
            UserInfo userInfo = new UserInfo
            {
                Name = inp_reg_user_name.Value,
                Password = inp_reg_password.Value
            };
            PersonInfo personInfo = new PersonInfo
            {
                Name = inp_reg_real_name.Value,
                GenderTypeID = (int)GenderType.其他,
                IDCardNO = null,
            };
            ContactInfo contactInfo = new ContactInfo
            {
                Phone = inp_reg_phone.Value,
                EMail = inp_reg_email.Value,
            };

            userInfo = Ziri.BLL.SYS.User.UserInfoUpload(userInfo, null, personInfo, contactInfo, true, out AlertMessage alertMessage);
            string JS = null;
            if (alertMessage != null)
            {
                JS = string.Format("<script> document.getElementById('{0}').click(); swal('{1}', '', '{2}'); </script>"
                    , btn_reg_application.ID, alertMessage.Message, alertMessage.Type);
            }
            else
            {
                var notifications_user = Ziri.BLL.SYS.User.GetUserInfo("admin");
                var notifications = new Ziri.MDL.Notifications
                {
                    Description = string.Format("新用户[{0}]待审核", userInfo.Name),
                    ProcessURL = string.Format("/MOD/SYS/UserInfo.aspx?action=checked&userid={0}", userInfo.ID),
                    UpdateTime = DateTime.Now
                };
                var userNotifications = Ziri.BLL.SYS.User.SendNotifications(notifications_user, notifications);
                if (userNotifications == null)
                {
                    JS = string.Format("<script> swal('注册完成，用户编号[{0}]，通知系统管理员审核失败。', '', '{1}'); </script>"
                        , userInfo.ID, AlertType.warning);
                }
                else
                {
                    JS = string.Format("<script> swal('注册完成，用户编号[{0}]，已提交系统管理员审核，可以通过注册查询功能关注进度。', '', '{1}'); </script>"
                        , userInfo.ID, AlertType.success);
                }
            }
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "RegistersMessage", JS);
        }

        //状态查询按钮
        protected void StateQuery_ServerClick(object sender, EventArgs e)
        {
            var userInfo = Ziri.BLL.SYS.User.GetUserInfo(inp_que_user_name.Value);
            AlertMessage alertMessage = null;
            if (userInfo == null)
            {
                alertMessage = new AlertMessage
                {
                    Type = AlertType.error,
                    Message = "用户[" + inp_que_user_name.Value + "]不存在"
                };
            }
            else if (inp_que_password.Value != userInfo.Password)
            {
                alertMessage = new AlertMessage
                {
                    Type = AlertType.error,
                    Message = "登录密码不正确"
                };
            }
            else
            {
                alertMessage = new AlertMessage
                {
                    Type = userInfo.StateID == (int)UserState.启用 ? AlertType.success : AlertType.warning,
                    Message = string.Format("用户[{0}]的状态为：{1}。", userInfo.Name, Enum.Parse(typeof(UserState), userInfo.StateID.ToString()))
                };
            }
            string JS = string.Format("<script> document.getElementById('{0}').click(); swal('{1}', '', '{2}'); </script>"
                , btn_reg_query.ID, alertMessage.Message, alertMessage.Type);
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "RegistersMessage", JS);
        }

        //登录提交按钮
        protected void Login_ServerClick(object sender, EventArgs e)
        {
            //检查输入
            if (string.IsNullOrWhiteSpace(inp_log_user_name.Value))
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "LoginMessage"
                    , string.Format("<script> LoginAlert('登录错误', '用户名必须填写！', '{0}'); </script>", inp_log_user_name.ID)
                );
                return;
            }
            if (string.IsNullOrWhiteSpace(inp_log_password.Value))
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "LoginMessage"
                    , string.Format("<script> LoginAlert('登录错误', '登录密码必须填写！', '{0}'); </script>", inp_log_password.ID)
                );
                return;
            }

            //查询数据
            var userInfo = Ziri.BLL.SYS.User.GetUserInfo(inp_log_user_name.Value);
            if (userInfo == null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "LoginMessage"
                    , string.Format("<script> LoginAlert('登录错误', '用户名[{0}]不存在！', '{1}'); </script>", inp_log_user_name.Value, inp_log_user_name.ID)
                );
                return;
            }
            if (userInfo.StateID != (int)UserState.启用)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "LoginMessage"
                    , string.Format("<script> LoginAlert('登录错误', '用户[{0}]状态为[{1}]，非[{2}]状态不允许登录！', '{3}'); </script>"
                        , userInfo.Name, Enum.Parse(typeof(UserState), userInfo.StateID.ToString()), UserState.启用, inp_log_user_name.ID)
                );
                return;
            }
            if (inp_log_password.Value != userInfo.Password)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "LoginMessage"
                    , string.Format("<script> LoginAlert('登录错误', 用户密码不正确！', '{0}'); </script>", inp_log_password.ID)
                );
                return;
            }

            //登录完成
            Ziri.BLL.Login.SetLoginInfo(userInfo);

            string RequestURL = null;
            try { RequestURL = Session["RequestURL"].ToString(); Session["RequestURL"] = null; } catch { }
            if (string.IsNullOrWhiteSpace(RequestURL)) { Response.Redirect("/Workbench.aspx"); }
            Server.Transfer(RequestURL);

            //string RequestURL = null;
            //try { RequestURL = Session["RequestURL"].ToString(); Session["RequestURL"] = null; } catch { }
            //Response.Redirect("/" + (string.IsNullOrWhiteSpace(RequestURL) ? null :
            //     (RequestURL.Contains("Workbench.aspx") ? null : "?taburl=" + HttpUtility.UrlEncode(RequestURL))));
        }
    }
}