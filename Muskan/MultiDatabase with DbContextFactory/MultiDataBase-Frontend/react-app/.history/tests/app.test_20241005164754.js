const { Builder, By, until } = require("selenium-webdriver");
const chrome = require("selenium-webdriver/chrome");

async function appTest() {
  const options = new chrome.Options();
  options.addArguments("--headless");

  const driver = await new Builder()
    .forBrowser("chrome")
    .setChromeOptions(options)
    .build();

  try {
    await driver.get("http://localhost:3000");
    console.log("Website loaded successfully!");

    // Wait for the inputs to be present
    await driver.wait(until.elementLocated(By.id("user_input")), 10000);
    const userInput = await driver.findElement(By.id("user_input"));
    const addressInput = await driver.findElement(By.id("address_input"));
    const designationInput = await driver.findElement(
      By.id("designation_input")
    );
    const salaryInput = await driver.findElement(By.id("salary_input"));

    // Check that form fields are accessible
    await userInput.sendKeys("John Doe");
    await addressInput.sendKeys("123 Main St");
    await designationInput.sendKeys("Software Engineer");
    await salaryInput.sendKeys("100000");

    // Optionally, you can log the input values for debugging
    console.log(
      "User input value is: " + (await userInput.getAttribute("value"))
    );
    console.log(
      "Address input value is: " + (await addressInput.getAttribute("value"))
    );

    // Submit the form
    const submitButton = await driver.findElement(By.id("button_submit"));
    await submitButton.click();

    console.log("Form submitted successfully!");
  } catch (error) {
    console.error("An error occurred during the test:", error);
  } finally {
    await driver.quit();
  }
}

// Run the test
appTest();
