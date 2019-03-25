const app = getApp();

Page({
  data: {
    StoreList: null,
    SelectedStoreID: null
  },
  onLoad: function(options) {
    this.setData({
      SelectedStoreID: options.storeid
    });
    this.getStoreList();
  },
  //获取门店列表
  getStoreList: function(event) {
    if (app.globalData.StoreList != null) {
      this.setData({
        StoreList: app.globalData.StoreList,
      });
    } else {
      var that = this;
      app.getStoreList().then(function(res) {
        that.setData({
          StoreList: app.globalData.StoreList,
        });
      });
    };
  },
  //电话咨询
  onPhoneCall: function(event) {
    wx.makePhoneCall({
      phoneNumber: event.currentTarget.dataset.phone
    });
  },
  //浏览图册
  imgBrowse: function(event) {
    wx.previewImage({
      current: event.currentTarget.dataset.src,
      urls: this.data.StoreList[this.data.SelectedStoreID].Photos
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