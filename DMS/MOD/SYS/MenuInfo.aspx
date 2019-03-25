<%@ Page Title="" Language="C#" MasterPageFile="~/UI/Keen.Master" AutoEventWireup="true" CodeBehind="MenuInfo.aspx.cs" Inherits="DMS.MOD.SYS.MenuInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageHeadPlaceHolder" runat="server">
    <link href="/CSS/List.css" rel="stylesheet" type="text/css" />
    <script src="/JS/List.js" type="text/javascript"></script>
    <script>
        function MenuFormCheck() {
            var inps = ['inpInfoFormName', 'inpInfoFormTitle', 'inpInfoFormIconFont', 'inpInfoFormURL'];
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
                    <h3 class="k-portlet__head-title">菜单信息</h3>
                </div>
                <div class="k-portlet__head-toolbar">
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
                                        <p>初始化将导致菜单信息恢复原始状态，确认继续吗？</p>
                                    </div>
                                    <div class="modal-footer">
                                        <div style="width: 100%; text-align: center;">
                                            <button type="button" class="btn btn-secondary" data-dismiss="modal">取消</button>
                                            <button type="button" class="btn btn-brand" id="btnMenuInit" onserverclick="MenuInit_Click" runat="server">确认初始化</button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <a data-toggle="modal" data-target="#form_modal" id="btnFormModal" style="display: none;"></a>
                    </div>
                </div>
            </div>
            <div class="k-portlet__body">
                <div id="m_table_2_wrapper" class="dataTables_wrapper dt-bootstrap4 no-footer">
                    <div class="row">
                        <!--begin: MenuGroupList -->
                        <asp:ListView ID="lvMenuGroups" runat="server">
                            <LayoutTemplate>
                                <ul>
                                    <li id="itemPlaceholder" runat="server" />
                                </ul>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <li>
                                    <asp:HiddenField ID="lvMenuGroupID" Value='<%#Eval("GroupID")%>' runat="server" />
                                    <span><%#Eval("GroupTitle")%></span><span style="display: none;">（<%#Eval("GroupName")%>）</span>
                                    <!--begin: MenuRootList -->
                                    <asp:ListView ID="lvMenuRoots" runat="server">
                                        <LayoutTemplate>
                                            <ul>
                                                <li id="itemPlaceholder" runat="server" />
                                            </ul>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <li>
                                                <asp:HiddenField ID="lvMenuRootID" Value='<%#Eval("ID")%>' runat="server" />
                                                <span>[<%#Eval("OrderBy")%>]</span>
                                                <i class="k-menu__link-icon <%#Eval("IconFont")%>"></i>
                                                <span style="font-weight:bold;"><%#Eval("Title")%></span><span style="display: none;">（<%#Eval("Name")%>）</span>
                                                <span class="dropdown">
                                                    &nbsp;
                                                    <a href="#" class="btn btn-sm btn-clean btn-icon btn-icon-md" data-toggle="dropdown" aria-expanded="true">
                                                        <%#((bool)Eval("Enabled"))
                                                            ? "<span class=\"badge badge-success list-enabled\">启用</span>"
                                                            : "<span class=\"badge badge-danger list-enabled\">禁用</span>"
                                                            %>
                                                    </a>
                                                    <div class="dropdown-menu dropdown-menu-right">
                                                        <a class="dropdown-item" menuid='<%#Eval("ID")%>' id="btnListEnabled" runat="server"
                                                            onserverclick="ListEnabled_Click"><i class="la la-angellist"></i>启用</a>
                                                        <a class="dropdown-item" menuid='<%#Eval("ID")%>' id="btnListDisabled" runat="server"
                                                            onserverclick="ListEnabled_Click"><i class="la la-lock"></i>禁用</a>
                                                    </div>
                                                </span>
                                                <span><%#Eval("URL")%></span>
                                                <a class="btn btn-sm btn-clean btn-icon btn-icon-md" title="修改" menuid='<%#Eval("ID")%>'
                                                    id="btnListEdit" autopostback="true" onserverclick="ListEdit_Click" runat="server">
                                                    <i class="la la-edit"></i></a>
                                                <!--begin: MenuL1List -->
                                                <asp:ListView ID="lvMenuL1" runat="server">
                                                    <LayoutTemplate>
                                                        <ul>
                                                            <li id="itemPlaceholder" runat="server" />
                                                        </ul>
                                                    </LayoutTemplate>
                                                    <ItemTemplate>
                                                        <li>
                                                            <asp:HiddenField ID="lvMenuL1ID" Value='<%#Eval("ID")%>' runat="server" />
                                                            <span>[<%#Eval("OrderBy")%>]</span>
                                                            <i class="k-menu__link-icon <%#Eval("IconFont")%>"></i>
                                                            <span><%#Eval("Title")%></span><span style="display: none;">（<%#Eval("Name")%>）</span>
                                                            <span class="dropdown">
                                                                &nbsp;
                                                                <a href="#" class="btn btn-sm btn-clean btn-icon btn-icon-md" data-toggle="dropdown" aria-expanded="true">
                                                                    <%#((bool)Eval("Enabled"))
                                                                        ? "<span class=\"badge badge-success list-enabled\">启用</span>"
                                                                        : "<span class=\"badge badge-danger list-enabled\">禁用</span>"
                                                                        %>
                                                                </a>
                                                                <div class="dropdown-menu dropdown-menu-right">
                                                                    <a class="dropdown-item" menuid='<%#Eval("ID")%>' id="btnListEnabled" runat="server"
                                                                        onserverclick="ListEnabled_Click"><i class="la la-angellist"></i>启用</a>
                                                                    <a class="dropdown-item" menuid='<%#Eval("ID")%>' id="btnListDisabled" runat="server"
                                                                        onserverclick="ListEnabled_Click"><i class="la la-lock"></i>禁用</a>
                                                                </div>
                                                            </span>
                                                            <span><%#Eval("URL")%></span>
                                                            <a class="btn btn-sm btn-clean btn-icon btn-icon-md" title="修改" menuid='<%#Eval("ID")%>'
                                                                id="btnListEdit" autopostback="true" onserverclick="ListEdit_Click" runat="server">
                                                                <i class="la la-edit"></i></a>
                                                            <!--begin: MenuL2List -->
                                                            <asp:ListView ID="lvMenuL2" runat="server">
                                                                <LayoutTemplate>
                                                                    <ul>
                                                                        <li id="itemPlaceholder" runat="server" />
                                                                    </ul>
                                                                </LayoutTemplate>
                                                                <ItemTemplate>
                                                                    <li>
                                                                        <asp:HiddenField ID="lvMenuL2ID" Value='<%#Eval("ID")%>' runat="server" />
                                                                        <span>[<%#Eval("OrderBy")%>]</span>
                                                                        <i class="k-menu__link-icon <%#Eval("IconFont")%>"></i>
                                                                        <span><%#Eval("Title")%></span><span style="display: none;">（<%#Eval("Name")%>）</span>
                                                                        <span class="dropdown">
                                                                            &nbsp;
                                                                            <a href="#" class="btn btn-sm btn-clean btn-icon btn-icon-md" data-toggle="dropdown" aria-expanded="true">
                                                                                <%#((bool)Eval("Enabled"))
                                                                                    ? "<span class=\"badge badge-success list-enabled\">启用</span>"
                                                                                    : "<span class=\"badge badge-danger list-enabled\">禁用</span>"
                                                                                    %>
                                                                            </a>
                                                                            <div class="dropdown-menu dropdown-menu-right">
                                                                                <a class="dropdown-item" menuid='<%#Eval("ID")%>' id="btnListEnabled" runat="server"
                                                                                    onserverclick="ListEnabled_Click"><i class="la la-angellist"></i>启用</a>
                                                                                <a class="dropdown-item" menuid='<%#Eval("ID")%>' id="btnListDisabled" runat="server"
                                                                                    onserverclick="ListEnabled_Click"><i class="la la-lock"></i>禁用</a>
                                                                            </div>
                                                                        </span>
                                                                        <a class="btn btn-sm btn-clean btn-icon btn-icon-md" title="修改" menuid='<%#Eval("ID")%>'
                                                                            id="btnListEdit" autopostback="true" onserverclick="ListEdit_Click" runat="server">
                                                                            <i class="la la-edit"></i></a>
                                                                        <a href="javascript:parent.AURLTab('<%#Eval("Title")%>','<%#Eval("URL")%>');"><%#Eval("URL")%></a>
                                                                    </li>
                                                                </ItemTemplate>
                                                            </asp:ListView>
                                                            <!--end: MenuL2List -->
                                                        </li>
                                                    </ItemTemplate>
                                                </asp:ListView>
                                                <!--end: MenuL1List -->
                                            </li>
                                        </ItemTemplate>
                                    </asp:ListView>
                                    <!--end: MenuRootList -->
                                </li>
                            </ItemTemplate>
                        </asp:ListView>
                        <!--end: MenuGroupList -->
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!--end: List -->
    <!--begin: Form -->
    <div class="modal fade" id="form_modal" tabindex="-1" role="dialog" aria-labelledby="" aria-hidden="true">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">菜单[<asp:Label ID="txtFormTitle" runat="server" />]的信息表单</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true" class="la la-remove"></span>
                    </button>
                    <asp:HiddenField ID="hidInfoFormMenuID" Value="0" runat="server" />
                </div>
                <div class="modal-body">
                    <div class="k-form k-form--label-right">
                        <!--顺序-->
                        <div class="form-group row k-margin-t-20">
                            <label class="col-form-label col-lg-3 col-sm-12">顺序</label>
                            <div class="col-lg-9 col-md-9 col-sm-12">
                                <input type="text" class="form-control" id="inpInfoFormOrderBy" clientidmode="static" runat="server" />
                            </div>
                        </div>
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
                        <!--链接-->
                        <div class="form-group row k-margin-t-20">
                            <label class="col-form-label col-lg-3 col-sm-12">链接</label>
                            <div class="col-lg-9 col-md-9 col-sm-12">
                                <input type="text" class="form-control" maxlength="256" title="链接"
                                    placeholder="链接最多256个字符。" id="inpInfoFormURL" clientidmode="static" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <div style="width: 100%; padding-left: 20%;">
                        <asp:Button ID="btnInfoFormSubmit" runat="server" Text="保存" class="btn btn-brand k-btn"
                            OnClientClick="if (!MenuFormCheck()) { return false; }" OnClick="InfoFormSubmit_Click" />
                        <div style="width: 120px; margin-left: 60px; display: inline-block;">
                            <button type="button" class="btn btn-secondary k-btn" data-dismiss="modal">放弃</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!--end: Form -->
    <!-- end:: Content -->
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PageFootPlaceHolder" runat="server">
    <script>
        $(document).ready(function () {
            //文字
            var inpNames = ['#inpInfoFormName', '#inpInfoFormTitle', '#inpInfoFormIconFont', '#inpInfoFormURL'];
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
