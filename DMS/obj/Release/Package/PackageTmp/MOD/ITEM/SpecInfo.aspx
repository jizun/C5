<%@ Page Title="" Language="C#" MasterPageFile="~/UI/Keen.Master" AutoEventWireup="true" CodeBehind="SpecInfo.aspx.cs" Inherits="DMS.MOD.ITEM.SpecInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageHeadPlaceHolder" runat="server">
    <link href="/CSS/List.css" rel="stylesheet" type="text/css" />
    <script src="/JS/List.js" type="text/javascript"></script>
    <script>
        //表单提交检查
        function SpecFormCheck() {
            var inps = ['inpInfoFormSpecName', 'inpInfoFormSpecTitle'];
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
                <h3 class="k-portlet__head-title">规格信息</h3>
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
                                        <button type="button" class="btn btn-brand" id="btnMenuInit" onserverclick="Init_Click" runat="server">确认初始化</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <a data-toggle="modal" data-target="#InfoFormModal" id="btnListInfoFormModal" style="display: none;"></a>
                </div>
            </div>
        </div>
        <div class="k-portlet__body">
            <div class="k-section">
                <div class="k-section__content k-section__content--x-fit k-section__content--border">
                    <asp:ListView ID="lvSpecInfo" runat="server">
                        <LayoutTemplate>
                            <ul class="k-nav k-nav--active-bg" id="m_nav" role="tablist">
                                <li id="itemPlaceholder" runat="server" />
                            </ul>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <li class="k-nav__item k-nav__item--sub">
                                <asp:HiddenField ID="lvSpecID" runat="server" Value='<%#Eval("ID")%>' />
                                <div class="k-nav__link collapsed" role="tab" id="m_nav_link_<%#Eval("ID")%>" data-toggle="collapse" href="#m_nav_sub_<%#Eval("ID")%>" aria-expanded="false">
                                    <i class="k-nav__link-icon <%#Eval("IconFont")%>"></i>
                                    <span class="k-nav__link-text"><%#Eval("Title")%>（<%#Eval("Name")%>）
                                        <span class="dropdown">&nbsp;
                                            <a href="#" class="btn btn-sm btn-clean btn-icon btn-icon-md" data-toggle="dropdown" aria-expanded="true">
                                                <%#((bool)Eval("Enabled"))
                                                    ? "<span class=\"badge badge-success list-enabled\">启用</span>"
                                                    : "<span class=\"badge badge-danger list-enabled\">禁用</span>"
                                                %>
                                            </a>
                                            <div class="dropdown-menu dropdown-menu-right">
                                                <a class="dropdown-item" specid='<%#Eval("ID")%>' id="btnListEnabled" runat="server"
                                                    onserverclick="ListEnabled_Click"><i class="la la-angellist"></i>启用</a>
                                                <a class="dropdown-item" specid='<%#Eval("ID")%>' id="btnListDisabled" runat="server"
                                                    onserverclick="ListEnabled_Click"><i class="la la-lock"></i>禁用</a>
                                            </div>
                                        </span>
                                        <a class="btn btn-sm btn-clean btn-icon btn-icon-md" title="修改" specid='<%#Eval("ID")%>'
                                            id="btnListEdit" autopostback="true" onserverclick="ListEdit_Click" runat="server">
                                            <i class="la la-edit"></i></a>
                                    </span>
                                    <span class="k-nav__link-arrow"></span>
                                </div>
                                <ul class="k-nav__sub collapse" id="m_nav_sub_<%#Eval("ID")%>" role="tabpanel" aria-labelledby="m_nav_link_<%#Eval("ID")%>" data-parent="#m_nav">
                                    <asp:ListView ID="lvSpecValue" runat="server">
                                        <ItemTemplate>
                                            <li class="k-nav__item">
                                                <div class="k-nav__link">
                                                    <span class="k-nav__link-bullet k-nav__link-bullet--line"><span></span></span>
                                                    <span class="k-nav__link-text">
                                                        <%#Eval("Value")%>
                                                        <span class="dropdown">&nbsp;
                                                            <a href="#" class="btn btn-sm btn-clean btn-icon btn-icon-md" data-toggle="dropdown" aria-expanded="true">
                                                                <%#((bool)Eval("Enabled"))
                                                                    ? "<span class=\"badge badge-success list-enabled\">启用</span>"
                                                                    : "<span class=\"badge badge-danger list-enabled\">禁用</span>"
                                                                %>
                                                            </a>
                                                            <div class="dropdown-menu dropdown-menu-right">
                                                                <a class="dropdown-item" valueid='<%#Eval("ID")%>' id="btnListEnabled" runat="server"
                                                                    onserverclick="ListValueEnabled_Click"><i class="la la-angellist"></i>启用</a>
                                                                <a class="dropdown-item" valueid='<%#Eval("ID")%>' id="btnListDisabled" runat="server"
                                                                    onserverclick="ListValueEnabled_Click"><i class="la la-lock"></i>禁用</a>
                                                            </div>
                                                        </span>
                                                    </span>
                                                </div>
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
    <!--end: List -->
    <!--begin: InfoForm -->
    <div class="modal fade" id="InfoFormModal" tabindex="-1" role="dialog" aria-labelledby="" aria-hidden="true">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header" id="InfoFormModalHead">
                    <h5 class="modal-title">规格[<asp:Label ID="txtInfoFormTitle" runat="server" />]的信息表单</h5>
                    <div class="k-portlet__head-toolbar">
                        <a href="#" class="btn btn-secondary k-margin-r-10" data-dismiss="modal" aria-label="Close">
                            <span class="k-hidden-mobile">返回</span>
                        </a>
                        <div class="btn-group">
                            <button type="button" class="btn btn-primary" id="btnInfoFormSubmit" runat="server"
                                onclick="if (!SpecFormCheck()) { return false; }" onserverclick="InfoFormSubmit_Click">
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
                    <asp:HiddenField ID="hidInfoFormSpecID" Value="0" runat="server" />
                    <div class="k-form k-form--label-right">
                        <!--图标-->
                        <div class="form-group row k-margin-t-20">
                            <label class="col-form-label col-lg-3 col-sm-12">规格图标</label>
                            <div class="col-lg-9 col-md-9 col-sm-12">
                                <asp:TextBox ID="inpInfoFormIconFont" runat="server" ClientIDMode="Static" class="form-control"
                                    MaxLength="32" title="规格图标" placeholder="图标最多32个字符。" />
                            </div>
                        </div>
                        <!--代码-->
                        <div class="form-group row k-margin-t-20">
                            <label class="col-form-label col-lg-3 col-sm-12">规格代码</label>
                            <div class="col-lg-9 col-md-9 col-sm-12">
                                <asp:TextBox ID="inpInfoFormSpecName" runat="server" ClientIDMode="Static" class="form-control"
                                    MaxLength="32" title="规格代码" placeholder="规格代码最多32个字符。" />
                            </div>
                        </div>
                        <!--名称-->
                        <div class="form-group row k-margin-t-20">
                            <label class="col-form-label col-lg-3 col-sm-12">规格名称</label>
                            <div class="col-lg-9 col-md-9 col-sm-12">
                                <asp:TextBox ID="inpInfoFormSpecTitle" runat="server" ClientIDMode="Static" class="form-control"
                                    MaxLength="32" title="规格名称" placeholder="规格名称最多32个字符。" />
                            </div>
                        </div>
                        <!--参数-->
                        <div class="form-group row">
                            <label class="col-lg-3 col-form-label">规格参数</label>
                            <div class="col-lg-6">
                                <asp:ListView ID="lvInfoFormSpecValue" runat="server">
                                    <LayoutTemplate>
                                        <div class="k-repeater">
                                            <div data-repeater-list="demo1">
                                                <div id="itemPlaceholder" runat="server" />
                                            </div>
                                            <div class="k-repeater__add-data">
                                                <span data-repeater-create="" class="btn btn-info btn-sm">
                                                    <i class="la la-plus"></i>增加规格参数
                                                </span>
                                            </div>
                                        </div>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <div data-repeater-item="" class="k-repeater__item">
                                            <div class="input-group">
                                                <div class="input-group-prepend"><span class="input-group-text"><i class="la la-puzzle-piece"></i></span></div>
                                                <input type="text" name='demo1[<%#Container.DataItemIndex%>][inpInfoFormSpecValue]'
                                                    value='<%#Eval("Value")%>' class="form-control" placeholder="参数名称">
                                            </div>
                                            <div class="k-separator k-separator--space-sm"></div>
                                        </div>
                                    </ItemTemplate>
                                </asp:ListView>
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
    <!-- end:: Content -->
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PageFootPlaceHolder" runat="server">
    <script>
        $(document).ready(function () {
            //表单
            var FormsName = ['#InfoFormModal'];
            for (var i = 0; i < FormsName.length; i++) { ModalHeadScroll(FormsName[i]); };
            //文字
            var inpNames = ['#inpInfoFormSpecName', '#inpInfoFormSpecTitle', '#inpInfoFormIconFont'];
            for (var i = 0; i < inpNames.length; i++) {
                $(inpNames[i]).maxlength({
                    alwaysShow: true,
                    threshold: 5,
                    warningClass: "k-badge k-badge--primary k-badge--rounded k-badge--inline",
                    limitReachedClass: "k-badge k-badge--brand k-badge--rounded k-badge--inline"
                });
            }
            //参数
            $('.k-repeater').each(function () {
                $(this).repeater({
                    show: function () { $(this).slideDown(); },
                    isFirstItemUndeletable: true
                });
            });
        });
    </script>
</asp:Content>
