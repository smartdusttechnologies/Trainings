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
    const user_input = await driver.findElementById("user_input");
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
