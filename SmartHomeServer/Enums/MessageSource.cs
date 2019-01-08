using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHomeServer.Enums
{
    public enum MessageSource
    {
        UnixSocket = 1,
        WebSocket = 2,
        Scheduler = 3,
        Internal = 4
    }
}
