using System.Threading.Tasks;
using BumpBotWorker.Resources;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace BumpBotWorker.Extensions
{
    public static class WebDriverExtension
    {
        public static IWebDriver ChromeDriverSettings(this ChromeDriver driver)
        {
            driver.Url = Constant.DefaultUrl;
            return driver;
        }

        public static bool CheckUrl(this IWebDriver web, out LoginType type)
        {
            type = LoginType.None;
            if (web.Url == Constant.DefaultUrl) return true;

            if (web.Url.StartsWith(Constant.Oauth2Url))
            {
                type = LoginType.Oauth;
                return false;
            }

            if (!web.Url.StartsWith(Constant.LoginUrl)) return false;
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