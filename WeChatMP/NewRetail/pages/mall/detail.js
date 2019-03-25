const app = getApp();

Page({
  data: {
    GoodsDetail: null,
    OrderInfo: {
      BillID: null,
      BillNO: null,
      SpecSelected: null,
      Counter: null,
      Quantity: 1,
      Amount: 0.00,
    },
  },
  //页面加载
  onLoad: function(options) {
    wx.showLoading({
      title: '加载中',
    });
    this.getGoodsList(options.goodsid);
  },
  //获取商品详情
  getGoodsList: function(goodsid) {
    var that = this;
    wx.request({
      url: app.globalData.apiBaseUrl + 'MOD/OMS/WeChat.asmx/GetMallGoodsDetail',
      data: {
        goods_id: goodsid
      },
      method: 'POST',
      header: {
        'cookie': wx.getStorageSync("sessionid")
      },
      success: res => {
        if (res.statusCode == 200) {
          if (res.data.d.Code == 'Success') {
            //商品信息 - 补充图片URL
            var goodsDetails = res.data.d.Data;
            for (var j = 0; j < goodsDetails.Photos.length; j++) {
              goodsDetails.Photos[j] = app.globalData.apiBaseUrl + 'doc/upload/' +
                goodsDetails.Photos[j];
            }
            if (goodsDetails.Description != null) {
              var reg = new RegExp('src="/DOC', 'g');
              goodsDetails.Description = goodsDetails.Description.replace(reg, 'src="' +
                app.globalData.apiBaseUrl + 'DOC');
            }
            that.setData({
              GoodsDetail: goodsDetails,
            });
            //订单信息
            var orderInfo = this.data.OrderInfo;
            var specSelected = [];
            var specValueIDs = '';
            for (var j = 0; j < goodsDetails.SpecInfos.length; j++) {
              var specValue = {
                spec: goodsDetails.SpecInfos[j],
                values: [],
                valueid: 0,
              };
              for (var k = 0; k < goodsDetails.SpecValues.length; k++) {
                if (goodsDetails.SpecValues[k].SpecID == specValue.spec.ID) {
                  specValue.values.push(goodsDetails.SpecValues[k]);
                  if (specValue.valueid == 0) {
                    specValue.valueid = goodsDetails.SpecValues[k].ID;
                    specValueIDs += (specValueIDs.length == 0 ? '' : ',') + goodsDetails.SpecValues[k].ID;
                  }
                }
              }
              specSelected.push(specValue);
            }
            orderInfo.SpecSelected = specSelected;
            if (specSelected.length == 0) {
              orderInfo.Counter = goodsDetails.GoodsCounter;
            } else {
              for (var j = 0; j < goodsDetails.GoodsSpecsFull.length; j++) {
                if (specValueIDs == goodsDetails.GoodsSpecsFull[j].SpecValueIDs) {
                  orderInfo.Counter = goodsDetails.GoodsSpecsFull[j];
                  break;
                }
              }
            }
            this.OrderInfoChange(orderInfo);
          } else {
            wx.showModal({
              title: '获取商品详情错误',
              content: '服务器错误' + res.data.d.Message,
              showCancel: false,
            });
          }
        } else {
          wx.showModal({
            title: '获取商品详情错误',
            content: '服务器错误码' + res.statusCode,
            showCancel: false,
          });
        }
      }
    });
  },
  //订单规格变更时
  onSpecSelected: function(event) {
    var orderInfo = this.data.OrderInfo;
    var specValueIDs = '';
    for (var i = 0; i < orderInfo.SpecSelected.length; i++) {
      if (orderInfo.SpecSelected[i].spec.ID == event.currentTarget.dataset.specid) {
        orderInfo.SpecSelected[i].valueid = event.currentTarget.dataset.specvalueid;
      }
      specValueIDs += (specValueIDs.length == 0 ? '' : ',') + orderInfo.SpecSelected[i].valueid;
    }
    var goodsDetails = this.data.GoodsDetail;
    for (var j = 0; j < goodsDetails.GoodsSpecsFull.length; j++) {
      if (specValueIDs == goodsDetails.GoodsSpecsFull[j].SpecValueIDs) {
        orderInfo.Counter = goodsDetails.GoodsSpecsFull[j];
        break;
      }
    }
    this.OrderInfoChange(orderInfo);
  },
  //订单数量变更时
  onOrderQtyChange: function(event) {
    var orderInfo = this.data.OrderInfo;
    switch (event.currentTarget.dataset.action) {
      case 'Add':
        orderInfo.Quantity += 1;
        break;
      case 'Reduce':
        orderInfo.Quantity -= 1;
        break;
      case 'Input':
        orderInfo.Quantity = event.detail.value;
        break;
    }
    this.OrderInfoChange(orderInfo);
  },
  //订单信息变更
  OrderInfoChange: function(orderInfo) {
    //检查单价、数量
    if (orderInfo.Quantity < 1) {
      orderInfo.Quantity = 1;
    }
    if (orderInfo.Quantity > orderInfo.Counter.Quantity) {
      wx.showModal({
        title: '库存限制',
        content: '订单数量不能超出库存数量' + orderInfo.Counter.Quantity,
        showCancel: false,
      });
      orderInfo.Quantity = orderInfo.Counter.Quantity
    }
    //计算金额
    if (orderInfo.Price <= 0) {
      orderInfo.Price = 0.00;
    }
    orderInfo.Amount = orderInfo.Counter.Price > 0 && orderInfo.Quantity > 0 ?
      orderInfo.Counter.Price * orderInfo.Quantity :
      0.00;
    //设置页面对象
    this.setData({
      OrderInfo: orderInfo,
    });
    wx.hideLoading();
  },
  //购买按钮
  onBuy: function() {
    var orderInfo = this.data.OrderInfo;
    orderInfo.BillID = null;
    orderInfo.BillNO = null;
    this.setData({
      OrderInfo: orderInfo,
    });
    wx.navigateTo({
      url: '/pages/order/order',
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