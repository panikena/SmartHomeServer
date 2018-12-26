using System;
using SmartHomeServer.Messages;
using log4net;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SmartHomeServer.ProcessingModules
{
    public class TestUnixModule : IModule
    {
        private static readonly ILog log = LogManager.GetLogger("LOGGER");

        public IProcessingResult ProcessCommand(IMessage message)
        {
            var brickMsg = (SmartBrickMessage)message;
           
            var webSocketMessages = new List<WebSocketMessage>();

            foreach (var socketId in WebSocketEndpoint.SocketDict.Keys)
            {
                var msg = JsonConvert.SerializeObject(brickMsg);

                webSocketMessages.Add(new WebSocketMessage()
                {
                    SocketSessionID = socketId,
                    Message = msg
                });
            }

            var result = new ProcessingResult(null, webSocketMessages);

            return result;
        }
    }
}
