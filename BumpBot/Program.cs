using System;
using System.IO;
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

        private static void Main() => new Program().BumpBot().GetAwaiter().GetResult();

        private async Task BumpBot()
        {
            _config = BuildConfig();
            _web = new ChromeDriver(@"Componements\");
            _client = new TwoCaptchaClient(_config["apiKey"]);

            _web.WebDriverSettings();

            await Task.Delay(TimeSpan.FromSeconds(15));

            var services = ConfigureServices();

            await _web.Login(_config);
            while (true)
            {
                if (_web.Url != "https://discord.me/dashboard") await _web.GoToPage("https://discord.me/dashboard");
                var bump = _web.FindElement(By.XPath("/html/body/div/div[1]/div[4]/div[1]/div/span/span"));

                await Task.Delay(200);

                if (bump.Text != "Available Now!")
                {
                    Console.WriteLine("no bump available");
                    continue;
                }

                _web.FindElement(By.XPath("/html/body/div/div[1]/div[4]/div[1]/div/div[4]/div[1]/span/a"))
                    .Click();

                await Task.Delay(500);

                var googleKey = _web.FindElement(By.Id("recaptcha")).GetAttribute("data-sitekey");
                var result = await _client.SolveCaptcha(googleKey, _web.PageSource, "username:password@ip:port",
                    ProxyType.Http);
                if (result == null) continue;
                _web.FindElement(By.Id("g-recaptcha-response")).SendKeys(result);
                await Task.Delay(500);
                _web.FindElement(By.XPath("//*[@id=\"servers\"]/div[3]/button")).Click();
                await Task.Delay(TimeSpan.FromSeconds(15));
            }
        }

        private IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddSingleton(_config);
            services.AddSingleton(_client);
            services.AddSingleton(_web);

            return services.BuildServiceProvider();
        }

        private static IConfiguration BuildConfig()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json")
                .Build();
        }
    }
}