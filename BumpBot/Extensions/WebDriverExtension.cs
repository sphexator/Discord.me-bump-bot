using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading.Tasks;

namespace BumpBot.Extensions
{
    public static class WebDriverExtension
    {
        private const string DefaultUrl = "https://discord.me";
        private const string LoginUsernamePath = "/html/body/div/div[1]/div/div/div/div/form/div[1]/input";
        private const string LoginPasswordPath = "/html/body/div/div[1]/div/div/div/div/form/div[2]/input";
        private const string LogInSubmissionPathOne = "/html/body/div/div[1]/div/div/div/div/form/div[4]/div";
        private const string LogInSubmissionPathTwo = "/html/body/div/div[1]/div/div/div/div/form/div[4]/div/button";

        public static IWebDriver ChromeDriverSettings(this ChromeDriver driver)
        {
            driver.Url = DefaultUrl;
            return driver;
        }

        public static void WebDriverSettings(this IWebDriver web)
        {
            web.Manage().Timeouts().PageLoad = TimeSpan.FromMinutes(5);
            web.Url = $"{DefaultUrl}/signin";
        }

        public static async Task<IWebDriver> Login(this IWebDriver web, IConfiguration config)
        {
            web.FindElement(By.XPath(LoginUsernamePath)).SendKeys(config["username"]);
            web.FindElement(By.XPath(LoginPasswordPath)).SendKeys(config["password"]);
            await Task.Delay(2000);

            try
            {
                web.FindElement(By.XPath(LogInSubmissionPathOne)).Click();
            }
            catch
            {
                web.FindElement(By.XPath(LogInSubmissionPathTwo)).Click();
            }

            return web;
        }

        public static async Task GoToPage(this IWebDriver web, string url)
        {
            web.Url = url;
            await Task.Delay(500);
        }
    }
}