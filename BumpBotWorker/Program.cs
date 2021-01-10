using _2CaptchaAPI;
using BumpBotWorker.Utility;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenQA.Selenium.Chrome;

namespace BumpBotWorker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                    services.UseQuartz(typeof(Worker));
                    services.AddSingleton(new _2Captcha(""));
                    services.AddSingleton(new ChromeDriver("Componements/"));
                });
    }
}
