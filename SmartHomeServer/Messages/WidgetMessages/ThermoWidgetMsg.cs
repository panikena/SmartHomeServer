using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHomeServer.Messages.WidgetMessages
{
    public class ThermoWidgetMsg
    {
        public float Temperature { get; set; }
        public float Humidity { get; set; }
        public float Pressure { get; set; }
    }
}
