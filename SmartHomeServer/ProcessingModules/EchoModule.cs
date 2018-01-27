using SmartHomeServer.Messages;

namespace SmartHomeServer.ProcessingModules
{
    public class EchoModule : IModule
    {
        public IProcessingResult ProcessCommand(IMessage command)
        {
            var webSocketMessage = (WebSocketMessage)command;
            //no SmartBrick required
            var result = new ProcessingResult(null, new WebSocketMessage[] { webSocketMessage });
            return result;
        }
    }
}
