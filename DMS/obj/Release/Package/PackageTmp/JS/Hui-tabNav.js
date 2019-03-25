"use strict";

//动态TAB
function AURLTab(title, url) {
    var a = document.createElement('a');
    a.setAttribute('data-title', title);
    a.setAttribute('data-url', url);
    Hui_admin_tab(a);
}

//创建TAB
function Hui_admin_tab(sender) {
    if (!$(sender).attr('data-url')) {
        alert('没有[data-url]属性！');
        return false;
    }

    var topWindow = $(window.parent.document);

    //父窗口菜单
    if (sender.parentNode) {
        var menus = topWindow.find('#k_aside_menu li.k-menu__item');
        menus.removeClass("k-menu__item--active");
        sender.parentNode.className += (' k-menu__item--active');
    }

    //TAB导航菜单
    var hasFindTab = false;
    var findTabIndex = 0;
    var dataUrl = $(sender).attr('data-url');
    var dataTitle = $(sender).attr("data-title");
    var show_navLi = topWindow.find("#min_title_list li");
    show_navLi.each(function () {
        if ($(this).find('span').attr("data-href") == dataUrl) {
            hasFindTab = true;
            findTabIndex = show_navLi.index($(this));
            return false;
        }
    });
    if (!hasFindTab) {
        creatIframe(dataUrl, dataTitle);
        min_titleList();
    }
    else {
        show_navLi.removeClass("active").eq(findTabIndex).addClass("active");
        var iframe_box = topWindow.find("#iframe_box");
        iframe_box.find(".show_iframe").hide().eq(findTabIndex).show().find("iframe").attr("src", dataUrl);
    }
}

//更新TAB
function tabNavallwidth() {
    var taballwidth = 0,
        $tabNav = hide_nav.find(".acrossTab"),
        $tabNavWp = hide_nav.find(".Hui-tabNav-wp"),
        $tabNavitem = hide_nav.find(".acrossTab li"),
        $tabNavmore = hide_nav.find(".Hui-tabNav-more");
    if (!$tabNav[0]) { return }
    $tabNavitem.each(function (index, element) {
        taballwidth += Number(parseFloat($(this).width() + 60))
    });
    $tabNav.width(taballwidth + 25);
    var w = $tabNavWp.width();
    if (taballwidth + 25 > w) {
        $tabNavmore.show()
    }
    else {
        $tabNavmore.hide();
        $tabNav.css({ left: 0 })
    }
}

//更新TAB标签
function min_titleList() {
    var topWindow = $(window.parent.document);
    var show_nav = topWindow.find("#min_title_list");
    var aLi = show_nav.find("li");
};

//创建TAB窗体
function creatIframe(href, titleName) {
    var topWindow = $(window.parent.document);
    var show_nav = topWindow.find('#min_title_list');
    show_nav.find('li').removeClass("active");
    var iframe_box = topWindow.find('#iframe_box');
    show_nav.append('<li class="active"><span data-href="' + href + '">' + titleName + '</span><i></i><em></em></li>');
    var taballwidth = 0,
        $tabNav = topWindow.find(".acrossTab"),
        $tabNavWp = topWindow.find(".Hui-tabNav-wp"),
        $tabNavitem = topWindow.find(".acrossTab li"),
        $tabNavmore = topWindow.find(".Hui-tabNav-more");
    if (!$tabNav[0]) { return }
    $tabNavitem.each(function (index, element) {
        taballwidth += Number(parseFloat($(this).width() + 60))
    });
    $tabNav.width(taballwidth + 25);
    var w = $tabNavWp.width();
    if (taballwidth + 25 > w) {
        $tabNavmore.show()
    }
    else {
        $tabNavmore.hide();
        $tabNav.css({ left: 0 })
    }
    var iframeBox = iframe_box.find('.show_iframe');
    iframeBox.hide();
    iframe_box.append('<div class="show_iframe"><div class="loading"></div><iframe frameborder="0" src=' + href + '></iframe></div>');
    var showBox = iframe_box.find('.show_iframe:visible');
    showBox.find('iframe').on('load', function () { showBox.find('.loading').hide(); });
}

//移除TAB窗体
function removeIframe() {
    var topWindow = $(window.parent.document);
    var iframe = topWindow.find('#iframe_box .show_iframe');
    var tab = topWindow.find(".acrossTab li");
    var showTab = topWindow.find(".acrossTab li.active");
    var showBox = topWindow.find('.show_iframe:visible');
    var i = showTab.index();
    tab.eq(i - 1).addClass("active");
    iframe.eq(i - 1).show();
    tab.eq(i).remove();
    iframe.eq(i).remove();
}

//初始化TAB
var num = 0, oUl = $("#min_title_list"), hide_nav = $("#Hui-tabNav");
$(function () {
    //绑定左边菜单点击事件
    $(".k-menu__nav").on("click", ".iframe_link", function () {
        Hui_admin_tab(this);
    });

    //绑定左下菜单点击事件
    $('.k-aside__footer-nav').on("click", ".iframe_link", function () {
        Hui_admin_tab(this);
    });

    //绑定通知菜单点击事件
    $(".k-notification").on("click", ".k-notification__item", function () {
        Hui_admin_tab(this);
    });

    //绑定TAB点击事件
    $(document).on("click", "#min_title_list li", function () {
        var bStopIndex = $(this).index();
        var iframe_box = $("#iframe_box");
        $("#min_title_list li").removeClass("active").eq(bStopIndex).addClass("active");
        iframe_box.find(".show_iframe").hide().eq(bStopIndex).show();
    });

    //绑定TAB关闭事件
    $(document).on("click", "#min_title_list li i", function () {
        var aCloseIndex = $(this).parents("li").index();
        $(this).parent().remove();
        $('#iframe_box').find('.show_iframe').eq(aCloseIndex).remove();
        num == 0 ? num = 0 : num--;
        tabNavallwidth();
    });

    //绑定TAB双击事件
    $(document).on("dblclick", "#min_title_list li", function () {
        var aCloseIndex = $(this).index();
        var iframe_box = $("#iframe_box");
        if (aCloseIndex > 0) {
            $(this).remove();
            $('#iframe_box').find('.show_iframe').eq(aCloseIndex).remove();
            num == 0 ? num = 0 : num--;
            $("#min_title_list li").removeClass("active").eq(aCloseIndex - 1).addClass("active");
            iframe_box.find(".show_iframe").hide().eq(aCloseIndex - 1).show();
            tabNavallwidth();
        } else {
            return false;
        }
    });

    //获取顶部选项卡总长度
    tabNavallwidth();

    //TAB导航
    $('#js-tabNav-next').click(function () {
        num == oUl.find('li').length - 1 ? num = oUl.find('li').length - 1 : num++;
        toNavPos();
    });
    $('#js-tabNav-prev').click(function () {
        num == 0 ? num = 0 : num--;
        toNavPos();
    });
    function toNavPos() {
        oUl.stop().animate({ 'left': -num * 100 }, 100);
    }
});
