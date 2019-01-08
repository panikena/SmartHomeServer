using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHomeServer.Enums.SmartBrickCommands
{
    public enum ThermoCommands
    {
        NOOP,
        GetAllValues,
        GetTemp,
        GetHumidity,
        GetPressure
    }
}
