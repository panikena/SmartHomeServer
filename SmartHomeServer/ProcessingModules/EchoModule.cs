using log4net;
using SmartHomeServer.Messages;
using System;

namespace SmartHomeServer.ProcessingModules
{
    public class EchoModule : IModule
    {

        private static readonly ILog log = LogManager.GetLogger("LOGGER");

        public IProcessingResult ProcessCommand(IMessage command)
        {
            log.Info("Processing in EchoModule");

            var webSocketMessage = (WebSocketMessage)command;

            byte smartBrickId = 5;

            var brickMsg = new SmartBrickMessage()
            {
                SmartBrickID = smartBrickId,
                CommandCode = Convert.ToByte(webSocketMessage.Message),
                Payload = null,
                PipeAddress = GetPipeAddress(smartBrickId)
            };

            var result = new ProcessingResult(new SmartBrickMessage[] { brickMsg }, new WebSocketMessage[] { webSocketMessage });
            return result;
        }

        private byte[] GetPipeAddress(byte smartBrickId)
        {
            byte[] pipe = new byte [] { 0xAB, 0xCD, 0xAB, 0xCD, 0x71 };
            return pipe;
        }
    }
}
