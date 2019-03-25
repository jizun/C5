App({
  globalData: {
    apiBaseUrl: 'https://www.jixinxj.com/',
    UserInfo: null,
    LocalAuth: false,
    StoreList: null,
    NearbyStore: null,
  },
  onLaunch: function() {
    this.getLocalAuth();
  },
  //获取用户信息
  getUserInfo: function() {
    var that = this;
    return new Promise(function(resolve, reject) {
      wx.login({
        success: function(res) {
          if (res.code) {
            wx.request({
              url: that.globalData.apiBaseUrl + 'MOD/OMS/WeChat.asmx/Login',
              data: {
                js_code: res.code,
              },
              method: 'POST',
              success: res => {
                if (res.statusCode == 200) {
                  if (res.data.d == null) {
                    reject(res.data);
                    return;
                  }
                  wx.removeStorageSync('sessionid')
                  wx.setStorageSync("sessionid", res.header["Set-Cookie"]);
                  if (res.data.d.Code == 'Success') {
                    if (res.data.d.Data.AvatarUrl == null) {
                      res.data.d.Data.AvatarUrl = that.globalData.apiBaseUrl +
                        '/media/users/avatar_default.png';
                    };
                    that.globalData.UserInfo = res.data.d.Data;
                    resolve(res);
                  } else {
                    reject(res.data.d.Message);
                    return;
                  }
                } else {
                  reject('服务器错误码' + res.statusCode);
                  return;
                }
              }
            });
          } else {
            reject('静默登录码为空');
            return;
          }
        },
        fail: function() {
          reject('静默登录失败');
          return;
        },
      });
    });
  },
  //注册新用户
  setUserInfo: function(event) {
    var that = this;
    return new Promise(function(resolve, reject) {
      if (event == null || event.detail == null || event.detail.userInfo == null) {
        reject('用户拒绝使用微信用户信息');
        return;
      }
      wx.login({
        success: res => {
          wx.request({
            url: that.globalData.apiBaseUrl + 'MOD/OMS/WeChat.asmx/SetUserInfo',
            data: {
              js_code: res.code,
              rawData: event.detail.rawData,
            },
            method: 'POST',
            header: {
              'cookie': wx.getStorageSync("sessionid")
            },
            success: res => {
              if (res.statusCode == 200) {
                if (res.data.d == null) {
                  reject(res.data);
                  return;
                }
                if (res.data.d.Code == 'Success') {
                  that.globalData.UserInfo = res.data.d.Data;
                  resolve(res);
                } else {
                  reject(res.data.d.Message);
                  return;
                }
              } else {
                reject('服务器错误码' + res.statusCode);
                return;
              }
            }
          });
        }
      });
    });
  },
  //获取定位授权
  getLocalAuth: function(event) {
    wx.getSetting({
      success: (res) => {
        if (res.authSetting['scope.userLocation']) {
          this.globalData.LocalAuth = res.authSetting['scope.userLocation'];
        }
      }
    });
  },
  //获取门店列表
  getStoreList: function() {
    var that = this;
    return new Promise(function(resolve, reject) {
      wx.getLocation({
        success: function(res1) {
          if (res1.latitude != null) {
            wx.request({
              url: that.globalData.apiBaseUrl + 'MOD/OMS/WeChat.asmx/GetWxStoreLists',
              data: {
                latitude: res1.latitude,
                longitude: res1.longitude,
              },
              method: 'POST',
              header: {
                'cookie': wx.getStorageSync("sessionid")
              },
              success: res => {
                if (res.statusCode == 200) {
                  if (res.data.d == null) {
                    reject(res.data);
                    return;
                  }
                  if (res.data.d.Code == 'Success') {
                    //补充图片URL
                    if (res.data.d.Data != null) {
                      for (var i = 0; i < res.data.d.Data.length; i++) {
                        res.data.d.Data[i].LogoUrl = that.globalData.apiBaseUrl +
                          'doc/upload/' + res.data.d.Data[i].LogoUrl;
                        var photos = res.data.d.Data[i].Photos;
                        if (photos != null) {
                          for (var j = 0; j < photos.length; j++) {
                            photos[j] = that.globalData.apiBaseUrl +
                              'doc/upload/' + photos[j];
                          }
                        }
                        var description = res.data.d.Data[i].Description;
                        if (description != null) {
                          var reg = new RegExp('src="/DOC', 'g');
                          description = description.replace(reg, 'src="' +
                            that.globalData.apiBaseUrl + 'DOC');
                          res.data.d.Data[i].Description = description;
                        }
                      }
                    };
                    //赋值给APP
                    that.globalData.StoreList = res.data.d.Data;
                    that.globalData.NearbyStore = [{
                      latitude: res.data.d.Data[0].Latitude,
                      longitude: res.data.d.Data[0].Longitude,
                      iconPath: '/media/images/location.png',
                      callout: {
                        content: res.data.d.Data[0].Title,
                        color: '#FF0000',
                        fontSize: 15,
                        borderRadius: 10,
                        display: 'ALWAYS',
                      },
                    }];
                    that.globalData.LocalAuth = true;
                    resolve(res);
                  } else {
                    reject(res.data.d.Message);
                    return;
                  }
                } else {
                  reject('服务器错误码' + res.statusCode);
                  return;
                }
              }
            });
          }
        },
        fail: function() {
          wx.showModal({
            title: '设置权限',
            content: '请授权获取定位',
            confirmText: '授权',
            showCancel: true,
            cancelText: '确定',
            success(res) {
              if (res.confirm) {
                wx.openSetting({
                  success(res) {
                    wx.showModal({
                      title: '设置完成',
                      content: '请重新获取附近的门店',
                      showCancel: false,
                    });
                  }
                })
              }
            }
          });
          reject('未授权获取定位');
        },
      });
    });
  },
})