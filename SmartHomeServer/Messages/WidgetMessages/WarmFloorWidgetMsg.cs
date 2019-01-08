using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHomeServer.Messages.WidgetMessages
{
    public class WarmFloorWidgetMsg
    {
        public bool IsTurnedOn { get; set; }
        public float CurrentTemperature { get; set; }
        public float TargetTemperature { get; set; }
        public float Hysteresis { get; set; }
    }
}
