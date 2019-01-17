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

namespace SmartHomeServer.ProcessingModules.UserSideModules
{
    public class WarmFloorWebModule : IModule
    {
        private static readonly ILog log = LogManager.GetLogger("LOGGER");

        public IProcessingResult ProcessCommand(IMessage message)
        {
            log.Info("Processing in WarmFloorWebModule");

            var webSocketMessage = (WebSocketMessage)message;
            var warmFloorWidgetMsg = JsonConvert.DeserializeObject<WarmFloorWidgetMsg>(webSocketMessage.Message);


            var sbMsg = new SmartBrickMessage()
            {
                SmartBrickID = 50,
                PipeAddress = GetPipeAddress(50)
            };
            

            if (warmFloorWidgetMsg.Action == "ToggledState")
            {
                if (warmFloorWidgetMsg.IsTurnedOn)
                {
                    sbMsg.CommandCode = (byte)WarmFloorCommands.AutoHeat;
                }
                else
                {
                    sbMsg.CommandCode = (byte)WarmFloorCommands.TurnOff;
                }
            }
            if (warmFloorWidgetMsg.Action == "ChangedTemp")
            {
                sbMsg.CommandCode = (byte)WarmFloorCommands.SetTemp;
                sbMsg.Payload = BitConverter.GetBytes(warmFloorWidgetMsg.TargetTemperature);
            }
      


            var wsMsgList = new List<WebSocketMessage>();
            var webSocketPayload = new WebSocketPayload()
            {
                WidgetID = 11,
                WidgetType = WidgetType.WarmFloor,
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
            return new ProcessingResult(new SmartBrickMessage[] { sbMsg }, wsMsgList);

        }

        private byte[] GetPipeAddress(byte smartBrickId)
        {
          return new byte[5] { 0xAB, 0xCD, 0xAB, 0xCD, 0x11 };     
        }
    }
}
