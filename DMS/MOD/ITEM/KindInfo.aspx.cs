using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Ziri.MDL;

namespace DMS.MOD.ITEM
{
    public partial class KindInfo : WorkbenchBase
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

        //新增按钮
        public void AddNew_Click(object sender, EventArgs e)
        {
            InfoFormFill(0);
        }

        //初始化按钮
        public void Init_Click(object sender, EventArgs e)
        {
            Ziri.BLL.ITEM.Kind.InitKindInfo(out AlertMessage alertMessage);
            if (alertMessage == null) { ListBind(); }
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "InitMessage", alertMessage == null
                ? string.Format("<script> swal('{0}', '', '{1}'); </script>", "模块初始化完成", AlertType.success)
                : string.Format("<script> swal('{0}', '', '{1}'); </script>", alertMessage.Message, alertMessage.Type));
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
            long hidLogoFileID = 0;
            try { hidLogoFileID = long.Parse(hidInfoFormLogoFileID.Value); } catch { }

            var kindInfo = new Ziri.MDL.KindInfo
            {
                ID = long.Parse(hidInfoFormKindID.Value),
                ParentID = long.Parse(hidInfoFormParentID.Value),
                OrderBy = int.Parse(inpInfoFormOrderBy.Text),
                Name = inpInfoFormKindName.Text,
                Title = inpInfoFormKindTitle.Text,
                LogoFileID = FileUploadInfos.Count == 0 ? hidLogoFileID : FileUploadInfos[0].FileInfo.ID,
            };

            kindInfo = Ziri.BLL.ITEM.Kind.KindInfoUpload(kindInfo, out AlertMessage alertMessage);
            if (alertMessage == null) { ListBind(); }
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "InfoFormMessage", alertMessage == null
                ? string.Format("<script> swal('保存完成，品类编号[{0}]。', '', '{1}'); </script>", kindInfo.ID, AlertType.success)
                : string.Format("<script> document.getElementById('btnListInfoFormModal').click(); swal('{0}', '', '{1}'); </script>", alertMessage.Message, alertMessage.Type));
        }

        //信息列表修改按钮
        public void ListEdit_Click(object sender, EventArgs e)
        {
            var a = (HtmlAnchor)sender;
            InfoFormFill(long.Parse(a.Attributes["kindid"]));
        }

        //信息列表新增按钮
        public void ListAddNew_Click(object sender, EventArgs e)
        {
            var a = (HtmlAnchor)sender;
            InfoFormFill(0, long.Parse(a.Attributes["parentid"]));
        }

        //列表绑定
        private void ListBind()
        {
            //根类
            lvKind.DataSource = Ziri.BLL.ITEM.Kind.GetKindRootInfos(out AlertMessage alertMessage);
            lvKind.DataBind();
            if (alertMessage != null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "ListMessage"
                    , string.Format("<script> swal('{0}', '', '{1}'); </script>", alertMessage.Message, alertMessage.Type));
                return;
            }
            ListLevelBild(lvKind);
        }

        private void ListLevelBild(ListView listView)
        {
            //子类
            foreach (var kindRoot in listView.Items)
            {
                var lvKind = (ListView)kindRoot.FindControl("lvKind");
                lvKind.DataSource = Ziri.BLL.ITEM.Kind.GetKindInfos(long.Parse(((HiddenField)kindRoot.FindControl("lvKindID")).Value));
                lvKind.DataBind();
            }
        }

        //列表操作启用、禁用时
        public void ListEnabled_Click(object sender, EventArgs e)
        {
            var a = (HtmlAnchor)sender;
            long KindID = long.Parse(a.Attributes["kindid"]);

            Ziri.BLL.ITEM.Kind.SetKindEnabled(KindID, a.ID == "btnListEnabled", out AlertMessage alertMessage);
            ListBind();
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "EnabledMessage",
                string.Format("<script> swal('{0}', '', '{1}'); </script>", alertMessage.Message, alertMessage.Type));
        }

        //信息表单填充
        private void InfoFormFill(long KindID, long ParentID = 0)
        {
            txtInfoFormTitle.Text = KindID == 0 ? "新品类" : KindID.ToString();
            hidInfoFormKindID.Value = KindID.ToString();
            hidInfoFormParentID.Value = ParentID.ToString();
            txtInfoFormParent.Text = ParentID == 0 ? "根类" : ParentID.ToString();
            inpInfoFormOrderBy.Text = 99.ToString();
            hidInfoFormLogoFileID.Value = 0.ToString();
            divInfoFormLogo.Attributes["style"] = "background-image: url(/media/logos/kind_template.png)";
            inpInfoFormKindName.Text = null;
            inpInfoFormKindTitle.Text = null;

            if (KindID > 0)
            {
                var kindInfo = Ziri.BLL.ITEM.Kind.GetModifyInfo(KindID, out AlertMessage alertMessage);
                if (alertMessage != null)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "InfoFormMessage"
                        , string.Format("<script> swal('{0}', '', '{1}'); </script>", alertMessage.Message, alertMessage.Type));
                    return;
                }
                if (kindInfo == null)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "InfoFormMessage"
                    , string.Format("<script> swal('{0}', '', '{1}'); </script>", "品类编号[" + KindID + "]不存在", AlertType.error));
                    return;
                }
                txtInfoFormTitle.Text = kindInfo.Title;
                hidInfoFormParentID.Value = kindInfo.ParentID.ToString();
                inpInfoFormOrderBy.Text = kindInfo.OrderBy.ToString();
                inpInfoFormKindName.Text = kindInfo.Name;
                inpInfoFormKindTitle.Text = kindInfo.Title;
                if (kindInfo.LogoFileID > 0)
                {
                    hidInfoFormLogoFileID.Value = kindInfo.LogoFileID.ToString();
                    var fileInfo = Ziri.BLL.SYS.DOC.GetFileUploadInfo(kindInfo.LogoFileID);
                    if (fileInfo.FileInfo != null)
                    {
                        divInfoFormLogo.Attributes["style"] = string.Format("background-image: url(/DOC/upload/{0}{1})", fileInfo.FileInfo.GUID, fileInfo.FileExtName.Name);
                    }
                }
                ParentID = kindInfo.ParentID;
            }

            if (ParentID > 0)
            {
                var parentInfo = Ziri.BLL.ITEM.Kind.GetModifyInfo(ParentID, out AlertMessage alertMessage);
                if (alertMessage != null)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "InfoFormMessage"
                        , string.Format("<script> swal('{0}', '', '{1}'); </script>", alertMessage.Message, alertMessage.Type));
                    return;
                }
                if (parentInfo == null)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "InfoFormMessage"
                    , string.Format("<script> swal('{0}', '', '{1}'); </script>", "父类编号[" + ParentID + "]不存在", AlertType.error));
                    return;
                }
                txtInfoFormParent.Text = parentInfo.Title;
            }

            Page.ClientScript.RegisterStartupScript(Page.GetType(), "InfoFormActive", "<script> document.getElementById('btnListInfoFormModal').click(); </script>");
        }

        //列表详情按钮
        public void ListDetails_Click(object sender, EventArgs e)
        {
            var a = (HtmlAnchor)sender;
            long KindID = long.Parse(a.Attributes["kindid"]);

            txtDefailsModalTitle.Text = null;
            divDetailsLogo.Attributes["style"] = "background-image: url(/media/logos/kind_template.png)";
            txtDetailsName.InnerText = null;
            txtDetailsTitle.InnerText = null;
            lvGoodsInfos.DataSource = null;
            lvGoodsInfos.DataBind();

            var kindInfo = Ziri.BLL.ITEM.Kind.GetDetailsInfo(KindID, out AlertMessage alertMessage);
            if (alertMessage != null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "DetailsMessage"
                    , string.Format("<script> swal('{0}', '', '{1}'); </script>", alertMessage.Message, alertMessage.Type));
                return;
            }
            if (kindInfo == null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "DefailsMessage"
                    , string.Format("<script> swal('{0}', '', '{1}'); </script>", "品类编号[" + KindID + "]不存在", AlertType.error));
                return;
            }
            txtDefailsModalTitle.Text = kindInfo.Title;
            txtDetailsName.InnerText = kindInfo.Name;
            txtDetailsTitle.InnerText = kindInfo.Title;
            if (kindInfo.LogoFileID > 0)
            {
                var fileInfo = Ziri.BLL.SYS.DOC.GetFileUploadInfo(kindInfo.LogoFileID);
                if (fileInfo.FileInfo != null)
                {
                    divDetailsLogo.Attributes["style"] = string.Format("background-image: url(/DOC/upload/{0}{1})", fileInfo.FileInfo.GUID, fileInfo.FileExtName.Name);
                }
            }
            lvGoodsInfos.DataSource = Ziri.BLL.ITEM.Kind.GetGoodsInfos(kindInfo.ID);
            lvGoodsInfos.DataBind();

            Page.ClientScript.RegisterStartupScript(Page.GetType(), "DetailsModalActive", "<script> document.getElementById('btnListDetailsModal').click(); </script>");
        }
    }
}