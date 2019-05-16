using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Quamotion.Malaga
{
    public enum Direction
    {
        [EnumMember(Value = "up")]
        Up,

        [EnumMember(Value = "down")]
        Down,

        [EnumMember(Value = "left")]
        Left,

        [EnumMember(Value = "right")]
        Right,
    }
}
