using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHomeServer.Messages;
using System.Runtime.Serialization.Formatters.Binary;

namespace SmartHomeServer
{
    public class UnixSocketEndpoint
    {
        private static byte[] _xteaKey { get; set; }

        private static readonly ILog log = LogManager.GetLogger("LOGGER");

        public Func<IMessage, Task> ProcessCommand { get; set; }

        public UnixSocketEndpoint()
        {
            _xteaKey = GetKey();
        }

        private byte[] GetKey()
        {
            return null;
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
            SmartBrickMessage brickMessage = null;
            try
            {
                brickMessage = SmartBrickMessage.Deserialize(decryptedPayload);
            }
            catch (Exception ex)
            {
                log.Error("Error during deserialization", ex);
                return;
            }

            await ProcessCommand(brickMessage);
        }

        public async Task SendCommand(SmartBrickMessage message)
        {
            //encrypt

            //serialize
            byte[] data = message.Serialize();
            //send
            await Task.Run(() => { return; });
            
        }
    }
}
