using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHomeServer.Messages.WidgetMessages
{
    public class ColorPickerSwitchWidgetMsg
    {
        public byte LightID
        {
            get;
            set;
        }

        public bool IsTurnedOn
        {
            get;
            set;
        }

        public byte Color1
        {
            get;
            set;
        }

        public byte Color2
        {
            get;
            set;
        }

        public byte Color3
        {
            get;
            set;
        }
    }

}
