<%@ Page Title="" Language="C#" MasterPageFile="~/UI/Keen.Master" AutoEventWireup="true" CodeBehind="StoreInfo.aspx.cs" Inherits="DMS.MOD.RMS.StoreInfo" %>

<%@ Register Src="~/UC/PagerControl.ascx" TagPrefix="uc1" TagName="PagerControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageHeadPlaceHolder" runat="server">
    <link href="/CSS/List.css" rel="stylesheet" type="text/css" />
    <script src="/JS/List.js" type="text/javascript"></script>
    <script>
        //表单提交检查
        function StoreFormCheck() {
            var inps = ['inpInfoFormStoreName', 'inpInfoFormStoreTitle'];
            if (FormCheck(inps)) { return true; };
            return false;
        }
        //表单规格表单显示
        function InfoFormTabShow(TabName) {
            var selectModalClose = document.getElementById('btn' + TabName + 'SelectClose');
            if (selectModalClose != null) { selectModalClose.click(); }
            document.getElementById('btnListInfoFormModal').click();
            document.getElementById('btnListInfoForm' + TabName + 'Tab').click();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageContentPlaceHolder" runat="server">
    <!-- begin:: Content -->
    <!-- begin: List -->
    <div class="k-portlet">
        <div class="k-portlet__head">
            <div class="k-portlet__head-label">
                <h3 class="k-portlet__head-title">门店信息</h3>
            </div>
            <div class="k-portlet__head-toolbar">
                <div class="head-toolbar-button">
                    <div class="input-group">
                        <asp:TextBox ID="inpFilter" runat="server" CssClass="form-control" placeholder="门店名称" ClientIDMode="Static" AutoPostBack="true" OnTextChanged="Filter_Change" />
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
                    <asp:ListView ID="StoreInfoList" runat="server">
                        <LayoutTemplate>
                            <table class="table table-striped table-bordered table-hover table-checkable list-table">
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
                                        <th>门店LOGO</th>
                                        <th>
                                            <button id="ListOrderByName" runat="server" onserverclick="OrderBy_Click">
                                                <span class="list-field-title">门店代码</span>
                                                <span class="list-orderby-none" id="ListOrderByNameAsc" runat="server">↓</span>
                                                <span class="list-orderby-none" id="ListOrderByNameDesc" runat="server">↑</span>
                                                <asp:HiddenField ID="ListOrderByNameValue" runat="server" />
                                            </button>
                                        </th>
                                        <th>
                                            <button id="ListOrderByTitle" runat="server" onserverclick="OrderBy_Click">
                                                <span class="list-field-title">门店名称</span>
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
                                    <a class="btn btn-sm btn-clean btn-icon btn-icon-md" title="详情" storeid='<%#Eval("ID")%>'
                                        id="btnListDetails" autopostback="true" onserverclick="ListDetails_Click" runat="server">
                                        <i class="la la-book"></i></a>
                                    <a class="btn btn-sm btn-clean btn-icon btn-icon-md" title="修改" storeid='<%#Eval("ID")%>'
                                        id="btnListEdit" autopostback="true" onserverclick="ListEdit_Click" runat="server">
                                        <i class="la la-edit"></i></a>
                                </td>
                                <td class="list-logo">
                                    <img src='<%#Eval("FileGUID") == null ? "/media/logos/logo_template.png" : "/DOC/upload/" + Eval("FileGUID") + Eval("FileExtName")%>' />
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
                                            <a class="dropdown-item" storeid='<%#Eval("ID")%>' id="btnListEnabled" runat="server"
                                                onserverclick="ListEnabled_Click"><i class="la la-angellist"></i>启用</a>
                                            <a class="dropdown-item" storeid='<%#Eval("ID")%>' id="btnListDisabled" runat="server"
                                                onserverclick="ListEnabled_Click"><i class="la la-lock"></i>禁用</a>
                                        </div>
                                    </span>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:ListView>
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
                    <h5 class="modal-title">门店[<asp:Label ID="txtInfoFormTitle" runat="server" />]的信息表单</h5>
                    <div class="k-portlet__head-toolbar">
                        <a href="#" class="btn btn-secondary k-margin-r-10" data-dismiss="modal" aria-label="Close">
                            <span class="k-hidden-mobile">返回</span>
                        </a>
                        <div class="btn-group">
                            <button type="button" class="btn btn-primary" id="btnInfoFormSubmit" runat="server"
                                onclick="if (!StoreFormCheck()) { return false; }" onserverclick="InfoFormSubmit_Click">
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
                    <asp:HiddenField ID="hidInfoFormStoreID" Value="0" runat="server" />
                    <ul class="nav nav-tabs nav-tabs-line nav-tabs-bold nav-tabs-line-3x nav-tabs-line-brand" role="tablist">
                        <li class="nav-item">
                            <a class="nav-link active" data-toggle="tab" href="#k_tabs_info" role="tab">店名</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" data-toggle="tab" href="#k_tabs_contact" role="tab" id="btnListInfoFormContactTab">联系信息</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" data-toggle="tab" href="#k_tabs_photos" role="tab">图片</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" data-toggle="tab" href="#k_tabs_desc" role="tab">描述</a>
                        </li>
                    </ul>
                    <div class="tab-content">
                        <div class="tab-pane active" id="k_tabs_info" role="tabpanel">
                            <div class="k-form k-form--label-right">
                                <!--代码-->
                                <div class="form-group row k-margin-t-20">
                                    <label class="col-form-label col-lg-3 col-sm-12">门店代码</label>
                                    <div class="col-lg-9 col-md-9 col-sm-12">
                                        <asp:TextBox ID="inpInfoFormStoreName" runat="server" ClientIDMode="Static" class="form-control inp-txt-len"
                                            MaxLength="32" title="门店代码" placeholder="门店代码最多32个字符。" />
                                    </div>
                                </div>
                                <!--名称-->
                                <div class="form-group row k-margin-t-20">
                                    <label class="col-form-label col-lg-3 col-sm-12">门店名称</label>
                                    <div class="col-lg-9 col-md-9 col-sm-12">
                                        <asp:TextBox ID="inpInfoFormStoreTitle" runat="server" ClientIDMode="Static" class="form-control inp-txt-len"
                                            MaxLength="32" title="门店名称" placeholder="门店名称最多32个字符。" />
                                    </div>
                                </div>
                                <!--LOGO-->
                                <asp:HiddenField ID="hidInfoFormLogoFileID" Value="0" runat="server" />
                                <div class="form-group row k-margin-t-20">
                                    <label class="col-form-label col-lg-3 col-sm-12">门店LOGO</label>
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
                            </div>
                        </div>
                        <!--位置-->
                        <div class="tab-pane" id="k_tabs_contact" role="tabpanel">
                            <div class="k-form k-form--label-right">
                                <!--负责人-->
                                <div class="form-group row k-margin-t-20">
                                    <label class="col-form-label col-lg-3 col-sm-12">负责人</label>
                                    <div class="col-lg-9 col-md-9 col-sm-12">
                                        <asp:TextBox ID="inpInfoFormPrincipal" runat="server" ClientIDMode="Static" class="form-control inp-txt-len"
                                            MaxLength="32" title="负责人" Enabled="false" placeholder="待完成可添加多个店员" />
                                    </div>
                                </div>
                                <!--邮件-->
                                <div class="form-group row k-margin-t-20">
                                    <label class="col-form-label col-lg-3 col-sm-12">邮件</label>
                                    <div class="col-lg-9 col-md-9 col-sm-12">
                                        <div class="input-group">
                                            <div class="input-group-prepend"><span class="input-group-text"><i class="la la-at"></i></span></div>
                                            <input type="text" class="form-control inp-txt-len" aria-describedby="basic-addon1" title="邮件"
                                                maxlength="64" placeholder="电子邮件地址。" id="inpInfoFormEmail" clientidmode="static" runat="server">
                                        </div>
                                    </div>
                                </div>
                                <!--电话-->
                                <div class="form-group row k-margin-t-20">
                                    <label class="col-form-label col-lg-3 col-sm-12">电话</label>
                                    <div class="col-lg-9 col-md-9 col-sm-12">
                                        <div class="input-group">
                                            <div class="input-group-prepend"><span class="input-group-text"><i class="la la-phone"></i></span></div>
                                            <input type="text" class="form-control inp-txt-len" aria-describedby="basic-addon1" title="电话"
                                                maxlength="16" placeholder="手机号码或电话号码。" id="inpInfoFormPhone" clientidmode="static" runat="server">
                                        </div>
                                    </div>
                                </div>
                                <div>待添加省、市、区县、商圈、地铁</div>
                                <!--地址-->
                                <div class="form-group row k-margin-t-20">
                                    <label class="col-form-label col-lg-3 col-sm-12">地址</label>
                                    <div class="col-lg-9 col-md-9 col-sm-12">
                                        <asp:TextBox ID="inpInfoFormAddress" runat="server" ClientIDMode="Static" class="form-control inp-txt-len"
                                            AutoPostBack="true" OnTextChanged="InfoFormAddress_Change" Text=""
                                            MaxLength="512" title="门店地址" placeholder="请输入详细地址回车定位，地址最多512个字符。" />
                                    </div>
                                </div>
                                <!--纬度-->
                                <div class="form-group row k-margin-t-20">
                                    <label class="col-form-label col-lg-3 col-sm-12">纬度</label>
                                    <div class="col-lg-9 col-md-9 col-sm-12">
                                        <asp:TextBox class="form-control txtGEOLat"
                                            ID="inpInfoFormLatitude" ClientIDMode="Static" runat="server" placeholder="纬度" />
                                    </div>
                                </div>
                                <!--经度-->
                                <div class="form-group row k-margin-t-20">
                                    <label class="col-form-label col-lg-3 col-sm-12">经度</label>
                                    <div class="col-lg-9 col-md-9 col-sm-12">
                                        <asp:TextBox class="form-control txtGEOLNG"
                                            ID="inpInfoFormLongitude" ClientIDMode="Static" runat="server" placeholder="经度" />
                                    </div>
                                </div>
                                <!--地图-->
                                <div id="container" style="width: 100%; height: 500px;">
                                </div>
                            </div>
                        </div>
                        <!--图片-->
                        <div class="tab-pane" id="k_tabs_photos" role="tabpanel">
                            <div class="dropzone dropzone-primary" id="k-dropzone-two"></div>
                            <asp:HiddenField ID="hidInfoFormStorePhoto" ClientIDMode="Static" runat="server" />
                        </div>
                        <!--描述-->
                        <div class="tab-pane" id="k_tabs_desc" role="tabpanel">
                            <div class="k-form k-form--label-right">
                                <!--营业时间-->
                                <div class="form-group row k-margin-t-20">
                                    <label class="col-form-label col-lg-3 col-sm-12">营业时间</label>
                                    <div class="col-lg-9 col-md-9 col-sm-12">
                                        <asp:TextBox ID="inpInfoFormBusinessHours" runat="server" ClientIDMode="Static" class="form-control inp-txt-len"
                                            MaxLength="32" title="营业时间" placeholder="09:00 - 22:00 节假日除外" />
                                    </div>
                                </div>
                            </div>
                            <div class="summernote" id="txtInfoFormStoreDesc">腾讯地图工具，如果不能显示，请手工填写经、纬度。</div>
                            <asp:HiddenField ID="hidInfoFormStoreDesc" ClientIDMode="Static" runat="server" />
                        </div>
                    </div>
                    <div class="modal-footer">
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
                    <h5 class="modal-title">门店[<asp:Label ID="txtDefailsModalTitle" runat="server" />]的详细信息</h5>
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
                </div>
            </div>
        </div>
    </div>
    <!--end: InfoDetails -->
    <!-- end:: Content -->
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PageFootPlaceHolder" runat="server">
    <script src="https://map.qq.com/api/js?v=2.exp&key=<%=new Ziri.MDL.QQ.QQWSKey().key%>"></script>
    <script src="https://3gimg.qq.com/lightmap/components/geolocation/geolocation.min.js"></script>
    <script>
        $(document).ready(function () {
            //表单
            var FormsName = ['#InfoFormModal'];
            for (var i = 0; i < FormsName.length; i++) { ModalHeadScroll(FormsName[i]); };
            //文字
            $('.inp-txt-len').maxlength({
                alwaysShow: true,
                threshold: 5,
                warningClass: "k-badge k-badge--primary k-badge--rounded k-badge--inline",
                limitReachedClass: "k-badge k-badge--brand k-badge--rounded k-badge--inline"
            });
            //纬度
            $('.txtGEOLat').TouchSpin({
                buttondown_class: 'btn btn-secondary',
                buttonup_class: 'btn btn-secondary',

                min: 0.0000000,
                max: 90.0000000,
                step: 0.0000001,
                decimals: 15,
                boostat: 5,
                maxboostedstep: 10,
            });
            //经度
            $('.txtGEOLNG').TouchSpin({
                buttondown_class: 'btn btn-secondary',
                buttonup_class: 'btn btn-secondary',

                min: 0.0000000,
                max: 180.0000000,
                step: 0.0000001,
                decimals: 15,
                boostat: 5,
                maxboostedstep: 10,
            });
            //图片
            var avatar3 = new KAvatar('k_profile_avatar_3');
            //富文本
            var htmDescID = '#txtInfoFormStoreDesc';
            var inpDesc = document.getElementById('hidInfoFormStoreDesc');
            $(htmDescID).summernote({
                //height: 500,
                //width:1000,
                minHeight: 350,
                //maxwidth: 1000,
                //minwidth: 200,
                //maxHeight: 1000,
                //focus: false,
                callbacks: {
                    onChange: function (contents) {
                        inpDesc.value = encodeURIComponent(contents);
                    },
                    onImageUpload: function (files, editor) {
                        var $files = $(files);
                        $files.each(function () {
                            var file = this;
                            var data = new FormData();
                            data.append("file", file);
                            $.ajax({
                                data: data,
                                type: "POST",
                                url: "/MOD/SYS/Upload.ashx?mod=store&act=sumnmber",
                                cache: false,
                                contentType: false,
                                processData: false,
                                success: function (response) {
                                    var json = $.parseJSON(response)
                                    if (json.state == "error") { alert("上传失败"); }
                                    else { $(htmDescID).summernote('insertImage', json.img_url); }
                                },
                            });
                        })
                    }
                }
            });
            $(htmDescID).summernote('code', decodeURIComponent(inpDesc.value));
            //腾讯地图
            //Javascript地图API https://lbs.qq.com/javascript_v2/index.html
            //JavaScript API https://lbs.qq.com/javascript_v2/doc/index.html
            //WebService API https://lbs.qq.com/webservice_v1/index.html
            //地图组件（H5） https://lbs.qq.com/tool/component-marker.html
            //微信小程序 https://lbs.qq.com/qqmap_wx_jssdk/index.html
            var latLng = new qq.maps.LatLng(23.10647, 113.324463);
            var inpLat = document.getElementById('inpInfoFormLatitude');
            var inpLng = document.getElementById('inpInfoFormLongitude');
            if (inpLat.value.length > 0 && inpLng.value.length > 0) {
                latLng = new qq.maps.LatLng(parseFloat(inpLat.value), parseFloat(inpLng.value));
            };
            var map = new qq.maps.Map(document.getElementById("container"), {
                center: latLng,
                zoom: 13,
            });
            var marker = new qq.maps.Marker({
                map: map,
                position: latLng,
                draggable: true,
            });
            marker.setTitle("可拖动修改门店位置");
            //点击事件
            qq.maps.event.addListener(map, 'click', function (event) {
                marker.setPosition(event.latLng);
                setInfoFormPosition(event);
            });
            //拖动事件
            qq.maps.event.addListener(marker, 'dragend', function (event) {
                setInfoFormPosition(event);
            });
            //设置地图经纬度到表单
            function setInfoFormPosition(event) {
                document.getElementById('inpInfoFormLatitude').value = event.latLng.lat;
                document.getElementById('inpInfoFormLongitude').value = event.latLng.lng;
            }


            //腾讯静默定位
            //DOChttps://lbs.qq.com/tool/component-geolocation.html
            <%--var tencent_geolocation = new qq.maps.Geolocation('<%=new Ziri.MDL.QQ.QQWSKey().key%>', 'myapp');
            tencent_geolocation.getLocation(Tencent_showPosition, Tencent_showErr, { timeout: 8000 })
            function Tencent_showPosition(position) { console.log(position); };
            function Tencent_showErr() { console.log("腾讯定位失败！"); };--%>

            //腾讯根据IP定位
            //function geolocation_ip() {
            //    var clientip = '202.104.127.192';
            //    citylocation.searchCityByIP(clientip);
            //}
            //腾讯根据IP定位并设置标点
            //citylocation = new qq.maps.CityService({
            //    map: map,
            //    complete: function (results) {
            //        map.setCenter(results.detail.latLng);
            //        var marker = new qq.maps.Marker({
            //            map: map,
            //            position: results.detail.latLng
            //        });
            //    }
            //});
        });
        //图片
        var KDropzoneDemo = function () {
            var demos = function () {
                Dropzone.options.kDropzoneTwo = {
                    url: "/MOD/SYS/Upload.ashx?mod=store&act=dropzone",
                    acceptedFiles: "image/*",
                    uploadMultiple: true,
                    maxFiles: 6,
                    maxFilesize: 5,             //MB
                    //autoProcessQueue: false,    //关闭自动上传功能
                    //parallelUploads: 100,       //每次上传的最多文件数，经测试默认为2，//记得修改web.config 限制上传文件大小的节
                    addRemoveLinks: true,
                    dictDefaultMessage: "拖拽图片或者点击选择",
                    dictMaxFilesExceeded: "最多只能上传6个文件！",
                    dictInvalidFileType: "只能上传图片",
                    dictFileTooBig: "图片最大5M",
                    dictRemoveFile: "移除",
                    init: function () {

                        StorePhotoFill(this);

                        this.on("addedfile", function () { });
                        this.on("success", function (file, imageInfo) { });
                        this.on("complete", function (data) {
                            if (data.xhr != null && data.xhr.readyState == 4 && data.xhr.status == 200) {
                                var res = eval('(' + data.xhr.responseText + ')');
                                if (res.Result) {
                                    StorePhotoAdd(res.Files);
                                }
                            }
                        });
                        this.on("removedfile", function (file) {
                            StorePhotoRemove(file);
                        });
                    }
                };
            }
            return {
                init: function () {
                    demos();
                }
            };
        }();
        KDropzoneDemo.init();
        // - 填充
        function StorePhotoFill(Dropzone) {
            var hidFiles = document.getElementById('hidInfoFormStorePhoto');
            var Exists = hidFiles.value.length == 0 ? [] : JSON.parse(hidFiles.value);
            for (var i = 0; i < Exists.length; i++) {
                var uploadInfo = Exists[i];
                var mockFile = {
                    name: uploadInfo.FileInfo.Name,
                    size: uploadInfo.FileInfo.Length,
                    type: uploadInfo.FileExtName.Name,  // uploadInfo.FileMIME.Name,
                };
                Dropzone.addFile.call(Dropzone, mockFile);
                Dropzone.options.thumbnail.call(Dropzone, mockFile, '/DOC/upload/thumb/' + uploadInfo.FileInfo.GUID + ".png");
            }
        }
        // - 新增
        function StorePhotoAdd(Files) {
            var hidFiles = document.getElementById('hidInfoFormStorePhoto');
            var Exists = hidFiles.value.length == 0 ? [] : JSON.parse(hidFiles.value);
            for (var i = 0; i < Files.length; i++) {
                var findIndex = -1;
                for (var j = 0; j < Exists.length; j++) {
                    if (Exists[j].FileInfo.ID == Files[i].FileInfo.ID) {
                        findIndex = j;
                        break;
                    }
                }
                if (findIndex == -1) { Exists.push(Files[i]); }
            }
            hidFiles.value = JSON.stringify(Exists);
        }
        // - 移除
        function StorePhotoRemove(File) {
            var hidFiles = document.getElementById('hidInfoFormStorePhoto');
            var Exists = hidFiles.value.length == 0 ? [] : JSON.parse(hidFiles.value);
            for (var i = 0; i < Exists.length; i++) {
                var ExistFileInfo = Exists[i].FileInfo;
                if (ExistFileInfo.Name == File.name && ExistFileInfo.Length == File.size) {
                    Exists.splice(i, 1);
                    break;
                }
            }
            hidFiles.value = JSON.stringify(Exists);
        }
    </script>
</asp:Content>
