using Newtonsoft.Json;

namespace Quamotion.Malaga
{
    /// <summary>
    /// The session capabilities as returned to us by the WebDriverAgent.
    /// </summary>
    public class WdaSessionCapabilities
    {
        /// <summary>
        /// Gets or sets the device on which the session is running.
        /// </summary>
        [JsonProperty("device")]
        public string Device
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the browser or app which is running.
        /// </summary>
        [JsonProperty("browserName")]
        public string BrowserName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the version of iOS which is running on the device.
        /// </summary>
        [JsonProperty("sdkVersion")]
        public string SdkVersion
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the bundle identifier of the currently running app. On most recent versions of iOS, usually returns the
        /// process ID instead.
        /// </summary>
        [JsonProperty("CFBundleIdentifier")]
        public string BundleIdentifier
        {
            get;
            set;
        }
    }
}
