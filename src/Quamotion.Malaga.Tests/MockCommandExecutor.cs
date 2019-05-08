using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.ObjectModel;

namespace Quamotion.Malaga.Tests
{
    class MockCommandExecutor : ICommandExecutor
    {
        public CommandInfoRepository CommandInfoRepository
        { get; }=  new W3CWireProtocolCommandInfoRepository();

        public Collection<Command> Commands
        { get; } = new Collection<Command>();

        public Response Execute(Command commandToExecute)
        {
            if (commandToExecute.Name == DriverCommand.NewSession)
            {
                return new Response()
                {
                    SessionId = Guid.NewGuid().ToString(),
                    Status = WebDriverResult.Success,
                    Value = null
                };
            }

            this.Commands.Add(commandToExecute);
            return new Response();
        }

        public void Dispose()
        {
        }
    }
}
