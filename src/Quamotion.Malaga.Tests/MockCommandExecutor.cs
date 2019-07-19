using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;

namespace Quamotion.Malaga.Tests
{
    class MockCommandExecutor : ICommandExecutor
    {
        public CommandInfoRepository CommandInfoRepository
        { get; } = new W3CWireProtocolCommandInfoRepository();

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

            if (commandToExecute.Name == DriverCommand.FindElement)
            {
                return new Response()
                {
                    SessionId = Guid.NewGuid().ToString(),
                    Status = WebDriverResult.Success,
                    Value = new RemoteWebElement(null, "56000000 -0000-0000-3A17-000000000000")
                };
            }

            if (commandToExecute.Name == WdaDriverCommand.ElementScreenShot)
            {
                var screenshot = File.ReadAllBytes("screenshot.jpg");
                return new Response()
                {
                    SessionId = Guid.NewGuid().ToString(),
                    Status = WebDriverResult.Success,
                    Value = Convert.ToBase64String(screenshot)
                };
            }

            if (commandToExecute.Name == WdaDriverCommand.GetActiveElement)
            {
                return new Response()
                {
                    SessionId = Guid.NewGuid().ToString(),
                    Status = WebDriverResult.Success,
                    Value = new Dictionary<string, object>()
                    {
                        { "ELEMENT", "56000000 -0000-0000-3A17-000000000000"}
                    }
                };
            }

            if (commandToExecute.Name == WdaDriverCommand.GetRectangle)
            {
                return new Response()
                {
                    SessionId = Guid.NewGuid().ToString(),
                    Status = WebDriverResult.Success,
                    Value = new Dictionary<string, object>()
                    {
                        { "x", 100L},
                        { "y", 200L},
                        { "width", 50L},
                        { "height", 150L}
                    }
                };
            }

            if (commandToExecute.Name == WdaDriverCommand.IsDisplayed)
            {
                return new Response()
                {
                    SessionId = Guid.NewGuid().ToString(),
                    Status = WebDriverResult.Success,
                    Value = true
                };
            }

            if (commandToExecute.Name == WdaDriverCommand.IsAccessible)
            {
                return new Response()
                {
                    SessionId = Guid.NewGuid().ToString(),
                    Status = WebDriverResult.Success,
                    Value = false
                };
            }

            if (commandToExecute.Name == WdaDriverCommand.IsAcessibilityContainer)
            {
                return new Response()
                {
                    SessionId = Guid.NewGuid().ToString(),
                    Status = WebDriverResult.Success,
                    Value = true
                };
            }

            if (commandToExecute.Name == "getWindowRect")
            {
                return new Response()
                {
                    SessionId = Guid.NewGuid().ToString(),
                    Status = WebDriverResult.Success,
                    Value = new Dictionary<string, object>()
                    {
                        { "width", 50L},
                        { "height", 150L}
                    }
                };
            }

            return new Response();
        }

        public void Dispose()
        {
        }
    }
}
