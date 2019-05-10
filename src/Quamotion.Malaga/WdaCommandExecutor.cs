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

            this.CommandInfoRepository.TryAddCommand(WdaDriverCommand.GetOrientation, new CommandInfo(CommandInfo.GetCommand, "/session/{sessionId}/orientation"));
            this.CommandInfoRepository.TryAddCommand(WdaDriverCommand.SetOrientation, new CommandInfo(CommandInfo.PostCommand, "/session/{sessionId}/orientation"));

            this.CommandInfoRepository.TryAddCommand(WdaDriverCommand.GetRotation, new CommandInfo(CommandInfo.GetCommand, "/session/{sessionId}/rotation"));
            this.CommandInfoRepository.TryAddCommand(WdaDriverCommand.SetRotation, new CommandInfo(CommandInfo.PostCommand, "/session/{sessionId}/rotation"));
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
