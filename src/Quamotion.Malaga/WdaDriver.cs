using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Quamotion.Malaga
{
    public class WdaDriver : RemoteWebDriver
    {
        public WdaDriver(Uri uri, string sessionId)
            : this(new WdaCommandExecutor(uri, sessionId, TimeSpan.FromSeconds(60)))
        {
        }

        public WdaDriver(ICommandExecutor commandExecutor)
            : base(commandExecutor, WdaCapabilities.Default)
        {
        }

        public Response LaunchApp(string bundleId, bool shouldWaitForQuiescence = false, Collection<string> arguments = null, Dictionary<string, string> environment = null)
        {
            var parameters = new Dictionary<string, object>()
            {
                { "bundleId", bundleId },
                { "shouldWaitForQuiescence", shouldWaitForQuiescence }
            };

            if (arguments != null)
            {
                parameters.Add("arguments", arguments);
            }

            if (environment != null)
            {
                parameters.Add("environment", environment);
            }

            return this.Execute(
                WdaDriverCommand.LaunchApp, parameters);
        }
    }
}
