using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using Ziri.MDL;

namespace DMS.MOD.ITEM
{
    public partial class BrandInfo : WorkbenchBase
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

        //初始化按钮
        public void Init_Click(object sender, EventArgs e)
        {
            Ziri.BLL.ITEM.Brand.InitBrandInfo(out AlertMessage alertMessage);
            if (alertMessage == null) { ListBind(); }
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "InitMessage", alertMessage == null
                ? string.Format("<script> swal('{0}', '', '{1}'); </script>", "模块初始化完成", AlertType.success)
                : string.Format("<script> swal('{0}', '', '{1}'); </script>", alertMessage.Message, alertMessage.Type));
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

        //信息表单保存按钮
        public void InfoFormSubmit_Click(object sender, EventArgs e)
        {
            //logo
            long hidLogoFileID = 0;
            try { hidLogoFileID = long.Parse(hidInfoFormLogoFileID.Value); } catch { }
            FileUploadInfo logoUploadInfo = null;
            if (inpInfoFormLogo.PostedFile.ContentLength > 0)
            {
                logoUploadInfo = Ziri.BLL.SYS.DOC.Upload(inpInfoFormLogo.PostedFile, MapPath("/DOC/upload/"), out string Message);
                if (Message != null)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "InfoFormSaveMessage"
                        , string.Format("<script> document.getElementById('btnListInfoFormModal').click(); swal('{0}', '', '{1}'); </script>", Message, AlertType.error));
                    return;
                }
            }

            //banner
            long hidBannerFileID = 0;
            try { hidBannerFileID = long.Parse(hidInfoFormBannerFileID.Value); } catch { }
            FileUploadInfo bannerUploadInfo = null;
            if (inpInfoFormBanner.PostedFile.ContentLength > 0)
            {
                bannerUploadInfo = Ziri.BLL.SYS.DOC.Upload(inpInfoFormBanner.PostedFile, MapPath("/DOC/upload/"), out string Message);
                if (Message != null)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "InfoFormSaveMessage"
                        , string.Format("<script> document.getElementById('btnListInfoFormModal').click(); swal('{0}', '', '{1}'); </script>", Message, AlertType.error));
                    return;
                }
            }

            //post data
            var BrandInfo = new Ziri.MDL.BrandInfo
            {
                ID = long.Parse(hidInfoFormBrandID.Value),
                Name = inpInfoFormBrandName.Text,
                Title = inpInfoFormBrandTitle.Text,
                LogoFileID = logoUploadInfo == null ? hidLogoFileID : logoUploadInfo.FileInfo.ID,
                BannerFileID = bannerUploadInfo == null ? hidBannerFileID : bannerUploadInfo.FileInfo.ID,
            };

            BrandInfo = Ziri.BLL.ITEM.Brand.BrandInfoUpload(BrandInfo, out AlertMessage alertMessage);
            if (alertMessage == null) { ListBind(); }
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "InfoFormMessage", alertMessage == null
                ? string.Format("<script> swal('保存完成，品牌编号[{0}]。', '', '{1}'); </script>", BrandInfo.ID, AlertType.success)
                : string.Format("<script> document.getElementById('btnListInfoFormModal').click(); swal('{0}', '', '{1}'); </script>", alertMessage.Message, alertMessage.Type));
        }

        //信息列表修改按钮
        public void ListEdit_Click(object sender, EventArgs e)
        {
            var a = (HtmlAnchor)sender;
            InfoFormFill(long.Parse(a.Attributes["brandid"]));
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
                GetOrderByField(BrandInfoList, item, OrderByFields, out OrderByFields);
            }

            //显示列表页
            var userInfos = Ziri.BLL.ITEM.Brand.GetBrandInfos(FilterFields, OrderByFields
                , InfoListPager.PageSize, InfoListPager.PageIndex, out long rowCount, out AlertMessage alertMessage);
            BrandInfoList.DataSource = userInfos;
            BrandInfoList.DataBind();
            InfoListPager.RowCount = rowCount;

            //提示信息
            if (alertMessage != null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "ListMessage"
                , string.Format("<script> swal('{0}', '', '{1}'); </script>", alertMessage.Message, alertMessage.Type));
            }
        }

        //列表操作启用、禁用时
        public void ListEnabled_Click(object sender, EventArgs e)
        {
            var a = (HtmlAnchor)sender;
            long BrandID = long.Parse(a.Attributes["brandid"]);

            Ziri.BLL.ITEM.Brand.SetBrandEnabled(BrandID, a.ID == "btnListEnabled", out AlertMessage alertMessage);
            ListBind();
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "EnabledMessage",
                string.Format("<script> swal('{0}', '', '{1}'); </script>", alertMessage.Message, alertMessage.Type));
        }

        //信息表单填充
        private void InfoFormFill(long BrandID)
        {
            txtInfoFormTitle.Text = BrandID == 0 ? "新品牌" : BrandID.ToString();
            hidInfoFormBrandID.Value = BrandID.ToString();
            hidInfoFormLogoFileID.Value = 0.ToString();
            divInfoFormLogo.Attributes["style"] = "background-image: url(/media/logos/logo_template.png)";
            hidInfoFormBannerFileID.Value = 0.ToString();
            divInfoFormBanner.Attributes["style"] = "background-image: url(/media/logos/kind-banner.gif)";
            inpInfoFormBrandName.Text = null;
            inpInfoFormBrandTitle.Text = null;

            if (BrandID > 0)
            {
                var brandInfo = Ziri.BLL.ITEM.Brand.GetModifyInfo(BrandID, out AlertMessage alertMessage);
                if (alertMessage != null)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "InfoFormMessage"
                        , string.Format("<script> swal('{0}', '', '{1}'); </script>", alertMessage.Message, alertMessage.Type));
                    return;
                }
                if (brandInfo == null)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "InfoFormMessage"
                    , string.Format("<script> swal('{0}', '', '{1}'); </script>", "品牌编号[" + BrandID + "]不存在", AlertType.error));
                    return;
                }
                txtInfoFormTitle.Text = brandInfo.Title;
                inpInfoFormBrandName.Text = brandInfo.Name;
                inpInfoFormBrandTitle.Text = brandInfo.Title;
                if (brandInfo.LogoFileID > 0)
                {
                    hidInfoFormLogoFileID.Value = brandInfo.LogoFileID.ToString();
                    var fileInfo = Ziri.BLL.SYS.DOC.GetFileUploadInfo(brandInfo.LogoFileID);
                    if (fileInfo.FileInfo != null)
                    {
                        divInfoFormLogo.Attributes["style"] = string.Format("background-image: url(/DOC/upload/{0}{1})", fileInfo.FileInfo.GUID, fileInfo.FileExtName.Name);
                    }
                }
                if (brandInfo.BannerFileID > 0)
                {
                    hidInfoFormBannerFileID.Value = brandInfo.BannerFileID.ToString();
                    var fileInfo = Ziri.BLL.SYS.DOC.GetFileUploadInfo(brandInfo.BannerFileID);
                    if (fileInfo.FileInfo != null)
                    {
                        divInfoFormBanner.Attributes["style"] = string.Format("background-image: url(/DOC/upload/{0}{1})", fileInfo.FileInfo.GUID, fileInfo.FileExtName.Name);
                    }
                }
            }

            Page.ClientScript.RegisterStartupScript(Page.GetType(), "InfoFormActive", "<script> document.getElementById('btnListInfoFormModal').click(); </script>");
        }

        //列表详情按钮
        public void ListDetails_Click(object sender, EventArgs e)
        {
            var a = (HtmlAnchor)sender;
            long BrandID = long.Parse(a.Attributes["brandid"]);

            txtDefailsModalTitle.Text = null;
            divDetailsLogo.Attributes["style"] = "background-image: url(/media/logos/logo_template.png)";
            txtDetailsName.InnerText = null;
            txtDetailsTitle.InnerText = null;
            lvGoodsInfos.DataSource = null;
            lvGoodsInfos.DataBind();

            var brandInfo = Ziri.BLL.ITEM.Brand.GetDetailsInfo(BrandID, out AlertMessage alertMessage);
            if (alertMessage != null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "DetailsMessage"
                    , string.Format("<script> swal('{0}', '', '{1}'); </script>", alertMessage.Message, alertMessage.Type));
                return;
            }
            if (brandInfo == null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "DefailsMessage"
                    , string.Format("<script> swal('{0}', '', '{1}'); </script>", "品牌编号[" + BrandID + "]不存在", AlertType.error));
                return;
            }
            txtDefailsModalTitle.Text = brandInfo.Title;
            txtDetailsName.InnerText = brandInfo.Name;
            txtDetailsTitle.InnerText = brandInfo.Title;
            if (brandInfo.LogoFileID > 0)
            {
                var fileInfo = Ziri.BLL.SYS.DOC.GetFileUploadInfo(brandInfo.LogoFileID);
                if (fileInfo.FileInfo != null)
                {
                    divDetailsLogo.Attributes["style"] = string.Format("background-image: url(/DOC/upload/{0}{1})", fileInfo.FileInfo.GUID, fileInfo.FileExtName.Name);
                }
            }
            lvGoodsInfos.DataSource = Ziri.BLL.ITEM.Brand.GetGoodsInfos(brandInfo.ID);
            lvGoodsInfos.DataBind();

            Page.ClientScript.RegisterStartupScript(Page.GetType(), "DetailsModalActive", "<script> document.getElementById('btnListDetailsModal').click(); </script>");
        }
    }
}