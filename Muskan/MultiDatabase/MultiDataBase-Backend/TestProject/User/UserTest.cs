using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using TestProject.DbContexts;
using Xunit;

namespace TestProject.Views
{
    public class UserTest : IDisposable
    {
        private readonly UserTestDbContext _ucontext;
        public IWebDriver driver { get; private set; }

        public UserTest()
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");
            Environment.SetEnvironmentVariable("USE_IN_MEMORY_DB", "true");

            var options = new DbContextOptionsBuilder<UserTestDbContext>()
               .UseInMemoryDatabase(databaseName: "TestDb")
               .Options;

            _ucontext = new UserTestDbContext(options);

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
            driver.FindElement(By.Id("Phone")).SendKeys("7894561236");
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            var button = wait.Until(d =>
            {
                var btn = d.FindElement(By.ClassName("btn"));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", btn);
                return btn.Displayed && btn.Enabled ? btn : null;
            });

            button.Click();
            Thread.Sleep(3000);
            var user = _ucontext.Users.FirstOrDefaultAsync(e => e.Name == "ghjh");
            Assert.NotNull(user);
          


            driver.Close();
        }

    }
}
