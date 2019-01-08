using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SmartHomeServer.Enums;
using SmartHomeServer.Messages;
using SmartHomeServer.Messages.WidgetMessages;
using System.Collections.Generic;

namespace SmartHomeServer.ProcessingModules.UserSideModules
{
    public class ColorPickerSwitchModule : IModule
    {
        private static readonly ILog log = LogManager.GetLogger("LOGGER");

        public IProcessingResult ProcessCommand(IMessage command)
        {
            log.Info("Processing in ColorPickerSwitchModule");

            var webSocketMessage = (WebSocketMessage)command;
            var colorPickerSwitchWidgetMsg = JsonConvert.DeserializeObject<ColorPickerSwitchWidgetMsg>(webSocketMessage.Message);
            var smartBrickMessage = new SmartBrickMessage();

            smartBrickMessage.SmartBrickID = 20;
            smartBrickMessage.CommandCode = (byte)(colorPickerSwitchWidgetMsg.IsTurnedOn ? 1 : 2);
            smartBrickMessage.Payload = new byte[3]
            {
                colorPickerSwitchWidgetMsg.Color1,
                colorPickerSwitchWidgetMsg.Color2,
                colorPickerSwitchWidgetMsg.Color3
            };
            smartBrickMessage.PipeAddress = GetPipeAddress(smartBrickMessage.SmartBrickID);

            var list = new List<WebSocketMessage>();
            foreach (string key in WebSocketEndpoint.SocketDict.Keys)
            {
                WebSocketPayload webSocketPayload = new WebSocketPayload()
                {
                    WidgetID = 2,
                    WidgetType = WidgetType.ColorPickerSwitch,
                    Message = JObject.Parse(webSocketMessage.Message)
                };
                list.Add(new WebSocketMessage()
                {
                    SocketSessionID = key,
                    Message = JsonConvert.SerializeObject(webSocketPayload)
                });
            }

            //Update other clients with current value
            return new ProcessingResult(new SmartBrickMessage[]{ smartBrickMessage }, list);
        }

        private byte[] GetPipeAddress(byte smartBrickId)
        {
            return new byte[5] { 0xAB, 0xCD, 0xAB, 0xCD, 0x90};
        }
    }


}
