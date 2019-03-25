<%@ Page Title="" Language="C#" MasterPageFile="~/UI/Keen.Master" AutoEventWireup="true" CodeBehind="SalesOrder.aspx.cs" Inherits="DMS.MOD.OMS.SalesOrder" %>

<%@ Register Src="~/UC/PagerControl.ascx" TagPrefix="uc1" TagName="PagerControl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageHeadPlaceHolder" runat="server">
    <link href="/CSS/List.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .detail-fieldset {
            border: rgba(0, 0, 0, 0.2) 1px solid;
            border-radius: 10px;
            margin: 20px 0px;
            padding: 10px;
            width: 100%;
        }

        .detail-legend {
            width: initial;
            padding: 0px 5px;
            font-weight: bold;
        }

        .legend-title {
        }

        .legend-text {
            font-weight: bold;
        }

        .Amount {
            color: rgba(0,0,255,1);
            font-family: Arial Narrow;
            font-size: 2em;
        }

        .txtPayCode {
            border: 1px solid #dddee1;
            border-radius: 3px;
            padding: 5px 10px;
            margin: 10px 0px 20px 0px;
            text-align: center;
            width: 100%;
            font-family: Arial Narrow;
            font-size: 2em !important;
            background: rgba(0,200,0,0.2);
        }
    </style>

    <script type="text/javascript">
        function SOFormCheck(sender) {
            return false;
        }
        function ListRowDBLClick(sender) {
            var links = sender.getElementsByTagName("a");
            for (var i = 0; i < links.length; i++) {
                var a = links[i];
                if (a.title != null && a.title == "详情") { a.click(); return; }  //修改
            }
        }
        function PayFormAmountChange(sender) {

            var PayFormCash = parseFloat(inpPayFormCash.value);
            var PayFormCard = parseFloat(inpPayFormCard.value);
            var PayAmount = ((isNaN(PayFormCash) ? 0 : PayFormCash) + (isNaN(PayFormCard) ? 0 : PayFormCard)).toFixed(2);

            var txtReceipt = document.getElementById('txtReceiptsMoney');
            var txtPayAmount = document.getElementById('txtPayAmount');
            var txtPayChange = document.getElementById('txtPayChange');
            var Unit = txtReceipt.innerText.substring(0, 1);
            var ReceiptAmount = parseFloat(txtReceipt.innerText.replace(Unit, '').replace(',', ''));
            txtPayChange.innerText = Unit + toThousands(PayAmount - ReceiptAmount);
            txtPayChange.style.color = PayAmount == ReceiptAmount ? 'blue' : 'red';
            txtPayAmount.innerText = Unit + toThousands(PayAmount);
        }
        function toThousands(num) {
            var num = (num || 0.00).toString().split('.');
            var result = '';
            while (num[0].length > 3) {
                result = ',' + num[0].slice(-3) + result;
                num[0] = num[0].slice(0, num[0].length - 3);
            }
            if (num[0]) { result = num[0] + result; }
            return result + '.' + (num[1] == null ? '00' : num[1]);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PageContentPlaceHolder" runat="server">
    <!-- begin:: Content -->
    <!-- begin: List -->
    <div class="k-portlet">
        <div class="k-portlet__head">
            <div class="k-portlet__head-label">
                <h3 class="k-portlet__head-title">销售订单</h3>
            </div>
            <div class="k-portlet__head-toolbar">
                <div class="head-toolbar-button">
                    <div class="input-group">
                        <asp:TextBox ID="inpFilter" runat="server" CssClass="form-control" placeholder="订单号"
                            ClientIDMode="Static" AutoPostBack="true" OnTextChanged="Filter_Change" />
                        <div class="input-group-append">
                            <button class="btn btn-primary head-toolbar-button-search" type="button" id="btnFilter" runat="server"
                                onclick="if (!FilterCheck()) { return false; }" onserverclick="Filter_Click">
                                <i class="fa fa-search"></i>
                            </button>
                        </div>
                    </div>
                </div>
                <div class="head-toolbar-button">
                    <asp:DropDownList ID="drpFilterState" runat="server" CssClass="form-control"
                        OnSelectedIndexChanged="FilterState_Click" AutoPostBack="true" />
                </div>
                <div class="head-toolbar-button">
                    <asp:DropDownList ID="drpFilterReceiptType" runat="server" CssClass="form-control"
                        OnSelectedIndexChanged="drpFilterReceiptType_Click" AutoPostBack="true" />
                </div>
                <div class="head-toolbar-button">
                    <button type="button" class="btn btn-clean btn-sm btn-icon btn-icon-md" title="新增"
                        id="btnAddNew" onserverclick="AddNew_Click" runat="server">
                        <i class="flaticon2-add-1"></i>
                    </button>
                    <a data-toggle="modal" data-target="#InfoFormModal" id="btnListInfoFormModal" style="display: none;"></a>
                    <a data-toggle="modal" data-target="#DetailsModal" id="btnListDetailsModal" style="display: none;"></a>
                    <a data-toggle="modal" data-target="#PayModal" id="btnListPayModal" style="display: none;"></a>
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
                                        <th>
                                            <button id="ListOrderByBillNO" runat="server" onserverclick="OrderBy_Click">
                                                <span class="list-field-title">订单单号</span>
                                                <span class="list-orderby-none" id="ListOrderByBillNOAsc" runat="server">↓</span>
                                                <span class="list-orderby-none" id="ListOrderByBillNODesc" runat="server">↑</span>
                                                <asp:HiddenField ID="ListOrderByBillNOValue" runat="server" />
                                            </button>
                                        </th>
                                        <th>
                                            <button id="ListOrderByGoodsName" runat="server" onserverclick="OrderBy_Click">
                                                <span class="list-field-title">商品代码</span>
                                                <span class="list-orderby-none" id="ListOrderByGoodsNameAsc" runat="server">↓</span>
                                                <span class="list-orderby-none" id="ListOrderByGoodsNameDesc" runat="server">↑</span>
                                                <asp:HiddenField ID="ListOrderByGoodsNameValue" runat="server" />
                                            </button>
                                        </th>
                                        <th>
                                            <button id="ListOrderByGoodsTitle" runat="server" onserverclick="OrderBy_Click">
                                                <span class="list-field-title">商品名称</span>
                                                <span class="list-orderby-none" id="ListOrderByGoodsTitleAsc" runat="server">↓</span>
                                                <span class="list-orderby-none" id="ListOrderByGoodsTitleDesc" runat="server">↑</span>
                                                <asp:HiddenField ID="ListOrderByGoodsTitleValue" runat="server" />
                                            </button>
                                        </th>
                                        <th>商品规格</th>
                                        <th>数量</th>
                                        <th>单价</th>
                                        <th>金额</th>
                                        <th>
                                            <button id="ListOrderByStateID" runat="server" onserverclick="OrderBy_Click">
                                                <span class="list-field-title">订单状态</span>
                                                <span class="list-orderby-none" id="ListOrderByStateIDAsc" runat="server">↓</span>
                                                <span class="list-orderby-none" id="ListOrderByStateIDDesc" runat="server">↑</span>
                                                <asp:HiddenField ID="ListOrderByStateIDValue" runat="server" />
                                            </button>
                                        </th>
                                        <th>
                                            <button id="ListOrderByReceiveTypeID" runat="server" onserverclick="OrderBy_Click">
                                                <span class="list-field-title">收货方式</span>
                                                <span class="list-orderby-none" id="ListOrderByReceiveTypeIDAsc" runat="server">↓</span>
                                                <span class="list-orderby-none" id="ListOrderByReceiveTypeIDDesc" runat="server">↑</span>
                                                <asp:HiddenField ID="ListOrderByReceiveTypeIDValue" runat="server" />
                                            </button>
                                        </th>
                                        <th>
                                            <button id="ListOrderByCreateTime" runat="server" onserverclick="OrderBy_Click">
                                                <span class="list-field-title">创建时间</span>
                                                <span class="list-orderby-none" id="ListOrderByCreateTimeAsc" runat="server">↓</span>
                                                <span class="list-orderby-none" id="ListOrderByCreateTimeDesc" runat="server">↑</span>
                                                <asp:HiddenField ID="ListOrderByCreateTimeValue" runat="server" />
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
                        <EmptyDataTemplate>
                            <div style="width: 100%; text-align: center;">没有记录</div>
                        </EmptyDataTemplate>
                        <ItemTemplate>
                            <tr ondblclick="ListRowDBLClick(this)">
                                <th><%#((Container.DataItemIndex + 1) + (InfoListPager.PageIndex * InfoListPager.PageSize - InfoListPager.PageSize))%></th>
                                <td>
                                    <span><%#Eval("ID")%></span>
                                    <a class="btn btn-sm btn-clean btn-icon btn-icon-md" title="详情" soid='<%#Eval("ID")%>'
                                        id="btnListDetails" autopostback="true" onserverclick="ListDetails_Click" runat="server">
                                        <i class="la la-book"></i></a>
                                    <a class="btn btn-sm btn-clean btn-icon btn-icon-md" title="修改" soid='<%#Eval("ID")%>'
                                        id="btnListEdit" autopostback="true" onserverclick="ListEdit_Click" runat="server">
                                        <i class="la la-edit"></i></a>
                                </td>
                                <td><%#FilterFormat(inpFilter, (string)Eval("BillNO"))%></td>
                                <td><%#FilterFormat(inpFilter, (string)Eval("GoodsName"))%></td>
                                <td><%#FilterFormat(inpFilter, (string)Eval("GoodsTitle"))%></td>
                                <td><%#Eval("GoodsSpecValues")%></td>
                                <td><%#Eval("Quantity")%></td>
                                <td><%#Eval("Price")%></td>
                                <td><%#Eval("Amount")%></td>
                                <td>
                                    <span class="dropdown">
                                        <a href="#" class="btn btn-sm btn-clean btn-icon btn-icon-md" data-toggle="dropdown" aria-expanded="true">
                                            <span class="badge badge-success list-enabled"><%#Eval("StateTitle")%></span>
                                        </a>
                                        <div class="dropdown-menu dropdown-menu-right">
                                            <a class="dropdown-item" soid='<%#Eval("ID")%>' id="btnListConfirm" runat="server"
                                                onserverclick="ListConfirm_Click"><i class="la la-gavel"></i>确认</a>
                                            <a class="dropdown-item" soid='<%#Eval("ID")%>' id="btnListUnconfirm" runat="server"
                                                onserverclick="ListUnconfirm_Click"><i class="la la-reply"></i>撤销确认</a>
                                            <a class="dropdown-item" soid='<%#Eval("ID")%>' id="btnListReceipt" runat="server"
                                                onserverclick="ListPaid_Click"><i class="la la-money"></i>收银</a>
                                            <a class="dropdown-item" soid='<%#Eval("ID")%>' id="btnListSend" runat="server"
                                                onserverclick="ListSend_Click"><i class="la la-gift"></i>发货</a>
                                            <a class="dropdown-item" soid='<%#Eval("ID")%>' id="btnListUnsend" runat="server"
                                                onserverclick="ListUnsend_Click"><i class="la la-reply"></i>撤销发货</a>
                                            <a class="dropdown-item" soid='<%#Eval("ID")%>' id="btnAgreedCancel" runat="server"
                                                onserverclick="ListAgreedCancel_Click"><i class="la la-gavel"></i>同意取消订单</a>
                                            <a class="dropdown-item" soid='<%#Eval("ID")%>' id="btnListCancel" runat="server"
                                                onserverclick="ListCancel_Click"><i class="la la-trash"></i>取消</a>
                                        </div>
                                    </span>
                                </td>
                                <td><%#Eval("ReceiveTypeTitle")%></td>
                                <td><%#((DateTime)Eval("CreateTime")).ToString("yyyy/MM/dd HH:mm:ss")%></td>
                                <td><%#Eval("UpdateTime") == null ? null : ((DateTime)Eval("UpdateTime")).ToString("yyyy/MM/dd HH:mm:ss")%></td>
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
                    <h5 class="modal-title">订单[<asp:Label ID="txtInfoFormTitle" runat="server" />]的信息表单</h5>
                    <div class="k-portlet__head-toolbar">
                        <a href="#" class="btn btn-secondary k-margin-r-10" data-dismiss="modal" aria-label="Close">
                            <span class="k-hidden-mobile">返回</span>
                        </a>
                        <div class="btn-group">
                            <button type="button" class="btn btn-primary" id="btnInfoFormSubmit" runat="server"
                                onclick="if (!SOFormCheck()) { return false; }" onserverclick="InfoFormSubmit_Click">
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
                    <asp:HiddenField ID="hidInfoFormSOID" Value="0" runat="server" />
                    <ul class="nav nav-tabs nav-tabs-line nav-tabs-bold nav-tabs-line-3x nav-tabs-line-brand" role="tablist">
                        <li class="nav-item">
                            <a class="nav-link active" data-toggle="tab" href="#k_tabs_so" role="tab">订单</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" data-toggle="tab" href="#k_tabs_customer" role="tab">客户</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" data-toggle="tab" href="#k_tabs_goods" role="tab">商品</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" data-toggle="tab" href="#k_tabs_receipt" role="tab">收货</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" data-toggle="tab" href="#k_tabs_pay" role="tab">支付</a>
                        </li>
                    </ul>
                    <div class="tab-content">
                        <div class="tab-pane active" id="k_tabs_so" role="tabpanel">
                            <div class="k-form k-form--label-right">
                                <!--单号-->
                                <div class="form-group row k-margin-t-20">
                                    <label class="col-form-label col-lg-3 col-sm-12">订单号</label>
                                    <div class="col-lg-9 col-md-9 col-sm-12">
                                        <asp:TextBox ID="inpInfoFormBillNO" runat="server" class="form-control" ReadOnly="true" />
                                    </div>
                                </div>
                                <!--状态-->
                                <div class="form-group row k-margin-t-20">
                                    <label class="col-form-label col-lg-3 col-sm-12">状态</label>
                                    <div class="col-lg-9 col-md-9 col-sm-12">
                                        <asp:TextBox ID="inpInfoFormState" runat="server" class="form-control" ReadOnly="true" />
                                    </div>
                                </div>
                                <!--备注-->
                                <div class="form-group row k-margin-t-20">
                                    <label class="col-form-label col-lg-3 col-sm-12">备注</label>
                                    <div class="col-lg-9 col-md-9 col-sm-12">
                                        <asp:TextBox ID="inpInfoFormRemark" runat="server" class="form-control" ReadOnly="true" />
                                    </div>
                                </div>
                                <!--创建时间-->
                                <div class="form-group row k-margin-t-20">
                                    <label class="col-form-label col-lg-3 col-sm-12">创建时间</label>
                                    <div class="col-lg-9 col-md-9 col-sm-12">
                                        <asp:TextBox ID="inpInfoFormCreateTime" runat="server" class="form-control" ReadOnly="true" />
                                    </div>
                                </div>
                                <!--更新时间-->
                                <div class="form-group row k-margin-t-20">
                                    <label class="col-form-label col-lg-3 col-sm-12">更新时间</label>
                                    <div class="col-lg-9 col-md-9 col-sm-12">
                                        <asp:TextBox ID="inpInfoFormUpdateTime" runat="server" class="form-control" ReadOnly="true" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!--客户-->
                        <div class="tab-pane" id="k_tabs_customer" role="tabpanel">
                            <div class="k-form k-form--label-right">
                                <!--客户类型-->
                                <div class="form-group row k-margin-t-20">
                                    <label class="col-form-label col-lg-3 col-sm-12">客户类型</label>
                                    <div class="col-lg-9 col-md-9 col-sm-12">
                                        <asp:TextBox ID="inpInfoFormCustomerType" runat="server" class="form-control" ReadOnly="true" />
                                    </div>
                                </div>
                                <!--客户昵称-->
                                <div class="form-group row k-margin-t-20">
                                    <label class="col-form-label col-lg-3 col-sm-12">客户昵称</label>
                                    <div class="col-lg-9 col-md-9 col-sm-12">
                                        <asp:TextBox ID="inpInfoFormCustomerNickName" runat="server" class="form-control" ReadOnly="true" />
                                    </div>
                                </div>
                                <!--客户性别-->
                                <div class="form-group row k-margin-t-20">
                                    <label class="col-form-label col-lg-3 col-sm-12">客户性别</label>
                                    <div class="col-lg-9 col-md-9 col-sm-12">
                                        <asp:TextBox ID="inpInfoFormCustomerGender" runat="server" class="form-control" ReadOnly="true" />
                                    </div>
                                </div>
                                <!--客户头像-->
                                <div class="form-group row k-margin-t-20">
                                    <label class="col-form-label col-lg-3 col-sm-12">客户头像</label>
                                    <div class="col-lg-9 col-md-9 col-sm-12">
                                        <asp:Image ID="imgInfoFormCustomerAvatar" runat="server" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!--商品-->
                        <div class="tab-pane" id="k_tabs_goods" role="tabpanel">
                            <asp:ListView ID="lvInfoFormGoodsList" runat="server">
                                <LayoutTemplate>
                                    <table class="table table-striped table-bordered table-hover table-checkable list-table">
                                        <thead>
                                            <tr>
                                                <th>行号</th>
                                                <th>商品图片</th>
                                                <th>商品名称</th>
                                                <th>商品规格</th>
                                                <th>数量</th>
                                                <th>单价</th>
                                                <th>金额</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr id="itemPlaceholder" runat="server" />
                                        </tbody>
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <th><%#Eval("OrderID")%></th>
                                        <td>
                                            <img src='/DOC/upload/<%#((List<string>)Eval("GoodsPhotos"))[0]%>' style="max-height: 50px;" /></td>
                                        <td><%#Eval("GoodsTitle")%></td>
                                        <td><%#Eval("GoodsSpecTitle")%></td>
                                        <td><%#Eval("Quantity")%></td>
                                        <td><%#Eval("Price")%></td>
                                        <td><%#Eval("Amount")%></td>
                                    </tr>
                                </ItemTemplate>
                            </asp:ListView>
                        </div>
                        <!--收货-->
                        <div class="tab-pane" id="k_tabs_receipt" role="tabpanel">
                            <div class="k-form k-form--label-right">
                                <!--收货类型-->
                                <div class="form-group row k-margin-t-20">
                                    <label class="col-form-label col-lg-3 col-sm-12">收货类型</label>
                                    <div class="col-lg-9 col-md-9 col-sm-12">
                                        <asp:TextBox ID="inpInfoFormReceiptType" runat="server" class="form-control" ReadOnly="true" />
                                    </div>
                                </div>
                                <!--联系人-->
                                <div class="form-group row k-margin-t-20">
                                    <label class="col-form-label col-lg-3 col-sm-12">联系人</label>
                                    <div class="col-lg-9 col-md-9 col-sm-12">
                                        <asp:TextBox ID="inpInfoFormReceiptConsignee" runat="server" class="form-control" ReadOnly="true" />
                                    </div>
                                </div>
                                <!--联系电话-->
                                <div class="form-group row k-margin-t-20">
                                    <label class="col-form-label col-lg-3 col-sm-12">联系电话</label>
                                    <div class="col-lg-9 col-md-9 col-sm-12">
                                        <asp:TextBox ID="inpInfoFormReceiptPhone" runat="server" class="form-control" ReadOnly="true" />
                                    </div>
                                </div>
                                <!--自提门店-->
                                <div class="form-group row k-margin-t-20">
                                    <label class="col-form-label col-lg-3 col-sm-12">自提门店</label>
                                    <div class="col-lg-9 col-md-9 col-sm-12">
                                        <asp:TextBox ID="inpInfoFormReceiptStore" runat="server" class="form-control" ReadOnly="true" />
                                    </div>
                                </div>
                                <!--邮寄地址-->
                                <div class="form-group row k-margin-t-20">
                                    <label class="col-form-label col-lg-3 col-sm-12">邮寄地址</label>
                                    <div class="col-lg-9 col-md-9 col-sm-12">
                                        <asp:TextBox ID="inpInfoFormReceiptAddress" runat="server" class="form-control" ReadOnly="true" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!--支付-->
                        <div class="tab-pane" id="k_tabs_pay" role="tabpanel">
                            <div class="k-form k-form--label-right">
                                <!--支付方式-->
                                <div class="form-group row k-margin-t-20">
                                    <label class="col-form-label col-lg-3 col-sm-12">支付方式</label>
                                    <div class="col-lg-9 col-md-9 col-sm-12">
                                        <asp:TextBox ID="inpInfoFormPayType" runat="server" class="form-control" ReadOnly="true" />
                                    </div>
                                </div>
                                <!--预付单号-->
                                <div class="form-group row k-margin-t-20">
                                    <label class="col-form-label col-lg-3 col-sm-12">预付单号</label>
                                    <div class="col-lg-9 col-md-9 col-sm-12">
                                        <asp:TextBox ID="inpInfoFormPayPreID" runat="server" class="form-control" ReadOnly="true" />
                                    </div>
                                </div>
                                <!--交易状态-->
                                <div class="form-group row k-margin-t-20">
                                    <label class="col-form-label col-lg-3 col-sm-12">交易状态</label>
                                    <div class="col-lg-9 col-md-9 col-sm-12">
                                        <asp:TextBox ID="inpInfoFormPayState" runat="server" class="form-control" ReadOnly="true" />
                                    </div>
                                </div>
                                <!--成交单号-->
                                <div class="form-group row k-margin-t-20">
                                    <label class="col-form-label col-lg-3 col-sm-12">成交单号</label>
                                    <div class="col-lg-9 col-md-9 col-sm-12">
                                        <asp:TextBox ID="inpInfoFormPayTransactionID" runat="server" class="form-control" ReadOnly="true" />
                                    </div>
                                </div>
                                <!--收银单号-->
                                <div class="form-group row k-margin-t-20">
                                    <label class="col-form-label col-lg-3 col-sm-12">收银单号</label>
                                    <div class="col-lg-9 col-md-9 col-sm-12">
                                        <asp:TextBox ID="inpDetailsF2FPayBillNO" runat="server" class="form-control" ReadOnly="true" />
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
    </div>
    <!--end: InfoForm -->
    <!--begin: InfoDetails -->
    <div class="modal fade" id="DetailsModal" tabindex="-1" role="dialog" aria-labelledby="" aria-hidden="true">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">订单[<asp:Label ID="txtDefailsModalTitle" runat="server" />]的详细信息</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true" class="la la-remove"></span>
                    </button>
                </div>
                <div class="modal-body">
                    <!-- 订单信息 -->
                    <fieldset class="detail-fieldset">
                        <legend class="detail-legend">订单信息</legend>
                        <ul>
                            <li><span class="legend-title">订单编号</span>
                                <asp:Label ID="txtDetailsBillNO" runat="server" class="legend-text" /></li>
                            <li><span class="legend-title">订单状态</span>
                                <asp:Label ID="txtDetailsState" runat="server" class="legend-text" /></li>
                            <li><span class="legend-title">订单备注</span>
                                <asp:Label ID="txtDetailsRemark" runat="server" class="legend-text" /></li>
                            <li><span class="legend-title">创建时间</span>
                                <asp:Label ID="txtDetailsCreateTime" runat="server" class="legend-text" /></li>
                            <li><span class="legend-title">更新时间</span>
                                <asp:Label ID="txtDetailsUpdateTime" runat="server" class="legend-text" /></li>
                        </ul>
                        <!-- 分录信息 -->
                        <asp:ListView ID="lvDetailsSOItems" runat="server">
                            <LayoutTemplate>
                                <table class="table table-striped table-bordered table-hover table-checkable list-table">
                                    <thead>
                                        <tr>
                                            <th>行号</th>
                                            <th>商品图片</th>
                                            <th>商品名称</th>
                                            <th>商品规格</th>
                                            <th>数量</th>
                                            <th>单价</th>
                                            <th>金额</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr id="itemPlaceholder" runat="server" />
                                    </tbody>
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr>
                                    <th><%#Eval("OrderID")%></th>
                                    <td>
                                        <img src='/DOC/upload/<%#((List<string>)Eval("GoodsPhotos"))[0]%>' style="max-height: 50px;" /></td>
                                    <td><%#Eval("GoodsTitle")%></td>
                                    <td><%#Eval("GoodsSpecTitle")%></td>
                                    <td><%#Eval("Quantity")%></td>
                                    <td><%#Eval("Price")%></td>
                                    <td><%#Eval("Amount")%></td>
                                </tr>
                            </ItemTemplate>
                        </asp:ListView>
                    </fieldset>
                    <!-- 客户信息 -->
                    <fieldset class="detail-fieldset">
                        <legend class="detail-legend">客户信息</legend>
                        <ul>
                            <li><span class="legend-title">客户类型</span>
                                <asp:Label ID="txtDetailsCustomerType" runat="server" class="legend-text" /></li>
                            <li><span class="legend-title">客户昵称</span>
                                <asp:Label ID="txtDetailsCustomerNickName" runat="server" class="legend-text" /></li>
                            <li><span class="legend-title">客户性别</span>
                                <asp:Label ID="txtDetailsCustomerGender" runat="server" class="legend-text" /></li>
                            <li><span class="legend-title">客户头像</span>
                                <asp:Image ID="imgDetailsCustomerAvatar" runat="server" Style="max-height: 50px;" />
                        </ul>
                    </fieldset>
                    <!-- 收货信息 -->
                    <fieldset class="detail-fieldset">
                        <legend class="detail-legend">收货信息</legend>
                        <ul>
                            <li><span class="legend-title">收货方式</span>
                                <asp:Label ID="txtDetailsReceiptType" runat="server" class="legend-text" /></li>
                            <li><span class="legend-title">联系人</span>
                                <asp:Label ID="txtDetailsReceiptConsignee" runat="server" class="legend-text" /></li>
                            <li><span class="legend-title">联系电话</span>
                                <asp:Label ID="txtDetailsReceiptPhone" runat="server" class="legend-text" /></li>
                            <li><span class="legend-title">提货门店</span>
                                <asp:Label ID="txtDetailsReceiptStore" runat="server" class="legend-text" /></li>
                            <li><span class="legend-title">送货地址</span>
                                <asp:Label ID="txtDetailsReceiptAddress" runat="server" class="legend-text" /></li>
                        </ul>
                    </fieldset>
                    <!-- 支付信息 -->
                    <fieldset class="detail-fieldset">
                        <legend class="detail-legend">支付信息</legend>
                        <ul>
                            <li><span class="legend-title">支付方式</span>
                                <asp:Label ID="txtDetailsPayType" runat="server" class="legend-text" /></li>
                            <li><span class="legend-title">预付单号</span>
                                <asp:Label ID="txtDetailsPayPreID" runat="server" class="legend-text" /></li>
                            <li><span class="legend-title">交易状态</span>
                                <asp:Label ID="txtDetailsPayState" runat="server" class="legend-text" /></li>
                            <li><span class="legend-title">成交单号</span>
                                <asp:Label ID="txtDetailsPayTransactionID" runat="server" class="legend-text" /></li>
                            <li><span class="legend-title">收银单号</span>
                                <asp:Label ID="txtDetailsF2FPayBillNO" runat="server" class="legend-text" /></li>
                        </ul>
                    </fieldset>
                </div>
            </div>
        </div>
    </div>
    <!--end: InfoDetails -->
    <!--begin: PayForm -->
    <div class="modal fade" id="PayModal" tabindex="-1" role="dialog" aria-labelledby="" aria-hidden="true">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">收银单</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true" class="la la-remove"></span>
                    </button>
                </div>
                <div class="modal-body">
                    <asp:HiddenField ID="hidPayFormSOID" runat="server" />
                    <div>
                        <span class="legend-title">订单号</span>
                        <asp:Label ID="txtPayFormBillNO" runat="server" class="legend-text" /><br />
                        <span>应收：</span><asp:Label ID="txtReceiptsMoney" runat="server" ClientIDMode="Static" class="Amount" />
                        <span>实收：</span><asp:Label ID="txtPayAmount" runat="server" ClientIDMode="Static" class="Amount" />
                        <span>找零：</span><asp:Label ID="txtPayChange" runat="server" ClientIDMode="Static" class="Amount" />
                    </div>

                    <fieldset class="detail-fieldset">
                        <legend class="detail-legend">收银信息</legend>
                        <asp:TextBox ID="inpPayFormCode" runat="server" class="txtPayCode" ClientIDMode="Static"
                            AutoPostBack="true" OnTextChanged="PayFormCode_Change" placeholder="微信、支付宝扫码" />
                        <div class="k-form k-form--label-right">
                            <div class="form-group row k-margin-t-20">
                                <label class="col-form-label col-lg-3 col-sm-12" style="flex: 0 0 10%;">现金</label>
                                <div class="col-lg-5 col-md-5 col-sm-12">
                                    <asp:TextBox ID="inpPayFormCash" runat="server" ClientIDMode="Static"
                                        onchange="PayFormAmountChange(this)" class="form-control txtPrice" />
                                </div>
                            </div>
                            <div class="form-group row k-margin-t-20">
                                <label class="col-form-label col-lg-3 col-sm-12" style="flex: 0 0 10%;">刷卡</label>
                                <div class="col-lg-5 col-md-5 col-sm-12">
                                    <asp:TextBox ID="inpPayFormCard" runat="server" ClientIDMode="Static"
                                        onchange="PayFormAmountChange(this)" class="form-control txtPrice" />
                                </div>
                            </div>
                        </div>
                    </fieldset>

                    <div class="k-form k-form--label-right">
                        <div class="form-group row k-margin-t-20">
                            <label class="col-form-label col-lg-3 col-sm-12" style="flex: 0 0 10%;">备注</label>
                            <div class="col-lg-10 col-md-10 col-sm-12">
                                <asp:TextBox ID="inpPayFormRemark" runat="server" class="form-control" />
                            </div>
                        </div>
                    </div>

                    <div style="text-align: center;">
                        <asp:Button ID="btnBalance" Text="收款" OnClick="btnBalance_Click" class="btn btn-primary" runat="server" />
                        <asp:Button ID="btnFree" Text="免单" OnClick="btnFree_Click" Visible="false" class="btn btn-danger" runat="server" />
                    </div>

                </div>
            </div>
        </div>
    </div>
    <!--end: PayForm -->
    <!-- end:: Content -->
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PageFootPlaceHolder" runat="server">
    <script>
        $(document).ready(function () {
            //金额
            $('.txtPrice').TouchSpin({
                buttondown_class: 'btn btn-secondary',
                buttonup_class: 'btn btn-secondary',
                prefix: '¥',

                min: 0,
                max: 1000000,
                step: 0.01,
                decimals: 2,
                boostat: 5,
                maxboostedstep: 10,
            });
        });
    </script>
</asp:Content>
