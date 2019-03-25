"use strict";

//筛选表单检查
function FilterCheck() {
    var inp = document.getElementById('inpFilter');
    if (inp.value.length == 0) {
        FormAlert('查询错误', '请输入要查找的关键字。', 'inpFilter');
        return false;
    }
    return true;
}
//信息表单检查
function FormCheck(inps) {
    for (var i = 0; i < inps.length; i++) {
        var inp = document.getElementById(inps[i]);
        if (inp == null) {
            console.log('控件[' + inps[i] + ']不存在');
            return false
        }
        if (inp.value.length == 0) {
            FormAlert('保存错误', inp.title + '必须填写', inp);
            return false;
        }
    }
    return true;
}
//表单错误提示
function FormAlert(title, text, inpOrName) {
    var inp = inpOrName;
    if (typeof (inpOrName) == 'string') {
        inp = document.getElementById(inpOrName);
        if (inp == null) {
            console.log('信息表单控件[' + inpOrName + ']不存在！');
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

//获取当前iframe
//var tFrames = $(window.parent.document).find("#iframe_box .show_iframe");
//for (var i = 0; i < tFrames.length; i++) {
//    if (tFrames[i].style.display == '') {
//        var tFrame = $(tFrames[i]).find("iframe")[0];
//        tFrame.contentWindow.addEventListener("scroll", function (e) { console.log('a'); });
//        break;
//    }
//}

//表单头固顶
function ModalHeadScroll(ModalIDs) {
    var modalIDs = ModalIDs.split(',');
    for (var i = 0; i < modalIDs.length; i++) {
        var modalID = $.trim(modalIDs[i]);
        var Modal = $(modalID);
        Modal.on("scroll", null, function () {
            var divModalHead = Modal.find('.modal-header')[0];
            var hasKeepTop = (Modal.scrollTop() > 0);
            divModalHead.style.zIndex = hasKeepTop ? '99999' : 'initial';
            divModalHead.style.position = hasKeepTop ? 'fixed' : 'initial';
            divModalHead.style.top = hasKeepTop ? '0px' : 'initial';
            divModalHead.style.left = hasKeepTop ? '0px' : 'initial';
            divModalHead.style.width = hasKeepTop ? '100%' : 'initial';
            divModalHead.style.background = hasKeepTop ? 'white' : 'initial';
        });
    }
}