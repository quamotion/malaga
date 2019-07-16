using OpenQA.Selenium.Remote;
using System;
using Xunit;

namespace Quamotion.Malaga.Tests
{
    public class WdaCommandExecutorTests
    {
        [Theory]
        [InlineData(WdaDriverCommand.LaunchApp, CommandInfo.PostCommand, "/session/{sessionId}/wda/apps/launch")]
        [InlineData(WdaDriverCommand.TerminateApp, CommandInfo.PostCommand, "/session/{sessionId}/wda/apps/terminate")]
        

        [InlineData(WdaDriverCommand.GetOrientation, CommandInfo.GetCommand, "/session/{sessionId}/orientation")]
        [InlineData(WdaDriverCommand.SetOrientation, CommandInfo.PostCommand, "/session/{sessionId}/orientation")]
        [InlineData(WdaDriverCommand.GetRotation, CommandInfo.GetCommand, "/session/{sessionId}/rotation")]
        [InlineData(WdaDriverCommand.SetRotation, CommandInfo.PostCommand, "/session/{sessionId}/rotation")]

        [InlineData(WdaDriverCommand.SendKeys, CommandInfo.PostCommand, "/session/{sessionId}/wda/keys")]
        [InlineData(WdaDriverCommand.DismissKeyboard, CommandInfo.PostCommand, "/session/{sessionId}/wda/keyboard/dismiss")]
        [InlineData(WdaDriverCommand.ElementScreenShot, CommandInfo.GetCommand, "/session/{sessionId}/element/{elementId}/screenshot")]

        [InlineData(WdaDriverCommand.GetRectangle, CommandInfo.GetCommand, "/session/{sessionId}/element/{elementId}/rect")]

        [InlineData(WdaDriverCommand.IsDisplayed, CommandInfo.GetCommand, "/session/{sessionId}/element/{elementId}/displayed")]
        [InlineData(WdaDriverCommand.IsAccessible, CommandInfo.GetCommand, "/session/{sessionId}/wda/element/{elementId}/accessible")]
        [InlineData(WdaDriverCommand.IsAcessibilityContainer, CommandInfo.GetCommand, "/session/{sessionId}/wda/element/{elementId}/accessibilityContainer")]
        [InlineData(WdaDriverCommand.Swipe, CommandInfo.PostCommand, "/session/{sessionId}/wda/element/{elementId}/swipe")]
        [InlineData(WdaDriverCommand.Pinch, CommandInfo.PostCommand, "/session/{sessionId}/wda/element/{elementId}/pinch")]
        [InlineData(WdaDriverCommand.ElementDoubleTap, CommandInfo.PostCommand, "/session/{sessionId}/wda/element/{elementId}/doubleTap")]
        [InlineData(WdaDriverCommand.TwoFingerTap, CommandInfo.PostCommand, "/session/{sessionId}/wda/element/{elementId}/twoFingerTap")]
        [InlineData(WdaDriverCommand.ElementTouchAndHold, CommandInfo.PostCommand, "/session/{sessionId}/wda/element/{elementId}/touchAndHold")]
        [InlineData(WdaDriverCommand.Scroll, CommandInfo.PostCommand, "/session/{sessionId}/wda/element/{elementId}/scroll")]
        [InlineData(WdaDriverCommand.ElementDragFromToForDuration, CommandInfo.PostCommand, "/session/{sessionId}/wda/element/{elementId}/dragfromtoforduration")]
        [InlineData(WdaDriverCommand.DragFromToForDuration, CommandInfo.PostCommand, "/session/{sessionId}/wda/dragfromtoforduration")]
        [InlineData(WdaDriverCommand.Tap, CommandInfo.PostCommand, "/session/{sessionId}/wda/tap/{elementId}")]
        [InlineData(WdaDriverCommand.TouchAndHold, CommandInfo.PostCommand, "/session/{sessionId}/wda/touchAndHold")]
        [InlineData(WdaDriverCommand.DoubleTap, CommandInfo.PostCommand, "/session/{sessionId}/wda/doubleTap")]
        [InlineData(WdaDriverCommand.WheelSelect, CommandInfo.PostCommand, "/session/{sessionId}/wda/pickerwheel/{elementId}/select")]
        [InlineData(WdaDriverCommand.ForceTouch, CommandInfo.PostCommand, "/session/{sessionId}/wda/element/{elementId}/forceTouch")]
        [InlineData("getWindowSize", CommandInfo.GetCommand, "/session/{sessionId}/window/size")]
        [InlineData("sendKeysToActiveElement", CommandInfo.PostCommand, "/session/{sessionId}/wda/keys")]
        public void CommandHandled(string command, string method, string endPoint)
        {
            var executor = new WdaCommandExecutor(new Uri("http://localhost"), "my-session-id", TimeSpan.FromSeconds(1));
            var commandInfo = executor.CommandInfoRepository.GetCommandInfo(command);
            Assert.NotNull(commandInfo);
            Assert.Equal(endPoint, commandInfo.ResourcePath);
            Assert.Equal(method, commandInfo.Method);
        }
    }
}
