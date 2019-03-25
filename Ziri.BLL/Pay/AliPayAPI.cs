using System.Collections.Generic;
using System.Text;
using Aop.Api;
using Aop.Api.Request;
using Aop.Api.Response;
using Aop.Api.Util;


public class AliPayAPI
{
    private AliPayAPI()  {  }

    //开发者公钥
    public static string alipay_public_key = @"MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAhk1yKKEGLHLUB8tlUOj1YYJOUMPGCwFbzavRFi0yugla16s7zhMCVo16gCQBVLWFz/+u6vx+4DKf+xH823iXAzBONavY3WgRbk3qOBM8+Am4AXbbfjtM2nTpAHJcRsxdkNxDCw/y2rEsQgcqkMrP/nmQUlR/zsc4qfPstq9Q4yf5CgzKJTmGPIzqrqTDQOgG8nbIfOvPsoeMWXOFM9/0DI5RCgAEkASFzdU9Pa0/6Wn7pAUbUgtGegeG4OkdH2GjIpGPbR631tplu1Re4xoSNpeBD/G4nyJfq9GQpH4oL4jiuuz5Zwwr2MmZtz4CnLkjeq5fJO41UTTnoQYRzMUATwIDAQAB";

    //开发者私钥
    public static string merchant_private_key = @"MIIEpAIBAAKCAQEAu2nfWrOFoPWeTvT5TvqQ+n6fy3XX+vesyPh33tXDGX5/jl6HYZcdZHw9rUVPLgoWuheVjHc2KNifi0Ucw3m5IwPbj8Qz8gUE6PvxVQhGds9L0m8IzVKYofQJYnb0v8YtmVmK4snpH5ZFWxsKVHEJMem7FQSqaMVg5K14gffC7SQsMCUdNYopdl9powKR2u0B5voZFeEOzr9TSzQJAI5n9BLwVRX6nBMDpHypfr9GZyBrfA76R2QQk9ZeFWp9t/jPLCFRthcngANub2yIrcoRMunOiY6/xn+dWaqp7nvOAwiRTDqSl/y7Yq30gyNlJ8k/DNCLZGkfPFuDqrEYSGOHgwIDAQABAoIBAQCbi6Z2CBN7YP/QNmAI3cugK6ICYr8sK437isUqbC4uJaBGkEw6ggpCER+7hI/P/U2Zi+NyHqsTwZ7BH05dKtSurA/5xXT7hJDrsdc9pM2e5DPg5ojeqJPHrtxvHZ3K4PeNlBTCX6QdtEHB+LJdHSfthjjgm+u3Y0Ik6Zjf/buJxxRWyYRPSreTnqIg2vPhTrmUlEwRiOFAU6nufT5rrHQJm/NnTiUMfJXW6JotD4459O9cwdKnRcGLU+6oHnxDikbUXmFPif2s2vNcPjQY4205SCjZSYoXQHHkggJCSq6r4JoDJaDD5Hd7AEmXs7uHvvPOV6Gqj73IVXppleJTYQ4RAoGBAOu+Offqg1yiU6pyvf8C6Hx5OZ/X0HTMpfxSiuQn5a6ciNLC3rgwqsHfLc97s2V2J2xa/wnDaaonXoqelWlT5Ng+k2s9VjCEw3h7FI4PKAARSjQOR7qi9t93dTyp+2hiOVJc0XdLR0EyqHgZq6C0PK/h6pXTUtohXYM5ho4arxdLAoGBAMuEg7mP/ZhHXjNzW8S3nYeXnxBb8R7zjmv8Lisk3MdzoyY2CDFiQT5JHad/rh/ECzMljlvKQhhKeIqfI1ZbYLr4DbE938qP9TJ6ny/BZvK8+5m9k22ZrIPbiQ2Ecum+/bCivlsNILtQUrnfwlmaOFHm3l9SFE5Dj5a144/HLBWpAoGAG4V6uQGs0ky1HcoFelb5k0aHvqxwTqJxolJ0mow52Te3FXvginpMBRQsAfP8DpVLpu+8pIQmutGbzO0UnlZH+iLcDQH9JO7q7w7XaHpGfjOGiSs9noFV9uK4Uhu6GQaWkJH7dkTYjbj/R/4fNkS0bQgf3dvs6UBJ7X7ywXZGCJ8CgYEAmZ7MQ3yCArkUTSfthD7JRPlZEuMjzaYwwigXvW9Uy6To6MXuR0CQWPCd1rpU+BtmDNRzcavpWiaUeKaI6P2OtXLZ+wjmbSNfBRx0HnsRN3cZP8JOj3NpOhqziCWLDA+jtPoTT+1H4LxPxYg2TqZPjtsCzdbKBw2OTEe9nuDL0QkCgYBG8kQWQDpIuwDdQkD2sL+kkwI6XDmTuFD4HrE6y8iIybjM+MTK8O1xfsSfMF/H/j3HEcyFsBzFialnxkZ9C4irGpuRVoK8KG7K65Y36B5NpVhb0eyVWJBIucaorP1rCiBK989SHnxAJzdq1fyi0dc84i2Rh86dHCrJuRw07uWo6A==";

    //开发者公钥
    public static string merchant_public_key = @"MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAhk1yKKEGLHLUB8tlUOj1YYJOUMPGCwFbzavRFi0yugla16s7zhMCVo16gCQBVLWFz/+u6vx+4DKf+xH823iXAzBONavY3WgRbk3qOBM8+Am4AXbbfjtM2nTpAHJcRsxdkNxDCw/y2rEsQgcqkMrP/nmQUlR/zsc4qfPstq9Q4yf5CgzKJTmGPIzqrqTDQOgG8nbIfOvPsoeMWXOFM9/0DI5RCgAEkASFzdU9Pa0/6Wn7pAUbUgtGegeG4OkdH2GjIpGPbR631tplu1Re4xoSNpeBD/G4nyJfq9GQpH4oL4jiuuz5Zwwr2MmZtz4CnLkjeq5fJO41UTTnoQYRzMUATwIDAQAB";

    //应用ID
    public static string appId = "2018031302364975";

    //合作伙伴ID：partnerID
    public static string Pid = "2088821899720545";


    //支付宝网关
    public static string serverUrl = "https://openapi.alipay.com/gateway.do";
    public static string mapiUrl = "https://mapi.alipay.com/gateway.do";
    public static string monitorUrl = "http://mcloudmonitor.com/gateway.do";

    //编码
    public static string charset = "utf-8";
    //签名类型
    public static string sign_type = "RSA2";
    //版本号
    public static string version = "1.0";
    public static IAopClient GetAlipayClient()
    {
        var client = new DefaultAopClient(serverUrl, appId, merchant_private_key, "json", "1.0", "RSA", alipay_public_key, "utf-8");
        client.return_url = "/return_url.aspx";
        return client;
    }

    public static string com_alipay_account_auth()
    {
        IDictionary<string, string> paramsMap = new Dictionary<string, string>();
        paramsMap.Add("scope", "kuaijie");
        paramsMap.Add("product_id", "APP_FAST_LOGIN");
        paramsMap.Add("pid", Pid);
        paramsMap.Add("apiname", "com.alipay.account.auth");
        paramsMap.Add("auth_type", "AUTHACCOUNT");
        paramsMap.Add("biz_type", "openservice");
        paramsMap.Add("app_id", appId);
        paramsMap.Add("target_id", "RSA");
        paramsMap.Add("app_name", "mc");
        paramsMap.Add("sign_type", "RSA");


        string privateKeyPem = merchant_private_key;
        string sign = AlipaySignature.RSASign(paramsMap, privateKeyPem, null, false, "RSA");
        paramsMap.Add("sign", System.Web.HttpUtility.UrlEncode(sign));

        var str = AlipaySignature.GetSignContent(paramsMap);
        return str;
    }

    public string H5LoginUrl()
    {
        return "https://openauth.alipay.com/oauth2/appToAppAuth.htm?app_id={0}&redirect_uri={1}return_url.aspx";
    }

    public static AlipayUserInfoAuthResponse alipay_user_info_auth(string state = "init")
    {
        var client = GetAlipayClient();
        AlipayUserInfoAuthRequest request = new AlipayUserInfoAuthRequest();
        request.BizContent = "{" +
        "      \"scopes\":[" +
        "        \"auth_base\"" +
        "      ]," +
        "    \"state\":\"" + state + "\"" +
        "  }";

        request.SetNotifyUrl("/return_url.aspx");

        AlipayUserInfoAuthResponse response = client.Execute(request);
        return response;
    }

    public static AlipaySystemOauthTokenResponse alipay_system_oauth_token(string refresh_token)
    {

        var client = GetAlipayClient();
        AlipaySystemOauthTokenRequest request = new AlipaySystemOauthTokenRequest();
        request.GrantType = "refresh_token";
        request.RefreshToken = refresh_token;
        AlipaySystemOauthTokenResponse response = client.Execute(request);
        return response;
    }


    public static AlipaySystemOauthTokenResponse alipay_system_oauth_token_Code(string Code)
    {

        var client = GetAlipayClient();
        AlipaySystemOauthTokenRequest request = new AlipaySystemOauthTokenRequest();
        request.GrantType = "authorization_code";
        request.Code = Code;
        AlipaySystemOauthTokenResponse response = client.Execute(request);
        return response;
    }


    public static AlipayOpenAuthTokenAppResponse alipay_open_auth_token_app(string code)
    {

        var client = GetAlipayClient();
        AlipayOpenAuthTokenAppRequest request = new AlipayOpenAuthTokenAppRequest();
        request.BizContent = "{\"grant_type\":\"authorization_code\",\"code\":\"" + code + "\"}";
        AlipayOpenAuthTokenAppResponse response = client.Execute(request);
        return response;
    }

    public static AlipayUserInfoShareResponse alipay_user_info_share(string accessToken)
    {
        var client = GetAlipayClient();
        AlipayUserInfoShareRequest request = new AlipayUserInfoShareRequest();
        AlipayUserInfoShareResponse response = client.Execute(request, accessToken);
        return response;
    }

    public static AlipayUserUserinfoShareResponse alipay_user_userinfo_share(string accessToken)
    {
        var client = GetAlipayClient();
        AlipayUserUserinfoShareRequest request = new AlipayUserUserinfoShareRequest();
        
        AlipayUserUserinfoShareResponse response = client.Execute(request, accessToken);
        return response;
    }


    public static string alipay_open_auth_token_app1(string code)
    {

        return PostUrl(alipay_open_auth_token_app(code).Body);
    }

    private static string PostDataString(SortedList<string, string> _SortedList)
    {
        string post = string.Empty;

        foreach (string v in _SortedList.Keys)
        {
            post += v + "=" + _SortedList[v] + "&";
        }

        return post;
    }

    private static string PostUrl(string PostData)
    {
        string serverUrl = "http://openapi.alipaydev.com/gateway.do";
        var _WebClient = new System.Net.WebClient();
        byte[] postData = Encoding.UTF8.GetBytes(PostData);
        _WebClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
        return Encoding.Default.GetString(_WebClient.UploadData(serverUrl, "POST", postData));

    }
}