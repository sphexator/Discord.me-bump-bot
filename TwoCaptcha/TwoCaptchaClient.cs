using System.Net.Http;

namespace TwoCaptcha
{
    public class TwoCaptchaClient
    {
        private readonly string _token;
        private readonly HttpClient _client;

        public TwoCaptchaClient(string token)
        {
            _token = token;
            _client = new HttpClient();
        }

        public TwoCaptchaClient(string token, HttpClient client)
        {
            _token = token;
            _client = client;
        }

        public string SolveCaptcha(string token)
        {

            return null;
        }
    }
}