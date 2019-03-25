using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DMS.UC
{
    public partial class PagerControl : UserControl
    {
        //页面加载
        protected void Page_Load(object sender, EventArgs e)
        {
            for (int i = 1; i <= NumberCount; i++)
            {
                Button btnPageNumber = new Button { ID = "btnPageNumber" + (i == 1 ? "First" : i == NumberCount ? "Last" : i.ToString()) };
                btnPageNumber.CssClass = "PageNumber";
                btnPageNumber.Click += PagerChange;
                divPageNumbers.Controls.Add(btnPageNumber);
            }
            PageNumbers();
            divPagerControl.Visible = !(RowCount == 0);
        }

        //页码切换
        protected void PagerChange(object sender, EventArgs e)
        {
            if (sender.GetType() == typeof(Button))
            {
                Button btnSender = ((Button)sender);
                if (btnSender.ID.Contains("btnPageNumber"))
                {
                    PageIndex = int.Parse(btnSender.ID.Contains("First") || btnSender.ID.Contains("Last") ? btnSender.ToolTip : btnSender.Text);
                }
                else
                {
                    switch (btnSender.ID)
                    {
                        case "btnPageFirst": PageIndex = 1; break;
                        case "btnPagePrevious": PageIndex -= 1; break;
                        case "btnPageNext": PageIndex += 1; break;
                        case "btnPageLast": PageIndex = PageCount; break;
                    }
                }
            }

            Page page = this.Page;
            page.GetType().GetMethod("PagerChange").Invoke(page, new object[] { sender, e });
        }

        //总行数
        public long RowCount
        {
            get { return long.Parse(txtRowCount.Text); }
            set
            {
                txtRowCount.Text = value.ToString();
                PageCount = (int)Math.Ceiling((double)value / PageSize);
                if (PageIndex < 1) { PageIndex = 1; }
                if (PageIndex > PageCount) { PageIndex = PageCount; }
                PageNumbers();
                divPagerControl.Visible = !(RowCount == 0);
            }
        }

        //页大小
        public int PageSize
        {
            get { return int.Parse(txtPageSize.SelectedValue); }
            set { if (txtPageSize.Items.FindByValue(value.ToString()) != null) { txtPageSize.SelectedValue = value.ToString(); } }
        }

        //当前页
        public int PageIndex
        {
            get { return int.Parse(txtPageIndex.Text); }
            set { txtPageIndex.Text = (value < 1 ? 1 : value).ToString(); }
        }

        //总页数
        public int PageCount
        {
            get { return int.Parse(txtPageCount.Text); }
            set { txtPageCount.Text = value.ToString(); }
        }

        //页码数
        public int NumberCount
        {
            get { return int.Parse(txtPageNumberCount.Value); }
            set { txtPageNumberCount.Value = value.ToString(); }
        }

        //页面按钮
        private void PageNumbers()
        {
            //前后按钮
            btnPageFirst.Enabled = true;
            btnPagePrevious.Enabled = true;
            btnPageNext.Enabled = true;
            btnPageLast.Enabled = true;
            if (PageIndex == 1)
            {
                btnPageFirst.Enabled = false;
                btnPagePrevious.Enabled = false;
            }
            if (PageIndex == PageCount)
            {
                btnPageNext.Enabled = false;
                btnPageLast.Enabled = false;
            }

            //数字按钮
            int PageIndexRange = NumberCount - 2;
            int PageIndexEnd = PageIndex % PageIndexRange == 0 ? PageIndex : ((int)((double)PageIndex / PageIndexRange + 0.01) * PageIndexRange) + PageIndexRange;
            int PageIndexStart = PageIndexEnd - PageIndexRange + 1;
            if (PageIndexStart < 0) { PageIndexStart = 0; }

            if (divPageNumbers.Controls.Count > 0)
            {
                // - 前页码
                Button btnPageNumberFirst = (Button)divPageNumbers.Controls[0];
                if (PageIndex <= PageIndexRange) { btnPageNumberFirst.Visible = false; }
                else
                {
                    btnPageNumberFirst.Visible = true;
                    btnPageNumberFirst.Text = "1..." + (PageIndexStart - 1);
                    btnPageNumberFirst.ToolTip = (PageIndexStart - 1).ToString();
                }

                // - 后页码
                Button btnPageNumberLast = (Button)divPageNumbers.Controls[NumberCount - 1];
                if (PageIndexEnd >= PageCount) { btnPageNumberLast.Visible = false; }
                else
                {
                    btnPageNumberLast.Visible = true;
                    btnPageNumberLast.Text = ((PageIndexEnd + 1) == PageCount ? null : (PageIndexEnd + 1) + "...") + PageCount;
                    btnPageNumberLast.ToolTip = (PageIndexEnd + 1).ToString();
                }

                // - 中页码
                if (PageIndexEnd > PageCount) { PageIndexEnd = PageCount; }
                int btnCount = 1;
                for (int i = PageIndexStart; i <= PageIndexEnd; i++)
                {
                    Button btnPageNumber = (Button)divPageNumbers.Controls[btnCount];
                    btnPageNumber.Text = i.ToString();
                    btnPageNumber.Visible = true;
                    btnPageNumber.Enabled = i == PageIndex ? false : true;
                    btnCount++;
                }
                for (int i = btnCount; i <= PageIndexRange; i++) { ((Button)divPageNumbers.Controls[i]).Visible = false; }
            }
        }
    }
}