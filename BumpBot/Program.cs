using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using BumpBot.Entities;
using BumpBot.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace BumpBot
{
    public class Program
    {
        private TwoCaptchaClient _client;
        private IConfiguration _config;
        private IWebDriver _web;
        private IServiceProvider _service;
        private bool _block = false;

        private static void Main() => new Program().BumpBot().GetAwaiter().GetResult();

        private async Task BumpBot()
        {
            _config = BuildConfig();
            _service = ConfigureServices();
            _web = new ChromeDriver(@"Componements\");
            _client = new TwoCaptchaClient(_config["apiKey"]);
            await Task.Delay(TimeSpan.FromSeconds(15));
            while (true)
            {
                if (_block)
                {
                    await Task.Delay(TimeSpan.FromMinutes(1));
                    continue;
                }

                await Task.Delay(TimeSpan.FromSeconds(15));
            }
        }

        private async Task CheckLinkAsync(CancellationToken token)
        {
            while (token.IsCancellationRequested)
            {
                if (!_web.CheckUrl(out var type))
                {
                    if(type == LoginType.Login) {  }
                    if(type == LoginType.Oauth) {  }
                }
                await Task.Delay(TimeSpan.FromMinutes(10), token);
            }
        }

        private async Task BumpAsync()
        {
            var bumpCards = _web.FindElements(By.ClassName("modal-content bump-modal"));
            for (var i = 0; i < bumpCards.Count; i++)
            {
                var x = bumpCards[i];

            }
            var googleKey = _web.FindElement(By.Id("recaptcha")).GetAttribute("data-sitekey");
            var result = await _client.SolveCaptcha(googleKey, _web.PageSource, "username:password@ip:port",
                ProxyType.Http);
            if (result == null) return;
            _web.FindElement(By.Id("g-recaptcha-response")).SendKeys(result);
            await Task.Delay(500);
            _web.FindElement(By.XPath("//*[@id=\"servers\"]/div[3]/button")).Click();
        }

        private IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddSingleton<HttpClient>();
            services.AddSingleton(_config);
            services.AddSingleton(_client);
            services.AddSingleton(_web);

            return services.BuildServiceProvider();
        }

        private IConfiguration BuildConfig()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json")
                .Build();
        }
    }
}