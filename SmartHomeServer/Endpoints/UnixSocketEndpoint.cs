using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHomeServer.InputMessages;
using System.Runtime.Serialization.Formatters.Binary;

namespace SmartHomeServer
{
    public class UnixSocketEndpoint 
    {
        private static readonly ILog log = LogManager.GetLogger("LOGGER");

        public Func<IInputMessage, Task> ProcessCommand { get; set; }

        public UnixSocketEndpoint()
        {

        }

      
        public void Open()
        {
            log.Info("Unix socket was opened");
        }
        public void Close()
        {
            log.Info("Unix socket was closed");
        }

        private async void OnMessageReceived(byte[] payload)
        {
            //decrypt
            byte[] decryptedPayload = payload;
            //deserialize
            SmartBrickCommand brickCommand = null ;
            try
            {
                System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(decryptedPayload);

                // create new BinaryFormatter
                BinaryFormatter binaryFormatter
                            = new BinaryFormatter();

                // set memory stream position to starting point
                memoryStream.Position = 0;

                // Deserializes a stream into an object graph and return as a object.
                brickCommand = (SmartBrickCommand)binaryFormatter.Deserialize(memoryStream);
            }
            catch (Exception ex)
            {
                log.Error("Error during deserialization", ex);
                return;
            }
            
            //process
            var msg = new SmartBrickMessage()
            {
                Payload = brickCommand
            };

            await ProcessCommand(msg);
        }

        public async Task SendCommand(ISmartBrickCommand message)
        {
            //encrypt

            //serialize

            //send
        }
    }
}
