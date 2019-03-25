<%@ Page Title="" Language="C#" MasterPageFile="~/UI/Keen.Master" AutoEventWireup="true" CodeBehind="UserInfo.aspx.cs" Inherits="DMS.MOD.SYS.UserInfo" %>

<%@ Register Src="~/UC/PagerControl.ascx" TagPrefix="uc1" TagName="PagerControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageHeadPlaceHolder" runat="server">
    <!--begin::Page Custom Styles(used by this page) -->
    <link href="/UI/keenthemes.com/keen/preview/default/assets/custom/user/profile-v1.css" rel="stylesheet" type="text/css" />
    <!--end::Page Custom Styles -->
    <link href="/CSS/List.css" rel="stylesheet" type="text/css" />
    <script src="/JS/SHA.js" type="text/javascript"></script>
    <script src="/JS/List.js" type="text/javascript"></script>
    <script>
        function UserFormCheck() {
            var inps = ['inpInfoFormUserName', 'inpInfoFormPassword'];
            if (FormCheck(inps)) {
                inpInfoFormPassword.value = SHA256(inpInfoFormPassword.value);
                return true;
            };
            return false;
        }
    </script>
    <style type="text/css">
        .card-header label {
            margin: 0px 0px -6px 0px !important;
        }

        .k-portlet__head-toolbar {
            padding-right: 20px;
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
                    <h3 class="k-portlet__head-title">用户信息列表</h3>
                </div>
                <div class="k-portlet__head-toolbar">
                    <div class="head-toolbar-button">
                        <div class="input-group">
                            <asp:TextBox ID="inpFilter" runat="server" CssClass="form-control" placeholder="用户名" ClientIDMode="Static" AutoPostBack="true" OnTextChanged="Filter_Change" />
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
                        <a data-toggle="modal" data-target="#InfoFormModal" id="btnListInfoFormModal" style="display: none;"></a>
                        <a data-toggle="modal" data-target="#DetailsModal" id="btnListDetailsModal" style="display: none;"></a>
                        <a data-toggle="modal" data-target="#ModuleAuthFormModal" id="btnListModuleAuthorityModal" style="display: none;"></a>
                        <a data-toggle="modal" data-target="#RoleAssignFormModal" id="btnListRoleAssignModal" style="display: none;"></a>
                    </div>
                    <div class="head-toolbar-button">
                        <div class="dropdown dropdown-inline">
                            <button type="button" class="btn btn-clean btn-sm btn-icon btn-icon-md" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                <i class="flaticon-more-1"></i>
                            </button>
                            <div class="dropdown-menu dropdown-menu-right">
                                <ul class="k-nav">
                                    <li class="k-nav__item">
                                        <a href="#" class="k-nav__link">
                                            <i class="k-nav__link-icon flaticon2-chart2"></i>
                                            <span class="k-nav__link-text">报表</span>
                                        </a>
                                    </li>
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="k-portlet__body">
                <div id="m_table_2_wrapper" class="dataTables_wrapper dt-bootstrap4 no-footer">
                    <div class="row">
                        <!--begin: Datatable -->
                        <asp:ListView ID="UserInfoList" runat="server">
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
                                                    <span class="list-field-title">用户名</span>
                                                    <span class="list-orderby-none" id="ListOrderByNameAsc" runat="server">↓</span>
                                                    <span class="list-orderby-none" id="ListOrderByNameDesc" runat="server">↑</span>
                                                    <asp:HiddenField ID="ListOrderByNameValue" runat="server" />
                                                </button>
                                            </th>
                                            <th>
                                                <button id="ListOrderByStateID" runat="server" onserverclick="OrderBy_Click">
                                                    <span class="list-field-title">状态</span>
                                                    <span class="list-orderby-none" id="ListOrderByStateIDAsc" runat="server">↓</span>
                                                    <span class="list-orderby-none" id="ListOrderByStateIDDesc" runat="server">↑</span>
                                                    <asp:HiddenField ID="ListOrderByStateIDValue" runat="server" />
                                                </button>
                                            </th>
                                            <th>
                                                <button id="ListOrderByUpdateTime" runat="server" onserverclick="OrderBy_Click">
                                                    <span class="list-field-title">更新时间</span>
                                                    <span class="list-orderby-none" id="ListOrderByUpdateTimeAsc" runat="server">↓</span>
                                                    <span class="list-orderby-none" id="ListOrderByUpdateTimeDesc" runat="server">↑</span>
                                                    <asp:HiddenField ID="ListOrderByUpdateTimeValue" runat="server" />
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
                                    <th><%#((Container.DataItemIndex + 1) + (UserInfoListPager.PageIndex * UserInfoListPager.PageSize - UserInfoListPager.PageSize))%></th>
                                    <td>
                                        <span><%#Eval("ID")%></span>
                                        <a class="btn btn-sm btn-clean btn-icon btn-icon-md" title="详情" userid='<%#Eval("ID")%>'
                                            id="btnListDetails" autopostback="true" onserverclick="ListDetails_Click" runat="server">
                                            <i class="la la-book"></i></a>
                                        <a class="btn btn-sm btn-clean btn-icon btn-icon-md" title="修改" userid='<%#Eval("ID")%>'
                                            id="btnListEdit" autopostback="true" onserverclick="ListEdit_Click" runat="server">
                                            <i class="la la-edit"></i></a>
                                        <a class="btn btn-sm btn-clean btn-icon btn-icon-md" title="角色指定" userid='<%#Eval("ID")%>'
                                            id="btnListRoleAssign" autopostback="true" onserverclick="ListRoleAssign_Click" runat="server">
                                            <i class="fa fa-user-tie"></i></a>
                                        <span class="dropdown">
                                            <a href="#" class="btn btn-sm btn-clean btn-icon btn-icon-md" data-toggle="dropdown" aria-expanded="true">
                                                <i class="la la-ellipsis-h"></i>
                                            </a>
                                            <div class="dropdown-menu dropdown-menu-right">
                                                <a class="dropdown-item" userid='<%#Eval("ID")%>' id="btnListAuthority" runat="server"
                                                    onserverclick="ListModuleAuthority_Click"><i class="flaticon-list-3"></i>模块授权</a>
                                                <a class="dropdown-item" userid='<%#Eval("ID")%>' id="btnListEnabled" runat="server"
                                                    onserverclick="ListEnabled_Click"><i class="la la-angellist"></i>启用</a>
                                                <a class="dropdown-item" userid='<%#Eval("ID")%>' id="btnListDisabled" runat="server"
                                                    onserverclick="ListEnabled_Click"><i class="la la-lock"></i>禁用</a>
                                            </div>
                                        </span>
                                    </td>
                                    <td><%#FilterFormat(inpFilter, (string)Eval("Name"))%></td>
                                    <td><%#StateFormat((int)Eval("StateID"))%></td>
                                    <td><%#Eval("UpdateTime")%></td>
                                </tr>
                            </ItemTemplate>
                        </asp:ListView>
                        <!--end: Datatable -->
                    </div>
                    <div class="row">
                        <uc1:PagerControl ID="UserInfoListPager" PageSize="10" ClientIDMode="Static" runat="server" />
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
                    <h5 class="modal-title">用户[<asp:Label ID="txtInfoFormTitle" runat="server" />]的信息表单</h5>
                    <div class="k-portlet__head-toolbar">
                        <a href="#" class="btn btn-secondary k-margin-r-10" data-dismiss="modal" aria-label="Close">
                            <span class="k-hidden-mobile">返回</span>
                        </a>
                        <div class="btn-group">
                            <button type="button" class="btn btn-primary" id="btnInfoFormSubmit" runat="server"
                                onclick="if (!UserFormCheck()) { return false; }" onserverclick="InfoFormSubmit_Click">
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
                    <asp:HiddenField ID="hidInfoFormUserID" Value="0" runat="server" />
                    <div class="k-form k-form--label-right">
                        <!--用户名-->
                        <div class="form-group row k-margin-t-20">
                            <label class="col-form-label col-lg-3 col-sm-12">用户名</label>
                            <div class="col-lg-9 col-md-9 col-sm-12">
                                <asp:TextBox ID="inpInfoFormUserName" runat="server" ClientIDMode="Static" class="form-control" MaxLength="16" title="用户名" placeholder="用户名最多16个字符。" />
                            </div>
                        </div>
                        <!--密码-->
                        <div class="form-group row k-margin-t-20">
                            <label class="col-form-label col-lg-3 col-sm-12">密码</label>
                            <div class="col-lg-9 col-md-9 col-sm-12">
                                <asp:TextBox TextMode="Password" ID="inpInfoFormPassword" runat="server" ClientIDMode="Static" class="form-control" title="密码" placeholder="密码将以SHA256加密发送，建议不要设置弱密码。" />
                            </div>
                        </div>
                        <!--姓名-->
                        <div class="form-group row k-margin-t-20">
                            <label class="col-form-label col-lg-3 col-sm-12">姓名</label>
                            <div class="col-lg-9 col-md-9 col-sm-12">
                                <input type="text" class="form-control" maxlength="16" title="姓名" placeholder="姓名最多16个字符。" id="inpInfoFormRealName" clientidmode="static" runat="server" />
                            </div>
                        </div>
                        <!--身份证号-->
                        <div class="form-group row k-margin-t-20">
                            <label class="col-form-label col-lg-3 col-sm-12">身份证号</label>
                            <div class="col-lg-9 col-md-9 col-sm-12">
                                <div class="input-group">
                                    <div class="input-group-prepend"><span class="input-group-text"><i class="la la-credit-card"></i></span></div>
                                    <input type="text" class="form-control" maxlength="18" title="身份证号" placeholder="身份证号最多18个字符。" id="inpInfoFormIDCardNO" clientidmode="static" runat="server" />
                                </div>
                            </div>
                        </div>
                        <!--性别-->
                        <div class="form-group row k-margin-t-20">
                            <label class="col-form-label col-lg-3 col-sm-12">性别</label>
                            <div class="col-lg-9 col-md-9 col-sm-12">
                                <div class="k-radio-inline">
                                    <label class="k-radio">
                                        <input type="radio" name="form_gender" title="性别" value="male" id="inpInfoFormGenderMale" runat="server">男性<span></span></label>
                                    <label class="k-radio">
                                        <input type="radio" name="form_gender" title="性别" value="female" id="inpInfoFormGenderFemale" runat="server">女性<span></span></label>
                                    <label class="k-radio">
                                        <input type="radio" name="form_gender" title="性别" value="other" id="inpInfoFormGenderOther" runat="server">其他<span></span></label>
                                </div>
                            </div>
                        </div>
                        <!--头像-->
                        <asp:HiddenField ID="hidInfoFormAvatarFileID" Value="0" runat="server" />
                        <div class="form-group row k-margin-t-20">
                            <label class="col-form-label col-lg-3 col-sm-12">头像</label>
                            <div class="col-lg-9 col-md-9 col-sm-12">
                                <div class="k-avatar k-avatar--outline k-avatar--danger k-avatar--circle" id="form_avatar_div">
                                    <div class="k-avatar__holder" style="background-image: url(/media/users/avatar_default.png)" id="divInfoFormAvatar" runat="server"></div>
                                    <label class="k-avatar__upload" data-toggle="k-tooltip" title="" data-original-title="更换头像">
                                        <i class="fa fa-pen"></i>
                                        <input type="file" name="profile_avatar" accept=".png, .jpg, .jpeg" id="inp_form_avatar" runat="server">
                                    </label>
                                    <span class="k-avatar__cancel" data-toggle="k-tooltip" title="" data-original-title="清除头像">
                                        <i class="fa fa-times"></i>
                                    </span>
                                </div>
                                <span class="form-text text-muted">头像需要正方形图片，最大高度和宽度为256像素。</span>
                            </div>
                        </div>
                        <!--生日-->
                        <div class="form-group row k-margin-t-20">
                            <label class="col-form-label col-lg-3 col-sm-12">生日</label>
                            <div class="col-lg-9 col-md-9 col-sm-12">
                                <div class="input-group">
                                    <div class="input-group-prepend"><span class="input-group-text"><i class="la la-calendar-check-o"></i></span></div>
                                    <input type="text" class="form-control" name="form_birthday" title="生日" placeholder="选择出生日期。" id="inpInfoFormBirthday" clientidmode="Static" runat="server">
                                </div>
                            </div>
                        </div>
                        <!--电话-->
                        <div class="form-group row k-margin-t-20">
                            <label class="col-form-label col-lg-3 col-sm-12">电话</label>
                            <div class="col-lg-9 col-md-9 col-sm-12">
                                <div class="input-group">
                                    <div class="input-group-prepend"><span class="input-group-text"><i class="la la-phone"></i></span></div>
                                    <input type="text" class="form-control" aria-describedby="basic-addon1" title="电话"
                                        maxlength="16" placeholder="手机号码或电话号码。" id="inpInfoFormPhone" clientidmode="static" runat="server">
                                </div>
                            </div>
                        </div>
                        <!--手机-->
                        <div class="form-group row k-margin-t-20">
                            <label class="col-form-label col-lg-3 col-sm-12">手机</label>
                            <div class="col-lg-9 col-md-9 col-sm-12">
                                <div class="input-group">
                                    <div class="input-group-prepend"><span class="input-group-text"><i class="la la-mobile-phone"></i></span></div>
                                    <input type="text" class="form-control" aria-describedby="basic-addon1" title="手机"
                                        maxlength="16" placeholder="手机号码或电话号码。" id="inpInfoFormMobile" clientidmode="static" runat="server">
                                </div>
                            </div>
                        </div>
                        <!--邮件-->
                        <div class="form-group row k-margin-t-20">
                            <label class="col-form-label col-lg-3 col-sm-12">邮件</label>
                            <div class="col-lg-9 col-md-9 col-sm-12">
                                <div class="input-group">
                                    <div class="input-group-prepend"><span class="input-group-text"><i class="la la-at"></i></span></div>
                                    <input type="text" class="form-control" aria-describedby="basic-addon1" title="邮件"
                                        maxlength="64" placeholder="电子邮件地址。" id="inpInfoFormEmail" clientidmode="static" runat="server">
                                </div>
                            </div>
                        </div>
                        <!--地址-->
                        <div class="form-group row k-margin-t-20">
                            <label class="col-form-label col-lg-3 col-sm-12">地址</label>
                            <div class="col-lg-9 col-md-9 col-sm-12">
                                <div class="input-group">
                                    <div class="input-group-prepend"><span class="input-group-text"><i class="la la-at"></i></span></div>
                                    <input type="text" class="form-control" aria-describedby="basic-addon1" title="地址"
                                        maxlength="256" placeholder="地址。" id="inpInfoFormAddress" clientidmode="static" runat="server">
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
                    <h5 class="modal-title">用户[<asp:Label ID="txtDetailsModalTitle" runat="server" />]的详细信息</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true" class="la la-remove"></span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="k-portlet k-profile">
                        <div class="k-profile__content">
                            <div class="row">
                                <div class="col-md-12 col-lg-5 col-xl-4">
                                    <div class="k-profile__main">
                                        <div class="k-profile__main-pic">
                                            <img src="/media/users/avatar_default.png" id="imgDetailsAvatar" runat="server">
                                            <label class="k-profile__main-pic-upload">
                                                <i class="flaticon-photo-camera"></i>
                                            </label>
                                        </div>
                                        <div class="k-profile__main-info">
                                            <div class="k-profile__main-info-name">
                                                <label id="txtDetailsUserName" runat="server" />
                                            </div>
                                            <div class="k-profile__main-info-position">
                                                <label id="txtDetailsPosition" runat="server" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-12 col-lg-4 col-xl-4">
                                    <div class="k-profile__contact">
                                        <a class="k-profile__contact-item">
                                            <span class="k-profile__contact-item-icon k-profile__contact-item-icon-whatsup"><i class="flaticon-whatsapp"></i></span>
                                            <span class="k-profile__contact-item-text">
                                                <label id="txtDetailsPhone" runat="server" />
                                            </span>
                                        </a>
                                        <a class="k-profile__contact-item">
                                            <span class="k-profile__contact-item-icon"><i class="flaticon-email-black-circular-button k-font-danger"></i></span>
                                            <span class="k-profile__contact-item-text">
                                                <label id="txtDetailsEMail" runat="server" />
                                            </span>
                                        </a>
                                        <a class="k-profile__contact-item">
                                            <span class="k-profile__contact-item-icon"><i class="flaticon-placeholder-3"></i></span>
                                            <span class="k-profile__contact-item-text">
                                                <label id="txtDetailsAddress" runat="server" />
                                            </span>
                                        </a>
                                    </div>
                                </div>
                                <div class="col-md-12 col-lg-3 col-xl-4">
                                    <div class="k-profile__person">
                                        <a class="k-profile__contact-item">
                                            <span class="k-profile__contact-item-icon">姓名</span>
                                            <span class="k-profile__contact-item-text">
                                                <label id="txtDetailsRaleName" runat="server" />
                                            </span>
                                        </a>
                                        <br />
                                        <a class="k-profile__contact-item">
                                            <span class="k-profile__contact-item-icon">性别</span>
                                            <span class="k-profile__contact-item-text">
                                                <label id="txtDetailsGender" runat="server" />
                                            </span>
                                        </a>
                                        <br />
                                        <a class="k-profile__contact-item">
                                            <span class="k-profile__contact-item-icon">生日</span>
                                            <span class="k-profile__contact-item-text">
                                                <label id="txtDetailsBirthday" runat="server" />
                                            </span>
                                        </a>
                                        <br />
                                        <a class="k-profile__contact-item">
                                            <span class="k-profile__contact-item-icon">身份证</span>
                                            <span class="k-profile__contact-item-text">
                                                <label id="txtDetailsIDCardNO" runat="server" />
                                            </span>
                                        </a>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!--end: InfoDetails -->
    <!--begin: ModuleAuthorityForm -->
    <div class="modal fade" id="ModuleAuthFormModal" tabindex="-1" role="dialog" aria-labelledby="" aria-hidden="true">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header" id="ModuleAuthFormModalHead">
                    <h5 class="modal-title">用户[<asp:Label ID="txtModuleAuthUserName" runat="server" />]的模块权限</h5>
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
                    <asp:HiddenField ID="hidModuleAuthUserID" Value="0" runat="server" />
                    <asp:ListView ID="lvModuleList" runat="server">
                        <LayoutTemplate>
                            <div class="accordion accordion-outline" id="accordionExample3">
                                <div id="itemPlaceholder" runat="server" />
                            </div>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <div class="card">
                                <div class="card-header" id="headingOneAuth<%#Eval("ID")%>">
                                    <div class="card-title collapsed" data-toggle="collapse" data-target="#collapseOneAuth<%#Eval("ID")%>" aria-expanded="false" aria-controls="collapseOneAuth<%#Eval("ID")%>">
                                        <span><i class='<%#Eval("IconFont")%>'></i>&nbsp<%#Eval("Title")%>模块</span><span style="display: none;">（<%#Eval("Name")%>）</span>
                                        <span class="k-switch k-switch--sm k-switch--icon">
                                            <label>
                                                <input type="checkbox" checked='<%#Eval("UserAuth")%>' moduleid='<%#Eval("ID")%>' id="inpModuleAuth" runat="server">
                                                <span></span>
                                            </label>
                                        </span>
                                    </div>
                                </div>
                                <div id="collapseOneAuth<%#Eval("ID")%>" class="card-body-wrapper collapse" aria-labelledby="headingOneAuth<%#Eval("ID")%>" data-parent="#accordionExample3">
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
                                                            <input type="checkbox" checked='<%#Eval("UserAuth")%>' actionid='<%#Eval("ID")%>' id="inpActionAuth" runat="server">
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
                </div>
                <div class="modal-footer">
                </div>
            </div>
        </div>
    </div>
    <!--end: ModuleAuthorityForm -->
    <!--begin: RoleAssignFormModal -->
    <div class="modal fade" id="RoleAssignFormModal" tabindex="-1" role="dialog" aria-labelledby="" aria-hidden="true">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header" id="RoleAssignFormModalHead">
                    <h5 class="modal-title">用户[<asp:Label ID="txtRoleAssignUserName" runat="server" />]的角色指定</h5>
                    <div class="k-portlet__head-toolbar">
                        <a href="#" class="btn btn-secondary k-margin-r-10" data-dismiss="modal" aria-label="Close">
                            <span class="k-hidden-mobile">返回</span>
                        </a>
                        <div class="btn-group">
                            <button type="button" class="btn btn-primary" id="btnRoleAssignSubmit" runat="server" onserverclick="RoleFormSubmit_Click">
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
                    <asp:HiddenField ID="hidRoleAssignUserID" Value="0" runat="server" />
                    <asp:ListView ID="lvRoleList" runat="server">
                        <LayoutTemplate>
                            <div class="accordion accordion-outline" id="accordionExample4">
                                <div id="itemPlaceholder" runat="server" />
                            </div>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <div class="card">
                                <div class="card-header" id="headingOneRole<%#Eval("ID")%>">
                                    <div class="card-title collapsed" data-toggle="collapse" data-target="#collapseOneRole<%#Eval("ID")%>" aria-expanded="false" aria-controls="collapseOneRole<%#Eval("ID")%>">
                                        <span><i class='<%#Eval("IconFont")%>'></i>&nbsp<%#Eval("Title")%>角色&nbsp;</span><span style="display: none;">（<%#Eval("Name")%>）</span>
                                        <span class="k-switch k-switch--sm k-switch--icon">
                                            <label>
                                                <input type="checkbox" checked='<%#Eval("UserAssign")%>' roleid='<%#Eval("ID")%>' id="inpUserRoleAssign" runat="server">
                                                <span></span>
                                            </label>
                                        </span>
                                    </div>
                                </div>
                                <div id="collapseOneRole<%#Eval("ID")%>" class="card-body-wrapper collapse" aria-labelledby="headingOneRole<%#Eval("ID")%>" data-parent="#accordionExample4">
                                    <div class="card-body">
                                        <asp:ListView ID="lvRoleModuleAuthList" runat="server">
                                            <LayoutTemplate>
                                                <ul>
                                                    <li id="itemPlaceholder" runat="server" />
                                                </ul>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <li>
                                                    <asp:HiddenField ID="ModuleID" Value='<%#Eval("ID")%>' runat="server" />
                                                    <span><i class='<%#Eval("IconFont")%>'></i>&nbsp;<%#Eval("Title")%>模块</span>
                                                    <span style="display: none;">（<%#Eval("Name")%>）</span>
                                                    <%#((bool)Eval("RoleAuth")) ? "<span class='badge badge-success'>完全权限</span>" : null%>
                                                    <asp:ListView ID="lvRoleActionAuthList" runat="server">
                                                        <LayoutTemplate>
                                                            <ul>
                                                                <li id="itemPlaceholder" runat="server" />
                                                            </ul>
                                                        </LayoutTemplate>
                                                        <ItemTemplate>
                                                            <li>
                                                                <span><i class='<%#Eval("IconFont")%>'></i>&nbsp;<%#Eval("Title")%></span>
                                                                <span style="display: none;">（<%#Eval("Name")%>）</span>
                                                                <%#((bool)Eval("RoleAuth")) ? "<span class='badge badge-success'>有权</span>" : null%>
                                                            </li>
                                                        </ItemTemplate>
                                                    </asp:ListView>
                                                </li>
                                            </ItemTemplate>
                                        </asp:ListView>
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:ListView>
                </div>
                <div class="modal-footer">
                </div>
            </div>
        </div>
    </div>
    <!--end: RoleAssignFormModal -->
    <!-- end:: Content -->
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageFootPlaceHolder" runat="server">
    <script>
        $(document).ready(function () {
            //表单
            var FormsName = ['#InfoFormModal', '#ModuleAuthFormModal', '#RoleAssignFormModal'];
            for (var i = 0; i < FormsName.length; i++) { ModalHeadScroll(FormsName[i]); };
            //头像
            new KAvatar('form_avatar_div');
            //文字
            var inpNames = ['#inpInfoFormUserName', '#inpInfoFormRealName', '#inpInfoFormIDCardNO'
                , '#inpInfoFormPhone', '#inpInfoFormMobile', '#inpInfoFormEmail', '#inpInfoFormAddress'];
            for (var i = 0; i < inpNames.length; i++) {
                $(inpNames[i]).maxlength({
                    alwaysShow: true,
                    threshold: 5,
                    warningClass: "k-badge k-badge--primary k-badge--rounded k-badge--inline",
                    limitReachedClass: "k-badge k-badge--brand k-badge--rounded k-badge--inline"
                });
            }
            //日期
            $('#inpInfoFormBirthday').datepicker({
                todayHighlight: true,
                templates: {
                    leftArrow: '<i class="la la-angle-left"></i>',
                    rightArrow: '<i class="la la-angle-right"></i>'
                }
            });
        });
    </script>
</asp:Content>
