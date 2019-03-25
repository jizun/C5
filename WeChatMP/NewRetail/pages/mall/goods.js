const app = getApp();

Page({
  data: {
    GoodsList: null,
  },
  //页面加载
  onLoad: function(options) {
    wx.showLoading({
      title: '加载中',
    });
    this.getGoodsList(options.brandid);
  },
  //获取商品列表
  getGoodsList: function(brandid) {
    wx.request({
      url: app.globalData.apiBaseUrl + 'MOD/OMS/WeChat.asmx/GetMallGoodsList',
      data: {
        brand_id: brandid
      },
      method: 'POST',
      header: {
        'cookie': wx.getStorageSync("sessionid")
      },
      success: res => {
        if (res.statusCode == 200) {
          if (res.data.d.Code == 'Success') {
            //补充图片URL
            var goodsList = res.data.d.Data;
            for (var i = 0; i < goodsList.length; i++) {
              var goodsInfo = goodsList[i];
              for (var j = 0; j < goodsInfo.Photos.length; j++) {
                goodsInfo.Photos[j] = app.globalData.apiBaseUrl + 'doc/upload/' + goodsInfo.Photos[j];
              }
            };
            //设置页面对象
            this.setData({
              GoodsList: goodsList,
            });
            wx.hideLoading();
          } else {
            wx.showModal({
              title: '获取商品列表错误',
              content: '服务器错误' + res.data.d.Message,
              showCancel: false,
            });
          }
        } else {
          wx.showModal({
            title: '获取商品列表错误',
            content: '服务器错误码' + res.statusCode,
            showCancel: false,
          });
        }
      }
    });
  },
  //浏览图册
  imgBrowse: function(event) {
    wx.previewImage({
      current: event.currentTarget.dataset.src,
      urls: this.data.GoodsList[event.currentTarget.dataset.goodsindex].Photos,
    });
  },

  /**
   * 生命周期函数--监听页面初次渲染完成
   */
  onReady: function() {

  },

  /**
   * 生命周期函数--监听页面显示
   */
  onShow: function() {

  },

  /**
   * 生命周期函数--监听页面隐藏
   */
  onHide: function() {

  },

  /**
   * 生命周期函数--监听页面卸载
   */
  onUnload: function() {

  },

  /**
   * 页面相关事件处理函数--监听用户下拉动作
   */
  onPullDownRefresh: function() {

  },

  /**
   * 页面上拉触底事件的处理函数
   */
  onReachBottom: function() {

  },

  /**
   * 用户点击右上角分享
   */
  onShareAppMessage: function() {

  }
})