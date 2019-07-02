using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;

namespace Quamotion.Malaga
{
    public class WdaCommandExecutor : HttpCommandExecutor
    {
        private string sessionId;

        public WdaCommandExecutor(Uri addressOfRemoteServer, string sessionId, TimeSpan timeout)
            : base(addressOfRemoteServer, timeout)
        {
            this.sessionId = sessionId;

            this.CommandInfoRepository.TryAddCommand(WdaDriverCommand.LaunchApp, new CommandInfo(CommandInfo.PostCommand, "/session/{sessionId}/wda/apps/launch"));
            this.CommandInfoRepository.TryAddCommand(WdaDriverCommand.TerminateApp, new CommandInfo(CommandInfo.PostCommand, "/session/{sessionId}/wda/apps/terminate"));
            this.CommandInfoRepository.TryAddCommand(WdaDriverCommand.SendKeys, new CommandInfo(CommandInfo.PostCommand, "/session/{sessionId}/wda/keys"));

            this.CommandInfoRepository.TryAddCommand(WdaDriverCommand.GetOrientation, new CommandInfo(CommandInfo.GetCommand, "/session/{sessionId}/orientation"));
            this.CommandInfoRepository.TryAddCommand(WdaDriverCommand.SetOrientation, new CommandInfo(CommandInfo.PostCommand, "/session/{sessionId}/orientation"));

            this.CommandInfoRepository.TryAddCommand(WdaDriverCommand.GetRotation, new CommandInfo(CommandInfo.GetCommand, "/session/{sessionId}/rotation"));
            this.CommandInfoRepository.TryAddCommand(WdaDriverCommand.SetRotation, new CommandInfo(CommandInfo.PostCommand, "/session/{sessionId}/rotation"));

            this.CommandInfoRepository.TryAddCommand(WdaDriverCommand.GetAlertText, new CommandInfo(CommandInfo.GetCommand, "/session/{sessionId}/alert/text"));
            this.CommandInfoRepository.TryAddCommand(WdaDriverCommand.GetAlertButtons, new CommandInfo(CommandInfo.GetCommand, "/session/{sessionId}/wda/alert/buttons"));
            this.CommandInfoRepository.TryAddCommand(WdaDriverCommand.ClickAlertButton, new CommandInfo(CommandInfo.GetCommand, "/session/{sessionId}/alert/accept"));

            this.CommandInfoRepository.TryAddCommand(WdaDriverCommand.GetSessionStatus, new CommandInfo(CommandInfo.GetCommand, "/session/{sessionId}/status"));

            this.CommandInfoRepository.TryAddCommand(WdaDriverCommand.Type, new CommandInfo(CommandInfo.PostCommand, "/session/{sessionId}/wda/keyboard/type"));
            this.CommandInfoRepository.TryAddCommand(WdaDriverCommand.PressDeviceButton, new CommandInfo(CommandInfo.PostCommand, "/session/{sessionId}/wda/pressDeviceButton"));
            this.CommandInfoRepository.TryAddCommand(WdaDriverCommand.DismissKeyboard, new CommandInfo(CommandInfo.PostCommand, "/session/{sessionId}/wda/keyboard/dismiss"));

            this.CommandInfoRepository.TryAddCommand(WdaDriverCommand.ElementScreenShot, new CommandInfo(CommandInfo.GetCommand, "/session/{sessionId}/element/{elementId}/screenshot"));

            this.CommandInfoRepository.TryAddCommand(WdaDriverCommand.GetRectangle, new CommandInfo(CommandInfo.GetCommand, "/session/{sessionId}/element/{elementId}/rect"));

            this.CommandInfoRepository.TryAddCommand(WdaDriverCommand.IsDisplayed, new CommandInfo(CommandInfo.GetCommand, "/session/{sessionId}/element/{elementId}/displayed"));

            this.CommandInfoRepository.TryAddCommand(WdaDriverCommand.IsAccessible, new CommandInfo(CommandInfo.GetCommand, "/session/{sessionId}/wda/element/{elementId}/accessible"));

            this.CommandInfoRepository.TryAddCommand(WdaDriverCommand.IsAcessibilityContainer, new CommandInfo(CommandInfo.GetCommand, "/session/{sessionId}/wda/element/{elementId}/accessibilityContainer"));

            this.CommandInfoRepository.TryAddCommand(WdaDriverCommand.Swipe, new CommandInfo(CommandInfo.PostCommand, "/session/{sessionId}/wda/element/{elementId}/swipe"));

            this.CommandInfoRepository.TryAddCommand(WdaDriverCommand.Pinch, new CommandInfo(CommandInfo.PostCommand, "/session/{sessionId}/wda/element/{elementId}/pinch"));

            this.CommandInfoRepository.TryAddCommand(WdaDriverCommand.ElementDoubleTap, new CommandInfo(CommandInfo.PostCommand, "/session/{sessionId}/wda/element/{elementId}/doubleTap"));

            this.CommandInfoRepository.TryAddCommand(WdaDriverCommand.TwoFingerTap, new CommandInfo(CommandInfo.PostCommand, "/session/{sessionId}/wda/element/{elementId}/twoFingerTap"));

            this.CommandInfoRepository.TryAddCommand(WdaDriverCommand.ElementTouchAndHold, new CommandInfo(CommandInfo.PostCommand, "/session/{sessionId}/wda/element/{elementId}/touchAndHold"));

            this.CommandInfoRepository.TryAddCommand(WdaDriverCommand.Scroll, new CommandInfo(CommandInfo.PostCommand, "/session/{sessionId}/wda/element/{elementId}/scroll"));

            this.CommandInfoRepository.TryAddCommand(WdaDriverCommand.ElementDragFromToForDuration, new CommandInfo(CommandInfo.PostCommand, "/session/{sessionId}/wda/element/{elementId}/dragfromtoforduration"));

            this.CommandInfoRepository.TryAddCommand(WdaDriverCommand.DragFromToForDuration, new CommandInfo(CommandInfo.PostCommand, "/session/{sessionId}/wda/dragfromtoforduration"));

            this.CommandInfoRepository.TryAddCommand(WdaDriverCommand.Tap, new CommandInfo(CommandInfo.PostCommand, "/session/{sessionId}/wda/tap/{elementId}"));

            this.CommandInfoRepository.TryAddCommand(WdaDriverCommand.TouchAndHold, new CommandInfo(CommandInfo.PostCommand, "/session/{sessionId}/wda/touchAndHold"));

            this.CommandInfoRepository.TryAddCommand(WdaDriverCommand.DoubleTap, new CommandInfo(CommandInfo.PostCommand, "/session/{sessionId}/wda/doubleTap"));

            this.CommandInfoRepository.TryAddCommand(WdaDriverCommand.WheelSelect, new CommandInfo(CommandInfo.PostCommand, "/session/{sessionId}/wda/pickerwheel/{elementId}/select"));

            this.CommandInfoRepository.TryAddCommand(WdaDriverCommand.ForceTouch, new CommandInfo(CommandInfo.PostCommand, "/session/{sessionId}/wda/element/{elementId}/forceTouch"));
        }

        public override Response Execute(Command commandToExecute)
        {
            if (commandToExecute == null)
            {
                throw new ArgumentNullException(nameof(commandToExecute));
            }

            if (commandToExecute.Name == DriverCommand.NewSession)
            {
                // Get the default session ID if the user has not provided an explicit session ID
                if (this.sessionId == null)
                {
                    var status = this.Execute(new Command(DriverCommand.Status, string.Empty));
                    this.sessionId = status.SessionId;
                }

                return new Response()
                {
                    SessionId = sessionId,
                    Status = WebDriverResult.Success,
                    Value = null
                };
            }
            else
            {
                return base.Execute(commandToExecute);
            }
        }
    }
}
