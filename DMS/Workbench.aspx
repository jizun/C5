<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Workbench.aspx.cs" Inherits="DMS.Workbench" %>

<!DOCTYPE html>
<html lang="zh-cn">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="description" content="DMS - ZIRI">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title>工作台</title>

    <!--begin::Global Theme Styles(used by all pages) -->
    <link href="/UI/keenthemes.com/keen/preview/default/assets/vendors/base/vendors.bundle.css" rel="stylesheet" type="text/css" />
    <link href="/UI/keenthemes.com/keen/preview/default/assets/demo/default/base/style.bundle.css" rel="stylesheet" type="text/css" />
    <!--end::Global Theme Styles -->

    <!--begin::Layout Skins(used by all pages) -->
    <link href="/UI/keenthemes.com/keen/preview/default/assets/demo/default/skins/header/base/light.css" rel="stylesheet" type="text/css" />
    <link href="/UI/keenthemes.com/keen/preview/default/assets/demo/default/skins/header/menu/light.css" rel="stylesheet" type="text/css" />
    <link href="/UI/keenthemes.com/keen/preview/default/assets/demo/default/skins/brand/navy.css" rel="stylesheet" type="text/css" />
    <link href="/UI/keenthemes.com/keen/preview/default/assets/demo/default/skins/aside/navy.css" rel="stylesheet" type="text/css" />
    <!--end::Layout Skins -->

    <link rel="stylesheet" type="text/css" href="/CSS/Hui-tabNav.css" />
    <style type="text/css">
        .k-menu__section {
            margin: 0px !important;
        }
    </style>
</head>
<body class="k-header--fixed k-header-mobile--fixed k-subheader--enabled k-subheader--transparent k-aside--enabled k-aside--fixed k-page--loading">
    <form id="Workbench" runat="server">
        <!-- begin:: Header Mobile -->
        <div id="k_header_mobile" class="k-header-mobile  k-header-mobile--fixed ">
            <div class="k-header-mobile__logo">
                <img alt="Logo" src="/UI/keenthemes.com/keen/preview/default/assets/media/logos/logo-6.png" />
            </div>
            <div class="k-header-mobile__toolbar">
                <div class="k-header-mobile__toolbar-toggler k-header-mobile__toolbar-toggler--left" id="k_aside_mobile_toggler"><span></span></div>
                <div class="k-header-mobile__toolbar-toggler" id="k_header_mobile_toggler"><span></span></div>
                <div class="k-header-mobile__toolbar-topbar-toggler" id="k_header_mobile_topbar_toggler"><i class="flaticon-more"></i></div>
            </div>
        </div>
        <!-- end:: Header Mobile -->

        <!-- begin:: Root -->
        <div class="k-grid k-grid--hor k-grid--root">
            <!-- begin:: Page -->
            <div class="k-grid__item k-grid__item--fluid k-grid k-grid--ver k-page">
                <!-- begin:: Aside -->
                <button class="k-aside-close " id="k_aside_close_btn"><i class="la la-close"></i></button>
                <div class="k-aside  k-aside--fixed  k-grid__item k-grid k-grid--desktop k-grid--hor-desktop" id="k_aside">

                    <!-- begin::Aside Brand -->
                    <div class="k-aside__brand k-grid__item " id="k_aside_brand">
                        <div class="k-aside__brand-logo">
                            <img alt="Logo" src="/media/logos/logo_hnlg_2.png" />
                        </div>
                        <div class="k-aside__brand-tools">
                            <div class="k-aside__brand-aside-toggler k-aside__brand-aside-toggler--left" id="k_aside_toggler"><span></span></div>
                        </div>
                    </div>
                    <!-- end:: Aside Brand -->

                    <!-- begin:: Aside Menu -->
                    <div class="k-aside-menu-wrapper k-grid__item k-grid__item--fluid" id="k_aside_menu_wrapper">
                        <div id="k_aside_menu" class="k-aside-menu " data-kmenu-vertical="1" data-kmenu-scroll="1" data-kmenu-dropdown-timeout="500">
                            <ul class="k-menu__nav">
                                <!-- 根目录 -->
                                <asp:ListView ID="lvMenuRoots" runat="server">
                                    <ItemTemplate>
                                        <li class="k-menu__section ">
                                            <asp:HiddenField ID="lvMenuRootID" Value='<%#Eval("ID")%>' runat="server" />
                                            <h4 class="k-menu__section-text"><%#Eval("Title")%></h4>
                                            <i class="k-menu__section-icon flaticon-more-v2"></i>
                                        </li>
                                        <!-- 一级目录 -->
                                        <asp:ListView ID="lvMenuLevel1" runat="server">
                                            <ItemTemplate>
                                                <li class='k-menu__item k-menu__item--submenu ' aria-haspopup="true" data-kmenu-submenu-toggle="hover" id="lv1li" runat="server">
                                                    <a class="k-menu__link k-menu__toggle">
                                                        <i class="k-menu__link-icon <%#Eval("IconFont")%>"></i>
                                                        <asp:HiddenField ID="lvMenuLevel1ID" Value='<%#Eval("ID")%>' runat="server" />
                                                        <span class="k-menu__link-text"><%#Eval("Title")%></span>
                                                        <i class="k-menu__ver-arrow la la-angle-right"></i>
                                                    </a>
                                                    <!-- 二级目录 -->
                                                    <div class="k-menu__submenu ">
                                                        <span class="k-menu__arrow"></span>
                                                        <ul class="k-menu__subnav">
                                                            <asp:ListView ID="lvMenuLevel2" runat="server">
                                                                <ItemTemplate>
                                                                    <li class="k-menu__item " aria-haspopup="true">
                                                                        <a data-title="<%#Eval("Title")%>" data-url="<%#Eval("URL")%>" class="k-menu__link iframe_link ">
                                                                            <i class="k-menu__link-icon <%#Eval("IconFont")%>"></i>
                                                                            <asp:HiddenField ID="lvMenuLevel2ID" Value='<%#Eval("ID")%>' runat="server" />
                                                                            <span class="k-menu__link-text"><%#Eval("Title")%></span>
                                                                        </a>
                                                                    </li>
                                                                </ItemTemplate>
                                                            </asp:ListView>
                                                        </ul>
                                                    </div>
                                                </li>
                                            </ItemTemplate>
                                        </asp:ListView>
                                    </ItemTemplate>
                                </asp:ListView>
                            </ul>
                        </div>
                    </div>
                    <!-- end:: Aside Menu -->

                    <!-- begin:: Aside Footer -->
                    <div class="k-aside__footer k-grid__item" id="k_aside_footer">
                        <div class="k-aside__footer-nav">
                            <div class="k-aside__footer-item">
                                <button type="button" class="btn btn-icon" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                    <i class="flaticon2-gear"></i>
                                </button>
                                <div class="dropdown-menu dropdown-menu-left">
                                    <ul class="k-nav">
                                        <li class="k-nav__section k-nav__section--first">
                                            <span class="k-nav__section-text">系统设置</span>
                                        </li>
                                        <li class="k-nav__item">
                                            <a data-title="菜单信息" data-url="/MOD/SYS/MenuInfo.aspx" class="k-nav__link iframe_link">
                                                <i class="k-nav__link-icon fa fa-list"></i>
                                                <span class="k-nav__link-text">菜单信息</span>
                                            </a>
                                        </li>
                                        <li class="k-nav__item">
                                            <a data-title="模块信息" data-url="/MOD/SYS/ModuleInfo.aspx" class="k-nav__link iframe_link">
                                                <i class="k-nav__link-icon fa fa-microchip"></i>
                                                <span class="k-nav__link-text">模块信息</span>
                                            </a>
                                        </li>
                                    </ul>
                                </div>
                            </div>
                            <div class="k-aside__footer-item">
                                <a href="#" class="btn btn-icon"><i class="flaticon2-cube"></i></a>
                            </div>
                            <div class="k-aside__footer-item">
                                <a href="#" class="btn btn-icon"><i class="flaticon2-bell-alarm-symbol"></i></a>
                            </div>
                            <div class="k-aside__footer-item">
                                <button type="button" class="btn btn-icon" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                    <i class="flaticon2-add"></i>
                                </button>
                                <div class="dropdown-menu dropdown-menu-left">
                                    <ul class="k-nav">
                                        <li class="k-nav__section k-nav__section--first">
                                            <span class="k-nav__section-text">Export Tools</span>
                                        </li>
                                        <li class="k-nav__item">
                                            <a href="#" class="k-nav__link">
                                                <i class="k-nav__link-icon la la-print"></i>
                                                <span class="k-nav__link-text">Print</span>
                                            </a>
                                        </li>
                                        <li class="k-nav__item">
                                            <a href="#" class="k-nav__link">
                                                <i class="k-nav__link-icon la la-copy"></i>
                                                <span class="k-nav__link-text">Copy</span>
                                            </a>
                                        </li>
                                        <li class="k-nav__item">
                                            <a href="#" class="k-nav__link">
                                                <i class="k-nav__link-icon la la-file-excel-o"></i>
                                                <span class="k-nav__link-text">Excel</span>
                                            </a>
                                        </li>
                                        <li class="k-nav__item">
                                            <a href="#" class="k-nav__link">
                                                <i class="k-nav__link-icon la la-file-text-o"></i>
                                                <span class="k-nav__link-text">CSV</span>
                                            </a>
                                        </li>
                                        <li class="k-nav__item">
                                            <a href="#" class="k-nav__link">
                                                <i class="k-nav__link-icon la la-file-pdf-o"></i>
                                                <span class="k-nav__link-text">PDF</span>
                                            </a>
                                        </li>
                                    </ul>
                                </div>
                            </div>
                            <div class="k-aside__footer-item">
                                <a href="#" class="btn btn-icon"><i class="flaticon2-calendar-2"></i></a>
                            </div>
                        </div>
                    </div>
                    <!-- end:: Aside Footer-->
                </div>
                <!-- end:: Aside -->

                <!-- begin:: Wrapper -->
                <div class="k-grid__item k-grid__item--fluid k-grid k-grid--hor k-wrapper" id="k_wrapper">
                    <!-- begin:: Header -->
                    <div id="k_header" class="k-header k-grid__item  k-header--fixed ">
                        <!-- begin:: Header Menu -->
                        <button class="k-header-menu-wrapper-close" id="k_header_menu_mobile_close_btn"><i class="la la-close"></i></button>
                        <div class="k-header-menu-wrapper" id="k_header_menu_wrapper">
                            <div id="k_header_menu" class="k-header-menu k-header-menu-mobile  k-header-menu--layout- ">
                                <ul class="k-menu__nav ">
                                    <li class="k-menu__item  k-menu__item--submenu k-menu__item--rel k-menu__item--active" data-kmenu-submenu-toggle="click" data-kmenu-link-redirect="1" aria-haspopup="true">
                                        <a href="javascript:;" class="k-menu__link k-menu__toggle">
                                            <span class="k-menu__link-text">页面</span>
                                            <i class="k-menu__hor-arrow la la-angle-down"></i>
                                            <i class="k-menu__ver-arrow la la-angle-right"></i>
                                        </a>
                                        <div class="k-menu__submenu k-menu__submenu--classic k-menu__submenu--left">
                                            <ul class="k-menu__subnav">
                                                <li class="k-menu__item " aria-haspopup="true"><a href="javascript:;" class="k-menu__link "><i class="k-menu__link-bullet k-menu__link-bullet--dot"><span></span></i><span class="k-menu__link-text">Create New Post</span></a></li>
                                                <li class="k-menu__item " data-kmenu-link-redirect="1" aria-haspopup="true"><a href="javascript:;" class="k-menu__link "><i class="k-menu__link-bullet k-menu__link-bullet--dot"><span></span></i><span class="k-menu__link-text">Generate Reports</span><span class="k-menu__link-badge"><span class="k-badge k-badge--success">2</span></span></a></li>
                                                <li class="k-menu__item  k-menu__item--submenu" data-kmenu-submenu-toggle="hover" data-kmenu-link-redirect="1" aria-haspopup="true"><a href="javascript:;" class="k-menu__link k-menu__toggle"><i class="k-menu__link-bullet k-menu__link-bullet--dot"><span></span></i><span class="k-menu__link-text">Manage Orders</span><i class="k-menu__hor-arrow la la-angle-right"></i><i class="k-menu__ver-arrow la la-angle-right"></i></a><div class="k-menu__submenu k-menu__submenu--classic k-menu__submenu--right">
                                                    <ul class="k-menu__subnav">
                                                        <li class="k-menu__item " data-kmenu-link-redirect="1" aria-haspopup="true"><a href="javascript:;" class="k-menu__link "><span class="k-menu__link-text">Latest Orders</span></a></li>
                                                        <li class="k-menu__item " data-kmenu-link-redirect="1" aria-haspopup="true"><a href="javascript:;" class="k-menu__link "><span class="k-menu__link-text">Pending Orders</span></a></li>
                                                        <li class="k-menu__item " data-kmenu-link-redirect="1" aria-haspopup="true"><a href="javascript:;" class="k-menu__link "><span class="k-menu__link-text">Processed Orders</span></a></li>
                                                        <li class="k-menu__item " data-kmenu-link-redirect="1" aria-haspopup="true"><a href="javascript:;" class="k-menu__link "><span class="k-menu__link-text">Delivery Reports</span></a></li>
                                                        <li class="k-menu__item " data-kmenu-link-redirect="1" aria-haspopup="true"><a href="javascript:;" class="k-menu__link "><span class="k-menu__link-text">Payments</span></a></li>
                                                        <li class="k-menu__item " data-kmenu-link-redirect="1" aria-haspopup="true"><a href="javascript:;" class="k-menu__link "><span class="k-menu__link-text">Customers</span></a></li>
                                                    </ul>
                                                </div>
                                                </li>
                                                <li class="k-menu__item  k-menu__item--submenu" data-kmenu-submenu-toggle="hover" data-kmenu-link-redirect="1" aria-haspopup="true"><a href="/UI/keenthemes.com/keen/preview/default/#.html" class="k-menu__link k-menu__toggle"><i class="k-menu__link-bullet k-menu__link-bullet--dot"><span></span></i><span class="k-menu__link-text">Customer Feedbacks</span><i class="k-menu__hor-arrow la la-angle-right"></i><i class="k-menu__ver-arrow la la-angle-right"></i></a><div class="k-menu__submenu k-menu__submenu--classic k-menu__submenu--right">
                                                    <ul class="k-menu__subnav">
                                                        <li class="k-menu__item " data-kmenu-link-redirect="1" aria-haspopup="true"><a href="javascript:;" class="k-menu__link "><span class="k-menu__link-text">Customer Feedbacks</span></a></li>
                                                        <li class="k-menu__item " data-kmenu-link-redirect="1" aria-haspopup="true"><a href="javascript:;" class="k-menu__link "><span class="k-menu__link-text">Supplier Feedbacks</span></a></li>
                                                        <li class="k-menu__item " data-kmenu-link-redirect="1" aria-haspopup="true"><a href="javascript:;" class="k-menu__link "><span class="k-menu__link-text">Reviewed Feedbacks</span></a></li>
                                                        <li class="k-menu__item " data-kmenu-link-redirect="1" aria-haspopup="true"><a href="javascript:;" class="k-menu__link "><span class="k-menu__link-text">Resolved Feedbacks</span></a></li>
                                                        <li class="k-menu__item " data-kmenu-link-redirect="1" aria-haspopup="true"><a href="javascript:;" class="k-menu__link "><span class="k-menu__link-text">Feedback Reports</span></a></li>
                                                    </ul>
                                                </div>
                                                </li>
                                                <li class="k-menu__item " data-kmenu-link-redirect="1" aria-haspopup="true"><a href="javascript:;" class="k-menu__link "><i class="k-menu__link-bullet k-menu__link-bullet--dot"><span></span></i><span class="k-menu__link-text">Register Member</span></a></li>
                                            </ul>
                                        </div>
                                    </li>
                                    <li class="k-menu__item  k-menu__item--submenu k-menu__item--rel" data-kmenu-submenu-toggle="click" data-kmenu-link-redirect="1" aria-haspopup="true"><a href="javascript:;" class="k-menu__link k-menu__toggle">
                                        <span class="k-menu__link-text">报表</span>
                                        <i class="k-menu__hor-arrow la la-angle-down"></i>
                                        <i class="k-menu__ver-arrow la la-angle-right"></i>
                                    </a>
                                        <div class="k-menu__submenu  k-menu__submenu--fixed k-menu__submenu--left" style="width: 1000px">
                                            <div class="k-menu__subnav">
                                                <ul class="k-menu__content">
                                                    <li class="k-menu__item ">
                                                        <h3 class="k-menu__heading k-menu__toggle"><i class="k-menu__link-bullet k-menu__link-bullet--dot"><span></span></i><span class="k-menu__link-text">Finance Reports</span><i class="k-menu__ver-arrow la la-angle-right"></i></h3>
                                                        <ul class="k-menu__inner">
                                                            <li class="k-menu__item " data-kmenu-link-redirect="1" aria-haspopup="true"><a href="javascript:;" class="k-menu__link "><i class="k-menu__link-icon flaticon-map"></i><span class="k-menu__link-text">Annual Reports</span></a></li>
                                                            <li class="k-menu__item " data-kmenu-link-redirect="1" aria-haspopup="true"><a href="javascript:;" class="k-menu__link "><i class="k-menu__link-icon flaticon-user"></i><span class="k-menu__link-text">HR Reports</span></a></li>
                                                            <li class="k-menu__item " data-kmenu-link-redirect="1" aria-haspopup="true"><a href="javascript:;" class="k-menu__link "><i class="k-menu__link-icon flaticon-clipboard"></i><span class="k-menu__link-text">IPO Reports</span></a></li>
                                                            <li class="k-menu__item " data-kmenu-link-redirect="1" aria-haspopup="true"><a href="javascript:;" class="k-menu__link "><i class="k-menu__link-icon flaticon-graphic-1"></i><span class="k-menu__link-text">Finance Margins</span></a></li>
                                                            <li class="k-menu__item " data-kmenu-link-redirect="1" aria-haspopup="true"><a href="javascript:;" class="k-menu__link "><i class="k-menu__link-icon flaticon-graphic-2"></i><span class="k-menu__link-text">Revenue Reports</span></a></li>
                                                        </ul>
                                                    </li>
                                                    <li class="k-menu__item ">
                                                        <h3 class="k-menu__heading k-menu__toggle"><i class="k-menu__link-bullet k-menu__link-bullet--dot"><span></span></i><span class="k-menu__link-text">Project Reports</span><i class="k-menu__ver-arrow la la-angle-right"></i></h3>
                                                        <ul class="k-menu__inner">
                                                            <li class="k-menu__item " data-kmenu-link-redirect="1" aria-haspopup="true"><a href="javascript:;" class="k-menu__link "><i class="k-menu__link-bullet k-menu__link-bullet--line"><span></span></i><span class="k-menu__link-text">Coca Cola CRM</span></a></li>
                                                            <li class="k-menu__item " data-kmenu-link-redirect="1" aria-haspopup="true"><a href="javascript:;" class="k-menu__link "><i class="k-menu__link-bullet k-menu__link-bullet--line"><span></span></i><span class="k-menu__link-text">Delta Airlines Booking Site</span></a></li>
                                                            <li class="k-menu__item " data-kmenu-link-redirect="1" aria-haspopup="true"><a href="javascript:;" class="k-menu__link "><i class="k-menu__link-bullet k-menu__link-bullet--line"><span></span></i><span class="k-menu__link-text">Malibu Accounting</span></a></li>
                                                            <li class="k-menu__item " data-kmenu-link-redirect="1" aria-haspopup="true"><a href="javascript:;" class="k-menu__link "><i class="k-menu__link-bullet k-menu__link-bullet--line"><span></span></i><span class="k-menu__link-text">Vineseed Website Rewamp</span></a></li>
                                                            <li class="k-menu__item " data-kmenu-link-redirect="1" aria-haspopup="true"><a href="javascript:;" class="k-menu__link "><i class="k-menu__link-bullet k-menu__link-bullet--line"><span></span></i><span class="k-menu__link-text">Zircon Mobile App</span></a></li>
                                                            <li class="k-menu__item " data-kmenu-link-redirect="1" aria-haspopup="true"><a href="javascript:;" class="k-menu__link "><i class="k-menu__link-bullet k-menu__link-bullet--line"><span></span></i><span class="k-menu__link-text">Mercury CMS</span></a></li>
                                                        </ul>
                                                    </li>
                                                    <li class="k-menu__item ">
                                                        <h3 class="k-menu__heading k-menu__toggle"><i class="k-menu__link-bullet k-menu__link-bullet--dot"><span></span></i><span class="k-menu__link-text">HR Reports</span><i class="k-menu__ver-arrow la la-angle-right"></i></h3>
                                                        <ul class="k-menu__inner">
                                                            <li class="k-menu__item " data-kmenu-link-redirect="1" aria-haspopup="true"><a href="javascript:;" class="k-menu__link "><i class="k-menu__link-bullet k-menu__link-bullet--dot"><span></span></i><span class="k-menu__link-text">Staff Directory</span></a></li>
                                                            <li class="k-menu__item " data-kmenu-link-redirect="1" aria-haspopup="true"><a href="javascript:;" class="k-menu__link "><i class="k-menu__link-bullet k-menu__link-bullet--dot"><span></span></i><span class="k-menu__link-text">Client Directory</span></a></li>
                                                            <li class="k-menu__item " data-kmenu-link-redirect="1" aria-haspopup="true"><a href="javascript:;" class="k-menu__link "><i class="k-menu__link-bullet k-menu__link-bullet--dot"><span></span></i><span class="k-menu__link-text">Salary Reports</span></a></li>
                                                            <li class="k-menu__item " data-kmenu-link-redirect="1" aria-haspopup="true"><a href="javascript:;" class="k-menu__link "><i class="k-menu__link-bullet k-menu__link-bullet--dot"><span></span></i><span class="k-menu__link-text">Staff Payslips</span></a></li>
                                                            <li class="k-menu__item " data-kmenu-link-redirect="1" aria-haspopup="true"><a href="javascript:;" class="k-menu__link "><i class="k-menu__link-bullet k-menu__link-bullet--dot"><span></span></i><span class="k-menu__link-text">Corporate Expenses</span></a></li>
                                                            <li class="k-menu__item " data-kmenu-link-redirect="1" aria-haspopup="true"><a href="javascript:;" class="k-menu__link "><i class="k-menu__link-bullet k-menu__link-bullet--dot"><span></span></i><span class="k-menu__link-text">Project Expenses</span></a></li>
                                                        </ul>
                                                    </li>
                                                    <li class="k-menu__item ">
                                                        <h3 class="k-menu__heading k-menu__toggle"><i class="k-menu__link-bullet k-menu__link-bullet--dot"><span></span></i><span class="k-menu__link-text">Reporting Apps</span><i class="k-menu__ver-arrow la la-angle-right"></i></h3>
                                                        <ul class="k-menu__inner">
                                                            <li class="k-menu__item " data-kmenu-link-redirect="1" aria-haspopup="true"><a href="javascript:;" class="k-menu__link "><span class="k-menu__link-text">Report Adjusments</span></a></li>
                                                            <li class="k-menu__item " data-kmenu-link-redirect="1" aria-haspopup="true"><a href="javascript:;" class="k-menu__link "><span class="k-menu__link-text">Sources & Mediums</span></a></li>
                                                            <li class="k-menu__item " data-kmenu-link-redirect="1" aria-haspopup="true"><a href="javascript:;" class="k-menu__link "><span class="k-menu__link-text">Reporting Settings</span></a></li>
                                                            <li class="k-menu__item " data-kmenu-link-redirect="1" aria-haspopup="true"><a href="javascript:;" class="k-menu__link "><span class="k-menu__link-text">Conversions</span></a></li>
                                                            <li class="k-menu__item " data-kmenu-link-redirect="1" aria-haspopup="true"><a href="javascript:;" class="k-menu__link "><span class="k-menu__link-text">Report Flows</span></a></li>
                                                            <li class="k-menu__item " data-kmenu-link-redirect="1" aria-haspopup="true"><a href="javascript:;" class="k-menu__link "><span class="k-menu__link-text">Audit & Logs</span></a></li>
                                                        </ul>
                                                    </li>
                                                </ul>
                                            </div>
                                        </div>
                                    </li>
                                    <li class="k-menu__item  k-menu__item--submenu k-menu__item--rel" data-kmenu-submenu-toggle="click" data-kmenu-link-redirect="1" aria-haspopup="true">
                                        <a href="javascript:;" class="k-menu__link k-menu__toggle">
                                            <span class="k-menu__link-text">应用</span>
                                            <i class="k-menu__hor-arrow la la-angle-down"></i>
                                            <i class="k-menu__ver-arrow la la-angle-right"></i>
                                        </a>
                                        <div class="k-menu__submenu k-menu__submenu--classic k-menu__submenu--left">
                                            <ul class="k-menu__subnav">
                                                <li class="k-menu__item " data-kmenu-link-redirect="1" aria-haspopup="true"><a href="javascript:;" class="k-menu__link "><i class="k-menu__link-bullet k-menu__link-bullet--dot"><span></span></i><span class="k-menu__link-text">eCommerce</span></a></li>
                                                <li class="k-menu__item  k-menu__item--submenu" data-kmenu-submenu-toggle="hover" data-kmenu-link-redirect="1" aria-haspopup="true"><a href="/UI/keenthemes.com/keen/preview/default/components/datatable_v1.html" class="k-menu__link k-menu__toggle"><i class="k-menu__link-bullet k-menu__link-bullet--dot"><span></span></i><span class="k-menu__link-text">Audience</span><i class="k-menu__hor-arrow la la-angle-right"></i><i class="k-menu__ver-arrow la la-angle-right"></i></a><div class="k-menu__submenu k-menu__submenu--classic k-menu__submenu--right">
                                                    <ul class="k-menu__subnav">
                                                        <li class="k-menu__item " data-kmenu-link-redirect="1" aria-haspopup="true"><a href="javascript:;" class="k-menu__link "><i class="k-menu__link-icon flaticon-users"></i><span class="k-menu__link-text">Active Users</span></a></li>
                                                        <li class="k-menu__item " data-kmenu-link-redirect="1" aria-haspopup="true"><a href="javascript:;" class="k-menu__link "><i class="k-menu__link-icon flaticon-interface-1"></i><span class="k-menu__link-text">User Explorer</span></a></li>
                                                        <li class="k-menu__item " data-kmenu-link-redirect="1" aria-haspopup="true"><a href="javascript:;" class="k-menu__link "><i class="k-menu__link-icon flaticon-lifebuoy"></i><span class="k-menu__link-text">Users Flows</span></a></li>
                                                        <li class="k-menu__item " data-kmenu-link-redirect="1" aria-haspopup="true"><a href="javascript:;" class="k-menu__link "><i class="k-menu__link-icon flaticon-graphic-1"></i><span class="k-menu__link-text">Market Segments</span></a></li>
                                                        <li class="k-menu__item " data-kmenu-link-redirect="1" aria-haspopup="true"><a href="javascript:;" class="k-menu__link "><i class="k-menu__link-icon flaticon-graphic"></i><span class="k-menu__link-text">User Reports</span></a></li>
                                                    </ul>
                                                </div>
                                                </li>
                                                <li class="k-menu__item " data-kmenu-link-redirect="1" aria-haspopup="true"><a href="javascript:;" class="k-menu__link "><i class="k-menu__link-bullet k-menu__link-bullet--dot"><span></span></i><span class="k-menu__link-text">Marketing</span></a></li>
                                                <li class="k-menu__item " data-kmenu-link-redirect="1" aria-haspopup="true"><a href="javascript:;" class="k-menu__link "><i class="k-menu__link-bullet k-menu__link-bullet--dot"><span></span></i><span class="k-menu__link-text">Campaigns</span><span class="k-menu__link-badge"><span class="k-badge k-badge--success">3</span></span></a></li>
                                                <li class="k-menu__item  k-menu__item--submenu" data-kmenu-submenu-toggle="hover" data-kmenu-link-redirect="1" aria-haspopup="true"><a href="javascript:;" class="k-menu__link k-menu__toggle"><i class="k-menu__link-bullet k-menu__link-bullet--dot"><span></span></i><span class="k-menu__link-text">Cloud Manager</span><i class="k-menu__hor-arrow la la-angle-right"></i><i class="k-menu__ver-arrow la la-angle-right"></i></a><div class="k-menu__submenu k-menu__submenu--classic k-menu__submenu--right">
                                                    <ul class="k-menu__subnav">
                                                        <li class="k-menu__item " data-kmenu-link-redirect="1" aria-haspopup="true"><a href="javascript:;" class="k-menu__link "><i class="k-menu__link-icon flaticon-add"></i><span class="k-menu__link-text">File Upload</span><span class="k-menu__link-badge"><span class="k-badge k-badge--danger">3</span></span></a></li>
                                                        <li class="k-menu__item " data-kmenu-link-redirect="1" aria-haspopup="true"><a href="javascript:;" class="k-menu__link "><i class="k-menu__link-icon flaticon-signs-1"></i><span class="k-menu__link-text">File Attributes</span></a></li>
                                                        <li class="k-menu__item " data-kmenu-link-redirect="1" aria-haspopup="true"><a href="javascript:;" class="k-menu__link "><i class="k-menu__link-icon flaticon-folder"></i><span class="k-menu__link-text">Folders</span></a></li>
                                                        <li class="k-menu__item " data-kmenu-link-redirect="1" aria-haspopup="true"><a href="javascript:;" class="k-menu__link "><i class="k-menu__link-icon flaticon-cogwheel-2"></i><span class="k-menu__link-text">System Settings</span></a></li>
                                                    </ul>
                                                </div>
                                                </li>
                                            </ul>
                                        </div>
                                    </li>
                                </ul>
                            </div>
                        </div>
                        <!-- end:: Header Menu -->

                        <!-- begin:: Header Topbar -->
                        <div class="k-header__topbar">

                            <!--begin: Notifications -->
                            <div class="k-header__topbar-item dropdown">
                                <div class="k-header__topbar-wrapper" data-toggle="dropdown" data-offset="30px -2px" aria-expanded="true">
                                    <span class="k-header__topbar-icon">
                                        <i class="flaticon2-bell-alarm-symbol"></i>
                                        <span id="icoNotification" runat="server" class="k-badge k-badge--dot k-badge--notify k-badge--sm k-badge--brand"></span>
                                    </span>
                                </div>
                                <div class="dropdown-menu dropdown-menu-fit dropdown-menu-right dropdown-menu-anim dropdown-menu-top-unround dropdown-menu-lg">
                                    <div class="k-head" style="background-image: url('/UI/keenthemes.com/keen/preview/default/assets/media/misc/head_bg_sm.jpg')">
                                        <h3 class="k-head__title">通知</h3>
                                        <div class="k-head__sub"><span class="k-head__desc" id="txt_notifications_count" runat="server"></span></div>
                                    </div>
                                    <div class="k-notification k-margin-t-30 k-margin-b-20 k-scroll" data-scroll="true" data-height="270" data-mobile-height="220">
                                        <asp:ListView ID="livNotification" runat="server">
                                            <ItemTemplate>
                                                <a data-title="通知内容" data-url="<%#Eval("ProcessURL")%>&notificationsid=<%#Eval("ID")%>" class="k-notification__item">
                                                    <div class="k-notification__item-icon"><i class="la <%#Eval("ModuleTypeIcon")%>"></i></div>
                                                    <div class="k-notification__item-details">
                                                        <div class="k-notification__item-title"><%#Eval("Description")%></div>
                                                        <div class="k-notification__item-time"><%#Eval("UpdateTime")%></div>
                                                    </div>
                                                </a>
                                            </ItemTemplate>
                                        </asp:ListView>
                                    </div>
                                </div>
                            </div>
                            <!--end: Notifications -->

                            <!--begin: Quick Actions -->
                            <div class="k-header__topbar-item">
                                <div class="k-header__topbar-wrapper" id="k_offcanvas_toolbar_quick_actions_toggler_btn">
                                    <span class="k-header__topbar-icon"><i class="flaticon2-gear"></i></span>
                                </div>
                            </div>
                            <!--end: Quick Actions -->

                            <!--begin: User Bar -->
                            <div class="k-header__topbar-item k-header__topbar-item--user">
                                <!-- 按钮 -->
                                <div class="k-header__topbar-wrapper" data-toggle="dropdown" data-offset="0px -2px">
                                    <div class="k-header__topbar-user">
                                        <span class="k-header__topbar-welcome k-hidden-mobile">欢迎您！</span>
                                        <span id="txtLoginUserName" runat="server" class="k-header__topbar-username k-hidden-mobile" />
                                        <img id="imgLoginUserAvatar" runat="server" src="/media/users/avatar_default.png" />
                                        <span class="k-badge k-badge--username k-badge--lg k-badge--brand k-hidden k-badge--bold">S</span>
                                    </div>
                                </div>
                                <!-- 浮窗 -->
                                <div class="dropdown-menu dropdown-menu-fit dropdown-menu-right dropdown-menu-anim dropdown-menu-top-unround dropdown-menu-sm">
                                    <div class="k-user-card k-margin-b-50 k-margin-b-30-tablet-and-mobile"
                                        style="background-image: url('/UI/keenthemes.com/keen/preview/default/assets/media/misc/head_bg_sm.jpg')">
                                        <div class="k-user-card__wrapper">
                                            <div class="k-user-card__pic">
                                                <img id="imgLoginUserAvatar2" runat="server" src="/media/users/avatar_default.png" />
                                            </div>
                                            <div class="k-user-card__details">
                                                <div id="txtLoginUserName2" runat="server" class="k-user-card__name"></div>
                                                <div id="txtLoginUserRole" runat="server" class="k-user-card__position"></div>
                                            </div>
                                        </div>
                                    </div>

                                    <ul class="k-nav k-margin-b-10">
                                        <li class="k-nav__item">
                                            <a href="/UI/keenthemes.com/keen/preview/default/custom/user/profile-v1.html" class="k-nav__link">
                                                <span class="k-nav__link-icon"><i class="flaticon2-calendar-3"></i></span>
                                                <span class="k-nav__link-text">My Profile</span>
                                            </a>
                                        </li>
                                        <li class="k-nav__item">
                                            <a href="/UI/keenthemes.com/keen/preview/default/custom/user/profile-v1.html" class="k-nav__link">
                                                <span class="k-nav__link-icon"><i class="flaticon2-browser-2"></i></span>
                                                <span class="k-nav__link-text">My Tasks</span>
                                            </a>
                                        </li>
                                        <li class="k-nav__item">
                                            <a href="/UI/keenthemes.com/keen/preview/default/custom/user/profile-v1.html" class="k-nav__link">
                                                <span class="k-nav__link-icon"><i class="flaticon2-mail"></i></span>
                                                <span class="k-nav__link-text">Messages</span>
                                            </a>
                                        </li>
                                        <li class="k-nav__item">
                                            <a href="/UI/keenthemes.com/keen/preview/default/custom/user/profile-v1.html" class="k-nav__link">
                                                <span class="k-nav__link-icon"><i class="flaticon2-gear"></i></span>
                                                <span class="k-nav__link-text">Settings</span>
                                            </a>
                                        </li>
                                        <li class="k-nav__item k-nav__item--custom k-margin-t-15">
                                            <%--<a href="/UI/keenthemes.com/keen/preview/default/custom/user/login-v2.html" target="_blank"
                                            class="btn btn-outline-metal btn-hover-brand btn-upper btn-font-dark btn-sm btn-bold">Sign Out</a>--%>

                                            <button id="btnLogout" runat="server" class="btn btn-outline-metal btn-hover-brand btn-upper btn-font-dark btn-sm btn-bold"
                                                onserverclick="Logout_ServerClick">
                                                退出</button>

                                        </li>
                                    </ul>
                                </div>

                            </div>
                            <!--end: User Bar -->
                            <!--begin:: Quick Panel Toggler -->
                            <div class="k-header__topbar-item k-header__topbar-item--quick-panel" data-toggle="k-tooltip" title="Quick panel" data-placement="right">
                                <span class="k-header__topbar-icon" id="k_quick_panel_toggler_btn">
                                    <i class="flaticon2-grids"></i>

                                </span>
                            </div>
                            <!--end:: Quick Panel Toggler -->
                        </div>
                        <!-- end:: Header Topbar -->
                    </div>
                    <!-- end:: Header -->

                    <!-- begin:: Content -->
                    <div class="k-grid__item k-grid__item--fluid k-grid k-grid--hor page-content">
                        <!-- begin:: IFrame -->
                        <section class="Hui-article-box">
                            <div id="Hui-tabNav" class="Hui-tabNav">
                                <div class="Hui-tabNav-wp">
                                    <ul id="min_title_list" class="acrossTab">
                                        <li class="active"><span title="工作台" data-href="/MOD/SYS/Dashboard.aspx">工作台</span><em></em></li>
                                    </ul>
                                </div>
                                <div class="Hui-tabNav-more btn-group">
                                    <a id="js-tabNav-prev"><i class="fa fa-angle-left"></i></a>
                                    <a id="js-tabNav-next"><i class="fa fa-angle-right"></i></a>
                                </div>
                            </div>
                            <div id="iframe_box" class="Hui-article">
                                <div class="show_iframe">
                                    <div class="loading" style="display: none;"></div>
                                    <iframe src="/MOD/BI/Dashboard.aspx"></iframe>
                                </div>
                            </div>
                        </section>
                        <!-- end:: IFrame -->
                    </div>
                    <!-- end:: Content -->

                </div>
                <!-- end:: Wrapper -->

            </div>
            <!-- end:: Page -->
        </div>
        <!-- end:: Root -->


        <!-- begin:: Topbar Offcanvas Panels -->

        <!-- begin::Offcanvas Toolbar Quick Actions -->
        <div id="k_offcanvas_toolbar_quick_actions" class="k-offcanvas-panel">
            <div class="k-offcanvas-panel__head">
                <h3 class="k-offcanvas-panel__title">Quick Actions
                </h3>
                <a href="#" class="k-offcanvas-panel__close" id="k_offcanvas_toolbar_quick_actions_close"><i class="flaticon2-delete"></i></a>
            </div>
            <div class="k-offcanvas-panel__body">
                <div class="k-grid-nav-v2">
                    <a href="#" class="k-grid-nav-v2__item">
                        <div class="k-grid-nav-v2__item-icon"><i class="flaticon2-box"></i></div>
                        <div class="k-grid-nav-v2__item-title">Orders</div>
                    </a>
                    <a href="#" class="k-grid-nav-v2__item">
                        <div class="k-grid-nav-v2__item-icon"><i class="flaticon-download-1"></i></div>
                        <div class="k-grid-nav-v2__item-title">Uploades</div>
                    </a>
                    <a href="#" class="k-grid-nav-v2__item">
                        <div class="k-grid-nav-v2__item-icon"><i class="flaticon2-supermarket"></i></div>
                        <div class="k-grid-nav-v2__item-title">Products</div>
                    </a>
                    <a href="#" class="k-grid-nav-v2__item">
                        <div class="k-grid-nav-v2__item-icon"><i class="flaticon2-avatar"></i></div>
                        <div class="k-grid-nav-v2__item-title">Customers</div>
                    </a>
                    <a href="#" class="k-grid-nav-v2__item">
                        <div class="k-grid-nav-v2__item-icon"><i class="flaticon2-list"></i></div>
                        <div class="k-grid-nav-v2__item-title">Blog Posts</div>
                    </a>
                    <a href="#" class="k-grid-nav-v2__item">
                        <div class="k-grid-nav-v2__item-icon"><i class="flaticon2-settings"></i></div>
                        <div class="k-grid-nav-v2__item-title">Settings</div>
                    </a>
                </div>
            </div>
        </div>
        <!-- end::Offcanvas Toolbar Quick Actions -->
        <!-- end:: Topbar Offcanvas Panels -->

        <!-- begin:: Quick Panel -->
        <div id="k_quick_panel" class="k-offcanvas-panel">
            <div class="k-offcanvas-panel__nav">
                <ul class="nav nav-pills" role="tablist">
                    <li class="nav-item active">
                        <a class="nav-link active" data-toggle="tab" href="#k_quick_panel_tab_notifications" role="tab">Notifications</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" data-toggle="tab" href="#k_quick_panel_tab_actions" role="tab">Actions</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" data-toggle="tab" href="#k_quick_panel_tab_settings" role="tab">Settings</a>
                    </li>
                </ul>

                <button class="k-offcanvas-panel__close" id="k_quick_panel_close_btn"><i class="flaticon2-delete"></i></button>
            </div>

            <div class="k-offcanvas-panel__body">
                <div class="tab-content">
                    <div class="tab-pane fade show k-offcanvas-panel__content k-scroll active" id="k_quick_panel_tab_notifications" role="tabpanel">
                        <!--Begin::Timeline -->
                        <div class="k-timeline">
                            <!--Begin::Item -->
                            <div class="k-timeline__item k-timeline__item--success">
                                <div class="k-timeline__item-section">
                                    <div class="k-timeline__item-section-border">
                                        <div class="k-timeline__item-section-icon">
                                            <i class="flaticon-feed k-font-success"></i>
                                        </div>
                                    </div>
                                    <span class="k-timeline__item-datetime">02:30 PM</span>
                                </div>

                                <a class="k-timeline__item-text">KeenThemes created new layout whith tens of new options for Keen Admin panel                                                                                             
                                </a>
                                <div class="k-timeline__item-info">
                                    HTML,CSS,VueJS                                                                                            
                                </div>
                            </div>
                            <!--End::Item -->

                            <!--Begin::Item -->
                            <div class="k-timeline__item k-timeline__item--danger">
                                <div class="k-timeline__item-section">
                                    <div class="k-timeline__item-section-border">
                                        <div class="k-timeline__item-section-icon">
                                            <i class="flaticon-safe-shield-protection k-font-danger"></i>
                                        </div>
                                    </div>
                                    <span class="k-timeline__item-datetime">01:20 AM</span>
                                </div>

                                <a class="k-timeline__item-text">New secyrity alert by Firewall & order to take aktion on User Preferences                                                                                             
                                </a>
                                <div class="k-timeline__item-info">
                                    Security, Fieewall                                                                                         
                                </div>
                            </div>
                            <!--End::Item -->

                            <!--Begin::Item -->
                            <div class="k-timeline__item k-timeline__item--brand">
                                <div class="k-timeline__item-section">
                                    <div class="k-timeline__item-section-border">
                                        <div class="k-timeline__item-section-icon">
                                            <i class="flaticon2-box k-font-brand"></i>
                                        </div>
                                    </div>
                                    <span class="k-timeline__item-datetime">Yestardey</span>
                                </div>

                                <a class="k-timeline__item-text">FlyMore design mock-ups been uploadet by designers Bob, Naomi, Richard                                                                                            
                                </a>
                                <div class="k-timeline__item-info">
                                    PSD, Sketch, AJ                                                                                       
                                </div>
                            </div>
                            <!--End::Item -->

                            <!--Begin::Item -->
                            <div class="k-timeline__item k-timeline__item--warning">
                                <div class="k-timeline__item-section">
                                    <div class="k-timeline__item-section-border">
                                        <div class="k-timeline__item-section-icon">
                                            <i class="flaticon-pie-chart-1 k-font-warning"></i>
                                        </div>
                                    </div>
                                    <span class="k-timeline__item-datetime">Aug 13,2018</span>
                                </div>

                                <a class="k-timeline__item-text">Meeting with Ken Digital Corp ot Unit14, 3 Edigor Buildings, George Street, Loondon<br>
                                    England, BA12FJ                                                                                           
                                </a>
                                <div class="k-timeline__item-info">
                                    Meeting, Customer                                                                                          
                                </div>
                            </div>
                            <!--End::Item -->

                            <!--Begin::Item -->
                            <div class="k-timeline__item k-timeline__item--info">
                                <div class="k-timeline__item-section">
                                    <div class="k-timeline__item-section-border">
                                        <div class="k-timeline__item-section-icon">
                                            <i class="flaticon-notepad k-font-info"></i>
                                        </div>
                                    </div>
                                    <span class="k-timeline__item-datetime">May 09, 2018</span>
                                </div>

                                <a class="k-timeline__item-text">KeenThemes created new layout whith tens of new options for Keen Admin panel                                                                                                
                                </a>
                                <div class="k-timeline__item-info">
                                    HTML,CSS,VueJS                                                                                            
                                </div>
                            </div>
                            <!--End::Item -->

                            <!--Begin::Item -->
                            <div class="k-timeline__item k-timeline__item--accent">
                                <div class="k-timeline__item-section">
                                    <div class="k-timeline__item-section-border">
                                        <div class="k-timeline__item-section-icon">
                                            <i class="flaticon-bell k-font-accent"></i>
                                        </div>
                                    </div>
                                    <span class="k-timeline__item-datetime">01:20 AM</span>
                                </div>

                                <a class="k-timeline__item-text">New secyrity alert by Firewall & order to take aktion on User Preferences                                                                                             
                                </a>
                                <div class="k-timeline__item-info">
                                    Security, Fieewall                                                                                         
                                </div>
                            </div>
                            <!--End::Item -->

                        </div>
                        <!--End::Timeline -->
                    </div>
                    <div class="tab-pane fade k-offcanvas-panel__content k-scroll" id="k_quick_panel_tab_actions" role="tabpanel">
                        <!--begin::Portlet-->
                        <div class="k-portlet k-portlet--solid-success">
                            <div class="k-portlet__head">
                                <div class="k-portlet__head-label">
                                    <span class="k-portlet__head-icon k-hide"><i class="flaticon-stopwatch"></i></span>
                                    <h3 class="k-portlet__head-title">Recent Bills</h3>
                                </div>
                                <div class="k-portlet__head-toolbar">
                                    <div class="k-portlet__head-group">
                                        <div class="dropdown dropdown-inline">
                                            <button type="button" class="btn btn-sm btn-font-light btn-outline-hover-light btn-circle btn-icon" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                                                <i class="flaticon-more"></i>
                                            </button>
                                            <div class="dropdown-menu dropdown-menu-right">
                                                <a class="dropdown-item" href="#">Action</a>
                                                <a class="dropdown-item" href="#">Another action</a>
                                                <a class="dropdown-item" href="#">Something else here</a>
                                                <div class="dropdown-divider"></div>
                                                <a class="dropdown-item" href="#">Separated link</a>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="k-portlet__body">
                                <div class="k-portlet__content">
                                    Lorem Ipsum is simply dummy text of the printing and typesetting simply dummy text of the printing industry. 
                                </div>
                            </div>
                            <div class="k-portlet__foot k-portlet__foot--sm k-align-right">
                                <a href="#" class="btn btn-bold btn-upper btn-sm btn-font-light btn-outline-hover-light">Dismiss</a>
                                <a href="#" class="btn btn-bold btn-upper btn-sm btn-font-light btn-outline-hover-light">View</a>
                            </div>
                        </div>
                        <!--end::Portlet-->

                        <!--begin::Portlet-->
                        <div class="k-portlet k-portlet--solid-focus">
                            <div class="k-portlet__head">
                                <div class="k-portlet__head-label">
                                    <span class="k-portlet__head-icon k-hide"><i class="flaticon-stopwatch"></i></span>
                                    <h3 class="k-portlet__head-title">Latest Orders</h3>
                                </div>
                                <div class="k-portlet__head-toolbar">
                                    <div class="k-portlet__head-group">
                                        <div class="dropdown dropdown-inline">
                                            <button type="button" class="btn btn-sm btn-font-light btn-outline-hover-light btn-circle btn-icon" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                                                <i class="flaticon-more"></i>
                                            </button>
                                            <div class="dropdown-menu dropdown-menu-right">
                                                <a class="dropdown-item" href="#">Action</a>
                                                <a class="dropdown-item" href="#">Another action</a>
                                                <a class="dropdown-item" href="#">Something else here</a>
                                                <div class="dropdown-divider"></div>
                                                <a class="dropdown-item" href="#">Separated link</a>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="k-portlet__body">
                                <div class="k-portlet__content">
                                    Lorem Ipsum is simply dummy text of the printing and typesetting simply dummy text of the printing industry. 
                                </div>
                            </div>
                            <div class="k-portlet__foot k-portlet__foot--sm k-align-right">
                                <a href="#" class="btn btn-bold btn-upper btn-sm btn-font-light btn-outline-hover-light">Dismiss</a>
                                <a href="#" class="btn btn-bold btn-upper btn-sm btn-font-light btn-outline-hover-light">View</a>
                            </div>
                        </div>
                        <!--end::Portlet-->

                        <!--begin::Portlet-->
                        <div class="k-portlet k-portlet--solid-info">
                            <div class="k-portlet__head">
                                <div class="k-portlet__head-label">
                                    <h3 class="k-portlet__head-title">Latest Invoices</h3>
                                </div>
                                <div class="k-portlet__head-toolbar">
                                    <div class="k-portlet__head-group">
                                        <div class="dropdown dropdown-inline">
                                            <button type="button" class="btn btn-sm btn-font-light btn-outline-hover-light btn-circle btn-icon" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                                                <i class="flaticon-more"></i>
                                            </button>
                                            <div class="dropdown-menu dropdown-menu-right">
                                                <a class="dropdown-item" href="#">Action</a>
                                                <a class="dropdown-item" href="#">Another action</a>
                                                <a class="dropdown-item" href="#">Something else here</a>
                                                <div class="dropdown-divider"></div>
                                                <a class="dropdown-item" href="#">Separated link</a>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="k-portlet__body">
                                <div class="k-portlet__content">
                                    Lorem Ipsum is simply dummy text of the printing and typesetting simply dummy text of the printing industry. 
                                </div>
                            </div>
                            <div class="k-portlet__foot k-portlet__foot--sm k-align-right">
                                <a href="#" class="btn btn-bold btn-upper btn-sm btn-font-light btn-outline-hover-light">Dismiss</a>
                                <a href="#" class="btn btn-bold btn-upper btn-sm btn-font-light btn-outline-hover-light">View</a>
                            </div>
                        </div>
                        <!--end::Portlet-->

                        <!--begin::Portlet-->
                        <div class="k-portlet k-portlet--solid-warning">
                            <div class="k-portlet__head">
                                <div class="k-portlet__head-label">
                                    <h3 class="k-portlet__head-title">New Comments</h3>
                                </div>
                                <div class="k-portlet__head-toolbar">
                                    <div class="k-portlet__head-group">
                                        <div class="dropdown dropdown-inline">
                                            <button type="button" class="btn btn-sm btn-font-light btn-outline-hover-light btn-circle btn-icon" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                                                <i class="flaticon-more"></i>
                                            </button>
                                            <div class="dropdown-menu dropdown-menu-right">
                                                <a class="dropdown-item" href="#">Action</a>
                                                <a class="dropdown-item" href="#">Another action</a>
                                                <a class="dropdown-item" href="#">Something else here</a>
                                                <div class="dropdown-divider"></div>
                                                <a class="dropdown-item" href="#">Separated link</a>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="k-portlet__body">
                                <div class="k-portlet__content">
                                    Lorem Ipsum is simply dummy text of the printing and typesetting simply dummy text of the printing industry. 
                                </div>
                            </div>
                            <div class="k-portlet__foot k-portlet__foot--sm k-align-right">
                                <a href="#" class="btn btn-bold btn-upper btn-sm btn-font-light btn-outline-hover-light">Dismiss</a>
                                <a href="#" class="btn btn-bold btn-upper btn-sm btn-font-light btn-outline-hover-light">View</a>
                            </div>
                        </div>
                        <!--end::Portlet-->

                        <!--begin::Portlet-->
                        <div class="k-portlet k-portlet--solid-brand">
                            <div class="k-portlet__head">
                                <div class="k-portlet__head-label">
                                    <h3 class="k-portlet__head-title">Recent Posts</h3>
                                </div>
                                <div class="k-portlet__head-toolbar">
                                    <div class="k-portlet__head-group">
                                        <div class="dropdown dropdown-inline">
                                            <button type="button" class="btn btn-sm btn-font-light btn-outline-hover-light btn-circle btn-icon" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                                                <i class="flaticon-more"></i>
                                            </button>
                                            <div class="dropdown-menu dropdown-menu-right">
                                                <a class="dropdown-item" href="#">Action</a>
                                                <a class="dropdown-item" href="#">Another action</a>
                                                <a class="dropdown-item" href="#">Something else here</a>
                                                <div class="dropdown-divider"></div>
                                                <a class="dropdown-item" href="#">Separated link</a>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="k-portlet__body">
                                <div class="k-portlet__content">
                                    Lorem Ipsum is simply dummy text of the printing and typesetting simply dummy text of the printing industry. 
                                </div>
                            </div>
                            <div class="k-portlet__foot k-portlet__foot--sm k-align-right">
                                <a href="#" class="btn btn-bold btn-upper btn-sm btn-font-light btn-outline-hover-light">Dismiss</a>
                                <a href="#" class="btn btn-bold btn-upper btn-sm btn-font-light btn-outline-hover-light">View</a>
                            </div>
                        </div>
                        <!--end::Portlet-->
                    </div>
                    <div class="tab-pane fade k-offcanvas-panel__content k-scroll" id="k_quick_panel_tab_settings" role="tabpanel">
                        <div class="k-form">
                            <div class="k-heading k-heading--space-sm">Notifications</div>

                            <div class="form-group form-group-xs row">
                                <label class="col-8 col-form-label">Enable notifications:</label>
                                <div class="col-4 k-align-right">
                                    <span class="k-switch k-switch--sm">
                                        <label>
                                            <input type="checkbox" checked="checked" name="quick_panel_notifications_1">
                                            <span></span>
                                        </label>
                                    </span>
                                </div>
                            </div>
                            <div class="form-group form-group-xs row">
                                <label class="col-8 col-form-label">Enable audit log:</label>
                                <div class="col-4 k-align-right">
                                    <span class="k-switch k-switch--sm">
                                        <label>
                                            <input type="checkbox" name="quick_panel_notifications_2">
                                            <span></span>
                                        </label>
                                    </span>
                                </div>
                            </div>
                            <div class="form-group form-group-last form-group-xs row">
                                <label class="col-8 col-form-label">Notify on new orders:</label>
                                <div class="col-4 k-align-right">
                                    <span class="k-switch k-switch--sm">
                                        <label>
                                            <input type="checkbox" checked="checked" name="quick_panel_notifications_2">
                                            <span></span>
                                        </label>
                                    </span>
                                </div>
                            </div>

                            <div class="k-separator k-separator--space-md k-separator--border-dashed"></div>

                            <div class="k-heading k-heading--space-sm">Orders</div>

                            <div class="form-group form-group-xs row">
                                <label class="col-8 col-form-label">Enable order tracking:</label>
                                <div class="col-4 k-align-right">
                                    <span class="k-switch k-switch--sm k-switch--danger">
                                        <label>
                                            <input type="checkbox" checked="checked" name="quick_panel_notifications_3">
                                            <span></span>
                                        </label>
                                    </span>
                                </div>
                            </div>
                            <div class="form-group form-group-xs row">
                                <label class="col-8 col-form-label">Enable orders reports:</label>
                                <div class="col-4 k-align-right">
                                    <span class="k-switch k-switch--sm k-switch--danger">
                                        <label>
                                            <input type="checkbox" name="quick_panel_notifications_3">
                                            <span></span>
                                        </label>
                                    </span>
                                </div>
                            </div>
                            <div class="form-group form-group-last form-group-xs row">
                                <label class="col-8 col-form-label">Allow order status auto update:</label>
                                <div class="col-4 k-align-right">
                                    <span class="k-switch k-switch--sm k-switch--danger">
                                        <label>
                                            <input type="checkbox" checked="checked" name="quick_panel_notifications_4">
                                            <span></span>
                                        </label>
                                    </span>
                                </div>
                            </div>

                            <div class="k-separator k-separator--space-md k-separator--border-dashed"></div>

                            <div class="k-heading k-heading--space-sm">Customers</div>

                            <div class="form-group form-group-xs row">
                                <label class="col-8 col-form-label">Enable customer singup:</label>
                                <div class="col-4 k-align-right">
                                    <span class="k-switch k-switch--sm k-switch--success">
                                        <label>
                                            <input type="checkbox" checked="checked" name="quick_panel_notifications_5">
                                            <span></span>
                                        </label>
                                    </span>
                                </div>
                            </div>
                            <div class="form-group form-group-xs row">
                                <label class="col-8 col-form-label">Enable customers reporting:</label>
                                <div class="col-4 k-align-right">
                                    <span class="k-switch k-switch--sm k-switch--success">
                                        <label>
                                            <input type="checkbox" name="quick_panel_notifications_5">
                                            <span></span>
                                        </label>
                                    </span>
                                </div>
                            </div>
                            <div class="form-group form-group-last form-group-xs row">
                                <label class="col-8 col-form-label">Notifiy on new customer registration:</label>
                                <div class="col-4 k-align-right">
                                    <span class="k-switch k-switch--sm k-switch--success">
                                        <label>
                                            <input type="checkbox" checked="checked" name="quick_panel_notifications_6">
                                            <span></span>
                                        </label>
                                    </span>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- end:: Quick Panel -->


        <!-- begin:: Scrolltop -->
        <div id="k_scrolltop" class="k-scrolltop">
            <i class="la la-arrow-up"></i>
        </div>
        <!-- end:: Scrolltop -->

        <!-- begin:: Demo Toolbar -->
        <ul class="k-sticky-toolbar" style="margin-top: 30px; display: none;">
            <li class="k-sticky-toolbar__item k-sticky-toolbar__item--demo-toggle" id="k_demo_panel_toggle" data-toggle="k-tooltip" title="Check out more demos" data-placement="right">
                <a href="#" class="">Demos</a>
            </li>
            <li class="k-sticky-toolbar__item k-sticky-toolbar__item--builder" data-toggle="k-tooltip" title="Layout Builder" data-placement="left">
                <a href="/UI/keenthemes.com/keen/preview/default/builder.html"><i class="flaticon2-box"></i></a>
            </li>
            <li class="k-sticky-toolbar__item k-sticky-toolbar__item--docs" data-toggle="k-tooltip" title="Documentation" data-placement="left">
                <a href="https://keenthemes.com/keen/?page=docs" target="_blank"><i class="flaticon2-file"></i></a>
            </li>
        </ul>
        <!-- end:: Demo Toolbar -->
        <!-- begin::Demo Panel -->
        <div id="k_demo_panel" class="k-demo-panel">
            <div class="k-demo-panel__head">
                <h3 class="k-demo-panel__title">Select A Demo</h3>
                <a href="#" class="k-demo-panel__close" id="k_demo_panel_close"><i class="flaticon2-delete"></i></a>
            </div>
            <div class="k-demo-panel__body">
                <div class="k-demo-panel__item k-demo-panel__item--active">
                    <div class="k-demo-panel__item-title">
                        Default
                    </div>
                    <div class="k-demo-panel__item-preview">
                        <img src="/UI/keenthemes.com/keen/preview/default/assets/media/demos-mini/default.png" alt="" />
                        <div class="k-demo-panel__item-preview-overlay">
                            <a href="/UI/keenthemes.com/keen/preview/default/index.html" class="btn btn-brand btn-elevate " target="_blank">Default</a>
                            <a href="/UI/keenthemes.com/keen/preview/default/rtl/index.html" class="btn btn-light btn-elevate" target="_blank">RTL Version</a>
                        </div>
                    </div>
                </div>
                <a href="https://themes.getbootstrap.com/product/keen-the-ultimate-bootstrap-admin-theme/"
                    target="_blank" class="k-demo-panel__purchase btn btn-brand btn-elevate btn-bold btn-upper">Buy Keen Now!
                </a>
            </div>
        </div>
        <!-- end::Demo Panel -->

        <!-- begin::Global Config(global config for global JS sciprts) -->
        <script>
            var KAppOptions = {
                "colors": {
                    "state": {
                        "brand": "#5d78ff",
                        "metal": "#c4c5d6",
                        "light": "#ffffff",
                        "accent": "#00c5dc",
                        "primary": "#5867dd",
                        "success": "#34bfa3",
                        "info": "#36a3f7",
                        "warning": "#ffb822",
                        "danger": "#fd3995",
                        "focus": "#9816f4"
                    },
                    "base": {
                        "label": [
                            "#c5cbe3",
                            "#a1a8c3",
                            "#3d4465",
                            "#3e4466"
                        ],
                        "shape": [
                            "#f0f3ff",
                            "#d9dffa",
                            "#afb4d4",
                            "#646c9a"
                        ]
                    }
                }
            };
        </script>
        <!-- end::Global Config -->

        <!--begin::Global Theme Bundle(used by all pages) -->
        <script src="/UI/keenthemes.com/keen/preview/default/assets/vendors/base/vendors.bundle.copy.js" type="text/javascript"></script>
        <script src="/UI/keenthemes.com/keen/preview/default/assets/demo/default/base/scripts.bundle.js" type="text/javascript"></script>
        <!--end::Global Theme Bundle -->

        <!--begin::Global App Bundle(used by all pages) -->
        <script src="/UI/keenthemes.com/keen/preview/default/assets/app/scripts/bundle/app.bundle.js" type="text/javascript"></script>
        <!--end::Global App Bundle -->

        <%--<script type="text/javascript" src="/UI/www.datatables.club/example/static/h-ui/js/H-ui.js"></script>--%>
        <script type="text/javascript" src="/JS/Hui-tabNav.js"></script>
        <script>
            //修正左菜单偏移
            var navhidden = false;
            $(document).on('click', '#k_aside_toggler', function () {
                navhidden = !navhidden;
                $('.page-content').width('calc(100% - ' + (navhidden ? '78' : '260') + 'px)');
                $('.Hui-article').width('calc(100% - ' + (navhidden ? '78' : '260') + 'px)');
            });
            $(document).on('click', '#k_aside_mobile_toggler', function () {
                $('.page-content').width('100%');
                $('.Hui-article').width('100%');
            });
        </script>
    </form>
</body>
</html>
