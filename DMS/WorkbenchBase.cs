using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Ziri.MDL;

namespace DMS
{
    public class WorkbenchBase : Page
    {
        public LoginInfo LoginInfo { get; private set; } = null;

        //加载
        protected override void OnLoad(EventArgs e)
        {
            LoginInfo = Ziri.BLL.Login.GetLoginInfo();
            if (LoginInfo == null)
            {
                Session["RequestURL"] = Request.Url.PathAndQuery;
                Response.Redirect("/");
            }
            base.OnLoad(e);
        }

        //获取排序字段
        public void GetOrderByField(ListView ListView, string FieldName, List<ListOrderField> OrderByFields, out List<ListOrderField> orderByFields)
        {
            orderByFields = OrderByFields;
            var OrderByButton = (HtmlButton)ListView.FindControl(FieldName);
            if (OrderByButton == null) { return; };
            var OrderByValue = (HiddenField)OrderByButton.FindControl(OrderByButton.ID + "Value");
            OrderByMode OrderByMode = OrderByMode.None;
            if (!string.IsNullOrWhiteSpace(OrderByValue.Value)) { OrderByMode = (OrderByMode)Enum.Parse(typeof(OrderByMode), OrderByValue.Value); }
            if (OrderByMode != OrderByMode.None)
            {
                orderByFields.Add(new ListOrderField
                {
                    Name = FieldName.Replace("ListOrderBy", ""),
                    Mode = OrderByMode
                });
            }
        }

        //设置排序标记
        public void SetOrderByFlag(object sender)
        {
            var titleButton = (HtmlButton)sender;
            var valueButton = (HiddenField)titleButton.FindControl(titleButton.ID + "Value");
            OrderByMode OrderByMode = OrderByMode.None;
            if (!string.IsNullOrWhiteSpace(valueButton.Value)) { OrderByMode = (OrderByMode)Enum.Parse(typeof(OrderByMode), valueButton.Value); }
            OrderByMode = OrderByMode == OrderByMode.Asc ? OrderByMode.None : OrderByMode == OrderByMode.None ? OrderByMode.Desc : OrderByMode.Asc;
            ((HtmlGenericControl)titleButton.FindControl(titleButton.ID + "Desc")).Attributes["class"] = OrderByMode == OrderByMode.Desc ? "list-orderby-active" : "list-orderby-none";
            ((HtmlGenericControl)titleButton.FindControl(titleButton.ID + "Asc")).Attributes["class"] = OrderByMode == OrderByMode.Asc ? "list-orderby-active" : "list-orderby-none";
            valueButton.Value = OrderByMode.ToString();
        }

        //设置筛选标记
        public string FilterFormat(TextBox inpFilter, string FieldText)
        {
            if (string.IsNullOrWhiteSpace(inpFilter.Text.Trim())) { return FieldText; }

            FieldText = FieldText.ToLower();
            var filters = inpFilter.Text.Split(' ');
            foreach (var keyword in filters)
            {
                FieldText = FieldText.Replace(keyword, string.Format("<span class=\"ListFilterMark\">{0}</span>", keyword));
            }
            return FieldText;
        }
    }
}