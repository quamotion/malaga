using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Quamotion.Malaga
{
    public class WdaDriver : RemoteWebDriver
    {
        protected WdaDriver()
            : base(WdaCapabilities.Default)
        {
            // This constructor should be used for mocking/unit tests only.
        }

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

        public IWebElement FindElementByClassChain(string classChain)
        {
            return this.FindElement("class chain", classChain);
        }

        public ReadOnlyCollection<IWebElement> FindElementsByClassChain(string classChain)
        {
            return this.FindElements("class chain", classChain);
        }

        public IWebElement FindElementByPredicateString(string classChain)
        {
            return this.FindElement("predicate string", classChain);
        }

        public ReadOnlyCollection<IWebElement> FindElementsByPredicateString(string classChain)
        {
            return this.FindElements("predicate string", classChain);
        }

        public virtual ScreenOrientation Orientation
        {
            get
            {
                var commandResponse = this.Execute(WdaDriverCommand.GetOrientation, null);
                return (ScreenOrientation)Enum.Parse(typeof(ScreenOrientation), (string)commandResponse.Value);
            }
            set
            {
                this.Execute(
                    WdaDriverCommand.SetOrientation,
                    new Dictionary<string, object>()
                    {
                        { "orientation", value.ToString() }
                    });
            }
        }

        public virtual ScreenOrientation Rotation
        {
            get
            {
                var commandResponse = this.Execute(WdaDriverCommand.GetRotation, null);
                return (ScreenOrientation)Enum.Parse(typeof(ScreenOrientation), (string)commandResponse.Value);
            }
            set
            {
                this.Execute(
                    WdaDriverCommand.SetRotation,
                    new Dictionary<string, object>()
                    {
                        { "rotation", value.ToString() }
                    });
            }
        }
    }
}
