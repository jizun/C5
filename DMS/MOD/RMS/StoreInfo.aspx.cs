using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Ziri.MDL;

namespace DMS.MOD.RMS
{
    public partial class StoreInfo : WorkbenchBase
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
            Ziri.BLL.RMS.Store.InitStoreInfo(out AlertMessage alertMessage);
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
            var FileUploadInfos = new List<FileUploadInfo>();
            if (inpInfoFormLogo.PostedFile.ContentLength > 0)
            {
                var FileUploadInfo = Ziri.BLL.SYS.DOC.Upload(inpInfoFormLogo.PostedFile, MapPath("/DOC/upload/"), out string Message);
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
            //店名
            var storeInfo = new Ziri.MDL.StoreInfo
            {
                ID = long.Parse(hidInfoFormStoreID.Value),
                Name = inpInfoFormStoreName.Text,
                Title = inpInfoFormStoreTitle.Text,
                LogoFileID = FileUploadInfos.Count == 0 ? hidLogoFileID : FileUploadInfos[0].FileInfo.ID,
            };
            //联系信息
            var latitude = 0M;
            try { latitude = decimal.Parse(inpInfoFormLatitude.Text); } catch { }
            var longitude = 0M;
            try { longitude = decimal.Parse(inpInfoFormLongitude.Text); } catch { }
            var contactInfo = new ContactInfo
            {
                EMail = inpInfoFormEmail.Value,
                Phone = inpInfoFormPhone.Value,
                Address = inpInfoFormAddress.Text,
                Latitude = latitude,
                Longitude = longitude,
            };
            //图文
            var photoUploadInfos = JsonConvert.DeserializeObject<List<FileUploadInfo>>(hidInfoFormStorePhoto.Value);
            var storePhoto = new StorePhoto
            {
                FileIDs = string.Join(",", photoUploadInfos.Select(i => i.FileInfo.ID)),
            };
            var storeDesc = new StoreDesc
            {
                BusinessHours = inpInfoFormBusinessHours.Text,
                Description = HttpUtility.UrlDecode(hidInfoFormStoreDesc.Value)
            };
            //保存
            storeInfo = Ziri.BLL.RMS.Store.StoreInfoUpload(storeInfo, contactInfo, storePhoto, storeDesc, out AlertMessage alertMessage);
            if (alertMessage == null) { ListBind(); }
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "InfoFormMessage", alertMessage == null
                ? string.Format("<script> swal('保存完成，门店编号[{0}]。', '', '{1}'); </script>", storeInfo.ID, AlertType.success)
                : string.Format("<script> document.getElementById('btnListInfoFormModal').click(); swal('{0}', '', '{1}'); </script>", alertMessage.Message, alertMessage.Type));
        }

        //信息列表修改按钮
        public void ListEdit_Click(object sender, EventArgs e)
        {
            var a = (HtmlAnchor)sender;
            InfoFormFill(long.Parse(a.Attributes["storeid"]));
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
                GetOrderByField(StoreInfoList, item, OrderByFields, out OrderByFields);
            }

            //显示列表页
            var userInfos = Ziri.BLL.RMS.Store.GetStoreInfos(FilterFields, OrderByFields
                , InfoListPager.PageSize, InfoListPager.PageIndex, out long rowCount, out AlertMessage alertMessage);
            StoreInfoList.DataSource = userInfos;
            StoreInfoList.DataBind();
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
            long StoreID = long.Parse(a.Attributes["storeid"]);

            Ziri.BLL.RMS.Store.SetStoreEnabled(StoreID, a.ID == "btnListEnabled", out AlertMessage alertMessage);
            ListBind();
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "EnabledMessage",
                string.Format("<script> swal('{0}', '', '{1}'); </script>", alertMessage.Message, alertMessage.Type));
        }

        //信息表单填充
        private void InfoFormFill(long StoreID)
        {
            txtInfoFormTitle.Text = StoreID == 0 ? "新门店" : StoreID.ToString();
            hidInfoFormStoreID.Value = StoreID.ToString();

            inpInfoFormStoreName.Text = null;
            inpInfoFormStoreTitle.Text = null;
            hidInfoFormLogoFileID.Value = 0.ToString();
            divInfoFormLogo.Attributes["style"] = "background-image: url(/media/logos/logo_template.png)";

            inpInfoFormEmail.Value = null;
            inpInfoFormPhone.Value = null;
            inpInfoFormAddress.Text = null;
            inpInfoFormLatitude.Text = null;
            inpInfoFormLongitude.Text = null;

            hidInfoFormStorePhoto.Value = "[]";
            hidInfoFormStoreDesc.Value = null;

            if (StoreID > 0)
            {
                var storeDetails = Ziri.BLL.RMS.Store.GetModifyInfo(StoreID, out AlertMessage alertMessage);
                if (alertMessage != null)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "InfoFormMessage"
                        , string.Format("<script> swal('{0}', '', '{1}'); </script>", alertMessage.Message, alertMessage.Type));
                    return;
                }
                if (storeDetails == null)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "InfoFormMessage"
                    , string.Format("<script> swal('{0}', '', '{1}'); </script>", "门店编号[" + StoreID + "]不存在", AlertType.error));
                    return;
                }
                //店名
                txtInfoFormTitle.Text = storeDetails.StoreFullInfo.Title;
                inpInfoFormStoreName.Text = storeDetails.StoreFullInfo.Name;
                inpInfoFormStoreTitle.Text = storeDetails.StoreFullInfo.Title;
                if (storeDetails.StoreFullInfo.LogoFileID > 0)
                {
                    hidInfoFormLogoFileID.Value = storeDetails.StoreFullInfo.LogoFileID.ToString();
                    divInfoFormLogo.Attributes["style"] = string.Format("background-image: url(/DOC/upload/{0}{1})"
                        , storeDetails.StoreFullInfo.FileGUID, storeDetails.StoreFullInfo.FileExtName);
                }
                //联系信息
                if (storeDetails.ContactInfo != null)
                {
                    inpInfoFormEmail.Value = storeDetails.ContactInfo.EMail;
                    inpInfoFormPhone.Value = storeDetails.ContactInfo.Phone;
                    inpInfoFormAddress.Text = storeDetails.ContactInfo.Address;
                    inpInfoFormLatitude.Text = storeDetails.ContactInfo.Latitude.ToString();
                    inpInfoFormLongitude.Text = storeDetails.ContactInfo.Longitude.ToString();
                }
                //图文
                hidInfoFormStorePhoto.Value = storeDetails.PhotoUploadInfos.Count == 0 ? "[]" : JsonConvert.SerializeObject(storeDetails.PhotoUploadInfos);
                inpInfoFormBusinessHours.Text = storeDetails.BusinessHours;
                hidInfoFormStoreDesc.Value = storeDetails.Description == null ? null : HttpUtility.UrlEncode(storeDetails.Description).Replace("+", "%20");
            }

            Page.ClientScript.RegisterStartupScript(Page.GetType(), "InfoFormActive", "<script> document.getElementById('btnListInfoFormModal').click(); </script>");
        }

        //信息表单地址变更时
        public void InfoFormAddress_Change(object sender, EventArgs e)
        {
            inpInfoFormLatitude.Text = 0.ToString();
            inpInfoFormLongitude.Text = 0.ToString();

            //查询
            var QQAPIGeoCode = Ziri.BLL.QQ.WS.GetQQWSGeoCode(inpInfoFormAddress.Text, out AlertMessage alertMessage);
            if (alertMessage != null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "InfoFormMessage"
                        , string.Format("<script> swal('{0}', '', '{1}'); InfoFormTabShow('Contact'); </script>", alertMessage.Message, alertMessage.Type));
                return;
            }
            if (QQAPIGeoCode.status > 0)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "InfoFormMessage"
                        , string.Format("<script> swal('{0}', '', '{1}'); InfoFormTabShow('Contact'); </script>", QQAPIGeoCode.message, AlertType.info));
                return;
            }
            if (QQAPIGeoCode.result == null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "InfoFormMessage"
                        , string.Format("<script> swal('{0}', '', '{1}'); InfoFormTabShow('Contact'); </script>", "腾讯API定位信息返回空", AlertType.info));
                return;
            }

            //结果
            inpInfoFormLatitude.Text = QQAPIGeoCode.result.location.lat.ToString();
            inpInfoFormLongitude.Text = QQAPIGeoCode.result.location.lng.ToString();

            //显示表单
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "InfoFormActive", "<script> InfoFormTabShow('Contact'); </script>");
        }

        //列表详情按钮
        public void ListDetails_Click(object sender, EventArgs e)
        {
            var a = (HtmlAnchor)sender;
            long StoreID = long.Parse(a.Attributes["storeid"]);

            txtDefailsModalTitle.Text = null;
            divDetailsLogo.Attributes["style"] = "background-image: url(/media/logos/logo_template.png)";
            txtDetailsName.InnerText = null;
            txtDetailsTitle.InnerText = null;

            var storeInfo = Ziri.BLL.RMS.Store.GetDetailsInfo(StoreID, out AlertMessage alertMessage);
            if (alertMessage != null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "DetailsMessage"
                    , string.Format("<script> swal('{0}', '', '{1}'); </script>", alertMessage.Message, alertMessage.Type));
                return;
            }
            if (storeInfo == null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "DefailsMessage"
                    , string.Format("<script> swal('{0}', '', '{1}'); </script>", "门店编号[" + StoreID + "]不存在", AlertType.error));
                return;
            }
            txtDefailsModalTitle.Text = storeInfo.Title;
            txtDetailsName.InnerText = storeInfo.Name;
            txtDetailsTitle.InnerText = storeInfo.Title;
            if (storeInfo.LogoFileID > 0)
            {
                var fileInfo = Ziri.BLL.SYS.DOC.GetFileUploadInfo(storeInfo.LogoFileID);
                if (fileInfo.FileInfo != null)
                {
                    divDetailsLogo.Attributes["style"] = string.Format("background-image: url(/DOC/upload/{0}{1})", fileInfo.FileInfo.GUID, fileInfo.FileExtName.Name);
                }
            }

            Page.ClientScript.RegisterStartupScript(Page.GetType(), "DetailsModalActive", "<script> document.getElementById('btnListDetailsModal').click(); </script>");
        }
    }
}