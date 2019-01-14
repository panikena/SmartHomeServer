using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SmartHomeServer.Enums;
using SmartHomeServer.Enums.SmartBrickCommands;
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
            var ledDriverMsg = new SmartBrickMessage();

            ledDriverMsg.SmartBrickID = 20;
            ledDriverMsg.CommandCode = (byte)(colorPickerSwitchWidgetMsg.IsTurnedOn ? 1 : 2);

            if (colorPickerSwitchWidgetMsg.IsTurnedOn)
            {
                ledDriverMsg.Payload = new byte[3]
                {
                    colorPickerSwitchWidgetMsg.Color1,
                    colorPickerSwitchWidgetMsg.Color2,
                    colorPickerSwitchWidgetMsg.Color3
                };
            }
            else
            {
                ledDriverMsg.Payload = new byte[3]
                {
                    0, 0, 0
                };
            }

            ledDriverMsg.PipeAddress = GetPipeAddress(ledDriverMsg.SmartBrickID);


            var dimmerMsg = new SmartBrickMessage();

            dimmerMsg.SmartBrickID = 40;
            dimmerMsg.CommandCode = (byte)DimmerCommands.UPDATE;
            dimmerMsg.Payload = ledDriverMsg.Payload;


            var wsMsgList = new List<WebSocketMessage>();
            var webSocketPayload = new WebSocketPayload()
            {
                WidgetID = 2,
                WidgetType = WidgetType.ColorPickerSwitch,
                Message = JObject.Parse(webSocketMessage.Message)
            };
            foreach (string key in WebSocketEndpoint.SocketDict.Keys)
            {
                wsMsgList.Add(new WebSocketMessage()
                {
                    SocketSessionID = key,
                    Message = JsonConvert.SerializeObject(webSocketPayload)
                });
            }

            //Update other clients with current value
            return new ProcessingResult(new SmartBrickMessage[] { ledDriverMsg, dimmerMsg }, wsMsgList);
        }

        private byte[] GetPipeAddress(byte smartBrickId)
        {
            //id 20
            if (smartBrickId == 20)
            {
                return new byte[5] { 0xAB, 0xCD, 0xAB, 0xCD, 0x90 };
            }
            //dimmer
            if (smartBrickId == 40)
            {
                return new byte[5] { 0xAB, 0xCD, 0xAB, 0xCD, 0x33 };
            }
            return null;
            
        }

     
    }


}
