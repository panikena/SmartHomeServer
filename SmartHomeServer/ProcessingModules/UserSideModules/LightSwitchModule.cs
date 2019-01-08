using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHomeServer.Messages;
using log4net;
using SmartHomeServer.Enums.SmartBrickCommands;
using Newtonsoft.Json;
using SmartHomeServer.Messages.WidgetMessages;

namespace SmartHomeServer.ProcessingModules.UserSideModules
{
    public class LightSwitchModule : IModule
    {
        private static readonly ILog log = LogManager.GetLogger("LOGGER");

        public IProcessingResult ProcessCommand(IMessage command)
        {
            log.Info("Processing in LightSwitchModule");

            var webSocketMessage = (WebSocketMessage)command;

            var msg = JsonConvert.DeserializeObject<LightSwitchWidgetMsg>(webSocketMessage.Message);
            
            byte smartBrickId = 15;

            var brickMsg = new SmartBrickMessage()
            {
                SmartBrickID = smartBrickId,
                CommandCode = (byte)RelayDriverCommands.Switch,
                Payload = new byte[] { (byte)msg.LightID },
                PipeAddress = GetPipeAddress(smartBrickId)
            };

            //Do not send response to UI. Widget should be updated by SmartBrick that checks light status
            var result = new ProcessingResult(new SmartBrickMessage[] { brickMsg }, null);
            return result;
        }

        private byte[] GetPipeAddress(byte smartBrickId)
        {
            byte[] pipe = new byte[] { 0xAB, 0xCD, 0xAB, 0xCD, 0x80 };
            return pipe;
        }
    }


}
