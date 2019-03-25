using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Linq;
using Ziri.MDL;

namespace DMS.MOD.OMS
{
    public partial class SalesOrder : WorkbenchBase
    {
        //加载
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //列表绑定
                drpFilterState.DataTextField = "Title";
                drpFilterState.DataValueField = "ID";
                drpFilterState.DataSource = Ziri.BLL.OMS.SalesOrder.State;
                drpFilterState.DataBind();
                drpFilterState.Items.Insert(0, new ListItem { Text = "订单状态", Value = string.Empty, Selected = true });

                drpFilterReceiptType.DataTextField = "Title";
                drpFilterReceiptType.DataValueField = "ID";
                drpFilterReceiptType.DataSource = Ziri.BLL.OMS.SalesOrder.ReceiveType;
                drpFilterReceiptType.DataBind();
                drpFilterReceiptType.Items.Insert(0, new ListItem { Text = "收货方式", Value = string.Empty, Selected = true });

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

        //筛选状态按钮
        public void FilterState_Click(object sender, EventArgs e)
        {
            ListBind();
        }

        //筛选收货方式按钮
        public void drpFilterReceiptType_Click(object sender, EventArgs e)
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

        //信息表单保存按钮
        public void InfoFormSubmit_Click(object sender, EventArgs e)
        {
            //var FileUploadInfos = new List<FileUploadInfo>();
            //if (inpInfoFormLogo.PostedFile.ContentLength > 0)
            //{
            //    var FileUploadInfo = Ziri.BLL.SYS.DOC.Upload(inpInfoFormLogo.PostedFile, MapPath("/DOC/upload/"), out string Message);
            //    if (Message != null)
            //    {
            //        Page.ClientScript.RegisterStartupScript(Page.GetType(), "InfoFormSaveMessage"
            //            , string.Format("<script> document.getElementById('btnListInfoFormModal').click(); swal('{0}', '', '{1}'); </script>", Message, AlertType.error));
            //        return;
            //    }
            //    FileUploadInfos.Add(FileUploadInfo);
            //}
            //long hidLogoFileID = 0;
            //try { hidLogoFileID = long.Parse(hidInfoFormLogoFileID.Value); } catch { }
            ////店名
            //var storeInfo = new Ziri.MDL.StoreInfo
            //{
            //    ID = long.Parse(hidInfoFormSOID.Value),
            //    Name = inpInfoFormStoreName.Text,
            //    Title = inpInfoFormStoreTitle.Text,
            //    LogoFileID = FileUploadInfos.Count == 0 ? hidLogoFileID : FileUploadInfos[0].FileInfo.ID,
            //};
            ////联系信息
            //var latitude = 0M;
            //try { latitude = decimal.Parse(inpInfoFormLatitude.Text); } catch { }
            //var longitude = 0M;
            //try { longitude = decimal.Parse(inpInfoFormLongitude.Text); } catch { }
            //var contactInfo = new ContactInfo
            //{
            //    EMail = inpInfoFormEmail.Value,
            //    Phone = inpInfoFormPhone.Value,
            //    Address = inpInfoFormAddress.Text,
            //    Latitude = latitude,
            //    Longitude = longitude,
            //};
            ////图文
            //var photoUploadInfos = JsonConvert.DeserializeObject<List<FileUploadInfo>>(hidInfoFormStorePhoto.Value);
            //var storePhoto = new StorePhoto
            //{
            //    FileIDs = string.Join(",", photoUploadInfos.Select(i => i.FileInfo.ID)),
            //};
            //var storeDesc = new StoreDesc
            //{
            //    BusinessHours = inpInfoFormBusinessHours.Text,
            //    Description = HttpUtility.UrlDecode(hidInfoFormStoreDesc.Value)
            //};
            ////保存
            //storeInfo = Ziri.BLL.RMS.Store.StoreInfoUpload(storeInfo, contactInfo, storePhoto, storeDesc, out AlertMessage alertMessage);
            //if (alertMessage == null) { ListBind(); }
            //Page.ClientScript.RegisterStartupScript(Page.GetType(), "InfoFormMessage", alertMessage == null
            //    ? string.Format("<script> swal('保存完成，门店编号[{0}]。', '', '{1}'); </script>", storeInfo.ID, AlertType.success)
            //    : string.Format("<script> document.getElementById('btnListInfoFormModal').click(); swal('{0}', '', '{1}'); </script>", alertMessage.Message, alertMessage.Type));
        }

        //信息列表修改按钮
        public void ListEdit_Click(object sender, EventArgs e)
        {
            var a = (HtmlAnchor)sender;
            InfoFormFill(long.Parse(a.Attributes["soid"]));
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
                    Name = "BillNO",
                    CmpareMode = FilterCmpareMode.Like,
                    Value = new List<string>(inpFilter.Text.Trim().Split(' '))
                });
            }
            if (!string.IsNullOrWhiteSpace(drpFilterState.SelectedValue))
            {
                FilterFields.Add(new ListFilterField
                {
                    Name = "StateID",
                    CmpareMode = FilterCmpareMode.Equal,
                    Value = new List<string> { drpFilterState.SelectedValue }
                });
            }
            if (!string.IsNullOrWhiteSpace(drpFilterReceiptType.SelectedValue))
            {
                FilterFields.Add(new ListFilterField
                {
                    Name = "ReceiptTypeID",
                    CmpareMode = FilterCmpareMode.Equal,
                    Value = new List<string> { drpFilterReceiptType.SelectedValue }
                });
            }

            //排序字段
            var OrderByFields = new List<ListOrderField>();
            foreach (string item in new string[] { "ListOrderByID", "ListOrderByBillNO"
                ,"ListOrderByGoodsName", "ListOrderByGoodsTitle", "ListOrderByStateID"
                , "ListOrderByCreateTime", "ListOrderByUpdateTime" })
            {
                GetOrderByField(StoreInfoList, item, OrderByFields, out OrderByFields);
            }

            //显示列表页
            var userInfos = Ziri.BLL.OMS.SalesOrder.GetSOInfos(FilterFields, OrderByFields
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

        //列表确认按钮
        public void ListConfirm_Click(object sender, EventArgs e)
        {
            var a = (HtmlAnchor)sender;

            var SOInfo = Ziri.BLL.OMS.SalesOrder.SetConfirm(long.Parse(a.Attributes["soid"]), true, out string Message);
            if (Message != null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "ConfirmMessage"
                   , string.Format("<script> swal('{0}', '', '{1}'); </script>", Message, AlertType.error));
                return;
            }
            ListBind();
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "ConfirmMessage"
                , string.Format("<script> swal('{0}', '', '{1}'); </script>", "订单[" + SOInfo.ID + "]确认完成！", AlertType.success));
        }

        //列表撤消确认按钮
        public void ListUnconfirm_Click(object sender, EventArgs e)
        {
            var a = (HtmlAnchor)sender;

            var SOInfo = Ziri.BLL.OMS.SalesOrder.SetConfirm(long.Parse(a.Attributes["soid"]), false, out string Message);
            if (Message != null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "ConfirmMessage"
                   , string.Format("<script> swal('{0}', '', '{1}'); </script>", Message, AlertType.error));
                return;
            }
            ListBind();
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "ConfirmMessage"
                , string.Format("<script> swal('{0}', '', '{1}'); </script>", "订单[" + SOInfo.ID + "]撤销确认完成！", AlertType.success));
        }

        //列表发货按钮
        public void ListSend_Click(object sender, EventArgs e)
        {
            var a = (HtmlAnchor)sender;

            var SOInfo = Ziri.BLL.OMS.SalesOrder.SetSend(long.Parse(a.Attributes["soid"]), true, out string Message);
            if (Message != null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "ConfirmMessage"
                   , string.Format("<script> swal('{0}', '', '{1}'); </script>", Message, AlertType.error));
                return;
            }
            ListBind();
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "ConfirmMessage"
                , string.Format("<script> swal('{0}', '', '{1}'); </script>", "订单[" + SOInfo.ID + "]发货完成！", AlertType.success));
        }

        //列表撤消发货按钮
        public void ListUnsend_Click(object sender, EventArgs e)
        {
            var a = (HtmlAnchor)sender;

            var SOInfo = Ziri.BLL.OMS.SalesOrder.SetSend(long.Parse(a.Attributes["soid"]), false, out string Message);
            if (Message != null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "ConfirmMessage"
                   , string.Format("<script> swal('{0}', '', '{1}'); </script>", Message, AlertType.error));
                return;
            }
            ListBind();
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "ConfirmMessage"
                , string.Format("<script> swal('{0}', '', '{1}'); </script>", "订单[" + SOInfo.ID + "]撤销发货完成！", AlertType.success));
        }

        //列表收银按钮
        public void ListPaid_Click(object sender, EventArgs e)
        {
            hidPayFormSOID.Value = null;
            txtPayFormBillNO.Text = null;
            txtReceiptsMoney.Text = 0.ToString("c");
            txtPayAmount.Text = txtPayChange.Text = 0.ToString("c");
            inpPayFormCode.Text = null;

            //是否已结算
            var a = (HtmlAnchor)sender;
            long SOID = 0;
            try { SOID = long.Parse(a.Attributes["soid"]); } catch { }
            var SOInfo = Ziri.BLL.OMS.SalesOrder.GetReceiptCheck(SOID, out string Message);
            if (Message != null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "PaidMessage"
                , string.Format("<script> swal('{0}', '', '{1}'); </script>", Message, AlertType.error));
                return;
            }

            //结算
            hidPayFormSOID.Value = SOInfo.SalesOrder.ID.ToString();
            txtPayFormBillNO.Text = SOInfo.SalesOrder.BillNO;
            txtReceiptsMoney.Text = SOInfo.SalesOrderItems.Sum(i => i.Amount).ToString("c");
            txtPayAmount.Text = txtPayChange.Text = 0.ToString("c");
            Page.ClientScript.RegisterStartupScript(Page.GetType()
                , "PayModalActive", "<script> document.getElementById('btnListPayModal').click();"
                    + " setTimeout(\"document.getElementById('inpPayFormCode').focus();\", 500); </script>");
            return;
        }

        //列表同意取消订单
        public void ListAgreedCancel_Click(object sender, EventArgs e)
        {
            var a = (HtmlAnchor)sender;

            var SOInfo = Ziri.BLL.OMS.SalesOrder.SetAgreedCancel(long.Parse(a.Attributes["soid"]), out string Message);
            if (Message != null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "ConfirmMessage"
                   , string.Format("<script> swal('{0}', '', '{1}'); </script>", Message, AlertType.error));
                return;
            }
            ListBind();
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "ConfirmMessage"
                , string.Format("<script> swal('{0}', '', '{1}'); </script>", "订单[" + SOInfo.BillNO + "]同意申请取消完成！", AlertType.success));
        }

        //收银扫码输入
        public void PayFormCode_Change(object sender, EventArgs e)
        {
            long SOID = 0;
            try { SOID = long.Parse(hidPayFormSOID.Value); } catch { }
            var SOInfo = Ziri.BLL.OMS.SalesOrder.GetReceiptCheck(SOID, out string Message);
            if (Message != null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "PaidMessage"
                , string.Format("<script> swal('{0}', '', '{1}'); </script>", Message, AlertType.error));
                return;
            }
            string PayCode = inpPayFormCode.Text.Trim();
            if (string.IsNullOrWhiteSpace(PayCode)) { return; }

            int PayCodeFlag = 0;
            try { PayCodeFlag = int.Parse(PayCode.Substring(0, 2)); }
            catch
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "PaidMessage"
               , string.Format("<script> swal('{0}', '', '{1}'); </script>", "支付码格式错误", AlertType.error));
                return;
            }
            if (PayCodeFlag >= 10 && PayCodeFlag <= 15)
            {
                //微信
                var WxPayResult = Ziri.BLL.WeChat.MicroPay(SOInfo, PayCode, out Message);
                if (Message != null)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "PaidMessage"
                    , string.Format("<script> swal('{0}', '', '{1}'); </script>", Message, AlertType.error));
                    return;
                }
                var F2FPayItems = new List<F2FPayItem> {
                    new F2FPayItem
                    {
                        PayTypeID = Ziri.BLL.OMS.SalesOrder.PayType.FirstOrDefault(i => i.Name == "WxPay").ID,
                        Amount = decimal.Parse(WxPayResult.GetValue("total_fee").ToString())*100,
                        TransactionID = WxPayResult.GetValue("transaction_id").ToString(),
                    }
                };
                Balance(SOInfo, F2FPayItems);
            }
            else if (PayCodeFlag >= 25 && PayCodeFlag <= 30)
            {
                //支付宝
                var AliPayResult = new Ziri.BLL.AliPay().F2FPay(SOInfo, PayCode, out Message);
                if (Message != null)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "PaidMessage"
                    , string.Format("<script> swal('{0}', '', '{1}'); </script>", Message, AlertType.error));
                    return;
                }
                var F2FPayItems = new List<F2FPayItem> {
                    new F2FPayItem
                    {
                        PayTypeID = Ziri.BLL.OMS.SalesOrder.PayType.FirstOrDefault(i => i.Name == "AliPay").ID,
                        Amount = decimal.Parse(AliPayResult["alipay_trade_pay_response"]["buyer_pay_amount"].ToString()),
                        TransactionID = AliPayResult["alipay_trade_pay_response"]["trade_no"].ToString(),
                    }
                };
                Balance(SOInfo, F2FPayItems);
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "PaidMessage"
              , string.Format("<script> swal('{0}', '', '{1}'); </script>", "只支持微信、支付宝付款码", AlertType.error));
                return;
            }
        }

        //收银确认按钮
        public void btnBalance_Click(object sender, EventArgs e)
        {
            long SOID = 0;
            try { SOID = long.Parse(hidPayFormSOID.Value); } catch { }
            var SOInfo = Ziri.BLL.OMS.SalesOrder.GetReceiptCheck(SOID, out string Message);
            if (Message != null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "PaidMessage"
                , string.Format("<script> swal('{0}', '', '{1}'); </script>", Message, AlertType.error));
                return;
            }
            //计算实收
            var PayFormCash = 0M;
            try { PayFormCash = decimal.Parse(inpPayFormCash.Text.Trim()); } catch { }
            var PayFormCard = 0M;
            try { PayFormCard = decimal.Parse(inpPayFormCard.Text.Trim()); } catch { }
            var PayAmount = PayFormCash + PayFormCard;
            var ReceiptAmount = SOInfo.SalesOrderItems.Sum(i => i.Amount);
            if (PayAmount < ReceiptAmount)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "PaidMessage"
                , string.Format("<script> swal('{0}', '', '{1}'); document.getElementById('btnListPayModal').click(); </script>"
                , "实收金额" + PayAmount + "不能小于应收金额" + ReceiptAmount, AlertType.error));
                return;
            }
            var F2FPayItems = new List<F2FPayItem>();
            if (PayFormCash > 0)
            {
                F2FPayItems.Add(new F2FPayItem
                {
                    PayTypeID = Ziri.BLL.OMS.SalesOrder.PayType.FirstOrDefault(i => i.Name == "Cash").ID,
                    Amount = PayFormCash,
                });
            }
            if (PayFormCard > 0)
            {
                F2FPayItems.Add(new F2FPayItem
                {
                    PayTypeID = Ziri.BLL.OMS.SalesOrder.PayType.FirstOrDefault(i => i.Name == "Card").ID,
                    Amount = PayFormCard,
                });
            }
            Balance(SOInfo, F2FPayItems);
        }

        //收银完成
        private void Balance(SOInfo SOInfo, List<F2FPayItem> F2FPayItems)
        {
            var F2FPayInfo = new F2FPayInfo
            {
                F2FPay = new F2FPay { Remark = string.IsNullOrWhiteSpace(inpPayFormRemark.Text.Trim()) ? null : inpPayFormRemark.Text.Trim(), },
                F2FPayItems = F2FPayItems,
            };
            F2FPayInfo = Ziri.BLL.Pay.SetF2FPay(F2FPayInfo, out string Message);
            if (Message != null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "PaidMessage"
                , string.Format("<script> swal('{0}', '', '{1}'); document.getElementById('btnListPayModal').click(); </script>", Message, AlertType.error));
                return;
            }

            //设置订单支付状态
            Ziri.BLL.OMS.SalesOrder.SetPayInfo(SOInfo.SalesOrder.ID, new SOPayInfo { PayID = F2FPayInfo.F2FPay.ID, }, true, out Message);
            if (Message != null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "PaidMessage"
                , string.Format("<script> swal('{0}', '', '{1}'); document.getElementById('btnListPayModal').click(); </script>", Message, AlertType.error));
                return;
            }

            //刷新列表
            ListBind();
        }

        //收银免单按钮
        public void btnFree_Click(object sender, EventArgs e)
        {
            //应收==0时免单，当前无此业务场景
        }

        //列表取消按钮
        public void ListCancel_Click(object sender, EventArgs e)
        {
            var a = (HtmlAnchor)sender;

            var SOInfo = Ziri.BLL.OMS.SalesOrder.SetCancel(long.Parse(a.Attributes["soid"]), out string Message);
            if (Message != null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "ConfirmMessage"
                   , string.Format("<script> swal('{0}', '', '{1}'); </script>", Message, AlertType.error));
                return;
            }
            ListBind();
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "ConfirmMessage"
                , string.Format("<script> swal('{0}', '', '{1}'); </script>", "订单[" + SOInfo.BillNO + "]取消完成！", AlertType.success));
        }

        //信息表单填充
        private void InfoFormFill(long soid)
        {
            txtInfoFormTitle.Text = soid == 0 ? "新订单" : soid.ToString();
            hidInfoFormSOID.Value = soid.ToString();

            inpInfoFormBillNO.Text = null;
            inpInfoFormState.Text = null;
            inpInfoFormRemark.Text = null;
            inpInfoFormCreateTime.Text = null;
            inpInfoFormUpdateTime.Text = null;

            inpInfoFormCustomerType.Text = null;
            inpInfoFormCustomerNickName.Text = null;
            inpInfoFormCustomerGender.Text = null;
            imgInfoFormCustomerAvatar.ImageUrl = null;

            lvInfoFormGoodsList.DataSource = null;
            lvInfoFormGoodsList.DataBind();

            inpInfoFormReceiptType.Text = null;
            inpInfoFormReceiptConsignee.Text = null;
            inpInfoFormReceiptPhone.Text = null;
            inpInfoFormReceiptStore.Text = null;
            inpInfoFormReceiptAddress.Text = null;

            inpInfoFormPayType.Text = null;
            inpInfoFormPayPreID.Text = null;
            inpInfoFormPayState.Text = null;
            inpInfoFormPayTransactionID.Text = null;
            inpDetailsF2FPayBillNO.Text = null;

            if (soid > 0)
            {
                var soInfo = Ziri.BLL.OMS.SalesOrder.GetSalesOrder(soid, out string Message);
                if (Message != null)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "InfoFormMessage"
                        , string.Format("<script> swal('{0}', '', '{1}'); </script>", Message, AlertType.error));
                    return;
                }

                inpInfoFormBillNO.Text = soInfo.SalesOrder.BillNO;
                inpInfoFormState.Text = soInfo.SOStateTitle;
                inpInfoFormRemark.Text = soInfo.SalesOrder.Remark;
                inpInfoFormCreateTime.Text = soInfo.SalesOrder.CreateTime.ToString("yyyy/MM/dd HH:mm:ss");
                if (soInfo.SalesOrder.UpdateTime != null)
                {
                    inpInfoFormUpdateTime.Text = (soInfo.SalesOrder.UpdateTime ?? DateTime.MinValue).ToString("yyyy/MM/dd HH:mm:ss");
                }

                inpInfoFormCustomerType.Text = soInfo.CustomerTypeTitle;
                inpInfoFormCustomerNickName.Text = soInfo.CustomerNickName;
                inpInfoFormCustomerGender.Text = soInfo.CustomerGender;
                imgInfoFormCustomerAvatar.ImageUrl = soInfo.CustomerAvatar;

                lvInfoFormGoodsList.DataSource = soInfo.SOItems;
                lvInfoFormGoodsList.DataBind();

                inpInfoFormReceiptType.Text = soInfo.ReceiptTypeTitle;
                inpInfoFormReceiptConsignee.Text = soInfo.SOReceiveInfo.Consignee;
                inpInfoFormReceiptPhone.Text = soInfo.SOReceiveInfo.Phone;
                inpInfoFormReceiptStore.Text = soInfo.ReceiptStore.Title;
                inpInfoFormReceiptAddress.Text = soInfo.SOReceiveInfo.Address;

                inpInfoFormPayType.Text = soInfo.PayTypeTitle;
                inpInfoFormPayPreID.Text = soInfo.SOPayInfo.PrePayID;
                inpInfoFormPayState.Text = soInfo.PayStateTitle;
                inpInfoFormPayTransactionID.Text = soInfo.SOPayInfo.TransactionID;
                if (soInfo.F2FPayInfo != null) { inpDetailsF2FPayBillNO.Text = soInfo.F2FPayInfo.F2FPay.BillNO; }
            }

            Page.ClientScript.RegisterStartupScript(Page.GetType(), "InfoFormActive", "<script> document.getElementById('btnListInfoFormModal').click(); </script>");
        }

        //列表详情按钮
        public void ListDetails_Click(object sender, EventArgs e)
        {
            var a = (HtmlAnchor)sender;
            long soid = long.Parse(a.Attributes["soid"]);

            txtDefailsModalTitle.Text = null;

            txtDetailsBillNO.Text = null;
            txtDetailsState.Text = null;
            txtDetailsRemark.Text = null;
            txtDetailsCreateTime.Text = null;
            txtDetailsUpdateTime.Text = null;

            txtDetailsCustomerType.Text = null;
            txtDetailsCustomerNickName.Text = null;
            txtDetailsCustomerGender.Text = null;
            imgDetailsCustomerAvatar.ImageUrl = null;

            lvDetailsSOItems.DataSource = null;
            lvDetailsSOItems.DataBind();

            txtDetailsReceiptType.Text = null;
            txtDetailsReceiptConsignee.Text = null;
            txtDetailsReceiptPhone.Text = null;
            txtDetailsReceiptStore.Text = null;
            txtDetailsReceiptAddress.Text = null;

            txtDetailsPayType.Text = null;
            txtDetailsPayPreID.Text = null;
            txtDetailsPayState.Text = null;
            txtDetailsPayTransactionID.Text = null;

            if (soid > 0)
            {
                var soInfo = Ziri.BLL.OMS.SalesOrder.GetSalesOrder(soid, out string Message);
                if (Message != null)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "InfoFormMessage"
                        , string.Format("<script> swal('{0}', '', '{1}'); </script>", Message, AlertType.error));
                    return;
                }
                txtDefailsModalTitle.Text = soInfo.SalesOrder.BillNO;

                txtDetailsBillNO.Text = soInfo.SalesOrder.BillNO;
                txtDetailsState.Text = soInfo.SOStateTitle;
                txtDetailsRemark.Text = soInfo.SalesOrder.Remark;
                txtDetailsCreateTime.Text = soInfo.SalesOrder.CreateTime.ToString("yyyy/MM/dd HH:mm:ss");
                if (soInfo.SalesOrder.UpdateTime != null)
                {
                    txtDetailsUpdateTime.Text = (soInfo.SalesOrder.UpdateTime ?? DateTime.MinValue).ToString("yyyy/MM/dd HH:mm:ss");
                }

                txtDetailsCustomerType.Text = soInfo.CustomerTypeTitle;
                txtDetailsCustomerNickName.Text = soInfo.CustomerNickName;
                txtDetailsCustomerGender.Text = soInfo.CustomerGender;
                imgDetailsCustomerAvatar.ImageUrl = soInfo.CustomerAvatar;

                lvDetailsSOItems.DataSource = soInfo.SOItems;
                lvDetailsSOItems.DataBind();

                txtDetailsReceiptType.Text = soInfo.ReceiptTypeTitle;
                txtDetailsReceiptConsignee.Text = soInfo.SOReceiveInfo.Consignee;
                txtDetailsReceiptPhone.Text = soInfo.SOReceiveInfo.Phone;
                txtDetailsReceiptStore.Text = soInfo.ReceiptStore.Title;
                txtDetailsReceiptAddress.Text = soInfo.SOReceiveInfo.Address;

                txtDetailsPayType.Text = soInfo.PayTypeTitle;
                txtDetailsPayPreID.Text = soInfo.SOPayInfo.PrePayID;
                txtDetailsPayState.Text = soInfo.PayStateTitle;
                txtDetailsPayTransactionID.Text = soInfo.SOPayInfo.TransactionID;
                if (soInfo.F2FPayInfo != null) { txtDetailsF2FPayBillNO.Text = soInfo.F2FPayInfo.F2FPay.BillNO; }
            }

            Page.ClientScript.RegisterStartupScript(Page.GetType(), "DetailsModalActive", "<script> document.getElementById('btnListDetailsModal').click(); </script>");
        }
    }
}