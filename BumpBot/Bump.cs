using System;
using System.Threading.Tasks;
using BumpBot.Entities;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using Quartz;

namespace BumpBot
{
    public class Bump : IJob
    {
        private readonly TwoCaptchaClient _client;
        private readonly IConfiguration _config;
        private readonly IWebDriver _web;

        public Bump(TwoCaptchaClient client, IConfiguration config, IWebDriver web)
        {
            _client = client;
            _config = config;
            _web = web;
        }

        public Task Execute(IJobExecutionContext context) => BumpAsync();

        private async Task BumpAsync()
        {
            var bumpCards = _web.FindElements(By.ClassName("modal-content bump-modal"));
            for (var i = 0; i < bumpCards.Count; i++)
            {
                var x = bumpCards[i];
                var server = x.FindElement(By.Name("serverEid"));
                var id = server.GetAttribute("value");
                var button = x.FindElement(By.ClassName("btn btn-primary"));
                button.Click();
                await Task.Delay(TimeSpan.FromSeconds(5));
            }

            var googleKey = _web.FindElement(By.Id("recaptcha")).GetAttribute("data-sitekey");
            var result = await _client.SolveCaptcha(googleKey, _web.PageSource, "username:password@ip:port",
                ProxyType.Http);
            if (result == null) return;
            _web.FindElement(By.Id("g-recaptcha-response")).SendKeys(result);
            await Task.Delay(500);
            _web.FindElement(By.XPath("//*[@id=\"servers\"]/div[3]/button")).Click();
        }
    }
}
