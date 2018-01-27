using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHomeServer.Exceptions
{
    public class NoModuleFoundException : Exception
    {
        public NoModuleFoundException()
        {

        }

        public NoModuleFoundException(string message) : base(message)
        {
                
        }

    }
}
