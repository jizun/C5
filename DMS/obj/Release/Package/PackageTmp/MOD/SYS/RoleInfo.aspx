<%@ Page Title="" Language="C#" MasterPageFile="~/UI/Keen.Master" AutoEventWireup="true" CodeBehind="RoleInfo.aspx.cs" Inherits="DMS.MOD.SYS.RoleInfo" %>

<%@ Register Src="~/UC/PagerControl.ascx" TagPrefix="uc1" TagName="PagerControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageHeadPlaceHolder" runat="server">
    <link href="/CSS/List.css" rel="stylesheet" type="text/css" />
    <script src="/JS/List.js" type="text/javascript"></script>
    <script>
        function RoleFormCheck() {
            var inps = ['inpInfoFormRoleName', 'inpInfoFormRoleTitle'];
            if (FormCheck(inps)) { return true; };
            return false;
        }
    </script>
    <style>
        .z-switch label {
            margin: 0px !important;
        }

        .card-header label {
            margin: 0px 0px -6px 0px !important;
        }

        .module_auth_li {
            line-height: 40px;
        }

            .module_auth_li label {
                margin: 0px 0px -9px 0px !important;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageContentPlaceHolder" runat="server">
    <!-- begin:: Content -->
    <!--begin: List -->
    <div class="k-content  k-grid__item k-grid__item--fluid" id="k_content">
        <div class="k-portlet k-portlet--mobile">
            <div class="k-portlet__head">
                <div class="k-portlet__head-label">
                    <h3 class="k-portlet__head-title">角色信息列表</h3>
                </div>
                <div class="k-portlet__head-toolbar">
                    <div class="head-toolbar-button">
                        <div class="input-group">
                            <asp:TextBox ID="inpFilter" runat="server" CssClass="form-control" placeholder="角色名称" ClientIDMode="Static" AutoPostBack="true" OnTextChanged="Filter_Change" />
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
                                        <p>初始化将导致角色信息恢复原始状态，确认继续吗？</p>
                                    </div>
                                    <div class="modal-footer">
                                        <div style="width: 100%; text-align: center;">
                                            <button type="button" class="btn btn-secondary" data-dismiss="modal">取消</button>
                                            <button type="button" class="btn btn-brand" id="btnMenuInit" onserverclick="RoleInit_Click" runat="server">确认初始化</button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <a data-toggle="modal" data-target="#InfoFormModal" id="btnListInfoFormModal" style="display: none;"></a>
                        <a data-toggle="modal" data-target="#DetailsModal" id="btnListDetailsModal" style="display: none;"></a>
                        <a data-toggle="modal" data-target="#ModuleAuthFormModal" id="btnListModuleAuthModal" style="display: none;"></a>
                    </div>
                </div>
            </div>
            <div class="k-portlet__body">
                <div id="m_table_2_wrapper" class="dataTables_wrapper dt-bootstrap4 no-footer">
                    <div class="row">
                        <!--begin: Datatable -->
                        <asp:ListView ID="RoleInfoList" runat="server">
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
                                                    <span class="list-field-title">角色代码</span>
                                                    <span class="list-orderby-none" id="ListOrderByNameAsc" runat="server">↓</span>
                                                    <span class="list-orderby-none" id="ListOrderByNameDesc" runat="server">↑</span>
                                                    <asp:HiddenField ID="ListOrderByNameValue" runat="server" />
                                                </button>
                                            </th>
                                            <th>
                                                <button id="ListOrderByTitle" runat="server" onserverclick="OrderBy_Click">
                                                    <span class="list-field-title">角色名称</span>
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
                                    <th><%#((Container.DataItemIndex + 1) + (RoleInfoListPager.PageIndex * RoleInfoListPager.PageSize - RoleInfoListPager.PageSize))%></th>
                                    <td>
                                        <span><%#Eval("ID")%></span>
                                        <a class="btn btn-sm btn-clean btn-icon btn-icon-md" title="详情" roleid='<%#Eval("ID")%>'
                                            id="btnListDetails" autopostback="true" onserverclick="ListDetails_Click" runat="server">
                                            <i class="la la-book"></i></a>
                                        <a class="btn btn-sm btn-clean btn-icon btn-icon-md" title="修改" roleid='<%#Eval("ID")%>'
                                            id="btnListEdit" autopostback="true" onserverclick="ListEdit_Click" runat="server">
                                            <i class="la la-edit"></i></a>
                                        <a class="btn btn-sm btn-clean btn-icon btn-icon-md" title="模块授权" roleid='<%#Eval("ID")%>'
                                            id="btnListModuleAuth" autopostback="true" onserverclick="ListModuleAuth_Click" runat="server">
                                            <i class="flaticon-list-3"></i></a>
                                    </td>
                                    <td><%#FilterFormat(inpFilter, (string)Eval("Name"))%></td>
                                    <td><%#FilterFormat(inpFilter, (string)Eval("Title"))%></td>
                                    <td>
                                        <span class="k-switch k-switch--sm k-switch--icon z-switch">
                                            <label>
                                                <input type="checkbox" checked='<%#Eval("Enabled")%>' roleid='<%#Eval("ID")%>'
                                                    id="inpActionAuth" runat="server" onclick="__doPostBack(this.id, '')" onserverchange="ListEnabled_Change" />
                                                <span></span>
                                            </label>
                                        </span>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:ListView>
                        <!--end: Datatable -->
                    </div>
                    <div class="row">
                        <uc1:PagerControl ID="RoleInfoListPager" PageSize="10" ClientIDMode="Static" runat="server" />
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
                    <h5 class="modal-title">角色[<asp:Label ID="txtInfoFormTitle" runat="server" />]的信息表单</h5>
                    <div class="k-portlet__head-toolbar">
                        <a href="#" class="btn btn-secondary k-margin-r-10" data-dismiss="modal" aria-label="Close">
                            <span class="k-hidden-mobile">返回</span>
                        </a>
                        <div class="btn-group">
                            <button type="button" class="btn btn-primary" id="btnInfoFormSubmit" runat="server"
                                onclick="if (!RoleFormCheck()) { return false; }" onserverclick="InfoFormSubmit_Click">
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
                    <asp:HiddenField ID="hidInfoFormRoleID" Value="0" runat="server" />
                    <div class="k-form k-form--label-right">
                        <!--角色代码-->
                        <div class="form-group row k-margin-t-20">
                            <label class="col-form-label col-lg-3 col-sm-12">角色代码</label>
                            <div class="col-lg-9 col-md-9 col-sm-12">
                                <asp:TextBox ID="inpInfoFormRoleName" runat="server" ClientIDMode="Static" class="form-control"
                                    MaxLength="16" title="角色代码" placeholder="角色代码最多16个字符。" />
                            </div>
                        </div>
                        <!--角色名称-->
                        <div class="form-group row k-margin-t-20">
                            <label class="col-form-label col-lg-3 col-sm-12">角色名称</label>
                            <div class="col-lg-9 col-md-9 col-sm-12">
                                <asp:TextBox ID="inpInfoFormRoleTitle" runat="server" ClientIDMode="Static" class="form-control"
                                    MaxLength="16" title="角色名称" placeholder="角色名称最多16个字符。" />
                            </div>
                        </div>
                        <!--图标-->
                        <div class="form-group row k-margin-t-20">
                            <label class="col-form-label col-lg-3 col-sm-12">图标</label>
                            <div class="col-lg-9 col-md-9 col-sm-12">
                                <input type="text" class="form-control" maxlength="32" title="图标"
                                    placeholder="图标最多32个字符。" id="inpInfoFormIconFont" clientidmode="static" runat="server" />
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
                    <h5 class="modal-title">[<asp:Label ID="txtDefailsModalTitle" runat="server" />]角色的详细信息</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true" class="la la-remove"></span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-12 col-lg-5 col-xl-4">
                            <span id="txtDetailsIconFnot" runat="server" />
                            <span id="txtDetailsTitle" runat="server" />（<span id="txtDetailsName" runat="server" />）
                            <asp:ListView ID="lvRoleUserInfos" runat="server">
                                <LayoutTemplate>
                                    <ul>
                                        <li id="itemPlaceholder" runat="server" />
                                    </ul>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <li><%#Eval("Name")%></li>
                                </ItemTemplate>
                            </asp:ListView>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!--end: InfoDetails -->
    <!--begin: ModuleAuthFormModal -->
    <div class="modal fade" id="ModuleAuthFormModal" tabindex="-1" role="dialog" aria-labelledby="" aria-hidden="true">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header" id="ModuleAuthFormModalHead">
                    <h5 class="modal-title">角色[<asp:Label ID="txtModuleAuthRoleTitle" runat="server" />]的模块权限</h5>
                    <div class="k-portlet__head-toolbar">
                        <a href="#" class="btn btn-secondary k-margin-r-10" data-dismiss="modal" aria-label="Close">
                            <span class="k-hidden-mobile">返回</span>
                        </a>
                        <div class="btn-group">
                            <button type="button" class="btn btn-primary" id="btnAuthFormSubmit" runat="server" onserverclick="AuthFormSubmit_Click">
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
                    <!--begin::Accordion-->
                    <asp:HiddenField ID="hidModuleAuthRoleID" Value="0" runat="server" />
                    <asp:ListView ID="lvModuleList" runat="server">
                        <LayoutTemplate>

                            <div class="accordion accordion-outline" id="accordionExample3">
                                <div id="itemPlaceholder" runat="server" />
                            </div>

                        </LayoutTemplate>
                        <ItemTemplate>
                            <div class="card">
                                <div class="card-header" id="headingOne<%#Eval("ID")%>">
                                    <div class="card-title collapsed" data-toggle="collapse" data-target="#collapseOne<%#Eval("ID")%>" aria-expanded="false" aria-controls="collapseOne<%#Eval("ID")%>">
                                        <span><i class='<%#Eval("IconFont")%>'></i>&nbsp<%#Eval("Title")%>模块</span><span style="display: none;">（<%#Eval("Name")%>）</span>
                                        <span class="k-switch k-switch--sm k-switch--icon">
                                            <label>
                                                <input type="checkbox" checked='<%#Eval("RoleAuth")%>' moduleid='<%#Eval("ID")%>' id="inpModuleAuth" runat="server">
                                                <span></span>
                                            </label>
                                        </span>
                                    </div>
                                </div>
                                <div id="collapseOne<%#Eval("ID")%>" class="card-body-wrapper collapse" aria-labelledby="headingOne3" data-parent="#accordionExample3">
                                    <div class="card-body">
                                        <asp:ListView ID="lvActionList" runat="server">
                                            <LayoutTemplate>
                                                <ul>
                                                    <li id="itemPlaceholder" runat="server" />
                                                </ul>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <li class="module_auth_li">
                                                    <span><i class='<%#Eval("IconFont")%>'></i>&nbsp;<%#Eval("Title")%></span><span style="display: none;">（<%#Eval("Name")%>）</span>
                                                    <span class="k-switch k-switch--sm k-switch--icon">
                                                        <label>
                                                            <input type="checkbox" checked='<%#Eval("RoleAuth")%>' actionid='<%#Eval("ID")%>' id="inpActionAuth" runat="server">
                                                            <span></span>
                                                        </label>
                                                    </span>
                                                </li>
                                            </ItemTemplate>
                                        </asp:ListView>
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:ListView>
                    <!--end::Accordion-->
                </div>
                <div class="modal-footer">
                </div>
            </div>
        </div>
    </div>
    <!--end: ModuleAuthFormModal -->
    <!-- end:: Content -->
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PageFootPlaceHolder" runat="server">
    <script>
        $(document).ready(function () {
            //表单
            var FormsName = ['#InfoFormModal', '#ModuleAuthFormModal'];
            for (var i = 0; i < FormsName.length; i++) { ModalHeadScroll(FormsName[i]); };
            //文字
            var inpNames = ['#inpInfoFormRoleName', '#inpInfoFormRoleTitle'];
            for (var i = 0; i < inpNames.length; i++) {
                $(inpNames[i]).maxlength({
                    alwaysShow: true,
                    threshold: 5,
                    warningClass: "k-badge k-badge--primary k-badge--rounded k-badge--inline",
                    limitReachedClass: "k-badge k-badge--brand k-badge--rounded k-badge--inline"
                });
            }
        });
    </script>
</asp:Content>
