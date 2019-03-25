"use strict";

//表单提交检查
function GoodsFormCheck() {
    var inps = ['inpInfoFormGoodsName', 'inpInfoFormGoodsTitle'];
    if (FormCheck(inps)) { return true; };
    return false;
}
//表单规格全选按钮
function SpecSelectAll(sender, ulid) {
    var inputs = $(ulid + ' input');
    for (var i = 0; i < inputs.length; i++) { inputs[i].checked = sender.checked; }
}
//表单规格统一单价
function SpecPriceAll(sender) {
    var inpSpecPrices = $('.txtSpecPrice');
    for (var i = 0; i < inpSpecPrices.length; i++) {
        inpSpecPrices[i].value = sender.value;
    }
}
//表单规格统一库存
function SpecQtyAll(sender) {
    var inpSpecQtys = $('.txtSpecQty');
    for (var i = 0; i < inpSpecQtys.length; i++) {
        inpSpecQtys[i].value = sender.value;
    }
}
//表单规格表单显示
function InfoFormTabShow(TabName) {
    var selectModalClose = document.getElementById('btn' + TabName + 'SelectClose');
    if (selectModalClose != null) { selectModalClose.click(); }
    document.getElementById('btnListInfoFormModal').click();
    document.getElementById('btnListInfoForm' + TabName + 'Tab').click();
}
//详情数量变化时
function DetailsQtyChange(sender) {
    DetailsPriceCounter();
}

//详情规格选择时
function DetailsSpecSelect(sender) {
    var spanValues = $(sender).parent().find('span');
    spanValues.attr('selected', 'false');
    sender.setAttribute('selected', 'true');
    DetailsPriceCounter();
}

//详情计算金额
function DetailsPriceCounter() {
    //金额
    var txtAmount = document.getElementById('PageContentPlaceHolder_txtDetailsAmount');
    var amountUnit = txtAmount.innerText.substring(0, 1);
    var amount = 0.00;
    //单价、数量
    var inpBillQty = document.getElementById('PageContentPlaceHolder_inpDetailsQty');
    var billQty = parseFloat(inpBillQty.value);
    var invQty = 0;
    var unitPrice = 0.00;
    var hidGoodsSpecs = document.getElementById('hidDetailsGoodsSpecs');
    var goodsSpecs = hidGoodsSpecs == null ? null : JSON.parse(document.getElementById('hidDetailsGoodsSpecs').value);
    if (goodsSpecs == null) {
        //无规格
        invQty = parseFloat(document.getElementById('PageContentPlaceHolder_txtDetailsQty').innerText);
        unitPrice = parseFloat(document.getElementById('PageContentPlaceHolder_txtDetailsPrice').innerText.replace(amountUnit, ''));
    }
    else {
        //有规格
        var specValues = {
            Nulls: '',
            IDs: '',
        };
        var specs = $('.spec-fieldset');
        for (var i = 0; i < specs.length; i++) {
            var valueSelected = $(specs[i]).find("span[selected$='true']");
            if (valueSelected.length == 0) { specValues.Nulls += (specValues.Nulls.length == 0 ? '' : ',') + $(specs[i]).find('legend')[0].innerText; }
            else { specValues.IDs += (specValues.IDs.length == 0 ? '' : ',') + valueSelected[0].getAttribute('specvalueid'); }
        }
        //规格单价
        if (specValues.Nulls.length == 0) {
            for (var i = 0; i < goodsSpecs.length; i++) {
                var goodsSpec = goodsSpecs[i];
                if (specValues.IDs == goodsSpec.SpecValueIDs) {
                    invQty = goodsSpec.Quantity;
                    unitPrice = goodsSpec.Price;
                }
            }
        }
    }
    //计算
    if (billQty <= 0) {
        billQty = 1;
        inpBillQty.value = billQty;
    }
    else if (billQty > invQty) {
        billQty = invQty;
        inpBillQty.value = billQty;
    }
    amount = billQty * unitPrice;
    txtAmount.innerText = amountUnit + amount.toFixed(2);
}