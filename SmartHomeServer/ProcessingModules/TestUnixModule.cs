using System;
using SmartHomeServer.Messages;
using log4net;

namespace SmartHomeServer.ProcessingModules
{
    public class TestUnixModule : IModule
    {
        private static readonly ILog log = LogManager.GetLogger("LOGGER");

        public IProcessingResult ProcessCommand(IMessage message)
        {
            var unixMessage = new SmartBrickMessage()
            {
                SmartBrickID = 1,
                CommandCode = 1,
                Payload = new byte[] { 1, 2, 3 }
            };


            string socket = null;
            try {
                socket = WebSocketEndpoint.SocketDict["Web"];
            } catch (Exception ex)
            {
            }

            WebSocketMessage webMessage = null;
            if (socket != null)
            {
                webMessage = new WebSocketMessage()
                {
                    SocketSessionID = socket,
                    Message = "Test Unix!"
                };
            }
                      


            var result = new ProcessingResult(null, new WebSocketMessage[] { webMessage });

            return result;
        }
    }
}
