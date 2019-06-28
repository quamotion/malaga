using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

[assembly: InternalsVisibleTo("Quamotion.Malaga.Tests")]
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

        public WdaDriver(Uri uri, TimeSpan serverResponseTimeout)
            : this(uri, null, serverResponseTimeout)
        {
        }

        public WdaDriver(Uri uri)
            : this(uri, null, TimeSpan.FromSeconds(60))
        {
        }

        public WdaDriver(Uri uri, string sessionId)
            : this(new WdaCommandExecutor(uri, sessionId, TimeSpan.FromSeconds(60)))
        {
        }

        public WdaDriver(Uri uri, string sessionId, TimeSpan serverResponseTimeout)
            : this(new WdaCommandExecutor(uri, sessionId, serverResponseTimeout))
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

        public virtual Rectangle GetRectangle(IWebElement element)
        {
            var elementId = this.GetElementId(element);

            var response = this.Execute(
                WdaDriverCommand.GetRectangle,
                new Dictionary<string, object>()
                {
                    { "elementId", elementId }
                });

            var value = response.Value as Dictionary<string, object>;
            var x = (int)(long)value["x"];
            var y = (int)(long)value["y"];
            var width = (int)(long)value["width"];
            var height = (int)(long)value["height"];

            return new Rectangle(x, y, width, height);
        }

        public virtual bool IsDisplayed(IWebElement element)
        {
            var elementId = this.GetElementId(element);

            var response = this.Execute(
                WdaDriverCommand.IsDisplayed,
                new Dictionary<string, object>()
                {
                    { "elementId", elementId }
                });

            return (bool)response.Value;
        }

        public virtual bool IsAccessible(IWebElement element)
        {
            var elementId = this.GetElementId(element);

            var response = this.Execute(
                WdaDriverCommand.IsAccessible,
                new Dictionary<string, object>()
                {
                    { "elementId", elementId }
                });

            return (bool)response.Value;
        }

        public virtual bool IsAcessibilityContainer(IWebElement element)
        {
            var elementId = this.GetElementId(element);

            var response = this.Execute(
                WdaDriverCommand.IsAcessibilityContainer,
                new Dictionary<string, object>()
                {
                    { "elementId", elementId }
                });

            return (bool)response.Value;
        }

        public virtual void Swipe(IWebElement element, Direction direction)
        {
            var elementId = this.GetElementId(element);

            this.Execute(
                WdaDriverCommand.Swipe,
                new Dictionary<string, object>()
                {
                    { "elementId", elementId },
                    { "direction",  this.GetEnumMemberValue(direction)}
                });
        }

        public virtual void Pinch(IWebElement element, double scale, double velocity)
        {
            var elementId = this.GetElementId(element);

            this.Execute(
                WdaDriverCommand.Pinch,
                new Dictionary<string, object>()
                {
                    { "elementId", elementId },
                    { "scale", scale},
                    { "velocity", velocity}
                });
        }

        public virtual void DoubleTap(IWebElement element)
        {
            var elementId = this.GetElementId(element);

            this.Execute(
                WdaDriverCommand.ElementDoubleTap,
                new Dictionary<string, object>()
                {
                    { "elementId", elementId }
                });
        }

        public virtual void Tap(IWebElement element, double x, double y)
        {
            var elementId = this.GetElementId(element);

            this.Execute(
                WdaDriverCommand.Tap,
                new Dictionary<string, object>()
                {
                    { "elementId", elementId },
                    { "x", x },
                    { "y", y }
                });
        }


        public virtual void TwoFingerTap(IWebElement element)
        {
            var elementId = this.GetElementId(element);

            this.Execute(
                WdaDriverCommand.TwoFingerTap,
                new Dictionary<string, object>()
                {
                    { "elementId", elementId }
                });
        }

        public virtual void TouchAndHold(IWebElement element, double duration)
        {
            var elementId = this.GetElementId(element);

            this.Execute(
                WdaDriverCommand.ElementTouchAndHold,
                new Dictionary<string, object>()
                {
                    { "elementId", elementId },
                    { "duration", duration}
                });
        }

        public virtual void TouchAndHold(double x, double y, double duration)
        {
            this.Execute(
                WdaDriverCommand.TouchAndHold,
                new Dictionary<string, object>()
                {
                    { "x", x },
                    { "y", y },
                    { "duration", duration}
                });
        }

        public virtual void ScrollToVisible(IWebElement element)
        {
            var elementId = this.GetElementId(element);

            this.Execute(
                WdaDriverCommand.Scroll,
                new Dictionary<string, object>()
                {
                    { "elementId", elementId },
                    { "toVisible", true}
                });
        }

        public virtual void ScrollToName(IWebElement container, string name)
        {
            var elementId = this.GetElementId(container);

            this.Execute(
                WdaDriverCommand.Scroll,
                new Dictionary<string, object>()
                {
                    { "elementId", elementId },
                    { "name", name}
                });
        }
        public virtual void ScrollToPredicateString(IWebElement container, string predicateString)
        {
            var elementId = this.GetElementId(container);

            this.Execute(
                WdaDriverCommand.Scroll,
                new Dictionary<string, object>()
                {
                    { "elementId", elementId },
                    { "predicateString", predicateString}
                });
        }

        public virtual void Scroll(IWebElement container, Direction direction, double distance)
        {
            var elementId = this.GetElementId(container);

            this.Execute(
                WdaDriverCommand.Scroll,
                new Dictionary<string, object>()
                {
                    { "elementId", elementId },
                    { "direction",  this.GetEnumMemberValue(direction)},
                    { "distance", distance}
                });
        }


        public virtual void Drag(IWebElement element, double fromX, double fromY, double toX, double toY, double duration)
        {
            var elementId = this.GetElementId(element);

            this.Execute(
                WdaDriverCommand.ElementDragFromToForDuration,
                new Dictionary<string, object>()
                {
                    { "elementId", elementId },
                    { "fromX", fromX },
                    { "fromY", fromY },
                    { "toX", toX },
                    { "toY", toY },
                    { "duration", duration }
                });
        }

        public virtual void Drag(double fromX, double fromY, double toX, double toY, double duration)
        {
            this.Execute(
                WdaDriverCommand.DragFromToForDuration,
                new Dictionary<string, object>()
                {
                    { "fromX", fromX },
                    { "fromY", fromY },
                    { "toX", toX },
                    { "toY", toY },
                    { "duration", duration }
                });
        }

        public virtual void ForceTouch(IWebElement element, double x, double y, double pressure, double duration)
        {
            var elementId = this.GetElementId(element);

            this.Execute(
                WdaDriverCommand.ForceTouch,
                new Dictionary<string, object>()
                {
                    { "elementId", elementId },
                    { "x", x },
                    { "y", y },
                    { "pressure", pressure },
                    { "duration", duration }
                });
        }

        public virtual void WheelSelect(IWebElement element, Order order, double offset = 0.2)
        {
            var elementId = this.GetElementId(element);

            this.Execute(
                WdaDriverCommand.WheelSelect,
                new Dictionary<string, object>()
                {
                    { "elementId", elementId },
                    { "order", this.GetEnumMemberValue(order) },
                    { "offset", offset }
                });
        }

        /// <summary>
        /// Gets the <see cref="ICommandExecutor"/> which executes commands for this driver.
        /// </summary>
        internal ICommandExecutor GetCommandExecutor()
        {
            return base.CommandExecutor;
        }

        private string GetEnumMemberValue(Enum value)
        {
            var enumMember = value.GetType()
                                    .GetTypeInfo()
                                    .GetMember(value.ToString())
                                    .First()
                                    .GetCustomAttributes(false)
                                    .OfType<EnumMemberAttribute>()
                                    .LastOrDefault();

            if (enumMember != null && enumMember.Value != null)
            {
                return enumMember.Value;
            }
            else
            {
                return value.ToString();
            }
        }

        private string GetElementId(IWebElement webElement)
        {
            var remoteWebElementType = typeof(RemoteWebElement);
            var elementIdField = remoteWebElementType.GetField("elementId", BindingFlags.Instance | BindingFlags.NonPublic);
            return elementIdField.GetValue(webElement) as string;
        }
    }
}
