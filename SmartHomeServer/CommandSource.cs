using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHomeServer
{
    public enum CommandSource
    {
        UnixSocket = 1,
        WebSocket = 2,
        Scheduler = 3
    }
}
