using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using MultiDatabase.Data;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using TestProject.DbContexts;
using Xunit;

namespace TestProject.Views
{
    public class UserTest : IDisposable
    {
        private readonly Application2DbContext _ucontext;
        public IWebDriver Driver { get; private set; }
        public IDictionary<string, object> vars { get; private set; }
        public IJavaScriptExecutor js { get; private set; }

        public UserTest()
        {
          
            var options = new DbContextOptionsBuilder<Application2DbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            _ucontext = new Application2DbContext(options);
            Driver = new ChromeDriver();
            js = (IJavaScriptExecutor)Driver;
            vars = new Dictionary<string, object>();
        }

        public void Dispose()
        {
            Driver.Quit();
          
            if (_ucontext != null)
            {
                _ucontext.Database.EnsureDeleted();
                _ucontext.Dispose(); 
            }
        }

        [Fact]
        public void AddUser()
        {
            // Arrange
            Driver.Navigate().GoToUrl("https://localhost:7122/");
            Driver.Manage().Window.Size = new System.Drawing.Size(1280, 680);
            Driver.FindElement(By.LinkText("User")).Click();
            Driver.FindElement(By.LinkText("Create")).Click();

            // Act
            Driver.FindElement(By.Id("Name")).SendKeys("test");
            Driver.FindElement(By.Id("Address")).SendKeys("jhgjgh");
            Driver.FindElement(By.Id("Email")).SendKeys("User5@gmail.com");

            IWebElement FileUpload = Driver.FindElement(By.XPath("//*[@id='File']"));
            FileUpload.SendKeys(@"C:\Users\raj\Downloads\1.jpeg");
            Driver.FindElement(By.Id("Phone")).SendKeys("7894561236");

            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            var button = wait.Until(d =>
            {
                var btn = d.FindElement(By.ClassName("btn"));
                ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView(true);", btn);
                return btn.Displayed && btn.Enabled ? btn : null;
            });

            button.Click();
            Thread.Sleep(3000);
        }
    }
}
