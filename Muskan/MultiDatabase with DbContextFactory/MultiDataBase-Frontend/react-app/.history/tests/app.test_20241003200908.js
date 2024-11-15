const { Builder, Browser } = require("selenium-webdriver");
const chrome = require("selenium-webdriver/chrome");
async function appTest() {
  const options = new chrome.Options(); //It is instance of chrome options
  options.addArguments("--headless"); //it is used for operate browser without gui
  const driver = await new Builder() //control the Chrome browser
    .forBrowser("chrome")
    .setChromeOptions(options)
    .build();
  try {
    await driver.get("https://localhost:3000"); // open the app     console.log("Website loaded successfully!");
    const title = await driver.getTitle();
    console.log("Page title is: " + title);
    // Find element by ID (correct method)
    const userInput = await driver.findElement(By.id("user_input"));

    // Log the element or extract some information (e.g., its value)
    const inputValue = await userInput.getAttribute("value");
    console.log("User input value is: " + inputValue);

    // const result = await driver.findElement(By.id("result"));
    // const text = await result.getText();

    // if (text === "Expected Text") {
    //   console.log("Test Passed");
    // } else {
    //   console.log("Test Failed");
    // }
  } finally {
    await driver.quit();
  }
}
appTest();
