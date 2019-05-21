using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace BumpBot.Entities
{
    public class TwoCaptchaClient
    {
        private string ApiKey { get; }

        public TwoCaptchaClient(string apikey)
        {
            ApiKey = apikey;
        }

        public async Task<string> SolveCaptcha(string googleKey, string pageUrl, string proxy, ProxyType proxyType)
        {
            var requestUrl = "http://2captcha.com/in.php?key=" + ApiKey + "&method=userrecaptcha&googlekey=" +
                             googleKey + "&pageurl=" + pageUrl + "&proxy=" + proxy + "&proxytype=";

            switch (proxyType)
            {
                case ProxyType.Http:
                    requestUrl += "HTTP";
                    break;
                case ProxyType.Https:
                    requestUrl += "HTTPS";
                    break;
                case ProxyType.Socks4:
                    requestUrl += "SOCKS4";
                    break;
                case ProxyType.Socks5:
                    requestUrl += "SOCKS5";
                    break;
            }

            var req = WebRequest.Create(requestUrl);
            using (var resp = await req.GetResponseAsync())
            using (var read = new StreamReader(resp.GetResponseStream() ?? throw new InvalidOperationException()))
            {
                var response = await read.ReadToEndAsync();
                if (response.Length < 3) return null;
                if (response.Substring(0, 3) != "OK|") return null;
                var captchaId = response.Remove(0, 3);
                for (var i = 0; i < 24; i++)
                {
                    var getAnswer = WebRequest.Create("http://2captcha.com/res.php?key=" + ApiKey + "&action=get&id=" + captchaId);
                    using (var answerResp = await getAnswer.GetResponseAsync())
                    using (var answerStream = new StreamReader(answerResp.GetResponseStream() ?? throw new InvalidOperationException()))
                    {
                        var answerResponse = await answerStream.ReadToEndAsync();
                        if (answerResponse.Length < 3) return null;
                        if (answerResponse.Substring(0, 3) == "OK|") return answerResponse.Remove(0, 3);
                        if (answerResponse != "CAPCHA_NOT_READY") return null;
                    }
                    await Task.Delay(5000);
                }
                return null;
            }
        }
    }

    public enum ProxyType
    {
        Http,
        Https,
        Socks4,
        Socks5
    }
}