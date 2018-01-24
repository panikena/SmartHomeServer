using log4net;
using SuperSocket.SocketBase;
using SuperSocket.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHomeServer
{
    public class WebSocketEndpoint
    {
        private WebSocketServer SocketServer { get; set; }

        private static readonly ILog log = LogManager.GetLogger(typeof(WebSocketEndpoint));

        public WebSocketEndpoint()
        {
            SocketServer = new WebSocketServer();
        }

        public void Start()
        {
            //Setup the appServer
            if (!SocketServer.Setup(2012)) //Setup with listening port
            {
                log.Error("Could not setup WebSocket server");
                return;
            }

            SocketServer.NewMessageReceived += new SessionHandler<WebSocketSession, string>(OnMessageReceived);
            

            //Try to start the appServer
            if (!SocketServer.Start())
            {
                log.Error("Could not setup WebSocket server");
                return;
            }

            log.Info("WebSocket server was started");
        }

        public void Stop()
        {
            //Console.WriteLine("The server was stopped!");
            SocketServer.Stop();
            log.Info("WebSocket server was stopped");
        }

        private void OnMessageReceived(WebSocketSession session, string message)
        {
            //Send the received message back
            session.Send("Server: " + message);
        }
    
    }
}
