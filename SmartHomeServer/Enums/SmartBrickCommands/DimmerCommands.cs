using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHomeServer.Enums.SmartBrickCommands
{
    public enum DimmerCommands
    {
        NOOP = 0,
        PRESSED_BTN = 1, 
        ROTATED = 2,
        UPDATE = 3
    }
}
