using System;
using System.Web;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Ziri.MDL;
using Ziri.MDL.QQ;

namespace Ziri.BLL.QQ
{
    public class WS
    {
        public static Geocoder GetQQWSGeoCode(string address, out AlertMessage alertMessage)
        {
            alertMessage = null;
            var url = string.Format(new GeocoderWSUrl().url, new QQWSKey().key, HttpUtility.UrlEncode(address, Encoding.UTF8));
            using (WebClient webClient = new WebClient())
            {
                try { return JsonConvert.DeserializeObject<Geocoder>(Encoding.UTF8.GetString(webClient.DownloadData(url))); }
                catch (Exception ex)
                {
                    alertMessage = new AlertMessage
                    {
                        Type = AlertType.error,
                        Message = HttpUtility.HtmlEncode(ex.Message),
                    };
                    return null;
                }
            }
        }
    }
}
