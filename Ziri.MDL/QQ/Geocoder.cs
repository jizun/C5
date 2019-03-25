namespace Ziri.MDL.QQ
{
    //服务Key
    public class QQWSKey
    {
        public string key { get; set; } = "XZ4BZ-5JUCW-Q7BRC-OBU6N-BGGTH-MDBJ5";
    }

    #region 地址解析  https://lbs.qq.com/webservice_v1/guide-geocoder.html

    //解析URL
    public class GeocoderWSUrl
    {
        public string url { get; set; } = "https://apis.map.qq.com/ws/geocoder/v1/?key={0}&address={1}";
    }

    //解析结果
    public class Geocoder
    {
        //状态码状态码，
        //0为正常
        //310请求参数信息有误
        //311Key格式错误
        //306请求有护持信息请检查字符串
        //110请求来源未被授权
        public int status { get; set; }
        //状态说明
        public string message { get; set; }
        //地址解析结果
        public result result { get; set; }
    }

    //地址解析结果
    public class result
    {
        public string title { get; set; }
        //解析到的坐标
        public location location { get; set; }
        //行政区划信息
        public ad_info ad_info { get; set; }
        //解析后的地址部件
        public address_components address_components { get; set; }
        //即将下线，由reliability代替
        public decimal similarity { get; set; }
        //即将下线，由level代替
        public int deviation { get; set; }
        //可信度参考：值范围 1 <低可信> - 10 <高可信>
        //我们根据用户输入地址的准确程度，在解析过程中，将解析结果的可信度(质量)，
        //由低到高，分为1 - 10级，该值>=7时，解析结果较为准确，<7时，会存各类不可靠因素，
        //开发者可根据自己的实际使用场景，对于解析质量的实际要求，进行参考。
        public int reliability { get; set; }
        //解析精度级别，分为11个级别，一般>=9即可采用（定位到点，精度较高） 
        //也可根据实际业务需求自行调整，完整取值表见下文。
        //1     城市
        //2	    区、县
        //3	    乡镇、街道
        //4	    村、社区
        //5	    开发区
        //6	    热点区域、商圈
        //7	    道路
        //8	    道路附属点：交叉口、收费站、出入口等
        //9	    门址
        //10	小区、大厦
        //11	POI点
        public int level { get; set; }
    }

    //解析到的坐标
    public class location
    {
        //纬度
        public decimal lng { get; set; }
        //经度
        public decimal lat { get; set; }
    }

    //行政区划信息
    public class ad_info
    {
        //行政区划代码
        public string adcode { get; set; }
    }

    //解析后的地址部件
    public class address_components
    {
        //省
        public string province { get; set; }
        //市
        public string city { get; set; }
        //区，可能为空字串
        public string district { get; set; }
        //街道，可能为空字串
        public string street { get; set; }
        //门牌，可能为空字串
        public string street_number { get; set; }
    }
    #endregion
}
