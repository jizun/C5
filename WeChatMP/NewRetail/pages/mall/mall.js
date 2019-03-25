const app = getApp();
Page({
  data: {
    BrandList: null,
  },
  //页面加载
  onLoad: function(options) {
    wx.showLoading({
      title: '加载中',
    });
    this.getBrandList();
  },
  //获取品牌列表
  getBrandList: function() {
    wx.request({
      url: app.globalData.apiBaseUrl + 'MOD/OMS/WeChat.asmx/GetMallBrandList',
      data: "",
      method: 'POST',
      header: {
        'cookie': wx.getStorageSync("sessionid")
      },
      success: res => {
        if (res.statusCode == 200) {
          if (res.data.d.Code == 'Success') {
            this.setData({
              BrandList: res.data.d.Data,
            });
          } else {
            wx.showModal({
              title: '获取品牌列表错误',
              content: res.data.d.Message,
              showCancel: false,
            });
          }
          wx.hideLoading();
        } else {
          wx.showModal({
            title: '获取品牌列表错误',
            content: '服务器错误码' + res.statusCode,
            showCancel: false,
          });
        }
      }
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