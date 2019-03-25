const app = getApp();

Page({
  data: {
    LocalAuth: false,
    StoreList: null,
  },
  onLoad: function(options) {
    this.getLocalAuth();
    this.getStoreList();
  },
  //获取定位授权
  getLocalAuth: function(event) {
    wx.getSetting({
      success: (res) => {
        this.setData({
          LocalAuth: res.authSetting['scope.userLocation']
        });
      }
    });
  },
  //获取门店列表
  getStoreList: function(event) {
    if (app.globalData.StoreList != null) {
      this.setData({
        StoreList: app.globalData.StoreList,
        NearbyStore: app.globalData.NearbyStore,
      });
    } else {
      var that = this;
      app.getStoreList().then(function(res) {
        that.setData({
          StoreList: app.globalData.StoreList,
          NearbyStore: app.globalData.NearbyStore,
        });
      }).catch(res => {
        //console.log(res);
      });
    };
    this.getLocalAuth();
  },
  onReady: function() {},
  onShow: function() {},
  onHide: function() {},
  onUnload: function() {},
  onPullDownRefresh: function() {},
  onReachBottom: function() {},
  onShareAppMessage: function() {}
})