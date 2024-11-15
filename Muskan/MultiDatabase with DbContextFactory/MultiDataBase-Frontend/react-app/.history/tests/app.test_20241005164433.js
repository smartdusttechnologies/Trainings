const { Builder, By, until } = require("selenium-webdriver");
const chrome = require("selenium-webdriver/chrome");

async function appTest() {
  const options = new chrome.Options(); // Instance of Chrome options
  options.addArguments("--headless"); // Operate browser without GUI

  // Build Chrome WebDriver with specified options
  const driver = await new Builder()
    .forBrowser("chrome")
    .setChromeOptions(options)
    .build();

  try {
    await driver.get("http://localhost:3000");
    console.log("Website loaded successfully!");

    const title = await driver.getTitle();
    console.log("Page title is: " + title);

    await driver.wait(until.elementLocated(By.id("user_input")), 10000);
    const userInput = await driver.findElement(By.id("user_input"));
    const addressInput = await driver.findElement(By.id("address_input"));
    const designationInput = await driver.findElement(
      By.id("designation_input")
    );
    const salaryInput = await driver.findElement(By.id("salary_input"));

    const inputValue = await userInput.getAttribute("value");
    console.log("User input value is: " + inputValue);
  } catch (error) {
    console.error("An error occurred during the test:", error);
  } finally {
    // Close the browser instance after the test
    await driver.quit();
  }
}

// Run the test
appTest();
