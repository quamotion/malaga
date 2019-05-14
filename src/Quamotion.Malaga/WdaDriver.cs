﻿using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;

namespace Quamotion.Malaga
{
    public class WdaDriver : RemoteWebDriver
    {
        private readonly Dictionary<string, object> EmptyDictionary = new Dictionary<string, object>();

        protected WdaDriver()
            : base(WdaCapabilities.Default)
        {
            // This constructor should be used for mocking/unit tests only.
        }

        public WdaDriver(Uri uri)
            : this(uri, null)
        {
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

        public void DismissKeyboard(string label = "return")
        {
            if (string.IsNullOrEmpty(label))
            {
                this.Execute(
                   WdaDriverCommand.DismissKeyboard,
                   new Dictionary<string, object>()
                   {
                   });
            }
            else
            {
                var element = this.FindElementByPredicateString($"label='{label}'");
                element.Click();
            }
        }

        public byte[] GetScreenshot(IWebElement element)
        {
            var elementId = this.GetElementId(element);
            var commandResponse = this.Execute(
                WdaDriverCommand.ElementScreenShot,
                new Dictionary<string, object>()
                {
                    { "elementId", elementId}
                });
            return Convert.FromBase64String((string)commandResponse.Value);
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

        /// <summary>
        /// Gets the text of the current alert.
        /// </summary>
        /// <returns>
        /// The text of the current alert.
        /// </returns>
        public virtual string GetAlertText()
        {
            var response = this.Execute(WdaDriverCommand.GetAlertText, this.EmptyDictionary);
            return (string)response.Value;
        }

        /// <summary>
        /// Gets the buttons of the current active alert.
        /// </summary>
        /// <returns>
        /// The labels of the buttons of the current active alert.
        /// </returns>
        public virtual IEnumerable<string> GetAlertButtons()
        {
            var response = this.Execute(WdaDriverCommand.GetAlertButtons, this.EmptyDictionary);
            return (IEnumerable<string>)response.Value;
        }

        /// <summary>
        /// Clicks on an alert button.
        /// </summary>
        /// <param name="button">
        /// The button on which to click.
        /// </param>
        public virtual void ClickAlertButton(string button)
        {
            this.Execute(
                WdaDriverCommand.ClickAlertButton,
                new Dictionary<string, object>()
                {
                    {  "name", button }
                });
        }

        /// <summary>
        /// Gets the current session's status
        /// </summary>
        public virtual WdaStatus GetSessionStatus()
        {
            var response = this.Execute(WdaDriverCommand.GetSessionStatus, this.EmptyDictionary);
            return JsonConvert.DeserializeObject<WdaStatus>(JsonConvert.SerializeObject(response.Value));
        }

        /// <summary>
        /// Types text using the on-screen keyboard.
        /// </summary>
        /// <param name="text">
        /// The text to type.</param>
        public virtual void TypeText(string text)
        {
            this.Execute(
                WdaDriverCommand.Type,
                new Dictionary<string, object>()
                {
                    { "text", text }
                });
        }

        /// <summary>
        /// Presses a hardware (device) button.
        /// </summary>
        /// <param name="button">
        /// The button to press.
        /// </param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the asynchronous operation.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> which represents the asynchronous operation.
        /// </returns>
        public virtual void PressDeviceButton(DeviceButton button)
        {
            this.Execute(
                WdaDriverCommand.PressDeviceButton,
                new Dictionary<string, object>()
                {
                    { "button", (int)button }
                });
        }

        private string GetElementId(IWebElement webElement)
        {
            var remoteWebElementType = typeof(RemoteWebElement);
            var elementIdField = remoteWebElementType.GetField("elementId", BindingFlags.Instance | BindingFlags.NonPublic);
            return elementIdField.GetValue(webElement) as string;
        }
    }
}
