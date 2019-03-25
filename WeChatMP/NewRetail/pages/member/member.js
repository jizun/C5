const app = getApp();

Page({
  data: {
    UserInfo: null,
    MemberTabs: ['积分', '优惠券', '会员卡', '更多...'],
  },
  onLoad: function(options) {
    wx.showLoading({
      title: '加载中',
    });
    this.getUserInfo();
  },
  //获取用户信息
  getUserInfo: function(event) {
    if (app.globalData.UserInfo != null) {
      this.setData({
        UserInfo: app.globalData.UserInfo,
      });
      wx.hideLoading();
    } else {
      var that = this;
      app.getUserInfo().then(function(res) {
        that.setData({
          UserInfo: app.globalData.UserInfo,
        });
        wx.hideLoading();
      }).catch(res => {
        wx.showModal({
          title: '静默登录错误',
          content: res,
          showCancel: false,
        });
        wx.hideLoading();
      });
    };
  },
  //注册新用户
  onSetUserInfo: function(event) {
    var that = this;
    app.setUserInfo(event).then(function(event) {
      that.setData({
        UserInfo: app.globalData.UserInfo,
      });
    }).catch(res => {
      wx.showModal({
        title: '登记用户信息错误',
        content: res,
        showCancel: false,
      });
      wx.hideLoading();
      return;
    });
  },
  onReady: function() {},
  onShow: function() {},
  onHide: function() {},
  onUnload: function() {},
  onPullDownRefresh: function() {},
  onReachBottom: function() {},
  onShareAppMessage: function() {}
})