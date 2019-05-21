using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading.Tasks;
using BumpBot.Entities;

namespace BumpBot.Extensions
{
    public static class WebDriverExtension
    {
        private const string DefaultUrl = "https://discord.me/Dashboard";
        private const string ProfilePath = "/html/body/header/div/div/div[3]/nav/ul";

        private const string Oauth2Url = "https://discordapp.com/oauth2";
        private const string LoginUrl = "https://discordapp.com/login";

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

        public static bool CheckUrl(this IWebDriver web, out LoginType type)
        {
            type = LoginType.None;
            if (web.Url == DefaultUrl) return true;

            if (web.Url.StartsWith(Oauth2Url))
            {
                type = LoginType.Oauth;
                return false;
            }

            if (!web.Url.StartsWith(LoginUrl)) return false;
            type = LoginType.Login;
            return false;
        }

        public static async Task GoToPage(this IWebDriver web, string url)
        {
            web.Url = url;
            await Task.Delay(500);
        }
    }
}