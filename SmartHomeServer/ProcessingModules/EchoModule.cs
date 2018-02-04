using SmartHomeServer.Messages;
using System;

namespace SmartHomeServer.ProcessingModules
{
    public class EchoModule : IModule
    {
        public IProcessingResult ProcessCommand(IMessage command)
        {
            var webSocketMessage = (WebSocketMessage)command;

            var brickMsg = new SmartBrickMessage()
            {
                SmartBrickID = 5,
                CommandCode = Convert.ToByte(webSocketMessage.Message),
                Payload = null
            };

            //no SmartBrick required
            var result = new ProcessingResult(new SmartBrickMessage[] { brickMsg }, new WebSocketMessage[] { webSocketMessage });
            return result;
        }
    }
}
