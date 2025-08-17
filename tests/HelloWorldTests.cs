using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PracticeManagementSystem.Tests
{
    /// <summary>
    /// Unit tests for HelloWorld functionality.
    /// </summary>
    [TestClass]
    public class HelloWorldTests
    {
        [TestMethod]
        public void Simple_Test_Should_Pass()
        {
            // Arrange
            var expected = true;

            // Act
            var actual = true;

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}