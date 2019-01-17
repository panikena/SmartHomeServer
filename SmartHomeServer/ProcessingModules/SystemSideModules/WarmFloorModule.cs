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
    public class WarmFloorModule :IModule
    {
        private static readonly ILog log = LogManager.GetLogger("LOGGER");

        public IProcessingResult ProcessCommand(IMessage message)
        {
            var smartBrickMessage = (SmartBrickMessage)message;
            log.Info("Warm floor data" + string.Join(" ", smartBrickMessage.Payload));

            var warmFloorWidget = new WarmFloorWidgetMsg();

            if (smartBrickMessage.CommandCode == (byte)WarmFloorCommands.GetCurrentState)
            {
                warmFloorWidget.CurrentTemperature = BitConverter.ToSingle(smartBrickMessage.Payload, 0);
                warmFloorWidget.TargetTemperature = BitConverter.ToSingle(smartBrickMessage.Payload, 4);
                warmFloorWidget.Hysteresis = BitConverter.ToSingle(smartBrickMessage.Payload, 8);
                warmFloorWidget.IsTurnedOn = BitConverter.ToBoolean(smartBrickMessage.Payload, 12);
            }

            List<WebSocketMessage> list = new List<WebSocketMessage>();

            foreach (string key in WebSocketEndpoint.SocketDict.Keys)
            {
                WebSocketPayload webSocketPayload = new WebSocketPayload
                {
                    WidgetID = 11,
                    WidgetType = WidgetType.WarmFloor,
                    Message = JObject.Parse(JsonConvert.SerializeObject(warmFloorWidget))
                };
                list.Add(new WebSocketMessage
                {
                    SocketSessionID = key,
                    Message = JsonConvert.SerializeObject(webSocketPayload)
                });
            }
            return new ProcessingResult(null, list);
        }
    }
}
