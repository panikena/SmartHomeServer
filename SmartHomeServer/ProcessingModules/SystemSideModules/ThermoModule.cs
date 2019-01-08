using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SmartHomeServer.Enums;
using SmartHomeServer.Enums.SmartBrickCommands;
using SmartHomeServer.Messages;
using SmartHomeServer.Messages.WidgetMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHomeServer.ProcessingModules.SystemSideModules
{
    public class ThermoModule : IModule
    {
        private static readonly ILog log = LogManager.GetLogger("LOGGER");

        public IProcessingResult ProcessCommand(IMessage message)
        {
            var smartBrickMessage = (SmartBrickMessage)message;
            log.Info("Thermo data" + string.Join(" ", smartBrickMessage.Payload));

            var thermoWidgetMsg = new ThermoWidgetMsg();

            if (smartBrickMessage.CommandCode == (byte)ThermoCommands.GetAllValues)
            {
                thermoWidgetMsg.Temperature = BitConverter.ToSingle(smartBrickMessage.Payload, 0);
                thermoWidgetMsg.Humidity = BitConverter.ToSingle(smartBrickMessage.Payload, 4);
                thermoWidgetMsg.Pressure = BitConverter.ToSingle(smartBrickMessage.Payload, 8);
            }

            List<WebSocketMessage> list = new List<WebSocketMessage>();

            foreach (string key in WebSocketEndpoint.SocketDict.Keys)
            {
                WebSocketPayload webSocketPayload = new WebSocketPayload
                {
                    WidgetID = 3,
                    WidgetType = WidgetType.Thermo,
                    Message = JObject.Parse(JsonConvert.SerializeObject(thermoWidgetMsg))
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
