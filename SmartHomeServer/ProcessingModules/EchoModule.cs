using SmartHomeServer.InputMessages;

namespace SmartHomeServer.ProcessingModules
{
    public class EchoModule : IModule
    {
        public IProcessingResult ProcessCommand(IInputMessage command)
        {
            var webSocketMessage = (WebSocketMessage)command;
            //no SmartBrick required
            var result = new ProcessingResult(null, webSocketMessage.SocketSessionID, webSocketMessage.Message);
            return result;
        }
    }
}
