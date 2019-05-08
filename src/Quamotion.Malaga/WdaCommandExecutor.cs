using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;

namespace Quamotion.Malaga
{
    public class WdaCommandExecutor : HttpCommandExecutor
    {
        private readonly string sessionId;

        public WdaCommandExecutor(Uri addressOfRemoteServer, string sessionId, TimeSpan timeout)
            : base(addressOfRemoteServer, timeout)
        {
            this.sessionId = sessionId;
        }

        public override Response Execute(Command commandToExecute)
        {
            if (commandToExecute == null)
            {
                throw new ArgumentNullException(nameof(commandToExecute));
            }

            if (commandToExecute.Name == DriverCommand.NewSession)
            {
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
