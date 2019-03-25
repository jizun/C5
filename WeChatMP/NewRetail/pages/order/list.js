const app = getApp();

Page({
  data: {
    Tabs: ['全部', '待付款', '待提货', '待收货'],
    TabIndex: 0,
    StateCount: [0, 0, 0, 0],
    UserCode: null,
    OrderList: null,
    SOPager: {
      ItemCount: 0,
      PageSize: 10,
      PageCount: 0,
      PageIndex: 1,
    }
  },
  //页面加载
  onLoad: function(options) {
    this.getSOList();
  },
  //切换订单组
  onTab: function(event) {
    this.setData({
      TabIndex: event.currentTarget.dataset.tabindex,
    });
    this.getSOList();
  },
  //获取订单列表
  getSOList: function() {
    wx.showLoading({
      title: '加载中',
    });
    if (this.data.UserCode != null) {
      this.setSOList();
      return;
    }
    //静默登录
    var that = this;
    wx.login({
      success: function(res) {
        that.setData({
          UserCode: res.code,
        });
        that.setSOList();
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
  //设置订单列表
  setSOList() {
    var that = this;
    wx.request({
      url: app.globalData.apiBaseUrl + 'MOD/OMS/WeChat.asmx/GetSOList',
      data: {
        user_code: that.data.UserCode,
        state_name: that.data.Tabs[that.data.TabIndex],
        pager: JSON.stringify(that.data.SOPager),
      },
      method: 'POST',
      header: {
        'cookie': wx.getStorageSync("sessionid")
      },
      success: res => {
        if (res.statusCode == 200) {
          if (res.data.d.Code == 'Success') {
            var state_count = that.data.StateCount;
            state_count[0] = res.data.d.Data.StateCount.Total;
            state_count[1] = res.data.d.Data.StateCount.Submit;
            state_count[2] = res.data.d.Data.StateCount.Paid;
            state_count[3] = res.data.d.Data.StateCount.Send;
            var list = res.data.d.Data.list;
            if (list != null) {
              for (var i = 0; i < list.length; i++) {
                for (var j = 0; j < list[i].SOItems.length; j++) {
                  for (var k = 0; k < list[i].SOItems[j].GoodsPhotos.length; k++) {
                    list[i].SOItems[j].GoodsPhotos[k] = app.globalData.apiBaseUrl +
                      'doc/upload/' + list[i].SOItems[j].GoodsPhotos[k];
                  }
                }
              }
            }
            that.setData({
              StateCount: state_count,
              OrderList: list,
              SOPager: res.data.d.Data.pager,
            });
          } else {
            wx.showModal({
              title: '查询错误',
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
        wx.pageScrollTo({
          scrollTop: 0
        })
      }
    });
  },
  //页码切换时
  onPagerChange: function(event) {
    var index = event.currentTarget.dataset.index;
    var pager = this.data.SOPager;
    pager.PageIndex = index < 1 ? 1 : index;
    this.setData({
      SOPager: pager,
    });
    this.getSOList();
  },
  //支付
  onPay: function(event) {
    if (event.currentTarget.dataset.paytypetitle == '到店支付') {
      wx.navigateTo({
        url: '/pages/order/qrcode?sobillno=' + event.currentTarget.dataset.sobillno,
      });
      return;
    }
    if (this.data.UserCode != null) {
      this.setWxPay(event);
      return;
    }
    //静默登录
    var that = this;
    wx.login({
      success: function (res) {
        that.setData({
          UserCode: res.code,
        });
        that.setWxPay(event);
      },
      fail: function (res) {
        wx.showModal({
          title: '微信小程序错误',
          content: '静默登录失败：' + res.errMsg,
          showCancel: false,
        });
        return;
      }
    });
  },
  //设置支付
  setWxPay: function (event){
    var that = this;
    wx.request({
      url: app.globalData.apiBaseUrl + 'MOD/OMS/WeChat.asmx/GetOrderPay',
      data: {
        user_code: that.data.UserCode,
        so_id: event.currentTarget.dataset.soid,
      },
      method: 'POST',
      header: {
        'cookie': wx.getStorageSync("sessionid")
      },
      success: res => {
        if (res.statusCode == 200) {
          if (res.data.d.Code == 'Success') {
            if (res.data.d.Data.Paid) {
              wx.showModal({
                title: '支付成功',
                content: '订单已支付！',
                showCancel: false,
              });
              that.getSOList();
              return;
            }
            that.showWxPay(res.data.d.Data.SOInfo, res.data.d.Data.PayInfo);
          }
        }
      }
    });
  },
  //调起支付
  showWxPay: function(soInfo, payInfo) {
    var that = this;
    if (payInfo == null) {
      wx.showModal({
        title: '支付失败',
        content: '不符合支付条件',
        showCancel: false,
      });
      return;
    }
    payInfo = JSON.parse(payInfo);
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
              order_info: JSON.stringify(soInfo),
            },
            method: 'POST',
            header: {
              'cookie': wx.getStorageSync("sessionid")
            },
            success: res => {
              if (res.statusCode == 200) {
                if (res.data.d.Code == 'Success') {
                  wx.showModal({
                    title: '支付成功',
                    content: '订单已支付！',
                    showCancel: false,
                  });
                  that.getSOList();
                  return;
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
  //取消
  onCancel: function(event) {
    var that = this;
    wx.showModal({
      title: '取消订单',
      content: '确认取消订单吗？',
      confirmText: '确认',
      showCancel: true,
      cancelText: '点错了',
      success(res) {
        if (res.confirm) {
          wx.request({
            url: app.globalData.apiBaseUrl + 'MOD/OMS/WeChat.asmx/SetApplyCancel',
            data: {
              user_code: null,
              so_id: event.currentTarget.dataset.soid,
            },
            method: 'POST',
            header: {
              'cookie': wx.getStorageSync("sessionid")
            },
            success: res => {
              console.log(res);
              wx.showModal({
                title: '取消订单',
                content: res.data.d.Data,
                showCancel: false,
              });
              that.getSOList();
            }
          });
        }
      }
    });
  },
  //收货
  onTaken: function(event) {
    var that = this;
    wx.showModal({
      title: '确认收货',
      content: '您确认收到宝贝了吗？',
      confirmText: '确认',
      showCancel: true,
      cancelText: '点错了',
      success(res) {
        if (res.confirm) {
          wx.request({
            url: app.globalData.apiBaseUrl + 'MOD/OMS/WeChat.asmx/SetTaken',
            data: {
              user_code: null,
              so_id: event.currentTarget.dataset.soid,
            },
            method: 'POST',
            header: {
              'cookie': wx.getStorageSync("sessionid")
            },
            success: res => {
              console.log(res);
              wx.showModal({
                title: '确认收货',
                content: res.data.d.Data,
                showCancel: false,
              });
              that.getSOList();
            }
          });
        }
      }
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