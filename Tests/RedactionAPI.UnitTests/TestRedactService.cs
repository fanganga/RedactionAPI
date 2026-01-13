using Microsoft.Extensions.Configuration;
using RedactionAPI.Services;

namespace RedactionAPI.UnitTests
{
    public class TestRedactService
    {
        private IConfiguration _testConfig;
        [SetUp]
        public void Setup()
        {
            var testConfigDictionary = new Dictionary<string, string>
            {
                { "RedactionSettings:BannedWords","Alpha,Bravo" }
            };

            _testConfig = new ConfigurationBuilder().AddInMemoryCollection(testConfigDictionary).Build();
        }

        [Test]
        public void ShouldReturnAnEmptyStringWhenPassedAnEmptyString()
        {
            //Arrange
            RedactService serviceUnderTest = new RedactService(_testConfig);

            //Act
            string redactedString = serviceUnderTest.Redact(String.Empty);

            //Assert
            Assert.That(redactedString, Is.EqualTo(String.Empty));

        }

        [Test]
        public void ShouldReplaceOnlyWordsOnBannedListWithREDACTED()
        {
            //Arrange
            RedactService serviceUnderTest = new RedactService(_testConfig);
            string testString = "Alpha Bravo Charlie";

            //Act
            string redactedString = serviceUnderTest.Redact(testString);

            //Assert
            Assert.That(redactedString, Is.EqualTo("REDACTED REDACTED Charlie"));
        }

        [Test]
        public void ShouldReplaceOnlyWordsOnBannedListWithREDACTEDCaseInsensitively()
        {
            //Arrange
            RedactService serviceUnderTest = new RedactService(_testConfig);
            string testString = "alpha BRAVO Charlie";

            //Act
            string redactedString = serviceUnderTest.Redact(testString);

            //Assert
            Assert.That(redactedString, Is.EqualTo("REDACTED REDACTED Charlie"));
        }

        [Test]
        public void ShouldNotRedactPartialMatches()
        {
            //Arrange
            RedactService serviceUnderTest = new RedactService(_testConfig);
            string testString = "alphas BRAVO Charlie AlphaBravo";

            //Act
            string redactedString = serviceUnderTest.Redact(testString);

            //Assert
            Assert.That(redactedString, Is.EqualTo("alphas REDACTED Charlie AlphaBravo"));
        }

        [Test]
        public void ShouldPreserveTrailingPunctuation()
        {
            //Arrange
            RedactService serviceUnderTest = new RedactService(_testConfig);
            string testString = "Alpha, BRAVO Charlie Bravo.";

            //Act
            string redactedString = serviceUnderTest.Redact(testString);

            //Assert
            Assert.That(redactedString, Is.EqualTo("REDACTED, REDACTED Charlie REDACTED."));
        }

        [Test]
        public void ShouldPreserveLeadingPunctuation()
        {
            //Arrange
            RedactService serviceUnderTest = new RedactService(_testConfig);
            string testString = "Alpha (BRAVO ) Charlie Bravo.";

            //Act
            string redactedString = serviceUnderTest.Redact(testString);

            //Assert
            Assert.That(redactedString, Is.EqualTo("REDACTED (REDACTED ) Charlie REDACTED."));
        }
    }
}