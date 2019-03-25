// pages/responsive/responsive.js
const QRCode = require('../../utils/qrcode.js')
import rpx2px from '../../utils/rpx2px.js'
let qrcode;

// 300rpx 在6s上为 150px
const qrcodeWidth = rpx2px(300)

Page({
  data: {
    text: '',
    image: '',
    qrcodeWidth: qrcodeWidth,
    imgsrc: ''
  },
  onLoad: function(options) {
    this.setData({
      text: options.sobillno,
    });
    const z = this
    qrcode = new QRCode('canvas', {
      usingIn: this,
      text: options.sobillno,
      image: '',
      width: qrcodeWidth,
      height: qrcodeWidth,
      colorDark: "black",
      colorLight: "white",
      correctLevel: QRCode.CorrectLevel.H,
    });

    // 生成图片，绘制完成后调用回调
    qrcode.makeCode(z.data.text, () => {
      // 回调
      qrcode.exportImage(function(path) {
        z.setData({
          imgsrc: path
        })
      })
    })
  },
  onReady() {},
  confirmHandler: function(e) {
    let {
      value
    } = e.detail
    this.renderCode(value)
  },
  renderCode(value) {
    const z = this
    console.log('make handler')
    qrcode.makeCode(value, () => {
      console.log('make')
      qrcode.exportImage(function(path) {
        console.log(path)
        z.setData({
          imgsrc: path
        })
      })
    })
  },
  inputHandler: function(e) {
    var value = e.detail.value
    this.setData({
      text: value
    })
  },
  tapHandler: function() {
    this.renderCode(this.data.text)
  },
  // 长按保存
  save: function() {
    console.log('save')
    wx.showActionSheet({
      itemList: ['保存图片'],
      success: function(res) {
        console.log(res.tapIndex)
        if (res.tapIndex == 0) {
          qrcode.exportImage(function(path) {
            wx.saveImageToPhotosAlbum({
              filePath: path,
            })
          })
        }
      }
    })
  }
})