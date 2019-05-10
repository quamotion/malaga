using Newtonsoft.Json;
using System;

namespace Quamotion.Malaga
{
    /// <summary>
    /// The status information as reported by the WebDriverAgent.
    /// </summary>
    public class WdaStatus
    {
        /// <summary>
        /// Gets or sets the ID of the currently running session.
        /// </summary>
        [JsonProperty("sessionId")]
        public Guid SessionId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the capabilities of the currently running session.
        /// </summary>
        [JsonProperty("capabilities")]
        public WdaSessionCapabilities Capabilities
        {
            get;
            set;
        }
    }
}
