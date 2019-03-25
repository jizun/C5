const app = getApp();

var QQMapWX = require('../../lib/qqmap-wx-jssdk.min.js');
var qqmapsdk;

Page({
  data: {
    GoodsDetail: null,
    UserCode: null,
    OrderInfo: null,
    StoreList: null,
    ReceiveInfo: {
      ReceiveType: 'Stores', //Logistics
      Consignee: null,
      Phone: null,
      StoreID: null,
      Address: null,
      AddressCheck: null,
      PayType: null,
      Remark: null,
    },
    AddressMarkers: null,
    AddressLocation: null,
    Paid: false,
    PayInfo: null,
  },
  //页面加载时
  onLoad: function(options) {
    wx.showLoading({
      title: '加载中',
    });
    var pages = getCurrentPages(); 
    var lastPage = pages[pages.length - 2]; 
    this.setData({
      GoodsDetail: lastPage.data.GoodsDetail,
      OrderInfo: lastPage.data.OrderInfo,
    });
    this.getStoreList();
    qqmapsdk = new QQMapWX({
      key: '63VBZ-ZOKAU-BLAV7-4D74C-RZ2TJ-ZZBPY'
    });
  },
  //获取门店列表
  getStoreList: function(event) {
    if (app.globalData.StoreList != null) {
      this.setData({
        StoreList: app.globalData.StoreList,
      });
      this.setReceiveStore();
    } else {
      var that = this;
      app.getStoreList().then(function(res) {
        that.setData({
          StoreList: app.globalData.StoreList,
        });
        this.setReceiveStore();
      }).catch(res => {
        //console.log(res);
        wx.hideLoading();
      });
    };
  },
  //设置默认自提门店
  setReceiveStore() {
    if (this.data.StoreList.length > 0) {
      var receiveInfo = this.data.ReceiveInfo;
      receiveInfo.StoreID = this.data.StoreList[0].ID;
      this.setData({
        ReceiveInfo: receiveInfo,
      });
      wx.hideLoading();
    }
  },
  //收货方式变更
  onReceiveTypeChange: function(event) {
    var receiveInfo = this.data.ReceiveInfo;
    receiveInfo.ReceiveType = event.currentTarget.dataset.receive_type;
    this.setData({
      ReceiveInfo: receiveInfo,
    });
  },
  //选择门店时
  onStoreChange: function(event) {
    var receiveInfo = this.data.ReceiveInfo;
    receiveInfo.StoreID = this.data.StoreList[event.detail.value].ID;
    this.setData({
      ReceiveInfo: receiveInfo,
    });
  },
  //表单控件变更
  onInputChange(event) {
    var receiveInfo = this.data.ReceiveInfo;
    receiveInfo[event.currentTarget.dataset.prop] = event.detail.value;
    this.setData({
      ReceiveInfo: receiveInfo,
    });
    //检查地址有效性
    if (event.currentTarget.dataset.prop == 'Address' && event.detail.value.length > 0) {
      var that = this;
      qqmapsdk.geocoder({
        address: event.detail.value,
        success: function(res) {
          that.setMapMarker(res.result);
        },
        fail: function(res) {
          wx.showModal({
            title: '填写错误',
            content: '收货地址不够明确哦，请检查一下哈。',
            showCancel: false,
          });
          that.setMapMarker(null);
        },
      });
    }
  },
  //设置地图标点
  setMapMarker: function(res) {
    var receiveInfo = this.data.ReceiveInfo;
    var markers = null;
    var location = null;
    if (res != null && res.reliability >= 7) {
      receiveInfo.AddressCheck = res;
      markers = [{
        id: 0,
        title: res.title,
        latitude: res.location.lat,
        longitude: res.location.lng,
        iconPath: '/media/images/location.png',
        callout: {
          content: res.title,
          color: '#000',
          display: 'ALWAYS'
        }
      }];
      location = res.location;
    };
    this.setData({
      ReceiveInfo: receiveInfo,
      AddressMarkers: markers,
      AddressLocation: location,
    });
  },
  //提交订单
  onOrderSubmit(event) {
    if (this.data.OrderInfo.BillID == 0) {
      return; //正在创建中
    }
    //检查表单
    var receiveInfo = this.data.ReceiveInfo;
    if (receiveInfo.Consignee == null || receiveInfo.Consignee.length == 0) {
      wx.showModal({
        title: '填写错误',
        content: '收货联系人未填写',
        showCancel: false,
      });
      return;
    }
    if (receiveInfo.Phone == null || receiveInfo.Phone.length == 0) {
      wx.showModal({
        title: '填写错误',
        content: '收货联系电话未填写',
        showCancel: false,
      });
      return;
    }
    if (receiveInfo.ReceiveType == 'Logistics' &&
      (receiveInfo.Address == null || receiveInfo.Address.length == 0)) {
      wx.showModal({
        title: '填写错误',
        content: '收货地址未填写',
        showCancel: false,
      });
      return;
    }
    if (event.currentTarget.dataset.paytype) {
      receiveInfo.PayType = event.currentTarget.dataset.paytype;
    }
    this.setData({
      ReceiveInfo: receiveInfo,
    });
    var that = this;
    //静默登录
    wx.login({
      success: function(res) {
        that.setData({
          UserCode: res.code,
        });
        that.setOrderInfo();
      },
      fail: function(res) {
        wx.showModal({
          title: '微信小程序错误',
          content: '静默登录失败：' + res.errMsg,
          showCancel: false,
        });
        return;
      }
    });
  },
  //设置订单信息
  setOrderInfo: function() {
    var orderInfo = this.data.OrderInfo;
    if (orderInfo.BillID == null) {
      orderInfo.BillID = 0;
      this.setData({
        OrderInfo: orderInfo,
      });
    };
    wx.showLoading({
      title: '加载中',
    });
    var that = this;
    var action_title = orderInfo.BillID == 0 ? '创建' : '修改';
    wx.request({
      url: app.globalData.apiBaseUrl + 'MOD/OMS/WeChat.asmx/SetOrderInfo',
      data: {
        user_code: that.data.UserCode,
        order_info: JSON.stringify(that.data.OrderInfo),
        receive_info: JSON.stringify(that.data.ReceiveInfo),
      },
      method: 'POST',
      header: {
        'cookie': wx.getStorageSync("sessionid")
      },
      success: res => {
        if (res.statusCode == 200) {
          if (res.data.d.Code == 'Success') {
            var orderInfo = that.data.OrderInfo;
            orderInfo.BillID = res.data.d.Data.SOInfo.SalesOrder.ID;
            orderInfo.BillNO = res.data.d.Data.SOInfo.SalesOrder.BillNO;
            var paid = res.data.d.Data.Paid;
            var payInfo = JSON.parse(res.data.d.Data.PayInfo);
            //显示订单信息
            that.setData({
              OrderInfo: orderInfo,
              Paid: paid,
              PayInfo: payInfo,
            });
            //支付状态
            if (that.data.ReceiveInfo.PayType == 'WxPay') {
              //微信支付
              if (!paid) {
                if (res.data.d.Message != null) {
                  wx.showModal({
                    title: '订单已' + action_title,
                    content: '单号' + orderInfo.BillNO +
                      '，\r\n' + res.data.d.Message + '请稍候重试，谢谢！',
                    showCancel: false,
                  });
                } else {
                  that.onWxPay();
                }
              }
            } else {
              //到店支付
              wx.showModal({
                title: '订单已' + action_title,
                content: '单号' + orderInfo.BillNO +
                  '，门店客服专员将与您联系确认订单，请保持电话畅通哦！',
                showCancel: false,
              });
            }
          } else {
            wx.showModal({
              title: '订单错误',
              content: res.data.d.Message,
              showCancel: false,
            });
          }
        } else {
          wx.showModal({
            title: '服务器错误',
            content: '错误码' + res.statusCode,
            showCancel: false,
          });
        }
      },
      complete: function() {
        wx.hideLoading();
      }
    });
  },
  //微信支付
  onWxPay: function() {
    var that = this;
    var payInfo = this.data.PayInfo;
    if (payInfo == null) {
      wx.showModal({
        title: '支付失败',
        content: '不符合支付条件',
        showCancel: false,
      });
      return;
    }
    wx.requestPayment({
      'timeStamp': payInfo.timeStamp,
      'nonceStr': payInfo.nonceStr,
      'package': payInfo.package,
      'signType': payInfo.signType,
      'paySign': payInfo.paySign,
      'complete': res => {
        var that = this;
        if (res.errMsg == 'requestPayment:ok') {
          wx.request({
            url: app.globalData.apiBaseUrl + 'MOD/OMS/WeChat.asmx/SetOrderPaid',
            data: {
              user_code: null,
              order_info: JSON.stringify(that.data.OrderInfo),
            },
            method: 'POST',
            header: {
              'cookie': wx.getStorageSync("sessionid")
            },
            success: res => {
              if (res.statusCode == 200) {
                if (res.data.d.Code == 'Success') {
                  var paid = res.data.d.Data.Paid;
                  that.setData({
                    Paid: paid,
                  });
                }
              }
            }
          });
        } else if (res.errMsg == 'requestPayment:fail cancel') {
          wx.showModal({
            title: '支付失败',
            content: '用户取消支付',
            confirmText: '重新支付',
            showCancel: true,
            cancelText: '稍候支付',
            success(res) {
              if (res.confirm) {
                that.onWxPay();
              }
            }
          });
        } else {
          wx.showModal({
            title: '支付失败',
            content: res.errMsg + '请稍候重试，谢谢！',
            showCancel: false,
          });
        }
      },
    });
  },
  onOrderList: function(res) {
    wx.navigateTo({
      url: '/pages/order/list',
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