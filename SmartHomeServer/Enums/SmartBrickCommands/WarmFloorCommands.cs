using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHomeServer.Enums.SmartBrickCommands
{
    public enum WarmFloorCommands
    {
        //Sent to MCU
        NOOP = 0, 
        AutoHeat = 1,
        TurnOff = 2,
        SetTemp = 3,
        SetHysteresis = 4,

        //Received from MCU
        GetCurrentState = 5,
    }
}
