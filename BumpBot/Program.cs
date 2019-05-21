using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using BumpBot.Entities;
using BumpBot.Extensions;
using BumpBot.Utility;
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
            


            await Task.Delay(-1);
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

        private IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            services.UseQuartz(typeof(Bump));
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