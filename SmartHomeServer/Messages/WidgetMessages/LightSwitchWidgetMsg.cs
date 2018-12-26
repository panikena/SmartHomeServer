using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHomeServer.Messages.WidgetMessages
{
    public class LightSwitchWidgetMsg
    {
        public int LightID { get; set; }
        public string Command { get; set; }
    }
}
