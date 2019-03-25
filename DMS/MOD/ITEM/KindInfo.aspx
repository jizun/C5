<%@ Page Title="" Language="C#" MasterPageFile="~/UI/Keen.Master" AutoEventWireup="true" CodeBehind="KindInfo.aspx.cs" Inherits="DMS.MOD.ITEM.KindInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageHeadPlaceHolder" runat="server">
    <link href="/CSS/List.css" rel="stylesheet" type="text/css" />
    <style>
        .kind-ul li {
            padding: 5px;
            border-radius: 3px;
        }

            .kind-ul li:hover {
                background: rgba(0,0,0,0.05);
            }
    </style>
    <script src="/JS/List.js" type="text/javascript"></script>
    <script>
        function KindFormCheck() {
            var inps = ['inpInfoFormKindName', 'inpInfoFormKindTitle'];
            return FormCheck(inps);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageContentPlaceHolder" runat="server">
    <!-- begin:: Content -->
    <!-- begin: List -->
    <div class="k-portlet">
        <div class="k-portlet__head">
            <div class="k-portlet__head-label">
                <h3 class="k-portlet__head-title">品类信息</h3>
            </div>
            <div class="k-portlet__head-toolbar">
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
                                        <button type="button" class="btn btn-brand" id="btnKindInit" onserverclick="Init_Click" runat="server">确认初始化</button>
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
            <!--begin: KindRoot -->
            <asp:ListView ID="lvKind" runat="server">
                <LayoutTemplate>
                    <ul class="kind-ul">
                        <li id="itemPlaceholder" runat="server" />
                    </ul>
                </LayoutTemplate>
                <ItemTemplate>
                    <li>
                        <asp:HiddenField ID="lvKindID" Value='<%#Eval("ID")%>' runat="server" />
                        <span>[<%#Eval("OrderBy")%>]</span>
                        <span style="font-weight: bold;"><%#Eval("Title")%></span><span style="display: none;">（<%#Eval("Name")%>）</span>
                        <span class="dropdown">&nbsp;
                            <a href="#" class="btn btn-sm btn-clean btn-icon btn-icon-md" data-toggle="dropdown" aria-expanded="true">
                                <%#((bool)Eval("Enabled"))
                                    ? "<span class=\"badge badge-success list-enabled\">启用</span>"
                                    : "<span class=\"badge badge-danger list-enabled\">禁用</span>"
                                %>
                            </a>
                            <div class="dropdown-menu dropdown-menu-right">
                                <a class="dropdown-item" kindid='<%#Eval("ID")%>' id="btnListEnabled" runat="server"
                                    onserverclick="ListEnabled_Click"><i class="la la-angellist"></i>启用</a>
                                <a class="dropdown-item" kindid='<%#Eval("ID")%>' id="btnListDisabled" runat="server"
                                    onserverclick="ListEnabled_Click"><i class="la la-lock"></i>禁用</a>
                            </div>
                        </span>
                        <a class="btn btn-sm btn-clean btn-icon btn-icon-md" title="详情" kindid='<%#Eval("ID")%>'
                            id="btnListDetails" autopostback="true" onserverclick="ListDetails_Click" runat="server">
                            <i class="la la-book"></i></a>
                        <a class="btn btn-sm btn-clean btn-icon btn-icon-md" title="修改" kindid='<%#Eval("ID")%>'
                            id="btnListEdit" autopostback="true" onserverclick="ListEdit_Click" runat="server">
                            <i class="la la-edit"></i></a>
                        <a class="btn btn-sm btn-clean btn-icon btn-icon-md" title='<%#"增加[" + Eval("Title") + "]的子类"%>' parentid='<%#Eval("ID")%>'
                            id="btnListAddNew" autopostback="true" onserverclick="ListAddNew_Click" runat="server">
                            <i class="la la-plus-circle"></i></a>
                        <!--begin: KindL1 -->
                        <asp:ListView ID="lvKind" runat="server">
                            <LayoutTemplate>
                                <ul>
                                    <li id="itemPlaceholder" runat="server" />
                                </ul>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <li>
                                    <asp:HiddenField ID="lvKindID" Value='<%#Eval("ID")%>' runat="server" />
                                    <span>[<%#Eval("OrderBy")%>]</span>
                                    <span><%#Eval("Title")%></span><span style="display: none;">（<%#Eval("Name")%>）</span>
                                    <span class="dropdown">&nbsp;
                                        <a href="#" class="btn btn-sm btn-clean btn-icon btn-icon-md" data-toggle="dropdown" aria-expanded="true">
                                            <%#((bool)Eval("Enabled"))
                                                ? "<span class=\"badge badge-success list-enabled\">启用</span>"
                                                : "<span class=\"badge badge-danger list-enabled\">禁用</span>"
                                            %>
                                        </a>
                                        <div class="dropdown-menu dropdown-menu-right">
                                            <a class="dropdown-item" kindid='<%#Eval("ID")%>' id="btnListEnabled" runat="server"
                                                onserverclick="ListEnabled_Click"><i class="la la-angellist"></i>启用</a>
                                            <a class="dropdown-item" kindid='<%#Eval("ID")%>' id="btnListDisabled" runat="server"
                                                onserverclick="ListEnabled_Click"><i class="la la-lock"></i>禁用</a>
                                        </div>
                                    </span>
                                    <a class="btn btn-sm btn-clean btn-icon btn-icon-md" title="详情" kindid='<%#Eval("ID")%>'
                                        id="btnListDetails" autopostback="true" onserverclick="ListDetails_Click" runat="server">
                                        <i class="la la-book"></i></a>
                                    <a class="btn btn-sm btn-clean btn-icon btn-icon-md" title="修改" kindid='<%#Eval("ID")%>'
                                        id="btnListEdit" autopostback="true" onserverclick="ListEdit_Click" runat="server">
                                        <i class="la la-edit"></i></a>
                                    <!--begin: KindL2 -->

                                    <!--end: KindL2 -->
                                </li>
                            </ItemTemplate>
                        </asp:ListView>
                        <!--end: KindL1 -->
                    </li>
                </ItemTemplate>
            </asp:ListView>
            <!--end: KindRoot -->
        </div>
    </div>
    <!--end: List -->
    <!--begin: InfoForm -->
    <div class="modal fade" id="InfoFormModal" tabindex="-1" role="dialog" aria-labelledby="" aria-hidden="true">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header" id="InfoFormModalHead">
                    <h5 class="modal-title">品类[<asp:Label ID="txtInfoFormTitle" runat="server" />]的信息表单</h5>
                    <div class="k-portlet__head-toolbar">
                        <a href="#" class="btn btn-secondary k-margin-r-10" data-dismiss="modal" aria-label="Close">
                            <span class="k-hidden-mobile">返回</span>
                        </a>
                        <div class="btn-group">
                            <button type="button" class="btn btn-primary" id="btnInfoFormSubmit" runat="server"
                                onclick="if (!KindFormCheck()) { return false; }" onserverclick="InfoFormSubmit_Click">
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
                    <asp:HiddenField ID="hidInfoFormKindID" Value="0" runat="server" />
                    <div class="k-form k-form--label-right">
                        <!--父类-->
                        <asp:HiddenField ID="hidInfoFormParentID" Value="0" runat="server" />
                        <div class="form-group row k-margin-t-20">
                            <label class="col-form-label col-lg-3 col-sm-12">父类</label>
                            <div class="col-lg-9 col-md-9 col-sm-12">
                                <asp:Label ID="txtInfoFormParent" runat="server" class="badge badge-info" Style="margin-top: 8px;" />
                            </div>
                        </div>
                        <!--代码-->
                        <div class="form-group row k-margin-t-20">
                            <label class="col-form-label col-lg-3 col-sm-12">顺序</label>
                            <div class="col-lg-9 col-md-9 col-sm-12">
                                <asp:TextBox ID="inpInfoFormOrderBy" runat="server" ClientIDMode="Static"
                                    Text="99" class="form-control bootstrap-touchspin-vertical-btn inpOrderBy" />
                            </div>
                        </div>
                        <!--代码-->
                        <div class="form-group row k-margin-t-20">
                            <label class="col-form-label col-lg-3 col-sm-12">代码</label>
                            <div class="col-lg-9 col-md-9 col-sm-12">
                                <asp:TextBox ID="inpInfoFormKindName" runat="server" ClientIDMode="Static" class="form-control"
                                    MaxLength="32" title="品类代码" placeholder="品类代码最多32个字符。" />
                            </div>
                        </div>
                        <!--名称-->
                        <div class="form-group row k-margin-t-20">
                            <label class="col-form-label col-lg-3 col-sm-12">名称</label>
                            <div class="col-lg-9 col-md-9 col-sm-12">
                                <asp:TextBox ID="inpInfoFormKindTitle" runat="server" ClientIDMode="Static" class="form-control"
                                    MaxLength="32" title="品类名称" placeholder="品类名称最多32个字符。" />
                            </div>
                        </div>
                        <!--LOGO-->
                        <asp:HiddenField ID="hidInfoFormLogoFileID" Value="0" runat="server" />
                        <div class="form-group row k-margin-t-20">
                            <label class="col-form-label col-lg-3 col-sm-12">LOGO</label>
                            <div class="col-lg-9 col-md-9 col-sm-12">
                                <div class="k-avatar k-avatar--outline k-avatar--success" id="k_profile_avatar_3">
                                    <div class="k-avatar__holder" style="background-image: url(/media/logos/kind_template.png)" id="divInfoFormLogo" runat="server"></div>
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
                    <h5 class="modal-title">品类[<asp:Label ID="txtDefailsModalTitle" runat="server" />]的详细信息</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true" class="la la-remove"></span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-12 col-lg-5 col-xl-4">
                            <div class="k-avatar k-avatar--outline k-avatar--success">
                                <div class="k-avatar__holder" style="background-image: url(/media/logos/kind_template.png)" id="divDetailsLogo" runat="server"></div>
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
            $('#inpInfoFormKindName, #inpInfoFormKindTitle').maxlength({
                alwaysShow: true,
                threshold: 5,
                warningClass: "k-badge k-badge--primary k-badge--rounded k-badge--inline",
                limitReachedClass: "k-badge k-badge--brand k-badge--rounded k-badge--inline"
            });
            //数值
            $('.inpOrderBy').TouchSpin({
                buttondown_class: 'btn btn-secondary',
                buttonup_class: 'btn btn-secondary',
                verticalbuttons: true,
                verticalupclass: 'la la-plus',
                verticaldownclass: 'la la-minus',

                min: 0,
                max: 1000000,
                step: 1,
            });
            //图片
            var avatar3 = new KAvatar('k_profile_avatar_3');
        });
    </script>
</asp:Content>
