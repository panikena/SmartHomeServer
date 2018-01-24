using SmartHomeServer.ProcessingModules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHomeServer
{
    public class ProcessingStrategyResolver
    {
        public ProcessingStrategyResolver()
        {

        }

        public IModule GetProcessingModule(IInputCommand command)
        {
            return null;
        }


    }
}
