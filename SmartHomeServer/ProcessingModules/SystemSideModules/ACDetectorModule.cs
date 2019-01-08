using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHomeServer.Messages;
using log4net;
using Newtonsoft.Json;

namespace SmartHomeServer.ProcessingModules.SystemSideModules
{
    public class ACDetectorModule : IModule
    {
        private static readonly ILog log = LogManager.GetLogger("LOGGER");

        public IProcessingResult ProcessCommand(IMessage command)
        {
            log.Info("Processing in AC Detector");

            var brickMsg = (SmartBrickMessage)command;

            var payload = brickMsg.Payload;

            var webSocketMessages = new List<WebSocketMessage>();
            var widgetsToUpdate = GetWidgetIDs();


            foreach (var socketId in WebSocketEndpoint.SocketDict.Keys)
            {
                foreach (var widgetId in widgetsToUpdate)
                {
                    webSocketMessages.Add(new WebSocketMessage()
                    {
                        WidgetID = widgetId,
                        SocketSessionID = socketId,
                       // Message = msg
                    });
                }
            }
            
            //Update UI of each client with the current state of AC channels
            var result = new ProcessingResult(null, webSocketMessages);
            return result;
        }

        private IEnumerable<int> GetWidgetIDs()
        {
            return new List<int>() { 1, 2 };
        }
    }
}
