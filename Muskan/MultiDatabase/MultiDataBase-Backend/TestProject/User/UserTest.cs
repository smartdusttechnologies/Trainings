using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Xunit;

namespace TestProject.Views
{
    public class UserTest : IDisposable
    {
        public IWebDriver driver { get; private set; }

        public UserTest()
        {
            driver = new ChromeDriver();
        }

        public void Dispose()
        {
            driver.Quit();
        }

        [Fact]
        public void User()
        {
            driver.Navigate().GoToUrl("https://localhost:7122/");
            driver.Manage().Window.Size = new System.Drawing.Size(1280, 680);
            driver.FindElement(By.LinkText("User")).Click();
            driver.FindElement(By.LinkText("Create")).Click();            
            driver.FindElement(By.Id("Name")).SendKeys("ghjh");
            driver.FindElement(By.Id("Address")).SendKeys("jhgjgh");
            driver.FindElement(By.Id("Email")).SendKeys("User5@gmail.com");           
            IWebElement FileUpload = driver.FindElement(By.XPath("//*[@id='File']"));
            FileUpload.SendKeys(@"C:\Users\raj\Downloads\1.jpeg");
            driver.FindElement(By.Id("Phone")).SendKeys("6546546");           
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            var button = wait.Until(d =>
            {
                var btn = d.FindElement(By.ClassName("btn"));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", btn);
                return btn.Displayed && btn.Enabled ? btn : null;
            });

            button.Click(); 
            driver.Close();
        }

    }
}
