using SmartHomeServer.Messages;
using SmartHomeServer.ProcessingModules;
using System;

namespace SmartHomeServer
{
    public class ProcessingStrategyResolver
    {
        public ProcessingStrategyResolver()
        {

        }

        public IModule GetProcessingModule(IMessage message)
        {
            if (message.Source == MessageSource.WebSocket)
            {
                return new EchoModule();
            }
            if (message.Source == MessageSource.UnixSocket)
            {
                return new TestUnixModule();
            }
            throw new Exception("No module found"); 
        }
    }
}
