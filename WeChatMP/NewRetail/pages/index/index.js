const app = getApp();

Page({
  data: {
    Banner: {
      imgUrls: [
        'https://www.jixinxj.com/media/banner/1.jpg',
        'https://www.jixinxj.com/media/banner/2.jpg',
        'https://www.jixinxj.com/media/banner/3.jpg',
        'https://www.jixinxj.com/media/banner/4.jpg',
        'https://www.jixinxj.com/media/banner/5.jpg',
      ],
      indicatorDots: true,
      indicatorColor: 'rgba(0, 0, 0, 0.3)',
      indicatorActiveColor: '#000000',
      autoplay: true,
      interval: 5000,
      duration: 1000,
    },
    UserInfo: null,
    LocalAuth: null,
    StoreList: null,
    NearbyStore: null,
    MapKey: '63VBZ-ZOKAU-BLAV7-4D74C-RZ2TJ-ZZBPY',
    GoodsList: null,
  },
  //页面加载
  onLoad: function() {
    wx.showLoading({
      title: '加载中',
    });
    this.getUserInfo();
    this.getStoreList();
    this.getGoodsList(0);
  },
  onReady: function() {},
  //获取用户信息
  getUserInfo: function(event) {
    if (app.globalData.UserInfo != null) {
      this.setData({
        UserInfo: app.globalData.UserInfo,
      });
    } else {
      var that = this;
      app.getUserInfo().then(function(res) {
        that.setData({
          UserInfo: app.globalData.UserInfo,
        });
      }).catch(res => {
        wx.showModal({
          title: '静默登录错误',
          content: res,
          showCancel: false,
        });
        wx.hideLoading();
        return;
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
  //获取门店列表
  getStoreList: function(event) {
    if (app.globalData.StoreList != null) {
      this.setData({
        LocalAuth: app.globalData.LocalAuth,
        StoreList: app.globalData.StoreList,
        NearbyStore: app.globalData.NearbyStore,
      });
    } else {
      var that = this;
      app.getStoreList().then(res => {
        that.setData({
          LocalAuth: app.globalData.LocalAuth,
          StoreList: app.globalData.StoreList,
          NearbyStore: app.globalData.NearbyStore,
        });
      }).catch(res => {
        wx.showModal({
          title: '查询附近门店错误',
          content: res,
          showCancel: false,
        });
        return;
      });
    };
  },
  //获取商品列表
  getGoodsList: function (brandid) {
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
            wx.hideLoading();
          }
        } else {
          wx.showModal({
            title: '获取商品列表错误',
            content: '服务器错误码' + res.statusCode,
            showCancel: false,
          });
          wx.hideLoading();
        }
      }
    });
  },
  //浏览图册
  imgBrowse: function(event) {
    wx.previewImage({
      current: event.currentTarget.dataset.src,
      urls: this.data.StoreList[0].Photos
    });
  },
})