using OpenQA.Selenium;
using System;

namespace Quamotion.Malaga
{
    public class WdaCapabilities : RemoteSessionSettings
    {
        public static WdaCapabilities Default
        { get; } = new WdaCapabilities();
    }
}
