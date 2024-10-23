using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
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
                driver.FindElement(By.Id("Name")).Click();
                driver.FindElement(By.Id("Name")).SendKeys("ghjh");
                driver.FindElement(By.Id("Address")).Click();
                driver.FindElement(By.Id("Address")).SendKeys("jhgjgh");
                driver.FindElement(By.Id("Email")).Click();
                driver.FindElement(By.Id("Email")).SendKeys("User5@gmail.com");
                driver.FindElement(By.Id("Phone")).Click();
                driver.FindElement(By.Id("Phone")).SendKeys("6546546");
                driver.FindElement(By.CssSelector(".btn-primary")).Click();
            }
        }
    }
