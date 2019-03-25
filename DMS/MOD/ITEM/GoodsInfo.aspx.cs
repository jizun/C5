using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using Newtonsoft.Json;
using Ziri.MDL;

namespace DMS.MOD.ITEM
{
    public partial class GoodsInfo : WorkbenchBase
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

        //初始化按钮
        public void GoodsInit_Click(object sender, EventArgs e)
        {
            Ziri.BLL.ITEM.Goods.InitGoodsInfo(out AlertMessage alertMessage);
            if (alertMessage != null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "InitMessage"
                        , string.Format("<script> swal('{0}', '', '{1}'); </script>", alertMessage.Message, alertMessage.Type));
                return;
            }

            ListBind();
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "InitMessage"
                , string.Format("<script> swal('{0}', '', '{1}'); </script>", "模块初始化完成", AlertType.success));
        }

        //新增按钮
        public void AddNew_Click(object sender, EventArgs e)
        {
            InfoFormFill(0);
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

        //列表修改按钮
        public void ListEdit_Click(object sender, EventArgs e)
        {
            var a = (HtmlAnchor)sender;
            InfoFormFill(long.Parse(a.Attributes["goodsid"]));
        }

        //信息表单保存按钮
        public void InfoFormSubmit_Click(object sender, EventArgs e)
        {
            //品名
            var goodsInfo = new Ziri.MDL.GoodsInfo
            {
                ID = long.Parse(hidInfoFormGoodsID.Value),
                Name = inpInfoFormGoodsName.Text,
                Title = inpInfoFormGoodsTitle.Text,
            };
            //品牌
            GoodsBrand goodsBrand = null;
            if (!string.IsNullOrWhiteSpace(hidInfoFormBrandID.Value))
            {
                goodsBrand = new GoodsBrand
                {
                    BrandID = long.Parse(hidInfoFormBrandID.Value),
                };
            }
            //品类
            List<GoodsKind> goodsKinds = new List<GoodsKind>();
            long kindRootID = long.Parse(drpInfoFormKindRoot.SelectedValue);
            long kindL1ID = 0;
            try { kindL1ID = long.Parse(drpInfoFormKindL1.SelectedValue); } catch { }
            if (kindRootID > 0) { goodsKinds.Add(new GoodsKind { KindLevel = 0, KindID = kindRootID }); }
            if (kindL1ID > 0) { goodsKinds.Add(new GoodsKind { KindLevel = 1, KindID = kindL1ID }); }
            //图文
            var PhotoUploadInfos = JsonConvert.DeserializeObject<List<FileUploadInfo>>(hidInfoFormGoodsPhoto.Value);
            var goodsPhoto = new GoodsPhoto
            {
                FileIDs = string.Join(",", PhotoUploadInfos.Select(i => i.FileInfo.ID)),
            };
            var goodsDesc = new GoodsDesc
            {
                Description = HttpUtility.UrlDecode(hidInfoFormGoodsDesc.Value)
            };
            //规格
            var goodsSpecsFull = new List<GoodsSpecFull>();
            foreach (var specInfo in lvInfoFormSpec.Items)
            {
                var valueIDs = (HiddenField)specInfo.FindControl("hidValueIDs");
                if (!string.IsNullOrWhiteSpace(valueIDs.Value))
                {
                    var inpSpecPrice = (TextBox)specInfo.FindControl("inpSpecPrice");
                    var inpSpecQty = (TextBox)specInfo.FindControl("inpSpecQty");
                    goodsSpecsFull.Add(new GoodsSpecFull
                    {
                        SpecValueIDs = valueIDs.Value,
                        SpecValues = ((HiddenField)specInfo.FindControl("hidValues")).Value,
                        Enabled = ((HtmlInputCheckBox)specInfo.FindControl("inpSpecEnabled")).Checked,
                        SKU = ((TextBox)specInfo.FindControl("inpInfoFormSKU")).Text,
                        UPC = ((TextBox)specInfo.FindControl("inpInfoFormUPC")).Text,
                        EAN = ((TextBox)specInfo.FindControl("inpInfoFormEAN")).Text,
                        JAN = ((TextBox)specInfo.FindControl("inpInfoFormJAN")).Text,
                        ISBN = ((TextBox)specInfo.FindControl("inpInfoFormISBN")).Text,
                        Price = string.IsNullOrWhiteSpace(inpSpecPrice.Text) ? 0 : decimal.Parse(inpSpecPrice.Text),
                        Quantity = string.IsNullOrWhiteSpace(inpSpecQty.Text) ? 0 : decimal.Parse(inpSpecQty.Text),
                    });
                }
            }
            //单价
            var goodsCounter = new GoodsCounter
            {
                SKU = inpInfoFormSKU.Text,
                UPC = inpInfoFormUPC.Text,
                EAN = inpInfoFormEAN.Text,
                JAN = inpInfoFormJAN.Text,
                ISBN = inpInfoFormISBN.Text,
                Price = decimal.Parse(inpInfoFormPrice.Text),
                Quantity = decimal.Parse(inpInfoFormQuantity.Text),
            };
            //保存
            goodsInfo = Ziri.BLL.ITEM.Goods.GoodsInfoUpload(goodsInfo, goodsBrand, goodsKinds, GetGoodsTags(), goodsPhoto, goodsDesc, goodsSpecsFull, goodsCounter, out AlertMessage alertMessage);
            if (alertMessage == null) { ListBind(); }
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "InfoFormMessage", alertMessage == null
                ? string.Format("<script> swal('保存完成，商品编号[{0}]。', '', '{1}'); </script>", goodsInfo.ID, AlertType.success)
                : string.Format("<script> document.getElementById('btnListInfoFormModal').click(); swal('{0}', '', '{1}'); </script>", alertMessage.Message, alertMessage.Type));
        }

        //获取信息表单标签列表
        private List<GoodsTag> GetGoodsTags()
        {
            var goodsTags = new List<GoodsTag>();
            var TagCount = 0;
            while (true)
            {
                var goodsValue = Request["demo1[" + TagCount + "][inpInfoFormTag]"];
                if (goodsValue == null) { break; }
                if (!string.IsNullOrWhiteSpace(goodsValue.ToString())) { goodsTags.Add(new GoodsTag { Title = goodsValue }); }
                TagCount++;
            }
            return goodsTags;
        }

        //列表详情按钮
        public void ListDetails_Click(object sender, EventArgs e)
        {
            var a = (HtmlAnchor)sender;
            long GoodsID = long.Parse(a.Attributes["goodsid"]);

            txtDetailsModalTitle.Text = null;
            txtDetailsName.InnerText = null;
            txtDetailsTitle.InnerText = null;

            txtDetailsAmount.InnerText = 0.00D.ToString("c");
            inpDetailsQty.Text = 1.ToString();

            divDetailsBrand.Visible = false;
            divDetailsBrandLogo.Attributes["style"] = null;
            divDetailsBrandTitle.InnerText = null;
            divDetailsBrandName.InnerText = null;

            divDetailsSpec.Visible = false;

            divDetailsSinglePrice.Visible = false;
            txtDetailsSKU.Text = null;
            txtDetailsUPC.Text = null;
            txtDetailsEAN.Text = null;
            txtDetailsJAN.Text = null;
            txtDetailsISBN.Text = null;
            txtDetailsPrice.Text = null;
            txtDetailsQty.Text = null;

            divGoodsPhotos.Visible = false;
            txtDetailsDesc.InnerHtml = null;

            var goodsDetails = Ziri.BLL.ITEM.Goods.GetDetailsInfo(GoodsID, out AlertMessage alertMessage);
            if (alertMessage != null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "DetailsMessage"
                    , string.Format("<script> swal('{0}', '', '{1}'); </script>", alertMessage.Message, alertMessage.Type));
                return;
            }
            if (goodsDetails == null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "DefailsMessage"
                    , string.Format("<script> swal('{0}', '', '{1}'); </script>", "商品编号[" + GoodsID + "]不存在", AlertType.error));
                return;
            }
            //品名
            txtDetailsModalTitle.Text = goodsDetails.Title;
            txtDetailsName.InnerText = goodsDetails.Name;
            txtDetailsTitle.InnerText = goodsDetails.Title;
            //品牌
            if (goodsDetails.GoodsBrandID != null)
            {
                divDetailsBrand.Visible = true;
                divDetailsBrandLogo.Attributes["style"] = string.Format("background-image: url({0})", goodsDetails.BrandFullInfo.LogoFileGUID == null
                    ? "/media/logos/logo_template.png"
                    : string.Format("/DOC/upload/{0}{1}", goodsDetails.BrandFullInfo.LogoFileGUID, goodsDetails.BrandFullInfo.LogoFileExtName));
                divDetailsBrandTitle.InnerText = goodsDetails.BrandFullInfo.Title;
                divDetailsBrandName.InnerText = goodsDetails.BrandFullInfo.Name;
            }
            //品类
            lvDetailsKinds.DataSource = goodsDetails.KindInfos;
            lvDetailsKinds.DataBind();
            //标签
            lvDetailsTags.DataSource = goodsDetails.GoodsTags;
            lvDetailsTags.DataBind();
            //规格
            lvDetailsSpecs.DataSource = goodsDetails.SpecInfos;
            lvDetailsSpecs.DataBind();
            foreach (var item in lvDetailsSpecs.Items)
            {
                long detailsSpecID = long.Parse(((HiddenField)item.FindControl("lvDetailsSpecID")).Value);
                var lvDetailsSpecValues = (ListView)item.FindControl("lvDetailsSpecValues");
                lvDetailsSpecValues.DataSource = goodsDetails.SpecValues.Where(i => i.SpecID == detailsSpecID).ToList();
                lvDetailsSpecValues.DataBind();
                ((Label)lvDetailsSpecValues.Items[0].FindControl("txtDetailsSpecValue")).Attributes["selected"] = "true";
            }
            hidDetailsGoodsSpecs.Value = JsonConvert.SerializeObject(goodsDetails.GoodsSpecsFull);
            lvDetailsGoodsSpecs.DataSource = goodsDetails.GoodsSpecsFull;
            lvDetailsGoodsSpecs.DataBind();
            if (goodsDetails.GoodsSpecsFull.Count > 0)
            {
                divDetailsSpec.Visible = true;
                txtDetailsAmount.InnerText = (goodsDetails.GoodsSpecsFull[0].Price ?? 0).ToString("c");
            }
            //单价
            if (goodsDetails.GoodsSpecsFull.Count == 0)
            {
                divDetailsSinglePrice.Visible = true;
                if (goodsDetails.GoodsCounter != null)
                {
                    txtDetailsSKU.Text = goodsDetails.GoodsCounter.SKU;
                    txtDetailsUPC.Text = goodsDetails.GoodsCounter.UPC;
                    txtDetailsEAN.Text = goodsDetails.GoodsCounter.EAN;
                    txtDetailsJAN.Text = goodsDetails.GoodsCounter.JAN;
                    txtDetailsISBN.Text = goodsDetails.GoodsCounter.ISBN;
                    txtDetailsPrice.Text = txtDetailsAmount.InnerText = goodsDetails.GoodsCounter.Price.ToString("c");
                    txtDetailsQty.Text = goodsDetails.GoodsCounter.Quantity.ToString();
                }
            }
            //图文
            if (goodsDetails.PhotoUploadInfos != null && goodsDetails.PhotoUploadInfos.Count > 0)
            {
                divGoodsPhotos.Visible = true;
                lvGoodsPhoto1.DataSource = lvGoodsPhoto2.DataSource = goodsDetails.PhotoUploadInfos.Select(i => "/DOC/upload/" + i.FileInfo.GUID + i.FileExtName.Name).ToList();
                lvGoodsPhoto1.DataBind();
                lvGoodsPhoto2.DataBind();
            }
            txtDetailsDesc.InnerHtml = goodsDetails.Description;
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "DetailsModalActive", "<script> document.getElementById('btnListDetailsModal').click(); </script>");
        }

        //列表状态按钮
        public void ListEnabled_Change(object sender, EventArgs e)
        {
            var checkbox = (HtmlInputCheckBox)sender;
            Ziri.BLL.ITEM.Goods.SetGoodsEnabled(long.Parse(checkbox.Attributes["goodsid"]), checkbox.Checked, out AlertMessage alertMessage);
            if (alertMessage.Type == AlertType.success) { ListBind(); };
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "EnabledMessage"
                , string.Format("<script> swal('{0}', '', '{1}'); </script>", alertMessage.Message, alertMessage.Type));
        }

        //列表上、下架按钮
        public void ListSalesState_Click(object sender, EventArgs e)
        {
            var a = (HtmlAnchor)sender;
            Ziri.BLL.ITEM.Goods.SetPublicSales(long.Parse(a.Attributes["goodsid"]), a.ID == "btnListPublicSales"
                ? GoodsState.上架 : GoodsState.下架, out AlertMessage alertMessage);
            if (alertMessage.Type == AlertType.success) { ListBind(); }
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "EnabledMessage"
                , string.Format("<script> swal('{0}', '', '{1}'); </script>", alertMessage.Message, alertMessage.Type));
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
            foreach (string item in new string[] { "ListOrderByID", "ListOrderByName", "ListOrderByTitle", "ListOrderByEnabled", "ListOrderByStateID" })
            {
                GetOrderByField(GoodsInfoList, item, OrderByFields, out OrderByFields);
            }

            //显示列表页
            var userInfos = Ziri.BLL.ITEM.Goods.GetGoodsInfos(FilterFields, OrderByFields
                , InfoListPager.PageSize, InfoListPager.PageIndex, out long rowCount, out AlertMessage alertMessage);
            GoodsInfoList.DataSource = userInfos;
            GoodsInfoList.DataBind();
            InfoListPager.RowCount = rowCount;

            //提示信息
            if (alertMessage != null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "ListMessage"
                , string.Format("<script> swal('{0}', '', '{1}'); </script>", alertMessage.Message, alertMessage.Type));
            }
        }

        //信息表单填充
        private void InfoFormFill(long GoodsID)
        {
            txtInfoFormTitle.Text = GoodsID == 0 ? "新商品" : GoodsID.ToString();
            hidInfoFormGoodsID.Value = GoodsID.ToString();
            inpInfoFormGoodsName.Text = null;
            inpInfoFormGoodsTitle.Text = null;

            divInfoFormBrand.Visible = false;
            hidInfoFormBrandID.Value = null;
            divInfoFormBrandLogo.Attributes["style"] = null;
            txtInfoFormBrandTitle.InnerText = null;
            txtInfoFormBrandName.InnerText = null;

            drpInfoFormKindRoot.DataValueField = "ID";
            drpInfoFormKindRoot.DataTextField = "Title";
            drpInfoFormKindRoot.DataSource = Ziri.BLL.ITEM.Kind.GetKindRootInfos(out AlertMessage alertMessage, true);
            drpInfoFormKindRoot.DataBind();
            drpInfoFormKindRoot.Items.Insert(0, new ListItem("-未选择-", "0"));
            drpInfoFormKindL1.Items.Clear();

            hidInfoFormGoodsPhoto.Value = "[]";
            hidInfoFormGoodsDesc.Value = null;

            lvInfoFormTags.DataSource = new List<GoodsTag> { new GoodsTag { Title = null } };
            lvInfoFormTags.DataBind();

            lvInfoFormSpec.DataSource = null;
            lvInfoFormSpec.DataBind();

            inpInfoFormSKU.Text = null;
            inpInfoFormUPC.Text = null;
            inpInfoFormEAN.Text = null;
            inpInfoFormJAN.Text = null;
            inpInfoFormISBN.Text = null;
            inpInfoFormPrice.Text = 0.00.ToString();
            inpInfoFormQuantity.Text = 0.ToString();

            if (GoodsID > 0)
            {
                var goodsDetails = Ziri.BLL.ITEM.Goods.GetModifyInfo(GoodsID, out alertMessage);
                if (alertMessage != null)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "InfoFormMessage"
                        , string.Format("<script> swal('{0}', '', '{1}'); </script>", alertMessage.Message, alertMessage.Type));
                    return;
                }
                if (goodsDetails == null)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "InfoFormMessage"
                        , string.Format("<script> swal('{0}', '', '{1}'); </script>", "商品编号[" + GoodsID + "]不存在", AlertType.error));
                    return;
                }
                txtInfoFormTitle.Text = goodsDetails.Name;
                inpInfoFormGoodsName.Text = goodsDetails.Name;
                inpInfoFormGoodsTitle.Text = goodsDetails.Title;
                //品牌
                if (goodsDetails.GoodsBrandID != null)
                {
                    divInfoFormBrand.Visible = true;
                    hidInfoFormBrandID.Value = goodsDetails.GoodsBrandID == 0 ? null : goodsDetails.GoodsBrandID.ToString();
                    divInfoFormBrandLogo.Attributes["style"] = string.Format("background-image: url({0})", goodsDetails.BrandFullInfo.LogoFileGUID == null
                        ? "/media/logos/logo_template.png"
                        : string.Format("/DOC/upload/{0}{1}", goodsDetails.BrandFullInfo.LogoFileGUID, goodsDetails.BrandFullInfo.LogoFileExtName));
                    txtInfoFormBrandTitle.InnerText = goodsDetails.BrandFullInfo.Title;
                    txtInfoFormBrandName.InnerText = goodsDetails.BrandFullInfo.Name;
                }
                //品类
                if (goodsDetails.GoodsKinds != null)
                {
                    var kindRoot = goodsDetails.GoodsKinds.Where(i => i.KindLevel == 0).FirstOrDefault();
                    if (kindRoot != null)
                    {
                        drpInfoFormKindRoot.SelectedValue = kindRoot.KindID.ToString();
                        InfoFormKindL1Bind(kindRoot.KindID);
                        var kindRootL1 = goodsDetails.GoodsKinds.Where(i => i.KindLevel == 1).FirstOrDefault();
                        if (kindRootL1 != null)
                        {
                            drpInfoFormKindL1.SelectedValue = kindRootL1.KindID.ToString();
                        }
                    }
                }
                //标签
                if (goodsDetails.GoodsTags.Count > 0)
                {
                    lvInfoFormTags.DataSource = goodsDetails.GoodsTags;
                    lvInfoFormTags.DataBind();
                }
                //图文
                hidInfoFormGoodsPhoto.Value = goodsDetails.PhotoUploadInfos.Count == 0 ? "[]" : JsonConvert.SerializeObject(goodsDetails.PhotoUploadInfos);
                hidInfoFormGoodsDesc.Value = goodsDetails.Description == null ? null : HttpUtility.UrlEncode(goodsDetails.Description).Replace("+", "%20");
                //规格
                lvInfoFormSpec.DataSource = goodsDetails.GoodsSpecsFull;
                lvInfoFormSpec.DataBind();
                //单价
                if (goodsDetails.GoodsCounter != null)
                {
                    inpInfoFormSKU.Text = goodsDetails.GoodsCounter.SKU;
                    inpInfoFormUPC.Text = goodsDetails.GoodsCounter.UPC;
                    inpInfoFormEAN.Text = goodsDetails.GoodsCounter.EAN;
                    inpInfoFormJAN.Text = goodsDetails.GoodsCounter.JAN;
                    inpInfoFormISBN.Text = goodsDetails.GoodsCounter.ISBN;
                    inpInfoFormPrice.Text = goodsDetails.GoodsCounter.Price.ToString();
                    inpInfoFormQuantity.Text = goodsDetails.GoodsCounter.Quantity.ToString();
                }
            }

            Page.ClientScript.RegisterStartupScript(Page.GetType(), "InfoFormActive"
                , "<script> document.getElementById('btnListInfoFormModal').click(); </script>");
        }

        //信息表单品牌选择按钮
        public void InfoFormBrandSelect_Click(object sender, EventArgs e)
        {
            var GoodsID = long.Parse(hidInfoFormGoodsID.Value);
            txtBrandSelectModalTitle.Text = GoodsID == 0 ? "新商品" : inpInfoFormGoodsTitle.Text;

            //品牌列表（未实现将品牌列表传入商品业务层，打上已选业务标记返回）
            lvBrandInfo.DataSource = Ziri.BLL.ITEM.Brand.GetListBrandFullInfo(out AlertMessage alertMessage, true);
            lvBrandInfo.DataBind();

            if (alertMessage != null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "ListMessage"
                , string.Format("<script> swal('{0}', '', '{1}'); </script>", alertMessage.Message, alertMessage.Type));
            }

            Page.ClientScript.RegisterStartupScript(Page.GetType(), "InfoFormActive", "<script> document.getElementById('btnBrandSelectModal').click(); </script>");
        }

        //信息表单品牌选择确认按钮
        public void BrandSelectSubmit_Click(object sender, EventArgs e)
        {
            var a = (HtmlAnchor)sender;
            divInfoFormBrand.Visible = true;
            hidInfoFormBrandID.Value = ((HiddenField)a.FindControl("hidBrandID")).Value;
            divInfoFormBrandLogo.Attributes["style"] = string.Format("background-image: url({0})", ((HiddenField)a.FindControl("hidBrandLogoUrl")).Value);
            txtInfoFormBrandTitle.InnerText = ((HiddenField)a.FindControl("hidBrandTitle")).Value;
            txtInfoFormBrandName.InnerText = ((HiddenField)a.FindControl("hidBrandName")).Value;
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "InfoFormActive", "<script> InfoFormTabShow('Brand'); </script>");
        }

        //信息表单品类选择时
        public void InfoFormKindRoot_Change(object sender, EventArgs e)
        {
            long kindRootID = long.Parse(drpInfoFormKindRoot.SelectedValue);
            InfoFormKindL1Bind(kindRootID);
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "InfoFormActive", "<script> InfoFormTabShow('Kind'); </script>");
        }

        //信息表单品类列表绑定
        private void InfoFormKindL1Bind(long kindRootID)
        {
            if (kindRootID == 0)
            {
                drpInfoFormKindL1.Items.Clear();
            }
            else
            {
                drpInfoFormKindL1.DataValueField = "ID";
                drpInfoFormKindL1.DataTextField = "Title";
                drpInfoFormKindL1.DataSource = Ziri.BLL.ITEM.Kind.GetKindInfos(kindRootID, true);
                drpInfoFormKindL1.DataBind();
                drpInfoFormKindL1.Items.Insert(0, new ListItem("-未选择-", "0"));
            }
        }

        //信息表单规格选择按钮
        public void InfoFormSpecSelect_Click(object sender, EventArgs e)
        {
            var GoodsID = long.Parse(hidInfoFormGoodsID.Value);
            txtSpecSelectModalTitle.Text = GoodsID == 0 ? "新商品" : inpInfoFormGoodsTitle.Text;

            //规格列表（未实现将规格列表、值列表传入商品业务层，打上已选业务标记返回）
            lvSpecInfo.DataSource = Ziri.BLL.ITEM.Spec.GetSpecInfos(out AlertMessage alertMessage, true);
            lvSpecInfo.DataBind();
            foreach (var item in lvSpecInfo.Items)
            {
                var lvSpecValue = (ListView)item.FindControl("lvSpecValue");
                lvSpecValue.DataSource = Ziri.BLL.ITEM.Spec.GetSpecValues(long.Parse(((HiddenField)item.FindControl("hidSpecID")).Value), out alertMessage, true);
                lvSpecValue.DataBind();
            }

            if (alertMessage != null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "ListMessage"
                , string.Format("<script> swal('{0}', '', '{1}'); </script>", alertMessage.Message, alertMessage.Type));
            }

            Page.ClientScript.RegisterStartupScript(Page.GetType(), "InfoFormActive"
                , "<script> document.getElementById('btnSpecSelectModal').click(); </script>");
        }

        //信息表单规格选择确认按钮
        public void SpecSelectSubmit_Click(object sender, EventArgs e)
        {
            //获取规格选择
            var SpecValues = new List<SpecValues>();
            foreach (var specInfo in lvSpecInfo.Items)
            {
                var Values = new List<SpecValue>();
                foreach (var specValue in ((ListView)specInfo.FindControl("lvSpecValue")).Items)
                {
                    var specCheckbox = (HtmlControl)specValue.FindControl("lvSpecValueID");
                    if (specCheckbox.Attributes["checked"] == "checked")
                    {
                        Values.Add(new SpecValue
                        {
                            ID = long.Parse(specCheckbox.Attributes["specvalueid"]),
                            Value = specCheckbox.Attributes["specvalue"],
                        });
                    }
                }
                if (Values.Count > 0)
                {
                    SpecValues.Add(new SpecValues
                    {
                        SpecInfo = new Ziri.MDL.SpecInfo
                        {
                            ID = long.Parse(((HiddenField)specInfo.FindControl("hidSpecID")).Value),
                            Title = ((HiddenField)specInfo.FindControl("hidSpecTitle")).Value,
                        },
                        Values = Values,
                    });
                }
            }
            //组合商品规格
            var GoodsSpecs = new List<GoodsSpecFull>();
            foreach (var specInfo in SpecValues)
            {
                //历遍参数
                var SpecGroupTemp = new List<GoodsSpecFull>();
                foreach (var valueInfo in specInfo.Values)
                {
                    if (GoodsSpecs.Count == 0)
                    {
                        //初始化已选参数
                        SpecGroupTemp.Add(new GoodsSpecFull
                        {
                            SpecValueIDs = valueInfo.ID.ToString(),
                            SpecValues = valueInfo.Value,
                            Enabled = true,
                        });
                    }
                    else
                    {
                        //组合到已选参数
                        foreach (var exist in GoodsSpecs)
                        {
                            SpecGroupTemp.Add(new GoodsSpecFull
                            {
                                SpecValueIDs = exist.SpecValueIDs + "," + valueInfo.ID.ToString(),
                                SpecValues = exist.SpecValues + "," + valueInfo.Value,
                                Enabled = true,
                            });
                        }
                    }
                }
                GoodsSpecs = SpecGroupTemp;
            }
            //已有商品规格
            var GoodsID = long.Parse(hidInfoFormGoodsID.Value);
            if (GoodsID > 0) { GoodsSpecs = Ziri.BLL.ITEM.Goods.GetModifyGoodsSpecs(GoodsID, GoodsSpecs); }
            //显示表单
            lvInfoFormSpec.DataSource = GoodsSpecs;
            lvInfoFormSpec.DataBind();
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "InfoFormActive", "<script> InfoFormTabShow('Spec'); </script>");
        }
    }
}