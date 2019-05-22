using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using _2Captcha;
using BumpBot.Extensions;
using BumpBot.Resources;
using BumpBot.Utility;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenQA.Selenium.Chrome;

namespace BumpBot
{
    public class Program
    {
        private IConfiguration _config;
        private ChromeDriver _web;
        private IServiceProvider _service;
        private TwoCaptcha _captcha;

        private static void Main() => new Program().BumpBot().GetAwaiter().GetResult();

        private async Task BumpBot()
        {
            _config = BuildConfig();
            _service = ConfigureServices();
            _captcha = new TwoCaptcha(_config["apiKey"]);
            _web = new ChromeDriver(@"Componements\");
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
            services.AddSingleton<Bump>();
            services.AddSingleton(_config);
            services.AddSingleton(_web);
            services.AddSingleton(_captcha);
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