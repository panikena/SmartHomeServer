using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHomeServer.Messages;
using log4net;

namespace SmartHomeServer.ProcessingModules
{
    public class TestUnixModule : IModule
    {
        private static readonly ILog log = LogManager.GetLogger("LOGGER");

        public IProcessingResult ProcessCommand(IMessage message)
        {
            log.Info("In test unix");
            var unixMessage = new SmartBrickMessage()
            {
                SmartBrickID = 1,
                CommandCode = 1,
                Payload = new byte[] { 1, 2, 3 }
            };

            var webMessage = new WebSocketMessage()
            {
                SocketSessionID = WebSocketEndpoint.SocketDict["Web"],
                Message = "Test Unix!"
            };
            log.Info("WebSocket ID: "+ WebSocketEndpoint.SocketDict["Web"]);


            var result = new ProcessingResult(null, new WebSocketMessage[] { webMessage });

            return result;
        }
    }
}
