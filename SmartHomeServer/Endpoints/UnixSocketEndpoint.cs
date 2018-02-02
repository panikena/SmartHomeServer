﻿using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartHomeServer.Messages;
using System.Runtime.Serialization.Formatters.Binary;
using Mono.Unix;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using SuperSocket.SocketBase;

namespace SmartHomeServer
{
    public static class SocketExtensions
    { 
        /// <summary>
        ///     Asynchronously waits to accept an incoming connection.
        /// </summary>
        /// <returns>Task&lt;Socket&gt;</returns>
        public static Task<Socket> AcceptTask(this Socket s)
        {
            return Task.Factory.FromAsync<Socket>(
                s.BeginAccept(null, null),
                s.EndAccept
            );
        }
        
        /// <summary>
        ///     Asynchronously requests to disconnect from a remote endpoint.
        /// </summary>
        /// <param name="reuseSocket">Re-use the socket after connection is closed</param>
        /// <returns>Task</returns>
        public static Task DisconnectTask(this Socket s, bool reuseSocket)
        {
            return Task.Factory.FromAsync(
                s.BeginDisconnect(reuseSocket, null, null),
                s.EndDisconnect
            );
        }
    
        /// <summary>
        ///     Asynchronously receives data from a connected System.Net.Sockets.Socket
        /// </summary>
        /// <param name="buffer">
        ///     An array of type System.Byte that is the storage location for the received data
        /// </param>
        /// <param name="offset">
        ///     The zero-based position in the buffer parameter at which to store the received data
        /// </param>
        /// <param name="size">The number of bytes to receive</param>
        /// <param name="flags">A bitwise combination of the System.Net.Sockets.SocketFlags values</param>
        /// <returns>Task&lt;int&gt;</returns>
        public static Task<int> ReceiveTask(this Socket s, byte[] buffer, int offset, int size, SocketFlags flags)
        {
            return Task.Factory.FromAsync<int>(
                s.BeginReceive(buffer, offset, size, flags, null, null),
                s.EndReceive
            );
        }

        /// <summary>
        ///     Sends data asynchronously to a connected System.Net.Sockets.Socket
        /// </summary>
        /// <param name="buffer">
        ///     An array of type System.Byte that contains the data to send
        /// </param>
        /// <param name="offset">
        ///     The zero-based position in the buffer parameter at which to begin sending data
        /// </param>
        /// <param name="size">The number of bytes to send</param>
        /// <param name="flags">A bitwise combination of the System.Net.Sockets.SocketFlags values</param>
        /// <returns>Task&lt;int&gt;</returns>
        public static Task<int> SendTask(this Socket s, byte[] buffer, int offset, int size, SocketFlags flags)
        {
            return Task.Factory.FromAsync<int>(
                s.BeginSend(buffer, offset, size, flags, null, null),
                s.EndSend
            );
        }
    }



    public class UnixSocketEndpoint
    {
        private const int BUFFER_SIZE = 100;

        private static readonly ILog log = LogManager.GetLogger("LOGGER");

        private Queue<SmartBrickMessage> MessageQueue { get; set; }
        private UnixEndPoint _endpoint { get; set; }
        private Socket _socket { get; set; }
        private const string SocketAddress = "/home/pi/projects/smartHome.sock";

        private volatile bool _working;
        private volatile bool _receiving;
        private volatile bool _sending;

        public Func<IMessage, Task> ProcessCommand { get; set; }


        public UnixSocketEndpoint()
        {
            _socket = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.Unspecified);
            _socket.Blocking = false;
            _endpoint = new UnixEndPoint(SocketAddress);
            //_xteaKey = GetKey();
        }

        public void Open()
        {
            try
            {
                _socket.Bind(_endpoint);
                log.Info("Unix socket was opened");
                //100 - some random number
                _socket.Listen(100);
                log.Info("Unix socket is listening");
                Task.Run(() => AcceptConnection());
            }
            catch (Exception ex)
            {
                log.Fatal("Unix socket failed to open", ex);
            }
        }

        public void Close()
        {
            try
            {
                //_working = false;
                _socket.DisconnectTask(false).Wait();
                _socket.Dispose();
                log.Info("Unix socket was closed");
            }
            catch (Exception ex)
            {
                log.Error("Unix socket failed to close", ex);
            }
        }

        private async Task AcceptConnection()
        {

            // Start an asynchronous socket to listen for connections.  
            log.Info("Unix socket awaiting connection");
            _socket = await _socket.AcceptTask();
            log.Info("Unix socket connection accepted");
            _working = true;
            //start new thread
            Task.Run(() => Work());
            log.Info("Unix socket started listening");
        }

        private async Task Work()
        {
            log.Info("In Unix work");
            while (_working)
            {
                try
                {
                    byte[] data = new byte[BUFFER_SIZE];
                    _receiving = true;
                    log.Info("Unix socket receiving");
                    if (_socket.Connected)
                    {
                        var bytesReceived = await _socket.ReceiveTask(data, 0, BUFFER_SIZE, SocketFlags.None);
                        log.Info("Unix socket received");
                        _receiving = false;
                        if (bytesReceived > 0)
                        {
                            log.Info("Unix on msg received");
                            Task.Run(() => OnMessageReceived(data));
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error while listening to socket", ex);
                }
            }
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

            //Wait for receive to complete
            //while (_receiving) ;
            _sending = true;
            //send
            await _socket.SendTask(data, 0, data.Length, SocketFlags.None);
            _sending = false;

        }
    }
}
