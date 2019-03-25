Page({
  data: {
    routeInfo:null
  },
  onLoad: function(options) {
    let that = this;
    that.setData({
      routeInfo: {
        endLat: parseFloat(options.endlat),
        endLng: parseFloat(options.endlng),
        endName: options.title,
        mode: "car"
      }
    })
  }
})