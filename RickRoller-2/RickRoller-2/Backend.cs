using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;
using System.IO;

namespace RickRoller_2
{
    public class Backend
    {
        private static string url = "https://www.facebook.com/";
        static private IWebDriver driver;
        public Backend()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddUserProfilePreference("profile.default_content_setting_values.notifications", 2);
            var chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.HideCommandPromptWindow = true;
            driver = new ChromeDriver(chromeDriverService ,options);
        }
        public string[] getFriendsList()
        {
            IWebElement profile = driver.FindElement(By.XPath("//*[@title=\"Profil\"]"));
            driver.Navigate().GoToUrl(profile.GetAttribute("href"));
            IWebElement friends = driver.FindElement(By.XPath("//*[@data-tab-key=\"friends\"]"));
            driver.Navigate().GoToUrl(friends.GetAttribute("href"));
            IReadOnlyCollection<IWebElement> linksBefore;
            int size;
            while (true)
            {
                linksBefore = driver.FindElements(By.XPath("//div[@class=\"fsl fwb fcb\"]/a"));
                IWebElement last = linksBefore.ElementAt(linksBefore.Count - 1);
                int y = last.Location.Y;
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                js.ExecuteScript("window.scrollTo(0," + y + ")");
                Thread.Sleep(2000);
                IReadOnlyCollection<IWebElement> linksAfter = driver.FindElements(By.XPath("//div[@class=\"fsl fwb fcb\"]/a"));
                size = linksAfter.Count;
                if (linksBefore.Count == linksAfter.Count)
                    break;
            }
            string[] names = new string[size];
            int i = 0;
            foreach (IWebElement webElement in linksBefore)
            {
                Console.WriteLine(webElement.Text);
                names[i] = webElement.Text;
                i++;
            }
            driver.Navigate().GoToUrl(url);
            return names;
        }

        public void rickRoll(String name, String path)
        {
            IWebElement firstSearcher = driver.FindElement(By.XPath("//*[@title=\"Messenger\"]"));
            driver.Navigate().GoToUrl(firstSearcher.GetAttribute("href"));
            IWebElement secondSearcher = driver.FindElement(By.XPath("//*[@aria-label=\"Wyszukaj w Messengerze\"]"));
            secondSearcher.Click();
            secondSearcher.SendKeys(name);
            Thread.Sleep(2000);
            IWebElement firstClicker = driver.FindElement(By.XPath("//*[text()=\"Kontakty\"]"));
            IWebElement secondClicker = firstClicker.FindElement(By.XPath("//*[text()=" + '"' + name + '"' + "]"));
            secondClicker.Click();
            Thread.Sleep(2000);
            IWebElement field = driver.FindElement(By.XPath("//*[@aria-label=\"Wpisz wiadomość...\"]"));
            field.Click();
            ArrayList script = songReader(path);
            foreach (var line in script)
            {
                field.SendKeys(line.ToString());
                field.SendKeys(Keys.Enter);
                Thread.Sleep(1000);
            }
            driver.Navigate().GoToUrl(url);

        }

        public void login(string login, string password)
        {
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(url);
            IWebElement loginField = driver.FindElement(By.XPath("//*[@id=\"email\"]"));
            loginField.SendKeys(login);
            IWebElement passwordField = driver.FindElement(By.XPath("//*[@id=\"pass\"]"));
            passwordField.SendKeys(password);
            passwordField.Submit();
            driver.FindElement(By.XPath(("//*[text()=\"Strona główna\"]")));
        }

        public ArrayList songReader(string path)
        {            
            ArrayList list = new ArrayList();
            const Int32 BufferSize = 128;
            using (var fileStream = File.OpenRead(path))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                String line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    list.Add(line);
                }
            }
            return list;           
        }

        public void killBrowser()
        {
            driver.Quit();
        }
    }
}
