using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Quamotion.Malaga
{
    public enum Order
    {
        [EnumMember(Value = "next")]
        Next,

        [EnumMember(Value = "previous")]
        Previous,
    }
}
