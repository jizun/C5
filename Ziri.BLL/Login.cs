using System.Web;
using Ziri.MDL;

namespace Ziri.BLL
{
    public class Login
    {
        //设置会话信息
        public static void SetLoginInfo(UserInfo UserInfo)
        {
            if (UserInfo == null)
            {
                //关闭会话
                HttpContext.Current.Session["LoginInfo"] = null;
                return;
            }
            else
            {
                //创建会话
                string AvatarURL = "/media/users/avatar_default.png";
                var avatarInfo = SYS.User.GetUserAvatarInfo(UserInfo.ID);
                if (avatarInfo != null)
                {
                    var fileExtName = SYS.DOC.GetFileExtName(avatarInfo.ExtNameID);
                    AvatarURL = string.Format("/DOC/upload/{0}{1}", avatarInfo.GUID, fileExtName.Name);
                }
                
                HttpContext.Current.Session["LoginInfo"] = new LoginInfo
                {
                    UserInfo = UserInfo,
                    UserAvatarURL = AvatarURL,
                    UserRoleName = SYS.User.GetUserRoleNames(UserInfo),
                };
            }
        }

        //获取会话信息
        public static LoginInfo GetLoginInfo()
        {
            if (HttpContext.Current.Session["LoginInfo"] == null) { return null; }
            return (LoginInfo)HttpContext.Current.Session["LoginInfo"];
        }
    }
}
