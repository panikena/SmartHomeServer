using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHomeServer.Messages
{
    public class WebSocketPayload
    {
        public int WidgetID { get; set; }
        public WidgetType WidgetType { get; set; }
        public string Message { get; set; }
    }
}
