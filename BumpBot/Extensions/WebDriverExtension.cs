using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium.Internal;

namespace BumpBot.Extensions
{
    public static class WebDriverExtension
    {
        private const long Speed = 10000;

        public static IWebDriver ChromeDriverSettings(this ChromeDriver driver)
        {
            //driver.NetworkConditions.DownloadThroughput = Speed;
            //driver.NetworkConditions.UploadThroughput = Speed;
            //driver.NetworkConditions.Latency = TimeSpan.FromMilliseconds(500);
            //driver.NetworkConditions.IsOffline = false;

            driver.Url = "http://discord.me/";
            
            return driver;
        }

        public static IWebDriver WebDriverSettings(this IWebDriver web)
        {
            web.Manage().Timeouts().PageLoad = TimeSpan.FromMinutes(1);
            return web;
        }

        public static async Task<IWebDriver> Login(this IWebDriver web, IConfiguration config)
        {
            web.FindElement(By.XPath("/html/body/div/div[1]/div/div/div/div/form/div[1]/input")).SendKeys(config["username"]);
            web.FindElement(By.XPath("/html/body/div/div[1]/div/div/div/div/form/div[2]/input")).SendKeys(config["password"]);
            await Task.Delay(2000);

            try
            {
                web.FindElement(By.XPath("/html/body/div/div[1]/div/div/div/div/form/div[4]/div")).Click();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                web.FindElement(By.XPath("/html/body/div/div[1]/div/div/div/div/form/div[4]/div/button")).Click();
            }

            return web;
        }

        private static bool CheckLogin(this ISearchContext driver)
        {
            var check = driver.FindElement(By.LinkText("sign in"));
            return check != null;
        }
    }
}
