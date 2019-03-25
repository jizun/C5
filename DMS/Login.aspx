<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="DMS.Login" %>

<!DOCTYPE html>
<html lang="zh-cn">
<head>
    <meta charset="utf-8" />
    <meta name="description" content="DMS Login">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />

    <title>后台管理登录 | DMS - ZIRI</title>

    <!--begin::Page Custom Styles(used by this page) -->
    <link href="/UI/keenthemes.com/keen/preview/default/assets/custom/user/login-v1.css" rel="stylesheet" type="text/css" />
    <!--end::Page Custom Styles -->

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

    <style type="text/css">
        body {
            background-image: url(/media/misc/bg_1.jpg);
        }

        #LoginForm, .k-grid--root {
            height: 100%;
        }

        .k-login-v1--enabled {
            background-repeat: repeat !important;
        }

        /*头像*/
        .k-avatar__upload i {
            color: #5d78ff !important;
        }

        .k-avatar__upload:hover {
            background: #5d78ff !important;
        }

            .k-avatar__upload:hover i {
                color: #ffffff !important;
            }

        .bootstrap-maxlength {
            border: 1px solid;
            color: red;
            z-index: 2147483647 !important;
        }

        .modal-footer {
            justify-content: initial;
        }
    </style>
    <script src="/JS/SHA.js" type="text/javascript"></script>
    <script type="text/javascript">
        //注册表单检查
        function RegistersFormCheck() {
            var inps = [document.getElementById('inp_reg_user_name'), document.getElementById('inp_reg_password')];
            for (var i = 0; i < inps.length; i++) {
                var inp = inps[i];
                if (inp.value.length == 0) {
                    LoginAlert('注册错误', inp.title + '必须填写', inp);
                    return false;
                }
            }
            inp_reg_password.value = SHA256(inp_reg_password.value);
            return true;
        }

        //状态查询表单检查
        function StateQueryFormCheck() {
            var inps = [document.getElementById('inp_que_user_name'), document.getElementById('inp_que_password')];
            for (var i = 0; i < inps.length; i++) {
                var inp = inps[i];
                if (inp.value.length == 0) {
                    LoginAlert('查询错误', inp.title + '必须填写', inp);
                    return false;
                }
            }
            inp_que_password.value = SHA256(inp_que_password.value);
            return true;
        }

        //登录表单检查
        function LoginFormCheck() {
            var inps = [document.getElementById('inp_log_user_name'), document.getElementById('inp_log_password')];
            for (var i = 0; i < inps.length; i++) {
                var inp = inps[i];
                if (inp.value.length == 0) {
                    LoginAlert('登录错误', inp.placeholder + '必须填写', inp);
                    return false;
                }
            }
            inp_log_password.value = SHA256(inp_log_password.value);
            return true;
        }

        //登录错误提示
        function LoginAlert(title, text, inpOrName) {
            var inp = inpOrName;
            if (typeof (inpOrName) == 'string') {
                inp = document.getElementById(inpOrName);
                if (inp == null) {
                    console.log('登录控件[' + inpOrName + ']不存在！');
                    swal('系统错误，请联系管理员。');
                    return;
                }
            }
            swal({
                title: text,
                text: null,
                type: 'warning',
                showCancelButton: false,
                confirmButtonText: '确定',
                animation: false,
                customClass: 'animated tada'
            }).then(function (result) { if (result.value) { setTimeout(function (inp) { inp.focus(); }, 100, inp); } }, inp);
        }
    </script>
</head>
<body class="k-login-v1--enabled k-header--fixed k-header-mobile--fixed k-subheader--enabled k-subheader--transparent k-aside--enabled k-aside--fixed k-page--loading">
    <form id="LoginForm" runat="server" autocomplete="off">
        <!-- begin:: Page -->
        <div class="k-grid k-grid--ver k-grid--root">
            <div class="k-grid__item   k-grid__item--fluid k-grid  k-grid k-grid--hor k-login-v1" id="k_login_v1">
                <!--begin::Item-->
                <div class="k-grid__item  k-grid--hor">
                    <!--begin::Heade-->
                    <div class="k-login-v1__head">
                        <div class="k-login-v1__head-logo">
                            <img src="/media/logos/logo_hnlg_1.png" />
                        </div>
                        <div class="k-login-v1__head-signup">
                            <h4>没有用户?</h4>
                            <a class="k-link" style="cursor: pointer;" data-toggle="modal" data-target="#registers_modal"
                                id="btn_reg_application" runat="server">注册申请</a>
                            <a class="k-link" style="cursor: pointer;" data-toggle="modal" data-target="#stateQuery_modal"
                                id="btn_reg_query" runat="server">注册查询</a>
                        </div>
                    </div>
                    <!--begin::Head-->
                </div>
                <!--end::Item-->

                <!--begin::Item-->
                <div class="k-grid__item  k-grid  k-grid--ver  k-grid__item--fluid ">

                    <!--begin::Body-->
                    <div class="k-login-v1__body">

                        <!--begin::Section-->
                        <div class="k-login-v1__body-section">
                            <div class="k-login-v1__body-section-info">
                                <h3 style="text-align: center; font-size: 1.5em;">
                                    <asp:Label ID="txtSlogan" runat="server" /></h3>
                                <p></p>
                            </div>
                        </div>
                        <!--begin::Section-->

                        <!--begin::Separator-->
                        <div class="k-login-v1__body-seaprator"></div>
                        <!--end::Separator-->

                        <!--begin::Wrapper-->
                        <div class="k-login-v1__body-wrapper">
                            <div class="k-login-v1__body-container">
                                <h3 class="k-login-v1__body-title">后台管理系统</h3>

                                <!--begin::Form-->
                                <div class="k-login-v1__body-form k-form">
                                    <div class="form-group">
                                        <input class="form-control" type="text" placeholder="用户名" value="admin" name="username" id="inp_log_user_name" runat="server">
                                    </div>
                                    <div class="form-group">
                                        <input class="form-control" type="password" placeholder="密码" value="" name="password" id="inp_log_password" runat="server">
                                    </div>
                                    <div class="k-login-v1__body-action">
                                        <a href="#" class="k-link">
                                            <span>登录密码：0</span>
                                            <span style="display: none;">忘记密码？</span>
                                        </a>
                                        <button id="btnLogin" runat="server" type="submit" class="btn btn-pill btn-elevate"
                                            onclick="if (!LoginFormCheck()) { return false; }" onserverclick="Login_ServerClick">
                                            登录</button>
                                    </div>
                                </div>
                                <!--end::Form-->

                                <!--begin::Divider-->
                                <div class="k-login-v1__body-divider" style="display: none;">
                                    <div class="k-divider">
                                        <span></span>
                                        <span>OR</span>
                                        <span></span>
                                    </div>
                                </div>
                                <!--end::Divider-->

                                <!--begin::Options-->
                                <div class="k-login-v1__body-options" style="display: none;">
                                    <a href="#" class="btn">
                                        <i class="fab fa-facebook-f"></i>
                                        Fcebook
                                    </a>
                                    <a href="#" class="btn">
                                        <i class="fab fa-twitter"></i>
                                        Twitter
                                    </a>
                                    <a href="#" class="btn">
                                        <i class="fab fa-google"></i>
                                        Google
                                    </a>
                                </div>
                                <!--end::Options-->
                            </div>
                        </div>
                        <!--end::Wrapper-->
                    </div>
                    <!--begin::Body-->
                </div>
                <!--end::Item-->

                <!--begin::Item-->
                <div class="k-grid__item">
                    <div class="k-login-v1__footer">
                        <div class="k-login-v1__footer-link">
                            <a href="#" class="k-link">系统说明</a>
                            <a href="#" class="k-link">用户手册</a>
                            <a href="#" class="k-link">常见问题</a>
                        </div>
                        <div class="k-login-v1__footer-info">
                            <a href="#" class="k-link">&copy; 2019 华南理工大学公开学院</a>
                        </div>
                    </div>
                </div>
                <!--end::Item-->
            </div>
        </div>
        <!-- end:: Page -->

        <!-- begin::Registers Form -->
        <div class="modal fade" id="registers_modal" tabindex="-1" role="dialog" aria-labelledby="" aria-hidden="true">
            <div class="modal-dialog modal-lg" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">注册新用户</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true" class="la la-remove"></span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <div class="k-form k-form--label-right">
                            <!--用户名-->
                            <div class="form-group row k-margin-t-20">
                                <label class="col-form-label col-lg-3 col-sm-12">用户名</label>
                                <div class="col-lg-9 col-md-9 col-sm-12">
                                    <input type="text" class="form-control" maxlength="16" title="用户名" placeholder="用户名最多16个字符。" id="inp_reg_user_name" runat="server" />
                                </div>
                            </div>
                            <!--密码-->
                            <div class="form-group row k-margin-t-20">
                                <label class="col-form-label col-lg-3 col-sm-12">密码</label>
                                <div class="col-lg-9 col-md-9 col-sm-12">
                                    <input type="password" class="form-control" title="密码" placeholder="密码将以SHA256加密发送，建议不要设置弱密码。" id="inp_reg_password" runat="server" />
                                </div>
                            </div>
                            <!--姓名-->
                            <div class="form-group row k-margin-t-20">
                                <label class="col-form-label col-lg-3 col-sm-12">姓名</label>
                                <div class="col-lg-9 col-md-9 col-sm-12">
                                    <input type="text" class="form-control" maxlength="16" title="姓名" placeholder="姓名最多16个字符。" id="inp_reg_real_name" runat="server" />
                                </div>
                            </div>
                            <!--电话-->
                            <div class="form-group row k-margin-t-20">
                                <label class="col-form-label col-lg-3 col-sm-12">电话</label>
                                <div class="col-lg-9 col-md-9 col-sm-12">
                                    <div class="input-group">
                                        <div class="input-group-prepend"><span class="input-group-text"><i class="la la-phone"></i></span></div>
                                        <input type="text" class="form-control" aria-describedby="basic-addon1" title="电话" placeholder="手机号码或电话号码。" id="inp_reg_phone" runat="server">
                                    </div>
                                </div>
                            </div>
                            <!--邮件-->
                            <div class="form-group row k-margin-t-20">
                                <label class="col-form-label col-lg-3 col-sm-12">邮件</label>
                                <div class="col-lg-9 col-md-9 col-sm-12">
                                    <div class="input-group">
                                        <div class="input-group-prepend"><span class="input-group-text"><i class="la la-at"></i></span></div>
                                        <input type="text" class="form-control" aria-describedby="basic-addon1" title="邮件" placeholder="电子邮件地址。" id="inp_reg_email" runat="server">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <div style="width: 100%; padding-left: 20%;">
                            <button type="button" class="btn btn-brand k-btn" id="registers_submit" runat="server"
                                onclick="if (!RegistersFormCheck()) { return false; }" onserverclick="Registers_ServerClick">
                                提交</button>
                            <div style="width: 120px; margin-left: 60px; display: inline-block;">
                                <input type="reset" value="重置" class="btn btn-secondary k-btn" />
                                <button type="button" class="btn btn-secondary k-btn" data-dismiss="modal">放弃</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- end::Registers Form -->

        <!-- begin::State Query Form -->
        <div class="modal fade" id="stateQuery_modal" tabindex="-1" role="dialog" aria-labelledby="" aria-hidden="true">
            <div class="modal-dialog modal-lg" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">用户状态查询</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true" class="la la-remove"></span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <div class="k-form k-form--label-right">
                            <!--用户名-->
                            <div class="form-group row k-margin-t-20">
                                <label class="col-form-label col-lg-3 col-sm-12">用户名</label>
                                <div class="col-lg-9 col-md-9 col-sm-12">
                                    <input type="text" class="form-control" maxlength="16" title="用户名" id="inp_que_user_name" runat="server" />
                                </div>
                            </div>
                            <!--密码-->
                            <div class="form-group row k-margin-t-20">
                                <label class="col-form-label col-lg-3 col-sm-12">密码</label>
                                <div class="col-lg-9 col-md-9 col-sm-12">
                                    <input type="password" class="form-control" title="密码" id="inp_que_password" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <div style="width: 100%; padding-left: 20%;">
                                <button type="button" class="btn btn-brand k-btn" id="stateQuery_submit" runat="server"
                                    onclick="if (!StateQueryFormCheck()) { return false; }" onserverclick="StateQuery_ServerClick">
                                    查询</button>
                                <div style="width: 120px; margin-left: 60px; display: inline-block;">
                                    <button type="button" class="btn btn-secondary k-btn" data-dismiss="modal">关闭</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- end::State Query Form -->

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
        <script src="/UI/keenthemes.com/keen/preview/default/assets/vendors/base/vendors.bundle.js" type="text/javascript"></script>
        <script src="/UI/keenthemes.com/keen/preview/default/assets/demo/default/base/scripts.bundle.js" type="text/javascript"></script>
        <!--end::Global Theme Bundle -->

        <script>
            //document.body.style.background = "url('/media/misc/bg_" + (Math.floor(Math.random() * 9) + 1) + ".jpg')";
            $(document).ready(function () {
                //头像
                new KAvatar('reg_avatar_div');
                //文字
                var inpNames = ['#inp_reg_user_name', '#inp_reg_real_name', '#inp_reg_id_card_no'];
                for (var i = 0; i < inpNames.length; i++) {
                    $(inpNames[i]).maxlength({
                        alwaysShow: true,
                        threshold: 5,
                        warningClass: "k-badge k-badge--primary k-badge--rounded k-badge--inline",
                        limitReachedClass: "k-badge k-badge--brand k-badge--rounded k-badge--inline"
                    });
                }
                //日期
                $('#inp_reg_birthday').datepicker({
                    todayHighlight: true,
                    templates: {
                        leftArrow: '<i class="la la-angle-left"></i>',
                        rightArrow: '<i class="la la-angle-right"></i>'
                    }
                });
            });
        </script>

    </form>
</body>
</html>
