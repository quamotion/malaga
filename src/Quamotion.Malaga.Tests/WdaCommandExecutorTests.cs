using System;
using Xunit;

namespace Quamotion.Malaga.Tests
{
    public class WdaCommandExecutorTests
    {
        [Theory]
        [InlineData(WdaDriverCommand.LaunchApp, "/session/{sessionId}/wda/apps/launch")]
        public void CommandHandled(string command, string endPoint)
        {
            var executor = new WdaCommandExecutor(new Uri("http://localhost"), "my-session-id", TimeSpan.FromSeconds(1));
            var commandInfo = executor.CommandInfoRepository.GetCommandInfo(command);
            Assert.NotNull(commandInfo);
            Assert.Equal(endPoint, commandInfo.ResourcePath);
        }
    }
}
