// Generated by Selenium IDE
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
public class EmployeeTest : IDisposable {
  public IWebDriver driver {get; private set;}
  public IDictionary<String, Object> vars {get; private set;}
  public IJavaScriptExecutor js {get; private set;}
  public EmployeeTest()
  {
    driver = new ChromeDriver();
    js = (IJavaScriptExecutor)driver;
    vars = new Dictionary<String, Object>();
  }
  public void Dispose()
  {
    driver.Quit();
  }
  public string waitForWindow(int timeout) {
    try {
      Thread.Sleep(timeout);
    } catch(Exception e) {
      Console.WriteLine("{0} Exception caught.", e);
    }
    var whNow = ((IReadOnlyCollection<object>)driver.WindowHandles).ToList();
    var whThen = ((IReadOnlyCollection<object>)vars["WindowHandles"]).ToList();
    if (whNow.Count > whThen.Count) {
      return whNow.Except(whThen).First().ToString();
    } else {
      return whNow.First().ToString();
    }
  }
  [Fact]
  public void Employee() {
    driver.Navigate().GoToUrl("https://localhost:7122/");
    driver.Manage().Window.Size = new System.Drawing.Size(1280, 680);
    vars["WindowHandles"] = driver.WindowHandles;
    driver.FindElement(By.LinkText("Employee")).Click();
    vars["win5973"] = waitForWindow(2000);
    driver.SwitchTo().Window(vars["win5973"].ToString());
    driver.FindElement(By.Id("user_input")).Click();
    driver.FindElement(By.Id("user_input")).SendKeys("muskan");
    driver.FindElement(By.Id("address_input")).Click();
    driver.FindElement(By.Id("address_input")).SendKeys("neem kuan ");
    driver.FindElement(By.Id("designation_input")).Click();
    driver.FindElement(By.Id("designation_input")).SendKeys("Project manager");
    driver.FindElement(By.Id("salary_input")).Click();
    driver.FindElement(By.Id("salary_input")).SendKeys("34433");
    driver.FindElement(By.Id("button_submit")).Click();
  }
}