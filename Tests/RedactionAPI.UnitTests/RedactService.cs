using RedactionAPI.Services;

namespace RedactionAPI.UnitTests
{
    public class TestRedactService
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ShouldReturnAnEmptyStringWhenPassedAnEmptyString()
        {
            //Arrange
            RedactService serviceUnderTest = new RedactService();

            //Act
            string redactedString = serviceUnderTest.Redact(String.Empty);

            //Assert
            Assert.That(redactedString, Is.EqualTo(String.Empty));

        }
    }
}