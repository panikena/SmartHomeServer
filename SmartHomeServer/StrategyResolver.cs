using SmartHomeServer.InputMessages;
using SmartHomeServer.ProcessingModules;

namespace SmartHomeServer
{
    public class ProcessingStrategyResolver
    {
        public ProcessingStrategyResolver()
        {

        }

        public IModule GetProcessingModule(IInputMessage message)
        {
            if (message.Source == MessageSource.WebSocket)
            {
                return new EchoModule();
            }
            return null;
        }


    }
}
