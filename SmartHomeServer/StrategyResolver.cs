using SmartHomeServer.Messages;
using SmartHomeServer.ProcessingModules;
using System;
using System.Threading.Tasks;

namespace SmartHomeServer
{
    public class ProcessingStrategyResolver
    {
        public ProcessingStrategyResolver()
        {

        }

        public async Task<IModule> GetProcessingModule(IMessage message)
        {
            return await Task.Run<IModule>(() =>
             {
                 if (message.Source == MessageSource.WebSocket)
                 {
                     return new EchoModule();
                 }
                 throw new Exception("No module found");
             });
        }
    }
}
