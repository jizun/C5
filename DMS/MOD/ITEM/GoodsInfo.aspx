<%@ Page Title="" Language="C#" MasterPageFile="~/UI/Keen.Master" AutoEventWireup="true" CodeBehind="GoodsInfo.aspx.cs" Inherits="DMS.MOD.ITEM.GoodsInfo" %>

<%@ Register Src="~/UC/PagerControl.ascx" TagPrefix="uc1" TagName="PagerControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageHeadPlaceHolder" runat="server">
    <link href="/CSS/List.css" rel="stylesheet" type="text/css" />
    <link href="/CSS/Goods.css" rel="stylesheet" type="text/css">
    <script src="/JS/List.js" type="text/javascript"></script>
    <script src="/JS/GoodsFun.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageContentPlaceHolder" runat="server">
    <!-- begin:: Content -->
    <!-- begin: List -->
    <div class="k-content  k-grid__item k-grid__item--fluid" id="k_content">
        <div class="k-portlet k-portlet--mobile">
            <div class="k-portlet__head">
                <div class="k-portlet__head-label">
                    <h3 class="k-portlet__head-title">商品信息列表</h3>
                </div>
                <div class="k-portlet__head-toolbar">
                    <div class="head-toolbar-button">
                        <div class="input-group">
                            <asp:TextBox ID="inpFilter" runat="server" CssClass="form-control" placeholder="商品代码、名称" ClientIDMode="Static" AutoPostBack="true" OnTextChanged="Filter_Change" />
                            <div class="input-group-append">
                                <button class="btn btn-primary head-toolbar-button-search" type="button" id="btnFilter" runat="server"
                                    onclick="if (!FilterCheck()) { return false; }" onserverclick="Filter_Click">
                                    <i class="fa fa-search"></i>
                                </button>
                            </div>
                        </div>
                    </div>
                    <div class="head-toolbar-button">
                        <button type="button" class="btn btn-clean btn-sm btn-icon btn-icon-md" title="新增" id="btnAddNew" onserverclick="AddNew_Click" runat="server">
                            <i class="flaticon2-add-1"></i>
                        </button>
                        <button type="button" class="btn btn-clean btn-sm" title="初始化" data-toggle="modal" data-target="#InitAlertModal">初始化</button>
                        <div class="modal fade bd-example-modal-sm" id="InitAlertModal" tabindex="-1" role="dialog" aria-labelledby="mySmallModalLabel" aria-hidden="true">
                            <div class="modal-dialog modal-sm">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <h5 class="modal-title">初始化确认</h5>
                                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                    </div>
                                    <div class="modal-body">
                                        <p>初始化将导致商品信息恢复原始状态，确认继续吗？</p>
                                    </div>
                                    <div class="modal-footer">
                                        <div style="width: 100%; text-align: center;">
                                            <button type="button" class="btn btn-secondary" data-dismiss="modal">取消</button>
                                            <button type="button" class="btn btn-brand" id="btnMenuInit" onserverclick="GoodsInit_Click" runat="server">确认初始化</button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <a data-toggle="modal" data-target="#InfoFormModal" id="btnListInfoFormModal" style="display: none;"></a>
                        <a data-toggle="modal" data-target="#DetailsModal" id="btnListDetailsModal" style="display: none;"></a>
                        <a data-toggle="modal" data-target="#BrandSelectModal" id="btnBrandSelectModal" style="display: none;"></a>
                        <a data-toggle="modal" data-target="#SpecSelectModal" id="btnSpecSelectModal" style="display: none;"></a>
                    </div>
                </div>
            </div>
            <div class="k-portlet__body">
                <div id="m_table_2_wrapper" class="dataTables_wrapper dt-bootstrap4 no-footer">
                    <div class="row">
                        <!--begin: Datatable -->
                        <asp:ListView ID="GoodsInfoList" runat="server">
                            <LayoutTemplate>
                                <table class="table table-striped- table-bordered table-hover table-checkable list-table">
                                    <thead>
                                        <tr>
                                            <th>行序</th>
                                            <th>
                                                <button id="ListOrderByID" runat="server" onserverclick="OrderBy_Click">
                                                    <span class="list-field-title">编号</span>
                                                    <span class="list-orderby-none" id="ListOrderByIDAsc" runat="server">↓</span>
                                                    <span class="list-orderby-active" id="ListOrderByIDDesc" runat="server">↑</span>
                                                    <asp:HiddenField ID="ListOrderByIDValue" Value="Desc" runat="server" />
                                                </button>
                                            </th>
                                            <th>
                                                <button id="ListOrderByName" runat="server" onserverclick="OrderBy_Click">
                                                    <span class="list-field-title">商品代码</span>
                                                    <span class="list-orderby-none" id="ListOrderByNameAsc" runat="server">↓</span>
                                                    <span class="list-orderby-none" id="ListOrderByNameDesc" runat="server">↑</span>
                                                    <asp:HiddenField ID="ListOrderByNameValue" runat="server" />
                                                </button>
                                            </th>
                                            <th>
                                                <button id="ListOrderByTitle" runat="server" onserverclick="OrderBy_Click">
                                                    <span class="list-field-title">商品名称</span>
                                                    <span class="list-orderby-none" id="ListOrderByTitleAsc" runat="server">↓</span>
                                                    <span class="list-orderby-none" id="ListOrderByTitleDesc" runat="server">↑</span>
                                                    <asp:HiddenField ID="ListOrderByTitleValue" runat="server" />
                                                </button>
                                            </th>
                                            <th>
                                                <button id="ListOrderByEnabled" runat="server" onserverclick="OrderBy_Click">
                                                    <span class="list-field-title">启用状态</span>
                                                    <span class="list-orderby-none" id="ListOrderByEnabledAsc" runat="server">↓</span>
                                                    <span class="list-orderby-none" id="ListOrderByEnabledDesc" runat="server">↑</span>
                                                    <asp:HiddenField ID="ListOrderByEnabledValue" runat="server" />
                                                </button>
                                            </th>
                                            <th>
                                                <button id="ListOrderByStateID" runat="server" onserverclick="OrderBy_Click">
                                                    <span class="list-field-title">货架状态</span>
                                                    <span class="list-orderby-none" id="ListOrderByStateIDdAsc" runat="server">↓</span>
                                                    <span class="list-orderby-none" id="ListOrderByStateIDDesc" runat="server">↑</span>
                                                    <asp:HiddenField ID="ListOrderByStateIDValue" runat="server" />
                                                </button>
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr id="itemPlaceholder" runat="server" />
                                    </tbody>
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr>
                                    <th><%#((Container.DataItemIndex + 1) + (InfoListPager.PageIndex * InfoListPager.PageSize - InfoListPager.PageSize))%></th>
                                    <td>
                                        <span><%#Eval("ID")%></span>
                                        <a class="btn btn-sm btn-clean btn-icon btn-icon-md" title="详情" goodsid='<%#Eval("ID")%>'
                                            id="btnListDetails" autopostback="true" onserverclick="ListDetails_Click" runat="server">
                                            <i class="la la-book"></i></a>
                                        <a class="btn btn-sm btn-clean btn-icon btn-icon-md" title="修改" goodsid='<%#Eval("ID")%>'
                                            id="btnListEdit" autopostback="true" onserverclick="ListEdit_Click" runat="server">
                                            <i class="la la-edit"></i></a>
                                    </td>
                                    <td><%#FilterFormat(inpFilter,(string)Eval("Name"))%></td>
                                    <td><%#FilterFormat(inpFilter,(string)Eval("Title"))%></td>
                                    <td>
                                        <span class="k-switch k-switch--sm k-switch--icon z-switch">
                                            <label>
                                                <input type="checkbox" checked='<%#Eval("Enabled")%>' goodsid='<%#Eval("ID")%>'
                                                    id="inpActionAuth" runat="server" onclick="__doPostBack(this.id, '')" onserverchange="ListEnabled_Change" />
                                                <span></span>
                                            </label>
                                        </span>
                                    </td>
                                    <td>
                                        <span class="dropdown">
                                            <a href="#" class="btn btn-sm btn-clean btn-icon btn-icon-md" data-toggle="dropdown" aria-expanded="true">
                                                <%#((int)Eval("StateID") == (int)Ziri.MDL.GoodsState.下架)
                                                    ? "<span class=\"badge badge-danger list-enabled\">下架</span>"
                                                    : "<span class=\"badge badge-success list-enabled\">"
                                                        + Enum.Parse(typeof(Ziri.MDL.GoodsState), Eval("StateID").ToString()) + "</span>"
                                                %>
                                            </a>
                                            <div class="dropdown-menu dropdown-menu-right">
                                                <a class="dropdown-item" goodsid='<%#Eval("ID")%>' id="btnListPublicSales" runat="server"
                                                    onserverclick="ListSalesState_Click"><i class="la la-angellist"></i>上架</a>
                                                <a class="dropdown-item" goodsid='<%#Eval("ID")%>' id="btnListStopSales" runat="server"
                                                    onserverclick="ListSalesState_Click"><i class="la la-lock"></i>下架</a>
                                            </div>
                                        </span>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:ListView>
                        <!--end: Datatable -->
                    </div>
                    <div class="row">
                        <uc1:PagerControl ID="InfoListPager" PageSize="10" ClientIDMode="Static" runat="server" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!--end: List -->
    <!--begin: InfoForm -->
    <div class="modal fade" id="InfoFormModal" tabindex="-1" role="dialog" aria-labelledby="" aria-hidden="true">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header" id="InfoFormModalHead">
                    <h5 class="modal-title">商品[<asp:Label ID="txtInfoFormTitle" runat="server" />]的信息表单</h5>
                    <div class="k-portlet__head-toolbar">
                        <a href="#" class="btn btn-secondary k-margin-r-10" data-dismiss="modal" aria-label="Close">
                            <span class="k-hidden-mobile">返回</span>
                        </a>
                        <div class="btn-group">
                            <button type="button" class="btn btn-primary" id="btnInfoFormSubmit" runat="server"
                                onclick="if (!GoodsFormCheck()) { return false; }" onserverclick="InfoFormSubmit_Click">
                                <i class="la la-check"></i>
                                <span class="k-hidden-mobile">保存</span>
                            </button>
                            <button type="button" class="btn btn-primary dropdown-toggle dropdown-toggle-split" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                            </button>
                            <div class="dropdown-menu dropdown-menu-right">
                                <a class="dropdown-item" href="#"><i class="la la-plus"></i>Save & New</a>
                                <div class="dropdown-divider"></div>
                                <a class="dropdown-item" href="#"><i class="la la-close"></i>Cancel</a>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-body">
                    <asp:HiddenField ID="hidInfoFormGoodsID" Value="0" runat="server" />
                    <ul class="nav nav-tabs nav-tabs-line nav-tabs-bold nav-tabs-line-3x nav-tabs-line-brand" role="tablist">
                        <li class="nav-item">
                            <a class="nav-link active" data-toggle="tab" href="#k_tabs_info" role="tab">品名</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" data-toggle="tab" href="#k_tabs_brand" role="tab" id="btnListInfoFormBrandTab">品牌</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" data-toggle="tab" href="#k_tabs_kind" role="tab" id="btnListInfoFormKindTab">品类</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" data-toggle="tab" href="#k_tabs_tag" role="tab">标签</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" data-toggle="tab" href="#k_tabs_photos" role="tab">图片</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" data-toggle="tab" href="#k_tabs_desc" role="tab">描述</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" data-toggle="tab" href="#k_tabs_spec" role="tab" id="btnListInfoFormSpecTab">规格</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" data-toggle="tab" href="#k_tabs_counter" role="tab">单价</a>
                        </li>
                    </ul>
                    <div class="tab-content">
                        <div class="tab-pane active" id="k_tabs_info" role="tabpanel">
                            <div class="k-form k-form--label-right">
                                <!--商品代码-->
                                <div class="form-group row k-margin-t-20">
                                    <label class="col-form-label col-lg-3 col-sm-12">商品代码</label>
                                    <div class="col-lg-9 col-md-9 col-sm-12">
                                        <asp:TextBox ID="inpInfoFormGoodsName" runat="server" ClientIDMode="Static" class="form-control"
                                            MaxLength="32" title="商品代码" placeholder="商品代码最多32个字符。" />
                                    </div>
                                </div>
                                <!--商品名称-->
                                <div class="form-group row k-margin-t-20">
                                    <label class="col-form-label col-lg-3 col-sm-12">商品名称</label>
                                    <div class="col-lg-9 col-md-9 col-sm-12">
                                        <asp:TextBox ID="inpInfoFormGoodsTitle" runat="server" ClientIDMode="Static" class="form-control"
                                            MaxLength="256" title="商品名称" placeholder="商品名称最多256个字符。" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!--品牌-->
                        <div class="tab-pane" id="k_tabs_brand" role="tabpanel">
                            <asp:Button class="btn btn-primary" ID="btnInfoFromBrandSelect" runat="server" Text="选择品牌" OnClick="InfoFormBrandSelect_Click" /><br />
                            <asp:HiddenField ID="hidInfoFormBrandID" runat="server" Value='<%#Eval("ID")%>' />
                            <div class="row" id="divInfoFormBrand" runat="server">
                                <div class="col-md-12 col-lg-5 col-xl-4" style="text-align: right;">
                                    <div class="k-avatar k-avatar--outline k-avatar--success">
                                        <div class="k-avatar__holder" id="divInfoFormBrandLogo" runat="server"></div>
                                    </div>
                                </div>
                                <div class="col-md-12 col-lg-5 col-xl-4">
                                    <h1 style="line-height: 120px;">
                                        <span id="txtInfoFormBrandTitle" runat="server" />
                                        <span class="badge badge-primary" id="txtInfoFormBrandName" runat="server" />
                                    </h1>
                                </div>
                            </div>
                        </div>
                        <!--品类-->
                        <div class="tab-pane" id="k_tabs_kind" role="tabpanel">
                            <div class="k-form k-form--label-right">
                                <!--根类-->
                                <div class="form-group row k-margin-t-20">
                                    <label class="col-form-label col-lg-3 col-sm-12">根类</label>
                                    <div class="col-lg-9 col-md-9 col-sm-12">
                                        <asp:DropDownList ID="drpInfoFormKindRoot" runat="server" OnSelectedIndexChanged="InfoFormKindRoot_Change" AutoPostBack="true" class="form-control" />
                                    </div>
                                </div>
                                <!--一级分类-->
                                <div class="form-group row k-margin-t-20">
                                    <label class="col-form-label col-lg-3 col-sm-12">一级分类</label>
                                    <div class="col-lg-9 col-md-9 col-sm-12">
                                        <asp:DropDownList ID="drpInfoFormKindL1" runat="server" class="form-control" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!--标签-->
                        <div class="tab-pane" id="k_tabs_tag" role="tabpanel">
                            <div class="alert alert-info fade show" role="alert">
                                <div class="alert-icon"><i class="flaticon-questions-circular-button"></i></div>
                                <div class="alert-text">示例：新品、热卖、特价等。</div>
                            </div>
                            <div class="col-lg-6">
                                <asp:ListView ID="lvInfoFormTags" runat="server">
                                    <LayoutTemplate>
                                        <div class="k-repeater">
                                            <div data-repeater-list="demo1">
                                                <div id="itemPlaceholder" runat="server" />
                                            </div>
                                            <div class="k-repeater__add-data">
                                                <span data-repeater-create="" class="btn btn-info btn-sm">
                                                    <i class="la la-plus"></i>增加标签
                                                </span>
                                            </div>
                                        </div>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <div data-repeater-item="" class="k-repeater__item">
                                            <div class="input-group">
                                                <div class="input-group-prepend"><span class="input-group-text"><i class="la la-puzzle-piece"></i></span></div>
                                                <input type="text" name='demo1[<%#Container.DataItemIndex%>][inpInfoFormTag]'
                                                    value='<%#Eval("Title")%>' class="form-control" placeholder="标签文字">
                                            </div>
                                            <div class="k-separator k-separator--space-sm"></div>
                                        </div>
                                    </ItemTemplate>
                                </asp:ListView>
                            </div>
                        </div>
                        <!--图片-->
                        <div class="tab-pane" id="k_tabs_photos" role="tabpanel">
                            <div class="dropzone dropzone-primary" id="k-dropzone-two"></div>
                            <asp:HiddenField ID="hidInfoFormGoodsPhoto" ClientIDMode="Static" runat="server" />
                        </div>
                        <!--描述-->
                        <div class="tab-pane" id="k_tabs_desc" role="tabpanel">
                            <div class="summernote" id="txtInfoFormGoodsDesc"></div>
                            <asp:HiddenField ID="hidInfoFormGoodsDesc" ClientIDMode="Static" runat="server" />
                        </div>
                        <!--规格-->
                        <div class="tab-pane" id="k_tabs_spec" role="tabpanel">
                            <div class="form-group row">
                                <div class="col-lg-4">
                                    <asp:Button class="btn btn-primary" ID="btnInfoFromSpecSelect" runat="server" Text="选择规格" OnClick="InfoFormSpecSelect_Click" />
                                </div>
                                <div class="col-lg-4">
                                    <input type="text" class="form-control txtPrice" placeholder="统一单价" onchange="SpecPriceAll(this);" />
                                </div>
                                <div class="col-lg-4">
                                    <input type="text" class="form-control bootstrap-touchspin-vertical-btn txtQuantity" placeholder="统一数量" onchange="SpecQtyAll(this);" />
                                </div>
                            </div>
                            <asp:ListView ID="lvInfoFormSpec" runat="server">
                                <LayoutTemplate>
                                    <table class="table table-striped m-table">
                                        <thead class="thead-light">
                                            <tr>
                                                <th>序号</th>
                                                <th>规格</th>
                                                <th>编码</th>
                                                <th>单价</th>
                                                <th>数量</th>
                                                <th>启用</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr id="itemPlaceholder" runat="server" />
                                        </tbody>
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <th scope="row">
                                            <%#Container.DataItemIndex + 1%>
                                        </th>
                                        <td>
                                            <%#Eval("SpecValues")%>
                                            <asp:HiddenField ID="hidValueIDs" Value='<%#Eval("SpecValueIDs")%>' runat="server" />
                                            <asp:HiddenField ID="hidValues" Value='<%#Eval("SpecValues")%>' runat="server" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="inpInfoFormSKU" runat="server" Value='<%#Eval("SKU")%>' class="form-control inp-txt-len" MaxLength="32" title="SKU" placeholder="SKU" /><br />
                                            <asp:TextBox ID="inpInfoFormUPC" runat="server" Value='<%#Eval("UPC")%>' class="form-control inp-txt-len" MaxLength="32" title="UPC" placeholder="UPC" /><br />
                                            <asp:TextBox ID="inpInfoFormEAN" runat="server" Value='<%#Eval("EAN")%>' class="form-control inp-txt-len" MaxLength="32" title="EAN" placeholder="EAN" /><br />
                                            <asp:TextBox ID="inpInfoFormJAN" runat="server" Value='<%#Eval("JAN")%>' class="form-control inp-txt-len" MaxLength="32" title="JAN" placeholder="JAN" /><br />
                                            <asp:TextBox ID="inpInfoFormISBN" runat="server" Value='<%#Eval("ISBN")%>' class="form-control inp-txt-len" MaxLength="32" title="ISBN" placeholder="ISBN" />
                                        </td>
                                        <td>
                                            <asp:TextBox class="form-control txtPrice txtSpecPrice" ID="inpSpecPrice" Text='<%#Eval("Price")%>' runat="server" />
                                        </td>
                                        <td>
                                            <asp:TextBox class="form-control bootstrap-touchspin-vertical-btn txtQuantity txtSpecQty"
                                                ID="inpSpecQty" Text='<%#Eval("Quantity")%>' runat="server" />
                                        </td>
                                        <td>
                                            <span class="k-switch k-switch--sm k-switch--icon z-switch">
                                                <label>
                                                    <input type="checkbox" id="inpSpecEnabled" runat="server" checked='<%#Eval("Enabled")%>' />
                                                    <span></span>
                                                </label>
                                            </span>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:ListView>
                        </div>
                        <!--单价-->
                        <div class="tab-pane" id="k_tabs_counter" role="tabpanel">
                            <div class="k-form k-form--label-right">
                                <!--提示-->
                                <div class="alert alert-danger" role="alert">
                                    <div class="alert-icon"><i class="flaticon-warning"></i></div>
                                    <div class="alert-text">在没有规格的情况下有效。</div>
                                </div>
                                <!--SKU-->
                                <div class="form-group row k-margin-t-20">
                                    <label class="col-form-label col-lg-3 col-sm-12">SKU</label>
                                    <div class="col-lg-9 col-md-9 col-sm-12">
                                        <asp:TextBox ID="inpInfoFormSKU" runat="server" ClientIDMode="Static" class="form-control inp-txt-len"
                                            MaxLength="32" title="SKU" placeholder="SKU" />
                                    </div>
                                </div>
                                <!--UPC-->
                                <div class="form-group row k-margin-t-20">
                                    <label class="col-form-label col-lg-3 col-sm-12">UPC</label>
                                    <div class="col-lg-9 col-md-9 col-sm-12">
                                        <asp:TextBox ID="inpInfoFormUPC" runat="server" ClientIDMode="Static" class="form-control inp-txt-len"
                                            MaxLength="32" title="UPC" placeholder="UPC" />
                                    </div>
                                </div>
                                <!--EAN-->
                                <div class="form-group row k-margin-t-20">
                                    <label class="col-form-label col-lg-3 col-sm-12">EAN</label>
                                    <div class="col-lg-9 col-md-9 col-sm-12">
                                        <asp:TextBox ID="inpInfoFormEAN" runat="server" ClientIDMode="Static" class="form-control inp-txt-len"
                                            MaxLength="32" title="EAN" placeholder="EAN" />
                                    </div>
                                </div>
                                <!--JAN-->
                                <div class="form-group row k-margin-t-20">
                                    <label class="col-form-label col-lg-3 col-sm-12">JAN</label>
                                    <div class="col-lg-9 col-md-9 col-sm-12">
                                        <asp:TextBox ID="inpInfoFormJAN" runat="server" ClientIDMode="Static" class="form-control inp-txt-len"
                                            MaxLength="32" title="JAN" placeholder="JAN" />
                                    </div>
                                </div>
                                <!--ISBN-->
                                <div class="form-group row k-margin-t-20">
                                    <label class="col-form-label col-lg-3 col-sm-12">ISBN</label>
                                    <div class="col-lg-9 col-md-9 col-sm-12">
                                        <asp:TextBox ID="inpInfoFormISBN" runat="server" ClientIDMode="Static" class="form-control inp-txt-len"
                                            MaxLength="32" title="ISBN" placeholder="ISBN" />
                                    </div>
                                </div>
                                <!--单价-->
                                <div class="form-group row k-margin-t-20">
                                    <label class="col-form-label col-lg-3 col-sm-12">单价</label>
                                    <div class="col-lg-9 col-md-9 col-sm-12">
                                        <asp:TextBox class="form-control txtPrice"
                                            ID="inpInfoFormPrice" runat="server" Text='<%#Eval("Price")%>' placeholder="单价" />
                                    </div>
                                </div>
                                <!--数量-->
                                <div class="form-group row k-margin-t-20">
                                    <label class="col-form-label col-lg-3 col-sm-12">数量</label>
                                    <div class="col-lg-9 col-md-9 col-sm-12">
                                        <asp:TextBox class="form-control bootstrap-touchspin-vertical-btn txtQuantity"
                                            ID="inpInfoFormQuantity" runat="server" Text='<%#Eval("Quantity")%>' placeholder="数量" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                </div>
            </div>
        </div>
    </div>
    <!--end: InfoForm -->
    <!--begin: InfoFormBrandSelect -->
    <div class="modal fade" id="BrandSelectModal" tabindex="-1" role="dialog" aria-labelledby="" aria-hidden="true">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">商品[<asp:Label ID="txtBrandSelectModalTitle" runat="server" />]的品牌选择</h5>
                    <div class="k-portlet__head-toolbar">
                        <a data-dismiss="modal" aria-label="Close" id="btnBrandSelectClose" style="display: none;"></a>
                        <a class="btn btn-secondary k-margin-r-10" id="btnBrandSelectModalBack"
                            onclick="InfoFormTabShow('Brand');"><span class="k-hidden-mobile">返回</span></a>
                        <button type="button" class="btn btn-primary" id="btnBrandSelectSubmit" runat="server" onserverclick="BrandSelectSubmit_Click">
                            <i class="la la-check"></i>
                            <span class="k-hidden-mobile">确定</span>
                        </button>
                    </div>
                </div>
                <div class="modal-body">
                    <asp:ListView ID="lvBrandInfo" runat="server">
                        <ItemTemplate>
                            <a style="display: inline-block;" id="divBrandSelectSubmit" runat="server" onserverclick="BrandSelectSubmit_Click">
                                <asp:HiddenField ID="hidBrandID" runat="server" Value='<%#Eval("ID")%>' />
                                <asp:HiddenField ID="hidBrandName" runat="server" Value='<%#Eval("Name")%>' />
                                <asp:HiddenField ID="hidBrandTitle" runat="server" Value='<%#Eval("Title")%>' />
                                <asp:HiddenField ID="hidBrandLogoUrl" runat="server" Value='<%#Eval("LogoFileGUID") == null
                                        ? "/media/logos/logo_template.png"
                                        : string.Format("/DOC/upload/{0}{1}",Eval("LogoFileGUID"),Eval("LogoFileExtName"))%>' />
                                <div class="k-avatar k-avatar--outline k-avatar--success">
                                    <div class="k-avatar__holder" style='background-image: url(<%#Eval("LogoFileGUID") == null
                                        ? "/media/logos/logo_template.png"
                                        : string.Format("/DOC/upload/{0}{1}",Eval("LogoFileGUID"),Eval("LogoFileExtName"))%>)'>
                                    </div>
                                </div>
                                <div style="text-align: center;">
                                    <span><%#Eval("Title")%></span>
                                    <span><%#Eval("Name")%></span>
                                </div>
                            </a>
                        </ItemTemplate>
                    </asp:ListView>
                </div>
            </div>
        </div>
    </div>
    <!--end: InfoFormBrandSelect -->
    <!--begin: InfoFormSpecSelect -->
    <div class="modal fade" id="SpecSelectModal" tabindex="-1" role="dialog" aria-labelledby="" aria-hidden="true">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">商品[<asp:Label ID="txtSpecSelectModalTitle" runat="server" />]的规格选择</h5>
                    <div class="k-portlet__head-toolbar">
                        <a data-dismiss="modal" aria-label="Close" id="btnSpecSelectClose" style="display: none;"></a>
                        <a class="btn btn-secondary k-margin-r-10" id="btnSpecSelectModalBack"
                            onclick="InfoFormTabShow('Spec');"><span class="k-hidden-mobile">返回</span></a>
                        <button type="button" class="btn btn-primary" id="btnSpecSelectSubmit" runat="server" onserverclick="SpecSelectSubmit_Click">
                            <i class="la la-check"></i>
                            <span class="k-hidden-mobile">确定</span>
                        </button>
                    </div>
                </div>
                <div class="modal-body">
                    <asp:ListView ID="lvSpecInfo" runat="server">
                        <LayoutTemplate>
                            <ul class="k-nav k-nav--active-bg" id="m_nav" role="tablist">
                                <li id="itemPlaceholder" runat="server" />
                            </ul>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <li class="k-nav__item k-nav__item--sub">
                                <asp:HiddenField ID="hidSpecID" runat="server" Value='<%#Eval("ID")%>' />
                                <asp:HiddenField ID="hidSpecTitle" runat="server" Value='<%#Eval("Title")%>' />
                                <div class="k-nav__link collapsed" role="tab" id="m_nav_link_<%#Eval("ID")%>" data-toggle="collapse" href="#m_nav_sub_<%#Eval("ID")%>" aria-expanded="false">
                                    <i class="k-nav__link-icon <%#Eval("IconFont")%>"></i>
                                    <span class="k-nav__link-text">
                                        <%#Eval("Title")%>（<%#Eval("Name")%>）
                                        <label class="k-checkbox k-checkbox--brand">
                                            <input type="checkbox" onclick="SpecSelectAll(this, '#m_nav_sub_<%#Eval("ID")%>')" />&nbsp;<span></span>
                                        </label>
                                    </span>
                                    <span class="k-nav__link-arrow"></span>
                                </div>
                                <ul class="k-nav__sub collapse show" id="m_nav_sub_<%#Eval("ID")%>" role="tabpanel" aria-labelledby="m_nav_link_<%#Eval("ID")%>" data-parent="#m_nav">
                                    <asp:ListView ID="lvSpecValue" runat="server">
                                        <ItemTemplate>
                                            <li class="k-nav__item">
                                                <a class="k-nav__link">
                                                    <span class="k-nav__link-bullet k-nav__link-bullet--line"><span></span></span>
                                                    <span class="k-nav__link-text"><%#Eval("Value")%>
                                                        <label class="k-checkbox k-checkbox--brand">
                                                            <input type="checkbox" id="lvSpecValueID" runat="server"
                                                                specvalueid='<%#Eval("ID")%>' specvalue='<%#Eval("Value")%>' />&nbsp;<span></span>
                                                        </label>
                                                    </span>
                                                </a>
                                            </li>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </ul>
                            </li>
                        </ItemTemplate>
                    </asp:ListView>
                </div>
            </div>
        </div>
    </div>
    <!--end: InfoFormSpecSelect -->
    <!--begin: InfoDetails -->
    <div class="modal fade" id="DetailsModal" tabindex="-1" role="dialog" aria-labelledby="" aria-hidden="true">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">商品[<asp:Label ID="txtDetailsModalTitle" runat="server" />]的详细信息</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true" class="la la-remove"></span>
                    </button>
                </div>
                <div class="modal-body">
                    <!-- 品类 -->
                    <asp:ListView ID="lvDetailsKinds" runat="server">
                        <LayoutTemplate>
                            <nav aria-label="breadcrumb">
                                <ol class="breadcrumb">
                                    <li id="itemPlaceholder" runat="server" />
                                </ol>
                            </nav>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <li class="breadcrumb-item"><%#Eval("Title")%></li>
                        </ItemTemplate>
                    </asp:ListView>
                    <!-- 图片 -->
                    <div id="divGoodsPhotos" runat="server">
                        <asp:ListView ID="lvGoodsPhoto1" runat="server">
                            <LayoutTemplate>
                                <ul id="slider">
                                    <li id="itemPlaceholder" runat="server" />
                                </ul>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <li id="<%#Container.DataItemIndex + 1%>">
                                    <img src="<%#Container.DataItem%>" />
                                </li>
                            </ItemTemplate>
                        </asp:ListView>
                        <asp:ListView ID="lvGoodsPhoto2" runat="server">
                            <LayoutTemplate>
                                <ul id="thumb">
                                    <li id="itemPlaceholder" runat="server" />
                                </ul>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <li>
                                    <a href="#<%#Container.DataItemIndex + 1%>">
                                        <img src="<%#Container.DataItem%>" /></a>
                                </li>
                            </ItemTemplate>
                        </asp:ListView>
                    </div>
                    <!-- 品名标签 -->
                    <div>
                        <h1 id="txtDetailsTitle" runat="server" style="display: inline;" />
                        <span id="txtDetailsName" runat="server" style="font-size: 0.5em; line-height: 2em; padding-left: 10px; display: none;" />
                        <div style="float: right;">
                            <asp:ListView ID="lvDetailsTags" runat="server">
                                <ItemTemplate>
                                    <span class="badge badge-pill badge-dark"><i class="fa fa-tag"></i>&nbsp;<%#Eval("Title")%></span>
                                </ItemTemplate>
                            </asp:ListView>
                        </div>
                    </div>
                    <!-- 数量金额 -->
                    <div class="row">
                        <div class="col-lg-4" style="text-align: left;">
                            <span class="spec-amount" id="txtDetailsAmount" runat="server"></span>
                            <span class="spec-quantity">
                                <asp:TextBox class="form-control bootstrap-touchspin-vertical-btn txtQuantity" onchange="DetailsQtyChange(this);"
                                    Style="text-align: center;" ID="inpDetailsQty" runat="server" Text='1' placeholder="数量" />
                            </span>
                        </div>
                        <div class="col-lg-8" style="text-align: right;">
                            <div class="spec-buy">
                                <button type="button" class="btn btn-focus btn-danger btn-pill"><i class="flaticon-cart"></i>&nbsp;加入购物车</button>
                                <button type="button" class="btn btn-focus btn-elevate btn-pill"><i class="flaticon-business"></i>&nbsp;立即购买</button>
                            </div>
                        </div>
                    </div>
                    <!-- 规格参数 -->
                    <asp:ListView ID="lvDetailsSpecs" runat="server">
                        <LayoutTemplate>
                            <fieldset id="itemPlaceholder" runat="server" />
                        </LayoutTemplate>
                        <ItemTemplate>
                            <fieldset class="spec-fieldset">
                                <legend class="spec-legend"><%#Eval("Title")%></legend>
                                <asp:HiddenField ID="lvDetailsSpecID" runat="server" Value='<%#Eval("ID")%>' />
                                <asp:ListView ID="lvDetailsSpecValues" runat="server">
                                    <ItemTemplate>
                                        <asp:Label ID="txtDetailsSpecValue" runat="server" class="spec-button badge" Text='<%#Eval("Value")%>'
                                            specid='<%#Eval("SpecID")%>' specvalueid='<%#Eval("ID")%>' onclick="DetailsSpecSelect(this);" />
                                    </ItemTemplate>
                                </asp:ListView>
                            </fieldset>
                        </ItemTemplate>
                    </asp:ListView>
                    <!-- 品牌 -->
                    <fieldset class="brand-fieldset" id="divDetailsBrand" runat="server">
                        <legend class="spec-legend">品牌</legend>
                        <div class="row">
                            <div class="col-lg-4" style="text-align: center;">
                                <div class="k-avatar k-avatar--outline k-avatar--success">
                                    <div class="k-avatar__holder" id="divDetailsBrandLogo" runat="server"></div>
                                </div>
                            </div>
                            <div class="col-lg-8" style="text-align: center;">
                                <h1 style="line-height: 120px;">
                                    <span id="divDetailsBrandTitle" runat="server" />
                                    <span class="badge badge-primary" id="divDetailsBrandName" runat="server" />
                                </h1>
                            </div>
                        </div>
                    </fieldset>
                    <!-- 规格 -->
                    <fieldset class="spec-list-fieldset" id="divDetailsSpec" runat="server">
                        <legend class="spec-legend">规格</legend>
                        <asp:HiddenField ID="hidDetailsGoodsSpecs" runat="server" ClientIDMode="Static" />
                        <asp:ListView ID="lvDetailsGoodsSpecs" runat="server">
                            <LayoutTemplate>
                                <table class="table table-striped m-table">
                                    <thead class="thead-light">
                                        <tr>
                                            <th>序号</th>
                                            <th>规格</th>
                                            <th>编码</th>
                                            <th>单价</th>
                                            <th>数量</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr id="itemPlaceholder" runat="server" />
                                    </tbody>
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr>
                                    <th scope="row">
                                        <%#Container.DataItemIndex + 1%>
                                    </th>
                                    <td>
                                        <%#Eval("SpecValues")%>
                                        <asp:HiddenField ID="hidValueIDs" Value='<%#Eval("SpecValueIDs")%>' runat="server" />
                                        <asp:HiddenField ID="hidValues" Value='<%#Eval("SpecValues")%>' runat="server" />
                                    </td>
                                    <td>
                                        <ul>
                                            <li>SKU
                                            <asp:Label ID="txtDetailsSKU" runat="server" Text='<%#Eval("SKU")%>' /></li>
                                            <li>UPC
                                            <asp:Label ID="txtDetailsUPC" runat="server" Text='<%#Eval("UPC")%>' /></li>
                                            <li>EAN
                                            <asp:Label ID="txtDetailsEAN" runat="server" Text='<%#Eval("EAN")%>' /></li>
                                            <li>JAN
                                            <asp:Label ID="txtDetailsJAN" runat="server" Text='<%#Eval("JAN")%>' /></li>
                                            <li>ISBN
                                            <asp:Label ID="txtDetailsISBN" runat="server" Text='<%#Eval("ISBN")%>' /></li>
                                        </ul>
                                    </td>
                                    <td>
                                        <asp:Label ID="txtSpecPrice" runat="server" Text='<%#((decimal)Eval("Price")).ToString("c")%>' />
                                    </td>
                                    <td>
                                        <asp:Label ID="txtSpecQty" runat="server" Text='<%#((decimal)Eval("Quantity")).ToString("#0")%>' />
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:ListView>
                    </fieldset>
                    <!-- 单价 -->
                    <fieldset class="price-fieldset" id="divDetailsSinglePrice" runat="server">
                        <legend class="spec-legend">单价</legend>
                        <ul>
                            <li>SKU
                                <asp:Label ID="txtDetailsSKU" runat="server" /></li>
                            <li>UPC
                                <asp:Label ID="txtDetailsUPC" runat="server" /></li>
                            <li>EAN
                                <asp:Label ID="txtDetailsEAN" runat="server" /></li>
                            <li>JAN
                                <asp:Label ID="txtDetailsJAN" runat="server" /></li>
                            <li>ISBN
                                <asp:Label ID="txtDetailsISBN" runat="server" /></li>
                            <li>单价
                                <asp:Label ID="txtDetailsPrice" runat="server" /></li>
                            <li>数量
                                <asp:Label ID="txtDetailsQty" runat="server" /></li>
                        </ul>
                    </fieldset>
                    <!-- 描述 -->
                    <div id="txtDetailsDesc" runat="server" />
                </div>
            </div>
        </div>
    </div>
    <!--end: InfoDetails -->
    <!-- end:: Content -->
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PageFootPlaceHolder" runat="server">
    <script src="/JS/GoodsInit.js" type="text/javascript"></script>
</asp:Content>
