using System;
using System.Threading.Tasks;

namespace TwoCaptcha
{
    public class TwoCaptchaClient
    {
        private string _token { get; set; }
        public TwoCaptchaClient(string token) => _token = token;

        public async Task StartAsync()
        {

        }
    }
}
