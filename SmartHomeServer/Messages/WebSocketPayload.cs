using SmartHomeServer.Enums;

namespace SmartHomeServer.Messages
{
    public class WebSocketPayload
    {
        public int WidgetID { get; set; }

        public WidgetType WidgetType { get; set; }
 
        public dynamic Message { get; set; }
    }
}
