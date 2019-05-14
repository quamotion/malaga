using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace Quamotion.Malaga.Tests
{
    public class WdaDriverTests
    {
        private readonly MockCommandExecutor commandExecutor;
        private readonly WdaDriver driver;

        public WdaDriverTests()
        {
            this.commandExecutor = new MockCommandExecutor();
            this.driver = new WdaDriver(this.commandExecutor);
        }

        [Fact]
        public void GetScreenshotTest()
        {
            var element = new RemoteWebElement(this.driver, "4D000000-0000-0000-B42A-000000000000");
            var image = this.driver.GetScreenshot(element);

            Assert.Single(commandExecutor.Commands);

            var originalScreenshot = File.ReadAllBytes("screenshot.jpg");

            Assert.Equal(originalScreenshot, image);
        }

        [Fact]
        public void LaunchAppTest()
        {
            var result = this.driver.LaunchApp("mobi.quamotion.app");

            var command = Assert.Single(commandExecutor.Commands);

            Assert.Equal(WdaDriverCommand.LaunchApp, command.Name);
            Assert.Equal(
                new Dictionary<string, object>
                {
                    { "bundleId", "mobi.quamotion.app" },
                    { "shouldWaitForQuiescence", false }
                },
                command.Parameters);
        }

        [Fact]
        public void FindElementByClassChainTest()
        {
            this.driver.FindElementByClassChain("my-class-chain");

            var command = Assert.Single(commandExecutor.Commands);

            Assert.Equal(DriverCommand.FindElement, command.Name);
            Assert.Equal(
                new Dictionary<string, object>
                {
                    { "using", "class chain" },
                    { "value", "my-class-chain" }
                },
                command.Parameters);
        }

        [Fact]
        public void FindElementsByClassChainTest()
        {
            this.driver.FindElementsByClassChain("another-class-chain");

            var command = Assert.Single(commandExecutor.Commands);

            Assert.Equal(DriverCommand.FindElements, command.Name);
            Assert.Equal(
                new Dictionary<string, object>
                {
                    { "using", "class chain" },
                    { "value", "another-class-chain" }
                },
                command.Parameters);
        }

        [Fact]
        public void FindElementByPredicateStringTest()
        {
            this.driver.FindElementByPredicateString("my-predicate-string");

            var command = Assert.Single(commandExecutor.Commands);

            Assert.Equal(DriverCommand.FindElement, command.Name);
            Assert.Equal(
                new Dictionary<string, object>
                {
                    { "using", "predicate string" },
                    { "value", "my-predicate-string" }
                },
                command.Parameters);
        }

        [Fact]
        public void FindElementsByPredicateStringTest()
        {
            this.driver.FindElementsByPredicateString("another-predicate-string");

            var command = Assert.Single(commandExecutor.Commands);

            Assert.Equal(DriverCommand.FindElements, command.Name);
            Assert.Equal(
                new Dictionary<string, object>
                {
                    { "using", "predicate string" },
                    { "value", "another-predicate-string" }
                },
                command.Parameters);
        }

        [Fact]
        public void SetOrientationTest()
        {
            this.driver.Orientation = ScreenOrientation.Landscape;

            var command = Assert.Single(commandExecutor.Commands);

            Assert.Equal(DriverCommand.SetOrientation, command.Name);
            Assert.Equal(
                new Dictionary<string, object>
                {
                    { "orientation", "Landscape" },
                },
                command.Parameters);
        }

        [Fact]
        public void SetRotationTest()
        {
            this.driver.Rotation = ScreenOrientation.Landscape;

            var command = Assert.Single(commandExecutor.Commands);

            Assert.Equal(WdaDriverCommand.SetRotation, command.Name);
            Assert.Equal(
                new Dictionary<string, object>
                {
                    { "rotation", "Landscape" },
                },
                command.Parameters);
        }
    }
}
