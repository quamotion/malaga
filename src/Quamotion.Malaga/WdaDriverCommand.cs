namespace Quamotion.Malaga
{
    public static class WdaDriverCommand
    {
        /// <summary>
        /// Launch App Command.
        /// </summary>
        public const string LaunchApp = "launchApp";
        public const string TerminateApp = "terminateApp";

        public const string SendKeys = "sendKeys";
        public const string HideKeyboard = "hideKeyboard";

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
        public const string DismissKeyboard = "dismissKeyboard";

        public const string ElementScreenShot = "wdaElementScreenshot";

        public const string GetRectangle = "wdaGetRectangle";

        public const string IsDisplayed = "wdaIsDisplayed";

        public const string IsAccessible = "wdaIsAccessible";

        public const string IsAcessibilityContainer = "wdaIsAcessibilityContainer";

        public const string Swipe = "wdaSwipe";

        public const string Pinch = "wdaPinch";

        public const string ElementDoubleTap = "wdaElementDoubleTap";

        public const string TwoFingerTap = "wdaTwoFingerTap";

        public const string ElementTouchAndHold = "wdaElementTouchAndHold";

        public const string Scroll = "wdaScroll";

        public const string ElementDragFromToForDuration = "wdaElementDragFromToForDuration";

        public const string DragFromToForDuration = "wdaDragFromToForDuration";

        public const string Tap = "wdaTap";

        public const string TouchAndHold = "wdaTouchAndHold";

        public const string DoubleTap = "wdaDoubleTap";

        public const string WheelSelect = "wdaWheelSelect";

        public const string ForceTouch = "wdaForceTouch";

        public const string GetActiveElement = "wdaGetActiveElement";
    }
}
