using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHomeServer
{
    public interface ISmartBrickCommand
    {
        short SmartBrickID { get; set; }
        short CommandCode { get; set; }
        byte[] Payload { get; set; }
    }

    public class SmartBrickCommand : ISmartBrickCommand 
    {
        public short SmartBrickID { get; set; }
        public short CommandCode { get; set; }
        public byte[] Payload { get; set; }
    }
}
