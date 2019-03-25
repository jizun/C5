using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Ziri.MDL;

namespace DMS.MOD.ITEM
{
    public partial class SpecInfo : WorkbenchBase
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
            Ziri.BLL.ITEM.Spec.InitSpecInfo(out AlertMessage alertMessage);
            if (alertMessage == null) { ListBind(); }
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "InitMessage", alertMessage == null
                ? string.Format("<script> swal('{0}', '', '{1}'); </script>", "模块初始化完成", AlertType.success)
                : string.Format("<script> swal('{0}', '', '{1}'); </script>", alertMessage.Message, alertMessage.Type));
        }

        //信息表单保存按钮
        public void InfoFormSubmit_Click(object sender, EventArgs e)
        {
            var specInfo = new Ziri.MDL.SpecInfo
            {
                ID = long.Parse(hidInfoFormSpecID.Value),
                Name = inpInfoFormSpecName.Text,
                Title = inpInfoFormSpecTitle.Text,
                IconFont = inpInfoFormIconFont.Text,
            };

            specInfo = Ziri.BLL.ITEM.Spec.SpecInfoUpload(specInfo, GetSpecValues(), out AlertMessage alertMessage);
            if (alertMessage == null) { ListBind(); }
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "InfoFormMessage", alertMessage == null
                ? string.Format("<script> swal('保存完成，规格编号[{0}]。', '', '{1}'); </script>", specInfo.ID, AlertType.success)
                : string.Format("<script> document.getElementById('btnListInfoFormModal').click(); swal('{0}', '', '{1}'); </script>", alertMessage.Message, alertMessage.Type));
        }

        //信息列表修改按钮
        public void ListEdit_Click(object sender, EventArgs e)
        {
            var a = (HtmlAnchor)sender;
            InfoFormFill(long.Parse(a.Attributes["specid"]));
        }

        //获取信息表单参数列表
        private List<string> GetSpecValues()
        {
            var sepcValues = new List<string>();
            var specCount = 0;
            while (true)
            {
                var specValue = Request["demo1[" + specCount + "][inpInfoFormSpecValue]"];
                if (specValue == null) { break; }
                if (!string.IsNullOrWhiteSpace(specValue.ToString())) { sepcValues.Add(specValue.ToString()); }
                specCount++;
            }
            return sepcValues;
        }

        //列表绑定
        private void ListBind()
        {
            //规格列表
            lvSpecInfo.DataSource = Ziri.BLL.ITEM.Spec.GetSpecInfos(out AlertMessage alertMessage);
            lvSpecInfo.DataBind();
            foreach (var item in lvSpecInfo.Items)
            {
                var lvSpecValue = (ListView)item.FindControl("lvSpecValue");
                lvSpecValue.DataSource = Ziri.BLL.ITEM.Spec.GetSpecValues(long.Parse(((HiddenField)item.FindControl("lvSpecID")).Value), out alertMessage);
                lvSpecValue.DataBind();
            }

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
            long SpecID = long.Parse(a.Attributes["specid"]);

            Ziri.BLL.ITEM.Spec.SetSpecEnabled(SpecID, a.ID == "btnListEnabled", out AlertMessage alertMessage);
            ListBind();
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "EnabledMessage",
                string.Format("<script> swal('{0}', '', '{1}'); </script>", alertMessage.Message, alertMessage.Type));
        }

        //列表参数操作启用、禁用时
        public void ListValueEnabled_Click(object sender, EventArgs e)
        {
            var a = (HtmlAnchor)sender;
            long ValueID = long.Parse(a.Attributes["valueid"]);

            Ziri.BLL.ITEM.Spec.SetValueEnabled(ValueID, a.ID == "btnListEnabled", out AlertMessage alertMessage);
            ListBind();
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "EnabledMessage",
                string.Format("<script> swal('{0}', '', '{1}'); </script>", alertMessage.Message, alertMessage.Type));
        }

        //信息表单填充
        private void InfoFormFill(long SpecID)
        {
            txtInfoFormTitle.Text = SpecID == 0 ? "新规格" : SpecID.ToString();
            hidInfoFormSpecID.Value = SpecID.ToString();
            inpInfoFormIconFont.Text = "flaticon2-gift";
            inpInfoFormSpecName.Text = null;
            inpInfoFormSpecTitle.Text = null;

            List<SpecValue> specValues = null;
            if (SpecID > 0)
            {
                //规格
                var specInfo = Ziri.BLL.ITEM.Spec.GetModifySpecInfo(SpecID, out AlertMessage alertMessage);
                if (alertMessage != null)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "InfoFormMessage"
                        , string.Format("<script> swal('{0}', '', '{1}'); </script>", alertMessage.Message, alertMessage.Type));
                    return;
                }
                if (specInfo == null)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "InfoFormMessage"
                    , string.Format("<script> swal('{0}', '', '{1}'); </script>", "规格编号[" + SpecID + "]不存在", AlertType.error));
                    return;
                }
                txtInfoFormTitle.Text = specInfo.Title;
                inpInfoFormIconFont.Text = specInfo.IconFont;
                inpInfoFormSpecName.Text = specInfo.Name;
                inpInfoFormSpecTitle.Text = specInfo.Title;

                //参数
                specValues = Ziri.BLL.ITEM.Spec.GetModifySpecValues(SpecID, out alertMessage);
            }
            if (specValues == null || specValues.Count == 0) { specValues = new List<SpecValue> { new SpecValue { Value = null } }; }
            lvInfoFormSpecValue.DataSource = specValues;
            lvInfoFormSpecValue.DataBind();

            Page.ClientScript.RegisterStartupScript(Page.GetType(), "InfoFormActive"
                , "<script> document.getElementById('btnListInfoFormModal').click(); </script>");
        }
    }
}