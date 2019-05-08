using System.Collections.Generic;
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
    }
}
