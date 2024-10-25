using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Xunit;
using Microsoft.EntityFrameworkCore;
using OpenQA.Selenium.Support.UI;
using TestProject.DbContexts;
using MultiDatabase.Data;
using MultiDatabase.Controllers;

namespace TestProject.Views
{
    public class EmployeeTest : IDisposable
    {
    
        private readonly EmployeeDbContext _context;
        public IWebDriver driver { get; private set; }
        public IDictionary<string, object> vars { get; private set; }
        public IJavaScriptExecutor js { get; private set; }

        public EmployeeTest()
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");
            Environment.SetEnvironmentVariable("USE_IN_MEMORY_DB", "true");

            var options = new DbContextOptionsBuilder<EmployeeDbContext>()
               .UseInMemoryDatabase(databaseName: "TestDb")
               .Options;

            _context = new EmployeeDbContext(options);
          

            driver = new ChromeDriver();
            js = (IJavaScriptExecutor)driver;
            vars = new Dictionary<string, object>();
        }

        public void Dispose()
        {
            driver?.Quit();
            _context?.Database.EnsureDeleted(); 
            _context?.Dispose();
        }

        public string waitForWindow(int timeout)
        {
            try
            {
                Console.WriteLine("Connection with database is done.");
                Thread.Sleep(timeout);
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
            }

            var whNow = driver.WindowHandles.ToList();
            var whThen = ((IReadOnlyCollection<object>)vars["WindowHandles"]).ToList();

            return whNow.Count > whThen.Count
                ? whNow.Except(whThen).First().ToString()
                : whNow.First().ToString();
        }

   
        [Fact]
        public async Task EmployeeAdd()
        {
            driver.Navigate().GoToUrl("https://localhost:7122/");
            driver.Manage().Window.Size = new System.Drawing.Size(1280, 680);
            vars["WindowHandles"] = driver.WindowHandles;

            driver.FindElement(By.LinkText("Employee")).Click();
            vars["win826"] = waitForWindow(2000);
            driver.SwitchTo().Window(vars["win826"].ToString());
            driver.FindElement(By.Id("name-input")).SendKeys("test");
            driver.FindElement(By.Id("address-input")).SendKeys("neem kuan");
            driver.FindElement(By.Id("designation-input")).SendKeys("Test Designation");
            driver.FindElement(By.Id("EmployeeSurname-input")).SendKeys("Test EmployeeSurname ");

            IWebElement FileUpload = driver.FindElement(By.XPath("//*[@id=\"file-input\"]"));
            FileUpload.SendKeys(@"C:\Users\raj\Downloads\1.jpeg");

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            var button = wait.Until(d =>
            {
                var btn = d.FindElement(By.ClassName("btn"));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", btn);
                return btn.Displayed && btn.Enabled ? btn : null;
            });

            button.Click();

            Thread.Sleep(3000);
            //var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Name == "test");
            //Assert.NotNull(employee);
            //Assert.Equal("neem kuan", employee.HomeAddress);
            //Assert.Equal("Test Designation ", employee.Designation);
            //Assert.Equal("Test EmployeeSurname ", employee.EmployeeSurname);
        }
    }

}
