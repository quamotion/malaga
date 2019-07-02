using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
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
        public void TerminateAppTest()
        {
            var result = this.driver.TerminateApp("mobi.quamotion.app");

            var command = Assert.Single(commandExecutor.Commands);

            Assert.Equal(WdaDriverCommand.TerminateApp, command.Name);
            Assert.Equal(
                new Dictionary<string, object>
                {
                    { "bundleId", "mobi.quamotion.app" }
                },
                command.Parameters);
        }

        [Fact]
        public void SendKeysTest()
        {
            this.driver.SendKeys("test");

            var command = Assert.Single(commandExecutor.Commands);

            Assert.Equal(WdaDriverCommand.SendKeys, command.Name);
            Assert.Equal(
                new Dictionary<string, object>
                {
                    { "value", "test" }
                },
                command.Parameters);
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

        [Fact]
        public void GetRectangleTest()
        {
            var element = new RemoteWebElement(this.driver, "4D000000-0000-0000-B42A-000000000000");
            var rectangle = this.driver.GetRectangle(element);

            Assert.Equal(100, rectangle.X);
            Assert.Equal(200, rectangle.Y);
            Assert.Equal(50, rectangle.Width);
            Assert.Equal(150, rectangle.Height);

            var command = Assert.Single(commandExecutor.Commands);

            Assert.Equal(WdaDriverCommand.GetRectangle, command.Name);
            Assert.Equal(
                new Dictionary<string, object>
                {
                    { "elementId", "4D000000-0000-0000-B42A-000000000000" }
                },
                command.Parameters);
        }

        [Fact]
        public void IsDisplayedTest()
        {
            var element = new RemoteWebElement(this.driver, "4D000000-0000-0000-B42A-000000000000");
            var displayed = this.driver.IsDisplayed(element);

            var command = Assert.Single(commandExecutor.Commands);

            Assert.True(displayed);
            Assert.Equal(WdaDriverCommand.IsDisplayed, command.Name);
            Assert.Equal(
                new Dictionary<string, object>
                {
                    { "elementId", "4D000000-0000-0000-B42A-000000000000" }
                },
                command.Parameters);
        }

        [Fact]
        public void IsAccessibleTest()
        {
            var element = new RemoteWebElement(this.driver, "4D000000-0000-0000-B42A-000000000000");
            var accessible = this.driver.IsAccessible(element);

            var command = Assert.Single(commandExecutor.Commands);

            Assert.False(accessible);
            Assert.Equal(WdaDriverCommand.IsAccessible, command.Name);
            Assert.Equal(
                new Dictionary<string, object>
                {
                    { "elementId", "4D000000-0000-0000-B42A-000000000000" }
                },
                command.Parameters);
        }

        [Fact]
        public void IsAcessibilityContainerTest()
        {
            var element = new RemoteWebElement(this.driver, "4D000000-0000-0000-B42A-000000000000");
            var isAcessibilityContainer = this.driver.IsAcessibilityContainer(element);

            var command = Assert.Single(commandExecutor.Commands);

            Assert.True(isAcessibilityContainer);
            Assert.Equal(WdaDriverCommand.IsAcessibilityContainer, command.Name);
            Assert.Equal(
                new Dictionary<string, object>
                {
                    { "elementId", "4D000000-0000-0000-B42A-000000000000" }
                },
                command.Parameters);
        }

        [Fact]
        public void SwipeTest()
        {
            var element = new RemoteWebElement(this.driver, "4D000000-0000-0000-B42A-000000000000");
            this.driver.Swipe(element, Direction.Left);

            var command = Assert.Single(commandExecutor.Commands);

            Assert.Equal(WdaDriverCommand.Swipe, command.Name);
            Assert.Equal(
                new Dictionary<string, object>
                {
                    { "elementId", "4D000000-0000-0000-B42A-000000000000" },
                    { "direction", "left" }
                },
                command.Parameters);
        }

        [Fact]
        public void DoubleTapTest()
        {
            var element = new RemoteWebElement(this.driver, "4D000000-0000-0000-B42A-000000000000");
            this.driver.DoubleTap(element);

            var command = Assert.Single(commandExecutor.Commands);

            Assert.Equal(WdaDriverCommand.ElementDoubleTap, command.Name);
            Assert.Equal(
                new Dictionary<string, object>
                {
                    { "elementId", "4D000000-0000-0000-B42A-000000000000" }
                },
                command.Parameters);
        }

        [Fact]
        public void TwoFingerTapTest()
        {
            var element = new RemoteWebElement(this.driver, "4D000000-0000-0000-B42A-000000000000");
            this.driver.TwoFingerTap(element);

            var command = Assert.Single(commandExecutor.Commands);

            Assert.Equal(WdaDriverCommand.TwoFingerTap, command.Name);
            Assert.Equal(
                new Dictionary<string, object>
                {
                    { "elementId", "4D000000-0000-0000-B42A-000000000000" }
                },
                command.Parameters);
        }

        [Fact]
        public void TouchAndHoldTest()
        {
            var element = new RemoteWebElement(this.driver, "4D000000-0000-0000-B42A-000000000000");
            this.driver.TouchAndHold(element, 1);

            var command = Assert.Single(commandExecutor.Commands);

            Assert.Equal(WdaDriverCommand.ElementTouchAndHold, command.Name);
            Assert.Equal(
                new Dictionary<string, object>
                {
                    { "elementId", "4D000000-0000-0000-B42A-000000000000" },
                    { "duration", 1.0}
                },
                command.Parameters);
        }

        [Fact]
        public void TouchAndHoldCoordinateTest()
        {
            this.driver.TouchAndHold(100, 200, 2);

            var command = Assert.Single(commandExecutor.Commands);

            Assert.Equal(WdaDriverCommand.TouchAndHold, command.Name);
            Assert.Equal(
                new Dictionary<string, object>
                {
                    { "x", 100.0},
                    { "y", 200.0},
                    { "duration", 2.0}
                },
                command.Parameters);
        }

        [Fact]
        public void ScrollTest()
        {
            var menu = new RemoteWebElement(this.driver, "4D000000-0000-0000-B42A-000000000000");
            this.driver.Scroll(menu, Direction.Down, 10);

            var command = Assert.Single(commandExecutor.Commands);

            Assert.Equal(WdaDriverCommand.Scroll, command.Name);
            Assert.Equal(
                new Dictionary<string, object>
                {
                    { "elementId", "4D000000-0000-0000-B42A-000000000000" },
                    { "direction", "down"},
                    { "distance", 10.0}
                },
                command.Parameters);
        }

        [Fact]
        public void ScrollToPredicateStringTest()
        {
            var menu = new RemoteWebElement(this.driver, "4D000000-0000-0000-B42A-000000000000");
            this.driver.ScrollToPredicateString(menu, "name == 'Screen Time'");

            var command = Assert.Single(commandExecutor.Commands);

            Assert.Equal(WdaDriverCommand.Scroll, command.Name);
            Assert.Equal(
                new Dictionary<string, object>
                {
                    { "elementId", "4D000000-0000-0000-B42A-000000000000" },
                    { "predicateString", "name == 'Screen Time'"}
                },
                command.Parameters);
        }

        [Fact]
        public void ScrollToNameTest()
        {
            var menu = new RemoteWebElement(this.driver, "4D000000-0000-0000-B42A-000000000000");
            this.driver.ScrollToName(menu, "Screen Time");

            var command = Assert.Single(commandExecutor.Commands);

            Assert.Equal(WdaDriverCommand.Scroll, command.Name);
            Assert.Equal(
                new Dictionary<string, object>
                {
                    { "elementId", "4D000000-0000-0000-B42A-000000000000" },
                    { "name", "Screen Time"}
                },
                command.Parameters);
        }

        [Fact]
        public void ScrollToVisibleTest()
        {
            var menu = new RemoteWebElement(this.driver, "4D000000-0000-0000-B42A-000000000000");
            this.driver.ScrollToVisible(menu);

            var command = Assert.Single(commandExecutor.Commands);

            Assert.Equal(WdaDriverCommand.Scroll, command.Name);
            Assert.Equal(
                new Dictionary<string, object>
                {
                    { "elementId", "4D000000-0000-0000-B42A-000000000000" },
                    { "toVisible", true}
                },
                command.Parameters);
        }

        [Fact]
        public void PinchTest()
        {
            var picture = new RemoteWebElement(this.driver, "4D000000-0000-0000-B42A-000000000000");
            this.driver.Pinch(picture, 20, 30);

            var command = Assert.Single(commandExecutor.Commands);

            Assert.Equal(WdaDriverCommand.Pinch, command.Name);
            Assert.Equal(
                new Dictionary<string, object>
                {
                    { "elementId", "4D000000-0000-0000-B42A-000000000000" },
                    { "scale", 20.0},
                    { "velocity", 30.0}
                },
                command.Parameters);
        }

        [Fact]
        public void DragTest()
        {
            var picture = new RemoteWebElement(this.driver, "4D000000-0000-0000-B42A-000000000000");
            this.driver.Drag(picture, 20, 30, 100, 200, 2);

            var command = Assert.Single(commandExecutor.Commands);

            Assert.Equal(WdaDriverCommand.ElementDragFromToForDuration, command.Name);
            Assert.Equal(
                new Dictionary<string, object>
                {
                    { "elementId", "4D000000-0000-0000-B42A-000000000000" },
                    { "fromX", 20.0},
                    { "fromY", 30.0},
                    { "toX", 100.0},
                    { "toY", 200.0},
                    { "duration", 2.0}
                },
                command.Parameters);
        }

        [Fact]
        public void DragCoordinateTest()
        {
            this.driver.Drag(20, 30, 100, 200, 2);

            var command = Assert.Single(commandExecutor.Commands);

            Assert.Equal(WdaDriverCommand.DragFromToForDuration, command.Name);
            Assert.Equal(
                new Dictionary<string, object>
                {
                    { "fromX", 20.0},
                    { "fromY", 30.0},
                    { "toX", 100.0},
                    { "toY", 200.0},
                    { "duration", 2.0}
                },
                command.Parameters);
        }

        [Fact]
        public void TapTest()
        {
            var appIcon = new RemoteWebElement(this.driver, "4D000000-0000-0000-B42A-000000000000");
            this.driver.Tap(appIcon, 20, 60);

            var command = Assert.Single(commandExecutor.Commands);

            Assert.Equal(WdaDriverCommand.Tap, command.Name);
            Assert.Equal(
                new Dictionary<string, object>
                {
                    { "elementId", "4D000000-0000-0000-B42A-000000000000" },
                    { "x", 20.0},
                    { "y", 60.0}
                },
                command.Parameters);
        }

        [Fact]
        public void WheelSelectTest()
        {
            var wheel = new RemoteWebElement(this.driver, "4D000000-0000-0000-B42A-000000000000");
            this.driver.WheelSelect(wheel, Order.Next, 0.1);

            var command = Assert.Single(commandExecutor.Commands);

            Assert.Equal(WdaDriverCommand.WheelSelect, command.Name);
            Assert.Equal(
                new Dictionary<string, object>
                {
                    { "elementId", "4D000000-0000-0000-B42A-000000000000" },
                    { "order", "next"},
                    { "offset", 0.1}
                },
                command.Parameters);
        }
    }
}
