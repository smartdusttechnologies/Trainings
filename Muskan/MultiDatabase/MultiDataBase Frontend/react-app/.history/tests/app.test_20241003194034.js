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
  } finally {
    await driver.quit();
  }
}
