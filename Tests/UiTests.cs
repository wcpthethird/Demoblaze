namespace Tests
{
    [TestFixture]
    public class UiTests
    {
        //Declare webdriver
        IWebDriver driver;

        //Holds username and password for login and sign up tests
        private struct UserInfo
        {
            public string Username;
            public string Password;
        }

        //Navigate to demoblaze website
        private void NavigateToDemoblazeWebsite()
        {
            //Define target url
            driver.Url = "https://www.demoblaze.com";
        }

        //Wait for web element to exist
        private void WaitForSomethingToBeDisplayed(IWebElement objectToBeDisplayed)
        {
            //Instantiate wait element
            var wait = new WebDriverWait(driver, new TimeSpan(0, 0, 30));

            //For for object to be displayed
            wait.Until(condition =>
            {
                try
                {
                    //Find and return object upon successful location
                    return objectToBeDisplayed.Displayed;
                }
                catch (StaleElementReferenceException)
                {
                    return false;
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            });
        }

        private static string GenerateRandomUsername(int length)
        {
            Random r = new();
            StringBuilder builder = new();

            //Build random string for username
            for (int i = 0; i < length; i++)
            {
                //Define multiplier for number to add to 65
                double multiplier = r.NextDouble();
                //Define number to add to ASCII value for 'a'
                int alter = Convert.ToInt32(Math.Floor(25 * multiplier));
                //Define letter using ASCII number
                char letter = Convert.ToChar(alter + 65);
                //Add letter to word
                builder.Append(letter);
            }
            //Return random username
            return builder.ToString();
        }

        [SetUp]
        public void Setup()
        {
            //Instantiate webdriver
            driver = new ChromeDriver();
        }

        [Test]
        public void LoginSuccessful()
        {
            //Valid login information
            UserInfo validLogin = new()
            {
                Username = "username",
                Password = "password"
            };

            NavigateToDemoblazeWebsite();

            //Find and click login button
            IWebElement loginButton = driver.FindElement(By.XPath("//*[@id='login2']"));
            loginButton.Click();

            //Find username field
            var loginUsernameField = driver.FindElement(By.Id("loginusername"));

            //Wait for username and password screen to display
            WaitForSomethingToBeDisplayed(loginUsernameField);
            //Make assertion that username field should be displayed
            Assert.That(loginUsernameField.Displayed, Is.True);

            //Find fields for user input in login modal window
            IWebElement loginUsername = driver.FindElement(By.XPath("//*[@id='loginusername']"));
            IWebElement loginPassword = driver.FindElement(By.XPath("//*[@id='loginpassword']"));

            //IWebElement loginSubmit = driver.FindElement(By.XPath("/html/body/div[3]/div/div/div[3]/button[2]"));
            //Locate login button using text search instead of precise xpath
            IWebElement loginSubmit = driver.FindElement(By.XPath("//button[contains(text(),'Log in')]"));

            //Enter username and password and submit
            loginUsername.SendKeys(validLogin.Username);
            loginPassword.SendKeys(validLogin.Password);
            loginSubmit.Click();

            //Find name of user displayed after successful login
            IWebElement nameOfUser = driver.FindElement(By.Id("nameofuser"));
            //Verify that usernames match
            Assert.That(nameOfUser.Text == loginUsername.Text, Is.True);
        }

        [Test]
        public void SignUpUnsuccessfulUsernameExists()
        {
            //Invalid sign up info
            UserInfo invalidSignUp = new()
            {
                Username = "username",
                Password = "password"
            };

            //Navigate to demoblaze website
            NavigateToDemoblazeWebsite();

            //Find and click login button
            IWebElement signUpButton = driver.FindElement(By.XPath("//*[@id='signin2']"));
            signUpButton.Click();

            //Find username field
            var signUpUsernameField = driver.FindElement(By.Id("sign-username"));

            //Wait for username and password screen to display
            WaitForSomethingToBeDisplayed(signUpUsernameField);

            //Make assertion that username field should be displayed
            Assert.That(signUpUsernameField.Displayed, Is.True);

            //Find fields for user input in login modal window
            IWebElement signUpUsername = driver.FindElement(By.XPath("//*[@id='sign-username']"));
            IWebElement signUpPassword = driver.FindElement(By.XPath("//*[@id='sign-password']"));

            IWebElement signUpSubmitButton = driver.FindElement(By.XPath("//button[contains(text(),'Sign up')]"));

            //Enter username and password and submit
            signUpUsername.SendKeys(invalidSignUp.Username);
            signUpPassword.SendKeys(invalidSignUp.Password);
            signUpSubmitButton.Click();

            //Create wait element for alert
            var wait = new WebDriverWait(driver, new TimeSpan(0, 0, 30));
            //Wait until alert window exists
            IAlert alert = wait.Until(ExpectedConditions.AlertIsPresent());

            //Verify that user already exists
            Assert.That(alert.Text.Contains("user already exist"));
            //Close alert
            alert.Accept();
        }

        [Test]
        public void SignUpSuccessful()
        {
            //Invalid sign up info
            UserInfo validSignUp = new()
            {
                //Random username with specific length
                Username = GenerateRandomUsername(8),
                Password = "password"
            };

            //Navigate to demoblaze website
            NavigateToDemoblazeWebsite();

            //Find and click login button
            IWebElement signUpButton = driver.FindElement(By.XPath("//*[@id='signin2']"));
            signUpButton.Click();

            //Find username field
            var signUpUsernameField = driver.FindElement(By.Id("sign-username"));

            //Wait for username and password screen to display
            WaitForSomethingToBeDisplayed(signUpUsernameField);

            //Make assertion that username field should be displayed
            Assert.That(signUpUsernameField.Displayed, Is.True);

            //Find fields for user input in login modal window
            IWebElement signUpUsername = driver.FindElement(By.XPath("//*[@id='sign-username']"));
            IWebElement signUpPassword = driver.FindElement(By.XPath("//*[@id='sign-password']"));

            IWebElement signUpSubmitButton = driver.FindElement(By.XPath("//button[contains(text(),'Sign up')]"));

            //Enter username and password and submit
            signUpUsername.SendKeys(validSignUp.Username);
            signUpPassword.SendKeys(validSignUp.Password);
            signUpSubmitButton.Click();

            //Create wait element for alert
            var wait = new WebDriverWait(driver, new TimeSpan(0, 0, 30));
            //Wait until alert window exists
            IAlert alert = wait.Until(ExpectedConditions.AlertIsPresent());

            //Verify that user already exists
            Assert.That(alert.Text.Contains("success"));
            //Close alert
            alert.Accept();
        }

        [TearDown]
        public void EndTest()
        {
            driver.Close();
        }
    }
}