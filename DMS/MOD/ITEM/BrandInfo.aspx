<%@ Page Title="" Language="C#" MasterPageFile="~/UI/Keen.Master" AutoEventWireup="true" CodeBehind="BrandInfo.aspx.cs" Inherits="DMS.MOD.ITEM.BrandInfo" %>

<%@ Register Src="~/UC/PagerControl.ascx" TagPrefix="uc1" TagName="PagerControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageHeadPlaceHolder" runat="server">
    <link href="/CSS/List.css" rel="stylesheet" type="text/css" />
    <style>
        .banner_width {
            width: 350px !important;
        }
    </style>
    <script src="/JS/List.js" type="text/javascript"></script>
    <script>
        //表单提交检查
        function BrandFormCheck() {
            var inps = ['inpInfoFormBrandName', 'inpInfoFormBrandTitle'];
            if (FormCheck(inps)) { return true; };
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageContentPlaceHolder" runat="server">
    <!-- begin:: Content -->
    <!-- begin: List -->
    <div class="k-portlet">
        <div class="k-portlet__head">
            <div class="k-portlet__head-label">
                <h3 class="k-portlet__head-title">品牌信息</h3>
            </div>
            <div class="k-portlet__head-toolbar">
                <div class="head-toolbar-button">
                    <div class="input-group">
                        <asp:TextBox ID="inpFilter" runat="server" CssClass="form-control" placeholder="品牌名称" ClientIDMode="Static" AutoPostBack="true" OnTextChanged="Filter_Change" />
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
                                    <p>初始化将导致模块信息恢复原始状态，确认继续吗？</p>
                                </div>
                                <div class="modal-footer">
                                    <div style="width: 100%; text-align: center;">
                                        <button type="button" class="btn btn-secondary" data-dismiss="modal">取消</button>
                                        <button type="button" class="btn btn-brand" id="btnMenuInit" onserverclick="Init_Click" runat="server">确认初始化</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <a data-toggle="modal" data-target="#InfoFormModal" id="btnListInfoFormModal" style="display: none;"></a>
                    <a data-toggle="modal" data-target="#DetailsModal" id="btnListDetailsModal" style="display: none;"></a>
                </div>
            </div>
        </div>
        <div class="k-portlet__body">
            <div id="m_table_2_wrapper" class="dataTables_wrapper dt-bootstrap4 no-footer">
                <div class="row">
                    <!--begin: Datatable -->
                    <asp:ListView ID="BrandInfoList" runat="server">
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
                                        <th>品牌LOGO</th>
                                        <th>
                                            <button id="ListOrderByName" runat="server" onserverclick="OrderBy_Click">
                                                <span class="list-field-title">品牌代码</span>
                                                <span class="list-orderby-none" id="ListOrderByNameAsc" runat="server">↓</span>
                                                <span class="list-orderby-none" id="ListOrderByNameDesc" runat="server">↑</span>
                                                <asp:HiddenField ID="ListOrderByNameValue" runat="server" />
                                            </button>
                                        </th>
                                        <th>
                                            <button id="ListOrderByTitle" runat="server" onserverclick="OrderBy_Click">
                                                <span class="list-field-title">品牌名称</span>
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
                                    <a class="btn btn-sm btn-clean btn-icon btn-icon-md" title="详情" brandid='<%#Eval("ID")%>'
                                        id="btnListDetails" autopostback="true" onserverclick="ListDetails_Click" runat="server">
                                        <i class="la la-book"></i></a>
                                    <a class="btn btn-sm btn-clean btn-icon btn-icon-md" title="修改" brandid='<%#Eval("ID")%>'
                                        id="btnListEdit" autopostback="true" onserverclick="ListEdit_Click" runat="server">
                                        <i class="la la-edit"></i></a>
                                </td>
                                <td class="list-logo">
                                    <img src='<%#Eval("LogoFileGUID") == null ? "/media/logos/logo_template.png" : "/DOC/upload/" + Eval("LogoFileGUID") + Eval("LogoFileExtName")%>' />
                                </td>
                                <td><%#FilterFormat(inpFilter, (string)Eval("Name"))%></td>
                                <td><%#FilterFormat(inpFilter, (string)Eval("Title"))%></td>
                                <td>
                                    <span class="dropdown">
                                        <a href="#" class="btn btn-sm btn-clean btn-icon btn-icon-md" data-toggle="dropdown" aria-expanded="true">
                                            <%#((bool)Eval("Enabled"))
                                                ? "<span class=\"badge badge-success list-enabled\">启用</span>"
                                                : "<span class=\"badge badge-danger list-enabled\">禁用</span>"
                                            %>
                                        </a>
                                        <div class="dropdown-menu dropdown-menu-right">
                                            <a class="dropdown-item" brandid='<%#Eval("ID")%>' id="btnListEnabled" runat="server"
                                                onserverclick="ListEnabled_Click"><i class="la la-angellist"></i>启用</a>
                                            <a class="dropdown-item" brandid='<%#Eval("ID")%>' id="btnListDisabled" runat="server"
                                                onserverclick="ListEnabled_Click"><i class="la la-lock"></i>禁用</a>
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
    <!--end: List -->
    <!--begin: InfoForm -->
    <div class="modal fade" id="InfoFormModal" tabindex="-1" role="dialog" aria-labelledby="" aria-hidden="true">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header" id="InfoFormModalHead">
                    <h5 class="modal-title">品牌[<asp:Label ID="txtInfoFormTitle" runat="server" />]的信息表单</h5>
                    <div class="k-portlet__head-toolbar">
                        <a href="#" class="btn btn-secondary k-margin-r-10" data-dismiss="modal" aria-label="Close">
                            <span class="k-hidden-mobile">返回</span>
                        </a>
                        <div class="btn-group">
                            <button type="button" class="btn btn-primary" id="btnInfoFormSubmit" runat="server"
                                onclick="if (!BrandFormCheck()) { return false; }" onserverclick="InfoFormSubmit_Click">
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
                    <asp:HiddenField ID="hidInfoFormBrandID" Value="0" runat="server" />
                    <div class="k-form k-form--label-right">
                        <!--代码-->
                        <div class="form-group row k-margin-t-20">
                            <label class="col-form-label col-lg-3 col-sm-12">品牌代码</label>
                            <div class="col-lg-9 col-md-9 col-sm-12">
                                <asp:TextBox ID="inpInfoFormBrandName" runat="server" ClientIDMode="Static" class="form-control"
                                    MaxLength="32" title="品牌代码" placeholder="品牌代码最多32个字符。" />
                            </div>
                        </div>
                        <!--名称-->
                        <div class="form-group row k-margin-t-20">
                            <label class="col-form-label col-lg-3 col-sm-12">品牌名称</label>
                            <div class="col-lg-9 col-md-9 col-sm-12">
                                <asp:TextBox ID="inpInfoFormBrandTitle" runat="server" ClientIDMode="Static" class="form-control"
                                    MaxLength="32" title="品牌名称" placeholder="品牌名称最多32个字符。" />
                            </div>
                        </div>
                        <!--LOGO-->
                        <asp:HiddenField ID="hidInfoFormLogoFileID" Value="0" runat="server" />
                        <div class="form-group row k-margin-t-20">
                            <label class="col-form-label col-lg-3 col-sm-12">品牌LOGO</label>
                            <div class="col-lg-9 col-md-9 col-sm-12">
                                <div class="k-avatar k-avatar--outline k-avatar--success" id="k_profile_avatar_3">
                                    <div class="k-avatar__holder" style="background-image: url(/media/logos/logo_template.png)" id="divInfoFormLogo" runat="server"></div>
                                    <label class="k-avatar__upload" data-toggle="k-tooltip" title="更换LOGO">
                                        <i class="fa fa-pen"></i>
                                        <input type="file" name="profile_avatar" accept=".png, .jpg, .jpeg" id="inpInfoFormLogo" runat="server">
                                    </label>
                                    <span class="k-avatar__cancel" data-toggle="k-tooltip" title="清除LOGO">
                                        <i class="fa fa-times"></i>
                                    </span>
                                </div>
                            </div>
                        </div>
                        <!--Banner-->
                        <asp:HiddenField ID="hidInfoFormBannerFileID" Value="0" runat="server" />
                        <div class="form-group row k-margin-t-20">
                            <label class="col-form-label col-lg-3 col-sm-12">品牌Banner</label>
                            <div class="col-lg-9 col-md-9 col-sm-12">
                                <div class="k-avatar k-avatar--outline k-avatar--success" id="k_info_form_banner">
                                    <div class="k-avatar__holder banner_width" style="background-image: url(/media/logos/kind-banner.gif);" id="divInfoFormBanner" runat="server"></div>
                                    <label class="k-avatar__upload" data-toggle="k-tooltip" title="更换Banner">
                                        <i class="fa fa-pen"></i>
                                        <input type="file" name="profile_avatar" accept=".png, .jpg, .jpeg" id="inpInfoFormBanner" runat="server">
                                    </label>
                                    <span class="k-avatar__cancel" data-toggle="k-tooltip" title="清除Banner">
                                        <i class="fa fa-times"></i>
                                    </span>
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
    <!--begin: InfoDetails -->
    <div class="modal fade" id="DetailsModal" tabindex="-1" role="dialog" aria-labelledby="" aria-hidden="true">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">品牌[<asp:Label ID="txtDefailsModalTitle" runat="server" />]的详细信息</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true" class="la la-remove"></span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-12 col-lg-5 col-xl-4">
                            <div class="k-avatar k-avatar--outline k-avatar--success">
                                <div class="k-avatar__holder" style="background-image: url(/media/logos/logo_template.png)" id="divDetailsLogo" runat="server"></div>
                            </div>
                        </div>
                        <div class="col-md-12 col-lg-5 col-xl-4">
                            <h1 style="line-height: 120px;">
                                <span id="txtDetailsTitle" runat="server" />
                                <span class="badge badge-primary" id="txtDetailsName" runat="server" />
                            </h1>
                        </div>
                    </div>
                    <div class="row">
                        <h2 style="width: 100%;">商品列表</h2>
                        <asp:ListView ID="lvGoodsInfos" runat="server">
                            <LayoutTemplate>
                                <ul>
                                    <li id="itemPlaceholder" runat="server" />
                                </ul>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <li><%#Eval("Name")%>&nbsp;<%#Eval("Title")%></li>
                            </ItemTemplate>
                        </asp:ListView>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!--end: InfoDetails -->
    <!-- end:: Content -->
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PageFootPlaceHolder" runat="server">
    <script>
        $(document).ready(function () {
            //表单
            var FormsName = ['#InfoFormModal'];
            for (var i = 0; i < FormsName.length; i++) { ModalHeadScroll(FormsName[i]); };
            //文字
            $('#inpInfoFormBrandName, #inpInfoFormBrandTitle').maxlength({
                alwaysShow: true,
                threshold: 5,
                warningClass: "k-badge k-badge--primary k-badge--rounded k-badge--inline",
                limitReachedClass: "k-badge k-badge--brand k-badge--rounded k-badge--inline"
            });
            //图片
            new KAvatar('k_profile_avatar_3');
            new KAvatar('k_info_form_banner');
        });
    </script>
</asp:Content>
