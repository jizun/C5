"use strict";

$(document).ready(function () {
    //表单
    var FormsName = ['#InfoFormModal', '#SpecSelectModal'];
    for (var i = 0; i < FormsName.length; i++) { ModalHeadScroll(FormsName[i]); };
    //ModalHeadScroll('#InfoFormModal, #SpecSelectModal');
    //文字
    $('#inpInfoFormGoodsName, #inpInfoFormGoodsTitle, .inp-txt-len').maxlength({
        alwaysShow: true,
        threshold: 5,
        warningClass: "k-badge k-badge--primary k-badge--rounded k-badge--inline",
        limitReachedClass: "k-badge k-badge--brand k-badge--rounded k-badge--inline"
    });
    //数值
    $('.txtQuantity').TouchSpin({
        buttondown_class: 'btn btn-secondary',
        buttonup_class: 'btn btn-secondary',
        verticalbuttons: true,
        verticalupclass: 'la la-plus',
        verticaldownclass: 'la la-minus',

        min: 0,
        max: 1000000,
        step: 1,
    });
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
    //标签
    $('.k-repeater').each(function () {
        $(this).repeater({
            show: function () { $(this).slideDown(); },
            isFirstItemUndeletable: true
        });
    });
    //富文本
    var htmDescID = '#txtInfoFormGoodsDesc';
    var inpDesc = document.getElementById('hidInfoFormGoodsDesc');
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
                        url: "/MOD/SYS/Upload.ashx?mod=goods&act=sumnmber",
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
});

//图片
var KDropzoneDemo = function () {
    var demos = function () {
        Dropzone.options.kDropzoneTwo = {
            url: "/MOD/SYS/Upload.ashx?mod=goods&act=dropzone",
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

                GoodsPhotoFill(this);

                this.on("addedfile", function () { });
                this.on("success", function (file, imageInfo) { });
                this.on("complete", function (data) {
                    if (data.xhr != null && data.xhr.readyState == 4 && data.xhr.status == 200) {
                        var res = eval('(' + data.xhr.responseText + ')');
                        if (res.Result) {
                            GoodsPhotoAdd(res.Files);
                        }
                    }
                });
                this.on("removedfile", function (file) {
                    GoodsPhotoRemove(file);
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
function GoodsPhotoFill(Dropzone) {
    var hidFiles = document.getElementById('hidInfoFormGoodsPhoto');
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
function GoodsPhotoAdd(Files) {
    var hidFiles = document.getElementById('hidInfoFormGoodsPhoto');
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
function GoodsPhotoRemove(File) {
    var hidFiles = document.getElementById('hidInfoFormGoodsPhoto');
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