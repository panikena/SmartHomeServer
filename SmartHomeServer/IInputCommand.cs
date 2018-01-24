using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHomeServer
{
    public interface IInputCommand
    {
        CommandSource Source { get; }
        int NodeID { get; }
        string Command { get; set; }
    }
}
