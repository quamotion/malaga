using System;
using System.Collections.Generic;
using System.Text;

namespace Quamotion.Malaga
{
    /// <summary>
    /// Represents a touch operation.
    /// </summary>
    public enum TouchOperation
    {
        /// <summary>
        /// Performs a single tap.
        /// </summary>
        Tap,

        /// <summary>
        /// Performs a finger down and wait operation.
        /// </summary>
        LongPress,

        /// <summary>
        /// Performs a finger down operation.
        /// </summary>
        Press,

        /// <summary>
        /// Performs a finger up operation.
        /// </summary>
        Release,

        /// <summary>
        /// Performs a finger move operation.
        /// </summary>
        MoveTo,

        /// <summary>
        /// Performs a wait operation.
        /// </summary>
        Wait,

        /// <summary>
        /// Cancels the current operation.
        /// </summary>
        Cancel
    }
}
