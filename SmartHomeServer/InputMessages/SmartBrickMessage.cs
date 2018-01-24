using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHomeServer.InputMessages
{
    public class SmartBrickMessage : IInputMessage
    {
        public MessageSource Source { get { return MessageSource.UnixSocket; } }
        public ISmartBrickCommand Payload { get; set; }

    }
}
