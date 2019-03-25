<%@ Page Title="" Language="C#" MasterPageFile="~/UI/Keen.Master" AutoEventWireup="true" CodeBehind="ModuleInfo.aspx.cs" Inherits="DMS.MOD.SYS.ModuleInfo" %>

<%@ Register Src="~/UC/PagerControl.ascx" TagPrefix="uc1" TagName="PagerControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageHeadPlaceHolder" runat="server">
    <link href="/CSS/List.css" rel="stylesheet" type="text/css" />
    <script src="/JS/List.js" type="text/javascript"></script>
    <script>
        function ModuleFormCheck() {
            var inpNames = ['#inpInfoFormName', '#inpInfoFormTitle', '#inpInfoFormIconFont'];
            return FormCheck(inps);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageContentPlaceHolder" runat="server">
    <!-- begin:: Content -->
    <!--begin: List -->
    <div class="k-content  k-grid__item k-grid__item--fluid" id="k_content">
        <div class="k-portlet k-portlet--mobile">
            <div class="k-portlet__head">
                <div class="k-portlet__head-label">
                    <h3 class="k-portlet__head-title">模块信息列表</h3>
                </div>
                <div class="k-portlet__head-toolbar">
                    <div class="head-toolbar-button">
                        <div class="input-group">
                            <asp:TextBox ID="inpFilter" runat="server" CssClass="form-control" placeholder="模块名称、标题" ClientIDMode="Static" AutoPostBack="true" OnTextChanged="Filter_Change" />
                            <div class="input-group-append">
                                <button class="btn btn-primary head-toolbar-button-search" type="button" id="btnFilter" runat="server" onclick="if (!FilterCheck()) { return false; }" onserverclick="Filter_Click">
                                    <i class="fa fa-search"></i>
                                </button>
                            </div>
                        </div>
                    </div>
                    <div class="head-toolbar-button">
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
                                            <button type="button" class="btn btn-brand" id="btnMenuInit" onserverclick="ModuleInit_Click" runat="server">确认初始化</button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <a data-toggle="modal" data-target="#InfoFormModal" id="btnFormModal" style="display: none;"></a>
                        <a data-toggle="modal" data-target="#DetailsModal" id="btnListDetailsModal" style="display: none;"></a>
                    </div>
                </div>
            </div>
            <div class="k-portlet__body">
                <div id="m_table_2_wrapper" class="dataTables_wrapper dt-bootstrap4 no-footer">
                    <div class="row">
                        <!--begin: Datatable -->
                        <asp:ListView ID="ModuleInfoList" runat="server">
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
                                                <button id="ListOrderByIconFont" runat="server" onserverclick="OrderBy_Click">
                                                    <span class="list-field-title">图标</span>
                                                    <span class="list-orderby-none" id="ListOrderByIconFontAsc" runat="server">↓</span>
                                                    <span class="list-orderby-none" id="ListOrderByIconFontDesc" runat="server">↑</span>
                                                    <asp:HiddenField ID="ListOrderByIconFontValue" runat="server" />
                                                </button>
                                            </th>
                                            <th>
                                                <button id="ListOrderByName" runat="server" onserverclick="OrderBy_Click">
                                                    <span class="list-field-title">模块名称</span>
                                                    <span class="list-orderby-none" id="ListOrderByNameAsc" runat="server">↓</span>
                                                    <span class="list-orderby-none" id="ListOrderByNameDesc" runat="server">↑</span>
                                                    <asp:HiddenField ID="ListOrderByNameValue" runat="server" />
                                                </button>
                                            </th>
                                            <th>
                                                <button id="ListOrderByTitle" runat="server" onserverclick="OrderBy_Click">
                                                    <span class="list-field-title">模块标题</span>
                                                    <span class="list-orderby-none" id="ListOrderByTitleAsc" runat="server">↓</span>
                                                    <span class="list-orderby-none" id="ListOrderByTitleDesc" runat="server">↑</span>
                                                    <asp:HiddenField ID="ListOrderByTitleValue" runat="server" />
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
                                    <th><%#((Container.DataItemIndex + 1) + (ModuleInfoListPager.PageIndex * ModuleInfoListPager.PageSize - ModuleInfoListPager.PageSize))%></th>
                                    <td>
                                        <span><%#Eval("ID")%></span>
                                        <a class="btn btn-sm btn-clean btn-icon btn-icon-md" title="详情" moduleid='<%#Eval("ID")%>'
                                            id="btnListDetails" autopostback="true" onserverclick="ListDetails_Click" runat="server">
                                            <i class="la la-book"></i></a>
                                        <a class="btn btn-sm btn-clean btn-icon btn-icon-md" title="修改" moduleid='<%#Eval("ID")%>'
                                            id="btnListEdit" autopostback="true" onserverclick="ListEdit_Click" runat="server">
                                            <i class="la la-edit"></i></a>
                                    </td>
                                    <td><i class='<%#Eval("IconFont")%>'></i></td>
                                    <td><%#FilterFormat(inpFilter, (string)Eval("Name"))%></td>
                                    <td><%#FilterFormat(inpFilter, (string)Eval("Title"))%></td>
                                </tr>
                            </ItemTemplate>
                        </asp:ListView>
                        <!--end: Datatable -->
                    </div>
                    <div class="row">
                        <uc1:PagerControl ID="ModuleInfoListPager" PageSize="10" ClientIDMode="Static" runat="server" />
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
                <div class="modal-header">
                    <h5 class="modal-title">模块[<asp:Label ID="txtInfoFormTitle" runat="server" />]的信息表单</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true" class="la la-remove"></span>
                    </button>
                    <asp:HiddenField ID="hidInfoFormModuleID" Value="0" runat="server" />
                </div>
                <div class="modal-body">
                    <div class="k-form k-form--label-right">
                        <!--名称-->
                        <div class="form-group row k-margin-t-20">
                            <label class="col-form-label col-lg-3 col-sm-12">名称</label>
                            <div class="col-lg-9 col-md-9 col-sm-12">
                                <input type="text" class="form-control" maxlength="16" title="名称"
                                    placeholder="名称最多16个字符。" id="inpInfoFormName" clientidmode="static" runat="server" />
                            </div>
                        </div>
                        <!--标题-->
                        <div class="form-group row k-margin-t-20">
                            <label class="col-form-label col-lg-3 col-sm-12">标题</label>
                            <div class="col-lg-9 col-md-9 col-sm-12">
                                <input type="text" class="form-control" maxlength="16" title="标题"
                                    placeholder="标题最多16个字符。" id="inpInfoFormTitle" clientidmode="static" runat="server" />
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
                    <div style="width: 100%; padding-left: 20%;">
                        <asp:Button ID="btnInfoFormSubmit" runat="server" Text="保存" class="btn btn-brand k-btn"
                            OnClientClick="if (!ModuleFormCheck()) { return false; }" OnClick="InfoFormSubmit_Click" />
                        <div style="width: 120px; margin-left: 60px; display: inline-block;">
                            <button type="button" class="btn btn-secondary k-btn" data-dismiss="modal">放弃</button>
                        </div>
                    </div>
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
                    <h5 class="modal-title">模块[<asp:Label ID="txtDetailsModalTitle" runat="server" />]的详细信息</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true" class="la la-remove"></span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-12 col-lg-5 col-xl-4">
                            <span id="txtDetailsIconFnot" runat="server" />
                            <span id="txtDetailsTitle" runat="server" />（<span id="txtDetailsName" runat="server" />）
                            <asp:ListView ID="lvActionInfos" runat="server">
                                <LayoutTemplate>
                                    <ul>
                                        <li id="itemPlaceholder" runat="server" />
                                    </ul>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <li><i class='<%#Eval("IconFont")%>'></i><%#Eval("Title")%>（<%#Eval("Name")%>）</li>
                                </ItemTemplate>
                            </asp:ListView>
                        </div>
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
            //文字
            var inpNames = ['#inpInfoFormName', '#inpInfoFormTitle', '#inpInfoFormIconFont'];
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
