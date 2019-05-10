namespace Quamotion.Malaga
{
    public static class WdaDriverCommand
    {
        /// <summary>
        /// Launch App Command.
        /// </summary>
        public const string LaunchApp = "launchApp";

        public const string GetOrientation = "getOrientation";
        public const string SetOrientation = "setOrientation";
        public const string GetRotation = "getRotation";
        public const string SetRotation = "setRotation";

        public const string GetAlertText = "getAlertText";
        public const string GetAlertButtons = "getAlertButtons";
        public const string ClickAlertButton = "clickAlertButton";

        public const string GetSessionStatus = "getSessionStatus";

        public const string Type = "wdaType";
        public const string PressDeviceButton = "deviceButton";

        public const string TouchAndHold = "touchAndHold";
        public const string Drag = "drag";
        public const string PerformTouch = "performTouch";
    }
}
