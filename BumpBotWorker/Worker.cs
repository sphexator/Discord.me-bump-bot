using System;
using System.Threading;
using System.Threading.Tasks;
using _2CaptchaAPI;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium.Chrome;
using Quartz;

namespace BumpBotWorker
{
    public class Worker : BackgroundService, IJob
    {
        private readonly ILogger<Worker> _logger;
        private readonly ChromeDriver _chrome;
        private readonly _2Captcha _captcha;

        public Worker(ILogger<Worker> logger, ChromeDriver chrome, _2Captcha captcha)
        {
            _logger = logger;
            _chrome = chrome;
            _captcha = captcha;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(-1, stoppingToken);
            }
        }

        public Task Execute(IJobExecutionContext context)
        {
            var _ = ExecuteAsync();
            return Task.CompletedTask;
        }

        private async Task ExecuteAsync()
        {

        }
    }
}