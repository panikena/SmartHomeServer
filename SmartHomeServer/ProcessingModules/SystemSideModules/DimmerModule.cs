using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SmartHomeServer.Enums;
using SmartHomeServer.Enums.SmartBrickCommands;
using SmartHomeServer.Messages;
using SmartHomeServer.Messages.WidgetMessages;

namespace SmartHomeServer.ProcessingModules.SystemSideModules
{
    public class DimmerModule : IModule
    {
        private static readonly ILog log = LogManager.GetLogger("LOGGER");

        private byte[] GetColorPayload(byte color, byte colorValue)
        {
            byte[] payload;


            //to be revised
            switch (color)
            {
                //all colors
                case 0:
                    payload = new byte[3] { colorValue, colorValue, colorValue };
                    break;
                case 1:
                    payload = new byte[3] { colorValue, 0, 0 };
                    break;
                case 2:
                    payload = new byte[3] { 0, colorValue, 0 };
                    break;
                case 3:
                    payload = new byte[3] { 0, 0, colorValue };
                    break;
                default:
                    payload = new byte[0];
                    break;
            }


            return payload;
        }

        public IProcessingResult ProcessCommand(IMessage message)
        {

            var incomingMsg = (SmartBrickMessage)message;

            byte rotationValue = 0;
            byte color1 = 0;
            byte color2 = 1;
            byte color3 = 2;

            if (incomingMsg.CommandCode == (byte)DimmerCommands.PRESSED_BTN)
            {
            }

            if (incomingMsg.CommandCode == (byte)DimmerCommands.ROTATED)
            {
                //0-255
                //rotationValue = incomingMsg.Payload[0];
                //color = incomingMsg.Payload[1];

                color1 = incomingMsg.Payload[0];
                color2 = incomingMsg.Payload[1];
                color3 = incomingMsg.Payload[2];
            }


            var ledDriverMessage = new SmartBrickMessage();



            LEDDriverCommands commandCode;

       /*     if (rotationValue == 0)
            {
                //all colors
                if (color == 0)
                {
                    commandCode = LEDDriverCommands.TURN_OFF;
                }
                else
                {
                    commandCode = LEDDriverCommands.UPDATE_COLOR;
                }
            }
            else
            {
                //all colors
                if (color == 0)
                {
                    commandCode = LEDDriverCommands.TURN_ON;
                }
                else
                {
                    commandCode = LEDDriverCommands.UPDATE_COLOR;

                }
            }*/

            ledDriverMessage.SmartBrickID = 20;
            ledDriverMessage.CommandCode = (byte)LEDDriverCommands.TURN_ON;
            ledDriverMessage.Payload = incomingMsg.Payload;// GetColorPayload(color, rotationValue);
            ledDriverMessage.PipeAddress = GetPipeAddress(ledDriverMessage.SmartBrickID);

            var wsMessageList = new List<WebSocketMessage>();
            var widgetMsg = new ColorPickerSwitchWidgetMsg()
            {
                IsTurnedOn = true,//rotationValue > 0 || color != 0,
                Color1 = ledDriverMessage.Payload[1],
                Color2 = ledDriverMessage.Payload[2],
                Color3 = ledDriverMessage.Payload[3],

            };

            WebSocketPayload webSocketPayload = new WebSocketPayload()
            {
                WidgetID = 2,
                WidgetType = WidgetType.ColorPickerSwitch,
                Message = JsonConvert.SerializeObject(widgetMsg)
            };

            foreach (string key in WebSocketEndpoint.SocketDict.Keys)
            {
                wsMessageList.Add(new WebSocketMessage()
                {
                    SocketSessionID = key,
                    Message = JsonConvert.SerializeObject(webSocketPayload)
                });
            }

            //Update other clients with current value
            return new ProcessingResult(new SmartBrickMessage[] { ledDriverMessage }, wsMessageList);
        }

        private byte[] GetPipeAddress(byte smartBrickId)
        {
            return new byte[5] { 0xAB, 0xCD, 0xAB, 0xCD, 0x90 };
        }
    }
}
