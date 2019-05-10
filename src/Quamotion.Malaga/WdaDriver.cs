using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;

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
        /// Sends a touch and hold event to the device.
        /// </summary>
        /// <param name="location">
        /// The position at which to touch.
        /// </param>
        /// <param name="duration">
        /// The duration of the touch event, in seconds.
        /// </param>
        public virtual void TouchAndHold(PointF location, TimeSpan duration)
        {
            // TODO: find a way to handle the timeout
            // var timeout = Math.Max(Timeouts.TestServerRequestTimeout.TotalSeconds, 2 * duration.TotalSeconds);
            this.Execute(
                WdaDriverCommand.TouchAndHold,
                new Dictionary<string, object>()
                {
                    { "x", location.X },
                    { "y", location.Y },
                    { "duration", duration.TotalSeconds }
                });
        }

        /// <summary>
        /// Sends a drag gesture to the device.
        /// </summary>
        /// <param name="from">
        /// The location at which the drag gesture begins.
        /// </param>
        /// <param name="to">
        /// The location at which the drag gesture ends.
        /// </param>
        /// <param name="duration">
        /// The duration of the drag gesture.
        /// </param>
        public virtual void Drag(PointF from, PointF to, TimeSpan duration)
        {
            // Drag on iOS is slow, _very_ slow.
            // Increase the timeout to make sure we don't time out (which would be a TaskCancelledException)
            // TODO: find a way to work with the timeout
            // var timeout = Math.Max(Timeouts.TestServerRequestTimeout.TotalSeconds, 15 + (5 * duration.TotalSeconds));

            this.Execute(
                WdaDriverCommand.Drag,
                new Dictionary<string, object>()
                {

                    { "fromX", from.X },
                            { "fromY", from.Y },
                            { "toX", to.X },
                            { "toY", to.Y },
                            { "duration", duration.TotalSeconds }
                });
        }

        /// <summary>
        /// Performs a sequence of touch actions.
        /// </summary>
        /// <param name="touches">
        /// The touch actions to execute.
        /// </param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> which can be used to cancel the asynchronous operation.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> which represents the asynchronous operation.
        /// </returns>
        public virtual void PerformTouch(Collection<TouchAction> touches)
        {
            double duration = 0;

            foreach (var touch in touches)
            {
                if (touch.Action == TouchOperation.Wait)
                {
                    duration += (double)touch.Options["ms"];
                }
                else if (touch.Action == TouchOperation.LongPress)
                {
                    duration += (double)touch.Options["duration"];
                }
            }

            // TODO: figure out a way to pass the timeout
            // TimeSpan.FromMilliseconds(Timeouts.TestServerRequestTimeout.TotalMilliseconds + (2 * duration)),

            var json = JsonConvert.SerializeObject(
                new
                {
                    actions = touches
                });

            this.Execute(
                WdaDriverCommand.PerformTouch
                json);
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
    }
}
