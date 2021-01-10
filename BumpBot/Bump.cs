using System;
using System.Threading.Tasks;
using _2CaptchaAPI;
using BumpBot.Resources;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Quartz;

namespace BumpBot
{
    public class Bump : IJob
    {
        private readonly IConfiguration _config;
        private readonly ChromeDriver _web;
        private readonly _2Captcha _captcha;

        public Bump(IConfiguration config, ChromeDriver web, _2Captcha captcha)
        {
            _config = config;
            _web = web;
            _captcha = captcha;
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
                var googleKey = x.FindElement(By.Id("recaptcha")).GetAttribute("data-sitekey");
                var result = await _captcha.SolveReCaptchaV2(googleKey, Constant.DefaultUrl);
                if (!result.Success) continue;
                
                x.FindElement(By.Id("g-recaptcha-response")).SendKeys(result.Response);
                var button = x.FindElement(By.ClassName("btn btn-primary"));
                button.Click();
                await Task.Delay(TimeSpan.FromSeconds(5));
            }
        }
    }
}