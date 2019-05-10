using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Quamotion.Malaga
{   /// <summary>
    /// Represents a touch action which can be executed using the WebDriverAgent.
    /// </summary>
    public class TouchAction
    {
        /// <summary>
        /// Gets the operation being executed.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter), /* camelCaseText */ true)]
        [JsonProperty("action")]
        public TouchOperation Action { get; private set; }

        /// <summary>
        /// Gets a dictionary which contains the options for this operation.
        /// </summary>
        [JsonProperty("options")]
        public Dictionary<string, object> Options { get; private set; } = new Dictionary<string, object>();

        /// <summary>
        /// Performs a tap (finger down and finger up) operation.
        /// </summary>
        /// <param name="x">
        /// The x-coordinate of the position at which to tap.
        /// </param>
        /// <param name="y">
        /// The y-coordinate of the position at which to tap.
        /// </param>
        /// <param name="count">
        /// The number of taps to perform.
        /// </param>
        /// <returns>
        /// A <see cref="TouchAction"/> which represents the tap operation.
        /// </returns>
        public static TouchAction Tap(double x, double y, int count = 1)
        {
            return new TouchAction()
            {
                Action = TouchOperation.Tap,
                Options = new Dictionary<string, object>()
                {
                    { nameof(x), x },
                    { nameof(y),  y },
                    { nameof(count), count }
                }
            };
        }

        /// <summary>
        /// Performs a long press operation.
        /// </summary>
        /// <param name="x">
        /// The x-coordinate of the position at which to tap.
        /// </param>
        /// <param name="y">
        /// The y-coordinate of the position at which to tap.
        /// </param>
        /// <param name="duration">
        /// The duration, in milliseconds, of the tap gesture.
        /// </param>
        /// <param name="pressure">
        /// The amount of pressure to apply.
        /// </param>
        /// <returns>
        /// A <see cref="TouchAction"/> which represents the long press operation.
        /// </returns>
        public static TouchAction LongPress(double x, double y, double duration = 500, double pressure = 0)
        {
            return new TouchAction()
            {
                Action = TouchOperation.LongPress,
                Options = new Dictionary<string, object>()
                {
                    { nameof(x), x },
                    { nameof(y),  y },
                    { nameof(duration), duration },
                    { nameof(pressure), pressure }
                }
            };
        }

        /// <summary>
        /// Performs a finger down operation.
        /// </summary>
        /// <param name="x">
        /// The x-coordinate of the position at which to perform the finger down.
        /// </param>
        /// <param name="y">
        /// The y-coordinate of the position at which to perform the finger down.
        /// </param>
        /// <param name="pressure">
        /// The amount of pressure to apply.
        /// </param>
        /// <returns>
        /// A <see cref="TouchAction"/> which represents the finger down operation.
        /// </returns>
        public static TouchAction Press(double x, double y, double pressure = 0)
        {
            return new TouchAction()
            {
                Action = TouchOperation.Press,
                Options = new Dictionary<string, object>()
                {
                    { nameof(x), x },
                    { nameof(y),  y },
                    { nameof(pressure), pressure }
                }
            };
        }

        /// <summary>
        /// Performs a finger up operation.
        /// </summary>
        /// <returns>
        /// A <see cref="TouchAction"/> which represents the finger up operation.
        /// </returns>
        public static TouchAction Release()
        {
            return new TouchAction()
            {
                Action = TouchOperation.Release,
                Options = new Dictionary<string, object>()
                {
                }
            };
        }

        /// <summary>
        /// Moves the finger to a new position.
        /// </summary>
        /// <param name="x">
        /// The x-coordiante of the position to which to move the finger.
        /// </param>
        /// <param name="y">
        /// The y-coordinate of the position to which to move the finger.
        /// </param>
        /// <returns>
        /// A <see cref="TouchAction"/> which represents the move operation.
        /// </returns>
        public static TouchAction MoveTo(double x, double y)
        {
            return new TouchAction()
            {
                Action = TouchOperation.MoveTo,
                Options = new Dictionary<string, object>()
                {
                    { nameof(x), x },
                    { nameof(y),  y },
                }
            };
        }

        /// <summary>
        /// Waits for a specific amount of time.
        /// </summary>
        /// <param name="timeSpan">
        /// The amount of time to wait.
        /// </param>
        /// <returns>
        /// A <see cref="TouchOperation"/> which represents the wait operation.
        /// </returns>
        public static TouchAction Wait(TimeSpan timeSpan)
        {
            return Wait(timeSpan.TotalMilliseconds);
        }

        /// <summary>
        /// Waits for a specific amount of time.
        /// </summary>
        /// <param name="ms">
        /// The amount of time to wait, in milliseconds.
        /// </param>
        /// <returns>
        /// A <see cref="TouchOperation"/> which represents the wait operation.
        /// </returns>
        public static TouchAction Wait(double ms)
        {
            return new TouchAction()
            {
                Action = TouchOperation.Wait,
                Options = new Dictionary<string, object>()
                {
                    { nameof(ms), ms },
                }
            };
        }

        /// <summary>
        /// Cancels the current operation.
        /// </summary>
        /// <returns>
        /// A <see cref="TouchAction"/> which represents the cancel operation.
        /// </returns>
        public static TouchAction Cancel()
        {
            return new TouchAction()
            {
                Action = TouchOperation.Cancel,
                Options = new Dictionary<string, object>()
                {
                }
            };
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return this.Action.ToString();
        }
    }
}