const options = new chrome.Options();
options.addArguments("--headless"); // Run headless Chrome
const driver = await new Builder()
  .forBrowser("chrome")
  .setChromeOptions(options)
  .build();
