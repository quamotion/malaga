using OpenQA.Selenium.Remote;
using System;
using Xunit;

namespace Quamotion.Malaga.Tests
{
    public class WdaCommandExecutorTests
    {
        [Theory]
        [InlineData(WdaDriverCommand.LaunchApp, CommandInfo.PostCommand, "/session/{sessionId}/wda/apps/launch")]
        [InlineData(WdaDriverCommand.GetOrientation, CommandInfo.GetCommand, "/session/{sessionId}/orientation")]
        [InlineData(WdaDriverCommand.SetOrientation, CommandInfo.PostCommand, "/session/{sessionId}/orientation")]
        [InlineData(WdaDriverCommand.GetRotation, CommandInfo.GetCommand, "/session/{sessionId}/rotation")]
        [InlineData(WdaDriverCommand.SetRotation, CommandInfo.PostCommand, "/session/{sessionId}/rotation")]
        [InlineData(WdaDriverCommand.DismissKeyboard, CommandInfo.PostCommand, "/session/{sessionId}/wda/keyboard/dismiss")]
        [InlineData(WdaDriverCommand.ElementScreenShot, CommandInfo.GetCommand, "/session/{sessionId}/element/{elementId}/screenshot")]
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
